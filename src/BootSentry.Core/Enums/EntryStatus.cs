namespace BootSentry.Core.Enums;

/// <summary>
/// Current status of the startup entry.
/// </summary>
public enum EntryStatus
{
    /// <summary>Entry is active and will run at startup</summary>
    Enabled,

    /// <summary>Entry has been disabled (by BootSentry or other means)</summary>
    Disabled,

    /// <summary>Status could not be determined</summary>
    Unknown
}
