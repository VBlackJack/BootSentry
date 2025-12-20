// Copyright (c) Julien Bombled. All rights reserved.
// Licensed under the Apache 2.0 license. See LICENSE file in the project root for full license information.

namespace BootSentry.UI.Enums;

/// <summary>
/// High-level navigation tabs for the main UI.
/// Groups entry types by user intention rather than technical category.
/// </summary>
public enum NavigationTab
{
    /// <summary>
    /// Applications tab: Startup programs (Registry Run, Startup Folder) and Scheduled Tasks.
    /// What users typically want to manage.
    /// </summary>
    Applications,

    /// <summary>
    /// Browsers tab: Browser extensions and BHOs.
    /// Browser-related startup items.
    /// </summary>
    Browsers,

    /// <summary>
    /// System tab: Services, Drivers, Print Monitors.
    /// System-level components (requires more care).
    /// </summary>
    System,

    /// <summary>
    /// Advanced tab: Winlogon, AppInit_DLLs, IFEO, Shell Extensions, Session Manager.
    /// Expert-only items that can break the system if misconfigured.
    /// Only visible in Expert Mode.
    /// </summary>
    Advanced
}
