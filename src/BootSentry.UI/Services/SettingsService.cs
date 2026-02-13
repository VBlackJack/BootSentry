using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using BootSentry.Core;
using BootSentry.Core.Interfaces;
using Microsoft.Extensions.Logging;

namespace BootSentry.UI.Services;

/// <summary>
/// Service for managing application settings.
/// </summary>
public class SettingsService : ISettingsService
{
    private readonly ILogger<SettingsService> _logger;
    private readonly string _settingsPath;
    private AppSettings _settings;

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        WriteIndented = true,
        Converters = { new JsonStringEnumConverter() }
    };

    public SettingsService(ILogger<SettingsService> logger)
    {
        _logger = logger;

        var appDataPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            Constants.AppName);

        Directory.CreateDirectory(appDataPath);
        _settingsPath = Path.Combine(appDataPath, Constants.Files.Settings);
        _settings = new AppSettings();
    }

    /// <summary>
    /// Gets the current settings.
    /// </summary>
    public AppSettings Settings => _settings;

    /// <summary>
    /// Loads settings from disk.
    /// </summary>
    public bool Load()
    {
        try
        {
            if (File.Exists(_settingsPath))
            {
                var json = File.ReadAllText(_settingsPath);
                _settings = JsonSerializer.Deserialize<AppSettings>(json, JsonOptions) ?? new AppSettings();
            }

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to load settings from {Path}; using defaults", _settingsPath);
            _settings = new AppSettings();
            return false;
        }
    }

    /// <summary>
    /// Saves settings to disk.
    /// </summary>
    public bool Save()
    {
        try
        {
            var json = JsonSerializer.Serialize(_settings, JsonOptions);
            File.WriteAllText(_settingsPath, json);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to save settings to {Path}", _settingsPath);
            return false;
        }
    }

    /// <summary>
    /// Resets settings to defaults.
    /// </summary>
    public bool Reset()
    {
        _settings = new AppSettings();
        return Save();
    }

    /// <summary>
    /// Purges all application data.
    /// </summary>
    public bool PurgeAllData()
    {
        var success = true;

        // Reset settings
        if (!Reset())
        {
            success = false;
        }

        // Delete logs
        try
        {
            var logsPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
                Constants.AppName, Constants.Directories.Logs);
            if (Directory.Exists(logsPath))
            {
                Directory.Delete(logsPath, true);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to purge logs");
            success = false;
        }

        // Delete backups
        try
        {
            var backupsPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
                Constants.AppName, Constants.Directories.Backups);
            if (Directory.Exists(backupsPath))
            {
                Directory.Delete(backupsPath, true);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to purge backups");
            success = false;
        }

        // Delete local app data
        try
        {
            var localPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                Constants.AppName);
            if (Directory.Exists(localPath))
            {
                // Keep settings file, delete others
                foreach (var file in Directory.GetFiles(localPath))
                {
                    if (!file.EndsWith(Constants.Files.Settings))
                    {
                        try
                        {
                            File.Delete(file);
                        }
                        catch (Exception ex)
                        {
                            _logger.LogWarning(ex, "Failed to delete local data file: {File}", file);
                            success = false;
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to purge local app data");
            success = false;
        }

        return success;
    }
}

/// <summary>
/// Application settings model.
/// </summary>
public class AppSettings
{
    /// <summary>
    /// Current language code (fr, en).
    /// </summary>
    public string Language { get; set; } = Constants.Defaults.Language;

    /// <summary>
    /// Theme mode (System, Light, Dark).
    /// </summary>
    public ThemeMode Theme { get; set; } = ThemeMode.System;

    /// <summary>
    /// Whether to start in expert mode.
    /// </summary>
    public bool ExpertModeDefault { get; set; } = false;

    /// <summary>
    /// Whether to check for updates on startup.
    /// </summary>
    public bool CheckUpdatesOnStartup { get; set; } = true;

    /// <summary>
    /// Days to retain backups (0 = forever).
    /// </summary>
    public int BackupRetentionDays { get; set; } = Constants.Defaults.BackupRetentionDays;

    /// <summary>
    /// Whether to show onboarding on first launch.
    /// </summary>
    public bool ShowOnboarding { get; set; } = true;

    /// <summary>
    /// Versions to ignore for update checks.
    /// </summary>
    public List<string> IgnoredVersions { get; set; } = new();

    /// <summary>
    /// Window position and size.
    /// </summary>
    public WindowState? Window { get; set; }

    /// <summary>
    /// Last scan date.
    /// </summary>
    public DateTime? LastScanDate { get; set; }

    /// <summary>
    /// Whether to calculate hashes automatically.
    /// </summary>
    public bool AutoCalculateHashes { get; set; } = false;

    /// <summary>
    /// Column widths for the entry list.
    /// </summary>
    public Dictionary<string, double>? ColumnWidths { get; set; }

    /// <summary>
    /// Whether to hide Microsoft system entries (Services/Drivers) from the list.
    /// </summary>
    public bool HideMicrosoftEntries { get; set; } = Constants.Defaults.HideMicrosoftEntries;

    /// <summary>
    /// Whether to enable real-time startup monitoring.
    /// </summary>
    public bool EnableRealTimeMonitoring { get; set; } = false;

    /// <summary>
    /// API Key for VirusTotal integration (Encrypted in storage).
    /// </summary>
    [JsonIgnore]
    public string? VirusTotalApiKey { get; set; }

    [JsonPropertyName("VirusTotalApiKey")]
    public string? VirusTotalApiKeyEncrypted
    {
        get
        {
            if (string.IsNullOrEmpty(VirusTotalApiKey)) return null;
            try
            {
                var bytes = System.Text.Encoding.UTF8.GetBytes(VirusTotalApiKey);
                var protectedBytes = System.Security.Cryptography.ProtectedData.Protect(
                    bytes, null, System.Security.Cryptography.DataProtectionScope.CurrentUser);
                return Convert.ToBase64String(protectedBytes);
            }
            catch { return null; }
        }
        set
        {
            if (string.IsNullOrEmpty(value))
            {
                VirusTotalApiKey = null;
                return;
            }

            try
            {
                var bytes = Convert.FromBase64String(value);
                var unprotectedBytes = System.Security.Cryptography.ProtectedData.Unprotect(
                    bytes, null, System.Security.Cryptography.DataProtectionScope.CurrentUser);
                VirusTotalApiKey = System.Text.Encoding.UTF8.GetString(unprotectedBytes);
            }
            catch
            {
                // Do not accept cleartext fallback for security reasons
                VirusTotalApiKey = null;
            }
        }
    }
}

/// <summary>
/// Window state for persistence.
/// </summary>
public class WindowState
{
    public double Left { get; set; }
    public double Top { get; set; }
    public double Width { get; set; }
    public double Height { get; set; }
    public bool IsMaximized { get; set; }
}
