using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Data;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using Microsoft.Win32;
using BootSentry.Actions;
using BootSentry.Core.Enums;
using BootSentry.Core.Helpers;
using BootSentry.Core.Interfaces;
using BootSentry.Core.Models;
using BootSentry.Core.Services;

namespace BootSentry.UI.ViewModels;

/// <summary>
/// Main view model for the application.
/// </summary>
public partial class MainViewModel : ObservableObject
{
    private readonly ILogger<MainViewModel> _logger;
    private readonly IEnumerable<IStartupProvider> _providers;
    private readonly ActionExecutor _actionExecutor;
    private readonly ICollectionView _entriesView;
    private readonly ExportService _exportService;
    private readonly RiskAnalyzer _riskAnalyzer;

    [ObservableProperty]
    private bool _isExpertMode;

    [ObservableProperty]
    private string _searchText = string.Empty;

    [ObservableProperty]
    private StartupEntry? _selectedEntry;

    [ObservableProperty]
    private string _statusMessage = "Prêt";

    [ObservableProperty]
    private bool _isLoading;

    [ObservableProperty]
    private bool _isAdmin;

    [ObservableProperty]
    private string _adminStatusText = string.Empty;

    public ObservableCollection<StartupEntry> Entries { get; } = [];

    public ICollectionView FilteredEntries => _entriesView;

    public int VisibleEntriesCount => _entriesView.Cast<object>().Count();
    public int TotalEntriesCount => Entries.Count;
    public int EnabledCount => Entries.Count(e => e.Status == EntryStatus.Enabled);
    public int DisabledCount => Entries.Count(e => e.Status == EntryStatus.Disabled);
    public int SuspiciousCount => Entries.Count(e => e.RiskLevel == RiskLevel.Suspicious || e.RiskLevel == RiskLevel.Critical);
    public bool HasSuspiciousEntries => SuspiciousCount > 0;

    public MainViewModel(
        ILogger<MainViewModel> logger,
        IEnumerable<IStartupProvider> providers,
        ActionExecutor actionExecutor)
    {
        _logger = logger;
        _providers = providers;
        _actionExecutor = actionExecutor;
        _exportService = new ExportService();
        _riskAnalyzer = new RiskAnalyzer();

        _entriesView = CollectionViewSource.GetDefaultView(Entries);
        _entriesView.Filter = FilterEntries;

        // Check admin status
        IsAdmin = UacHelper.IsRunningAsAdmin();
        AdminStatusText = IsAdmin ? "Administrateur" : "Standard";

        // Load entries on startup
        _ = RefreshAsync();
    }

    partial void OnSearchTextChanged(string value)
    {
        _entriesView.Refresh();
        OnPropertyChanged(nameof(VisibleEntriesCount));
    }

    partial void OnIsExpertModeChanged(bool value)
    {
        _entriesView.Refresh();
        OnPropertyChanged(nameof(VisibleEntriesCount));
        StatusMessage = value ? "Mode Expert activé" : "Mode Expert désactivé";
        _logger.LogInformation("Expert mode: {Mode}", value ? "enabled" : "disabled");
    }

    private bool FilterEntries(object obj)
    {
        if (obj is not StartupEntry entry)
            return false;

        // In non-expert mode, hide Microsoft entries and critical items
        if (!IsExpertMode)
        {
            if (entry.Publisher?.Contains("Microsoft", StringComparison.OrdinalIgnoreCase) == true
                && entry.SignatureStatus == SignatureStatus.SignedTrusted)
                return false;

            if (entry.IsProtected)
                return false;

            // Hide drivers in non-expert mode
            if (entry.Type == EntryType.Driver)
                return false;
        }

        // Apply search filter
        if (!string.IsNullOrWhiteSpace(SearchText))
        {
            var search = SearchText.ToLowerInvariant();
            return entry.DisplayName.Contains(search, StringComparison.OrdinalIgnoreCase)
                || entry.Publisher?.Contains(search, StringComparison.OrdinalIgnoreCase) == true
                || entry.TargetPath?.Contains(search, StringComparison.OrdinalIgnoreCase) == true
                || entry.CommandLineRaw?.Contains(search, StringComparison.OrdinalIgnoreCase) == true
                || entry.Type.ToString().Contains(search, StringComparison.OrdinalIgnoreCase);
        }

        return true;
    }

