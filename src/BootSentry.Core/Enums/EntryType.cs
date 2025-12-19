namespace BootSentry.Core.Enums;

/// <summary>
/// Type of startup entry source.
/// </summary>
public enum EntryType
{
    /// <summary>Registry Run key (HKCU or HKLM)</summary>
    RegistryRun,

    /// <summary>Registry RunOnce key</summary>
    RegistryRunOnce,

    /// <summary>Startup folder shortcut or executable</summary>
    StartupFolder,

    /// <summary>Windows Scheduled Task</summary>
    ScheduledTask,

    /// <summary>Windows Service</summary>
    Service,

    /// <summary>Kernel or filesystem driver</summary>
    Driver,

    /// <summary>Image File Execution Options (debugger hijack)</summary>
    IFEO,

    /// <summary>Shell extension (context menu handler, etc.)</summary>
    ShellExtension,

    /// <summary>Browser Helper Object (legacy)</summary>
    BHO,

    /// <summary>Winlogon Shell/Userinit entries</summary>
    Winlogon,

    /// <summary>Registry Policies Run key</summary>
    RegistryPolicies,

    /// <summary>Print Monitor (spooler subsystem)</summary>
    PrintMonitor,

    /// <summary>Session Manager BootExecute (runs before Windows starts)</summary>
    SessionManager,

    /// <summary>AppInit_DLLs (loaded into every process)</summary>
    AppInitDlls,

    /// <summary>Winsock Layered Service Provider (LSP)</summary>
    WinsockLSP,

    /// <summary>Browser Extension (Chrome, Edge, Firefox)</summary>
    BrowserExtension
}
