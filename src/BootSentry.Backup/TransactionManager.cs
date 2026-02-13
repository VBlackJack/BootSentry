using System.Security.Principal;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Win32;
using BootSentry.Backup.Models;
using BootSentry.Core.Enums;
using BootSentry.Core.Interfaces;
using BootSentry.Core.Models;

namespace BootSentry.Backup;

/// <summary>
/// Manages backup transactions for rollback operations.
/// </summary>
public sealed class TransactionManager : ITransactionManager
{
    private readonly ILogger<TransactionManager> _logger;
    private readonly BackupStorage _storage;

    public TransactionManager(ILogger<TransactionManager> logger, BackupStorage storage)
    {
        _logger = logger;
        _storage = storage;
    }

    /// <inheritdoc/>
    public async Task<Transaction> CreateTransactionAsync(
        StartupEntry entry,
        ActionType actionType,
        CancellationToken cancellationToken = default)
    {
        var transactionId = GenerateTransactionId();
        var userName = GetCurrentUserName();
        var userSid = GetCurrentUserSid();

        _logger.LogInformation("Creating transaction {Id} for {Action} on {Entry}",
            transactionId, actionType, entry.DisplayName);

        // Create transaction directory
        _storage.CreateTransactionDirectory(transactionId);

        // Create manifest
        var manifest = new TransactionManifest
        {
            Id = transactionId,
            Timestamp = DateTime.UtcNow,
            User = userName,
            UserSid = userSid,
            MachineName = Environment.MachineName,
            ActionType = actionType,
            EntryId = entry.Id,
            EntryDisplayName = entry.DisplayName,
            EntryType = entry.Type,
            EntryScope = entry.Scope,
            SourcePath = entry.SourcePath,
            SourceName = entry.SourceName,
            OriginalValue = entry.CommandLineRaw,
            Status = TransactionStatus.Pending
        };

        // Backup the original data based on entry type
        var payloadFiles = await BackupEntryDataAsync(transactionId, entry, cancellationToken);
        manifest.PayloadFiles.AddRange(payloadFiles);

        // Save manifest
        await _storage.SaveManifestAsync(manifest, cancellationToken);

        return ManifestToTransaction(manifest, entry);
    }

    /// <inheritdoc/>
    public async Task CommitAsync(string transactionId, CancellationToken cancellationToken = default)
    {
        var manifest = await _storage.LoadManifestAsync(transactionId, cancellationToken);
        if (manifest == null)
        {
            _logger.LogWarning("Transaction not found: {Id}", transactionId);
            return;
        }

        manifest.Status = TransactionStatus.Committed;
        manifest.CompletedAt = DateTime.UtcNow;

        await _storage.SaveManifestAsync(manifest, cancellationToken);
        _logger.LogInformation("Transaction committed: {Id}", transactionId);
    }

