using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Xunit;
using BootSentry.Actions.Strategies;
using BootSentry.Core.Enums;
using BootSentry.Core.Interfaces;
using BootSentry.Core.Models;

namespace BootSentry.Actions.Tests.Strategies;

public class RegistryActionStrategyTests
{
    private readonly RegistryActionStrategy _strategy;

    public RegistryActionStrategyTests()
    {
        var transactionManagerMock = new Mock<ITransactionManager>();
        _strategy = new RegistryActionStrategy(
            NullLogger<RegistryActionStrategy>.Instance,
            transactionManagerMock.Object);
    }

    [Fact]
    public void EntryType_IsRegistryRun()
    {
        _strategy.EntryType.Should().Be(EntryType.RegistryRun);
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
    public void RequiresAdmin_TrueForMachine_FalseForUser()
    {
        var machineEntry = CreateEntry(EntryScope.Machine, EntryType.RegistryRun, "TestValue");
        var userEntry = CreateEntry(EntryScope.User, EntryType.RegistryRun, "TestValue");

        _strategy.RequiresAdmin(machineEntry, ActionType.Disable).Should().BeTrue();
        _strategy.RequiresAdmin(userEntry, ActionType.Disable).Should().BeFalse();
    }

    [Fact]
    public async Task DisableAsync_WithWrongType_ReturnsInvalidType()
    {
        var entry = CreateEntry(EntryScope.User, EntryType.Service, "TestValue");

        var result = await _strategy.DisableAsync(entry);

        result.Success.Should().BeFalse();
        result.ErrorCode.Should().Be("ERR_INVALID_TYPE");
    }

    [Fact]
    public async Task DisableAsync_WithNoSourceName_ReturnsNoSourceName()
    {
        var entry = CreateEntry(EntryScope.User, EntryType.RegistryRun, null);

        var result = await _strategy.DisableAsync(entry);

        result.Success.Should().BeFalse();
        result.ErrorCode.Should().Be("ERR_NO_SOURCE_NAME");
    }

    [Fact]
    public async Task EnableAsync_WithWrongType_ReturnsInvalidType()
    {
        var entry = CreateEntry(EntryScope.User, EntryType.Service, "TestValue");

        var result = await _strategy.EnableAsync(entry);

        result.Success.Should().BeFalse();
        result.ErrorCode.Should().Be("ERR_INVALID_TYPE");
    }

    [Fact]
    public async Task DeleteAsync_WithWrongType_ReturnsInvalidType()
    {
        var entry = CreateEntry(EntryScope.User, EntryType.Service, "TestValue");

        var result = await _strategy.DeleteAsync(entry);

        result.Success.Should().BeFalse();
        result.ErrorCode.Should().Be("ERR_INVALID_TYPE");
    }

    [Fact]
    public async Task DeleteAsync_WithNoSourceName_ReturnsNoSourceName()
    {
        var entry = CreateEntry(EntryScope.User, EntryType.RegistryRun, null);

        var result = await _strategy.DeleteAsync(entry);

        result.Success.Should().BeFalse();
        result.ErrorCode.Should().Be("ERR_NO_SOURCE_NAME");
    }

    private static StartupEntry CreateEntry(EntryScope scope, EntryType type, string? sourceName)
    {
        return new StartupEntry
        {
            Id = "reg-test",
            Type = type,
            Scope = scope,
            DisplayName = "Test Run Entry",
            SourcePath = @"HKCU\Software\Microsoft\Windows\CurrentVersion\Run",
            SourceName = sourceName,
            CommandLineRaw = "test.exe"
        };
    }
}
