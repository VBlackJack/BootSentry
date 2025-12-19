using Microsoft.Data.Sqlite;
using BootSentry.Knowledge.Models;

namespace BootSentry.Knowledge.Services;

/// <summary>
/// Service for querying the knowledge base.
/// </summary>
public class KnowledgeService : IDisposable
{
    private readonly SqliteConnection _connection;
    private readonly string _dbPath;
    private bool _disposed;

    public KnowledgeService(string? dbPath = null)
    {
        _dbPath = dbPath ?? GetDefaultDbPath();
        _connection = new SqliteConnection($"Data Source={_dbPath}");
        _connection.Open();
        EnsureDatabase();
    }

    private static string GetDefaultDbPath()
    {
        var appData = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
        var dir = Path.Combine(appData, "BootSentry");
        Directory.CreateDirectory(dir);
        return Path.Combine(dir, "knowledge.db");
    }

    private void EnsureDatabase()
    {
        using var cmd = _connection.CreateCommand();
        cmd.CommandText = @"
            CREATE TABLE IF NOT EXISTS KnowledgeEntries (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Name TEXT NOT NULL,
                Aliases TEXT,
                Publisher TEXT,
                ExecutableNames TEXT,
                Category INTEGER NOT NULL,
                SafetyLevel INTEGER NOT NULL,
                ShortDescription TEXT NOT NULL,
                FullDescription TEXT,
                DisableImpact TEXT,
                PerformanceImpact TEXT,
                Recommendation TEXT,
                InfoUrl TEXT,
                Tags TEXT,
                LastUpdated TEXT NOT NULL
            );

            CREATE INDEX IF NOT EXISTS idx_name ON KnowledgeEntries(Name);
            CREATE INDEX IF NOT EXISTS idx_publisher ON KnowledgeEntries(Publisher);
            CREATE INDEX IF NOT EXISTS idx_executables ON KnowledgeEntries(ExecutableNames);
        ";
        cmd.ExecuteNonQuery();
    }

    /// <summary>
    /// Search for a knowledge entry by name, executable, or publisher.
    /// Uses multiple matching strategies for better coverage.
    /// </summary>
    public KnowledgeEntry? FindEntry(string? name, string? executable, string? publisher)
    {
        if (string.IsNullOrWhiteSpace(name) && string.IsNullOrWhiteSpace(executable) && string.IsNullOrWhiteSpace(publisher))
            return null;

        using var cmd = _connection.CreateCommand();

        // 1. Try executable match FIRST (most reliable)
        if (!string.IsNullOrWhiteSpace(executable))
        {
            var exeName = Path.GetFileName(executable).ToLowerInvariant();
            if (!string.IsNullOrWhiteSpace(exeName) && exeName.Length > 4)
            {
                cmd.CommandText = @"
                    SELECT * FROM KnowledgeEntries
                    WHERE ExecutableNames LIKE @exePattern COLLATE NOCASE
                    LIMIT 1";
                cmd.Parameters.AddWithValue("@exePattern", $"%{exeName}%");

                var entry = ReadEntry(cmd);
                if (entry != null) return entry;
                cmd.Parameters.Clear();
            }
        }

        // 2. Try exact name match
        if (!string.IsNullOrWhiteSpace(name))
        {
            cmd.CommandText = @"
                SELECT * FROM KnowledgeEntries
                WHERE Name = @name COLLATE NOCASE
                LIMIT 1";
            cmd.Parameters.AddWithValue("@name", name);

            var entry = ReadEntry(cmd);
            if (entry != null) return entry;
            cmd.Parameters.Clear();

            // 3. Try alias match (name contained in aliases)
            cmd.CommandText = @"
                SELECT * FROM KnowledgeEntries
                WHERE Aliases LIKE @namePattern COLLATE NOCASE
                LIMIT 1";
            cmd.Parameters.AddWithValue("@namePattern", $"%{name}%");

            entry = ReadEntry(cmd);
            if (entry != null) return entry;
            cmd.Parameters.Clear();

            // 4. Try partial name match (for names with GUIDs like "BraveSoftwareUpdate{GUID}")
            // Extract base name before special characters
            var baseName = ExtractBaseName(name);
            if (!string.IsNullOrWhiteSpace(baseName) && baseName.Length >= 4 && baseName != name)
            {
                cmd.CommandText = @"
                    SELECT * FROM KnowledgeEntries
                    WHERE Name LIKE @basePattern COLLATE NOCASE
                       OR Aliases LIKE @basePattern COLLATE NOCASE
                    LIMIT 1";
                cmd.Parameters.AddWithValue("@basePattern", $"%{baseName}%");

                entry = ReadEntry(cmd);
                if (entry != null) return entry;
                cmd.Parameters.Clear();
            }
        }

        // 5. Try publisher match (least reliable, skip generic publishers)
        if (!string.IsNullOrWhiteSpace(publisher) &&
            !publisher.Equals("N/A", StringComparison.OrdinalIgnoreCase) &&
            !publisher.Contains("Microsoft Windows", StringComparison.OrdinalIgnoreCase))
        {
            cmd.CommandText = @"
                SELECT * FROM KnowledgeEntries
                WHERE Publisher LIKE @pubPattern COLLATE NOCASE
                LIMIT 1";
            cmd.Parameters.AddWithValue("@pubPattern", $"%{publisher}%");

            return ReadEntry(cmd);
        }

        return null;
    }

