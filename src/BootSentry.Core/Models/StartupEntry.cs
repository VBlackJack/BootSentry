using System.ComponentModel;
using System.Runtime.CompilerServices;
using BootSentry.Core.Enums;

namespace BootSentry.Core.Models;

/// <summary>
/// Represents a single startup entry from any source.
/// This is the normalized model used throughout the application.
/// </summary>
public class StartupEntry : INotifyPropertyChanged
{
    /// <inheritdoc />
    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
    /// <summary>
    /// Unique, stable, deterministic identifier for this entry.
    /// Format: {Type}:{Scope}:{SourcePath}:{Name}
    /// </summary>
    public required string Id { get; init; }

    /// <summary>
    /// Type of startup source.
    /// </summary>
    public required EntryType Type { get; init; }

    /// <summary>
    /// Whether this entry is user-specific or machine-wide.
    /// </summary>
    public required EntryScope Scope { get; init; }

    /// <summary>
    /// Display name shown in the UI.
    /// </summary>
    public required string DisplayName { get; init; }

    /// <summary>
    /// Full path to the source (registry key path, folder path, task name, service name).
    /// </summary>
    public required string SourcePath { get; init; }

    /// <summary>
    /// Registry value name or filename within the source.
    /// </summary>
    public string? SourceName { get; init; }

    /// <summary>
    /// Raw command line as stored in the source.
    /// </summary>
    public string? CommandLineRaw { get; set; }

    /// <summary>
    /// Normalized command line with expanded environment variables.
    /// </summary>
    public string? CommandLineNormalized { get; set; }

    /// <summary>
    /// Resolved path to the target executable (if resolvable).
    /// </summary>
    public string? TargetPath { get; set; }

    /// <summary>
    /// Command line arguments (parsed from CommandLine).
    /// </summary>
    public string? Arguments { get; set; }

    /// <summary>
    /// Working directory (if applicable, e.g., from .lnk files).
    /// </summary>
    public string? WorkingDirectory { get; set; }

    /// <summary>
    /// Publisher/Company name from file metadata or signature.
    /// </summary>
    public string? Publisher { get; set; }

    /// <summary>
    /// Authenticode signature status.
    /// </summary>
    public SignatureStatus SignatureStatus { get; set; } = SignatureStatus.Unknown;

    /// <summary>
    /// SHA-256 hash of the target file (computed on demand).
    /// </summary>
    public string? Sha256 { get; set; }

    /// <summary>
    /// Whether the target file exists on disk.
    /// </summary>
    public bool FileExists { get; set; }

    /// <summary>
    /// Last modification date of the source or target file.
    /// </summary>
    public DateTime? LastModified { get; set; }

    /// <summary>
    /// Current status (enabled, disabled, unknown).
    /// </summary>
    public EntryStatus Status { get; set; } = EntryStatus.Unknown;

    /// <summary>
    /// Assessed risk level based on heuristics.
    /// </summary>
    public RiskLevel RiskLevel { get; set; } = RiskLevel.Unknown;

    /// <summary>
    /// Detailed risk factors contributing to the risk level.
    /// </summary>
    public IReadOnlyList<RiskFactor> RiskFactors { get; set; } = Array.Empty<RiskFactor>();

    /// <summary>
    /// Short explanation or note about this entry.
    /// </summary>
    public string? Notes { get; set; }

    /// <summary>
    /// Description of the entry (e.g., browser extension description).
    /// </summary>
    public string? Description { get; set; }

    // Optional metadata

    /// <summary>
    /// File version from FileVersionInfo.
    /// </summary>
    public string? FileVersion { get; set; }

    /// <summary>
    /// Product name from FileVersionInfo.
    /// </summary>
    public string? ProductName { get; set; }

    /// <summary>
    /// Company name from FileVersionInfo.
    /// </summary>
    public string? CompanyName { get; set; }

