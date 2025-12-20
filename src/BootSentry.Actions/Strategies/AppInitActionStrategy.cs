// Copyright (c) Julien Bombled. All rights reserved.
// Licensed under the Apache 2.0 license. See LICENSE file in the project root for full license information.

using Microsoft.Extensions.Logging;
using Microsoft.Win32;
using BootSentry.Core.Enums;
using BootSentry.Core.Interfaces;
using BootSentry.Core.Models;

namespace BootSentry.Actions.Strategies;

/// <summary>
/// Action strategy for AppInit_DLLs registry entries.
/// Handles multi-string values by parsing and modifying the DLL list.
/// </summary>
public sealed class AppInitActionStrategy : IActionStrategy
{
    private readonly ILogger<AppInitActionStrategy> _logger;

    public AppInitActionStrategy(ILogger<AppInitActionStrategy> logger)
    {
        _logger = logger;
    }

    public EntryType EntryType => EntryType.AppInitDlls;
    public bool CanDisable => true;
    public bool CanDelete => true;

    public bool RequiresAdmin(StartupEntry entry, ActionType action)
    {
        // AppInit_DLLs is always in HKLM, requires admin
        return true;
    }

    public async Task<ActionResult> DisableAsync(StartupEntry entry, CancellationToken cancellationToken = default)
    {
        return await Task.Run(() => RemoveDllFromList(entry, isDelete: false), cancellationToken);
    }

    public async Task<ActionResult> EnableAsync(StartupEntry entry, CancellationToken cancellationToken = default)
    {
        return await Task.Run(() => AddDllToList(entry), cancellationToken);
    }

    public async Task<ActionResult> DeleteAsync(StartupEntry entry, CancellationToken cancellationToken = default)
    {
        // Delete is same as Disable for AppInit_DLLs (remove from list)
        return await Task.Run(() => RemoveDllFromList(entry, isDelete: true), cancellationToken);
    }

    private ActionResult RemoveDllFromList(StartupEntry entry, bool isDelete)
    {
        if (entry.Type != EntryType.AppInitDlls)
        {
            return ActionResult.Fail("Invalid entry type for this strategy", "ERR_INVALID_TYPE");
        }

        var dllPath = entry.CommandLineRaw;
        if (string.IsNullOrWhiteSpace(dllPath))
        {
            return ActionResult.Fail("DLL path not found in entry", "ERR_NO_DLL_PATH");
        }

        var registryPath = ParseRegistryPath(entry.SourcePath);
        if (registryPath == null)
        {
            return ActionResult.Fail("Invalid registry path", "ERR_INVALID_PATH");
        }

        try
        {
            _logger.LogInformation("Removing DLL {DllPath} from AppInit_DLLs at {Path}", dllPath, registryPath);

            using var key = Registry.LocalMachine.OpenSubKey(registryPath, writable: true);
            if (key == null)
            {
                return ActionResult.Fail("Registry key not found or access denied", "ERR_KEY_NOT_FOUND");
            }

            // Read current value
            var currentValue = key.GetValue("AppInit_DLLs")?.ToString() ?? "";

            // Parse into list
            var dlls = ParseDllList(currentValue);

            // Find and remove the DLL (case-insensitive comparison)
            var originalCount = dlls.Count;
            dlls.RemoveAll(d => string.Equals(d, dllPath, StringComparison.OrdinalIgnoreCase));

            if (dlls.Count == originalCount)
            {
                // DLL not found, might already be removed
                _logger.LogWarning("DLL {DllPath} not found in AppInit_DLLs list", dllPath);
            }

            // Write back
            var newValue = string.Join(" ", dlls);
            key.SetValue("AppInit_DLLs", newValue, RegistryValueKind.String);

            entry.Status = EntryStatus.Disabled;
            _logger.LogInformation("Successfully removed DLL from AppInit_DLLs. New value: '{Value}'", newValue);

            return ActionResult.Ok(entry);
        }
        catch (UnauthorizedAccessException ex)
        {
            _logger.LogError(ex, "Access denied modifying AppInit_DLLs");
            return ActionResult.Fail("Access denied. Administrator privileges required.", "ERR_ACCESS_DENIED");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error removing DLL from AppInit_DLLs");
            return ActionResult.Fail(ex.Message, "ERR_REMOVE_FAILED");
        }
    }

