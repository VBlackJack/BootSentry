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

using System.IO.Compression;
using System.Text.Json;
using BootSentry.Core.Enums;
using BootSentry.Core.Models;
using BootSentry.Core.Services;
using FluentAssertions;
using Xunit;

namespace BootSentry.Core.Tests.Services;

public class ExportServiceTests : IDisposable
{
    private readonly ExportService _service = new();
    private readonly string _testDirectory;

    public ExportServiceTests()
    {
        _testDirectory = Path.Combine(Path.GetTempPath(), $"BootSentryExportTest_{Guid.NewGuid():N}");
        Directory.CreateDirectory(_testDirectory);
    }

    public void Dispose()
    {
        if (Directory.Exists(_testDirectory))
        {
            try
            {
                Directory.Delete(_testDirectory, recursive: true);
            }
            catch
            {
                // Ignore cleanup failures in tests.
            }
        }
    }

    // ============================================================
    // CSV Formula Neutralization Tests
    // ============================================================

    [Theory]
    [InlineData("=HYPERLINK(\"http://bad\")")]
    [InlineData("+SUM(11)")]
    [InlineData("-2+3")]
    [InlineData("@cmd")]
    public void ExportToCsv_WhenFieldLooksLikeFormula_PrefixesApostrophe(string displayName)
    {
        var entries = new[] { CreateEntry(displayName) };

        var csv = _service.ExportToCsv(entries, new ExportOptions());
        var firstDataCell = GetFirstDataCell(csv);

        firstDataCell.Should().StartWith("'");
        firstDataCell.Should().Contain(displayName);
    }

    [Fact]
    public void ExportToCsv_WhenFieldIsRegularText_DoesNotPrefixApostrophe()
    {
        var entries = new[] { CreateEntry("BootSentry Agent") };

        var csv = _service.ExportToCsv(entries, new ExportOptions());
        var firstDataCell = GetFirstDataCell(csv);

        firstDataCell.Should().Be("BootSentry Agent");
    }

    // ============================================================
    // CSV Basic Export Tests
    // ============================================================

    [Fact]
    public void ExportToCsv_WithDefaultOptions_ContainsHeaders()
    {
        var entries = new[] { CreateEntry("TestApp") };

        var csv = _service.ExportToCsv(entries, new ExportOptions());

        using var reader = new StringReader(csv);
        var headerLine = reader.ReadLine();

        headerLine.Should().NotBeNull();
        headerLine.Should().Contain("DisplayName");
        headerLine.Should().Contain("Type");
        headerLine.Should().Contain("Scope");
        headerLine.Should().Contain("Status");
        headerLine.Should().Contain("Publisher");
        headerLine.Should().Contain("SignatureStatus");
        headerLine.Should().Contain("RiskLevel");
        headerLine.Should().Contain("TargetPath");
    }

    [Fact]
    public void ExportToCsv_WithSingleEntry_ContainsOneDataRow()
    {
        var entries = new[] { CreateEntry("TestApp") };

        var csv = _service.ExportToCsv(entries, new ExportOptions());
        var lines = csv.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);

