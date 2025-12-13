using Microsoft.Extensions.Logging;
using Microsoft.Win32;
using BootSentry.Core.Enums;
using BootSentry.Core.Interfaces;
using BootSentry.Core.Models;

namespace BootSentry.Providers;

/// <summary>
/// Provider that scans boot-start and system-start drivers.
/// Expert mode only - modifying drivers is dangerous.
/// </summary>
public sealed class DriverProvider : IStartupProvider
{
    private readonly ILogger<DriverProvider> _logger;
    private readonly ISignatureVerifier? _signatureVerifier;

    private const string ServicesPath = @"SYSTEM\CurrentControlSet\Services";

    // Driver start types that run at boot
    private static readonly int[] BootStartTypes = { 0, 1, 2 }; // Boot, System, Automatic

    public DriverProvider(ILogger<DriverProvider> logger, ISignatureVerifier? signatureVerifier = null)
    {
        _logger = logger;
        _signatureVerifier = signatureVerifier;
    }

    public EntryType EntryType => EntryType.Driver;
    public string DisplayName => "Pilotes (Drivers)";
    public bool RequiresAdminToRead => false;
    public bool RequiresAdminToModify => true;

    public bool IsAvailable() => true;

    public async Task<IReadOnlyCollection<StartupEntry>> ScanAsync(CancellationToken cancellationToken = default)
    {
        var entries = new List<StartupEntry>();

        try
        {
            using var servicesKey = Registry.LocalMachine.OpenSubKey(ServicesPath);
            if (servicesKey == null)
            {
                _logger.LogWarning("Services registry key not found");
                return entries;
            }

            foreach (var serviceName in servicesKey.GetSubKeyNames())
            {
                cancellationToken.ThrowIfCancellationRequested();

                try
                {
                    using var serviceKey = servicesKey.OpenSubKey(serviceName);
                    if (serviceKey == null) continue;

                    // Check if it's a driver (Type 1 or 2)
                    var type = serviceKey.GetValue("Type") as int? ?? 0;
                    if (type != 1 && type != 2) // 1 = Kernel driver, 2 = File system driver
                        continue;

                    var startType = serviceKey.GetValue("Start") as int? ?? 4;
                    if (!BootStartTypes.Contains(startType))
                        continue;

                    var entry = await CreateDriverEntryAsync(serviceName, serviceKey, startType, cancellationToken);
                    if (entry != null)
                    {
                        entries.Add(entry);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Error reading driver {Name}", serviceName);
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error scanning drivers");
        }

        return entries;
    }

    private async Task<StartupEntry?> CreateDriverEntryAsync(
        string driverName,
        RegistryKey driverKey,
        int startType,
        CancellationToken cancellationToken)
    {
        var imagePath = driverKey.GetValue("ImagePath") as string;
        var displayName = driverKey.GetValue("DisplayName") as string ?? driverName;
        var description = driverKey.GetValue("Description") as string;
        var group = driverKey.GetValue("Group") as string;

        // Resolve the image path
        string? resolvedPath = null;
        if (!string.IsNullOrEmpty(imagePath))
        {
            resolvedPath = ResolveDriverPath(imagePath);
        }

        var fileExists = !string.IsNullOrEmpty(resolvedPath) && File.Exists(resolvedPath);
        var fullKeyPath = $@"{ServicesPath}\{driverName}";
        var id = StartupEntry.GenerateId(EntryType.Driver, EntryScope.Machine, fullKeyPath, driverName);

        var status = startType switch
        {
            4 => EntryStatus.Disabled, // Disabled
            _ => EntryStatus.Enabled
        };

        var entry = new StartupEntry
        {
            Id = id,
            Type = EntryType.Driver,
            Scope = EntryScope.Machine,
            DisplayName = ResolveDisplayName(displayName),
            SourcePath = $@"HKLM\{fullKeyPath}",
            SourceName = driverName,
            CommandLineRaw = imagePath,
            CommandLineNormalized = resolvedPath ?? imagePath,
            TargetPath = resolvedPath,
            FileExists = fileExists,
            Status = status,
            Notes = GenerateNotes(startType, group),
            IsProtected = IsProtectedDriver(driverName, group),
            ProtectionReason = GetProtectionReason(driverName, group),
            RiskLevel = DetermineRiskLevel(driverName, resolvedPath, fileExists)
        };

        // Store additional info
        entry.ServiceStartType = GetStartTypeName(startType);
        entry.FileDescription = description;

        // Enrich with file metadata
        if (fileExists && resolvedPath != null)
        {
            await EnrichWithFileMetadataAsync(entry, resolvedPath, cancellationToken);
        }

        return entry;
    }

    private static string ResolveDriverPath(string imagePath)
    {
        // Handle various driver path formats
        var path = imagePath;

        // Remove \??\ prefix
        if (path.StartsWith(@"\??\"))
            path = path[4..];

        // Handle system32\drivers paths
        if (path.StartsWith(@"system32\", StringComparison.OrdinalIgnoreCase) ||
            path.StartsWith(@"\SystemRoot\system32\", StringComparison.OrdinalIgnoreCase))
        {
            var systemRoot = Environment.GetFolderPath(Environment.SpecialFolder.Windows);
            path = path.Replace(@"\SystemRoot\", systemRoot + @"\", StringComparison.OrdinalIgnoreCase);
            if (path.StartsWith("system32", StringComparison.OrdinalIgnoreCase))
                path = Path.Combine(systemRoot, path);
        }

        // Expand environment variables
        path = Environment.ExpandEnvironmentVariables(path);

        return path;
    }

    private static string ResolveDisplayName(string displayName)
    {
        // Handle resource strings like @%SystemRoot%\system32\...
        if (displayName.StartsWith("@"))
        {
            // For now, just return the driver name - full resolution would require LoadString
            return displayName;
        }
        return displayName;
    }

    private static string GetStartTypeName(int startType)
    {
        return startType switch
        {
            0 => "Boot",
            1 => "System",
            2 => "Automatic",
            3 => "Manual",
            4 => "Disabled",
            _ => "Unknown"
        };
    }

    private static string GenerateNotes(int startType, string? group)
    {
        var startName = GetStartTypeName(startType);
        var groupInfo = string.IsNullOrEmpty(group) ? "" : $" (Groupe: {group})";
        return $"Pilote {startName}{groupInfo}";
    }

    // Drivers that should NEVER be disabled - system will not boot without them
    private static readonly HashSet<string> CriticalBootDrivers = new(StringComparer.OrdinalIgnoreCase)
    {
        // Storage/Boot critical
        "disk", "volmgr", "volsnap", "partmgr", "volume", "fvevol",
        "iorate", "rdyboost", "vdrvroot", "wof",
        // Storage controllers
        "storahci", "stornvme", "storufs", "spaceport", "EhStorClass",
        // File systems
        "ntfs", "refs", "fastfat", "exfat", "cdfs", "udfs",
        // Core system
        "acpi", "pci", "isapnp", "msisadrv", "pdc", "intelpep", "amdppm",
        // Filter manager
        "fltmgr", "fileinfo",
        // Security
        "ksecdd", "ksecpkg", "cng", "pcw", "wfplwfs"
    };

    // Drivers that are critical but may be disabled with extreme caution
    private static readonly HashSet<string> CriticalSystemDrivers = new(StringComparer.OrdinalIgnoreCase)
    {
        // Network (system functional but no network)
        "tcpip", "netbt", "afd", "tdx", "netio", "ndis", "nsiproxy",
        // File sharing
        "rdbss", "mrxsmb", "mrxsmb20", "srv", "srv2", "srvnet",
        // Named pipes/mailslots
        "npfs", "msfs",
        // Power management
        "acpiex", "acpipagr", "compbatt", "battc"
    };

    private static readonly HashSet<string> CriticalGroups = new(StringComparer.OrdinalIgnoreCase)
    {
        "Boot Bus Extender", "System Bus Extender", "SCSI miniport",
        "SCSI Class", "SCSI CDROM Class", "FSFilter Infrastructure",
        "FSFilter System", "FSFilter Bottom", "FSFilter Copy Protection",
        "FSFilter Security Enhancer", "Filter", "Boot File System",
        "File System", "PnP Filter", "Base"
    };

    private static bool IsProtectedDriver(string driverName, string? group)
    {
        if (CriticalBootDrivers.Contains(driverName))
            return true;

        if (CriticalSystemDrivers.Contains(driverName))
            return true;

        if (!string.IsNullOrEmpty(group) && CriticalGroups.Any(g =>
            group.Equals(g, StringComparison.OrdinalIgnoreCase)))
            return true;

        return false;
    }

    /// <summary>
    /// Checks if a driver is absolutely critical for boot.
    /// These drivers should NEVER be disabled under any circumstances.
    /// </summary>
    public static bool IsCriticalBootDriver(string driverName)
    {
        return CriticalBootDrivers.Contains(driverName);
    }

    private static string? GetProtectionReason(string driverName, string? group)
    {
        if (CriticalBootDrivers.Contains(driverName))
            return "CRITIQUE: Pilote de démarrage essentiel - Désactiver ce pilote rendra Windows NON DÉMARRABLE (BSOD)";

        if (CriticalSystemDrivers.Contains(driverName))
            return "Pilote système important - La désactivation peut causer des dysfonctionnements majeurs";

        if (!string.IsNullOrEmpty(group) && CriticalGroups.Any(g =>
            group.Equals(g, StringComparison.OrdinalIgnoreCase)))
            return $"Pilote du groupe critique '{group}' - Modification déconseillée";

        return null;
    }

    private static RiskLevel DetermineRiskLevel(string driverName, string? path, bool fileExists)
    {
        if (!fileExists)
            return RiskLevel.Suspicious;

        if (string.IsNullOrEmpty(path))
            return RiskLevel.Unknown;

        var pathLower = path.ToLowerInvariant();

        // Drivers in standard locations are typically safe
        if (pathLower.Contains(@"\windows\system32\drivers\"))
            return RiskLevel.Safe;

        // Drivers outside standard paths are suspicious
        return RiskLevel.Unknown;
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
            entry.Publisher = versionInfo.CompanyName;

            if (_signatureVerifier != null)
            {
                var sigInfo = await _signatureVerifier.VerifyAsync(filePath, cancellationToken);
                entry.SignatureStatus = sigInfo.Status;

                if (!string.IsNullOrEmpty(sigInfo.SignerName))
                    entry.Publisher = sigInfo.SignerName;

                // Unsigned drivers are highly suspicious
                if (sigInfo.Status == SignatureStatus.Unsigned)
                    entry.RiskLevel = RiskLevel.Suspicious;
            }
        }
        catch (Exception)
        {
            // Ignore metadata errors
        }
    }
}
