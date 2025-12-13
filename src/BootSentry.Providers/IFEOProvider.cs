using Microsoft.Extensions.Logging;
using Microsoft.Win32;
using BootSentry.Core.Enums;
using BootSentry.Core.Interfaces;
using BootSentry.Core.Models;

namespace BootSentry.Providers;

/// <summary>
/// Provider that scans Image File Execution Options (IFEO) for debugger entries.
/// IFEO debugger entries are a common malware persistence mechanism.
/// </summary>
public sealed class IFEOProvider : IStartupProvider
{
    private readonly ILogger<IFEOProvider> _logger;
    private readonly ISignatureVerifier? _signatureVerifier;

    private const string IFEOPath = @"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Image File Execution Options";

    public IFEOProvider(ILogger<IFEOProvider> logger, ISignatureVerifier? signatureVerifier = null)
    {
        _logger = logger;
        _signatureVerifier = signatureVerifier;
    }

    public EntryType EntryType => EntryType.IFEO;
    public string DisplayName => "Image File Execution Options";
    public bool RequiresAdminToRead => false;
    public bool RequiresAdminToModify => true;

    public bool IsAvailable() => true;

    public async Task<IReadOnlyCollection<StartupEntry>> ScanAsync(CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var entries = new List<StartupEntry>();

        // Scan HKLM IFEO
        await ScanIFEOAsync(Registry.LocalMachine, EntryScope.Machine, entries, cancellationToken);

        return entries;
    }

    private async Task ScanIFEOAsync(
        RegistryKey rootKey,
        EntryScope scope,
        List<StartupEntry> entries,
        CancellationToken cancellationToken)
    {
        try
        {
            using var ifeoKey = rootKey.OpenSubKey(IFEOPath);
            if (ifeoKey == null)
            {
                _logger.LogDebug("IFEO key not found in {Root}", rootKey.Name);
                return;
            }

            foreach (var subKeyName in ifeoKey.GetSubKeyNames())
            {
                cancellationToken.ThrowIfCancellationRequested();

                try
                {
                    using var appKey = ifeoKey.OpenSubKey(subKeyName);
                    if (appKey == null) continue;

                    // Check for Debugger value - this is the persistence mechanism
                    var debugger = appKey.GetValue("Debugger") as string;
                    if (!string.IsNullOrEmpty(debugger))
                    {
                        var entry = await CreateEntryFromDebuggerAsync(
                            subKeyName, debugger, scope, cancellationToken);
                        if (entry != null)
                        {
                            entries.Add(entry);
                        }
                    }

                    // Also check for GlobalFlag (less common but can be used for persistence)
                    var globalFlag = appKey.GetValue("GlobalFlag");
                    if (globalFlag != null)
                    {
                        // GlobalFlag with certain values can indicate tampering
                        _logger.LogDebug("GlobalFlag found for {App}: {Value}", subKeyName, globalFlag);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Error reading IFEO entry for {App}", subKeyName);
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error scanning IFEO in {Root}", rootKey.Name);
        }
    }

    private async Task<StartupEntry?> CreateEntryFromDebuggerAsync(
        string applicationName,
        string debuggerPath,
        EntryScope scope,
        CancellationToken cancellationToken)
    {
        var fullKeyPath = $@"{IFEOPath}\{applicationName}";
        var id = StartupEntry.GenerateId(EntryType.IFEO, scope, fullKeyPath, "Debugger");

        // Parse the debugger command line
        var parsed = Core.Parsing.CommandLineParser.Parse(debuggerPath);
        var targetPath = parsed?.Executable;
        var fileExists = !string.IsNullOrEmpty(targetPath) && File.Exists(targetPath);

        // Determine risk level - IFEO debuggers are usually suspicious
        var riskLevel = DetermineRiskLevel(applicationName, debuggerPath, targetPath);

        var entry = new StartupEntry
        {
            Id = id,
            Type = EntryType.IFEO,
            Scope = scope,
            DisplayName = $"IFEO: {applicationName}",
            SourcePath = $@"HKLM\{fullKeyPath}",
            SourceName = "Debugger",
            CommandLineRaw = debuggerPath,
            CommandLineNormalized = parsed?.Normalized ?? debuggerPath,
            TargetPath = targetPath,
            Arguments = parsed?.Arguments,
            FileExists = fileExists,
            Status = EntryStatus.Enabled,
            RiskLevel = riskLevel,
            Notes = GenerateNotes(applicationName, debuggerPath, riskLevel)
        };

        // Enrich with file metadata if target exists
        if (fileExists && targetPath != null)
        {
            await EnrichWithFileMetadataAsync(entry, targetPath, cancellationToken);
        }

        return entry;
    }

    private static RiskLevel DetermineRiskLevel(string applicationName, string debuggerPath, string? targetPath)
    {
        var pathLower = debuggerPath.ToLowerInvariant();
        var appLower = applicationName.ToLowerInvariant();

        // Known legitimate debuggers
        var legitimateDebuggers = new[]
        {
            "vsjitdebugger.exe",
            "windbg.exe",
            "devenv.exe",
            "msvsmon.exe"
        };

        if (legitimateDebuggers.Any(d => pathLower.Contains(d)))
        {
            return RiskLevel.Safe;
        }

        // Suspicious: debugger for system tools
        var suspiciousTargets = new[]
        {
            "taskmgr.exe",
            "regedit.exe",
            "cmd.exe",
            "powershell.exe",
            "mmc.exe",
            "msconfig.exe",
            "utilman.exe",
            "sethc.exe",
            "osk.exe",
            "narrator.exe",
            "magnify.exe"
        };

        if (suspiciousTargets.Any(t => appLower.Contains(t)))
        {
            return RiskLevel.Critical;
        }

        // Suspicious paths
        if (pathLower.Contains("temp") ||
            pathLower.Contains("appdata") ||
            pathLower.Contains("downloads") ||
            pathLower.Contains("users") && !pathLower.Contains("program"))
        {
            return RiskLevel.Suspicious;
        }

        return RiskLevel.Unknown;
    }

    private static string GenerateNotes(string applicationName, string debuggerPath, RiskLevel riskLevel)
    {
        return riskLevel switch
        {
            RiskLevel.Critical => $"ATTENTION: Debugger IFEO sur {applicationName} - vecteur malware courant!",
            RiskLevel.Suspicious => $"Debugger IFEO suspect pour {applicationName}",
            RiskLevel.Safe => $"Debugger légitime pour {applicationName}",
            _ => $"Debugger IFEO pour {applicationName}"
        };
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
            }
        }
        catch (Exception)
        {
            // Ignore metadata errors
        }
    }
}
