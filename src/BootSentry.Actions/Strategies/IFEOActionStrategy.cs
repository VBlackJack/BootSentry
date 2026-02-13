using Microsoft.Extensions.Logging;
using Microsoft.Win32;
using BootSentry.Core.Enums;
using BootSentry.Core.Interfaces;
using BootSentry.Core.Models;

namespace BootSentry.Actions.Strategies;

/// <summary>
/// Action strategy for Image File Execution Options entries.
/// </summary>
public sealed class IFEOActionStrategy : IActionStrategy
{
    private readonly ILogger<IFEOActionStrategy> _logger;
    private readonly ITransactionManager _transactionManager;

    private const string IFEOPath = @"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Image File Execution Options";
    private const string DisabledIFEOPath = @"SOFTWARE\BootSentry\Disabled\IFEO";

    public IFEOActionStrategy(
        ILogger<IFEOActionStrategy> logger,
        ITransactionManager transactionManager)
    {
        _logger = logger;
        _transactionManager = transactionManager;
    }

    public EntryType EntryType => EntryType.IFEO;
    public bool CanDisable => true;
    public bool CanDelete => true;

    public bool RequiresAdmin(StartupEntry entry, ActionType action)
    {
        // IFEO entries are in HKLM, always require admin
        return true;
    }

    public async Task<ActionResult> DisableAsync(StartupEntry entry, CancellationToken cancellationToken = default)
    {
        if (entry.Type != EntryType.IFEO)
        {
            return ActionResult.Fail("Invalid entry type for this strategy", "ERR_INVALID_TYPE");
        }

        try
        {
            // Extract the application name from the source path
            var appName = ExtractAppName(entry.SourcePath);
            if (string.IsNullOrEmpty(appName))
                return ActionResult.Fail("Unable to extract application name from source path", "ERR_INVALID_SOURCE");

            // Pre-check source key/value before creating transaction to avoid pending manifests.
            string debuggerValue;
            using (var sourceKey = Registry.LocalMachine.OpenSubKey($@"{IFEOPath}\{appName}", false))
            {
                if (sourceKey == null)
                    return ActionResult.Fail("IFEO key not found", "ERR_KEY_NOT_FOUND");

                debuggerValue = sourceKey.GetValue("Debugger") as string ?? string.Empty;
                if (string.IsNullOrWhiteSpace(debuggerValue))
                    return ActionResult.Fail("Debugger value not found", "ERR_VALUE_NOT_FOUND");
            }

            // Start transaction
            var transaction = await _transactionManager.CreateTransactionAsync(entry, ActionType.Disable, cancellationToken);

            try
            {
                // Create disabled key
                using var disabledRoot = Registry.LocalMachine.CreateSubKey(DisabledIFEOPath);
                using var disabledKey = disabledRoot.CreateSubKey(appName);
                disabledKey.SetValue("Debugger", debuggerValue);
                disabledKey.SetValue("_OriginalPath", entry.SourcePath);
                disabledKey.SetValue("_DisabledTime", DateTime.UtcNow.ToString("O"));

                // Delete from source
                using var writeKey = Registry.LocalMachine.OpenSubKey($@"{IFEOPath}\{appName}", true);
                writeKey?.DeleteValue("Debugger", false);

                // If no other values remain, delete the key
                if (writeKey?.ValueCount == 0)
                {
                    Registry.LocalMachine.DeleteSubKey($@"{IFEOPath}\{appName}", false);
                }

                entry.Status = EntryStatus.Disabled;
                await _transactionManager.CommitAsync(transaction.Id, cancellationToken);

                _logger.LogInformation("Disabled IFEO entry for {App}", appName);
                return ActionResult.Ok(entry, transaction.Id);
            }
            catch (Exception)
            {
                await _transactionManager.RollbackAsync(transaction.Id, cancellationToken);
                throw;
            }
        }
        catch (UnauthorizedAccessException)
        {
            return ActionResult.Fail("Access denied. Administrator privileges required.", "ERR_ACCESS_DENIED");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error disabling IFEO entry {Id}", entry.Id);
            return ActionResult.Fail(ex.Message, "ERR_DISABLE_FAILED");
        }
    }

