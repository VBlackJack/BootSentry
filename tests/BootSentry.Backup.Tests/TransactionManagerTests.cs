using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using BootSentry.Backup;
using BootSentry.Backup.Models;
using BootSentry.Core.Enums;

namespace BootSentry.Backup.Tests;

public class TransactionManagerTests : IDisposable
{
    private readonly Mock<ILogger<BackupStorage>> _storageLoggerMock;
    private readonly Mock<ILogger<TransactionManager>> _managerLoggerMock;
    private readonly string _testBasePath;
    private readonly BackupStorage _storage;
    private readonly TransactionManager _manager;

    public TransactionManagerTests()
    {
        _storageLoggerMock = new Mock<ILogger<BackupStorage>>();
        _managerLoggerMock = new Mock<ILogger<TransactionManager>>();

        _testBasePath = Path.Combine(Path.GetTempPath(), $"BootSentryTxTest_{Guid.NewGuid():N}");
        _storage = new BackupStorage(_storageLoggerMock.Object, _testBasePath);
        _manager = new TransactionManager(_managerLoggerMock.Object, _storage);
    }

    public void Dispose()
    {
        if (Directory.Exists(_testBasePath))
        {
            try
            {
                Directory.Delete(_testBasePath, recursive: true);
            }
            catch
            {
                // Ignore cleanup failures in tests.
            }
        }
    }

    [Fact]
    public async Task GetTransactionsAsync_OnlyCommittedTransactionsAreRestorable()
    {
        var committed = CreateManifest("committed", TransactionStatus.Committed);
        var pending = CreateManifest("pending", TransactionStatus.Pending);
        var failed = CreateManifest("failed", TransactionStatus.Failed);
        var rolledBack = CreateManifest("rolled-back", TransactionStatus.RolledBack);

        await SaveManifestAsync(committed);
        await SaveManifestAsync(pending);
        await SaveManifestAsync(failed);
        await SaveManifestAsync(rolledBack);

        var transactions = await _manager.GetTransactionsAsync();

        transactions.Should().HaveCount(4);
        transactions.Single(t => t.Id == committed.Id).CanRestore.Should().BeTrue();
        transactions.Single(t => t.Id == pending.Id).CanRestore.Should().BeFalse();
        transactions.Single(t => t.Id == failed.Id).CanRestore.Should().BeFalse();
        transactions.Single(t => t.Id == rolledBack.Id).CanRestore.Should().BeFalse();
    }

    [Fact]
    public async Task RollbackAsync_WithPendingTransaction_ReturnsInvalidState()
    {
        var pending = CreateManifest("pending-rollback", TransactionStatus.Pending);
        await SaveManifestAsync(pending);

        var result = await _manager.RollbackAsync(pending.Id);

        result.Success.Should().BeFalse();
        result.ErrorCode.Should().Be("ERR_INVALID_TRANSACTION_STATE");
    }

    [Fact]
    public async Task RollbackAsync_WithCommittedScheduledTask_SucceedsAndMarksRolledBack()
    {
        var committed = CreateManifest("committed-rollback", TransactionStatus.Committed);
        await SaveManifestAsync(committed);

        var result = await _manager.RollbackAsync(committed.Id);
        var updatedManifest = await _storage.LoadManifestAsync(committed.Id);

        result.Success.Should().BeTrue();
        updatedManifest.Should().NotBeNull();
        updatedManifest!.Status.Should().Be(TransactionStatus.RolledBack);
    }

    private async Task SaveManifestAsync(TransactionManifest manifest)
    {
        _storage.CreateTransactionDirectory(manifest.Id);
        await _storage.SaveManifestAsync(manifest);
    }

    private static TransactionManifest CreateManifest(string idPrefix, TransactionStatus status)
    {
        var id = $"{idPrefix}-{Guid.NewGuid():N}"[..24];

        return new TransactionManifest
        {
            Id = id,
            Timestamp = DateTime.UtcNow,
            User = "TestUser",
            ActionType = ActionType.Disable,
            EntryId = "entry-id",
            EntryDisplayName = "Test Entry",
            EntryType = EntryType.ScheduledTask,
            EntryScope = EntryScope.Machine,
            SourcePath = @"\Test\Task",
            SourceName = "TestTask",
            Status = status
        };
    }
}
