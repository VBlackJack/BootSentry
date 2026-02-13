using System.Text.Json;
using System.Globalization;
using BootSentry.Core;
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
            Constants.AppName,
            Constants.Directories.Backups);
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
        return Path.Combine(GetTransactionPath(transactionId), Constants.Files.Manifest);
    }

    /// <summary>
    /// Gets the HMAC sidecar file path for a transaction manifest.
    /// </summary>
    public string GetManifestHmacPath(string transactionId)
    {
        return GetManifestPath(transactionId) + Constants.Files.ManifestHmacSuffix;
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
    /// Saves a transaction manifest and its HMAC integrity sidecar file.
    /// </summary>
    public async Task SaveManifestAsync(TransactionManifest manifest, CancellationToken cancellationToken = default)
    {
        var path = GetManifestPath(manifest.Id);
        var json = JsonSerializer.Serialize(manifest, JsonOptions);
        await File.WriteAllTextAsync(path, json, cancellationToken).ConfigureAwait(false);

        // Write HMAC sidecar file for integrity verification
        var hmacPath = GetManifestHmacPath(manifest.Id);
        var hmac = ManifestIntegrity.ComputeHmac(json);
        await File.WriteAllTextAsync(hmacPath, hmac, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Saved manifest with HMAC: {Path}", path);
    }

    /// <summary>
    /// Loads a transaction manifest and verifies its HMAC integrity.
    /// Logs a warning if the HMAC is missing (legacy backup) or invalid (possible tampering).
    /// </summary>
    public async Task<TransactionManifest?> LoadManifestAsync(string transactionId, CancellationToken cancellationToken = default)
    {
        var path = GetManifestPath(transactionId);
        if (!File.Exists(path))
            return null;

        var json = await File.ReadAllTextAsync(path, cancellationToken).ConfigureAwait(false);
        VerifyManifestHmac(transactionId, json);
        return JsonSerializer.Deserialize<TransactionManifest>(json, JsonOptions);
    }

    /// <summary>
    /// Verifies the HMAC integrity of a loaded manifest.
    /// Does not fail on mismatch to maintain backwards compatibility with pre-HMAC backups.
    /// </summary>
    private void VerifyManifestHmac(string transactionId, string manifestJson)
    {
        var hmacPath = GetManifestHmacPath(transactionId);

        if (!File.Exists(hmacPath))
        {
            _logger.LogWarning(
                "No HMAC sidecar found for transaction {TransactionId}; "
                + "this may be a legacy backup created before integrity protection was enabled",
                transactionId);
            return;
        }

        try
        {
            var storedHmac = File.ReadAllText(hmacPath).Trim();

            if (!ManifestIntegrity.VerifyHmac(manifestJson, storedHmac))
            {
                _logger.LogWarning(
                    "HMAC verification failed for transaction {TransactionId}; "
                    + "the manifest may have been tampered with or copied from another machine",
                    transactionId);
            }
            else
            {
                _logger.LogDebug("HMAC verification passed for transaction {TransactionId}", transactionId);
            }
        }
        catch (FormatException ex)
        {
            _logger.LogWarning(
                ex,
                "Malformed HMAC sidecar for transaction {TransactionId}; "
                + "the HMAC file content is not valid base64",
                transactionId);
        }
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
            var manifest = await LoadManifestAsync(transactionId, cancellationToken).ConfigureAwait(false);
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
        await source.CopyToAsync(dest, cancellationToken).ConfigureAwait(false);

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

        var backupData = CreateRegistryBackupData(keyPath, valueName, value, valueKind);

        var fileName = $"{SanitizeFileName(valueName)}.json";
        var filePath = Path.Combine(regBackupPath, fileName);
        var json = JsonSerializer.Serialize(backupData, JsonOptions);
        await File.WriteAllTextAsync(filePath, json, cancellationToken).ConfigureAwait(false);

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
        var manifests = await GetAllManifestsAsync(cancellationToken).ConfigureAwait(false);
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

    private static RegistryValueBackupData CreateRegistryBackupData(
        string keyPath,
        string valueName,
        object value,
        Microsoft.Win32.RegistryValueKind valueKind)
    {
        return valueKind switch
        {
            Microsoft.Win32.RegistryValueKind.DWord => new RegistryValueBackupData
            {
                KeyPath = keyPath,
                ValueName = valueName,
                ValueKind = valueKind.ToString(),
                BackupTime = DateTime.UtcNow,
                DWordValue = Convert.ToInt32(value, CultureInfo.InvariantCulture)
            },
            Microsoft.Win32.RegistryValueKind.QWord => new RegistryValueBackupData
            {
                KeyPath = keyPath,
                ValueName = valueName,
                ValueKind = valueKind.ToString(),
                BackupTime = DateTime.UtcNow,
                QWordValue = Convert.ToInt64(value, CultureInfo.InvariantCulture)
            },
            Microsoft.Win32.RegistryValueKind.MultiString => new RegistryValueBackupData
            {
                KeyPath = keyPath,
                ValueName = valueName,
                ValueKind = valueKind.ToString(),
                BackupTime = DateTime.UtcNow,
                MultiStringValue = value as string[] ?? Array.Empty<string>()
            },
            Microsoft.Win32.RegistryValueKind.Binary => new RegistryValueBackupData
            {
                KeyPath = keyPath,
                ValueName = valueName,
                ValueKind = valueKind.ToString(),
                BackupTime = DateTime.UtcNow,
                BinaryBase64 = Convert.ToBase64String(value as byte[] ?? Array.Empty<byte>())
            },
            _ => new RegistryValueBackupData
            {
                KeyPath = keyPath,
                ValueName = valueName,
                ValueKind = valueKind.ToString(),
                BackupTime = DateTime.UtcNow,
                StringValue = value?.ToString()
            }
        };
    }
}
