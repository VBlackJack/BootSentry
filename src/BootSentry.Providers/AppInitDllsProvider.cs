using System.Diagnostics;
using Microsoft.Extensions.Logging;
using Microsoft.Win32;
using BootSentry.Core.Enums;
using BootSentry.Core.Interfaces;
using BootSentry.Core.Models;

namespace BootSentry.Providers;

/// <summary>
/// Provider that scans AppInit_DLLs registry entries.
/// These DLLs are loaded into every process that loads user32.dll.
/// Common malware persistence vector - Expert mode only.
/// </summary>
public sealed class AppInitDllsProvider : IStartupProvider
{
    private readonly ILogger<AppInitDllsProvider> _logger;
    private readonly ISignatureVerifier? _signatureVerifier;

    private static readonly (string Path, string View)[] Locations =
    [
        (@"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Windows", "64"),
        (@"SOFTWARE\Wow6432Node\Microsoft\Windows NT\CurrentVersion\Windows", "32"),
    ];

    public AppInitDllsProvider(ILogger<AppInitDllsProvider> logger, ISignatureVerifier? signatureVerifier = null)
    {
        _logger = logger;
        _signatureVerifier = signatureVerifier;
    }

    public EntryType EntryType => EntryType.AppInitDlls;
    public string DisplayName => "AppInit_DLLs";
    public bool RequiresAdminToRead => false;
    public bool RequiresAdminToModify => true;

    public bool IsAvailable() => true;

    public async Task<IReadOnlyCollection<StartupEntry>> ScanAsync(CancellationToken cancellationToken = default)
    {
        var entries = new List<StartupEntry>();

        await Task.Run(() =>
        {
            foreach (var location in Locations)
            {
                cancellationToken.ThrowIfCancellationRequested();
                ScanLocation(location.Path, location.View, entries);
            }
        }, cancellationToken);

        _logger.LogInformation("Found {Count} AppInit_DLLs entries", entries.Count);
        return entries;
    }

    private void ScanLocation(string path, string view, List<StartupEntry> entries)
    {
        try
        {
            using var key = Registry.LocalMachine.OpenSubKey(path);
            if (key == null) return;

            // Check if AppInit_DLLs loading is enabled
            var loadAppInit = key.GetValue("LoadAppInit_DLLs");
            var isEnabled = loadAppInit is int value && value == 1;

            // Get the DLL list
            var appInitDlls = key.GetValue("AppInit_DLLs")?.ToString();
            if (string.IsNullOrWhiteSpace(appInitDlls)) return;

            // AppInit_DLLs can contain multiple DLLs separated by space or comma
            var dlls = appInitDlls.Split(new[] { ' ', ',', ';' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var dll in dlls)
            {
                var dllPath = dll.Trim();
                if (string.IsNullOrEmpty(dllPath)) continue;

                var entry = CreateEntry(path, dllPath, view, isEnabled);
                entries.Add(entry);
            }
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Error scanning AppInit_DLLs at {Path}", path);
        }
    }

    private StartupEntry CreateEntry(string registryPath, string dllPath, string view, bool isEnabled)
    {
        var sourcePath = $"HKLM\\{registryPath}";
        var dllName = Path.GetFileName(dllPath);
        var id = StartupEntry.GenerateId(EntryType.AppInitDlls, EntryScope.Machine, sourcePath, dllName);

        // Resolve the path
        var resolvedPath = Environment.ExpandEnvironmentVariables(dllPath);
        if (!Path.IsPathRooted(resolvedPath))
        {
            resolvedPath = Path.Combine(Environment.SystemDirectory, resolvedPath);
        }

        var fileExists = File.Exists(resolvedPath);

        var entry = new StartupEntry
        {
            Id = id,
            Type = EntryType.AppInitDlls,
            Scope = EntryScope.Machine,
            DisplayName = $"AppInit: {dllName}",
            SourcePath = sourcePath,
            SourceName = "AppInit_DLLs",
            CommandLineRaw = dllPath,
            TargetPath = fileExists ? resolvedPath : dllPath,
            FileExists = fileExists,
            Status = isEnabled ? EntryStatus.Enabled : EntryStatus.Disabled,
            RiskLevel = RiskLevel.Critical,
            RegistryView = view,
            IsProtected = false,
            Notes = "AppInit_DLLs - vecteur de persistence malware courant"
        };

        if (fileExists)
        {
            try
            {
                var fileInfo = new FileInfo(resolvedPath);
                entry.FileSize = fileInfo.Length;
                entry.LastModified = fileInfo.LastWriteTime;
                var versionInfo = FileVersionInfo.GetVersionInfo(resolvedPath);
                entry.FileVersion = versionInfo.FileVersion;
                entry.Publisher = versionInfo.CompanyName;

                // Microsoft DLLs in AppInit_DLLs are still suspicious
                if (versionInfo.CompanyName?.Contains("Microsoft", StringComparison.OrdinalIgnoreCase) == true)
                {
                    entry.RiskLevel = RiskLevel.Suspicious;
                    entry.Notes = "DLL Microsoft dans AppInit_DLLs - inhabituel";
                }
            }
            catch (Exception ex)
            {
                _logger.LogDebug(ex, "Failed to get file info for {Path}", resolvedPath);
            }
        }
        else
        {
            entry.Notes = "DLL introuvable - entrée orpheline suspecte";
        }

        return entry;
    }
}
