using FluentAssertions;
using Xunit;
using BootSentry.Core.Enums;
using BootSentry.Core.Models;

namespace BootSentry.Core.Tests.Models;

public class StartupEntryTests
{
    [Fact]
    public void GenerateId_WithValidInputs_CreatesConsistentId()
    {
        var id1 = StartupEntry.GenerateId(
            EntryType.RegistryRun,
            EntryScope.User,
            @"HKCU\Software\Microsoft\Windows\CurrentVersion\Run",
            "TestApp");

        var id2 = StartupEntry.GenerateId(
            EntryType.RegistryRun,
            EntryScope.User,
            @"HKCU\Software\Microsoft\Windows\CurrentVersion\Run",
            "TestApp");

        id1.Should().Be(id2);
    }

    [Fact]
    public void GenerateId_IsCaseInsensitive()
    {
        var id1 = StartupEntry.GenerateId(
            EntryType.RegistryRun,
            EntryScope.User,
            @"HKCU\Software\Microsoft\Windows\CurrentVersion\Run",
            "TestApp");

        var id2 = StartupEntry.GenerateId(
            EntryType.RegistryRun,
            EntryScope.User,
            @"hkcu\software\microsoft\windows\currentversion\run",
            "testapp");

        id1.Should().Be(id2);
    }

    [Fact]
    public void GenerateId_NormalizesSlashes()
    {
        var id1 = StartupEntry.GenerateId(
            EntryType.RegistryRun,
            EntryScope.User,
            @"HKCU\Software\Microsoft",
            "Test");

        var id2 = StartupEntry.GenerateId(
            EntryType.RegistryRun,
            EntryScope.User,
            @"HKCU/Software/Microsoft",
            "Test");

        id1.Should().Be(id2);
    }

    [Fact]
    public void GenerateId_TrimsTrailingSlashes()
    {
        var id1 = StartupEntry.GenerateId(
            EntryType.RegistryRun,
            EntryScope.User,
            @"HKCU\Software\Microsoft",
            "Test");

        var id2 = StartupEntry.GenerateId(
            EntryType.RegistryRun,
            EntryScope.User,
            @"HKCU\Software\Microsoft\",
            "Test");

        id1.Should().Be(id2);
    }

    [Fact]
    public void GenerateId_DifferentTypes_CreatesDifferentIds()
    {
        var id1 = StartupEntry.GenerateId(
            EntryType.RegistryRun,
            EntryScope.User,
            @"Path",
            "Name");

        var id2 = StartupEntry.GenerateId(
            EntryType.RegistryRunOnce,
            EntryScope.User,
            @"Path",
            "Name");

        id1.Should().NotBe(id2);
    }

    [Fact]
    public void GenerateId_DifferentScopes_CreatesDifferentIds()
    {
        var id1 = StartupEntry.GenerateId(
            EntryType.RegistryRun,
            EntryScope.User,
            @"Path",
            "Name");

        var id2 = StartupEntry.GenerateId(
            EntryType.RegistryRun,
            EntryScope.Machine,
            @"Path",
            "Name");

        id1.Should().NotBe(id2);
    }

    [Fact]
    public void GenerateId_WithNullName_UsesEmptyString()
    {
        var id = StartupEntry.GenerateId(
            EntryType.RegistryRun,
            EntryScope.User,
            @"HKCU\Path",
            null);

        id.Should().EndWith(":");
    }

    [Fact]
    public void StartupEntry_CanBeCreated_WithRequiredProperties()
    {
        var entry = new StartupEntry
        {
            Id = "test-id",
            Type = EntryType.RegistryRun,
            Scope = EntryScope.User,
            DisplayName = "Test App",
            SourcePath = @"HKCU\Software\Test"
        };

        entry.Id.Should().Be("test-id");
        entry.Type.Should().Be(EntryType.RegistryRun);
        entry.Scope.Should().Be(EntryScope.User);
        entry.DisplayName.Should().Be("Test App");
        entry.SourcePath.Should().Be(@"HKCU\Software\Test");
    }

    [Fact]
    public void StartupEntry_DefaultValues_AreCorrect()
    {
        var entry = new StartupEntry
        {
            Id = "test-id",
            Type = EntryType.RegistryRun,
            Scope = EntryScope.User,
            DisplayName = "Test",
            SourcePath = "Path"
        };

        entry.Status.Should().Be(EntryStatus.Unknown);
        entry.RiskLevel.Should().Be(RiskLevel.Unknown);
        entry.SignatureStatus.Should().Be(SignatureStatus.Unknown);
        entry.FileExists.Should().BeFalse();
        entry.IsProtected.Should().BeFalse();
    }
}
