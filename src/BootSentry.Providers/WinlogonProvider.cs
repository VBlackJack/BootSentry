using System.Diagnostics;
using Microsoft.Extensions.Logging;
using Microsoft.Win32;
using BootSentry.Core.Enums;
using BootSentry.Core.Interfaces;
using BootSentry.Core.Models;
using BootSentry.Core.Parsing;

namespace BootSentry.Providers;

/// <summary>
/// Provider that scans sensitive Winlogon registry keys (Shell, Userinit, etc.).
/// These are often targeted by malware.
/// </summary>
public sealed class WinlogonProvider : IStartupProvider
{
    private readonly ILogger<WinlogonProvider> _logger;
    private readonly ISignatureVerifier? _signatureVerifier;

    // Expected default values for Winlogon entries
    private static readonly Dictionary<string, string> ExpectedValues = new(StringComparer.OrdinalIgnoreCase)
    {
        ["Shell"] = "explorer.exe",
        ["Userinit"] = @"C:\Windows\system32\userinit.exe,",
    };

    private static readonly (string ValueName, bool IsCritical)[] WinlogonValues =
    [
        ("Shell", true),
        ("Userinit", true),
        ("VmApplet", false),
        ("AppSetup", false),
        ("GinaDLL", true),
    ];

    public WinlogonProvider(ILogger<WinlogonProvider> logger, ISignatureVerifier? signatureVerifier = null)
    {
        _logger = logger;
        _signatureVerifier = signatureVerifier;
    }

    public EntryType EntryType => EntryType.Winlogon;
    public string DisplayName => "Winlogon";
    public bool RequiresAdminToRead => false;
    public bool RequiresAdminToModify => true;

    public bool IsAvailable() => true;

    public async Task<IReadOnlyCollection<StartupEntry>> ScanAsync(CancellationToken cancellationToken = default)
    {
        var entries = new List<StartupEntry>();

        await Task.Run(() =>
        {
            // Scan HKLM Winlogon
            ScanWinlogonKey(Registry.LocalMachine, @"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon", entries, cancellationToken);

            // Scan HKCU Winlogon (less common but possible)
            ScanWinlogonKey(Registry.CurrentUser, @"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon", entries, cancellationToken);
        }, cancellationToken);

        _logger.LogInformation("Found {Count} Winlogon entries", entries.Count);
        return entries;
    }

    private void ScanWinlogonKey(RegistryKey root, string path, List<StartupEntry> entries, CancellationToken cancellationToken)
    {
        try
        {
            using var key = root.OpenSubKey(path);
            if (key == null)
                return;

            var hiveName = root == Registry.LocalMachine ? "HKLM" : "HKCU";
            var fullPath = $"{hiveName}\\{path}";
            var scope = root == Registry.LocalMachine ? EntryScope.Machine : EntryScope.User;

            foreach (var (valueName, isCritical) in WinlogonValues)
            {
                cancellationToken.ThrowIfCancellationRequested();

                try
                {
                    var value = key.GetValue(valueName)?.ToString();
                    if (string.IsNullOrWhiteSpace(value))
                        continue;

                    var entry = CreateEntry(fullPath, valueName, value, scope, isCritical);
                    entries.Add(entry);
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Error reading Winlogon value {Value}", valueName);
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Error scanning Winlogon key");
        }
    }

    private StartupEntry CreateEntry(string keyPath, string valueName, string value, EntryScope scope, bool isCritical)
    {
        var id = StartupEntry.GenerateId(EntryType.Winlogon, scope, keyPath, valueName);

        // Check if value matches expected default
        var isDefaultValue = ExpectedValues.TryGetValue(valueName, out var expected) &&
                            value.Equals(expected, StringComparison.OrdinalIgnoreCase);

        // Parse the value - Winlogon values can contain multiple paths separated by commas
        var paths = value.Split(',', StringSplitOptions.RemoveEmptyEntries)
            .Select(p => p.Trim())
            .Where(p => !string.IsNullOrEmpty(p))
            .ToList();

        var primaryPath = paths.FirstOrDefault();
        var parsed = primaryPath != null ? CommandLineParser.Parse(primaryPath) : null;
        var targetPath = parsed != null ? CommandLineParser.ResolvePath(parsed.Executable) : null;
        var fileExists = targetPath != null && File.Exists(targetPath);

        var entry = new StartupEntry
        {
            Id = id,
            Type = EntryType.Winlogon,
            Scope = scope,
            DisplayName = $"Winlogon: {valueName}",
            SourcePath = keyPath,
            SourceName = valueName,
            CommandLineRaw = value,
            CommandLineNormalized = Environment.ExpandEnvironmentVariables(value),
            TargetPath = targetPath,
            FileExists = fileExists,
            Status = EntryStatus.Enabled,
            IsProtected = isCritical && isDefaultValue,
            ProtectionReason = isCritical && isDefaultValue ? "Valeur système par défaut" : null
        };

        // Determine risk level
        if (isDefaultValue)
        {
            entry.RiskLevel = RiskLevel.Safe;
            entry.Notes = "Valeur par défaut Windows";
        }
        else if (isCritical)
        {
            // Modified critical value - potentially suspicious
            entry.RiskLevel = RiskLevel.Suspicious;
            entry.Notes = "Valeur critique modifiée - vérification recommandée";
        }
        else
        {
            entry.RiskLevel = RiskLevel.Unknown;
        }

        // Get file metadata if target exists
        if (fileExists && targetPath != null)
        {
            EnrichWithFileMetadata(entry, targetPath);
        }

        return entry;
    }

    private void EnrichWithFileMetadata(StartupEntry entry, string filePath)
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

            // If Microsoft signed, update risk level
            if (versionInfo.CompanyName?.Contains("Microsoft", StringComparison.OrdinalIgnoreCase) == true)
            {
                if (entry.RiskLevel != RiskLevel.Safe)
                    entry.RiskLevel = RiskLevel.Safe;
            }
        }
        catch (Exception)
        {
            // Ignore metadata errors
        }
    }
}