    [RelayCommand]
    private async Task RefreshAsync()
    {
        IsLoading = true;
        StatusMessage = "Analyse en cours...";

        try
        {
            _logger.LogInformation("Starting scan with {Count} providers", _providers.Count());

            Entries.Clear();
            var totalEntries = 0;

            foreach (var provider in _providers)
            {
                if (!provider.IsAvailable())
                {
                    _logger.LogWarning("Provider {Provider} is not available", provider.DisplayName);
                    continue;
                }

                try
                {
                    StatusMessage = $"Analyse: {provider.DisplayName}...";
                    var providerEntries = await provider.ScanAsync();

                    foreach (var entry in providerEntries)
                    {
                        // Apply risk analysis
                        _riskAnalyzer.UpdateRiskLevel(entry);
                        Entries.Add(entry);
                        totalEntries++;
                    }

                    _logger.LogInformation("Provider {Provider} found {Count} entries",
                        provider.DisplayName, providerEntries.Count);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error scanning with provider {Provider}", provider.DisplayName);
                }
            }

            _entriesView.Refresh();
            UpdateCounts();

            StatusMessage = $"Scan terminé - {totalEntries} entrées trouvées";
            _logger.LogInformation("Scan complete: {Count} entries found", totalEntries);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during scan");
            StatusMessage = $"Erreur: {ex.Message}";
        }
        finally
        {
            IsLoading = false;
        }
    }

    private void UpdateCounts()
    {
        OnPropertyChanged(nameof(VisibleEntriesCount));
        OnPropertyChanged(nameof(TotalEntriesCount));
        OnPropertyChanged(nameof(EnabledCount));
        OnPropertyChanged(nameof(DisabledCount));
        OnPropertyChanged(nameof(SuspiciousCount));
        OnPropertyChanged(nameof(HasSuspiciousEntries));
    }

    [RelayCommand]
    private void FocusSearch()
    {
        // This would be handled in the view's code-behind
    }

    [RelayCommand]
    private void ToggleExpertMode()
    {
        IsExpertMode = !IsExpertMode;
    }

    [RelayCommand]
    private void ClearSelection()
    {
        SelectedEntry = null;
    }

    [RelayCommand]
    private async Task DisableSelectedAsync()
    {
        if (SelectedEntry == null)
            return;

        if (SelectedEntry.IsProtected)
        {
            MessageBox.Show(
                $"Cette entrée est protégée: {SelectedEntry.ProtectionReason}",
                "Action impossible",
                MessageBoxButton.OK,
                MessageBoxImage.Warning);
            return;
        }

        // Check if admin is required
        if (_actionExecutor.RequiresAdmin(SelectedEntry, ActionType.Disable) && !IsAdmin)
        {
            var result = MessageBox.Show(
                "Cette action nécessite des droits administrateur.\n\nVoulez-vous relancer l'application en tant qu'administrateur?",
                "Élévation requise",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                if (UacHelper.RestartAsAdmin())
                {
                    Application.Current.Shutdown();
                }
            }
            return;
        }

        _logger.LogInformation("Disabling entry: {Id}", SelectedEntry.Id);
        StatusMessage = $"Désactivation de {SelectedEntry.DisplayName}...";

        var actionResult = await _actionExecutor.DisableAsync(SelectedEntry);

        if (actionResult.Success)
        {
            _entriesView.Refresh();
            UpdateCounts();
            StatusMessage = $"{SelectedEntry.DisplayName} désactivé";
        }
        else
        {
            StatusMessage = $"Erreur: {actionResult.ErrorMessage}";
            MessageBox.Show(
                actionResult.ErrorMessage,
                "Erreur",
                MessageBoxButton.OK,
                MessageBoxImage.Error);
        }
    }

