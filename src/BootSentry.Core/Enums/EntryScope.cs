namespace BootSentry.Core.Enums;

/// <summary>
/// Scope of the startup entry (user-specific or machine-wide).
/// </summary>
public enum EntryScope
{
    /// <summary>Entry applies to current user only (HKCU, user startup folder)</summary>
    User,

    /// <summary>Entry applies to all users on the machine (HKLM, common startup folder)</summary>
    Machine
}
