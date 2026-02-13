using System.Text.Json;
using BootSentry.Core.Interfaces;
using BootSentry.Core.Models;
using BootSentry.Core.Snapshots;
using Microsoft.Extensions.Logging;

namespace BootSentry.Core.Services;

/// <summary>
/// Manages creating, saving, loading and comparing startup snapshots.
/// </summary>
public class SnapshotManager : ISnapshotManager
{
    private readonly ILogger<SnapshotManager> _logger;
    private readonly string _snapshotDirectory;
    
    // JSON options for pretty printing and respecting the StartupEntry structure
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        WriteIndented = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
    };

    public SnapshotManager(ILogger<SnapshotManager> logger)
    {
        _logger = logger;
        // Store snapshots in %LocalAppData%\BootSentry\Snapshots
        var appData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        _snapshotDirectory = Path.Combine(appData, Constants.AppName, Constants.Directories.Snapshots);
        
        if (!Directory.Exists(_snapshotDirectory))
        {
            Directory.CreateDirectory(_snapshotDirectory);
        }
    }

    /// <summary>
    /// Creates a snapshot from the current list of entries.
    /// </summary>
    public StartupSnapshot CreateSnapshot(string name, string description, IEnumerable<StartupEntry> entries)
    {
        _logger.LogInformation("Creating snapshot '{Name}' with {Count} entries", name, entries.Count());

        return new StartupSnapshot
        {
            Name = name,
            Description = description,
            Entries = entries.ToList()
        };
    }

    /// <summary>
    /// Saves a snapshot to disk.
    /// </summary>
    public async Task SaveSnapshotAsync(StartupSnapshot snapshot)
    {
        var fileName = $"{snapshot.CreatedAt:yyyyMMdd_HHmmss}_{SanitizeFileName(snapshot.Name)}.json";
        var filePath = Path.Combine(_snapshotDirectory, fileName);

        try
        {
            await using var stream = File.Create(filePath);
            await JsonSerializer.SerializeAsync(stream, snapshot, JsonOptions).ConfigureAwait(false);
            _logger.LogInformation("Snapshot saved to {Path}", filePath);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to save snapshot to {Path}", filePath);
            throw;
        }
    }

    /// <summary>
    /// Loads all snapshots from the storage directory.
    /// </summary>
    public async Task<List<StartupSnapshot>> LoadSnapshotsAsync()
    {
        var snapshots = new List<StartupSnapshot>();
        var files = Directory.GetFiles(_snapshotDirectory, "*.json");

        foreach (var file in files)
        {
            try
            {
                await using var stream = File.OpenRead(file);
                var snapshot = await JsonSerializer.DeserializeAsync<StartupSnapshot>(stream, JsonOptions).ConfigureAwait(false);
                if (snapshot != null)
                {
                    snapshots.Add(snapshot);
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to load snapshot from {Path}", file);
            }
        }

        return snapshots.OrderByDescending(s => s.CreatedAt).ToList();
    }
    
    /// <summary>
    /// Deletes a snapshot file.
    /// </summary>
    public void DeleteSnapshot(StartupSnapshot snapshot)
    {
        // Try to find the file based on ID or Name/Date match logic, or store filepath in model (transient)
        // For simplicity, we search for files containing the name or just re-scan.
        // Better approach: Store the file path in the model but don't serialize it?
        // Let's assume the caller manages the file or we search by ID.
        
        // Simple search by content ID is too slow. Let's rely on filename pattern or just re-list.
        // Actually, let's look for files created at the exact same time.
        var files = Directory.GetFiles(_snapshotDirectory, $"*{SanitizeFileName(snapshot.Name)}.json");
        foreach(var file in files)
        {
             // Check timestamp in filename or content? 
             // Simplest: The filename is constructed from Date + Name.
             var expectedFileName = $"{snapshot.CreatedAt:yyyyMMdd_HHmmss}_{SanitizeFileName(snapshot.Name)}.json";
             var expectedPath = Path.Combine(_snapshotDirectory, expectedFileName);
             
             if (File.Exists(expectedPath))
             {
                 File.Delete(expectedPath);
                 _logger.LogInformation("Deleted snapshot file {Path}", expectedPath);
                 return;
             }
        }
    }

    /// <summary>
    /// Compares two snapshots and returns the differences.
    /// </summary>
    public SnapshotComparisonResult Compare(StartupSnapshot baseSnapshot, StartupSnapshot targetSnapshot)
    {
        var result = new SnapshotComparisonResult(baseSnapshot, targetSnapshot);

        // Build maps resilient to duplicate IDs (can happen with profile-based entries in old snapshots).
        var baseEntries = BuildUniqueEntryMap(baseSnapshot.Entries);
        var targetEntries = BuildUniqueEntryMap(targetSnapshot.Entries);

        // Check for Added and Modified (in target).
        foreach (var (key, targetEntry) in targetEntries)
        {
            if (!baseEntries.TryGetValue(key, out var baseEntry))
            {
                result.AddedEntries.Add(targetEntry);
                continue;
            }

            var diff = CompareEntries(baseEntry, targetEntry);
            if (diff != null)
            {
                result.ModifiedEntries.Add(diff);
            }
        }

        // Check for Removed (in base but not target).
        foreach (var (key, baseEntry) in baseEntries)
        {
            if (!targetEntries.ContainsKey(key))
            {
                result.RemovedEntries.Add(baseEntry);
            }
        }

        return result;
    }

    private static Dictionary<string, StartupEntry> BuildUniqueEntryMap(IEnumerable<StartupEntry> entries)
    {
        var map = new Dictionary<string, StartupEntry>(StringComparer.Ordinal);
        var occurrences = new Dictionary<string, int>(StringComparer.Ordinal);

        foreach (var entry in entries)
        {
            var baseKey = BuildEntryBaseKey(entry);
            occurrences.TryGetValue(baseKey, out var count);
            occurrences[baseKey] = count + 1;

            var uniqueKey = $"{baseKey}#{count}";
            map[uniqueKey] = entry;
        }

        return map;
    }

    private static string BuildEntryBaseKey(StartupEntry entry)
    {
        return string.Join("|",
            entry.Id,
            entry.Type,
            entry.Scope,
            entry.SourcePath,
            entry.SourceName ?? string.Empty,
            entry.DisplayName);
    }

    private EntryDifference? CompareEntries(StartupEntry baseEntry, StartupEntry targetEntry)
    {
        var diff = new EntryDifference(baseEntry, targetEntry);
        bool hasChanges = false;

        // Compare critical properties
        if (baseEntry.Status != targetEntry.Status)
        {
            diff.ChangedProperties.Add($"Status changed from {baseEntry.Status} to {targetEntry.Status}");
            hasChanges = true;
        }

        if (baseEntry.CommandLineRaw != targetEntry.CommandLineRaw)
        {
            diff.ChangedProperties.Add("Command Line modified");
            hasChanges = true;
        }

        if (baseEntry.RiskLevel != targetEntry.RiskLevel)
        {
            diff.ChangedProperties.Add($"Risk Level changed from {baseEntry.RiskLevel} to {targetEntry.RiskLevel}");
            hasChanges = true;
        }

        return hasChanges ? diff : null;
    }

    private static string SanitizeFileName(string name)
    {
        foreach (var c in Path.GetInvalidFileNameChars())
        {
            name = name.Replace(c, '_');
        }
        return name;
    }
}
