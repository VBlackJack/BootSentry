using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using BootSentry.Core.Models;

namespace BootSentry.Core.Services;

/// <summary>
/// Service for exporting startup entries to various formats.
/// </summary>
public class ExportService
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        WriteIndented = true,
        Converters = { new JsonStringEnumConverter() },
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };

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
}
