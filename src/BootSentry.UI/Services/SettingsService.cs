using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace BootSentry.UI.Services;

/// <summary>
/// Service for managing application settings.
/// </summary>
public class SettingsService
{
    private readonly string _settingsPath;
    private AppSettings _settings;

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        WriteIndented = true,
        Converters = { new JsonStringEnumConverter() }
    };

    public SettingsService()
    {
        var appDataPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "BootSentry");

        Directory.CreateDirectory(appDataPath);
        _settingsPath = Path.Combine(appDataPath, "settings.json");
        _settings = new AppSettings();
    }

    /// <summary>
    /// Gets the current settings.
    /// </summary>
    public AppSettings Settings => _settings;

    /// <summary>
    /// Loads settings from disk.
    /// </summary>
    public void Load()
    {
        try
        {
            if (File.Exists(_settingsPath))
            {
                var json = File.ReadAllText(_settingsPath);
                _settings = JsonSerializer.Deserialize<AppSettings>(json, JsonOptions) ?? new AppSettings();
            }
        }
        catch
        {
            _settings = new AppSettings();
        }
    }

    /// <summary>
    /// Saves settings to disk.
    /// </summary>
    public void Save()
    {
        try
        {
            var json = JsonSerializer.Serialize(_settings, JsonOptions);
            File.WriteAllText(_settingsPath, json);
        }
        catch
        {
            // Ignore save errors
        }
    }

    /// <summary>
    /// Resets settings to defaults.
    /// </summary>
    public void Reset()
    {
        _settings = new AppSettings();
        Save();
    }

    /// <summary>
    /// Purges all application data.
    /// </summary>
    public void PurgeAllData()
    {
        try
        {
            // Reset settings
            Reset();

            // Delete logs
            var logsPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
                "BootSentry", "Logs");
            if (Directory.Exists(logsPath))
                Directory.Delete(logsPath, true);

            // Delete backups
            var backupsPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
                "BootSentry", "Backups");
            if (Directory.Exists(backupsPath))
                Directory.Delete(backupsPath, true);

            // Delete local app data
            var localPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "BootSentry");
            if (Directory.Exists(localPath))
            {
                // Keep settings file, delete others
                foreach (var file in Directory.GetFiles(localPath))
                {
                    if (!file.EndsWith("settings.json"))
                        File.Delete(file);
                }
            }
        }
        catch
        {
            // Ignore errors
        }
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
    public string Language { get; set; } = "fr";

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
    public int BackupRetentionDays { get; set; } = 30;

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