    private ActionResult AddDllToList(StartupEntry entry)
    {
        if (entry.Type != EntryType.AppInitDlls)
        {
            return ActionResult.Fail("Invalid entry type for this strategy", "ERR_INVALID_TYPE");
        }

        var dllPath = entry.CommandLineRaw;
        if (string.IsNullOrWhiteSpace(dllPath))
        {
            return ActionResult.Fail("DLL path not found in entry", "ERR_NO_DLL_PATH");
        }

        var registryPath = ParseRegistryPath(entry.SourcePath);
        if (registryPath == null)
        {
            return ActionResult.Fail("Invalid registry path", "ERR_INVALID_PATH");
        }

        try
        {
            _logger.LogInformation("Adding DLL {DllPath} to AppInit_DLLs at {Path}", dllPath, registryPath);

            using var key = Registry.LocalMachine.OpenSubKey(registryPath, writable: true);
            if (key == null)
            {
                return ActionResult.Fail("Registry key not found or access denied", "ERR_KEY_NOT_FOUND");
            }

            // Read current value
            var currentValue = key.GetValue("AppInit_DLLs")?.ToString() ?? "";

            // Parse into list
            var dlls = ParseDllList(currentValue);

            // Check if already present
            if (dlls.Any(d => string.Equals(d, dllPath, StringComparison.OrdinalIgnoreCase)))
            {
                _logger.LogInformation("DLL {DllPath} already in AppInit_DLLs list", dllPath);
                entry.Status = EntryStatus.Enabled;
                return ActionResult.Ok(entry);
            }

            // Add the DLL
            dlls.Add(dllPath);

            // Write back
            var newValue = string.Join(" ", dlls);
            key.SetValue("AppInit_DLLs", newValue, RegistryValueKind.String);

            entry.Status = EntryStatus.Enabled;
            _logger.LogInformation("Successfully added DLL to AppInit_DLLs. New value: '{Value}'", newValue);

            return ActionResult.Ok(entry);
        }
        catch (UnauthorizedAccessException ex)
        {
            _logger.LogError(ex, "Access denied modifying AppInit_DLLs");
            return ActionResult.Fail("Access denied. Administrator privileges required.", "ERR_ACCESS_DENIED");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding DLL to AppInit_DLLs");
            return ActionResult.Fail(ex.Message, "ERR_ADD_FAILED");
        }
    }

    /// <summary>
    /// Parses the registry path from SourcePath (e.g., "HKLM\SOFTWARE\..." -> "SOFTWARE\...").
    /// </summary>
    private static string? ParseRegistryPath(string? sourcePath)
    {
        if (string.IsNullOrWhiteSpace(sourcePath))
            return null;

        // Remove HKLM\ prefix if present
        const string hklmPrefix = @"HKLM\";
        if (sourcePath.StartsWith(hklmPrefix, StringComparison.OrdinalIgnoreCase))
        {
            return sourcePath.Substring(hklmPrefix.Length);
        }

        return sourcePath;
    }

    /// <summary>
    /// Parses the AppInit_DLLs value into a list of DLL paths.
    /// Handles space, comma, and semicolon separators.
    /// </summary>
    private static List<string> ParseDllList(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return new List<string>();

        return value
            .Split(new[] { ' ', ',', ';' }, StringSplitOptions.RemoveEmptyEntries)
            .Select(d => d.Trim())
            .Where(d => !string.IsNullOrEmpty(d))
            .ToList();
    }
}