    [RelayCommand]
    private async Task EnableSelectedAsync()
    {
        if (SelectedEntry == null)
            return;

        // Check if admin is required
        if (_actionExecutor.RequiresAdmin(SelectedEntry, ActionType.Enable) && !IsAdmin)
        {
            var result = MessageBox.Show(
                "Cette action nécessite des droits administrateur.\n\nVoulez-vous relancer l'application en tant qu'administrateur?",
                "Élévation requise",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                if (UacHelper.RestartAsAdmin())
                {
                    Application.Current.Shutdown();
                }
            }
            return;
        }

        _logger.LogInformation("Enabling entry: {Id}", SelectedEntry.Id);
        StatusMessage = $"Activation de {SelectedEntry.DisplayName}...";

        var actionResult = await _actionExecutor.EnableAsync(SelectedEntry);

        if (actionResult.Success)
        {
            _entriesView.Refresh();
            UpdateCounts();
            StatusMessage = $"{SelectedEntry.DisplayName} activé";
        }
        else
        {
            StatusMessage = $"Erreur: {actionResult.ErrorMessage}";
            MessageBox.Show(
                actionResult.ErrorMessage,
                "Erreur",
                MessageBoxButton.OK,
                MessageBoxImage.Error);
        }
    }

    [RelayCommand]
    private async Task DeleteSelectedAsync()
    {
        if (SelectedEntry == null || !IsExpertMode)
            return;

        if (SelectedEntry.IsProtected)
        {
            MessageBox.Show(
                $"Cette entrée est protégée: {SelectedEntry.ProtectionReason}",
                "Action impossible",
                MessageBoxButton.OK,
                MessageBoxImage.Warning);
            return;
        }

        // Confirmation dialog
        var confirmResult = MessageBox.Show(
            $"Êtes-vous sûr de vouloir supprimer '{SelectedEntry.DisplayName}'?\n\n" +
            "Un backup sera créé et permettra de restaurer cette entrée.",
            "Confirmation de suppression",
            MessageBoxButton.YesNo,
            MessageBoxImage.Warning);

        if (confirmResult != MessageBoxResult.Yes)
            return;

        // Check if admin is required
        if (_actionExecutor.RequiresAdmin(SelectedEntry, ActionType.Delete) && !IsAdmin)
        {
            var result = MessageBox.Show(
                "Cette action nécessite des droits administrateur.\n\nVoulez-vous relancer l'application en tant qu'administrateur?",
                "Élévation requise",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                if (UacHelper.RestartAsAdmin())
                {
                    Application.Current.Shutdown();
                }
            }
            return;
        }

        _logger.LogInformation("Deleting entry: {Id}", SelectedEntry.Id);
        StatusMessage = $"Suppression de {SelectedEntry.DisplayName}...";

        var actionResult = await _actionExecutor.DeleteAsync(SelectedEntry);

        if (actionResult.Success)
        {
            var entryName = SelectedEntry.DisplayName;
            Entries.Remove(SelectedEntry);
            SelectedEntry = null;

            _entriesView.Refresh();
            UpdateCounts();

            StatusMessage = $"{entryName} supprimé (backup créé)";
        }
        else
        {
            StatusMessage = $"Erreur: {actionResult.ErrorMessage}";
            MessageBox.Show(
                actionResult.ErrorMessage,
                "Erreur",
                MessageBoxButton.OK,
                MessageBoxImage.Error);
        }
    }