    /// <inheritdoc/>
    public async Task<ActionResult> RollbackAsync(string transactionId, CancellationToken cancellationToken = default)
    {
        var manifest = await _storage.LoadManifestAsync(transactionId, cancellationToken);
        if (manifest == null)
        {
            return ActionResult.Fail("Transaction not found", "ERR_TRANSACTION_NOT_FOUND");
        }

        if (!manifest.CanRestore)
        {
            return ActionResult.Fail("This transaction cannot be restored", "ERR_CANNOT_RESTORE");
        }

        if (manifest.Status != TransactionStatus.Committed)
        {
            return ActionResult.Fail(
                $"Transaction is not in a restorable state: {manifest.Status}",
                "ERR_INVALID_TRANSACTION_STATE");
        }

        _logger.LogInformation("Rolling back transaction: {Id}", transactionId);

        try
        {
            // Restore based on entry type
            await RestoreEntryDataAsync(manifest, cancellationToken);

            manifest.Status = TransactionStatus.RolledBack;
            manifest.CompletedAt = DateTime.UtcNow;
            await _storage.SaveManifestAsync(manifest, cancellationToken);

            _logger.LogInformation("Transaction rolled back successfully: {Id}", transactionId);
            return ActionResult.Ok(transactionId: transactionId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to rollback transaction: {Id}", transactionId);

            manifest.Status = TransactionStatus.Failed;
            manifest.ErrorMessage = ex.Message;
            await _storage.SaveManifestAsync(manifest, cancellationToken);

            return ActionResult.Fail(ex.Message, "ERR_ROLLBACK_FAILED");
        }
    }

    /// <inheritdoc/>
    public async Task<IReadOnlyList<Transaction>> GetTransactionsAsync(
        int? limit = null,
        CancellationToken cancellationToken = default)
    {
        var manifests = await _storage.GetAllManifestsAsync(cancellationToken);

        if (limit.HasValue)
            manifests = manifests.Take(limit.Value).ToList();

        return manifests.Select(m => ManifestToTransaction(m, null)).ToList();
    }

    /// <inheritdoc/>
    public async Task<Transaction?> GetTransactionAsync(string transactionId, CancellationToken cancellationToken = default)
    {
        var manifest = await _storage.LoadManifestAsync(transactionId, cancellationToken);
        return manifest != null ? ManifestToTransaction(manifest, null) : null;
    }

    /// <inheritdoc/>
    public async Task<int> PurgeOldTransactionsAsync(
        TimeSpan? maxAge = null,
        int? maxCount = null,
        CancellationToken cancellationToken = default)
    {
        return await _storage.PurgeOldTransactionsAsync(maxAge, maxCount, cancellationToken);
    }

    private async Task<List<string>> BackupEntryDataAsync(
        string transactionId,
        StartupEntry entry,
        CancellationToken cancellationToken)
    {
        var payloadFiles = new List<string>();

        switch (entry.Type)
        {
            case EntryType.RegistryRun:
            case EntryType.RegistryRunOnce:
            case EntryType.Winlogon:
            case EntryType.AppInitDlls:
            case EntryType.IFEO:
                // Backup registry value
                if (!string.IsNullOrWhiteSpace(entry.SourceName))
                {
                    var (hive, path) = ParseRegistryPath(entry.SourcePath);
                    using var baseKey = RegistryKey.OpenBaseKey(hive, RegistryView.Default);
                    using var key = baseKey.OpenSubKey(path);

                    if (key != null)
                    {
                        await BackupRegistryValueIfPresentAsync(
                            payloadFiles,
                            transactionId,
                            key,
                            entry.SourcePath,
                            entry.SourceName,
                            cancellationToken);
                    }
                }
                break;

            case EntryType.StartupFolder:
                // Backup the file/shortcut
                if (entry.SourceName != null)
                {
                    var filePath = Path.Combine(entry.SourcePath, entry.SourceName);
                    if (File.Exists(filePath))
                    {
                        var backupPath = await _storage.BackupFileAsync(
                            transactionId, filePath, entry.SourceName, cancellationToken);
                        payloadFiles.Add(backupPath);
                    }
                }
                break;

            case EntryType.ScheduledTask:
                // Task state restore is handled by action strategy.
                break;

            case EntryType.Service:
                if (!string.IsNullOrWhiteSpace(entry.SourceName))
                {
                    var serviceRegistryPath = $@"HKLM\SYSTEM\CurrentControlSet\Services\{entry.SourceName}";
                    using var serviceKey = Registry.LocalMachine.OpenSubKey($@"SYSTEM\CurrentControlSet\Services\{entry.SourceName}");

                    if (serviceKey != null)
                    {
                        // Start is always required for service state restore.
                        await BackupRegistryValueIfPresentAsync(
                            payloadFiles,
                            transactionId,
                            serviceKey,
                            serviceRegistryPath,
                            "Start",
                            cancellationToken);

                        // DelayedAutostart is optional but needed to restore "Automatic (Delayed)".
                        await BackupRegistryValueIfPresentAsync(
                            payloadFiles,
                            transactionId,
                            serviceKey,
                            serviceRegistryPath,
                            "DelayedAutostart",
                            cancellationToken);
                    }
                }
                break;
        }

        return payloadFiles;
    }

    private async Task RestoreEntryDataAsync(TransactionManifest manifest, CancellationToken cancellationToken)
    {
        switch (manifest.EntryType)
        {
            case EntryType.RegistryRun:
            case EntryType.RegistryRunOnce:
            case EntryType.Winlogon:
            case EntryType.AppInitDlls:
            case EntryType.IFEO:
            case EntryType.Service:
                await RestoreRegistryValuesAsync(manifest, cancellationToken);
                break;

            case EntryType.StartupFolder:
                await RestoreStartupFileAsync(manifest, cancellationToken);
                break;

            case EntryType.ScheduledTask:
                // Handled by action strategy
                break;
        }
    }

    private async Task RestoreRegistryValuesAsync(TransactionManifest manifest, CancellationToken cancellationToken)
    {
        if (manifest.PayloadFiles.Count == 0)
        {
            throw new InvalidOperationException("No backup data found for registry restore");
        }

        foreach (var backupFile in manifest.PayloadFiles)
        {
            var json = await File.ReadAllTextAsync(backupFile, cancellationToken);
            var backupData = JsonSerializer.Deserialize<JsonElement>(json);

            var keyPath = backupData.GetProperty("keyPath").GetString()!;
            var valueName = backupData.GetProperty("valueName").GetString()!;
            var value = backupData.GetProperty("value").GetString();
            var valueKindStr = backupData.GetProperty("valueKind").GetString()!;
            var valueKind = Enum.Parse<RegistryValueKind>(valueKindStr);

            var (hive, path) = ParseRegistryPath(keyPath);
            using var baseKey = RegistryKey.OpenBaseKey(hive, RegistryView.Default);
            using var key = baseKey.OpenSubKey(path, writable: true)
                ?? baseKey.CreateSubKey(path, writable: true);

            if (key == null)
            {
                throw new InvalidOperationException($"Registry key not found: {keyPath}");
            }

            key.SetValue(valueName, ConvertBackupValue(value, valueKind), valueKind);
            _logger.LogInformation("Restored registry value: {Key}\\{Value}", keyPath, valueName);
        }
    }

    private async Task RestoreStartupFileAsync(TransactionManifest manifest, CancellationToken cancellationToken)
    {
        if (manifest.PayloadFiles.Count == 0 || manifest.SourceName == null)
        {
            throw new InvalidOperationException("No backup data found for file restore");
        }

        var backupFile = manifest.PayloadFiles[0];
        var destPath = Path.Combine(manifest.SourcePath, manifest.SourceName);

        await using var source = new FileStream(backupFile, FileMode.Open, FileAccess.Read);
        await using var dest = new FileStream(destPath, FileMode.Create, FileAccess.Write);
        await source.CopyToAsync(dest, cancellationToken);

        _logger.LogInformation("Restored file: {Path}", destPath);
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
            "HKCR" or "HKEY_CLASSES_ROOT" => RegistryHive.ClassesRoot,
            "HKU" or "HKEY_USERS" => RegistryHive.Users,
            _ => throw new ArgumentException($"Unknown registry hive: {hiveName}")
        };

        return (hive, path);
    }

