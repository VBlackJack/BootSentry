// Copyright (c) Julien Bombled. All rights reserved.
// Licensed under the Apache 2.0 license. See LICENSE file in the project root for full license information.

using Microsoft.Extensions.Logging;
using Microsoft.Win32;
using BootSentry.Core;
using BootSentry.Core.Enums;
using BootSentry.Core.Interfaces;
using BootSentry.Core.Models;

namespace BootSentry.Actions.Strategies;

/// <summary>
/// Action strategy for Winlogon registry entries.
/// Handles critical values (Shell, Userinit) safely by resetting to defaults instead of deleting.
/// </summary>
public sealed class WinlogonActionStrategy : IActionStrategy
{
    private readonly ILogger<WinlogonActionStrategy> _logger;
    private readonly ITransactionManager _transactionManager;

    /// <summary>
    /// Default values for critical Winlogon entries.
    /// Deleting these values would cause Windows to fail to boot properly.
    /// </summary>
    private static Dictionary<string, string>? _defaultValues;
    internal static Dictionary<string, string> DefaultValues =>
        _defaultValues ??= new(StringComparer.OrdinalIgnoreCase)
        {
            ["Shell"] = Constants.Paths.DefaultShell,
            ["Userinit"] = Constants.Paths.DefaultUserInitPath, // Trailing comma is included in the constant
        };

    /// <summary>
    /// Checks if a value name is critical and should be reset instead of deleted.
    /// </summary>
    internal static bool IsCriticalValue(string? valueName)
    {
        return !string.IsNullOrEmpty(valueName) && DefaultValues.ContainsKey(valueName);
    }

    /// <summary>
    /// Gets the default value for a critical Winlogon entry, or null if not critical.
    /// </summary>
    internal static string? GetDefaultValue(string? valueName)
    {
        if (string.IsNullOrEmpty(valueName))
            return null;
        return DefaultValues.TryGetValue(valueName, out var value) ? value : null;
    }

    public WinlogonActionStrategy(ILogger<WinlogonActionStrategy> logger, ITransactionManager transactionManager)
    {
        _logger = logger;
        _transactionManager = transactionManager;
    }

    public EntryType EntryType => EntryType.Winlogon;
    public bool CanDisable => false; // Cannot simply disable Shell/Userinit
    public bool CanDelete => true;   // Delete = Reset to default for critical values

    public bool RequiresAdmin(StartupEntry entry, ActionType action)
    {
        // Winlogon is always in HKLM, requires admin
        return true;
    }

    public Task<ActionResult> DisableAsync(StartupEntry entry, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(ActionResult.Fail(
            "Disable is not supported for Winlogon entries. Use Delete to reset to Windows defaults.",
            "ERR_NOT_SUPPORTED"));
    }

    public Task<ActionResult> EnableAsync(StartupEntry entry, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(ActionResult.Fail(
            "Enable is not supported for Winlogon entries.",
            "ERR_NOT_SUPPORTED"));
    }

    public async Task<ActionResult> DeleteAsync(StartupEntry entry, CancellationToken cancellationToken = default)
    {
        if (entry.Type != EntryType.Winlogon)
        {
            return ActionResult.Fail("Invalid entry type for this strategy", "ERR_INVALID_TYPE");
        }

        if (string.IsNullOrWhiteSpace(entry.SourceName))
        {
            return ActionResult.Fail("Entry has no value name", "ERR_NO_SOURCE_NAME");
        }

        try
        {
            var (hive, keyPath) = ParseRegistryPath(entry.SourcePath);

            // Pre-check key existence before creating transaction to avoid pending manifests.
            using (var precheckBaseKey = RegistryKey.OpenBaseKey(hive, RegistryView.Default))
            using (var precheckKey = precheckBaseKey.OpenSubKey(keyPath, writable: false))
            {
                if (precheckKey == null)
                {
                    return ActionResult.Fail($"Registry key not found: {entry.SourcePath}", "ERR_KEY_NOT_FOUND");
                }
            }

            // Create backup transaction first
            var transaction = await _transactionManager.CreateTransactionAsync(entry, ActionType.Delete, cancellationToken).ConfigureAwait(false);

            try
            {
                using var baseKey = RegistryKey.OpenBaseKey(hive, RegistryView.Default);
                using var key = baseKey.OpenSubKey(keyPath, writable: true);

                if (key == null)
                {
                    throw new InvalidOperationException($"Registry key not found: {entry.SourcePath}");
                }

                // Check if this is a critical value that needs reset instead of delete
                if (DefaultValues.TryGetValue(entry.SourceName, out var defaultValue))
                {
                    // Reset to Windows default instead of deleting
                    _logger.LogInformation(
                        "Resetting critical Winlogon value '{ValueName}' to default: {DefaultValue}",
                        entry.SourceName, defaultValue);

                    key.SetValue(entry.SourceName, defaultValue, RegistryValueKind.String);

                    await _transactionManager.CommitAsync(transaction.Id, cancellationToken).ConfigureAwait(false);

                    _logger.LogInformation("Successfully reset Winlogon entry: {Name}", entry.DisplayName);

                    return ActionResult.Ok(transactionId: transaction.Id);
                }
                else
                {
                    // For non-critical values (GinaDLL, AppSetup, etc.), actually delete
                    _logger.LogInformation("Deleting Winlogon value: {ValueName}", entry.SourceName);

                    key.DeleteValue(entry.SourceName, throwOnMissingValue: false);

                    await _transactionManager.CommitAsync(transaction.Id, cancellationToken).ConfigureAwait(false);

                    _logger.LogInformation("Successfully deleted Winlogon entry: {Name}", entry.DisplayName);

                    return ActionResult.Ok(transactionId: transaction.Id);
                }
            }
            catch
            {
                await _transactionManager.RollbackAsync(transaction.Id, cancellationToken).ConfigureAwait(false);
                throw;
            }
        }
        catch (UnauthorizedAccessException ex)
        {
            _logger.LogError(ex, "Access denied modifying Winlogon entry");
            return ActionResult.Fail("Access denied. Administrator privileges required.", "ERR_ACCESS_DENIED");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error modifying Winlogon entry");
            return ActionResult.Fail(ex.Message, "ERR_DELETE_FAILED");
        }
    }

    /// <summary>
    /// Parses the registry path to extract hive and key path.
    /// </summary>
    private static (RegistryHive Hive, string Path) ParseRegistryPath(string? fullPath)
    {
        if (string.IsNullOrWhiteSpace(fullPath))
        {
            throw new ArgumentException("Registry path is empty");
        }

        var parts = fullPath.Split('\\', 2);
        var hiveName = parts[0].ToUpperInvariant();
        var path = parts.Length > 1 ? parts[1] : string.Empty;

        var hive = hiveName switch
        {
            "HKCU" or "HKEY_CURRENT_USER" => RegistryHive.CurrentUser,
            "HKLM" or "HKEY_LOCAL_MACHINE" => RegistryHive.LocalMachine,
            _ => throw new ArgumentException($"Unknown registry hive: {hiveName}")
        };

        return (hive, path);
    }
}