        // Header + 1 data row
        lines.Should().HaveCount(2);
    }

    [Fact]
    public void ExportToCsv_WithMultipleEntries_ContainsAllDataRows()
    {
        var entries = new[]
        {
            CreateEntry("App1"),
            CreateEntry("App2"),
            CreateEntry("App3")
        };

        var csv = _service.ExportToCsv(entries, new ExportOptions());
        var lines = csv.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);

        // Header + 3 data rows
        lines.Should().HaveCount(4);
    }

    [Fact]
    public void ExportToCsv_WithEmptyEntryList_ContainsOnlyHeaders()
    {
        var entries = Array.Empty<StartupEntry>();

        var csv = _service.ExportToCsv(entries, new ExportOptions());
        var lines = csv.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);

        lines.Should().HaveCount(1); // Header only
    }

    // ============================================================
    // CSV with Knowledge Info Tests
    // ============================================================

    [Fact]
    public void ExportToCsv_WithIncludeKnowledgeInfo_AddsKnowledgeColumns()
    {
        var entries = new[] { CreateEntry("TestApp") };
        var options = new ExportOptions { IncludeKnowledgeInfo = true };

        var csv = _service.ExportToCsv(entries, options);

        using var reader = new StringReader(csv);
        var headerLine = reader.ReadLine();

        headerLine.Should().Contain("HasDescription");
        headerLine.Should().Contain("KnowledgeMatch");
        headerLine.Should().Contain("KnowledgeDescription");
    }

    [Fact]
    public void ExportToCsv_WithoutIncludeKnowledgeInfo_DoesNotAddKnowledgeColumns()
    {
        var entries = new[] { CreateEntry("TestApp") };
        var options = new ExportOptions { IncludeKnowledgeInfo = false };

        var csv = _service.ExportToCsv(entries, options);

        using var reader = new StringReader(csv);
        var headerLine = reader.ReadLine();

        headerLine.Should().NotContain("HasDescription");
        headerLine.Should().NotContain("KnowledgeMatch");
    }

    // ============================================================
    // CSV with Details Tests
    // ============================================================

    [Fact]
    public void ExportToCsv_WithIncludeDetails_AddsDetailColumns()
    {
        var entries = new[] { CreateEntry("TestApp") };
        var options = new ExportOptions { IncludeDetails = true };

        var csv = _service.ExportToCsv(entries, options);

        using var reader = new StringReader(csv);
        var headerLine = reader.ReadLine();

        headerLine.Should().Contain("CommandLine");
        headerLine.Should().Contain("Arguments");
        headerLine.Should().Contain("FileVersion");
        headerLine.Should().Contain("FileSize");
        headerLine.Should().Contain("LastModified");
    }

    [Fact]
    public void ExportToCsv_WithoutIncludeDetails_DoesNotAddDetailColumns()
    {
        var entries = new[] { CreateEntry("TestApp") };
        var options = new ExportOptions { IncludeDetails = false };

        var csv = _service.ExportToCsv(entries, options);

        using var reader = new StringReader(csv);
        var headerLine = reader.ReadLine();

        headerLine.Should().NotContain("Arguments");
        headerLine.Should().NotContain("FileVersion");
        headerLine.Should().NotContain("FileSize");
    }

    // ============================================================
    // JSON Export Tests
    // ============================================================

    [Fact]
    public void ExportToJson_WithDefaultOptions_ReturnsValidJson()
    {
        var entries = new[] { CreateEntry("TestApp") };

        var json = _service.ExportToJson(entries, new ExportOptions());

        var action = () => JsonDocument.Parse(json);
        action.Should().NotThrow();
    }

    [Fact]
    public void ExportToJson_WithDefaultOptions_ContainsExpectedStructure()
    {
        var entries = new[] { CreateEntry("TestApp") };

        var json = _service.ExportToJson(entries, new ExportOptions());
        using var doc = JsonDocument.Parse(json);

        doc.RootElement.TryGetProperty("ExportDate", out _).Should().BeTrue();
        doc.RootElement.TryGetProperty("MachineName", out _).Should().BeTrue();
        doc.RootElement.TryGetProperty("UserName", out _).Should().BeTrue();
        doc.RootElement.TryGetProperty("TotalEntries", out _).Should().BeTrue();
        doc.RootElement.TryGetProperty("Entries", out _).Should().BeTrue();
    }

    [Fact]
    public void ExportToJson_WithDefaultOptions_TotalEntriesMatchesCount()
    {
        var entries = new[]
        {
            CreateEntry("App1"),
            CreateEntry("App2"),
            CreateEntry("App3")
        };

        var json = _service.ExportToJson(entries, new ExportOptions());
        using var doc = JsonDocument.Parse(json);

        doc.RootElement.GetProperty("TotalEntries").GetInt32().Should().Be(3);
        doc.RootElement.GetProperty("Entries").GetArrayLength().Should().Be(3);
    }

    // ============================================================
    // JSON Anonymization Tests
    // ============================================================

    [Fact]
    public void ExportToJson_WithAnonymize_MasksUserAndMachineInfo()
    {
        var entries = new[] { CreateEntry("TestApp") };
        var options = new ExportOptions { Anonymize = true };

        var json = _service.ExportToJson(entries, options);
        using var doc = JsonDocument.Parse(json);

        doc.RootElement.GetProperty("MachineName").GetString().Should().Be("[MACHINE]");
        doc.RootElement.GetProperty("UserName").GetString().Should().Be("[USER]");
    }

    [Fact]
    public void ExportToJson_WithoutAnonymize_ShowsRealMachineAndUserInfo()
    {
        var entries = new[] { CreateEntry("TestApp") };
        var options = new ExportOptions { Anonymize = false };

        var json = _service.ExportToJson(entries, options);
        using var doc = JsonDocument.Parse(json);

        doc.RootElement.GetProperty("MachineName").GetString().Should().Be(Environment.MachineName);
        doc.RootElement.GetProperty("UserName").GetString().Should().Be(Environment.UserName);
    }

    [Fact]
    public void ExportToJson_WithAnonymize_MasksPathsInEntries()
    {
        var userProfile = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        var entry = CreateEntryWithPaths(
            displayName: "TestApp",
            targetPath: Path.Combine(userProfile, "AppData", "Local", "app.exe"),
            sourcePath: @"HKCU\Software\Microsoft\Windows\CurrentVersion\Run",
            commandLineRaw: Path.Combine(userProfile, "AppData", "Local", "app.exe") + " --startup");

        var entries = new[] { entry };
        var options = new ExportOptions { Anonymize = true };

        var json = _service.ExportToJson(entries, options);

        json.Should().Contain("[USER_PROFILE]");
        json.Should().NotContain(userProfile);
    }

    [Fact]
    public void ExportToJson_WithoutAnonymize_PreservesOriginalPaths()
    {
        var targetPath = @"C:\SomeApp\app.exe";
        var entry = CreateEntryWithPaths(
            displayName: "TestApp",
            targetPath: targetPath,
            sourcePath: @"HKCU\Software\Microsoft\Windows\CurrentVersion\Run",
            commandLineRaw: targetPath + " --startup");

        var entries = new[] { entry };
        var options = new ExportOptions { Anonymize = false };

        var json = _service.ExportToJson(entries, options);

        // JSON escapes backslashes, so check for the escaped form
        json.Should().Contain(targetPath.Replace(@"\", @"\\"));
    }

    // ============================================================
    // JSON with Details and Hashes Tests
    // ============================================================

    [Fact]
    public void ExportToJson_WithIncludeDetails_AddsExtraFields()
    {
        var entry = CreateEntryWithDetails();
        var entries = new[] { entry };
        var options = new ExportOptions { IncludeDetails = true };

        var json = _service.ExportToJson(entries, options);
        using var doc = JsonDocument.Parse(json);

        var firstEntry = doc.RootElement.GetProperty("Entries")[0];
        firstEntry.TryGetProperty("Arguments", out _).Should().BeTrue();
        firstEntry.TryGetProperty("FileVersion", out _).Should().BeTrue();
        firstEntry.TryGetProperty("FileSize", out _).Should().BeTrue();
    }

    [Fact]
    public void ExportToJson_WithoutIncludeDetails_OmitsExtraFields()
    {
        var entry = CreateEntryWithDetails();
        var entries = new[] { entry };
        var options = new ExportOptions { IncludeDetails = false };

        var json = _service.ExportToJson(entries, options);
        using var doc = JsonDocument.Parse(json);

        var firstEntry = doc.RootElement.GetProperty("Entries")[0];
        firstEntry.TryGetProperty("Arguments", out _).Should().BeFalse();
        firstEntry.TryGetProperty("FileVersion", out _).Should().BeFalse();
    }

    [Fact]
    public void ExportToJson_WithIncludeHashes_AddsSha256Field()
    {
        var entry = CreateEntry("TestApp");
        entry.Sha256 = "abc123def456";
        var entries = new[] { entry };
        var options = new ExportOptions { IncludeHashes = true };

        var json = _service.ExportToJson(entries, options);
        using var doc = JsonDocument.Parse(json);

        var firstEntry = doc.RootElement.GetProperty("Entries")[0];
        firstEntry.TryGetProperty("Sha256", out var sha).Should().BeTrue();
        sha.GetString().Should().Be("abc123def456");
    }

    [Fact]
    public void ExportToJson_WithoutIncludeHashes_OmitsSha256Field()
    {
        var entry = CreateEntry("TestApp");
        entry.Sha256 = "abc123def456";
        var entries = new[] { entry };
        var options = new ExportOptions { IncludeHashes = false };

        var json = _service.ExportToJson(entries, options);
        using var doc = JsonDocument.Parse(json);

        var firstEntry = doc.RootElement.GetProperty("Entries")[0];
        firstEntry.TryGetProperty("Sha256", out _).Should().BeFalse();
    }

    // ============================================================
    // ExportToFileAsync Tests
    // ============================================================

    [Fact]
    public async Task ExportToFileAsync_WithJsonFormat_WritesValidJsonFile()
    {
        var entries = new[] { CreateEntry("TestApp") };
        var filePath = Path.Combine(_testDirectory, "export.json");

        await _service.ExportToFileAsync(entries, filePath, ExportFormat.Json, new ExportOptions());

        File.Exists(filePath).Should().BeTrue();
        var content = await File.ReadAllTextAsync(filePath);
        var action = () => JsonDocument.Parse(content);
        action.Should().NotThrow();
    }

    [Fact]
    public async Task ExportToFileAsync_WithCsvFormat_WritesValidCsvFile()
    {
        var entries = new[] { CreateEntry("TestApp") };
        var filePath = Path.Combine(_testDirectory, "export.csv");

        await _service.ExportToFileAsync(entries, filePath, ExportFormat.Csv, new ExportOptions());

        File.Exists(filePath).Should().BeTrue();
        var content = await File.ReadAllTextAsync(filePath);
        content.Should().Contain("DisplayName");
    }

    [Fact]
    public async Task ExportToFileAsync_ContentMatchesInMemoryExport()
    {
        var entries = new[] { CreateEntry("TestApp") };
        var filePath = Path.Combine(_testDirectory, "export.json");
        var options = new ExportOptions();

        await _service.ExportToFileAsync(entries, filePath, ExportFormat.Json, options);

        var fileContent = await File.ReadAllTextAsync(filePath);
        // Both should produce valid JSON with matching entry count
        using var doc = JsonDocument.Parse(fileContent);
        doc.RootElement.GetProperty("TotalEntries").GetInt32().Should().Be(1);
    }

    [Fact]
    public async Task ExportToFileAsync_WithCancellation_ThrowsOperationCanceledException()
    {
        var entries = new[] { CreateEntry("TestApp") };
        var filePath = Path.Combine(_testDirectory, "cancelled.json");
        using var cts = new CancellationTokenSource();
        cts.Cancel();

        var action = async () => await _service.ExportToFileAsync(
            entries, filePath, ExportFormat.Json, new ExportOptions(), cts.Token);

        await action.Should().ThrowAsync<OperationCanceledException>();
    }

    // ============================================================
    // ExportDiagnosticsZipAsync Tests
    // ============================================================

    [Fact]
    public async Task ExportDiagnosticsZipAsync_CreatesZipFile()
    {
        var entries = new[] { CreateEntry("TestApp") };
        var zipPath = Path.Combine(_testDirectory, "diagnostics.zip");

        await _service.ExportDiagnosticsZipAsync(entries, zipPath);

        File.Exists(zipPath).Should().BeTrue();
    }

    [Fact]
    public async Task ExportDiagnosticsZipAsync_ZipContainsEntriesJson()
    {
        var entries = new[] { CreateEntry("TestApp") };
        var zipPath = Path.Combine(_testDirectory, "diagnostics.zip");

        await _service.ExportDiagnosticsZipAsync(entries, zipPath);

        using var zip = ZipFile.OpenRead(zipPath);
        zip.Entries.Should().Contain(e => e.FullName == "entries.json");
    }

    [Fact]
    public async Task ExportDiagnosticsZipAsync_ZipContainsSystemInfo()
    {
        var entries = new[] { CreateEntry("TestApp") };
        var zipPath = Path.Combine(_testDirectory, "diagnostics.zip");

        await _service.ExportDiagnosticsZipAsync(entries, zipPath);

        using var zip = ZipFile.OpenRead(zipPath);
        zip.Entries.Should().Contain(e => e.FullName == "system_info.txt");
    }

    [Fact]
    public async Task ExportDiagnosticsZipAsync_EntriesAreAnonymized()
    {
        var entries = new[] { CreateEntry("TestApp") };
        var zipPath = Path.Combine(_testDirectory, "diagnostics.zip");

        await _service.ExportDiagnosticsZipAsync(entries, zipPath);

        using var zip = ZipFile.OpenRead(zipPath);
        var entriesEntry = zip.GetEntry("entries.json");
        entriesEntry.Should().NotBeNull();

        using var stream = entriesEntry!.Open();
        using var reader = new StreamReader(stream);
        var json = await reader.ReadToEndAsync();
        using var doc = JsonDocument.Parse(json);

        doc.RootElement.GetProperty("MachineName").GetString().Should().Be("[MACHINE]");
        doc.RootElement.GetProperty("UserName").GetString().Should().Be("[USER]");
    }

    // ============================================================
    // CSV Anonymization Tests
    // ============================================================

    [Fact]
    public void ExportToCsv_WithAnonymize_MasksUserProfilePaths()
    {
        var userProfile = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        var entry = CreateEntryWithPaths(
            displayName: "TestApp",
            targetPath: Path.Combine(userProfile, "AppData", "Local", "app.exe"),
            sourcePath: @"HKCU\Software\Microsoft\Windows\CurrentVersion\Run",
            commandLineRaw: null);

        var csv = _service.ExportToCsv(new[] { entry }, new ExportOptions { Anonymize = true });

        csv.Should().Contain("[USER_PROFILE]");
        csv.Should().NotContain(userProfile);
    }

    // ============================================================
    // Knowledge Finder Integration Tests
    // ============================================================

    [Fact]
    public void ExportToJson_WithKnowledgeFinder_IncludesKnowledgeData()
    {
        KnowledgeFinder finder = (name, executable, publisher) =>
        {
            return new { Name = "KnownApp", ShortDescription = "A well-known application" };
        };

        var service = new ExportService(finder);
        var entries = new[] { CreateEntry("TestApp") };
        var options = new ExportOptions { IncludeKnowledgeInfo = true };

        var json = service.ExportToJson(entries, options);
        using var doc = JsonDocument.Parse(json);

        var firstEntry = doc.RootElement.GetProperty("Entries")[0];
        firstEntry.GetProperty("HasKnowledgeEntry").GetBoolean().Should().BeTrue();
        firstEntry.GetProperty("KnowledgeMatch").GetString().Should().Be("KnownApp");
        firstEntry.GetProperty("KnowledgeDescription").GetString().Should().Be("A well-known application");
    }

    [Fact]
    public void ExportToJson_WithNullKnowledgeFinder_SetsHasKnowledgeEntryFalse()
    {
        var service = new ExportService();
        var entries = new[] { CreateEntry("TestApp") };
        var options = new ExportOptions { IncludeKnowledgeInfo = true };

        var json = service.ExportToJson(entries, options);
        using var doc = JsonDocument.Parse(json);

        var firstEntry = doc.RootElement.GetProperty("Entries")[0];
        firstEntry.GetProperty("HasKnowledgeEntry").GetBoolean().Should().BeFalse();
    }

    [Fact]
    public void ExportToCsv_WithKnowledgeFinder_IncludesKnowledgeData()
    {
        KnowledgeFinder finder = (name, executable, publisher) =>
        {
            return new { Name = "KnownApp", ShortDescription = "A well-known application" };
        };

        var service = new ExportService(finder);
        var entries = new[] { CreateEntry("TestApp") };
        var options = new ExportOptions { IncludeKnowledgeInfo = true };

        var csv = service.ExportToCsv(entries, options);

        csv.Should().Contain("Yes"); // HasDescription = Yes
        csv.Should().Contain("KnownApp");
        csv.Should().Contain("A well-known application");
    }

    // ============================================================
    // Helper Methods
    // ============================================================

    private static StartupEntry CreateEntry(string displayName)
    {
        return new StartupEntry
        {
            Id = Guid.NewGuid().ToString("N"),
            Type = EntryType.RegistryRun,
            Scope = EntryScope.User,
            DisplayName = displayName,
            SourcePath = @"HKCU\Software\Microsoft\Windows\CurrentVersion\Run",
            SourceName = "Test",
            Status = EntryStatus.Enabled,
            SignatureStatus = SignatureStatus.Unknown,
            RiskLevel = RiskLevel.Unknown,
            FileExists = false
        };
    }

    private static StartupEntry CreateEntryWithPaths(
        string displayName,
        string? targetPath,
        string sourcePath,
        string? commandLineRaw)
    {
        return new StartupEntry
        {
            Id = Guid.NewGuid().ToString("N"),
            Type = EntryType.RegistryRun,
            Scope = EntryScope.User,
            DisplayName = displayName,
            SourcePath = sourcePath,
            SourceName = "Test",
            TargetPath = targetPath,
            CommandLineRaw = commandLineRaw,
            Status = EntryStatus.Enabled,
            SignatureStatus = SignatureStatus.Unknown,
            RiskLevel = RiskLevel.Unknown,
            FileExists = false
        };
    }

    private static StartupEntry CreateEntryWithDetails()
    {
        return new StartupEntry
        {
            Id = Guid.NewGuid().ToString("N"),
            Type = EntryType.RegistryRun,
            Scope = EntryScope.User,
            DisplayName = "DetailedApp",
            SourcePath = @"HKCU\Software\Microsoft\Windows\CurrentVersion\Run",
            SourceName = "DetailedApp",
            TargetPath = @"C:\Program Files\DetailedApp\app.exe",
            CommandLineRaw = @"C:\Program Files\DetailedApp\app.exe --start",
            Arguments = "--start",
            FileVersion = "1.2.3.4",
            FileSize = 12345678,
            LastModified = new DateTime(2025, 1, 15, 10, 30, 0, DateTimeKind.Utc),
            Status = EntryStatus.Enabled,
            SignatureStatus = SignatureStatus.SignedTrusted,
            RiskLevel = RiskLevel.Safe,
            FileExists = true,
            Publisher = "Test Publisher Inc."
        };
    }

    private static string GetFirstDataCell(string csv)
    {
        using var reader = new StringReader(csv);
        _ = reader.ReadLine(); // Header
        var dataLine = reader.ReadLine();
        dataLine.Should().NotBeNull();

        var raw = dataLine!.Split(',')[0];

        // Strip CSV quoting if the field was wrapped in double quotes
        if (raw.Length >= 2 && raw[0] == '"' && raw[^1] == '"')
        {
            raw = raw[1..^1].Replace("\"\"", "\"");
        }

        return raw;
    }
}
