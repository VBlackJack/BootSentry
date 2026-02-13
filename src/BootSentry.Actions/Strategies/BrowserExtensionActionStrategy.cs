// Copyright (c) Julien Bombled. All rights reserved.
// Licensed under the Apache 2.0 license. See LICENSE file in the project root for full license information.

using Microsoft.Extensions.Logging;
using BootSentry.Core.Enums;
using BootSentry.Core.Interfaces;
using BootSentry.Core.Models;

namespace BootSentry.Actions.Strategies;

/// <summary>
/// Action strategy for Browser Extensions.
/// Uses Windows Registry Policies to block/unblock extensions.
/// </summary>
public sealed class BrowserExtensionActionStrategy : IActionStrategy
{
    private readonly ILogger<BrowserExtensionActionStrategy> _logger;
    private readonly IBrowserPolicyManager _policyManager;

    public BrowserExtensionActionStrategy(
        ILogger<BrowserExtensionActionStrategy> logger,
        IBrowserPolicyManager policyManager)
    {
        _logger = logger;
        _policyManager = policyManager;
    }

    public EntryType EntryType => EntryType.BrowserExtension;
    public bool CanDisable => true;
    public bool CanDelete => false; // Don't delete extension files to avoid corruption

    public bool RequiresAdmin(StartupEntry entry, ActionType action)
    {
        // Writing to HKLM policies always requires admin
        return true;
    }

    public async Task<ActionResult> DisableAsync(StartupEntry entry, CancellationToken cancellationToken = default)
    {
        if (entry.Type != EntryType.BrowserExtension)
        {
            return ActionResult.Fail("Invalid entry type for this strategy", "ERR_INVALID_TYPE");
        }

        var extensionId = entry.SourceName;
        if (string.IsNullOrWhiteSpace(extensionId))
        {
            return ActionResult.Fail("Extension ID not found", "ERR_NO_EXTENSION_ID");
        }

        var browserName = ExtractBrowserName(entry);
        if (string.IsNullOrWhiteSpace(browserName))
        {
            return ActionResult.Fail("Could not determine browser type", "ERR_UNKNOWN_BROWSER");
        }

        try
        {
            _logger.LogInformation("Disabling extension {ExtensionId} for {Browser}", extensionId, browserName);

            _policyManager.ToggleExtensionState(extensionId, browserName, shouldEnable: false);

            entry.Status = EntryStatus.Disabled;
            _logger.LogInformation("Successfully disabled extension {ExtensionId}", extensionId);

            return await Task.FromResult(ActionResult.Ok(entry));
        }
        catch (UnauthorizedAccessException ex)
        {
            _logger.LogError(ex, "Access denied disabling extension {ExtensionId}", extensionId);
            return ActionResult.Fail("Access denied. Administrator privileges required.", "ERR_ACCESS_DENIED");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error disabling extension {ExtensionId}", extensionId);
            return ActionResult.Fail(ex.Message, "ERR_DISABLE_FAILED");
        }
    }

    public async Task<ActionResult> EnableAsync(StartupEntry entry, CancellationToken cancellationToken = default)
    {
        if (entry.Type != EntryType.BrowserExtension)
        {
            return ActionResult.Fail("Invalid entry type for this strategy", "ERR_INVALID_TYPE");
        }

        var extensionId = entry.SourceName;
        if (string.IsNullOrWhiteSpace(extensionId))
        {
            return ActionResult.Fail("Extension ID not found", "ERR_NO_EXTENSION_ID");
        }

        var browserName = ExtractBrowserName(entry);
        if (string.IsNullOrWhiteSpace(browserName))
        {
            return ActionResult.Fail("Could not determine browser type", "ERR_UNKNOWN_BROWSER");
        }

        try
        {
            _logger.LogInformation("Enabling extension {ExtensionId} for {Browser}", extensionId, browserName);

            _policyManager.ToggleExtensionState(extensionId, browserName, shouldEnable: true);

            entry.Status = EntryStatus.Enabled;
            _logger.LogInformation("Successfully enabled extension {ExtensionId}", extensionId);

            return await Task.FromResult(ActionResult.Ok(entry));
        }
        catch (UnauthorizedAccessException ex)
        {
            _logger.LogError(ex, "Access denied enabling extension {ExtensionId}", extensionId);
            return ActionResult.Fail("Access denied. Administrator privileges required.", "ERR_ACCESS_DENIED");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error enabling extension {ExtensionId}", extensionId);
            return ActionResult.Fail(ex.Message, "ERR_ENABLE_FAILED");
        }
    }

    public Task<ActionResult> DeleteAsync(StartupEntry entry, CancellationToken cancellationToken = default)
    {
        // Deleting extension files is not supported to avoid browser profile corruption
        return Task.FromResult(ActionResult.Fail(
            "Deleting browser extensions is not supported. Please remove extensions from the browser settings.",
            "ERR_DELETE_NOT_SUPPORTED"));
    }

    /// <summary>
    /// Extracts the browser name from a StartupEntry.
    /// </summary>
    private static string? ExtractBrowserName(StartupEntry entry)
    {
        // First try to extract from DisplayName which has format "[BrowserName] ExtensionName"
        if (!string.IsNullOrWhiteSpace(entry.DisplayName))
        {
            var displayName = entry.DisplayName;
            if (displayName.StartsWith("[") && displayName.Contains("]"))
            {
                var endBracket = displayName.IndexOf(']');
                var browserName = displayName.Substring(1, endBracket - 1);
                return NormalizeBrowserName(browserName);
            }
        }

        // Fallback: try to extract from SourcePath
        if (!string.IsNullOrWhiteSpace(entry.SourcePath))
        {
            var sourcePath = entry.SourcePath.ToLowerInvariant();

            if (sourcePath.Contains("brave"))
                return "Brave";
            if (sourcePath.Contains("opera gx"))
                return "Opera GX";
            if (sourcePath.Contains("opera"))
                return "Opera";
            if (sourcePath.Contains("vivaldi"))
                return "Vivaldi";
            if (sourcePath.Contains("chrome"))
                return "Chrome";
            if (sourcePath.Contains("edge"))
                return "Edge";
            if (sourcePath.Contains("firefox") || sourcePath.Contains("mozilla"))
                return "Firefox";
        }

        return null;
    }

    /// <summary>
    /// Normalizes browser name to the supported policy names.
    /// </summary>
    private static string? NormalizeBrowserName(string browserName)
    {
        var normalized = browserName.ToLowerInvariant().Trim();

        return normalized switch
        {
            "chrome" => "Chrome",
            "edge" => "Edge",
            "firefox" => "Firefox",
            "brave" => "Brave",
            "opera" => "Opera",
            "opera gx" => "Opera GX",
            "vivaldi" => "Vivaldi",
            _ => null
        };
    }
}
