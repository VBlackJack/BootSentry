using Microsoft.Extensions.Logging;
using Microsoft.Win32;
using BootSentry.Core.Enums;
using BootSentry.Core.Interfaces;
using BootSentry.Core.Models;
using BootSentry.Core.Parsing;

namespace BootSentry.Providers;

/// <summary>
/// Provider that scans Windows Policies Run keys.
/// These are often used by Group Policy but can also be abused by malware.
/// </summary>
public sealed class RegistryPoliciesProvider : IStartupProvider
{
    private readonly ILogger<RegistryPoliciesProvider> _logger;
    private readonly ISignatureVerifier? _signatureVerifier;

    private static readonly (RegistryKey Root, string Path, EntryScope Scope)[] PolicyPaths =
    {
        (Registry.CurrentUser, @"Software\Microsoft\Windows\CurrentVersion\Policies\Explorer\Run", EntryScope.User),
        (Registry.LocalMachine, @"Software\Microsoft\Windows\CurrentVersion\Policies\Explorer\Run", EntryScope.Machine),
        // Also check system policies
        (Registry.LocalMachine, @"Software\Microsoft\Windows\CurrentVersion\Policies\System", EntryScope.Machine),
    };

    public RegistryPoliciesProvider(ILogger<RegistryPoliciesProvider> logger, ISignatureVerifier? signatureVerifier = null)
    {
        _logger = logger;
        _signatureVerifier = signatureVerifier;
    }

    public EntryType EntryType => EntryType.RegistryPolicies;
    public string DisplayName => "Policies";
    public bool RequiresAdminToRead => false;
    public bool RequiresAdminToModify => true; // Policies typically require admin

    public bool IsAvailable() => true;

    public async Task<IReadOnlyCollection<StartupEntry>> ScanAsync(CancellationToken cancellationToken = default)
    {
        var entries = new List<StartupEntry>();

        foreach (var (root, path, scope) in PolicyPaths)
        {
            cancellationToken.ThrowIfCancellationRequested();
            await ScanPolicyKeyAsync(root, path, scope, entries, cancellationToken);
        }

        _logger.LogInformation("Found {Count} policy entries", entries.Count);
        return entries;
    }

