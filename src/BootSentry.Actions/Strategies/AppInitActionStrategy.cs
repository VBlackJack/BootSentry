// Copyright (c) Julien Bombled. All rights reserved.
// Licensed under the Apache 2.0 license. See LICENSE file in the project root for full license information.

using Microsoft.Extensions.Logging;
using Microsoft.Win32;
using BootSentry.Core.Enums;
using BootSentry.Core.Interfaces;
using BootSentry.Core.Models;
using BootSentry.Core.Parsing;

namespace BootSentry.Actions.Strategies;

/// <summary>
/// Action strategy for AppInit_DLLs registry entries.
/// Handles multi-string values by parsing and modifying the DLL list.
/// </summary>
public sealed class AppInitActionStrategy : IActionStrategy
{
    private readonly ILogger<AppInitActionStrategy> _logger;
    private readonly ITransactionManager _transactionManager;

    public AppInitActionStrategy(ILogger<AppInitActionStrategy> logger, ITransactionManager transactionManager)
    {
        _logger = logger;
        _transactionManager = transactionManager;
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
        return await RemoveDllFromListAsync(entry, ActionType.Disable, cancellationToken).ConfigureAwait(false);
    }

    public async Task<ActionResult> EnableAsync(StartupEntry entry, CancellationToken cancellationToken = default)
    {
        return await AddDllToListAsync(entry, cancellationToken).ConfigureAwait(false);
    }

    public async Task<ActionResult> DeleteAsync(StartupEntry entry, CancellationToken cancellationToken = default)
    {
        // Delete is same as Disable for AppInit_DLLs (remove from list)
        return await RemoveDllFromListAsync(entry, ActionType.Delete, cancellationToken).ConfigureAwait(false);
    }

    private async Task<ActionResult> RemoveDllFromListAsync(StartupEntry entry, ActionType actionType, CancellationToken cancellationToken)
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

            // Pre-check key existence before creating transaction to avoid pending manifests on early failures.
            using (var preCheckKey = Registry.LocalMachine.OpenSubKey(registryPath, writable: false))
            {
                if (preCheckKey == null)
                {
                    return ActionResult.Fail("Registry key not found or access denied", "ERR_KEY_NOT_FOUND");
                }
            }

            // Create backup transaction BEFORE making changes
            var transaction = await _transactionManager.CreateTransactionAsync(entry, actionType, cancellationToken).ConfigureAwait(false);

            try
            {
                using var key = Registry.LocalMachine.OpenSubKey(registryPath, writable: true);
                if (key == null)
                    throw new InvalidOperationException("Registry key not found or access denied");

                // Read current value
                var currentValue = key.GetValue("AppInit_DLLs")?.ToString() ?? "";

                // Parse into list
                var dlls = ParseDllList(currentValue);

                // Find and remove the DLL (case-insensitive and quote-tolerant comparison)
                var originalCount = dlls.Count;
                dlls.RemoveAll(d => AppInitDllParser.AreEquivalent(d, dllPath));

                if (dlls.Count == originalCount)
                {
                    _logger.LogWarning("DLL {DllPath} not found in AppInit_DLLs list", dllPath);
                }

                // Write back safely (quote paths with spaces)
                var newValue = AppInitDllParser.Serialize(dlls);
                key.SetValue("AppInit_DLLs", newValue, RegistryValueKind.String);

                // Commit transaction
                await _transactionManager.CommitAsync(transaction.Id, cancellationToken).ConfigureAwait(false);

                entry.Status = EntryStatus.Disabled;
                _logger.LogInformation("Successfully removed DLL from AppInit_DLLs. New value: '{Value}'", newValue);

                return ActionResult.Ok(entry, transaction.Id);
            }
            catch
            {
                await _transactionManager.RollbackAsync(transaction.Id, cancellationToken).ConfigureAwait(false);
                throw;
            }
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

    private async Task<ActionResult> AddDllToListAsync(StartupEntry entry, CancellationToken cancellationToken)
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

            // Pre-check key existence before creating transaction to avoid pending manifests on early failures.
            using (var preCheckKey = Registry.LocalMachine.OpenSubKey(registryPath, writable: false))
            {
                if (preCheckKey == null)
                {
                    return ActionResult.Fail("Registry key not found or access denied", "ERR_KEY_NOT_FOUND");
                }
            }

            // Create backup transaction BEFORE making changes
            var transaction = await _transactionManager.CreateTransactionAsync(entry, ActionType.Enable, cancellationToken).ConfigureAwait(false);

            try
            {
                using var key = Registry.LocalMachine.OpenSubKey(registryPath, writable: true);
                if (key == null)
                    throw new InvalidOperationException("Registry key not found or access denied");

                // Read current value
                var currentValue = key.GetValue("AppInit_DLLs")?.ToString() ?? "";

                // Parse into list
                var dlls = ParseDllList(currentValue);

                // Check if already present
                if (dlls.Any(d => AppInitDllParser.AreEquivalent(d, dllPath)))
                {
                    _logger.LogInformation("DLL {DllPath} already in AppInit_DLLs list", dllPath);
                    await _transactionManager.CommitAsync(transaction.Id, cancellationToken).ConfigureAwait(false);
                    entry.Status = EntryStatus.Enabled;
                    return ActionResult.Ok(entry, transaction.Id);
                }

                // Add the DLL
                dlls.Add(dllPath);

                // Write back safely (quote paths with spaces)
                var newValue = AppInitDllParser.Serialize(dlls);
                key.SetValue("AppInit_DLLs", newValue, RegistryValueKind.String);

                // Commit transaction
                await _transactionManager.CommitAsync(transaction.Id, cancellationToken).ConfigureAwait(false);

                entry.Status = EntryStatus.Enabled;
                _logger.LogInformation("Successfully added DLL to AppInit_DLLs. New value: '{Value}'", newValue);

                return ActionResult.Ok(entry, transaction.Id);
            }
            catch
            {
                await _transactionManager.RollbackAsync(transaction.Id, cancellationToken).ConfigureAwait(false);
                throw;
            }
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
    internal static string? ParseRegistryPath(string? sourcePath)
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
    internal static List<string> ParseDllList(string? value)
    {
        return AppInitDllParser.Parse(value);
    }

    internal static string SerializeDllList(IEnumerable<string> values)
    {
        return AppInitDllParser.Serialize(values);
    }
}
