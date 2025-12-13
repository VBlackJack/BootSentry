using System.Diagnostics;
using Microsoft.Extensions.Logging;
using Microsoft.Win32;
using BootSentry.Core.Enums;
using BootSentry.Core.Interfaces;
using BootSentry.Core.Models;
using BootSentry.Core.Parsing;

namespace BootSentry.Providers;

/// <summary>
/// Provider that scans Windows Registry Run and RunOnce keys.
/// </summary>
public sealed class RegistryRunProvider : IStartupProvider
{
    private readonly ILogger<RegistryRunProvider> _logger;
    private readonly ISignatureVerifier? _signatureVerifier;

    private static readonly (RegistryHive Hive, string Path, EntryScope Scope, RegistryView View)[] RegistryLocations =
    [
        // HKCU Run keys (64-bit view)
        (RegistryHive.CurrentUser, @"Software\Microsoft\Windows\CurrentVersion\Run", EntryScope.User, RegistryView.Registry64),
        (RegistryHive.CurrentUser, @"Software\Microsoft\Windows\CurrentVersion\RunOnce", EntryScope.User, RegistryView.Registry64),

        // HKLM Run keys (64-bit view)
        (RegistryHive.LocalMachine, @"Software\Microsoft\Windows\CurrentVersion\Run", EntryScope.Machine, RegistryView.Registry64),
        (RegistryHive.LocalMachine, @"Software\Microsoft\Windows\CurrentVersion\RunOnce", EntryScope.Machine, RegistryView.Registry64),

        // WoW64 32-bit keys
        (RegistryHive.LocalMachine, @"Software\Wow6432Node\Microsoft\Windows\CurrentVersion\Run", EntryScope.Machine, RegistryView.Registry32),
        (RegistryHive.LocalMachine, @"Software\Wow6432Node\Microsoft\Windows\CurrentVersion\RunOnce", EntryScope.Machine, RegistryView.Registry32),

        // Policies Run keys
        (RegistryHive.CurrentUser, @"Software\Microsoft\Windows\CurrentVersion\Policies\Explorer\Run", EntryScope.User, RegistryView.Registry64),
        (RegistryHive.LocalMachine, @"Software\Microsoft\Windows\CurrentVersion\Policies\Explorer\Run", EntryScope.Machine, RegistryView.Registry64),
    ];

    public RegistryRunProvider(ILogger<RegistryRunProvider> logger, ISignatureVerifier? signatureVerifier = null)
    {
        _logger = logger;
        _signatureVerifier = signatureVerifier;
    }

    public EntryType EntryType => EntryType.RegistryRun;
    public string DisplayName => "Registre Run/RunOnce";
    public bool RequiresAdminToRead => false;
    public bool RequiresAdminToModify => true; // For HKLM keys

    public bool IsAvailable() => true;

    public async Task<IReadOnlyCollection<StartupEntry>> ScanAsync(CancellationToken cancellationToken = default)
    {
        var entries = new List<StartupEntry>();

        foreach (var location in RegistryLocations)
        {
            cancellationToken.ThrowIfCancellationRequested();

            try
            {
                var locationEntries = await ScanRegistryKeyAsync(
                    location.Hive,
                    location.Path,
                    location.Scope,
                    location.View,
                    cancellationToken);

                entries.AddRange(locationEntries);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Error scanning registry key {Hive}\\{Path}", location.Hive, location.Path);
            }
        }

        return entries;
    }

    private async Task<List<StartupEntry>> ScanRegistryKeyAsync(
        RegistryHive hive,
        string path,
        EntryScope scope,
        RegistryView view,
        CancellationToken cancellationToken)
    {
        var entries = new List<StartupEntry>();

        using var baseKey = RegistryKey.OpenBaseKey(hive, view);
        using var key = baseKey.OpenSubKey(path);

        if (key == null)
            return entries;

        var hiveName = hive == RegistryHive.CurrentUser ? "HKCU" : "HKLM";
        var fullKeyPath = $"{hiveName}\\{path}";

        foreach (var valueName in key.GetValueNames())
        {
            cancellationToken.ThrowIfCancellationRequested();

            try
            {
                var value = key.GetValue(valueName)?.ToString();
                if (string.IsNullOrWhiteSpace(value))
                    continue;

                var entry = await CreateEntryAsync(
                    fullKeyPath,
                    valueName,
                    value,
                    scope,
                    view == RegistryView.Registry32 ? "32" : "64",
                    path.Contains("RunOnce", StringComparison.OrdinalIgnoreCase),
                    cancellationToken);

                entries.Add(entry);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Error reading registry value {Key}\\{Value}", fullKeyPath, valueName);
            }
        }

        return entries;
    }