    private async Task ScanPolicyKeyAsync(
        RegistryKey rootKey,
        string keyPath,
        EntryScope scope,
        List<StartupEntry> entries,
        CancellationToken cancellationToken)
    {
        try
        {
            using var key = rootKey.OpenSubKey(keyPath);
            if (key == null)
            {
                _logger.LogDebug("Policy key not found: {Root}\\{Path}", rootKey.Name, keyPath);
                return;
            }

            // For Run keys, scan values
            if (keyPath.EndsWith("Run", StringComparison.OrdinalIgnoreCase))
            {
                foreach (var valueName in key.GetValueNames())
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    try
                    {
                        var value = key.GetValue(valueName) as string;
                        if (string.IsNullOrEmpty(value)) continue;

                        var entry = await CreateEntryFromValueAsync(
                            rootKey, keyPath, valueName, value, scope, cancellationToken);
                        if (entry != null)
                        {
                            entries.Add(entry);
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning(ex, "Error reading policy value {Value}", valueName);
                    }
                }
            }
            // For System key, check specific values
            else if (keyPath.EndsWith("System", StringComparison.OrdinalIgnoreCase))
            {
                // Check Shell value
                var shell = key.GetValue("Shell") as string;
                if (!string.IsNullOrEmpty(shell) && !shell.Equals("explorer.exe", StringComparison.OrdinalIgnoreCase))
                {
                    var entry = await CreateEntryFromValueAsync(
                        rootKey, keyPath, "Shell", shell, scope, cancellationToken);
                    if (entry != null)
                    {
                        entry.RiskLevel = RiskLevel.Critical;
                        entry.Notes = "Shell personnalisé via Policies - peut indiquer un malware!";
                        entries.Add(entry);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error scanning policy key {Root}\\{Path}", rootKey.Name, keyPath);
        }
    }

    private async Task<StartupEntry?> CreateEntryFromValueAsync(
        RegistryKey rootKey,
        string keyPath,
        string valueName,
        string value,
        EntryScope scope,
        CancellationToken cancellationToken)
    {
        var rootName = rootKey == Registry.CurrentUser ? "HKCU" : "HKLM";
        var fullPath = $@"{rootName}\{keyPath}";
        var id = StartupEntry.GenerateId(EntryType.RegistryPolicies, scope, fullPath, valueName);

        var parsed = CommandLineParser.Parse(value);
        var targetPath = parsed?.Executable;
        var fileExists = !string.IsNullOrEmpty(targetPath) && File.Exists(targetPath);

        var entry = new StartupEntry
        {
            Id = id,
            Type = EntryType.RegistryPolicies,
            Scope = scope,
            DisplayName = valueName,
            SourcePath = fullPath,
            SourceName = valueName,
            CommandLineRaw = value,
            CommandLineNormalized = parsed?.Normalized ?? value,
            TargetPath = targetPath,
            Arguments = parsed?.Arguments,
            FileExists = fileExists,
            Status = EntryStatus.Enabled,
            RiskLevel = DetermineRiskLevel(valueName, value, targetPath, fileExists),
            Notes = GenerateNotes(keyPath)
        };

        // Enrich with file metadata
        if (fileExists && targetPath != null)
        {
            await EnrichWithFileMetadataAsync(entry, targetPath, cancellationToken);
        }

        return entry;
    }

    private static RiskLevel DetermineRiskLevel(string valueName, string value, string? targetPath, bool fileExists)
    {
        if (!fileExists)
            return RiskLevel.Suspicious;

        var valueLower = value.ToLowerInvariant();

        // Check for suspicious patterns
        if (CommandLineParser.IsSuspiciousCommandLine(value))
            return RiskLevel.Suspicious;

        if (string.IsNullOrEmpty(targetPath))
            return RiskLevel.Unknown;

        var pathLower = targetPath.ToLowerInvariant();

        // Standard locations are typically safe
        if (pathLower.Contains(@"\program files") || pathLower.Contains(@"\windows\"))
            return RiskLevel.Safe;

        // Suspicious locations
        if (pathLower.Contains("temp") ||
            pathLower.Contains("appdata") ||
            pathLower.Contains("downloads"))
            return RiskLevel.Suspicious;

        return RiskLevel.Unknown;
    }

    private static string GenerateNotes(string keyPath)
    {
        if (keyPath.Contains("Policies"))
            return "Entrée définie par stratégie de groupe (GPO) ou politique locale";
        return "Entrée de démarrage via Policies";
    }

    private async Task EnrichWithFileMetadataAsync(StartupEntry entry, string filePath, CancellationToken cancellationToken)
    {
        try
        {
            var fileInfo = new FileInfo(filePath);
            entry.FileSize = fileInfo.Length;
            entry.LastModified = fileInfo.LastWriteTime;

            var versionInfo = System.Diagnostics.FileVersionInfo.GetVersionInfo(filePath);
            entry.FileVersion = versionInfo.FileVersion;
            entry.ProductName = versionInfo.ProductName;
            entry.CompanyName = versionInfo.CompanyName;
            entry.FileDescription = versionInfo.FileDescription;
            entry.Publisher = versionInfo.CompanyName;

            if (_signatureVerifier != null)
            {
                var sigInfo = await _signatureVerifier.VerifyAsync(filePath, cancellationToken);
                entry.SignatureStatus = sigInfo.Status;

                if (!string.IsNullOrEmpty(sigInfo.SignerName))
                    entry.Publisher = sigInfo.SignerName;

                if (sigInfo.Status == SignatureStatus.SignedTrusted)
                    entry.RiskLevel = RiskLevel.Safe;
            }
        }
        catch (Exception)
        {
            // Ignore metadata errors
        }
    }
}
