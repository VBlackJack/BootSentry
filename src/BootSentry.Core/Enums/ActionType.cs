namespace BootSentry.Core.Enums;

/// <summary>
/// Type of action performed on a startup entry.
/// </summary>
public enum ActionType
{
    /// <summary>Disable the entry (reversible)</summary>
    Disable,

    /// <summary>Re-enable a previously disabled entry</summary>
    Enable,

    /// <summary>Permanently delete the entry</summary>
    Delete,

    /// <summary>Restore from backup</summary>
    Restore
}
