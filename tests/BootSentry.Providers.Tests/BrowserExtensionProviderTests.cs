// Copyright (c) Julien Bombled. All rights reserved.
// Licensed under the Apache 2.0 license. See LICENSE file in the project root for full license information.

using FluentAssertions;
using Moq;
using Xunit;
using Microsoft.Extensions.Logging.Abstractions;
using BootSentry.Core.Enums;
using BootSentry.Core.Interfaces;

namespace BootSentry.Providers.Tests;

public class BrowserExtensionProviderTests
{
    private readonly Mock<IBrowserPolicyManager> _mockPolicyManager;
    private readonly BrowserExtensionProvider _provider;
    private readonly BrowserExtensionProvider _providerWithoutPolicy;

    public BrowserExtensionProviderTests()
    {
        _mockPolicyManager = new Mock<IBrowserPolicyManager>();
        _provider = new BrowserExtensionProvider(
            NullLogger<BrowserExtensionProvider>.Instance,
            _mockPolicyManager.Object);
        _providerWithoutPolicy = new BrowserExtensionProvider(
            NullLogger<BrowserExtensionProvider>.Instance,
            policyManager: null);
    }

    [Fact]
    public void EntryType_IsBrowserExtension()
    {
        _provider.EntryType.Should().Be(EntryType.BrowserExtension);
    }

