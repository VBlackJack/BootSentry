using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Data;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using BootSentry.Actions;
using BootSentry.Core.Enums;
using BootSentry.Core.Interfaces;
using BootSentry.Core.Models;

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

    public ObservableCollection<StartupEntry> Entries { get; } = [];

    public ICollectionView FilteredEntries => _entriesView;

    public int VisibleEntriesCount => _entriesView.Cast<object>().Count();
    public int EnabledCount => Entries.Count(e => e.Status == EntryStatus.Enabled);
    public int DisabledCount => Entries.Count(e => e.Status == EntryStatus.Disabled);

    public MainViewModel(
        ILogger<MainViewModel> logger,
        IEnumerable<IStartupProvider> providers,
        ActionExecutor actionExecutor)
    {
        _logger = logger;
        _providers = providers;
        _actionExecutor = actionExecutor;

        _entriesView = CollectionViewSource.GetDefaultView(Entries);
        _entriesView.Filter = FilterEntries;

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
        }

        // Apply search filter
        if (!string.IsNullOrWhiteSpace(SearchText))
        {
            var search = SearchText.ToLowerInvariant();
            return entry.DisplayName.Contains(search, StringComparison.OrdinalIgnoreCase)
                || entry.Publisher?.Contains(search, StringComparison.OrdinalIgnoreCase) == true
                || entry.TargetPath?.Contains(search, StringComparison.OrdinalIgnoreCase) == true
                || entry.CommandLineRaw?.Contains(search, StringComparison.OrdinalIgnoreCase) == true;
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
            OnPropertyChanged(nameof(VisibleEntriesCount));
            OnPropertyChanged(nameof(EnabledCount));
            OnPropertyChanged(nameof(DisabledCount));

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

    [RelayCommand]
    private void Search()
    {
        // Focus search box - handled in view
    }

    [RelayCommand]
    private void ToggleExpertMode()
    {
        IsExpertMode = !IsExpertMode;
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

        _logger.LogInformation("Disabling entry: {Id}", SelectedEntry.Id);
        StatusMessage = $"Désactivation de {SelectedEntry.DisplayName}...";

        var result = await _actionExecutor.DisableAsync(SelectedEntry);

        if (result.Success)
        {
            _entriesView.Refresh();
            OnPropertyChanged(nameof(EnabledCount));
            OnPropertyChanged(nameof(DisabledCount));
            StatusMessage = $"{SelectedEntry.DisplayName} désactivé";
        }
        else
        {
            StatusMessage = $"Erreur: {result.ErrorMessage}";
            MessageBox.Show(
                result.ErrorMessage,
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

        _logger.LogInformation("Enabling entry: {Id}", SelectedEntry.Id);
        StatusMessage = $"Activation de {SelectedEntry.DisplayName}...";

        var result = await _actionExecutor.EnableAsync(SelectedEntry);

        if (result.Success)
        {
            _entriesView.Refresh();
            OnPropertyChanged(nameof(EnabledCount));
            OnPropertyChanged(nameof(DisabledCount));
            StatusMessage = $"{SelectedEntry.DisplayName} activé";
        }
        else
        {
            StatusMessage = $"Erreur: {result.ErrorMessage}";
            MessageBox.Show(
                result.ErrorMessage,
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

        _logger.LogInformation("Deleting entry: {Id}", SelectedEntry.Id);
        StatusMessage = $"Suppression de {SelectedEntry.DisplayName}...";

        var result = await _actionExecutor.DeleteAsync(SelectedEntry);

        if (result.Success)
        {
            var entryName = SelectedEntry.DisplayName;
            Entries.Remove(SelectedEntry);
            SelectedEntry = null;

            _entriesView.Refresh();
            OnPropertyChanged(nameof(VisibleEntriesCount));
            OnPropertyChanged(nameof(EnabledCount));
            OnPropertyChanged(nameof(DisabledCount));

            StatusMessage = $"{entryName} supprimé (backup créé)";
        }
        else
        {
            StatusMessage = $"Erreur: {result.ErrorMessage}";
            MessageBox.Show(
                result.ErrorMessage,
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
                    System.Diagnostics.Process.Start("explorer.exe", $"/select,\"{SelectedEntry.TargetPath}\"");
                }
                else
                {
                    System.Diagnostics.Process.Start("explorer.exe", directory);
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
    private void OpenSettings()
    {
        // TODO: Open settings dialog
        MessageBox.Show(
            "Les paramètres seront disponibles dans une prochaine version.",
            "Paramètres",
            MessageBoxButton.OK,
            MessageBoxImage.Information);
    }
}
