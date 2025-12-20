// Copyright (c) Julien Bombled. All rights reserved.
// Licensed under the Apache 2.0 license. See LICENSE file in the project root for full license information.

using BootSentry.Core.Interfaces;
using Microsoft.Win32;

namespace BootSentry.Actions.Services;

/// <summary>
/// Manages browser extensions via Windows Registry Policies.
/// Uses the ExtensionInstallBlocklist policy for Chromium browsers and Extensions policy for Firefox.
/// </summary>
public sealed class BrowserPolicyManager : IBrowserPolicyManager
{
    // Chromium-based browsers blocklist paths
    private const string ChromeBlocklistPath = @"SOFTWARE\Policies\Google\Chrome\ExtensionInstallBlocklist";
    private const string EdgeBlocklistPath = @"SOFTWARE\Policies\Microsoft\Edge\ExtensionInstallBlocklist";

    // Firefox extensions policy path
    private const string FirefoxExtensionsPath = @"SOFTWARE\Policies\Mozilla\Firefox\Extensions";

    /// <inheritdoc />
    public void ToggleExtensionState(string extensionId, string browserName, bool shouldEnable)
    {
        if (string.IsNullOrWhiteSpace(extensionId))
            throw new ArgumentException("Extension ID cannot be null or empty.", nameof(extensionId));

        if (string.IsNullOrWhiteSpace(browserName))
            throw new ArgumentException("Browser name cannot be null or empty.", nameof(browserName));

        try
        {
            var normalizedBrowser = browserName.ToLowerInvariant();

            switch (normalizedBrowser)
            {
                case "chrome":
                case "google chrome":
                    ToggleChromiumExtension(extensionId, ChromeBlocklistPath, shouldEnable);
                    break;

                case "edge":
                case "microsoft edge":
                    ToggleChromiumExtension(extensionId, EdgeBlocklistPath, shouldEnable);
                    break;

                case "firefox":
                case "mozilla firefox":
                    ToggleFirefoxExtension(extensionId, shouldEnable);
                    break;

                default:
                    Console.WriteLine($"Unsupported browser: {browserName}");
                    break;
            }
        }
        catch (UnauthorizedAccessException ex)
        {
            Console.WriteLine($"Access denied when modifying registry for {browserName}: {ex.Message}");
            throw;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error toggling extension state for {browserName}: {ex.Message}");
            throw;
        }
    }

    /// <inheritdoc />
    public bool IsExtensionBlocked(string extensionId, string browserName)
    {
        if (string.IsNullOrWhiteSpace(extensionId) || string.IsNullOrWhiteSpace(browserName))
            return false;

        try
        {
            var normalizedBrowser = browserName.ToLowerInvariant();

            return normalizedBrowser switch
            {
                "chrome" or "google chrome" => IsChromiumExtensionBlocked(extensionId, ChromeBlocklistPath),
                "edge" or "microsoft edge" => IsChromiumExtensionBlocked(extensionId, EdgeBlocklistPath),
                "firefox" or "mozilla firefox" => IsFirefoxExtensionBlocked(extensionId),
                _ => false
            };
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error checking extension block status: {ex.Message}");
            return false;
        }
    }

    /// <summary>
    /// Toggles a Chromium-based browser extension in the blocklist.
    /// </summary>
    private void ToggleChromiumExtension(string extensionId, string registryPath, bool shouldEnable)
    {
        if (shouldEnable)
        {
            // Remove from blocklist (enable the extension)
            RemoveFromChromiumBlocklist(extensionId, registryPath);
        }
        else
        {
            // Add to blocklist (disable the extension)
            AddToChromiumBlocklist(extensionId, registryPath);
        }
    }