    [Fact]
    public void DisplayName_IsNotEmpty()
    {
        _provider.DisplayName.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public void RequiresAdminToRead_IsFalse()
    {
        _provider.RequiresAdminToRead.Should().BeFalse();
    }

    [Fact]
    public void RequiresAdminToModify_IsFalse()
    {
        // Note: Modifying via policy requires admin, but the provider itself doesn't
        _provider.RequiresAdminToModify.Should().BeFalse();
    }

    [Fact]
    public void IsAvailable_ReturnsTrue()
    {
        _provider.IsAvailable().Should().BeTrue();
    }

    [Fact]
    public async Task ScanAsync_ReturnsEntries()
    {
        // Integration test - scans actual browser extensions
        var entries = await _provider.ScanAsync();

        entries.Should().NotBeNull();
    }

    [Fact]
    public async Task ScanAsync_EntriesHaveRequiredProperties()
    {
        var entries = await _provider.ScanAsync();

        foreach (var entry in entries)
        {
            entry.Id.Should().NotBeNullOrEmpty();
            entry.DisplayName.Should().NotBeNullOrEmpty();
            entry.SourcePath.Should().NotBeNullOrEmpty();
            entry.SourceName.Should().NotBeNullOrEmpty("Extension ID should be stored in SourceName");
            entry.Type.Should().Be(EntryType.BrowserExtension);
            entry.Scope.Should().Be(EntryScope.User);
        }
    }

    [Fact]
    public async Task ScanAsync_EntriesHaveIds()
    {
        var entries = await _provider.ScanAsync();

        foreach (var entry in entries)
        {
            entry.Id.Should().NotBeNullOrEmpty();
        }

        entries.Select(e => e.Id).Should().OnlyHaveUniqueItems();
    }

    [Fact]
    public async Task ScanAsync_DisplayNameContainsBrowserName()
    {
        var entries = await _provider.ScanAsync();

        foreach (var entry in entries)
        {
            // DisplayName should be in format "[BrowserName] ExtensionName"
            entry.DisplayName.Should().MatchRegex(@"^\[.+\] .+$");
        }
    }

    [Fact]
    public async Task ScanAsync_WhenPolicyBlocksExtension_StatusShouldBeDisabled()
    {
        // Arrange: Setup mock to block ALL extensions
        _mockPolicyManager
            .Setup(m => m.IsExtensionBlocked(It.IsAny<string>(), It.IsAny<string>()))
            .Returns(true);

        // Act
        var entries = await _provider.ScanAsync();

        // Assert: All entries should be Disabled when policy blocks them
        foreach (var entry in entries)
        {
            entry.Status.Should().Be(EntryStatus.Disabled,
                $"Extension {entry.DisplayName} should be Disabled when policy blocks it");
        }
    }

    [Fact]
    public async Task ScanAsync_WhenPolicyDoesNotBlock_StatusShouldBeEnabled()
    {
        // Arrange: Setup mock to NOT block any extensions
        _mockPolicyManager
            .Setup(m => m.IsExtensionBlocked(It.IsAny<string>(), It.IsAny<string>()))
            .Returns(false);

        // Act
        var entries = await _provider.ScanAsync();

        // Assert: Chromium extensions should be Enabled (Firefox depends on JSON)
        var chromiumEntries = entries.Where(e =>
            e.DisplayName.StartsWith("[Chrome]") ||
            e.DisplayName.StartsWith("[Edge]") ||
            e.DisplayName.StartsWith("[Brave]") ||
            e.DisplayName.StartsWith("[Opera]") ||
            e.DisplayName.StartsWith("[Vivaldi]"));

        foreach (var entry in chromiumEntries)
        {
            entry.Status.Should().Be(EntryStatus.Enabled,
                $"Chromium extension {entry.DisplayName} should be Enabled when policy doesn't block it");
        }
    }

    [Fact]
    public async Task ScanAsync_WhenNoPolicyManager_ExtensionsShouldBeEnabled()
    {
        // Act: Use provider without policy manager
        var entries = await _providerWithoutPolicy.ScanAsync();

        // Assert: Chromium extensions should default to Enabled
        var chromiumEntries = entries.Where(e =>
            e.DisplayName.StartsWith("[Chrome]") ||
            e.DisplayName.StartsWith("[Edge]") ||
            e.DisplayName.StartsWith("[Brave]") ||
            e.DisplayName.StartsWith("[Opera]") ||
            e.DisplayName.StartsWith("[Vivaldi]"));

        foreach (var entry in chromiumEntries)
        {
            entry.Status.Should().Be(EntryStatus.Enabled,
                $"Extension {entry.DisplayName} should be Enabled when no policy manager is provided");
        }
    }

    [Fact]
    public async Task ScanAsync_PolicyManagerIsCalledForEachExtension()
    {
        // Arrange
        _mockPolicyManager
            .Setup(m => m.IsExtensionBlocked(It.IsAny<string>(), It.IsAny<string>()))
            .Returns(false);

        // Act
        var entries = await _provider.ScanAsync();

        // Assert: Verify IsExtensionBlocked was called for each entry
        if (entries.Any())
        {
            _mockPolicyManager.Verify(
                m => m.IsExtensionBlocked(It.IsAny<string>(), It.IsAny<string>()),
                Times.AtLeast(entries.Count));
        }
    }

    [Fact]
    public async Task ScanAsync_PolicyManagerReceivesCorrectBrowserName()
    {
        // Arrange: Capture the browser names passed to the mock
        var browserNames = new List<string>();
        _mockPolicyManager
            .Setup(m => m.IsExtensionBlocked(It.IsAny<string>(), It.IsAny<string>()))
            .Callback<string, string>((id, browser) => browserNames.Add(browser))
            .Returns(false);

        // Act
        await _provider.ScanAsync();

        // Assert: Browser names should be valid
        var validBrowserNames = new[] { "Chrome", "Edge", "Firefox", "Brave", "Opera", "Opera GX", "Vivaldi" };
        foreach (var name in browserNames)
        {
            validBrowserNames.Should().Contain(name,
                $"Browser name '{name}' should be one of the supported browsers");
        }
    }

    [Fact]
    public async Task ScanAsync_WithCancellation_CompletesWithoutError()
    {
        // Note: BrowserExtensionProvider doesn't currently check cancellation token
        // This test verifies the method accepts the token without crashing
        using var cts = new CancellationTokenSource();

        var entries = await _provider.ScanAsync(cts.Token);

        entries.Should().NotBeNull();
    }
}
