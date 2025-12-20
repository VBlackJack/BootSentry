// Copyright (c) Julien Bombled. All rights reserved.
// Licensed under the Apache 2.0 license. See LICENSE file in the project root for full license information.

namespace BootSentry.Core.Interfaces;

/// <summary>
/// Interface for managing browser extensions via Windows Registry Policies.
/// </summary>
public interface IBrowserPolicyManager
{
    /// <summary>
    /// Toggles the state of a browser extension by adding/removing it from the policy blocklist.
    /// </summary>
    /// <param name="extensionId">The unique identifier of the extension.</param>
    /// <param name="browserName">The browser name (Chrome, Edge, Firefox).</param>
    /// <param name="shouldEnable">True to enable (remove from blocklist), false to disable (add to blocklist).</param>
    void ToggleExtensionState(string extensionId, string browserName, bool shouldEnable);

    /// <summary>
    /// Checks if an extension is currently blocked by policy.
    /// </summary>
    /// <param name="extensionId">The unique identifier of the extension.</param>
    /// <param name="browserName">The browser name (Chrome, Edge, Firefox).</param>
    /// <returns>True if the extension is blocked, false otherwise.</returns>
    bool IsExtensionBlocked(string extensionId, string browserName);
}
