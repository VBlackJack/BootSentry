using Microsoft.Extensions.Logging;
using Microsoft.Win32;
using BootSentry.Core.Enums;
using BootSentry.Core.Interfaces;
using BootSentry.Core.Models;

namespace BootSentry.Actions.Strategies;

/// <summary>
/// Action strategy for Registry Run/RunOnce entries.
/// Disables by moving values to a BootSentry backup key.
/// </summary>
public sealed class RegistryActionStrategy : IActionStrategy
{
    private readonly ILogger<RegistryActionStrategy> _logger;
    private readonly ITransactionManager _transactionManager;

    private const string DisabledKeyPath = @"Software\BootSentry\Disabled";

    public RegistryActionStrategy(ILogger<RegistryActionStrategy> logger, ITransactionManager transactionManager)
    {
        _logger = logger;
        _transactionManager = transactionManager;
    }

    public EntryType EntryType => EntryType.RegistryRun;
    public bool CanDisable => true;
    public bool CanDelete => true;

    public bool RequiresAdmin(StartupEntry entry, ActionType action)
    {
        return entry.Scope == EntryScope.Machine;
    }

    public async Task<ActionResult> DisableAsync(StartupEntry entry, CancellationToken cancellationToken = default)
    {
        if (entry.Type is not (EntryType.RegistryRun or EntryType.RegistryRunOnce))
        {
            return ActionResult.Fail("Invalid entry type for this strategy", "ERR_INVALID_TYPE");
        }

        try
        {
            if (entry.SourceName == null)
            {
                return ActionResult.Fail("Entry has no source name", "ERR_NO_SOURCE_NAME");
            }

            // Parse the source path to get hive and key
            var (hive, keyPath) = ParseRegistryPath(entry.SourcePath);

            // Pre-check key/value before creating transaction to avoid pending manifests on early failures.
            object value;
            RegistryValueKind valueKind;
            using (var precheckBaseKey = RegistryKey.OpenBaseKey(hive, RegistryView.Default))
            using (var precheckSourceKey = precheckBaseKey.OpenSubKey(keyPath, writable: false))
            {
                if (precheckSourceKey == null)
                {
                    return ActionResult.Fail($"Registry key not found: {entry.SourcePath}", "ERR_KEY_NOT_FOUND");
                }

                value = precheckSourceKey.GetValue(entry.SourceName)!;
                if (value == null)
                {
                    return ActionResult.Fail($"Registry value not found: {entry.SourceName}", "ERR_VALUE_NOT_FOUND");
                }

                valueKind = precheckSourceKey.GetValueKind(entry.SourceName);
            }

            // Create backup transaction
            var transaction = await _transactionManager.CreateTransactionAsync(entry, ActionType.Disable, cancellationToken);

            try
            {
                using var baseKey = RegistryKey.OpenBaseKey(hive, RegistryView.Default);
                using var sourceKey = baseKey.OpenSubKey(keyPath, writable: true);

                if (sourceKey == null)
                    throw new InvalidOperationException($"Registry key not found: {entry.SourcePath}");

                // Create disabled key path (mirror structure)
                var disabledKeyPath = $"{DisabledKeyPath}\\{keyPath}";
                using var disabledBaseKey = RegistryKey.OpenBaseKey(hive, RegistryView.Default);
                using var disabledKey = disabledBaseKey.CreateSubKey(disabledKeyPath, writable: true);

                if (disabledKey == null)
                    throw new InvalidOperationException("Failed to create disabled key");

                // Move value to disabled key
                disabledKey.SetValue(entry.SourceName, value, valueKind);

                // Delete from original key
                sourceKey.DeleteValue(entry.SourceName, throwOnMissingValue: false);

                // Commit transaction
                await _transactionManager.CommitAsync(transaction.Id, cancellationToken);

                _logger.LogInformation("Disabled registry entry: {Name}", entry.DisplayName);

                // Update entry status
                entry.Status = EntryStatus.Disabled;
                return ActionResult.Ok(entry, transaction.Id);
            }
            catch
            {
                await _transactionManager.RollbackAsync(transaction.Id, cancellationToken);
                throw;
            }
        }
        catch (UnauthorizedAccessException ex)
        {
            _logger.LogError(ex, "Access denied disabling registry entry");
            return ActionResult.Fail("Access denied. Administrator privileges required.", "ERR_ACCESS_DENIED");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error disabling registry entry");
            return ActionResult.Fail(ex.Message, "ERR_DISABLE_FAILED");
        }
    }

