using System.Diagnostics;
using Microsoft.Extensions.Logging;
using Microsoft.Win32;
using BootSentry.Core.Enums;
using BootSentry.Core.Interfaces;
using BootSentry.Core.Localization;
using BootSentry.Core.Models;

namespace BootSentry.Providers;

/// <summary>
/// Provider that scans Winsock Layered Service Providers (LSP).
/// LSPs can intercept network traffic and are a legacy malware vector.
/// Expert mode only.
/// </summary>
public sealed class WinsockLSPProvider : IStartupProvider
{
    private readonly ILogger<WinsockLSPProvider> _logger;
    private readonly ISignatureVerifier? _signatureVerifier;

    private const string WinsockCatalogPath = @"SYSTEM\CurrentControlSet\Services\WinSock2\Parameters";
    private const string ProtocolCatalogPath = @"Protocol_Catalog9\Catalog_Entries";
    private const string NamespaceCatalogPath = @"NameSpace_Catalog5\Catalog_Entries";

    // Known safe Windows LSP DLLs
    private static readonly HashSet<string> KnownSafeDlls = new(StringComparer.OrdinalIgnoreCase)
    {
        "mswsock.dll",
        "mswsock.dll.dll",
        "napinsp.dll",
        "pnrpnsp.dll",
        "NLAapi.dll",
        "wshbth.dll",
        "winrnr.dll",
        "wshqos.dll",
        "wshtcpip.dll",
        "wshhyperv.dll"
    };

    public WinsockLSPProvider(ILogger<WinsockLSPProvider> logger, ISignatureVerifier? signatureVerifier = null)
    {
        _logger = logger;
        _signatureVerifier = signatureVerifier;
    }

    public EntryType EntryType => EntryType.WinsockLSP;
    public string DisplayName => "Winsock LSP";
    public bool RequiresAdminToRead => false;
    public bool RequiresAdminToModify => true;

    public bool IsAvailable() => true;

    public async Task<IReadOnlyCollection<StartupEntry>> ScanAsync(CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var entries = new List<StartupEntry>();

        await Task.Run(() =>
        {
            // Scan Protocol Catalog (64-bit)
            ScanCatalog(ProtocolCatalogPath, "Protocol", entries, cancellationToken);

            // Scan Namespace Catalog (64-bit)
            ScanCatalog(NamespaceCatalogPath, "Namespace", entries, cancellationToken);

            // Also check Wow6432Node for 32-bit entries
            ScanCatalog($@"Wow6432Node\{WinsockCatalogPath}\{ProtocolCatalogPath}", "Protocol32", entries, cancellationToken);
            ScanCatalog($@"Wow6432Node\{WinsockCatalogPath}\{NamespaceCatalogPath}", "Namespace32", entries, cancellationToken);
        }, cancellationToken).ConfigureAwait(false);

        _logger.LogInformation("Found {Count} Winsock LSP entries", entries.Count);
        return entries;
    }