    private static string GenerateTransactionId()
    {
        var timestamp = DateTime.UtcNow.ToString("yyyyMMdd-HHmmss");
        var guid = Guid.NewGuid().ToString("N")[..8];
        return $"{timestamp}-{guid}";
    }

    private static string GetCurrentUserName()
    {
        return Environment.UserName;
    }

    private static string? GetCurrentUserSid()
    {
        try
        {
            var identity = WindowsIdentity.GetCurrent();
            return identity.User?.Value;
        }
        catch (Exception)
        {
            // Can fail in certain impersonation or restricted contexts
            return null;
        }
    }

    private static Transaction ManifestToTransaction(TransactionManifest manifest, StartupEntry? entry)
    {
        // Create a snapshot entry if we don't have the original
        var snapshotEntry = entry ?? new StartupEntry
        {
            Id = manifest.EntryId,
            Type = manifest.EntryType,
            Scope = manifest.EntryScope,
            DisplayName = manifest.EntryDisplayName,
            SourcePath = manifest.SourcePath,
            SourceName = manifest.SourceName,
            CommandLineRaw = manifest.OriginalValue
        };

        return new Transaction
        {
            Id = manifest.Id,
            Timestamp = manifest.Timestamp,
            User = manifest.User,
            ActionType = manifest.ActionType,
            EntryId = manifest.EntryId,
            EntryDisplayName = manifest.EntryDisplayName,
            EntrySnapshotBefore = snapshotEntry,
            PayloadPaths = manifest.PayloadFiles,
            CanRestore = manifest.CanRestore && manifest.Status == TransactionStatus.Committed,
            Notes = manifest.Notes
        };
    }

    private async Task BackupRegistryValueIfPresentAsync(
        ICollection<string> payloadFiles,
        string transactionId,
        RegistryKey key,
        string keyPath,
        string valueName,
        CancellationToken cancellationToken)
    {
        if (!key.GetValueNames().Contains(valueName, StringComparer.OrdinalIgnoreCase))
            return;

        var value = key.GetValue(valueName);
        var valueKind = key.GetValueKind(valueName);

        var backupPath = await _storage.BackupRegistryValueAsync(
            transactionId,
            keyPath,
            valueName,
            value ?? string.Empty,
            valueKind,
            cancellationToken);

        payloadFiles.Add(backupPath);
    }

    private static object ConvertBackupValue(string? value, RegistryValueKind valueKind)
    {
        return valueKind switch
        {
            RegistryValueKind.DWord => int.TryParse(value, out var dwordValue) ? dwordValue : 0,
            RegistryValueKind.QWord => long.TryParse(value, out var qwordValue) ? qwordValue : 0L,
            _ => value ?? string.Empty
        };
    }
}
