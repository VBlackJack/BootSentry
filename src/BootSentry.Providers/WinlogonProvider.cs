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

        // Scan HKLM Winlogon
        await ScanWinlogonKeyAsync(Registry.LocalMachine, @"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon", entries, cancellationToken);

        // Scan HKCU Winlogon (less common but possible)
        await ScanWinlogonKeyAsync(Registry.CurrentUser, @"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon", entries, cancellationToken);

        _logger.LogInformation("Found {Count} Winlogon entries", entries.Count);
        return entries;
    }

    private async Task ScanWinlogonKeyAsync(RegistryKey root, string path, List<StartupEntry> entries, CancellationToken cancellationToken)
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

                    var entry = await CreateEntryAsync(fullPath, valueName, value, scope, isCritical, cancellationToken);
                    entries.Add(entry);
                }
                catch (OperationCanceledException)
                {
                    throw;
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Error reading Winlogon value {Value}", valueName);
                }
            }
        }
        catch (OperationCanceledException)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Error scanning Winlogon key");
        }
    }

    private async Task<StartupEntry> CreateEntryAsync(string keyPath, string valueName, string value, EntryScope scope, bool isCritical, CancellationToken cancellationToken)
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
            await EnrichWithFileMetadataAsync(entry, targetPath, cancellationToken);
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
            
            // Default publisher from file info
            entry.Publisher = versionInfo.CompanyName;

            // Use Signature Verifier if available
            if (_signatureVerifier != null)
            {
                var sigInfo = await _signatureVerifier.VerifyAsync(filePath, cancellationToken);
                entry.SignatureStatus = sigInfo.Status;
                
                // If signed, use the signer name as publisher (more trustworthy)
                if (sigInfo.Status != SignatureStatus.Unsigned && !string.IsNullOrEmpty(sigInfo.SignerName))
                {
                    entry.Publisher = sigInfo.SignerName;
                }

                // If Microsoft signed and trusted, mark as Safe
                if (sigInfo.Status == SignatureStatus.SignedTrusted && 
                    entry.Publisher?.Contains("Microsoft", StringComparison.OrdinalIgnoreCase) == true)
                {
                    if (entry.RiskLevel != RiskLevel.Safe)
                        entry.RiskLevel = RiskLevel.Safe;
                }
            }
            else
            {
                 // Fallback to simple check if verifier missing (legacy behavior)
                 if (versionInfo.CompanyName?.Contains("Microsoft", StringComparison.OrdinalIgnoreCase) == true)
                 {
                     if (entry.RiskLevel != RiskLevel.Safe)
                         entry.RiskLevel = RiskLevel.Safe;
                 }
            }
        }
        catch (Exception)
        {
            // Ignore metadata errors
        }
    }
}