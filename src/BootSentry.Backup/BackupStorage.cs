using System.Text.Json;
using Microsoft.Extensions.Logging;
using BootSentry.Backup.Models;

namespace BootSentry.Backup;

/// <summary>
/// Handles storage of backup transactions on disk.
/// </summary>
public sealed class BackupStorage
{
    private readonly ILogger<BackupStorage> _logger;
    private readonly string _basePath;
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        WriteIndented = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    public BackupStorage(ILogger<BackupStorage> logger, string? basePath = null)
    {
        _logger = logger;
        _basePath = basePath ?? GetDefaultBasePath();
        EnsureDirectoryExists();
    }

    public string BasePath => _basePath;

    private static string GetDefaultBasePath()
    {
        return Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
            "BootSentry",
            "Backups");
    }

    private void EnsureDirectoryExists()
    {
        try
        {
            if (!Directory.Exists(_basePath))
            {
                Directory.CreateDirectory(_basePath);
                _logger.LogInformation("Created backup directory: {Path}", _basePath);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create backup directory: {Path}", _basePath);
            throw;
        }
    }

    /// <summary>
    /// Gets the directory path for a specific transaction.
    /// </summary>
    /// <exception cref="ArgumentException">Thrown if transactionId contains invalid characters.</exception>
    public string GetTransactionPath(string transactionId)
    {
        ValidateTransactionId(transactionId);
        return Path.Combine(_basePath, transactionId);
    }

    /// <summary>
    /// Validates a transaction ID to prevent path traversal attacks.
    /// </summary>
    private static void ValidateTransactionId(string transactionId)
    {
        if (string.IsNullOrWhiteSpace(transactionId))
            throw new ArgumentException("Transaction ID cannot be null or empty.", nameof(transactionId));

        // Check for path traversal patterns
        if (transactionId.Contains("..") ||
            transactionId.Contains('/') ||
            transactionId.Contains('\\') ||
            transactionId.Contains(':'))
        {
            throw new ArgumentException("Transaction ID contains invalid characters.", nameof(transactionId));
        }

        // Validate against invalid filename characters
        var invalidChars = Path.GetInvalidFileNameChars();
        if (transactionId.IndexOfAny(invalidChars) >= 0)
        {
            throw new ArgumentException("Transaction ID contains invalid characters.", nameof(transactionId));
        }
    }

    /// <summary>
    /// Gets the manifest file path for a transaction.
    /// </summary>
    public string GetManifestPath(string transactionId)
    {
        return Path.Combine(GetTransactionPath(transactionId), "manifest.json");
    }

    /// <summary>
    /// Creates a new transaction directory.
    /// </summary>
    public string CreateTransactionDirectory(string transactionId)
    {
        var path = GetTransactionPath(transactionId);
        Directory.CreateDirectory(path);
        return path;
    }

    /// <summary>
    /// Saves a transaction manifest.
    /// </summary>
    public async Task SaveManifestAsync(TransactionManifest manifest, CancellationToken cancellationToken = default)
    {
        var path = GetManifestPath(manifest.Id);
        var json = JsonSerializer.Serialize(manifest, JsonOptions);
        await File.WriteAllTextAsync(path, json, cancellationToken);
        _logger.LogDebug("Saved manifest: {Path}", path);
    }

    /// <summary>
    /// Loads a transaction manifest.
    /// </summary>
    public async Task<TransactionManifest?> LoadManifestAsync(string transactionId, CancellationToken cancellationToken = default)
    {
        var path = GetManifestPath(transactionId);
        if (!File.Exists(path))
            return null;

        var json = await File.ReadAllTextAsync(path, cancellationToken);
        return JsonSerializer.Deserialize<TransactionManifest>(json, JsonOptions);
    }

    /// <summary>
    /// Gets all transaction manifests.
    /// </summary>
    public async Task<List<TransactionManifest>> GetAllManifestsAsync(CancellationToken cancellationToken = default)
    {
        var manifests = new List<TransactionManifest>();

        if (!Directory.Exists(_basePath))
            return manifests;

        var directories = Directory.GetDirectories(_basePath);
        foreach (var dir in directories)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var transactionId = Path.GetFileName(dir);
            var manifest = await LoadManifestAsync(transactionId, cancellationToken);
            if (manifest != null)
                manifests.Add(manifest);
        }

        return manifests.OrderByDescending(m => m.Timestamp).ToList();
    }

    /// <summary>
    /// Copies a file to the transaction backup directory.
    /// </summary>
    public async Task<string> BackupFileAsync(
        string transactionId,
        string sourceFile,
        string? relativeName = null,
        CancellationToken cancellationToken = default)
    {
        var transactionPath = GetTransactionPath(transactionId);
        var fileName = relativeName ?? Path.GetFileName(sourceFile);
        var destPath = Path.Combine(transactionPath, "files", fileName);

        var destDir = Path.GetDirectoryName(destPath);
        if (destDir != null && !Directory.Exists(destDir))
            Directory.CreateDirectory(destDir);

        await using var source = new FileStream(sourceFile, FileMode.Open, FileAccess.Read, FileShare.Read);
        await using var dest = new FileStream(destPath, FileMode.Create, FileAccess.Write);
        await source.CopyToAsync(dest, cancellationToken);

        _logger.LogDebug("Backed up file: {Source} -> {Dest}", sourceFile, destPath);
        return destPath;
    }

    /// <summary>
    /// Saves registry value data to backup.
    /// </summary>
    public async Task<string> BackupRegistryValueAsync(
        string transactionId,
        string keyPath,
        string valueName,
        object value,
        Microsoft.Win32.RegistryValueKind valueKind,
        CancellationToken cancellationToken = default)
    {
        var transactionPath = GetTransactionPath(transactionId);
        var regBackupPath = Path.Combine(transactionPath, "registry");

        if (!Directory.Exists(regBackupPath))
            Directory.CreateDirectory(regBackupPath);

        var backupData = new
        {
            KeyPath = keyPath,
            ValueName = valueName,
            Value = value?.ToString(),
            ValueKind = valueKind.ToString(),
            BackupTime = DateTime.UtcNow
        };

        var fileName = $"{SanitizeFileName(valueName)}.json";
        var filePath = Path.Combine(regBackupPath, fileName);
        var json = JsonSerializer.Serialize(backupData, JsonOptions);
        await File.WriteAllTextAsync(filePath, json, cancellationToken);

        _logger.LogDebug("Backed up registry value: {Key}\\{Value}", keyPath, valueName);
        return filePath;
    }

    /// <summary>
    /// Deletes a transaction and all its data.
    /// </summary>
    public void DeleteTransaction(string transactionId)
    {
        var path = GetTransactionPath(transactionId);
        if (Directory.Exists(path))
        {
            Directory.Delete(path, recursive: true);
            _logger.LogInformation("Deleted transaction: {Id}", transactionId);
        }
    }

    /// <summary>
    /// Purges old transactions based on criteria.
    /// </summary>
    public async Task<int> PurgeOldTransactionsAsync(
        TimeSpan? maxAge = null,
        int? maxCount = null,
        CancellationToken cancellationToken = default)
    {
        var manifests = await GetAllManifestsAsync(cancellationToken);
        var toDelete = new List<string>();

        // Filter by age
        if (maxAge.HasValue)
        {
            var cutoff = DateTime.UtcNow - maxAge.Value;
            toDelete.AddRange(manifests
                .Where(m => m.Timestamp < cutoff)
                .Select(m => m.Id));
        }

        // Filter by count (keep most recent)
        if (maxCount.HasValue && manifests.Count > maxCount.Value)
        {
            var excess = manifests
                .OrderByDescending(m => m.Timestamp)
                .Skip(maxCount.Value)
                .Select(m => m.Id);
            toDelete.AddRange(excess);
        }

        // Delete unique IDs
        var uniqueToDelete = toDelete.Distinct().ToList();
        foreach (var id in uniqueToDelete)
        {
            DeleteTransaction(id);
        }

        return uniqueToDelete.Count;
    }

    private static string SanitizeFileName(string name)
    {
        var invalid = Path.GetInvalidFileNameChars();
        return string.Join("_", name.Split(invalid, StringSplitOptions.RemoveEmptyEntries));
    }
}
