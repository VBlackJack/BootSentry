/*
 * Copyright 2025 Julien Bombled
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

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

    // ============================================================
    // GetTransactionsAsync Tests
    // ============================================================

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
    public async Task GetTransactionsAsync_WithLimit_ReturnsLimitedResults()
    {
        for (var i = 0; i < 5; i++)
        {
            var manifest = CreateManifest($"tx-{i}", TransactionStatus.Committed);
            await SaveManifestAsync(manifest);
        }

        var transactions = await _manager.GetTransactionsAsync(limit: 3);

        transactions.Should().HaveCount(3);
    }

    [Fact]
    public async Task GetTransactionsAsync_WithNoTransactions_ReturnsEmptyList()
    {
        var transactions = await _manager.GetTransactionsAsync();

        transactions.Should().NotBeNull();
        transactions.Should().BeEmpty();
    }

    // ============================================================
    // GetTransactionAsync Tests
    // ============================================================

    [Fact]
    public async Task GetTransactionAsync_WithValidId_ReturnsTransaction()
    {
        var manifest = CreateManifest("get-single", TransactionStatus.Committed);
        await SaveManifestAsync(manifest);

        var transaction = await _manager.GetTransactionAsync(manifest.Id);

        transaction.Should().NotBeNull();
        transaction!.Id.Should().Be(manifest.Id);
        transaction.EntryDisplayName.Should().Be(manifest.EntryDisplayName);
    }

    [Fact]
    public async Task GetTransactionAsync_WithInvalidId_ReturnsNull()
    {
        var transaction = await _manager.GetTransactionAsync("nonexistent-id-12345");

        transaction.Should().BeNull();
    }

    // ============================================================
    // RollbackAsync Tests
    // ============================================================

    [Fact]
    public async Task RollbackAsync_WithPendingTransaction_SucceedsAndMarksRolledBack()
    {
        var pending = CreateManifest("pending-rollback", TransactionStatus.Pending);
        await SaveManifestAsync(pending);

        var result = await _manager.RollbackAsync(pending.Id);
        var updatedManifest = await _storage.LoadManifestAsync(pending.Id);

        result.Success.Should().BeTrue();
        updatedManifest.Should().NotBeNull();
        updatedManifest!.Status.Should().Be(TransactionStatus.RolledBack);
    }

    [Fact]
    public async Task RollbackAsync_WithFailedTransaction_ReturnsInvalidState()
    {
        var failed = CreateManifest("failed-rollback", TransactionStatus.Failed);
        await SaveManifestAsync(failed);

        var result = await _manager.RollbackAsync(failed.Id);

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

    [Fact]
    public async Task RollbackAsync_WithNonExistentTransaction_ReturnsNotFound()
    {
        var result = await _manager.RollbackAsync("nonexistent-id-12345");

        result.Success.Should().BeFalse();
        result.ErrorCode.Should().Be("ERR_TRANSACTION_NOT_FOUND");
    }

    [Fact]
    public async Task RollbackAsync_WithRolledBackTransaction_ReturnsInvalidState()
    {
        var rolledBack = CreateManifest("already-rolled", TransactionStatus.RolledBack);
        await SaveManifestAsync(rolledBack);

        var result = await _manager.RollbackAsync(rolledBack.Id);

        result.Success.Should().BeFalse();
        result.ErrorCode.Should().Be("ERR_INVALID_TRANSACTION_STATE");
    }

    [Fact]
    public async Task RollbackAsync_WithNonRestorableTransaction_ReturnsCannotRestore()
    {
        var manifest = CreateNonRestorableManifest("non-restorable", TransactionStatus.Committed);
        await SaveManifestAsync(manifest);

        var result = await _manager.RollbackAsync(manifest.Id);

        result.Success.Should().BeFalse();
        result.ErrorCode.Should().Be("ERR_CANNOT_RESTORE");
    }

    // ============================================================
    // CommitAsync Tests
    // ============================================================

    [Fact]
    public async Task CommitAsync_WithPendingTransaction_SetsStatusToCommitted()
    {
        var pending = CreateManifest("commit-test", TransactionStatus.Pending);
        await SaveManifestAsync(pending);

        await _manager.CommitAsync(pending.Id);

        var updatedManifest = await _storage.LoadManifestAsync(pending.Id);
        updatedManifest.Should().NotBeNull();
        updatedManifest!.Status.Should().Be(TransactionStatus.Committed);
        updatedManifest.CompletedAt.Should().NotBeNull();
    }

    [Fact]
    public async Task CommitAsync_WithNonExistentTransaction_DoesNotThrow()
    {
        var action = async () => await _manager.CommitAsync("nonexistent-id-12345");

        await action.Should().NotThrowAsync();
    }

    // ============================================================
    // Concurrent Transaction Handling Tests
    // ============================================================

    [Fact]
    public async Task GetTransactionsAsync_WithMultipleConcurrentSaves_ReturnsAllTransactions()
    {
        var manifests = Enumerable.Range(0, 10)
            .Select(i => CreateManifest($"concurrent-{i}", TransactionStatus.Committed))
            .ToList();

        var tasks = manifests.Select(SaveManifestAsync);
        await Task.WhenAll(tasks);

        var transactions = await _manager.GetTransactionsAsync();

        transactions.Should().HaveCount(10);
    }

    [Fact]
    public async Task RollbackAsync_ConcurrentRollbacksOnDifferentTransactions_AllSucceed()
    {
        var manifests = Enumerable.Range(0, 5)
            .Select(i => CreateManifest($"concurrent-rb-{i}", TransactionStatus.Committed))
            .ToList();

        foreach (var manifest in manifests)
        {
            await SaveManifestAsync(manifest);
        }

        var rollbackTasks = manifests.Select(m => _manager.RollbackAsync(m.Id));
        var results = await Task.WhenAll(rollbackTasks);

        results.Should().OnlyContain(r => r.Success);
    }

    // ============================================================
    // Transaction Metadata Tests
    // ============================================================

    [Fact]
    public async Task GetTransactionAsync_PreservesEntryMetadata()
    {
        var manifest = CreateManifest("metadata-test", TransactionStatus.Committed);
        await SaveManifestAsync(manifest);

        var transaction = await _manager.GetTransactionAsync(manifest.Id);

        transaction.Should().NotBeNull();
        transaction!.ActionType.Should().Be(ActionType.Disable);
        transaction.EntryId.Should().Be("entry-id");
        transaction.EntryDisplayName.Should().Be("Test Entry");
        transaction.EntrySnapshotBefore.Should().NotBeNull();
        transaction.EntrySnapshotBefore.Type.Should().Be(EntryType.ScheduledTask);
        transaction.EntrySnapshotBefore.Scope.Should().Be(EntryScope.Machine);
    }

    [Fact]
    public async Task RollbackAsync_SetsCompletedAtTimestamp()
    {
        var before = DateTime.UtcNow;
        var manifest = CreateManifest("timestamp-test", TransactionStatus.Committed);
        await SaveManifestAsync(manifest);

        await _manager.RollbackAsync(manifest.Id);

        var updatedManifest = await _storage.LoadManifestAsync(manifest.Id);
        updatedManifest!.CompletedAt.Should().NotBeNull();
        updatedManifest.CompletedAt!.Value.Should().BeOnOrAfter(before);
    }

    // ============================================================
    // Helper Methods
    // ============================================================

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

    private static TransactionManifest CreateNonRestorableManifest(string idPrefix, TransactionStatus status)
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
            Status = status,
            CanRestore = false
        };
    }
}