    public async Task<ActionResult> EnableAsync(StartupEntry entry, CancellationToken cancellationToken = default)
    {
        if (entry.Type != EntryType.IFEO)
        {
            return ActionResult.Fail("Invalid entry type for this strategy", "ERR_INVALID_TYPE");
        }

        try
        {
            var appName = ExtractAppName(entry.SourcePath);
            if (string.IsNullOrEmpty(appName))
                return ActionResult.Fail("Unable to extract application name from source path", "ERR_INVALID_SOURCE");

            // Pre-check backup value before creating transaction to avoid pending manifests.
            string debuggerValue;
            using (var disabledKey = Registry.LocalMachine.OpenSubKey($@"{DisabledIFEOPath}\{appName}", false))
            {
                if (disabledKey == null)
                    return ActionResult.Fail("Backup not found", "ERR_BACKUP_NOT_FOUND");

                debuggerValue = disabledKey.GetValue("Debugger") as string ?? string.Empty;
                if (string.IsNullOrWhiteSpace(debuggerValue))
                    return ActionResult.Fail("Debugger value not found in backup", "ERR_VALUE_NOT_FOUND");
            }

            var transaction = await _transactionManager.CreateTransactionAsync(entry, ActionType.Enable, cancellationToken);

            try
            {
                // Restore to source
                using var sourceKey = Registry.LocalMachine.CreateSubKey($@"{IFEOPath}\{appName}");
                sourceKey.SetValue("Debugger", debuggerValue);

                // Remove from disabled
                Registry.LocalMachine.DeleteSubKey($@"{DisabledIFEOPath}\{appName}", false);

                entry.Status = EntryStatus.Enabled;
                await _transactionManager.CommitAsync(transaction.Id, cancellationToken);

                _logger.LogInformation("Enabled IFEO entry for {App}", appName);
                return ActionResult.Ok(entry, transaction.Id);
            }
            catch (Exception)
            {
                await _transactionManager.RollbackAsync(transaction.Id, cancellationToken);
                throw;
            }
        }
        catch (UnauthorizedAccessException)
        {
            return ActionResult.Fail("Access denied. Administrator privileges required.", "ERR_ACCESS_DENIED");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error enabling IFEO entry {Id}", entry.Id);
            return ActionResult.Fail(ex.Message, "ERR_ENABLE_FAILED");
        }
    }

    public async Task<ActionResult> DeleteAsync(StartupEntry entry, CancellationToken cancellationToken = default)
    {
        if (entry.Type != EntryType.IFEO)
        {
            return ActionResult.Fail("Invalid entry type for this strategy", "ERR_INVALID_TYPE");
        }

        try
        {
            var appName = ExtractAppName(entry.SourcePath);
            if (string.IsNullOrEmpty(appName))
                return ActionResult.Fail("Unable to extract application name from source path", "ERR_INVALID_SOURCE");

            // Pre-check source value before creating transaction to avoid pending manifests.
            using (var precheckKey = Registry.LocalMachine.OpenSubKey($@"{IFEOPath}\{appName}", false))
            {
                if (precheckKey == null)
                    return ActionResult.Fail("IFEO key not found", "ERR_KEY_NOT_FOUND");

                if (precheckKey.GetValue("Debugger") is not string debugger || string.IsNullOrWhiteSpace(debugger))
                    return ActionResult.Fail("Debugger value not found", "ERR_VALUE_NOT_FOUND");
            }

            var transaction = await _transactionManager.CreateTransactionAsync(entry, ActionType.Delete, cancellationToken);

            try
            {
                // Delete the Debugger value
                using var key = Registry.LocalMachine.OpenSubKey($@"{IFEOPath}\{appName}", true);
                if (key != null)
                {
                    key.DeleteValue("Debugger", false);

                    // If no other values remain, delete the key
                    if (key.ValueCount == 0)
                    {
                        Registry.LocalMachine.DeleteSubKey($@"{IFEOPath}\{appName}", false);
                    }
                }

                await _transactionManager.CommitAsync(transaction.Id, cancellationToken);

                _logger.LogInformation("Deleted IFEO entry for {App}", appName);
                return ActionResult.Ok(transactionId: transaction.Id);
            }
            catch (Exception)
            {
                await _transactionManager.RollbackAsync(transaction.Id, cancellationToken);
                throw;
            }
        }
        catch (UnauthorizedAccessException)
        {
            return ActionResult.Fail("Access denied. Administrator privileges required.", "ERR_ACCESS_DENIED");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting IFEO entry {Id}", entry.Id);
            return ActionResult.Fail(ex.Message, "ERR_DELETE_FAILED");
        }
    }

    private static string? ExtractAppName(string sourcePath)
    {
        // Source path format: HKLM\SOFTWARE\...\Image File Execution Options\app.exe
        var parts = sourcePath.Split('\\');
        for (int i = 0; i < parts.Length - 1; i++)
        {
            if (parts[i].Equals("Image File Execution Options", StringComparison.OrdinalIgnoreCase))
            {
                return parts[i + 1];
            }
        }
        return null;
    }
}
