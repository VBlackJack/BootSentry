using Microsoft.Data.Sqlite;
using BootSentry.Core.Interfaces;
using BootSentry.Knowledge.Models;

namespace BootSentry.Knowledge.Services;

/// <summary>
/// Service for querying the knowledge base.
/// </summary>
public class KnowledgeService : IKnowledgeService
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
                ShortDescriptionEn TEXT,
                FullDescription TEXT,
                FullDescriptionEn TEXT,
                DisableImpact TEXT,
                DisableImpactEn TEXT,
                PerformanceImpact TEXT,
                PerformanceImpactEn TEXT,
                Recommendation TEXT,
                RecommendationEn TEXT,
                InfoUrl TEXT,
                Tags TEXT,
                LastUpdated TEXT NOT NULL
            );

            CREATE INDEX IF NOT EXISTS idx_name ON KnowledgeEntries(Name);
            CREATE INDEX IF NOT EXISTS idx_publisher ON KnowledgeEntries(Publisher);
            CREATE INDEX IF NOT EXISTS idx_executables ON KnowledgeEntries(ExecutableNames);
        ";
        cmd.ExecuteNonQuery();

        // Migrate existing databases to add English columns
        MigrateDatabase();
    }

    private void MigrateDatabase()
    {
        // Check if English columns exist, add them if not
        var columns = new[] { "ShortDescriptionEn", "FullDescriptionEn", "DisableImpactEn", "PerformanceImpactEn", "RecommendationEn" };

        foreach (var column in columns)
        {
            try
            {
                using var checkCmd = _connection.CreateCommand();
                checkCmd.CommandText = $"SELECT {column} FROM KnowledgeEntries LIMIT 1";
                checkCmd.ExecuteNonQuery();
            }
            catch
            {
                // Column doesn't exist, add it
                using var addCmd = _connection.CreateCommand();
                addCmd.CommandText = $"ALTER TABLE KnowledgeEntries ADD COLUMN {column} TEXT";
                addCmd.ExecuteNonQuery();
            }
        }
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

        // 0. Try browser extension ID match (for extensions, the path contains the ID)
        var extensionId = ExtractBrowserExtensionId(executable);
        if (!string.IsNullOrWhiteSpace(extensionId))
        {
            cmd.CommandText = @"
                SELECT * FROM KnowledgeEntries
                WHERE Aliases LIKE @extPattern COLLATE NOCASE
                LIMIT 1";
            cmd.Parameters.AddWithValue("@extPattern", $"%{extensionId}%");

            var entry = ReadEntry(cmd);
            if (entry != null) return entry;
            cmd.Parameters.Clear();
        }

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
    /// Extracts browser extension ID from extension paths.
    /// e.g., "C:\Users\...\Extensions\cjpalhdlnbpafiamejdnhcphjbkeiagm\1.51.0_0" -> "cjpalhdlnbpafiamejdnhcphjbkeiagm"
    /// </summary>
    private static string? ExtractBrowserExtensionId(string? path)
    {
        if (string.IsNullOrWhiteSpace(path))
            return null;

        // Check if path contains "Extensions" folder (Chrome, Edge, Brave, etc.)
        var extensionsIndex = path.IndexOf("Extensions", StringComparison.OrdinalIgnoreCase);
        if (extensionsIndex == -1)
            return null;

        // Extract the part after "Extensions\"
        var afterExtensions = path.Substring(extensionsIndex + 11); // "Extensions\".Length = 11
        if (string.IsNullOrWhiteSpace(afterExtensions))
            return null;

        // The extension ID is the first folder after "Extensions\"
        var parts = afterExtensions.Split(new[] { '\\', '/' }, StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length == 0)
            return null;

        var potentialId = parts[0];

        // Chrome extension IDs are 32 lowercase letters (a-p)
        if (potentialId.Length == 32 && System.Text.RegularExpressions.Regex.IsMatch(potentialId, @"^[a-p]+$"))
            return potentialId;

        // Firefox extension IDs can be like {guid} or name@domain
        if (potentialId.StartsWith("{") && potentialId.EndsWith("}"))
            return potentialId;
        if (potentialId.Contains("@"))
            return potentialId;

        return null;
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
                 ShortDescription, ShortDescriptionEn, FullDescription, FullDescriptionEn,
                 DisableImpact, DisableImpactEn, PerformanceImpact, PerformanceImpactEn,
                 Recommendation, RecommendationEn, InfoUrl, Tags, LastUpdated)
                VALUES
                (@name, @aliases, @publisher, @exes, @cat, @safety,
                 @short, @shortEn, @full, @fullEn, @impact, @impactEn,
                 @perf, @perfEn, @rec, @recEn, @url, @tags, @updated)";
        }
        else
        {
            cmd.CommandText = @"
                UPDATE KnowledgeEntries SET
                    Name = @name, Aliases = @aliases, Publisher = @publisher,
                    ExecutableNames = @exes, Category = @cat, SafetyLevel = @safety,
                    ShortDescription = @short, ShortDescriptionEn = @shortEn,
                    FullDescription = @full, FullDescriptionEn = @fullEn,
                    DisableImpact = @impact, DisableImpactEn = @impactEn,
                    PerformanceImpact = @perf, PerformanceImpactEn = @perfEn,
                    Recommendation = @rec, RecommendationEn = @recEn,
                    InfoUrl = @url, Tags = @tags, LastUpdated = @updated
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
        cmd.Parameters.AddWithValue("@shortEn", entry.ShortDescriptionEn ?? (object)DBNull.Value);
        cmd.Parameters.AddWithValue("@full", entry.FullDescription ?? (object)DBNull.Value);
        cmd.Parameters.AddWithValue("@fullEn", entry.FullDescriptionEn ?? (object)DBNull.Value);
        cmd.Parameters.AddWithValue("@impact", entry.DisableImpact ?? (object)DBNull.Value);
        cmd.Parameters.AddWithValue("@impactEn", entry.DisableImpactEn ?? (object)DBNull.Value);
        cmd.Parameters.AddWithValue("@perf", entry.PerformanceImpact ?? (object)DBNull.Value);
        cmd.Parameters.AddWithValue("@perfEn", entry.PerformanceImpactEn ?? (object)DBNull.Value);
        cmd.Parameters.AddWithValue("@rec", entry.Recommendation ?? (object)DBNull.Value);
        cmd.Parameters.AddWithValue("@recEn", entry.RecommendationEn ?? (object)DBNull.Value);
        cmd.Parameters.AddWithValue("@url", entry.InfoUrl ?? (object)DBNull.Value);
        cmd.Parameters.AddWithValue("@tags", entry.Tags ?? (object)DBNull.Value);
        cmd.Parameters.AddWithValue("@updated", entry.LastUpdated.ToString("O"));

        cmd.ExecuteNonQuery();
    }

    /// <summary>
    /// Seed the database with initial entries if empty or needs update.
    /// </summary>
    public void SeedIfEmpty()
    {
        var count = GetCount();

        if (count == 0)
        {
            // Empty database, seed everything
            var seeder = new KnowledgeSeeder(this);
            seeder.Seed();
            return;
        }

        // Check if database needs English translations update
        if (NeedsEnglishTranslationsUpdate())
        {
            // Delete all entries and reseed with new translations
            using var cmd = _connection.CreateCommand();
            cmd.CommandText = "DELETE FROM KnowledgeEntries";
            cmd.ExecuteNonQuery();

            var seeder = new KnowledgeSeeder(this);
            seeder.Seed();
        }
    }

    private bool NeedsEnglishTranslationsUpdate()
    {
        try
        {
            using var cmd = _connection.CreateCommand();
            // Check if there are any entries WITHOUT English description
            cmd.CommandText = "SELECT COUNT(*) FROM KnowledgeEntries WHERE ShortDescriptionEn IS NULL OR ShortDescriptionEn = ''";
            var count = Convert.ToInt32(cmd.ExecuteScalar());
            // If there are entries missing translations, we need to update
            return count > 0;
        }
        catch
        {
            // Column doesn't exist yet
            return true;
        }
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
        var entry = new KnowledgeEntry
        {
            Id = reader.GetInt32(reader.GetOrdinal("Id")),
            Name = reader.GetString(reader.GetOrdinal("Name")),
            Aliases = GetNullableString(reader, "Aliases"),
            Publisher = GetNullableString(reader, "Publisher"),
            ExecutableNames = GetNullableString(reader, "ExecutableNames"),
            Category = (KnowledgeCategory)reader.GetInt32(reader.GetOrdinal("Category")),
            SafetyLevel = (SafetyLevel)reader.GetInt32(reader.GetOrdinal("SafetyLevel")),
            ShortDescription = reader.GetString(reader.GetOrdinal("ShortDescription")),
            ShortDescriptionEn = GetNullableString(reader, "ShortDescriptionEn"),
            FullDescription = GetNullableString(reader, "FullDescription"),
            FullDescriptionEn = GetNullableString(reader, "FullDescriptionEn"),
            DisableImpact = GetNullableString(reader, "DisableImpact"),
            DisableImpactEn = GetNullableString(reader, "DisableImpactEn"),
            PerformanceImpact = GetNullableString(reader, "PerformanceImpact"),
            PerformanceImpactEn = GetNullableString(reader, "PerformanceImpactEn"),
            Recommendation = GetNullableString(reader, "Recommendation"),
            RecommendationEn = GetNullableString(reader, "RecommendationEn"),
            InfoUrl = GetNullableString(reader, "InfoUrl"),
            Tags = GetNullableString(reader, "Tags"),
            LastUpdated = DateTime.Parse(reader.GetString(reader.GetOrdinal("LastUpdated")))
        };
        return entry;
    }

    private static string? GetNullableString(SqliteDataReader reader, string columnName)
    {
        try
        {
            var ordinal = reader.GetOrdinal(columnName);
            return reader.IsDBNull(ordinal) ? null : reader.GetString(ordinal);
        }
        catch
        {
            // Column doesn't exist in old databases
            return null;
        }
    }

    /// <summary>
    /// Explicit interface implementation bridging the strongly-typed FindEntry
    /// to the untyped IKnowledgeService contract.
    /// </summary>
    object? IKnowledgeService.FindEntry(string? name, string? executable, string? publisher)
        => FindEntry(name, executable, publisher);

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
