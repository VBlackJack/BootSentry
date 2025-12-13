namespace BootSentry.Core.Enums;

/// <summary>
/// Risk assessment level for a startup entry.
/// </summary>
public enum RiskLevel
{
    /// <summary>Entry is from a known trusted source (Microsoft, major vendors)</summary>
    Safe,

    /// <summary>Entry could not be assessed, requires manual review</summary>
    Unknown,

    /// <summary>Entry has suspicious characteristics (unsigned, unusual location, etc.)</summary>
    Suspicious,

    /// <summary>Entry is in a critical location (Winlogon, IFEO) - handle with extreme care</summary>
    Critical
}
