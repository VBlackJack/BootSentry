using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Xunit;
using BootSentry.Actions.Strategies;
using BootSentry.Core.Enums;
using BootSentry.Core.Interfaces;
using BootSentry.Core.Models;

namespace BootSentry.Actions.Tests.Strategies;

public class ServiceActionStrategyTests
{
    private readonly ServiceActionStrategy _strategy;

    public ServiceActionStrategyTests()
    {
        var transactionManagerMock = new Mock<ITransactionManager>();
        _strategy = new ServiceActionStrategy(
            NullLogger<ServiceActionStrategy>.Instance,
            transactionManagerMock.Object);
    }

    [Fact]
    public void EntryType_IsService()
    {
        _strategy.EntryType.Should().Be(EntryType.Service);
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
    public void RequiresAdmin_AlwaysReturnsTrue()
    {
        var entry = CreateEntry(EntryType.Service, "TestService");
        _strategy.RequiresAdmin(entry, ActionType.Disable).Should().BeTrue();
        _strategy.RequiresAdmin(entry, ActionType.Enable).Should().BeTrue();
        _strategy.RequiresAdmin(entry, ActionType.Delete).Should().BeTrue();
    }

    [Fact]
    public async Task DisableAsync_WithWrongType_ReturnsInvalidType()
    {
        var entry = CreateEntry(EntryType.RegistryRun, "TestService");

        var result = await _strategy.DisableAsync(entry);

        result.Success.Should().BeFalse();
        result.ErrorCode.Should().Be("ERR_INVALID_TYPE");
    }

    [Fact]
    public async Task DisableAsync_WithProtectedService_ReturnsProtected()
    {
        var entry = CreateEntry(EntryType.Service, "TestService");
        entry.IsProtected = true;
        entry.ProtectionReason = "Critical service";

        var result = await _strategy.DisableAsync(entry);

        result.Success.Should().BeFalse();
        result.ErrorCode.Should().Be("ERR_PROTECTED");
    }

    [Fact]
    public async Task DisableAsync_WithNoServiceName_ReturnsNoServiceName()
    {
        var entry = CreateEntry(EntryType.Service, null);

        var result = await _strategy.DisableAsync(entry);

        result.Success.Should().BeFalse();
        result.ErrorCode.Should().Be("ERR_NO_SERVICE_NAME");
    }

    [Fact]
    public async Task EnableAsync_WithWrongType_ReturnsInvalidType()
    {
        var entry = CreateEntry(EntryType.RegistryRun, "TestService");

        var result = await _strategy.EnableAsync(entry);

        result.Success.Should().BeFalse();
        result.ErrorCode.Should().Be("ERR_INVALID_TYPE");
    }

    [Fact]
    public async Task EnableAsync_WithNoServiceName_ReturnsNoServiceName()
    {
        var entry = CreateEntry(EntryType.Service, null);

        var result = await _strategy.EnableAsync(entry);

        result.Success.Should().BeFalse();
        result.ErrorCode.Should().Be("ERR_NO_SERVICE_NAME");
    }

    [Fact]
    public async Task DeleteAsync_ReturnsNotSupported()
    {
        var result = await _strategy.DeleteAsync(CreateEntry(EntryType.Service, "TestService"));

        result.Success.Should().BeFalse();
        result.ErrorCode.Should().Be("ERR_NOT_SUPPORTED");
    }

    private static StartupEntry CreateEntry(EntryType type, string? sourceName)
    {
        return new StartupEntry
        {
            Id = "svc-test",
            Type = type,
            Scope = EntryScope.Machine,
            DisplayName = "Test Service",
            SourcePath = "Services",
            SourceName = sourceName,
            ServiceStartType = "Automatic"
        };
    }
}
