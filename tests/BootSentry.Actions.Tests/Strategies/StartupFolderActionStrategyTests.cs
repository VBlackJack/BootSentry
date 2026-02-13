using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Xunit;
using BootSentry.Actions.Strategies;
using BootSentry.Core.Enums;
using BootSentry.Core.Interfaces;
using BootSentry.Core.Models;

namespace BootSentry.Actions.Tests.Strategies;

public class StartupFolderActionStrategyTests
{
    private readonly Mock<ITransactionManager> _transactionManagerMock;
    private readonly StartupFolderActionStrategy _strategy;

    public StartupFolderActionStrategyTests()
    {
        _transactionManagerMock = new Mock<ITransactionManager>();
        _strategy = new StartupFolderActionStrategy(
            NullLogger<StartupFolderActionStrategy>.Instance,
            _transactionManagerMock.Object);
    }

    [Fact]
    public void EntryType_IsStartupFolder()
    {
        _strategy.EntryType.Should().Be(EntryType.StartupFolder);
    }

    [Fact]
    public void CanDisable_IsTrue()
    {
        _strategy.CanDisable.Should().BeTrue();
    }

    [Fact]
    public void CanDelete_IsTrue()
    {
        _strategy.CanDelete.Should().BeTrue();
    }

    [Fact]
    public void RequiresAdmin_IsTrueForCommonStartup()
    {
        var entry = new StartupEntry
        {
            Id = "sf-common",
            Type = EntryType.StartupFolder,
            Scope = EntryScope.Machine,
            DisplayName = "Common",
            SourcePath = Environment.GetFolderPath(Environment.SpecialFolder.CommonStartup),
            SourceName = "test.lnk"
        };

        _strategy.RequiresAdmin(entry, ActionType.Disable).Should().BeTrue();
    }

    [Fact]
    public void RequiresAdmin_IsFalseForUserStartup()
    {
        var entry = new StartupEntry
        {
            Id = "sf-user",
            Type = EntryType.StartupFolder,
            Scope = EntryScope.User,
            DisplayName = "User",
            SourcePath = Environment.GetFolderPath(Environment.SpecialFolder.Startup),
            SourceName = "test.lnk"
        };

        _strategy.RequiresAdmin(entry, ActionType.Disable).Should().BeFalse();
    }

    [Fact]
    public async Task DisableAsync_WithWrongType_ReturnsInvalidType()
    {
        var entry = CreateEntry(EntryType.RegistryRun, "unused");

        var result = await _strategy.DisableAsync(entry);

        result.Success.Should().BeFalse();
        result.ErrorCode.Should().Be("ERR_INVALID_TYPE");
    }

    [Fact]
    public async Task DisableAsync_WithNoSourceName_ReturnsNoSourceName()
    {
        var entry = new StartupEntry
        {
            Id = "sf-nosource",
            Type = EntryType.StartupFolder,
            Scope = EntryScope.User,
            DisplayName = "No source",
            SourcePath = Path.GetTempPath(),
            SourceName = null
        };

        var result = await _strategy.DisableAsync(entry);

        result.Success.Should().BeFalse();
        result.ErrorCode.Should().Be("ERR_NO_SOURCE_NAME");
    }

    [Fact]
    public async Task DisableAsync_WithMissingFile_ReturnsFileNotFound()
    {
        var entry = CreateEntry(EntryType.StartupFolder, Guid.NewGuid() + ".lnk");

        var result = await _strategy.DisableAsync(entry);

        result.Success.Should().BeFalse();
        result.ErrorCode.Should().Be("ERR_FILE_NOT_FOUND");
    }

    [Fact]
    public async Task DeleteAsync_WhenCommitFails_RollsBackTransaction()
    {
        var tempDir = Path.Combine(Path.GetTempPath(), "BootSentry_SF_Test_" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(tempDir);
        var fileName = "rollback-test.lnk";
        var filePath = Path.Combine(tempDir, fileName);
        await File.WriteAllTextAsync(filePath, "test");

        var entry = new StartupEntry
        {
            Id = "sf-rollback",
            Type = EntryType.StartupFolder,
            Scope = EntryScope.User,
            DisplayName = "Rollback test",
            SourcePath = tempDir,
            SourceName = fileName
        };

        var tx = CreateTransaction("tx-sf-rollback", entry, ActionType.Delete);

        _transactionManagerMock
            .Setup(m => m.CreateTransactionAsync(
                It.IsAny<StartupEntry>(),
                ActionType.Delete,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(tx);

        _transactionManagerMock
            .Setup(m => m.CommitAsync(tx.Id, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new InvalidOperationException("Commit failed"));

        _transactionManagerMock
            .Setup(m => m.RollbackAsync(tx.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(ActionResult.Ok(transactionId: tx.Id));

        try
        {
            var result = await _strategy.DeleteAsync(entry);

            result.Success.Should().BeFalse();
            result.ErrorCode.Should().Be("ERR_DELETE_FAILED");
            _transactionManagerMock.Verify(
                m => m.RollbackAsync(tx.Id, It.IsAny<CancellationToken>()),
                Times.Once);
        }
        finally
        {
            if (Directory.Exists(tempDir))
                Directory.Delete(tempDir, recursive: true);
        }
    }

    private static StartupEntry CreateEntry(EntryType type, string fileName)
    {
        return new StartupEntry
        {
            Id = "sf-test",
            Type = type,
            Scope = EntryScope.User,
            DisplayName = "Startup file",
            SourcePath = Path.GetTempPath(),
            SourceName = fileName
        };
    }

    private static Transaction CreateTransaction(string id, StartupEntry entry, ActionType actionType)
    {
        return new Transaction
        {
            Id = id,
            Timestamp = DateTime.UtcNow,
            User = "test-user",
            ActionType = actionType,
            EntryId = entry.Id,
            EntryDisplayName = entry.DisplayName,
            EntrySnapshotBefore = entry
        };
    }
}
