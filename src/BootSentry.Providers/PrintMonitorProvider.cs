using System.Diagnostics;
using Microsoft.Extensions.Logging;
using Microsoft.Win32;
using BootSentry.Core.Enums;
using BootSentry.Core.Interfaces;
using BootSentry.Core.Models;

namespace BootSentry.Providers;

/// <summary>
/// Provider that scans Print Monitors (spooler subsystem).
/// Print monitors can be used for persistence - Expert mode only.
/// </summary>
public sealed class PrintMonitorProvider : IStartupProvider
{
    private readonly ILogger<PrintMonitorProvider> _logger;
    private readonly ISignatureVerifier? _signatureVerifier;

    private const string PrintMonitorsPath = @"SYSTEM\CurrentControlSet\Control\Print\Monitors";

    public PrintMonitorProvider(ILogger<PrintMonitorProvider> logger, ISignatureVerifier? signatureVerifier = null)
    {
        _logger = logger;
        _signatureVerifier = signatureVerifier;
    }

    public EntryType EntryType => EntryType.PrintMonitor;
    public string DisplayName => "Print Monitors";
    public bool RequiresAdminToRead => false;
    public bool RequiresAdminToModify => true;

    public bool IsAvailable() => true;

    public async Task<IReadOnlyCollection<StartupEntry>> ScanAsync(CancellationToken cancellationToken = default)
    {
        var entries = new List<StartupEntry>();

        await Task.Run(() =>
        {
            cancellationToken.ThrowIfCancellationRequested();
            ScanMonitors(entries, cancellationToken);
        }, cancellationToken);

        _logger.LogInformation("Found {Count} print monitor entries", entries.Count);
        return entries;
    }

    private void ScanMonitors(List<StartupEntry> entries, CancellationToken ct)
    {
        try
        {
            using var key = Registry.LocalMachine.OpenSubKey(PrintMonitorsPath);
            if (key == null) return;

            foreach (var monitorName in key.GetSubKeyNames())
            {
                ct.ThrowIfCancellationRequested();
                try
                {
                    using var monitorKey = key.OpenSubKey(monitorName);
                    if (monitorKey == null) continue;

                    var driver = monitorKey.GetValue("Driver")?.ToString();
                    if (string.IsNullOrEmpty(driver)) continue;

                    var dllPath = ResolveDllPath(driver);
                    var entry = CreateEntry(monitorName, driver, dllPath);
                    entries.Add(entry);
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Error scanning print monitor {Name}", monitorName);
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Error scanning print monitors");
        }
    }

    private static string? ResolveDllPath(string driver)
    {
        // Print monitor DLLs are typically in System32\spool\prtprocs\x64 or just System32
        var possiblePaths = new[]
        {
            Path.Combine(Environment.SystemDirectory, driver),
            Path.Combine(Environment.SystemDirectory, "spool", "prtprocs", "x64", driver),
            Path.Combine(Environment.SystemDirectory, "spool", "drivers", "x64", "3", driver),
        };

        foreach (var path in possiblePaths)
        {
            if (File.Exists(path))
                return path;
        }

        // Try as absolute path
        if (File.Exists(driver))
            return driver;

        // Return the System32 path as default
        return Path.Combine(Environment.SystemDirectory, driver);
    }

    private StartupEntry CreateEntry(string monitorName, string driver, string? dllPath)
    {
        var sourcePath = $"HKLM\\{PrintMonitorsPath}\\{monitorName}";
        var id = StartupEntry.GenerateId(EntryType.PrintMonitor, EntryScope.Machine, sourcePath, monitorName);
        var fileExists = !string.IsNullOrEmpty(dllPath) && File.Exists(dllPath);

        var entry = new StartupEntry
        {
            Id = id,
            Type = EntryType.PrintMonitor,
            Scope = EntryScope.Machine,
            DisplayName = $"Print Monitor: {monitorName}",
            SourcePath = sourcePath,
            SourceName = monitorName,
            CommandLineRaw = driver,
            TargetPath = dllPath,
            FileExists = fileExists,
            Status = EntryStatus.Enabled,
            RiskLevel = RiskLevel.Unknown,
            IsProtected = false
        };

        if (fileExists && dllPath != null)
        {
            try
            {
                var fileInfo = new FileInfo(dllPath);
                entry.FileSize = fileInfo.Length;
                entry.LastModified = fileInfo.LastWriteTime;
                var versionInfo = FileVersionInfo.GetVersionInfo(dllPath);
                entry.FileVersion = versionInfo.FileVersion;
                entry.Publisher = versionInfo.CompanyName;

                // Microsoft print monitors are safe
                if (versionInfo.CompanyName?.Contains("Microsoft", StringComparison.OrdinalIgnoreCase) == true)
                {
                    entry.RiskLevel = RiskLevel.Safe;
                    entry.IsProtected = true;
                    entry.ProtectionReason = "Composant système Microsoft";
                }
            }
            catch
            {
                // File info retrieval can fail for locked/inaccessible files
            }
        }

        // Non-Microsoft print monitors are potentially suspicious
        if (!entry.IsProtected && entry.RiskLevel == RiskLevel.Unknown)
        {
            entry.RiskLevel = RiskLevel.Suspicious;
            entry.Notes = "Print monitor tiers - vérifiez l'éditeur";
        }

        if (!entry.FileExists && entry.RiskLevel != RiskLevel.Safe)
        {
            entry.RiskLevel = RiskLevel.Suspicious;
            entry.Notes = "Fichier DLL introuvable";
        }

        return entry;
    }
}
