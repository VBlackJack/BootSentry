using CommunityToolkit.Mvvm.ComponentModel;

namespace BootSentry.UI.ViewModels;

/// <summary>
/// View model for application settings.
/// </summary>
public partial class SettingsViewModel : ObservableObject
{
    [ObservableProperty]
    private bool _isExpertModeDefault;

    [ObservableProperty]
    private bool _checkForUpdates = true;

    [ObservableProperty]
    private int _backupRetentionDays = 30;

    [ObservableProperty]
    private int _maxBackupCount = 100;

    [ObservableProperty]
    private bool _calculateHashesAutomatically;

    [ObservableProperty]
    private string _selectedLanguage = "fr-FR";
}
