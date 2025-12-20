// Copyright (c) Julien Bombled. All rights reserved.
// Licensed under the Apache 2.0 license. See LICENSE file in the project root for full license information.

using FluentAssertions;
using Moq;
using Xunit;
using Microsoft.Extensions.Logging.Abstractions;
using BootSentry.Actions.Strategies;
using BootSentry.Core.Enums;
using BootSentry.Core.Interfaces;
using BootSentry.Core.Models;
using BootSentry.Backup.Models;

namespace BootSentry.Actions.Tests.Strategies;

public class AppInitActionStrategyTests
{
    private readonly Mock<ITransactionManager> _mockTransactionManager;
    private readonly AppInitActionStrategy _strategy;

    public AppInitActionStrategyTests()
    {
        _mockTransactionManager = new Mock<ITransactionManager>();
        _strategy = new AppInitActionStrategy(
            NullLogger<AppInitActionStrategy>.Instance,
            _mockTransactionManager.Object);
    }

    #region Property Tests

    [Fact]
    public void EntryType_IsAppInitDlls()
    {
        _strategy.EntryType.Should().Be(EntryType.AppInitDlls);
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
    public void RequiresAdmin_AlwaysReturnsTrue()
    {
        var entry = CreateTestEntry();

        _strategy.RequiresAdmin(entry, ActionType.Disable).Should().BeTrue();
        _strategy.RequiresAdmin(entry, ActionType.Enable).Should().BeTrue();
        _strategy.RequiresAdmin(entry, ActionType.Delete).Should().BeTrue();
    }

    #endregion

    #region ParseDllList Tests

    [Fact]
    public void ParseDllList_WithSpaceSeparated_SplitsCorrectly()
    {
        var result = AppInitActionStrategy.ParseDllList("A.dll B.dll C.dll");

        result.Should().HaveCount(3);
        result.Should().ContainInOrder("A.dll", "B.dll", "C.dll");
    }

    [Fact]
    public void ParseDllList_WithCommaSeparated_SplitsCorrectly()
    {
        var result = AppInitActionStrategy.ParseDllList("A.dll,B.dll,C.dll");

        result.Should().HaveCount(3);
        result.Should().ContainInOrder("A.dll", "B.dll", "C.dll");
    }

    [Fact]
    public void ParseDllList_WithSemicolonSeparated_SplitsCorrectly()
    {
        var result = AppInitActionStrategy.ParseDllList("A.dll;B.dll;C.dll");

        result.Should().HaveCount(3);
        result.Should().ContainInOrder("A.dll", "B.dll", "C.dll");
    }

    [Fact]
    public void ParseDllList_WithMixedSeparators_SplitsCorrectly()
    {
        var result = AppInitActionStrategy.ParseDllList("A.dll B.dll,C.dll;D.dll");

        result.Should().HaveCount(4);
        result.Should().ContainInOrder("A.dll", "B.dll", "C.dll", "D.dll");
    }

    [Fact]
    public void ParseDllList_WithEmptyString_ReturnsEmptyList()
    {
        var result = AppInitActionStrategy.ParseDllList("");

        result.Should().BeEmpty();
    }

    [Fact]
    public void ParseDllList_WithNull_ReturnsEmptyList()
    {
        var result = AppInitActionStrategy.ParseDllList(null!);

        result.Should().BeEmpty();
    }

    [Fact]
    public void ParseDllList_WithWhitespaceOnly_ReturnsEmptyList()
    {
        var result = AppInitActionStrategy.ParseDllList("   ");

        result.Should().BeEmpty();
    }

    [Fact]
    public void ParseDllList_TrimsWhitespace()
    {
        var result = AppInitActionStrategy.ParseDllList("  A.dll   B.dll  ");

        result.Should().HaveCount(2);
        result[0].Should().Be("A.dll");
        result[1].Should().Be("B.dll");
    }

    [Fact]
    public void ParseDllList_RemovesDll_PreservesOthers()
    {
        // This tests the logic that would happen in Disable
        var dlls = AppInitActionStrategy.ParseDllList("A.dll B.dll C.dll");
        dlls.RemoveAll(d => string.Equals(d, "B.dll", StringComparison.OrdinalIgnoreCase));
        var result = string.Join(" ", dlls);

        result.Should().Be("A.dll C.dll");
    }

    [Fact]
    public void ParseDllList_RemovesLastDll_SetsEmpty()
    {
        // This tests the logic when removing the last DLL
        var dlls = AppInitActionStrategy.ParseDllList("A.dll");
        dlls.RemoveAll(d => string.Equals(d, "A.dll", StringComparison.OrdinalIgnoreCase));
        var result = string.Join(" ", dlls);

        result.Should().BeEmpty();
    }

    [Fact]
    public void ParseDllList_CaseInsensitiveRemoval()
    {
        var dlls = AppInitActionStrategy.ParseDllList("A.dll B.DLL C.dll");
        dlls.RemoveAll(d => string.Equals(d, "b.dll", StringComparison.OrdinalIgnoreCase));
        var result = string.Join(" ", dlls);

        result.Should().Be("A.dll C.dll");
    }

    #endregion

    #region ParseRegistryPath Tests

    [Fact]
    public void ParseRegistryPath_RemovesHklmPrefix()
    {
        var result = AppInitActionStrategy.ParseRegistryPath(@"HKLM\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Windows");

        result.Should().Be(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Windows");
    }

    [Fact]
    public void ParseRegistryPath_WithoutPrefix_ReturnsAsIs()
    {
        var result = AppInitActionStrategy.ParseRegistryPath(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Windows");

        result.Should().Be(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Windows");
    }

    [Fact]
    public void ParseRegistryPath_CaseInsensitive()
    {
        var result = AppInitActionStrategy.ParseRegistryPath(@"hklm\SOFTWARE\Test");

        result.Should().Be(@"SOFTWARE\Test");
    }

    [Fact]
    public void ParseRegistryPath_WithNull_ReturnsNull()
    {
        var result = AppInitActionStrategy.ParseRegistryPath(null);

        result.Should().BeNull();
    }

    [Fact]
    public void ParseRegistryPath_WithEmpty_ReturnsNull()
    {
        var result = AppInitActionStrategy.ParseRegistryPath("");

        result.Should().BeNull();
    }

    #endregion

    #region Error Handling Tests

    [Fact]
    public async Task DisableAsync_WithWrongEntryType_ReturnsError()
    {
        var entry = new StartupEntry
        {
            Id = "test",
            Type = EntryType.RegistryRun, // Wrong type
            Scope = EntryScope.Machine,
            SourcePath = "Test",
            DisplayName = "Test"
        };

        var result = await _strategy.DisableAsync(entry);

        result.Success.Should().BeFalse();
        result.ErrorCode.Should().Be("ERR_INVALID_TYPE");
    }

    [Fact]
    public async Task DisableAsync_WithNoDllPath_ReturnsError()
    {
        var entry = new StartupEntry
        {
            Id = "test",
            Type = EntryType.AppInitDlls,
            Scope = EntryScope.Machine,
            SourcePath = @"HKLM\SOFTWARE\Test",
            DisplayName = "Test",
            CommandLineRaw = null // No DLL path
        };

        var result = await _strategy.DisableAsync(entry);

        result.Success.Should().BeFalse();
        result.ErrorCode.Should().Be("ERR_NO_DLL_PATH");
    }

    [Fact]
    public async Task DisableAsync_WithInvalidRegistryPath_ReturnsError()
    {
        var entry = new StartupEntry
        {
            Id = "test",
            Type = EntryType.AppInitDlls,
            Scope = EntryScope.Machine,
            DisplayName = "Test",
            CommandLineRaw = "test.dll",
            SourcePath = "" // Invalid path
        };

        var result = await _strategy.DisableAsync(entry);

        result.Success.Should().BeFalse();
        result.ErrorCode.Should().Be("ERR_INVALID_PATH");
    }

    [Fact]
    public async Task EnableAsync_WithWrongEntryType_ReturnsError()
    {
        var entry = new StartupEntry
        {
            Id = "test",
            Type = EntryType.RegistryRun,
            Scope = EntryScope.Machine,
            SourcePath = "Test",
            DisplayName = "Test"
        };

        var result = await _strategy.EnableAsync(entry);

        result.Success.Should().BeFalse();
        result.ErrorCode.Should().Be("ERR_INVALID_TYPE");
    }

    [Fact]
    public async Task DeleteAsync_WithWrongEntryType_ReturnsError()
    {
        var entry = new StartupEntry
        {
            Id = "test",
            Type = EntryType.RegistryRun,
            Scope = EntryScope.Machine,
            SourcePath = "Test",
            DisplayName = "Test"
        };

        var result = await _strategy.DeleteAsync(entry);

        result.Success.Should().BeFalse();
        result.ErrorCode.Should().Be("ERR_INVALID_TYPE");
    }

    #endregion

    private static StartupEntry CreateTestEntry()
    {
        return new StartupEntry
        {
            Id = "test-appinit",
            Type = EntryType.AppInitDlls,
            Scope = EntryScope.Machine,
            DisplayName = "AppInit: test.dll",
            SourcePath = @"HKLM\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Windows",
            SourceName = "AppInit_DLLs",
            CommandLineRaw = "test.dll"
        };
    }
}
