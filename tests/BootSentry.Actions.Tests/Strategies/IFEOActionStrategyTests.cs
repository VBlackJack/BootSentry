using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Xunit;
using BootSentry.Actions.Strategies;
using BootSentry.Core.Enums;
using BootSentry.Core.Interfaces;
using BootSentry.Core.Models;

namespace BootSentry.Actions.Tests.Strategies;

public class IFEOActionStrategyTests
{
    private readonly IFEOActionStrategy _strategy;

    public IFEOActionStrategyTests()
    {
        var transactionManagerMock = new Mock<ITransactionManager>();
        _strategy = new IFEOActionStrategy(
            NullLogger<IFEOActionStrategy>.Instance,
            transactionManagerMock.Object);
    }

    [Fact]
    public void EntryType_IsIFEO()
    {
        _strategy.EntryType.Should().Be(EntryType.IFEO);
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
    public void RequiresAdmin_AlwaysTrue()
    {
        _strategy.RequiresAdmin(CreateEntry(EntryType.IFEO, @"HKLM\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Image File Execution Options\test.exe"), ActionType.Disable).Should().BeTrue();
    }

    [Fact]
    public async Task DisableAsync_WithWrongType_ReturnsInvalidType()
    {
        var entry = CreateEntry(EntryType.RegistryRun, @"HKLM\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Image File Execution Options\test.exe");

        var result = await _strategy.DisableAsync(entry);

        result.Success.Should().BeFalse();
        result.ErrorCode.Should().Be("ERR_INVALID_TYPE");
    }

    [Fact]
    public async Task EnableAsync_WithWrongType_ReturnsInvalidType()
    {
        var entry = CreateEntry(EntryType.RegistryRun, @"HKLM\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Image File Execution Options\test.exe");

        var result = await _strategy.EnableAsync(entry);

        result.Success.Should().BeFalse();
        result.ErrorCode.Should().Be("ERR_INVALID_TYPE");
    }

    [Fact]
    public async Task DeleteAsync_WithWrongType_ReturnsInvalidType()
    {
        var entry = CreateEntry(EntryType.RegistryRun, @"HKLM\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Image File Execution Options\test.exe");

        var result = await _strategy.DeleteAsync(entry);

        result.Success.Should().BeFalse();
        result.ErrorCode.Should().Be("ERR_INVALID_TYPE");
    }

    [Fact]
    public async Task DisableAsync_WithInvalidSourcePath_ReturnsInvalidSource()
    {
        var entry = CreateEntry(EntryType.IFEO, @"HKLM\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Image File Execution Options");

        var result = await _strategy.DisableAsync(entry);

        result.Success.Should().BeFalse();
        result.ErrorCode.Should().Be("ERR_INVALID_SOURCE");
    }

    [Fact]
    public async Task EnableAsync_WithInvalidSourcePath_ReturnsInvalidSource()
    {
        var entry = CreateEntry(EntryType.IFEO, @"HKLM\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Image File Execution Options");

        var result = await _strategy.EnableAsync(entry);

        result.Success.Should().BeFalse();
        result.ErrorCode.Should().Be("ERR_INVALID_SOURCE");
    }

    [Fact]
    public async Task DeleteAsync_WithInvalidSourcePath_ReturnsInvalidSource()
    {
        var entry = CreateEntry(EntryType.IFEO, @"HKLM\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Image File Execution Options");

        var result = await _strategy.DeleteAsync(entry);

        result.Success.Should().BeFalse();
        result.ErrorCode.Should().Be("ERR_INVALID_SOURCE");
    }

    private static StartupEntry CreateEntry(EntryType type, string sourcePath)
    {
        return new StartupEntry
        {
            Id = "ifeo-test",
            Type = type,
            Scope = EntryScope.Machine,
            DisplayName = "IFEO: test.exe",
            SourcePath = sourcePath,
            SourceName = "Debugger",
            CommandLineRaw = @"C:\Temp\debugger.exe"
        };
    }
}
