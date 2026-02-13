using BootSentry.Core.Models;

namespace BootSentry.Core.Snapshots;

/// <summary>
/// Represents the difference between two snapshots.
/// </summary>
public class SnapshotComparisonResult
{
    public StartupSnapshot BaseSnapshot { get; init; }
    public StartupSnapshot TargetSnapshot { get; init; }
    public DateTime ComparisonDate { get; init; } = DateTime.UtcNow;

    public List<StartupEntry> AddedEntries { get; } = new();
    public List<StartupEntry> RemovedEntries { get; } = new();
    public List<EntryDifference> ModifiedEntries { get; } = new();

    public bool HasDifferences => AddedEntries.Any() || RemovedEntries.Any() || ModifiedEntries.Any();

    public SnapshotComparisonResult(StartupSnapshot baseSnapshot, StartupSnapshot targetSnapshot)
    {
        BaseSnapshot = baseSnapshot;
        TargetSnapshot = targetSnapshot;
    }
}

/// <summary>
/// Detail of a modified entry.
/// </summary>
public class EntryDifference
{
    public StartupEntry BaseEntry { get; init; }
    public StartupEntry TargetEntry { get; init; }
    public List<string> ChangedProperties { get; } = new();

    public EntryDifference(StartupEntry baseEntry, StartupEntry targetEntry)
    {
        BaseEntry = baseEntry;
        TargetEntry = targetEntry;
    }
}
