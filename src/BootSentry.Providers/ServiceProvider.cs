using System.Diagnostics;
using System.ServiceProcess;
using Microsoft.Extensions.Logging;
using Microsoft.Win32;
using BootSentry.Core.Enums;
using BootSentry.Core.Interfaces;
using BootSentry.Core.Models;
using BootSentry.Core.Parsing;

namespace BootSentry.Providers;

/// <summary>
/// Provider that scans Windows Services for auto-start services.
/// </summary>
public sealed class ServiceProvider : IStartupProvider
{
    private readonly ILogger<ServiceProvider> _logger;
    private readonly ISignatureVerifier? _signatureVerifier;

    // Services that are critical to Windows operation
    private static readonly HashSet<string> ProtectedServices = new(StringComparer.OrdinalIgnoreCase)
    {
        "wuauserv", "wsearch", "winmgmt", "wmi", "w32time", "vss",
        "trustedinstaller", "themes", "systemeventsbroker", "storsvc",
        "spooler", "shellhwdetection", "sens", "seclogon", "schedule",
        "samss", "rpcss", "rpclocator", "plugplay", "pcasvc", "nsi",
        "netlogon", "mpssvc", "lmhosts", "lanmanworkstation", "lanmanserver",
        "keyiso", "iphlpsvc", "gpsvc", "eventlog", "dnscache", "dhcp",
        "dcomlaunch", "cryptsvc", "compsvc", "bits", "bfe", "audiosrv",
        "appinfo", "appidsvc"
    };

    public ServiceProvider(ILogger<ServiceProvider> logger, ISignatureVerifier? signatureVerifier = null)
    {
        _logger = logger;
        _signatureVerifier = signatureVerifier;
    }

    public EntryType EntryType => EntryType.Service;
    public string DisplayName => "Windows Services";
    public bool RequiresAdminToRead => false;
    public bool RequiresAdminToModify => true;

    public bool IsAvailable() => true;

    public async Task<IReadOnlyCollection<StartupEntry>> ScanAsync(CancellationToken cancellationToken = default)
    {
        var entries = new List<StartupEntry>();

        await Task.Run(() =>
        {
            try
            {
                var services = ServiceController.GetServices();

                foreach (var service in services)
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    try
                    {
                        // Only include auto-start services (not demand-start)
                        var startType = GetServiceStartType(service.ServiceName);
                        if (startType is not ("Automatic" or "Automatic (Delayed)" or "Auto"))
                            continue;

                        var entry = CreateEntry(service, startType);
                        if (entry != null)
                            entries.Add(entry);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning(ex, "Error processing service {Service}", service.ServiceName);
                    }
                    finally
                    {
                        service.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error scanning services");
            }
        }, cancellationToken);

        _logger.LogInformation("Found {Count} service entries", entries.Count);
        return entries;
    }

    private static string? GetServiceStartType(string serviceName)
    {
        try
        {
            using var key = Registry.LocalMachine.OpenSubKey($@"SYSTEM\CurrentControlSet\Services\{serviceName}");
            if (key == null)
                return null;

            var start = key.GetValue("Start");
            if (start == null)
                return null;

            var startValue = Convert.ToInt32(start);
            var delayedAutoStart = key.GetValue("DelayedAutostart");
            var isDelayed = delayedAutoStart != null && Convert.ToInt32(delayedAutoStart) == 1;

            return startValue switch
            {
                0 => "Boot",
                1 => "System",
                2 => isDelayed ? "Automatic (Delayed)" : "Automatic",
                3 => "Manual",
                4 => "Disabled",
                _ => null
            };
        }
        catch
        {
            return null;
        }
    }

    private StartupEntry? CreateEntry(ServiceController service, string startType)
    {
        var imagePath = GetServiceImagePath(service.ServiceName);
        if (string.IsNullOrEmpty(imagePath))
            return null;

        var parsed = CommandLineParser.Parse(imagePath);
        var targetPath = parsed != null ? CommandLineParser.ResolvePath(parsed.Executable) : null;
        var fileExists = targetPath != null && File.Exists(targetPath);

        var id = StartupEntry.GenerateId(EntryType.Service, EntryScope.Machine, "Services", service.ServiceName);

        // Determine if service is protected
        var isProtected = ProtectedServices.Contains(service.ServiceName);

        // Get service account
        var serviceAccount = GetServiceAccount(service.ServiceName);
        var isMicrosoftService = IsMicrosoftService(service, targetPath);

        var entry = new StartupEntry
        {
            Id = id,
            Type = EntryType.Service,
            Scope = EntryScope.Machine,
            DisplayName = service.DisplayName,
            SourcePath = "Services",
            SourceName = service.ServiceName,
            CommandLineRaw = imagePath,
            CommandLineNormalized = parsed?.Normalized ?? imagePath,
            TargetPath = targetPath,
            Arguments = parsed?.Arguments,
            FileExists = fileExists,
            Status = startType == "Disabled" ? EntryStatus.Disabled : EntryStatus.Enabled,
            ServiceImagePath = imagePath,
            ServiceStartType = startType,
            ServiceAccount = serviceAccount,
            IsProtected = isProtected || isMicrosoftService,
            ProtectionReason = isProtected ? "Service système critique" : (isMicrosoftService ? "Service Microsoft" : null)
        };

        // Get file metadata if target exists
        if (fileExists && targetPath != null)
        {
            EnrichWithFileMetadata(entry, targetPath, isMicrosoftService);
        }

        return entry;
    }

    private static string? GetServiceImagePath(string serviceName)
    {
        try
        {
            using var key = Registry.LocalMachine.OpenSubKey($@"SYSTEM\CurrentControlSet\Services\{serviceName}");
            return key?.GetValue("ImagePath")?.ToString();
        }
        catch
        {
            return null;
        }
    }

    private static string? GetServiceAccount(string serviceName)
    {
        try
        {
            using var key = Registry.LocalMachine.OpenSubKey($@"SYSTEM\CurrentControlSet\Services\{serviceName}");
            return key?.GetValue("ObjectName")?.ToString();
        }
        catch
        {
            return null;
        }
    }

    private static bool IsMicrosoftService(ServiceController service, string? targetPath)
    {
        // Check if path is in Windows directory
        if (targetPath != null)
        {
            var windowsDir = Environment.GetFolderPath(Environment.SpecialFolder.Windows);
            if (targetPath.StartsWith(windowsDir, StringComparison.OrdinalIgnoreCase))
                return true;
        }

        // Check service name patterns
        var name = service.ServiceName.ToLowerInvariant();
        if (name.StartsWith("microsoft") || name.StartsWith("windows") || name.StartsWith("win"))
            return true;

        return false;
    }

    private void EnrichWithFileMetadata(StartupEntry entry, string filePath, bool isMicrosoftService)
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

            // Update Microsoft detection based on file info
            if (versionInfo.CompanyName?.Contains("Microsoft", StringComparison.OrdinalIgnoreCase) == true)
            {
                entry.IsProtected = true;
                entry.ProtectionReason = "Service Microsoft";
                entry.RiskLevel = RiskLevel.Safe;
            }
            else if (isMicrosoftService)
            {
                entry.RiskLevel = RiskLevel.Safe;
            }
            else
            {
                // Non-Microsoft service
                entry.RiskLevel = RiskLevel.Unknown;
            }
        }
        catch (Exception)
        {
            // Ignore metadata errors
        }
    }
}