    /// <summary>
    /// Extracts base name from display names that contain GUIDs or version numbers.
    /// e.g., "BraveSoftwareUpdate{GUID}" -> "BraveSoftwareUpdate"
    /// e.g., "GoogleUpdaterTask144.0.7547.0{GUID}" -> "GoogleUpdater"
    /// </summary>
    private static string ExtractBaseName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return name;

        // Remove GUID patterns {xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx}
        var result = System.Text.RegularExpressions.Regex.Replace(name, @"\{[a-fA-F0-9\-]+\}", "");

        // Remove version numbers like 144.0.7547.0
        result = System.Text.RegularExpressions.Regex.Replace(result, @"\d+\.\d+\.\d+\.\d+", "");

        // Remove trailing special characters and "Task", "Machine", "Core", "System"
        result = System.Text.RegularExpressions.Regex.Replace(result, @"(Task|Machine|Core|System|Logon|Service)+\s*$", "",
            System.Text.RegularExpressions.RegexOptions.IgnoreCase);

        // Remove "electron.app." prefix
        if (result.StartsWith("electron.app.", StringComparison.OrdinalIgnoreCase))
            result = result.Substring(13);

        // Remove "com.todesktop." prefix
        if (result.StartsWith("com.todesktop.", StringComparison.OrdinalIgnoreCase))
            result = result.Substring(14);

        // Clean up
        result = result.Trim(' ', '.', '-', '_');

