namespace BootSentry.Core.Enums;

/// <summary>
/// Category grouping for startup entry types.
/// </summary>
public enum EntryCategory
{
    /// <summary>Classic startup locations (Registry Run, Startup Folder)</summary>
    Startup,

    /// <summary>Scheduled Tasks</summary>
    Tasks,

    /// <summary>Windows Services and Drivers</summary>
    Services,

    /// <summary>System-level hooks (IFEO, Winlogon, Session Manager, AppInit)</summary>
    System,

    /// <summary>Shell and browser extensions</summary>
    Extensions
}
