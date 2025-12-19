using System.IO.Compression;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using BootSentry.Core.Models;

namespace BootSentry.Core.Services;

/// <summary>
/// Delegate for finding knowledge entries (to avoid circular dependency with Knowledge project).
/// </summary>
public delegate object? KnowledgeFinder(string? name, string? executable, string? publisher);

/// <summary>
/// Service for exporting startup entries to various formats.
/// </summary>
public class ExportService
{
    private readonly KnowledgeFinder? _knowledgeFinder;

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        WriteIndented = true,
        Converters = { new JsonStringEnumConverter() },
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };

    public ExportService()
    {
    }

    public ExportService(KnowledgeFinder knowledgeFinder)
    {
        _knowledgeFinder = knowledgeFinder;
    }

    /// <summary>
    /// Exports entries to JSON format.
    /// </summary>
    public string ExportToJson(IEnumerable<StartupEntry> entries, ExportOptions options)
    {
        var exportData = entries.Select(e => CreateExportEntry(e, options)).ToList();

        var wrapper = new
        {
            ExportDate = DateTime.UtcNow,
            MachineName = options.Anonymize ? "[MACHINE]" : Environment.MachineName,
            UserName = options.Anonymize ? "[USER]" : Environment.UserName,
            TotalEntries = exportData.Count,
            Entries = exportData
        };

        return JsonSerializer.Serialize(wrapper, JsonOptions);
    }

    /// <summary>
    /// Exports entries to CSV format.
    /// </summary>
    public string ExportToCsv(IEnumerable<StartupEntry> entries, ExportOptions options)
    {
        var sb = new StringBuilder();

        // Header
        var headers = new List<string>
        {
            "DisplayName", "Type", "Scope", "Status", "Publisher",
            "SignatureStatus", "RiskLevel", "TargetPath"
        };

        if (options.IncludeKnowledgeInfo)
        {
            headers.AddRange(new[] { "HasDescription", "KnowledgeMatch", "KnowledgeDescription" });
        }

        if (options.IncludeDetails)
        {
            headers.AddRange(new[] { "CommandLine", "Arguments", "FileVersion", "FileSize", "LastModified" });
        }

        sb.AppendLine(string.Join(",", headers.Select(EscapeCsv)));

        // Data rows
        foreach (var entry in entries)
        {
            var values = new List<string>
            {
                entry.DisplayName,
                entry.Type.ToString(),
                entry.Scope.ToString(),
                entry.Status.ToString(),
                entry.Publisher ?? "",
                entry.SignatureStatus.ToString(),
                entry.RiskLevel.ToString(),
                (options.Anonymize ? AnonymizePath(entry.TargetPath) : entry.TargetPath) ?? ""
            };

            if (options.IncludeKnowledgeInfo)
            {
                var (hasDescription, matchName, description) = GetKnowledgeInfo(entry);
                values.AddRange(new[]
                {
                    hasDescription ? "Yes" : "No",
                    matchName,
                    description
                });
            }

            if (options.IncludeDetails)
            {
                values.AddRange(new[]
                {
                    (options.Anonymize ? AnonymizePath(entry.CommandLineRaw) : entry.CommandLineRaw) ?? "",
                    entry.Arguments ?? "",
                    entry.FileVersion ?? "",
                    entry.FileSize?.ToString() ?? "",
                    entry.LastModified?.ToString("O") ?? ""
                }!);
            }

            sb.AppendLine(string.Join(",", values.Select(EscapeCsv)));
        }

        return sb.ToString();
    }

    private (bool hasDescription, string matchName, string description) GetKnowledgeInfo(StartupEntry entry)
    {
        if (_knowledgeFinder == null)
            return (false, "", "");

        var knowledge = _knowledgeFinder(entry.DisplayName, entry.TargetPath, entry.Publisher);
        if (knowledge == null)
            return (false, "", "");

        // Use reflection to get properties since we can't reference KnowledgeEntry directly
        var type = knowledge.GetType();
        var name = type.GetProperty("Name")?.GetValue(knowledge)?.ToString() ?? "";
        var description = type.GetProperty("ShortDescription")?.GetValue(knowledge)?.ToString() ?? "";

        return (true, name, description);
    }

    /// <summary>
    /// Exports entries to a file.
    /// </summary>
    public async Task ExportToFileAsync(
        IEnumerable<StartupEntry> entries,
        string filePath,
        ExportFormat format,
        ExportOptions options,
        CancellationToken cancellationToken = default)
    {
        var content = format switch
        {
            ExportFormat.Json => ExportToJson(entries, options),
            ExportFormat.Csv => ExportToCsv(entries, options),
            _ => throw new ArgumentException($"Unsupported format: {format}")
        };

        await File.WriteAllTextAsync(filePath, content, Encoding.UTF8, cancellationToken);
    }

    private object CreateExportEntry(StartupEntry entry, ExportOptions options)
    {
        var basic = new Dictionary<string, object?>
        {
            ["Id"] = entry.Id,
            ["DisplayName"] = entry.DisplayName,
            ["Type"] = entry.Type.ToString(),
            ["Scope"] = entry.Scope.ToString(),
            ["Status"] = entry.Status.ToString(),
            ["Publisher"] = entry.Publisher,
            ["SignatureStatus"] = entry.SignatureStatus.ToString(),
            ["RiskLevel"] = entry.RiskLevel.ToString(),
            ["FileExists"] = entry.FileExists,
            ["IsProtected"] = entry.IsProtected
        };

        if (options.Anonymize)
        {
            basic["TargetPath"] = AnonymizePath(entry.TargetPath);
            basic["SourcePath"] = AnonymizePath(entry.SourcePath);
            basic["CommandLine"] = AnonymizePath(entry.CommandLineRaw);
        }
        else
        {
            basic["TargetPath"] = entry.TargetPath;
            basic["SourcePath"] = entry.SourcePath;
            basic["CommandLine"] = entry.CommandLineRaw;
        }

        if (options.IncludeKnowledgeInfo)
        {
            var (hasDescription, matchName, description) = GetKnowledgeInfo(entry);
            basic["HasKnowledgeEntry"] = hasDescription;
            basic["KnowledgeMatch"] = hasDescription ? matchName : null;
            basic["KnowledgeDescription"] = hasDescription ? description : null;
        }

        if (options.IncludeDetails)
        {
            basic["Arguments"] = entry.Arguments;
            basic["WorkingDirectory"] = options.Anonymize ? AnonymizePath(entry.WorkingDirectory) : entry.WorkingDirectory;
            basic["FileVersion"] = entry.FileVersion;
            basic["ProductName"] = entry.ProductName;
            basic["CompanyName"] = entry.CompanyName;
            basic["FileDescription"] = entry.FileDescription;
            basic["FileSize"] = entry.FileSize;
            basic["LastModified"] = entry.LastModified;
            basic["Notes"] = entry.Notes;
            basic["ProtectionReason"] = entry.ProtectionReason;
        }

        if (options.IncludeHashes && !string.IsNullOrEmpty(entry.Sha256))
        {
            basic["Sha256"] = entry.Sha256;
        }

        return basic;
    }

    private static string? AnonymizePath(string? path)
    {
        if (string.IsNullOrEmpty(path))
            return path;

        var result = path;

        // Replace user-specific paths
        var userProfile = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        if (!string.IsNullOrEmpty(userProfile))
        {
            result = result.Replace(userProfile, "[USER_PROFILE]", StringComparison.OrdinalIgnoreCase);
        }

        var userName = Environment.UserName;
        if (!string.IsNullOrEmpty(userName))
        {
            result = result.Replace($@"\Users\{userName}\", @"\Users\[USER]\", StringComparison.OrdinalIgnoreCase);
            result = result.Replace($@"\{userName}\", @"\[USER]\", StringComparison.OrdinalIgnoreCase);
        }

        // Replace machine name
        var machineName = Environment.MachineName;
        if (!string.IsNullOrEmpty(machineName))
        {
            result = result.Replace(machineName, "[MACHINE]", StringComparison.OrdinalIgnoreCase);
        }

        return result;
    }

    private static string EscapeCsv(string value)
    {
        if (string.IsNullOrEmpty(value))
            return "\"\"";

        // If contains comma, quote, or newline, wrap in quotes and escape existing quotes
        if (value.Contains(',') || value.Contains('"') || value.Contains('\n') || value.Contains('\r'))
        {
            return $"\"{value.Replace("\"", "\"\"")}\"";
        }

        return value;
    }

    /// <summary>
    /// Creates a diagnostics ZIP file containing logs, entries export, and system info.
    /// </summary>
    public async Task ExportDiagnosticsZipAsync(
        IEnumerable<StartupEntry> entries,
        string zipFilePath,
        CancellationToken cancellationToken = default)
    {
        var tempDir = Path.Combine(Path.GetTempPath(), $"BootSentry_Diag_{Guid.NewGuid():N}");
        Directory.CreateDirectory(tempDir);

        try
        {
            // 1. Export entries (anonymized)
            var entriesJson = ExportToJson(entries, new ExportOptions { Anonymize = true, IncludeDetails = true });
            await File.WriteAllTextAsync(Path.Combine(tempDir, "entries.json"), entriesJson, cancellationToken);

            // 2. System information
            var systemInfo = GetSystemInfo();
            await File.WriteAllTextAsync(Path.Combine(tempDir, "system_info.txt"), systemInfo, cancellationToken);

            // 3. Copy recent logs
            var logsDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "BootSentry", "Logs");
            if (Directory.Exists(logsDir))
            {
                var diagLogsDir = Path.Combine(tempDir, "logs");
                Directory.CreateDirectory(diagLogsDir);

                var logFiles = Directory.GetFiles(logsDir, "*.log")
                    .OrderByDescending(f => File.GetLastWriteTime(f))
                    .Take(5);

                foreach (var logFile in logFiles)
                {
                    try
                    {
                        var destPath = Path.Combine(diagLogsDir, Path.GetFileName(logFile));
                        File.Copy(logFile, destPath, overwrite: true);
                    }
                    catch { }
                }
            }

            // 4. Copy backup manifests (no payload data)
            var backupsDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "BootSentry", "Backups");
            if (Directory.Exists(backupsDir))
            {
                var diagBackupsDir = Path.Combine(tempDir, "backup_manifests");
                Directory.CreateDirectory(diagBackupsDir);

                var manifestFiles = Directory.GetFiles(backupsDir, "manifest.json", SearchOption.AllDirectories)
                    .OrderByDescending(f => File.GetLastWriteTime(f))
                    .Take(10);

                var i = 0;
                foreach (var manifest in manifestFiles)
                {
                    try
                    {
                        var destPath = Path.Combine(diagBackupsDir, $"manifest_{i++}.json");
                        File.Copy(manifest, destPath, overwrite: true);
                    }
                    catch { }
                }
            }

            // 5. App settings (without sensitive data)
            var settingsFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "BootSentry", "settings.json");
            if (File.Exists(settingsFile))
            {
                try
                {
                    File.Copy(settingsFile, Path.Combine(tempDir, "settings.json"), overwrite: true);
                }
                catch { }
            }

            // Create ZIP
            if (File.Exists(zipFilePath))
                File.Delete(zipFilePath);

            ZipFile.CreateFromDirectory(tempDir, zipFilePath, CompressionLevel.Optimal, includeBaseDirectory: false);
        }
        finally
        {
            // Cleanup temp directory
            try
            {
                Directory.Delete(tempDir, recursive: true);
            }
            catch { }
        }
    }

    private static string GetSystemInfo()
    {
        var sb = new StringBuilder();
        sb.AppendLine("=== BootSentry Diagnostics ===");
        sb.AppendLine($"Export Date: {DateTime.UtcNow:O}");
        sb.AppendLine();

        sb.AppendLine("=== System Information ===");
        sb.AppendLine($"OS Version: {Environment.OSVersion}");
        sb.AppendLine($"OS 64-bit: {Environment.Is64BitOperatingSystem}");
        sb.AppendLine($"Process 64-bit: {Environment.Is64BitProcess}");
        sb.AppendLine($".NET Version: {Environment.Version}");
        sb.AppendLine($"Machine Name: [REDACTED]");
        sb.AppendLine($"User Name: [REDACTED]");
        sb.AppendLine($"Processor Count: {Environment.ProcessorCount}");
        sb.AppendLine($"System Directory: {Environment.SystemDirectory}");
        sb.AppendLine();

        sb.AppendLine("=== Memory ===");
        try
        {
            var process = System.Diagnostics.Process.GetCurrentProcess();
            sb.AppendLine($"Working Set: {process.WorkingSet64 / 1024 / 1024} MB");
            sb.AppendLine($"Private Memory: {process.PrivateMemorySize64 / 1024 / 1024} MB");
        }
        catch
        {
            sb.AppendLine("(Unable to retrieve memory info)");
        }
        sb.AppendLine();

        sb.AppendLine("=== Application ===");
        var assembly = System.Reflection.Assembly.GetExecutingAssembly();
        sb.AppendLine($"Assembly Version: {assembly.GetName().Version}");
        sb.AppendLine($"Location: [REDACTED]");

        return sb.ToString();
    }
}

/// <summary>
/// Export format options.
/// </summary>
public enum ExportFormat
{
    Json,
    Csv
}

/// <summary>
/// Options for export operations.
/// </summary>
public class ExportOptions
{
    /// <summary>
    /// Whether to anonymize user-specific paths.
    /// </summary>
    public bool Anonymize { get; set; }

    /// <summary>
    /// Whether to include detailed metadata.
    /// </summary>
    public bool IncludeDetails { get; set; } = true;

    /// <summary>
    /// Whether to include file hashes.
    /// </summary>
    public bool IncludeHashes { get; set; }

    /// <summary>
    /// Whether to include knowledge base match information.
    /// </summary>
    public bool IncludeKnowledgeInfo { get; set; }
}
