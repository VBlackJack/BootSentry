using Microsoft.Extensions.Logging;
using BootSentry.Core.Enums;
using BootSentry.Core.Interfaces;
using BootSentry.Core.Models;

namespace BootSentry.Actions.Strategies;

/// <summary>
/// Action strategy for Startup Folder entries.
/// Disables by moving files to a quarantine folder.
/// </summary>
public sealed class StartupFolderActionStrategy : IActionStrategy
{
    private readonly ILogger<StartupFolderActionStrategy> _logger;
    private readonly ITransactionManager _transactionManager;

    private static readonly string QuarantinePath = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
        "BootSentry",
        "Quarantine",
        "StartupFolder");

    public StartupFolderActionStrategy(ILogger<StartupFolderActionStrategy> logger, ITransactionManager transactionManager)
    {
        _logger = logger;
        _transactionManager = transactionManager;
    }

    public EntryType EntryType => EntryType.StartupFolder;
    public bool CanDisable => true;
    public bool CanDelete => true;

    public bool RequiresAdmin(StartupEntry entry, ActionType action)
    {
        // Common startup folder requires admin
        var commonStartup = Environment.GetFolderPath(Environment.SpecialFolder.CommonStartup);
        return entry.SourcePath.StartsWith(commonStartup, StringComparison.OrdinalIgnoreCase);
    }

    public async Task<ActionResult> DisableAsync(StartupEntry entry, CancellationToken cancellationToken = default)
    {
        if (entry.Type != EntryType.StartupFolder)
        {
            return ActionResult.Fail("Invalid entry type for this strategy", "ERR_INVALID_TYPE");
        }

        if (entry.SourceName == null)
        {
            return ActionResult.Fail("Entry has no source name", "ERR_NO_SOURCE_NAME");
        }

        var sourcePath = Path.Combine(entry.SourcePath, entry.SourceName);

        if (!File.Exists(sourcePath))
        {
            return ActionResult.Fail($"File not found: {sourcePath}", "ERR_FILE_NOT_FOUND");
        }

        try
        {
            // Create backup transaction
            var transaction = await _transactionManager.CreateTransactionAsync(entry, ActionType.Disable, cancellationToken);

            // Ensure quarantine directory exists
            var quarantineDir = Path.Combine(QuarantinePath, GetRelativeFolderName(entry.SourcePath));
            Directory.CreateDirectory(quarantineDir);

            // Move file to quarantine
            var destPath = Path.Combine(quarantineDir, entry.SourceName);

            // Handle existing file in quarantine
            if (File.Exists(destPath))
            {
                var backupPath = destPath + ".bak";
                if (File.Exists(backupPath))
                    File.Delete(backupPath);
                File.Move(destPath, backupPath);
            }

            File.Move(sourcePath, destPath);

            // Commit transaction
            await _transactionManager.CommitAsync(transaction.Id, cancellationToken);

            _logger.LogInformation("Disabled startup folder entry: {Name} (moved to {Dest})", entry.DisplayName, destPath);

            entry.Status = EntryStatus.Disabled;
            return ActionResult.Ok(entry, transaction.Id);
        }
        catch (UnauthorizedAccessException ex)
        {
            _logger.LogError(ex, "Access denied disabling startup folder entry");
            return ActionResult.Fail("Access denied. Check file permissions.", "ERR_ACCESS_DENIED");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error disabling startup folder entry");
            return ActionResult.Fail(ex.Message, "ERR_DISABLE_FAILED");
        }
    }

    public async Task<ActionResult> EnableAsync(StartupEntry entry, CancellationToken cancellationToken = default)
    {
        if (entry.Type != EntryType.StartupFolder)
        {
            return ActionResult.Fail("Invalid entry type for this strategy", "ERR_INVALID_TYPE");
        }

        if (entry.SourceName == null)
        {
            return ActionResult.Fail("Entry has no source name", "ERR_NO_SOURCE_NAME");
        }

        try
        {
            // Find file in quarantine
            var quarantineDir = Path.Combine(QuarantinePath, GetRelativeFolderName(entry.SourcePath));
            var quarantinePath = Path.Combine(quarantineDir, entry.SourceName);

            if (!File.Exists(quarantinePath))
            {
                return ActionResult.Fail($"Quarantined file not found: {quarantinePath}", "ERR_FILE_NOT_FOUND");
            }

            var destPath = Path.Combine(entry.SourcePath, entry.SourceName);

            // Check if destination already exists
            if (File.Exists(destPath))
            {
                return ActionResult.Fail($"File already exists at destination: {destPath}", "ERR_FILE_EXISTS");
            }

            // Move file back
            File.Move(quarantinePath, destPath);

            _logger.LogInformation("Enabled startup folder entry: {Name}", entry.DisplayName);

            entry.Status = EntryStatus.Enabled;
            return ActionResult.Ok(entry);
        }
        catch (UnauthorizedAccessException ex)
        {
            _logger.LogError(ex, "Access denied enabling startup folder entry");
            return ActionResult.Fail("Access denied. Check file permissions.", "ERR_ACCESS_DENIED");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error enabling startup folder entry");
            return ActionResult.Fail(ex.Message, "ERR_ENABLE_FAILED");
        }
    }

    public async Task<ActionResult> DeleteAsync(StartupEntry entry, CancellationToken cancellationToken = default)
    {
        if (entry.Type != EntryType.StartupFolder)
        {
            return ActionResult.Fail("Invalid entry type for this strategy", "ERR_INVALID_TYPE");
        }

        if (entry.SourceName == null)
        {
            return ActionResult.Fail("Entry has no source name", "ERR_NO_SOURCE_NAME");
        }

        var sourcePath = Path.Combine(entry.SourcePath, entry.SourceName);

        if (!File.Exists(sourcePath))
        {
            return ActionResult.Fail($"File not found: {sourcePath}", "ERR_FILE_NOT_FOUND");
        }

        try
        {
            // Create backup transaction first
            var transaction = await _transactionManager.CreateTransactionAsync(entry, ActionType.Delete, cancellationToken);

            // Delete the file
            File.Delete(sourcePath);

            // Commit transaction
            await _transactionManager.CommitAsync(transaction.Id, cancellationToken);

            _logger.LogInformation("Deleted startup folder entry: {Name}", entry.DisplayName);

            return ActionResult.Ok(transactionId: transaction.Id);
        }
        catch (UnauthorizedAccessException ex)
        {
            _logger.LogError(ex, "Access denied deleting startup folder entry");
            return ActionResult.Fail("Access denied. Check file permissions.", "ERR_ACCESS_DENIED");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting startup folder entry");
            return ActionResult.Fail(ex.Message, "ERR_DELETE_FAILED");
        }
    }

    private static string GetRelativeFolderName(string folderPath)
    {
        var userStartup = Environment.GetFolderPath(Environment.SpecialFolder.Startup);
        var commonStartup = Environment.GetFolderPath(Environment.SpecialFolder.CommonStartup);

        if (folderPath.Equals(userStartup, StringComparison.OrdinalIgnoreCase))
            return "User";

        if (folderPath.Equals(commonStartup, StringComparison.OrdinalIgnoreCase))
            return "Common";

        return "Other";
    }
}
