using FluentAssertions;
using Xunit;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using BootSentry.Actions;
using BootSentry.Core.Enums;
using BootSentry.Core.Interfaces;
using BootSentry.Core.Models;

namespace BootSentry.Actions.Tests;

public class ActionExecutorTests
{
    private readonly Mock<IActionStrategy> _mockStrategy;
    private readonly ActionExecutor _executor;

    public ActionExecutorTests()
    {
        _mockStrategy = new Mock<IActionStrategy>();
        _mockStrategy.Setup(s => s.EntryType).Returns(EntryType.RegistryRun);
        _mockStrategy.Setup(s => s.CanDisable).Returns(true);
        _mockStrategy.Setup(s => s.CanDelete).Returns(true);

        _executor = new ActionExecutor(
            NullLogger<ActionExecutor>.Instance,
            new[] { _mockStrategy.Object });
    }

    [Fact]
    public void GetStrategy_WithKnownType_ReturnsStrategy()
    {
        var strategy = _executor.GetStrategy(EntryType.RegistryRun);
        strategy.Should().NotBeNull();
        strategy.Should().Be(_mockStrategy.Object);
    }

    [Fact]
    public void GetStrategy_WithUnknownType_ReturnsNull()
    {
        var strategy = _executor.GetStrategy(EntryType.Service);
        strategy.Should().BeNull();
    }

    [Fact]
    public void CanPerformAction_WithSupportedAction_ReturnsTrue()
    {
        var entry = CreateTestEntry(EntryType.RegistryRun);

        _executor.CanPerformAction(entry, ActionType.Disable).Should().BeTrue();
        _executor.CanPerformAction(entry, ActionType.Delete).Should().BeTrue();
    }

    [Fact]
    public void CanPerformAction_WithUnsupportedType_ReturnsFalse()
    {
        var entry = CreateTestEntry(EntryType.Service);

        _executor.CanPerformAction(entry, ActionType.Disable).Should().BeFalse();
    }

    [Fact]
    public async Task DisableAsync_CallsStrategyDisable()
    {
        var entry = CreateTestEntry(EntryType.RegistryRun);
        _mockStrategy.Setup(s => s.DisableAsync(entry, It.IsAny<CancellationToken>()))
            .ReturnsAsync(ActionResult.Ok(entry));

        var result = await _executor.DisableAsync(entry);

        result.Success.Should().BeTrue();
        _mockStrategy.Verify(s => s.DisableAsync(entry, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task EnableAsync_CallsStrategyEnable()
    {
        var entry = CreateTestEntry(EntryType.RegistryRun);
        _mockStrategy.Setup(s => s.EnableAsync(entry, It.IsAny<CancellationToken>()))
            .ReturnsAsync(ActionResult.Ok(entry));

        var result = await _executor.EnableAsync(entry);

        result.Success.Should().BeTrue();
        _mockStrategy.Verify(s => s.EnableAsync(entry, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_CallsStrategyDelete()
    {
        var entry = CreateTestEntry(EntryType.RegistryRun);
        _mockStrategy.Setup(s => s.DeleteAsync(entry, It.IsAny<CancellationToken>()))
            .ReturnsAsync(ActionResult.Ok());

        var result = await _executor.DeleteAsync(entry);

        result.Success.Should().BeTrue();
        _mockStrategy.Verify(s => s.DeleteAsync(entry, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task ExecuteAsync_WithNoStrategy_ReturnsFailure()
    {
        var entry = CreateTestEntry(EntryType.Service);

        var result = await _executor.ExecuteAsync(entry, ActionType.Disable);

        result.Success.Should().BeFalse();
        result.ErrorCode.Should().Be("ERR_NO_STRATEGY");
    }

    [Fact]
    public void RequiresAdmin_DelegatesToStrategy()
    {
        var entry = CreateTestEntry(EntryType.RegistryRun);
        _mockStrategy.Setup(s => s.RequiresAdmin(entry, ActionType.Disable)).Returns(true);

        var result = _executor.RequiresAdmin(entry, ActionType.Disable);

        result.Should().BeTrue();
        _mockStrategy.Verify(s => s.RequiresAdmin(entry, ActionType.Disable), Times.Once);
    }

    private static StartupEntry CreateTestEntry(EntryType type)
    {
        return new StartupEntry
        {
            Id = "test-id",
            Type = type,
            Scope = EntryScope.User,
            DisplayName = "Test Entry",
            SourcePath = "Test\\Path"
        };
    }
}