    /// <summary>
    /// File description from FileVersionInfo.
    /// </summary>
    public string? FileDescription { get; set; }

    /// <summary>
    /// File size in bytes.
    /// </summary>
    public long? FileSize { get; set; }

    /// <summary>
    /// Registry view (32-bit or 64-bit) for registry entries.
    /// </summary>
    public string? RegistryView { get; set; }

    /// <summary>
    /// For services: the image path.
    /// </summary>
    public string? ServiceImagePath { get; set; }

    /// <summary>
    /// For services: the start type (Auto, Manual, Disabled).
    /// </summary>
    public string? ServiceStartType { get; set; }

    /// <summary>
    /// For services: previous start type before disable (used for in-session re-enable).
    /// </summary>
    public string? PreviousServiceStartType { get; set; }

    /// <summary>
    /// For services: the account the service runs under.
    /// </summary>
    public string? ServiceAccount { get; set; }

    /// <summary>
    /// For scheduled tasks: the trigger description.
    /// </summary>
    public string? TaskTrigger { get; set; }

    /// <summary>
    /// Whether this entry is protected (should not be modified).
    /// </summary>
    public bool IsProtected { get; set; }

    /// <summary>
    /// Reason why this entry is protected.
    /// </summary>
    public string? ProtectionReason { get; set; }

    private ScanResult? _malwareScanResult;

    /// <summary>
    /// Result of the last malware scan for the target file.
    /// </summary>
    public ScanResult? MalwareScanResult
    {
        get => _malwareScanResult;
        set
        {
            if (_malwareScanResult != value)
            {
                _malwareScanResult = value;
                OnPropertyChanged();
            }
        }
    }

    private DateTime? _lastMalwareScan;

    /// <summary>
    /// Date and time of the last malware scan.
    /// </summary>
    public DateTime? LastMalwareScan
    {
        get => _lastMalwareScan;
        set
        {
            if (_lastMalwareScan != value)
            {
                _lastMalwareScan = value;
                OnPropertyChanged();
            }
        }
    }

    /// <summary>
    /// VirusTotal detection score (e.g. 5/70).
    /// </summary>
    public string? VirusTotalScore { get; set; }

    /// <summary>
    /// VirusTotal permalink.
    /// </summary>
    public string? VirusTotalLink { get; set; }

    /// <summary>
    /// Category grouping based on entry type.
    /// </summary>
    public EntryCategory Category => Type switch
    {
        EntryType.RegistryRun => EntryCategory.Startup,
        EntryType.RegistryRunOnce => EntryCategory.Startup,
        EntryType.StartupFolder => EntryCategory.Startup,
        EntryType.RegistryPolicies => EntryCategory.Startup,
        EntryType.ScheduledTask => EntryCategory.Tasks,
        EntryType.Service => EntryCategory.Services,
        EntryType.Driver => EntryCategory.Services,
        EntryType.IFEO => EntryCategory.System,
        EntryType.Winlogon => EntryCategory.System,
        EntryType.SessionManager => EntryCategory.System,
        EntryType.AppInitDlls => EntryCategory.System,
        EntryType.ShellExtension => EntryCategory.Extensions,
        EntryType.BHO => EntryCategory.Extensions,
        EntryType.PrintMonitor => EntryCategory.Extensions,
        EntryType.WinsockLSP => EntryCategory.Extensions,
        EntryType.BrowserExtension => EntryCategory.Extensions,
        _ => EntryCategory.System
    };

    /// <summary>
    /// Generates a stable ID for a startup entry.
    /// </summary>
    public static string GenerateId(EntryType type, EntryScope scope, string sourcePath, string? name)
    {
        var normalizedPath = sourcePath.ToLowerInvariant().Replace('/', '\\').TrimEnd('\\');
        var normalizedName = name?.ToLowerInvariant() ?? string.Empty;
        return $"{type}:{scope}:{normalizedPath}:{normalizedName}";
    }
}