    public async Task<ActionResult> EnableAsync(StartupEntry entry, CancellationToken cancellationToken = default)
    {
        if (entry.Type is not (EntryType.RegistryRun or EntryType.RegistryRunOnce))
        {
            return ActionResult.Fail("Invalid entry type for this strategy", "ERR_INVALID_TYPE");
        }

        try
        {
            // Parse paths
            var (hive, keyPath) = ParseRegistryPath(entry.SourcePath);
            var disabledKeyPath = $"{DisabledKeyPath}\\{keyPath}";

            // Read from disabled key
            using var baseKey = RegistryKey.OpenBaseKey(hive, RegistryView.Default);
            using var disabledKey = baseKey.OpenSubKey(disabledKeyPath, writable: true);

            if (disabledKey == null || entry.SourceName == null)
            {
                return ActionResult.Fail("Disabled entry not found", "ERR_NOT_FOUND");
            }

            var value = disabledKey.GetValue(entry.SourceName);
            if (value == null)
            {
                return ActionResult.Fail($"Disabled value not found: {entry.SourceName}", "ERR_VALUE_NOT_FOUND");
            }

            var valueKind = disabledKey.GetValueKind(entry.SourceName);

            // Write to original key
            using var sourceKey = baseKey.OpenSubKey(keyPath, writable: true)
                ?? baseKey.CreateSubKey(keyPath, writable: true);

            if (sourceKey == null)
            {
                return ActionResult.Fail("Failed to open/create source key", "ERR_CREATE_KEY_FAILED");
            }

            sourceKey.SetValue(entry.SourceName, value, valueKind);

            // Delete from disabled key
            disabledKey.DeleteValue(entry.SourceName);

            _logger.LogInformation("Enabled registry entry: {Name}", entry.DisplayName);

            // Update entry status
            entry.Status = EntryStatus.Enabled;
            return ActionResult.Ok(entry);
        }
        catch (UnauthorizedAccessException ex)
        {
            _logger.LogError(ex, "Access denied enabling registry entry");
            return ActionResult.Fail("Access denied. Administrator privileges required.", "ERR_ACCESS_DENIED");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error enabling registry entry");
            return ActionResult.Fail(ex.Message, "ERR_ENABLE_FAILED");
        }
    }

    public async Task<ActionResult> DeleteAsync(StartupEntry entry, CancellationToken cancellationToken = default)
    {
        if (entry.Type is not (EntryType.RegistryRun or EntryType.RegistryRunOnce))
        {
            return ActionResult.Fail("Invalid entry type for this strategy", "ERR_INVALID_TYPE");
        }

        try
        {
            if (entry.SourceName == null)
            {
                return ActionResult.Fail("Entry has no source name", "ERR_NO_SOURCE_NAME");
            }

            var sourceName = entry.SourceName;

            // Pre-check key/value before creating transaction to avoid pending manifests on early failures.
            var (hive, keyPath) = ParseRegistryPath(entry.SourcePath);
            using (var precheckBaseKey = RegistryKey.OpenBaseKey(hive, RegistryView.Default))
            using (var precheckKey = precheckBaseKey.OpenSubKey(keyPath, writable: false))
            {
                if (precheckKey == null || precheckKey.GetValue(sourceName) == null)
                {
                    return ActionResult.Fail("Registry key/value not found", "ERR_NOT_FOUND");
                }
            }

            // Create backup transaction first
            var transaction = await _transactionManager.CreateTransactionAsync(entry, ActionType.Delete, cancellationToken);

            try
            {
                using var baseKey = RegistryKey.OpenBaseKey(hive, RegistryView.Default);
                using var key = baseKey.OpenSubKey(keyPath, writable: true);

                if (key == null)
                    throw new InvalidOperationException("Registry key not found");

                // Delete the value
                key.DeleteValue(sourceName, throwOnMissingValue: false);

                // Commit transaction
                await _transactionManager.CommitAsync(transaction.Id, cancellationToken);

                _logger.LogInformation("Deleted registry entry: {Name}", entry.DisplayName);
                return ActionResult.Ok(transactionId: transaction.Id);
            }
            catch
            {
                await _transactionManager.RollbackAsync(transaction.Id, cancellationToken);
                throw;
            }
        }
        catch (UnauthorizedAccessException ex)
        {
            _logger.LogError(ex, "Access denied deleting registry entry");
            return ActionResult.Fail("Access denied. Administrator privileges required.", "ERR_ACCESS_DENIED");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting registry entry");
            return ActionResult.Fail(ex.Message, "ERR_DELETE_FAILED");
        }
    }

    private static (RegistryHive Hive, string Path) ParseRegistryPath(string fullPath)
    {
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