        return result;
    }

    /// <summary>
    /// Search entries by keyword.
    /// </summary>
    public List<KnowledgeEntry> Search(string keyword, int limit = 20)
    {
        var results = new List<KnowledgeEntry>();

        using var cmd = _connection.CreateCommand();
        cmd.CommandText = @"
            SELECT * FROM KnowledgeEntries
            WHERE Name LIKE @pattern COLLATE NOCASE
               OR Aliases LIKE @pattern COLLATE NOCASE
               OR Publisher LIKE @pattern COLLATE NOCASE
               OR ShortDescription LIKE @pattern COLLATE NOCASE
               OR Tags LIKE @pattern COLLATE NOCASE
            ORDER BY Name
            LIMIT @limit";
        cmd.Parameters.AddWithValue("@pattern", $"%{keyword}%");
        cmd.Parameters.AddWithValue("@limit", limit);

        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            results.Add(MapEntry(reader));
        }

        return results;
    }

    /// <summary>
    /// Get all entries of a specific category.
    /// </summary>
    public List<KnowledgeEntry> GetByCategory(KnowledgeCategory category)
    {
        var results = new List<KnowledgeEntry>();

        using var cmd = _connection.CreateCommand();
        cmd.CommandText = "SELECT * FROM KnowledgeEntries WHERE Category = @cat ORDER BY Name";
        cmd.Parameters.AddWithValue("@cat", (int)category);

        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            results.Add(MapEntry(reader));
        }

        return results;
    }

    /// <summary>
    /// Get total entry count.
    /// </summary>
    public int GetCount()
    {
        using var cmd = _connection.CreateCommand();
        cmd.CommandText = "SELECT COUNT(*) FROM KnowledgeEntries";
        return Convert.ToInt32(cmd.ExecuteScalar());
    }

    /// <summary>
    /// Add or update a knowledge entry.
    /// </summary>
    public void SaveEntry(KnowledgeEntry entry)
    {
        using var cmd = _connection.CreateCommand();

        if (entry.Id == 0)
        {
            cmd.CommandText = @"
                INSERT INTO KnowledgeEntries
                (Name, Aliases, Publisher, ExecutableNames, Category, SafetyLevel,
                 ShortDescription, FullDescription, DisableImpact, PerformanceImpact,
                 Recommendation, InfoUrl, Tags, LastUpdated)
                VALUES
                (@name, @aliases, @publisher, @exes, @cat, @safety,
                 @short, @full, @impact, @perf, @rec, @url, @tags, @updated)";
        }
        else
        {
            cmd.CommandText = @"
                UPDATE KnowledgeEntries SET
                    Name = @name, Aliases = @aliases, Publisher = @publisher,
                    ExecutableNames = @exes, Category = @cat, SafetyLevel = @safety,
                    ShortDescription = @short, FullDescription = @full,
                    DisableImpact = @impact, PerformanceImpact = @perf,
                    Recommendation = @rec, InfoUrl = @url, Tags = @tags,
                    LastUpdated = @updated
                WHERE Id = @id";
            cmd.Parameters.AddWithValue("@id", entry.Id);
        }

        cmd.Parameters.AddWithValue("@name", entry.Name);
        cmd.Parameters.AddWithValue("@aliases", entry.Aliases ?? (object)DBNull.Value);
        cmd.Parameters.AddWithValue("@publisher", entry.Publisher ?? (object)DBNull.Value);
        cmd.Parameters.AddWithValue("@exes", entry.ExecutableNames ?? (object)DBNull.Value);
        cmd.Parameters.AddWithValue("@cat", (int)entry.Category);
        cmd.Parameters.AddWithValue("@safety", (int)entry.SafetyLevel);
        cmd.Parameters.AddWithValue("@short", entry.ShortDescription);
        cmd.Parameters.AddWithValue("@full", entry.FullDescription ?? (object)DBNull.Value);
        cmd.Parameters.AddWithValue("@impact", entry.DisableImpact ?? (object)DBNull.Value);
        cmd.Parameters.AddWithValue("@perf", entry.PerformanceImpact ?? (object)DBNull.Value);
        cmd.Parameters.AddWithValue("@rec", entry.Recommendation ?? (object)DBNull.Value);
        cmd.Parameters.AddWithValue("@url", entry.InfoUrl ?? (object)DBNull.Value);
        cmd.Parameters.AddWithValue("@tags", entry.Tags ?? (object)DBNull.Value);
        cmd.Parameters.AddWithValue("@updated", entry.LastUpdated.ToString("O"));

        cmd.ExecuteNonQuery();
    }

    /// <summary>
    /// Seed the database with initial entries if empty.
    /// </summary>
    public void SeedIfEmpty()
    {
        if (GetCount() > 0) return;

        var seeder = new KnowledgeSeeder(this);
        seeder.Seed();
    }

    private KnowledgeEntry? ReadEntry(SqliteCommand cmd)
    {
        using var reader = cmd.ExecuteReader();
        if (reader.Read())
        {
            return MapEntry(reader);
        }
        return null;
    }

    private static KnowledgeEntry MapEntry(SqliteDataReader reader)
    {
        return new KnowledgeEntry
        {
            Id = reader.GetInt32(0),
            Name = reader.GetString(1),
            Aliases = reader.IsDBNull(2) ? null : reader.GetString(2),
            Publisher = reader.IsDBNull(3) ? null : reader.GetString(3),
            ExecutableNames = reader.IsDBNull(4) ? null : reader.GetString(4),
            Category = (KnowledgeCategory)reader.GetInt32(5),
            SafetyLevel = (SafetyLevel)reader.GetInt32(6),
            ShortDescription = reader.GetString(7),
            FullDescription = reader.IsDBNull(8) ? null : reader.GetString(8),
            DisableImpact = reader.IsDBNull(9) ? null : reader.GetString(9),
            PerformanceImpact = reader.IsDBNull(10) ? null : reader.GetString(10),
            Recommendation = reader.IsDBNull(11) ? null : reader.GetString(11),
            InfoUrl = reader.IsDBNull(12) ? null : reader.GetString(12),
            Tags = reader.IsDBNull(13) ? null : reader.GetString(13),
            LastUpdated = DateTime.Parse(reader.GetString(14))
        };
    }

    public void Dispose()
    {
        if (!_disposed)
        {
            _connection.Close();
            _connection.Dispose();
            _disposed = true;
        }
        GC.SuppressFinalize(this);
    }
}
