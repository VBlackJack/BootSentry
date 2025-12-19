namespace BootSentry.Core.Enums;

/// <summary>
/// Result of a malware scan operation.
/// </summary>
public enum ScanResult
{
    /// <summary>File is clean, no malware detected.</summary>
    Clean,

    /// <summary>Malware was detected in the file.</summary>
    Malware,

    /// <summary>File access was blocked by security software.</summary>
    Blocked,

    /// <summary>An error occurred during scanning.</summary>
    Error,

    /// <summary>File was not scanned (inaccessible, locked, etc.).</summary>
    NotScanned,

    /// <summary>File is too large to scan.</summary>
    TooLarge,

    /// <summary>No antivirus provider available (Windows Defender disabled, no compatible AV).</summary>
    NoAntivirusProvider
}
