// Copyright (c) Julien Bombled. All rights reserved.
// Licensed under the Apache 2.0 license. See LICENSE file in the project root for full license information.

using BootSentry.Core.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Win32;

namespace BootSentry.Actions.Services;

/// <summary>
/// Manages browser extensions via Windows Registry Policies.
/// Uses the ExtensionInstallBlocklist policy for Chromium browsers and Extensions policy for Firefox.
/// </summary>
public sealed class BrowserPolicyManager : IBrowserPolicyManager
{
    private readonly ILogger<BrowserPolicyManager> _logger;
    // Chromium-based browsers blocklist paths
    private const string ChromeBlocklistPath = @"SOFTWARE\Policies\Google\Chrome\ExtensionInstallBlocklist";
    private const string EdgeBlocklistPath = @"SOFTWARE\Policies\Microsoft\Edge\ExtensionInstallBlocklist";
    private const string BraveBlocklistPath = @"SOFTWARE\Policies\BraveSoftware\Brave\ExtensionInstallBlocklist";
    private const string OperaBlocklistPath = @"SOFTWARE\Policies\Opera Software\Opera\ExtensionInstallBlocklist";
    private const string VivaldiBlocklistPath = @"SOFTWARE\Policies\Vivaldi\ExtensionInstallBlocklist";

    // Firefox extensions policy path
    private const string FirefoxExtensionsPath = @"SOFTWARE\Policies\Mozilla\Firefox\Extensions";

    public BrowserPolicyManager(ILogger<BrowserPolicyManager> logger)
    {
        _logger = logger;
    }

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

                case "brave":
                    ToggleChromiumExtension(extensionId, BraveBlocklistPath, shouldEnable);
                    break;

                case "opera":
                case "opera gx":
                    ToggleChromiumExtension(extensionId, OperaBlocklistPath, shouldEnable);
                    break;

                case "vivaldi":
                    ToggleChromiumExtension(extensionId, VivaldiBlocklistPath, shouldEnable);
                    break;

                case "firefox":
                case "mozilla firefox":
                    ToggleFirefoxExtension(extensionId, shouldEnable);
                    break;

                default:
                    _logger.LogWarning("Unsupported browser: {BrowserName}", browserName);
                    break;
            }
        }
        catch (UnauthorizedAccessException ex)
        {
            _logger.LogError(ex, "Access denied when modifying registry for {BrowserName}", browserName);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error toggling extension state for {BrowserName}", browserName);
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
                "brave" => IsChromiumExtensionBlocked(extensionId, BraveBlocklistPath),
                "opera" or "opera gx" => IsChromiumExtensionBlocked(extensionId, OperaBlocklistPath),
                "vivaldi" => IsChromiumExtensionBlocked(extensionId, VivaldiBlocklistPath),
                "firefox" or "mozilla firefox" => IsFirefoxExtensionBlocked(extensionId),
                _ => false
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking extension block status");
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
            _logger.LogWarning("Failed to create registry key: {RegistryPath}", registryPath);
            return;
        }

        // Check if already blocked
        var valueNames = key.GetValueNames();
        foreach (var name in valueNames)
        {
            var value = key.GetValue(name) as string;
            if (string.Equals(value, extensionId, StringComparison.OrdinalIgnoreCase))
            {
                _logger.LogInformation("Extension {ExtensionId} is already in the blocklist", extensionId);
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
        _logger.LogInformation("Added extension {ExtensionId} to blocklist at index {Index}", extensionId, nextIndex);
    }

    /// <summary>
    /// Removes an extension ID from the Chromium blocklist.
    /// </summary>
    private void RemoveFromChromiumBlocklist(string extensionId, string registryPath)
    {
        using var key = Registry.LocalMachine.OpenSubKey(registryPath, writable: true);
        if (key == null)
        {
            _logger.LogWarning("Registry key does not exist: {RegistryPath}", registryPath);
            return;
        }

        var valueNames = key.GetValueNames();
        foreach (var name in valueNames)
        {
            var value = key.GetValue(name) as string;
            if (string.Equals(value, extensionId, StringComparison.OrdinalIgnoreCase))
            {
                key.DeleteValue(name);
                _logger.LogInformation("Removed extension {ExtensionId} from blocklist (was at index {Index})", extensionId, name);
                return;
            }
        }

        _logger.LogInformation("Extension {ExtensionId} was not found in the blocklist", extensionId);
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
            _logger.LogWarning("Failed to create registry key: {RegistryPath}", FirefoxExtensionsPath);
            return;
        }

        // Set the extension ID as name with value "0" to block
        key.SetValue(extensionId, "0", RegistryValueKind.String);
        _logger.LogInformation("Added Firefox extension policy to block {ExtensionId}", extensionId);
    }

    /// <summary>
    /// Removes a Firefox extension from the block policy.
    /// </summary>
    private void RemoveFirefoxExtensionPolicy(string extensionId)
    {
        using var key = Registry.LocalMachine.OpenSubKey(FirefoxExtensionsPath, writable: true);
        if (key == null)
        {
            _logger.LogWarning("Registry key does not exist: {RegistryPath}", FirefoxExtensionsPath);
            return;
        }

        try
        {
            key.DeleteValue(extensionId);
            _logger.LogInformation("Removed Firefox extension policy for {ExtensionId}", extensionId);
        }
        catch (ArgumentException)
        {
            _logger.LogInformation("Firefox extension {ExtensionId} was not found in policy", extensionId);
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
