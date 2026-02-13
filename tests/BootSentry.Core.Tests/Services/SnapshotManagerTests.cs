using BootSentry.Core.Enums;
using BootSentry.Core.Models;
using BootSentry.Core.Services;
using BootSentry.Core.Snapshots;
using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;

namespace BootSentry.Core.Tests.Services;

public class SnapshotManagerTests
{
    private readonly SnapshotManager _manager = new(NullLogger<SnapshotManager>.Instance);

    [Fact]
    public void Compare_WithDuplicateIdsAcrossProfiles_DoesNotThrowAndDetectsChanges()
    {
        var baseSnapshot = new StartupSnapshot
        {
            Name = "Base",
            Entries =
            [
                CreateEntry("dup-id", @"C:\Profiles\A", "ExtA", EntryStatus.Enabled, "cmd-a"),
                CreateEntry("dup-id", @"C:\Profiles\B", "ExtB", EntryStatus.Enabled, "cmd-b")
            ]
        };

        var targetSnapshot = new StartupSnapshot
        {
            Name = "Target",
            Entries =
            [
                CreateEntry("dup-id", @"C:\Profiles\A", "ExtA", EntryStatus.Enabled, "cmd-a"),
                CreateEntry("dup-id", @"C:\Profiles\B", "ExtB", EntryStatus.Disabled, "cmd-b"),
                CreateEntry("dup-id", @"C:\Profiles\C", "ExtC", EntryStatus.Enabled, "cmd-c")
            ]
        };

        var comparison = _manager.Compare(baseSnapshot, targetSnapshot);

        comparison.AddedEntries.Should().ContainSingle(e => e.SourcePath == @"C:\Profiles\C");
        comparison.RemovedEntries.Should().BeEmpty();
        comparison.ModifiedEntries.Should().ContainSingle(diff =>
            diff.BaseEntry.SourcePath == @"C:\Profiles\B" &&
            diff.TargetEntry.SourcePath == @"C:\Profiles\B");
    }

    private static StartupEntry CreateEntry(
        string id,
        string sourcePath,
        string sourceName,
        EntryStatus status,
        string commandLineRaw)
    {
        return new StartupEntry
        {
            Id = id,
            Type = EntryType.BrowserExtension,
            Scope = EntryScope.User,
            DisplayName = sourceName,
            SourcePath = sourcePath,
            SourceName = sourceName,
            CommandLineRaw = commandLineRaw,
            Status = status,
            FileExists = true
        };
    }
}