    [RelayCommand]
    private void OpenFileLocation()
    {
        if (SelectedEntry?.TargetPath == null)
            return;

        try
        {
            var directory = Path.GetDirectoryName(SelectedEntry.TargetPath);
            if (directory != null && Directory.Exists(directory))
            {
                if (File.Exists(SelectedEntry.TargetPath))
                {
                    Process.Start("explorer.exe", $"/select,\"{SelectedEntry.TargetPath}\"");
                }
                else
                {
                    Process.Start("explorer.exe", directory);
                }
            }
            else
            {
                MessageBox.Show(
                    "Le dossier cible n'existe pas.",
                    "Dossier introuvable",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error opening file location");
            MessageBox.Show(
                $"Erreur lors de l'ouverture: {ex.Message}",
                "Erreur",
                MessageBoxButton.OK,
                MessageBoxImage.Error);
        }
    }

    [RelayCommand]
    private void CopyPath()
    {
        if (SelectedEntry?.TargetPath != null)
        {
            Clipboard.SetText(SelectedEntry.TargetPath);
            StatusMessage = "Chemin copié dans le presse-papiers";
        }
        else if (SelectedEntry?.CommandLineRaw != null)
        {
            Clipboard.SetText(SelectedEntry.CommandLineRaw);
            StatusMessage = "Commande copiée dans le presse-papiers";
        }
    }

    [RelayCommand]
    private void WebSearch()
    {
        if (SelectedEntry == null)
            return;

        var query = Uri.EscapeDataString($"{SelectedEntry.DisplayName} {SelectedEntry.Publisher ?? ""}");
        var url = $"https://www.google.com/search?q={query}";

        try
        {
            Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error opening web search");
        }
    }

    [RelayCommand]
    private async Task ExportJsonAsync()
    {
        var dialog = new Microsoft.Win32.SaveFileDialog
        {
            Filter = "JSON Files (*.json)|*.json",
            DefaultExt = ".json",
            FileName = $"BootSentry_Export_{DateTime.Now:yyyyMMdd_HHmmss}"
        };

        if (dialog.ShowDialog() == true)
        {
            try
            {
                StatusMessage = "Exportation...";
                var options = new ExportOptions { IncludeDetails = true };
                await _exportService.ExportToFileAsync(Entries, dialog.FileName, ExportFormat.Json, options);
                StatusMessage = $"Export terminé: {dialog.FileName}";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error exporting to JSON");
                StatusMessage = $"Erreur d'export: {ex.Message}";
            }
        }
    }

    [RelayCommand]
    private async Task ExportCsvAsync()
    {
        var dialog = new Microsoft.Win32.SaveFileDialog
        {
            Filter = "CSV Files (*.csv)|*.csv",
            DefaultExt = ".csv",
            FileName = $"BootSentry_Export_{DateTime.Now:yyyyMMdd_HHmmss}"
        };

        if (dialog.ShowDialog() == true)
        {
            try
            {
                StatusMessage = "Exportation...";
                var options = new ExportOptions { IncludeDetails = true };
                await _exportService.ExportToFileAsync(Entries, dialog.FileName, ExportFormat.Csv, options);
                StatusMessage = $"Export terminé: {dialog.FileName}";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error exporting to CSV");
                StatusMessage = $"Erreur d'export: {ex.Message}";
            }
        }
    }

    [RelayCommand]
    private void Export()
    {
        // Default to JSON export
        ExportJsonCommand.Execute(null);
    }

    [RelayCommand]
    private void OpenSettings()
    {
        var settingsWindow = new Views.SettingsView
        {
            Owner = Application.Current.MainWindow
        };
        settingsWindow.ShowDialog();
    }

    [RelayCommand]
    private void ShowHistory()
    {
        var historyWindow = new Views.HistoryView
        {
            Owner = Application.Current.MainWindow
        };
        historyWindow.ShowDialog();
    }

    [RelayCommand]
    private void Undo()
    {
        // TODO: Implement undo functionality
        StatusMessage = "Annulation non disponible";
    }

    [RelayCommand]
    private void ShowHelp()
    {
        // Open documentation
        ShowDocumentationCommand.Execute(null);
    }

    [RelayCommand]
    private void ShowDocumentation()
    {
        try
        {
            Process.Start(new ProcessStartInfo("https://github.com/your-username/BootSentry") { UseShellExecute = true });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error opening documentation");
        }
    }

    [RelayCommand]
    private async Task CheckUpdatesAsync()
    {
        try
        {
            StatusMessage = "Vérification des mises à jour...";
            using var checker = new UpdateChecker();
            var update = await checker.CheckForUpdateAsync();

            if (update != null)
            {
                var result = MessageBox.Show(
                    $"La version {update.LatestVersion} est disponible.\n\nVoulez-vous la télécharger?",
                    "Mise à jour disponible",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Information);

                if (result == MessageBoxResult.Yes)
                {
                    Process.Start(new ProcessStartInfo(update.ReleaseUrl) { UseShellExecute = true });
                }
            }
            else
            {
                MessageBox.Show(
                    "Vous utilisez la dernière version.",
                    "Aucune mise à jour",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }

            StatusMessage = "Prêt";
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking for updates");
            StatusMessage = "Erreur lors de la vérification des mises à jour";
        }
    }

    [RelayCommand]
    private void ShowAbout()
    {
        MessageBox.Show(
            $"BootSentry v{UpdateChecker.CurrentVersion}\n\n" +
            "Gestionnaire de démarrage Windows\n" +
            "Safe and user-friendly\n\n" +
            "© 2025 Julien Bombled\n" +
            "Apache License 2.0",
            "À propos de BootSentry",
            MessageBoxButton.OK,
            MessageBoxImage.Information);
    }

    [RelayCommand]
    private void Exit()
    {
        Application.Current.Shutdown();
    }
}
