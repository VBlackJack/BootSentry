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

public class WinlogonActionStrategyTests
{
    private readonly Mock<ITransactionManager> _mockTransactionManager;
    private readonly WinlogonActionStrategy _strategy;

    public WinlogonActionStrategyTests()
    {
        _mockTransactionManager = new Mock<ITransactionManager>();
        _strategy = new WinlogonActionStrategy(
            NullLogger<WinlogonActionStrategy>.Instance,
            _mockTransactionManager.Object);
    }

    #region Property Tests

    [Fact]
    public void EntryType_IsWinlogon()
    {
        _strategy.EntryType.Should().Be(EntryType.Winlogon);
    }

    [Fact]
    public void CanDisable_IsFalse()
    {
        // Cannot simply disable Shell/Userinit
        _strategy.CanDisable.Should().BeFalse();
    }

    [Fact]
    public void CanDelete_IsTrue()
    {
        // Delete triggers reset for critical values
        _strategy.CanDelete.Should().BeTrue();
    }

    [Fact]
    public void RequiresAdmin_AlwaysReturnsTrue()
    {
        var entry = CreateTestEntry("Shell", "malware.exe");

        _strategy.RequiresAdmin(entry, ActionType.Delete).Should().BeTrue();
        _strategy.RequiresAdmin(entry, ActionType.Disable).Should().BeTrue();
        _strategy.RequiresAdmin(entry, ActionType.Enable).Should().BeTrue();
    }

    #endregion

    #region Default Values Tests

    [Fact]
    public void DefaultValues_ContainsShell()
    {
        WinlogonActionStrategy.DefaultValues.Should().ContainKey("Shell");
        WinlogonActionStrategy.DefaultValues["Shell"].Should().Be("explorer.exe");
    }

    [Fact]
    public void DefaultValues_ContainsUserinit()
    {
        WinlogonActionStrategy.DefaultValues.Should().ContainKey("Userinit");
        WinlogonActionStrategy.DefaultValues["Userinit"].Should().Be(@"C:\Windows\system32\userinit.exe,");
    }

    [Fact]
    public void DefaultValues_UserinitsHasTrailingComma()
    {
        // The trailing comma is critical for Windows to parse the value correctly
        var userinit = WinlogonActionStrategy.DefaultValues["Userinit"];
        userinit.Should().EndWith(",");
    }

    #endregion

    #region IsCriticalValue Tests

    [Theory]
    [InlineData("Shell", true)]
    [InlineData("shell", true)]  // Case insensitive
    [InlineData("SHELL", true)]
    [InlineData("Userinit", true)]
    [InlineData("userinit", true)]
    [InlineData("USERINIT", true)]
    [InlineData("GinaDLL", false)]
    [InlineData("AppSetup", false)]
    [InlineData("VmApplet", false)]
    [InlineData("", false)]
    [InlineData(null, false)]
    public void IsCriticalValue_ReturnsCorrectResult(string? valueName, bool expected)
    {
        WinlogonActionStrategy.IsCriticalValue(valueName).Should().Be(expected);
    }

    #endregion

    #region GetDefaultValue Tests

    [Fact]
    public void GetDefaultValue_Shell_ReturnsExplorer()
    {
        var result = WinlogonActionStrategy.GetDefaultValue("Shell");
        result.Should().Be("explorer.exe");
    }

    [Fact]
    public void GetDefaultValue_Userinit_ReturnsDefaultWithComma()
    {
        var result = WinlogonActionStrategy.GetDefaultValue("Userinit");
        result.Should().Be(@"C:\Windows\system32\userinit.exe,");
    }

    [Fact]
    public void GetDefaultValue_CaseInsensitive()
    {
        WinlogonActionStrategy.GetDefaultValue("SHELL").Should().Be("explorer.exe");
        WinlogonActionStrategy.GetDefaultValue("shell").Should().Be("explorer.exe");
    }

    [Fact]
    public void GetDefaultValue_NonCritical_ReturnsNull()
    {
        WinlogonActionStrategy.GetDefaultValue("GinaDLL").Should().BeNull();
        WinlogonActionStrategy.GetDefaultValue("AppSetup").Should().BeNull();
    }