    private async Task<StartupEntry> CreateEntryAsync(
        string keyPath,
        string valueName,
        string commandLine,
        EntryScope scope,
        string registryView,
        bool isRunOnce,
        CancellationToken cancellationToken)
    {
        var parsed = CommandLineParser.Parse(commandLine);
        var targetPath = parsed != null ? CommandLineParser.ResolvePath(parsed.Executable) : null;
        var fileExists = targetPath != null && File.Exists(targetPath);

        var entryType = isRunOnce ? EntryType.RegistryRunOnce : EntryType.RegistryRun;
        var id = StartupEntry.GenerateId(entryType, scope, keyPath, valueName);

        var entry = new StartupEntry
        {
            Id = id,
            Type = entryType,
            Scope = scope,
            DisplayName = valueName,
            SourcePath = keyPath,
            SourceName = valueName,
            CommandLineRaw = commandLine,
            CommandLineNormalized = parsed?.Normalized,
            TargetPath = targetPath,
            Arguments = parsed?.Arguments,
            FileExists = fileExists,
            Status = EntryStatus.Enabled,
            RegistryView = registryView
        };

        // Get file metadata if target exists
        if (fileExists && targetPath != null)
        {
            await EnrichWithFileMetadataAsync(entry, targetPath, cancellationToken);
        }

        // Check for suspicious command lines
        if (CommandLineParser.IsSuspiciousCommandLine(commandLine))
        {
            entry.RiskLevel = RiskLevel.Suspicious;
            entry.Notes = "Ligne de commande suspecte détectée";
        }

        return entry;
    }

    private async Task EnrichWithFileMetadataAsync(StartupEntry entry, string filePath, CancellationToken cancellationToken)
    {
        try
        {
            var fileInfo = new FileInfo(filePath);
            entry.FileSize = fileInfo.Length;
            entry.LastModified = fileInfo.LastWriteTime;

            var versionInfo = FileVersionInfo.GetVersionInfo(filePath);
            entry.FileVersion = versionInfo.FileVersion;
            entry.ProductName = versionInfo.ProductName;
            entry.CompanyName = versionInfo.CompanyName;
            entry.FileDescription = versionInfo.FileDescription;
            entry.Publisher = versionInfo.CompanyName;

            // Check signature if verifier is available
            if (_signatureVerifier != null)
            {
                var sigInfo = await _signatureVerifier.VerifyAsync(filePath, cancellationToken);
                entry.SignatureStatus = sigInfo.Status;

                if (!string.IsNullOrEmpty(sigInfo.SignerName))
                    entry.Publisher = sigInfo.SignerName;

                // Update risk level based on signature
                entry.RiskLevel = DetermineRiskLevel(entry, sigInfo);
            }
        }
        catch (Exception)
        {
            // Ignore metadata errors
        }
    }

    private static RiskLevel DetermineRiskLevel(StartupEntry entry, SignatureInfo sigInfo)
    {
        // Microsoft signed = Safe
        if (sigInfo.Status == SignatureStatus.SignedTrusted &&
            entry.Publisher?.Contains("Microsoft", StringComparison.OrdinalIgnoreCase) == true)
        {
            return RiskLevel.Safe;
        }

        // Signed and trusted = Safe
        if (sigInfo.Status == SignatureStatus.SignedTrusted)
        {
            return RiskLevel.Safe;
        }

        // Unsigned in suspicious location = Suspicious
        if (sigInfo.Status == SignatureStatus.Unsigned && entry.TargetPath != null)
        {
            var path = entry.TargetPath.ToLowerInvariant();
            if (path.Contains("\\temp\\") ||
                path.Contains("\\appdata\\local\\temp") ||
                path.Contains("\\downloads\\"))
            {
                return RiskLevel.Suspicious;
            }
        }

        // Explicitly untrusted = Suspicious
        if (sigInfo.Status == SignatureStatus.SignedUntrusted)
        {
            return RiskLevel.Suspicious;
        }

        return RiskLevel.Unknown;
    }
}
