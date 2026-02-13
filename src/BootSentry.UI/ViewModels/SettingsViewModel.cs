using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using BootSentry.Core.Interfaces;
using BootSentry.Core.Services.Integrations;
using BootSentry.UI.Resources;
using BootSentry.UI.Services;

namespace BootSentry.UI.ViewModels;

/// <summary>
/// View model for application settings.
/// </summary>
public partial class SettingsViewModel : ObservableObject
{
    private readonly ILogger<SettingsViewModel> _logger;
    private readonly SettingsService _settingsService;
    private readonly ThemeService _themeService;
    private readonly VirusTotalService _virusTotalService;
    private readonly IDialogService _dialogService;
    private readonly IProcessLauncher _processLauncher;
    private bool _isInitializing = true;

    // Design-time constructor
    public SettingsViewModel() : this(null!, null!, null!, null!, null!, null!) { }

    [ObservableProperty]
    private string _selectedLanguage = "fr";

    [ObservableProperty]
    private ThemeMode _selectedTheme = ThemeMode.System;

    [ObservableProperty]
    private bool _isExpertModeDefault;

    [ObservableProperty]
    private bool _checkForUpdatesOnStartup = true;

    [ObservableProperty]
    private int _backupRetentionDays = 30;

    [ObservableProperty]
    private bool _autoCalculateHashes;

    [ObservableProperty]
    private bool _showOnboarding = true;

    [ObservableProperty]
    private string? _virusTotalApiKey;

    [ObservableProperty]
    private string _statusMessage = string.Empty;

    public ObservableCollection<LanguageItem> Languages { get; } =
    [
        new LanguageItem("fr", "Français"),
        new LanguageItem("en", "English")
    ];

    public ObservableCollection<ThemeItem> Themes { get; } = new();

    public SettingsViewModel(
        ILogger<SettingsViewModel> logger,
        SettingsService settingsService,
        ThemeService themeService,
        VirusTotalService virusTotalService,
        IDialogService dialogService,
        IProcessLauncher processLauncher)
    {
        _logger = logger;
        _settingsService = settingsService;
        _themeService = themeService;
        _virusTotalService = virusTotalService;
        _dialogService = dialogService;
        _processLauncher = processLauncher;

        UpdateThemeList();
        LoadSettings();
        _isInitializing = false;
    }

    private void UpdateThemeList()
    {
        Themes.Clear();
        Themes.Add(new ThemeItem(ThemeMode.System, Strings.Get("SettingsThemeSystem")));
        Themes.Add(new ThemeItem(ThemeMode.Light, Strings.Get("SettingsThemeLight")));
        Themes.Add(new ThemeItem(ThemeMode.Dark, Strings.Get("SettingsThemeDark")));
    }

    private void LoadSettings()
    {
        var settings = _settingsService.Settings;

        SelectedLanguage = settings.Language;
        SelectedTheme = settings.Theme;
        IsExpertModeDefault = settings.ExpertModeDefault;
        CheckForUpdatesOnStartup = settings.CheckUpdatesOnStartup;
        BackupRetentionDays = settings.BackupRetentionDays;
        AutoCalculateHashes = settings.AutoCalculateHashes;
        ShowOnboarding = settings.ShowOnboarding;
        VirusTotalApiKey = settings.VirusTotalApiKey;
    }

    private bool SaveSettingsOrReportError()
    {
        if (_settingsService.Save())
            return true;

        StatusMessage = Strings.Get("SettingsSaveFailed");
        _logger.LogWarning("Failed to persist settings");
        return false;
    }

    partial void OnSelectedLanguageChanged(string value)
    {
        if (_isInitializing || string.IsNullOrEmpty(value))
            return;

        var previousLanguage = Strings.CurrentLanguage;
        Strings.CurrentLanguage = value;
        _settingsService.Settings.Language = value;
        var saved = SaveSettingsOrReportError();

        UpdateThemeList();

        if (previousLanguage != value)
        {
            if (_dialogService.Confirm(
                Strings.Get("SettingsLanguageRestartPrompt"),
                Strings.Get("SettingsLanguageChangeTitle")))
            {
                RestartApplication();
            }
        }

        if (saved)
        {
            StatusMessage = Strings.Get("SettingsLanguage") + ": " + value;
        }
    }

    private void RestartApplication()
    {
        var exePath = Environment.ProcessPath;
        if (exePath != null)
        {
            _processLauncher.OpenFile(exePath);
            System.Windows.Application.Current.Shutdown();
        }
    }

    partial void OnSelectedThemeChanged(ThemeMode value)
    {
        if (_isInitializing)
            return;

        _themeService.CurrentTheme = value;
        _settingsService.Settings.Theme = value;
        SaveSettingsOrReportError();
    }

    partial void OnIsExpertModeDefaultChanged(bool value)
    {
        _settingsService.Settings.ExpertModeDefault = value;
        SaveSettingsOrReportError();
    }

    partial void OnCheckForUpdatesOnStartupChanged(bool value)
    {
        _settingsService.Settings.CheckUpdatesOnStartup = value;
        SaveSettingsOrReportError();
    }

    partial void OnBackupRetentionDaysChanged(int value)
    {
        _settingsService.Settings.BackupRetentionDays = value;
        SaveSettingsOrReportError();
    }

    partial void OnAutoCalculateHashesChanged(bool value)
    {
        _settingsService.Settings.AutoCalculateHashes = value;
        SaveSettingsOrReportError();
    }

    partial void OnShowOnboardingChanged(bool value)
    {
        _settingsService.Settings.ShowOnboarding = value;
        SaveSettingsOrReportError();
    }

    partial void OnVirusTotalApiKeyChanged(string? value)
    {
        _settingsService.Settings.VirusTotalApiKey = value;
        if (value != null) _virusTotalService.SetApiKey(value);
        SaveSettingsOrReportError();
    }

    [RelayCommand]
    private void ResetToDefaults()
    {
        var confirmDialog = Views.ConfirmationDialog.ForReset(System.Windows.Application.Current.MainWindow);
        if (confirmDialog.ShowDialog() != true)
            return;

        var currentLanguage = Strings.CurrentLanguage;

        if (!_settingsService.Reset())
        {
            StatusMessage = Strings.Get("SettingsResetFailed");
            return;
        }

        _settingsService.Settings.Language = currentLanguage;
        if (!_settingsService.Save())
        {
            StatusMessage = Strings.Get("SettingsSaveFailed");
            return;
        }

        LoadSettings();
        _themeService.CurrentTheme = ThemeMode.System;
        Strings.CurrentLanguage = currentLanguage;
        StatusMessage = Strings.Get("SettingsResetDone");
        _logger.LogInformation("Settings reset to defaults");
    }

    [RelayCommand]
    private void PurgeAllData()
    {
        var confirmDialog = Views.ConfirmationDialog.ForPurge(System.Windows.Application.Current.MainWindow);
        if (confirmDialog.ShowDialog() != true)
            return;

        if (!_settingsService.PurgeAllData())
        {
            StatusMessage = Strings.Get("SettingsPurgeFailed");
            return;
        }

        LoadSettings();
        StatusMessage = Strings.Get("SettingsPurgeDone");
        _logger.LogInformation("All application data purged");
    }
}

public record LanguageItem(string Code, string Name);
public record ThemeItem(ThemeMode Mode, string Name);