    /// <summary>
    /// Adds an extension ID to the Chromium blocklist.
    /// </summary>
    private void AddToChromiumBlocklist(string extensionId, string registryPath)
    {
        using var key = Registry.LocalMachine.CreateSubKey(registryPath, writable: true);
        if (key == null)
        {
            Console.WriteLine($"Failed to create registry key: {registryPath}");
            return;
        }

        // Check if already blocked
        var valueNames = key.GetValueNames();
        foreach (var name in valueNames)
        {
            var value = key.GetValue(name) as string;
            if (string.Equals(value, extensionId, StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine($"Extension {extensionId} is already in the blocklist.");
                return;
            }
        }

        // Find the next available numeric index
        var nextIndex = 1;
        while (valueNames.Contains(nextIndex.ToString()))
        {
            nextIndex++;
        }

        // Add the extension ID to the blocklist
        key.SetValue(nextIndex.ToString(), extensionId, RegistryValueKind.String);
        Console.WriteLine($"Added extension {extensionId} to blocklist at index {nextIndex}");
    }

    /// <summary>
    /// Removes an extension ID from the Chromium blocklist.
    /// </summary>
    private void RemoveFromChromiumBlocklist(string extensionId, string registryPath)
    {
        using var key = Registry.LocalMachine.OpenSubKey(registryPath, writable: true);
        if (key == null)
        {
            Console.WriteLine($"Registry key does not exist: {registryPath}");
            return;
        }

        var valueNames = key.GetValueNames();
        foreach (var name in valueNames)
        {
            var value = key.GetValue(name) as string;
            if (string.Equals(value, extensionId, StringComparison.OrdinalIgnoreCase))
            {
                key.DeleteValue(name);
                Console.WriteLine($"Removed extension {extensionId} from blocklist (was at index {name})");
                return;
            }
        }

        Console.WriteLine($"Extension {extensionId} was not found in the blocklist.");
    }

    /// <summary>
    /// Checks if a Chromium extension is in the blocklist.
    /// </summary>
    private bool IsChromiumExtensionBlocked(string extensionId, string registryPath)
    {
        using var key = Registry.LocalMachine.OpenSubKey(registryPath);
        if (key == null)
            return false;

        var valueNames = key.GetValueNames();
        foreach (var name in valueNames)
        {
            var value = key.GetValue(name) as string;
            if (string.Equals(value, extensionId, StringComparison.OrdinalIgnoreCase))
                return true;
        }

        return false;
    }

    /// <summary>
    /// Toggles a Firefox extension in the policy.
    /// </summary>
    private void ToggleFirefoxExtension(string extensionId, bool shouldEnable)
    {
        if (shouldEnable)
        {
            // Remove from policy (enable the extension)
            RemoveFirefoxExtensionPolicy(extensionId);
        }
        else
        {
            // Add to policy with value "0" (disable the extension)
            AddFirefoxExtensionPolicy(extensionId);
        }
    }

    /// <summary>
    /// Adds a Firefox extension to the block policy.
    /// </summary>
    private void AddFirefoxExtensionPolicy(string extensionId)
    {
        using var key = Registry.LocalMachine.CreateSubKey(FirefoxExtensionsPath, writable: true);
        if (key == null)
        {
            Console.WriteLine($"Failed to create registry key: {FirefoxExtensionsPath}");
            return;
        }

        // Set the extension ID as name with value "0" to block
        key.SetValue(extensionId, "0", RegistryValueKind.String);
        Console.WriteLine($"Added Firefox extension policy to block {extensionId}");
    }

    /// <summary>
    /// Removes a Firefox extension from the block policy.
    /// </summary>
    private void RemoveFirefoxExtensionPolicy(string extensionId)
    {
        using var key = Registry.LocalMachine.OpenSubKey(FirefoxExtensionsPath, writable: true);
        if (key == null)
        {
            Console.WriteLine($"Registry key does not exist: {FirefoxExtensionsPath}");
            return;
        }

        try
        {
            key.DeleteValue(extensionId);
            Console.WriteLine($"Removed Firefox extension policy for {extensionId}");
        }
        catch (ArgumentException)
        {
            Console.WriteLine($"Firefox extension {extensionId} was not found in policy.");
        }
    }

    /// <summary>
    /// Checks if a Firefox extension is blocked by policy.
    /// </summary>
    private bool IsFirefoxExtensionBlocked(string extensionId)
    {
        using var key = Registry.LocalMachine.OpenSubKey(FirefoxExtensionsPath);
        if (key == null)
            return false;

        var value = key.GetValue(extensionId);
        return value != null;
    }
}
