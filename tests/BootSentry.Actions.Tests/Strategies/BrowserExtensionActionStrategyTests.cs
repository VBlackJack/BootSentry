using FluentAssertions;
using Moq;
using Xunit;
using Microsoft.Extensions.Logging.Abstractions;
using BootSentry.Actions.Strategies;
using BootSentry.Core.Enums;
using BootSentry.Core.Interfaces;
using BootSentry.Core.Models;

namespace BootSentry.Actions.Tests.Strategies;

public class BrowserExtensionActionStrategyTests
{
    private readonly Mock<IBrowserPolicyManager> _policyManagerMock;
    private readonly BrowserExtensionActionStrategy _strategy;

    public BrowserExtensionActionStrategyTests()
    {
        _policyManagerMock = new Mock<IBrowserPolicyManager>();
        _strategy = new BrowserExtensionActionStrategy(
            NullLogger<BrowserExtensionActionStrategy>.Instance,
            _policyManagerMock.Object);
    }

    [Theory]
    [InlineData("[Brave] uBlock Origin", "Brave")]
    [InlineData("[Opera] uBlock Origin", "Opera")]
    [InlineData("[Opera GX] uBlock Origin", "Opera GX")]
    [InlineData("[Vivaldi] uBlock Origin", "Vivaldi")]
    [InlineData("[Edge] uBlock Origin", "Edge")]
    public async Task DisableAsync_WithKnownBrowser_UsesExpectedPolicyTarget(string displayName, string expectedBrowserName)
    {
        var entry = CreateEntry(displayName);

        var result = await _strategy.DisableAsync(entry);

        result.Success.Should().BeTrue();
        entry.Status.Should().Be(EntryStatus.Disabled);
        _policyManagerMock.Verify(
            p => p.ToggleExtensionState("abcdef123456", expectedBrowserName, false),
            Times.Once);
    }

    [Theory]
    [InlineData("[Brave] uBlock Origin", "Brave")]
    [InlineData("[Opera GX] uBlock Origin", "Opera GX")]
    [InlineData("[Vivaldi] uBlock Origin", "Vivaldi")]
    public async Task EnableAsync_WithKnownBrowser_UsesExpectedPolicyTarget(string displayName, string expectedBrowserName)
    {
        var entry = CreateEntry(displayName);

        var result = await _strategy.EnableAsync(entry);

        result.Success.Should().BeTrue();
        entry.Status.Should().Be(EntryStatus.Enabled);
        _policyManagerMock.Verify(
            p => p.ToggleExtensionState("abcdef123456", expectedBrowserName, true),
            Times.Once);
    }

    [Fact]
    public async Task DisableAsync_WithUnknownBrowser_ReturnsError()
    {
        var entry = CreateEntry("[Unknown] Extension");

        var result = await _strategy.DisableAsync(entry);

        result.Success.Should().BeFalse();
        result.ErrorCode.Should().Be("ERR_UNKNOWN_BROWSER");
        _policyManagerMock.Verify(
            p => p.ToggleExtensionState(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>()),
            Times.Never);
    }

    private static StartupEntry CreateEntry(string displayName)
    {
        return new StartupEntry
        {
            Id = "browser-ext-test",
            Type = EntryType.BrowserExtension,
            Scope = EntryScope.User,
            DisplayName = displayName,
            SourcePath = @"C:\Users\User\AppData\Local\Browser\Extensions\abcdef123456",
            SourceName = "abcdef123456",
            Status = EntryStatus.Enabled
        };
    }
}
