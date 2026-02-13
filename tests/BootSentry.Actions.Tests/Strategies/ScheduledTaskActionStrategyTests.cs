using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;
using BootSentry.Actions.Strategies;
using BootSentry.Core.Enums;
using BootSentry.Core.Models;

namespace BootSentry.Actions.Tests.Strategies;

public class ScheduledTaskActionStrategyTests
{
    private readonly ScheduledTaskActionStrategy _strategy;

    public ScheduledTaskActionStrategyTests()
    {
        _strategy = new ScheduledTaskActionStrategy(
            NullLogger<ScheduledTaskActionStrategy>.Instance);
    }

    [Fact]
    public void EntryType_IsScheduledTask()
    {
        _strategy.EntryType.Should().Be(EntryType.ScheduledTask);
    }

    [Fact]
    public void CanDisable_IsTrue()
    {
        _strategy.CanDisable.Should().BeTrue();
    }

    [Fact]
    public void CanDelete_IsFalse()
    {
        _strategy.CanDelete.Should().BeFalse();
    }

    [Fact]
    public void RequiresAdmin_TrueForMachine_FalseForUser()
    {
        var machineEntry = CreateEntry(EntryScope.Machine, EntryType.ScheduledTask);
        var userEntry = CreateEntry(EntryScope.User, EntryType.ScheduledTask);

        _strategy.RequiresAdmin(machineEntry, ActionType.Disable).Should().BeTrue();
        _strategy.RequiresAdmin(userEntry, ActionType.Disable).Should().BeFalse();
    }

    [Fact]
    public async Task DisableAsync_WithWrongType_ReturnsInvalidType()
    {
        var entry = CreateEntry(EntryScope.User, EntryType.Service);

        var result = await _strategy.DisableAsync(entry);

        result.Success.Should().BeFalse();
        result.ErrorCode.Should().Be("ERR_INVALID_TYPE");
    }

    [Fact]
    public async Task EnableAsync_WithWrongType_ReturnsInvalidType()
    {
        var entry = CreateEntry(EntryScope.User, EntryType.Service);

        var result = await _strategy.EnableAsync(entry);

        result.Success.Should().BeFalse();
        result.ErrorCode.Should().Be("ERR_INVALID_TYPE");
    }

    [Fact]
    public async Task DeleteAsync_ReturnsNotSupported()
    {
        var result = await _strategy.DeleteAsync(CreateEntry(EntryScope.Machine, EntryType.ScheduledTask));

        result.Success.Should().BeFalse();
        result.ErrorCode.Should().Be("ERR_NOT_SUPPORTED");
    }

    private static StartupEntry CreateEntry(EntryScope scope, EntryType type)
    {
        return new StartupEntry
        {
            Id = "task-test",
            Type = type,
            Scope = scope,
            DisplayName = "Task Test",
            SourcePath = @"\Microsoft\Windows\TaskTest",
            SourceName = "TaskTest"
        };
    }
}