    [Fact]
    public void GetDefaultValue_NullOrEmpty_ReturnsNull()
    {
        WinlogonActionStrategy.GetDefaultValue(null).Should().BeNull();
        WinlogonActionStrategy.GetDefaultValue("").Should().BeNull();
    }

    #endregion

    #region Disable/Enable Not Supported Tests

    [Fact]
    public async Task DisableAsync_ReturnsNotSupported()
    {
        var entry = CreateTestEntry("Shell", "explorer.exe");

        var result = await _strategy.DisableAsync(entry);

        result.Success.Should().BeFalse();
        result.ErrorCode.Should().Be("ERR_NOT_SUPPORTED");
    }

    [Fact]
    public async Task EnableAsync_ReturnsNotSupported()
    {
        var entry = CreateTestEntry("Shell", "explorer.exe");

        var result = await _strategy.EnableAsync(entry);

        result.Success.Should().BeFalse();
        result.ErrorCode.Should().Be("ERR_NOT_SUPPORTED");
    }

    #endregion

    #region DeleteAsync Error Tests

    [Fact]
    public async Task DeleteAsync_WithWrongEntryType_ReturnsError()
    {
        var entry = new StartupEntry
        {
            Id = "test",
            Type = EntryType.RegistryRun, // Wrong type
            Scope = EntryScope.Machine,
            SourcePath = "Test",
            DisplayName = "Test"
        };

        var result = await _strategy.DeleteAsync(entry);

        result.Success.Should().BeFalse();
        result.ErrorCode.Should().Be("ERR_INVALID_TYPE");
    }

    [Fact]
    public async Task DeleteAsync_WithNoValueName_ReturnsError()
    {
        var entry = new StartupEntry
        {
            Id = "test",
            Type = EntryType.Winlogon,
            Scope = EntryScope.Machine,
            SourcePath = @"HKLM\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon",
            DisplayName = "Test",
            SourceName = null // No value name
        };

        var result = await _strategy.DeleteAsync(entry);

        result.Success.Should().BeFalse();
        result.ErrorCode.Should().Be("ERR_NO_SOURCE_NAME");
    }

    #endregion

    #region Reset Logic Verification

    [Fact]
    public void ResetLogic_ShellShouldBeResetNotDeleted()
    {
        // Verify that Shell is in critical values list
        // This ensures the delete logic will reset, not delete
        WinlogonActionStrategy.IsCriticalValue("Shell").Should().BeTrue();
        WinlogonActionStrategy.GetDefaultValue("Shell").Should().Be("explorer.exe");
    }

    [Fact]
    public void ResetLogic_UserinitShouldBeResetNotDeleted()
    {
        // Verify that Userinit is in critical values list
        WinlogonActionStrategy.IsCriticalValue("Userinit").Should().BeTrue();

        var defaultValue = WinlogonActionStrategy.GetDefaultValue("Userinit");
        defaultValue.Should().NotBeNull();
        defaultValue.Should().Contain("userinit.exe");
        defaultValue.Should().EndWith(","); // Critical trailing comma
    }

    [Fact]
    public void ResetLogic_GinaDllShouldBeActuallyDeleted()
    {
        // Verify that GinaDLL is NOT in critical values list
        // This ensures delete will actually delete
        WinlogonActionStrategy.IsCriticalValue("GinaDLL").Should().BeFalse();
        WinlogonActionStrategy.GetDefaultValue("GinaDLL").Should().BeNull();
    }

    [Fact]
    public void ResetLogic_AppSetupShouldBeActuallyDeleted()
    {
        // Verify that AppSetup is NOT in critical values list
        WinlogonActionStrategy.IsCriticalValue("AppSetup").Should().BeFalse();
        WinlogonActionStrategy.GetDefaultValue("AppSetup").Should().BeNull();
    }

    #endregion

    private static StartupEntry CreateTestEntry(string valueName, string value)
    {
        return new StartupEntry
        {
            Id = $"test-winlogon-{valueName}",
            Type = EntryType.Winlogon,
            Scope = EntryScope.Machine,
            DisplayName = $"Winlogon: {valueName}",
            SourcePath = @"HKLM\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon",
            SourceName = valueName,
            CommandLineRaw = value
        };
    }
}