    private void ScanCatalog(string catalogPath, string catalogType, List<StartupEntry> entries, CancellationToken cancellationToken)
    {
        try
        {
            var fullPath = catalogPath.StartsWith("Wow6432Node")
                ? catalogPath
                : $@"{WinsockCatalogPath}\{catalogPath}";

            using var key = Registry.LocalMachine.OpenSubKey(fullPath);
            if (key == null)
            {
                _logger.LogDebug("Winsock catalog not found: {Path}", fullPath);
                return;
            }

            foreach (var subKeyName in key.GetSubKeyNames())
            {
                cancellationToken.ThrowIfCancellationRequested();

                try
                {
                    using var entryKey = key.OpenSubKey(subKeyName);
                    if (entryKey == null) continue;

                    // Read PackedCatalogItem which contains the DLL path
                    var packedItem = entryKey.GetValue("PackedCatalogItem") as byte[];
                    if (packedItem == null || packedItem.Length == 0) continue;

                    // Extract DLL path from packed data (it's at a fixed offset in the structure)
                    var dllPath = ExtractDllPath(packedItem);
                    if (string.IsNullOrEmpty(dllPath)) continue;

                    var entry = CreateEntry(fullPath, subKeyName, dllPath, catalogType);
                    if (entry != null)
                    {
                        entries.Add(entry);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Error reading Winsock catalog entry: {Key}", subKeyName);
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Error scanning Winsock catalog: {Path}", catalogPath);
        }
    }

    private static string? ExtractDllPath(byte[] packedItem)
    {
        try
        {
            // The DLL path is stored as a Unicode string in the packed data
            // For Protocol_Catalog9, it starts at offset 8
            // For NameSpace_Catalog5, it starts at offset 12
            // Try both offsets

            foreach (var offset in new[] { 8, 12, 16 })
            {
                if (offset >= packedItem.Length) continue;

                var pathBytes = new List<byte>();
                for (var i = offset; i < packedItem.Length - 1; i += 2)
                {
                    var ch = BitConverter.ToChar(packedItem, i);
                    if (ch == '\0') break;
                    pathBytes.Add(packedItem[i]);
                    pathBytes.Add(packedItem[i + 1]);
                }

                if (pathBytes.Count > 0)
                {
                    var path = System.Text.Encoding.Unicode.GetString(pathBytes.ToArray());
                    if (!string.IsNullOrEmpty(path) &&
                        (path.EndsWith(".dll", StringComparison.OrdinalIgnoreCase) ||
                         path.Contains("system32", StringComparison.OrdinalIgnoreCase)))
                    {
                        return path;
                    }
                }
            }
        }
        catch
        {
            // Ignore parsing errors
        }

        return null;
    }

    private StartupEntry? CreateEntry(string registryPath, string entryName, string dllPath, string catalogType)
    {
        var sourcePath = $"HKLM\\{registryPath}\\{entryName}";
        var dllName = Path.GetFileName(dllPath);
        var id = StartupEntry.GenerateId(EntryType.WinsockLSP, EntryScope.Machine, sourcePath, dllName);

        // Resolve the path
        var resolvedPath = Environment.ExpandEnvironmentVariables(dllPath);
        if (!Path.IsPathRooted(resolvedPath))
        {
            resolvedPath = Path.Combine(Environment.SystemDirectory, resolvedPath);
        }

        var fileExists = File.Exists(resolvedPath);

        // Determine risk level
        var riskLevel = DetermineRiskLevel(dllName, resolvedPath, fileExists);

        var entry = new StartupEntry
        {
            Id = id,
            Type = EntryType.WinsockLSP,
            Scope = EntryScope.Machine,
            DisplayName = $"LSP: {dllName} ({catalogType})",
            SourcePath = sourcePath,
            SourceName = entryName,
            CommandLineRaw = dllPath,
            TargetPath = fileExists ? resolvedPath : dllPath,
            FileExists = fileExists,
            Status = EntryStatus.Enabled,
            RiskLevel = riskLevel,
            IsProtected = riskLevel == RiskLevel.Safe,
            ProtectionReason = riskLevel == RiskLevel.Safe ? "Composant Windows" : null,
            Notes = GenerateNotes(dllName, catalogType, riskLevel)
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
            }
            catch { }
        }

        return entry;
    }

    private static RiskLevel DetermineRiskLevel(string dllName, string resolvedPath, bool fileExists)
    {
        // Known safe Windows DLLs
        if (KnownSafeDlls.Contains(dllName))
        {
            return RiskLevel.Safe;
        }

        // Microsoft DLL in system32
        if (resolvedPath.Contains("system32", StringComparison.OrdinalIgnoreCase) &&
            resolvedPath.Contains("Windows", StringComparison.OrdinalIgnoreCase))
        {
            return RiskLevel.Safe;
        }

        // File doesn't exist - suspicious
        if (!fileExists)
        {
            return RiskLevel.Suspicious;
        }

        // Non-system path is suspicious
        if (!resolvedPath.Contains("Windows", StringComparison.OrdinalIgnoreCase) &&
            !resolvedPath.Contains("Program Files", StringComparison.OrdinalIgnoreCase))
        {
            return RiskLevel.Suspicious;
        }

        return RiskLevel.Unknown;
    }

    private static string GenerateNotes(string dllName, string catalogType, RiskLevel riskLevel)
    {
        return riskLevel switch
        {
            RiskLevel.Safe => $"{Localize.Get("ProviderWinsockStandard")} ({catalogType})",
            RiskLevel.Suspicious => $"{Localize.Get("ProviderWinsockSuspicious")} ({catalogType})",
            RiskLevel.Critical => $"{Localize.Get("ProviderWinsockCritical")} ({catalogType})",
            _ => $"Winsock LSP ({catalogType})"
        };
    }
}
