using System.Diagnostics;
using Microsoft.Extensions.Logging;
using Microsoft.Win32;
using BootSentry.Core.Enums;
using BootSentry.Core.Interfaces;
using BootSentry.Core.Models;

namespace BootSentry.Providers;

/// <summary>
/// Provider that scans Shell Extension handlers (context menu handlers).
/// Expert mode only.
/// </summary>
public sealed class ShellExtensionProvider : IStartupProvider
{
    private readonly ILogger<ShellExtensionProvider> _logger;
    private readonly ISignatureVerifier? _signatureVerifier;

    private static readonly string[] ShellExLocations =
    [
        @"SOFTWARE\Classes\*\ShellEx\ContextMenuHandlers",
        @"SOFTWARE\Classes\Directory\ShellEx\ContextMenuHandlers",
        @"SOFTWARE\Classes\Folder\ShellEx\ContextMenuHandlers",
    ];

    public ShellExtensionProvider(ILogger<ShellExtensionProvider> logger, ISignatureVerifier? signatureVerifier = null)
    {
        _logger = logger;
        _signatureVerifier = signatureVerifier;
    }

    public EntryType EntryType => EntryType.ShellExtension;
    public string DisplayName => "Shell Extensions";
    public bool RequiresAdminToRead => false;
    public bool RequiresAdminToModify => true;

    public bool IsAvailable() => true;

    public async Task<IReadOnlyCollection<StartupEntry>> ScanAsync(CancellationToken cancellationToken = default)
    {
        var entries = new List<StartupEntry>();

        await Task.Run(() =>
        {
            foreach (var location in ShellExLocations)
            {
                cancellationToken.ThrowIfCancellationRequested();
                ScanLocation(Registry.LocalMachine, location, EntryScope.Machine, entries, cancellationToken);
                ScanLocation(Registry.CurrentUser, location, EntryScope.User, entries, cancellationToken);
            }
        }, cancellationToken);

        _logger.LogInformation("Found {Count} shell extension entries", entries.Count);
        return entries;
    }

    private void ScanLocation(RegistryKey root, string path, EntryScope scope, List<StartupEntry> entries, CancellationToken ct)
    {
        try
        {
            using var key = root.OpenSubKey(path);
            if (key == null) return;

            var hiveName = root == Registry.LocalMachine ? "HKLM" : "HKCU";

            foreach (var subKeyName in key.GetSubKeyNames())
            {
                ct.ThrowIfCancellationRequested();
                try
                {
                    using var subKey = key.OpenSubKey(subKeyName);
                    if (subKey == null) continue;

                    var clsid = subKey.GetValue(null)?.ToString();
                    if (string.IsNullOrEmpty(clsid)) continue;

                    var (dllPath, description) = ResolveCLSID(clsid);
                    var entry = CreateEntry($"{hiveName}\\{path}", subKeyName, clsid, dllPath, description, scope);
                    entries.Add(entry);
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Error scanning shell extension {Name}", subKeyName);
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Error scanning shell extensions at {Path}", path);
        }
    }

    private static (string? Path, string? Description) ResolveCLSID(string clsid)
    {
        try
        {
            using var clsidKey = Registry.ClassesRoot.OpenSubKey($@"CLSID\{clsid}\InprocServer32");
            if (clsidKey != null)
            {
                var path = clsidKey.GetValue(null)?.ToString();
                if (!string.IsNullOrEmpty(path))
                    path = Environment.ExpandEnvironmentVariables(path);

                using var parentKey = Registry.ClassesRoot.OpenSubKey($@"CLSID\{clsid}");
                var description = parentKey?.GetValue(null)?.ToString();
                return (path, description);
            }
        }
        catch { }
        return (null, null);
    }

    private StartupEntry CreateEntry(string sourcePath, string name, string clsid, string? dllPath, string? description, EntryScope scope)
    {
        var id = StartupEntry.GenerateId(EntryType.ShellExtension, scope, sourcePath, name);
        var fileExists = !string.IsNullOrEmpty(dllPath) && File.Exists(dllPath);

        var entry = new StartupEntry
        {
            Id = id,
            Type = EntryType.ShellExtension,
            Scope = scope,
            DisplayName = string.IsNullOrEmpty(description) ? $"Shell: {name}" : description,
            SourcePath = sourcePath,
            SourceName = name,
            CommandLineRaw = clsid,
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

                if (versionInfo.CompanyName?.Contains("Microsoft", StringComparison.OrdinalIgnoreCase) == true)
                    entry.RiskLevel = RiskLevel.Safe;
            }
            catch { }
        }

        if (!entry.FileExists && entry.RiskLevel != RiskLevel.Safe)
            entry.RiskLevel = RiskLevel.Suspicious;

        return entry;
    }
}
