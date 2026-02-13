using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
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
    private bool _isInitializing = true;

    // Design-time constructor
    public SettingsViewModel() : this(null!, null!, null!) { }

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
        ThemeService themeService)
    {
        _logger = logger;
        _settingsService = settingsService;
        _themeService = themeService;

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
    }

    partial void OnSelectedLanguageChanged(string value)
    {
        if (_isInitializing || string.IsNullOrEmpty(value))
            return;

        var previousLanguage = Strings.CurrentLanguage;
        Strings.CurrentLanguage = value;
        _settingsService.Settings.Language = value;
        _settingsService.Save();

        UpdateThemeList();

        if (previousLanguage != value)
        {
            var result = System.Windows.MessageBox.Show(
                Strings.Get("SettingsLanguageRestartPrompt"),
                Strings.Get("SettingsLanguageChangeTitle"),
                System.Windows.MessageBoxButton.YesNo,
                System.Windows.MessageBoxImage.Question);

            if (result == System.Windows.MessageBoxResult.Yes)
            {
                RestartApplication();
            }
        }

        StatusMessage = Strings.Get("SettingsLanguage") + ": " + value;
    }

    private static void RestartApplication()
    {
        var exePath = Environment.ProcessPath;
        if (exePath != null)
        {
            System.Diagnostics.Process.Start(exePath);
            System.Windows.Application.Current.Shutdown();
        }
    }

    partial void OnSelectedThemeChanged(ThemeMode value)
    {
        if (_isInitializing)
            return;

        _themeService.CurrentTheme = value;
        _settingsService.Settings.Theme = value;
        _settingsService.Save();
    }

    partial void OnIsExpertModeDefaultChanged(bool value)
    {
        _settingsService.Settings.ExpertModeDefault = value;
        _settingsService.Save();
    }

    partial void OnCheckForUpdatesOnStartupChanged(bool value)
    {
        _settingsService.Settings.CheckUpdatesOnStartup = value;
        _settingsService.Save();
    }

    partial void OnBackupRetentionDaysChanged(int value)
    {
        _settingsService.Settings.BackupRetentionDays = value;
        _settingsService.Save();
    }

    partial void OnAutoCalculateHashesChanged(bool value)
    {
        _settingsService.Settings.AutoCalculateHashes = value;
        _settingsService.Save();
    }

    partial void OnShowOnboardingChanged(bool value)
    {
        _settingsService.Settings.ShowOnboarding = value;
        _settingsService.Save();
    }

    [RelayCommand]
    private void ResetToDefaults()
    {
        var confirmDialog = Views.ConfirmationDialog.ForReset(System.Windows.Application.Current.MainWindow);
        if (confirmDialog.ShowDialog() != true)
            return;

        var currentLanguage = Strings.CurrentLanguage;

        _settingsService.Reset();
        _settingsService.Settings.Language = currentLanguage;
        _settingsService.Save();
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

        _settingsService.PurgeAllData();
        LoadSettings();
        StatusMessage = Strings.Get("SettingsPurgeDone");
        _logger.LogInformation("All application data purged");
    }
}

public record LanguageItem(string Code, string Name);
public record ThemeItem(ThemeMode Mode, string Name);
