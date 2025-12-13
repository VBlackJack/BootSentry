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
using BootSentry.Security;

namespace BootSentry.UI.ViewModels;

/// <summary>
/// Main view model for the application.
/// </summary>
public partial class MainViewModel : ObservableObject
{
    private readonly ILogger<MainViewModel> _logger;
    private readonly IEnumerable<IStartupProvider> _providers;
    private readonly ActionExecutor _actionExecutor;
    private readonly ITransactionManager _transactionManager;
    private readonly ICollectionView _entriesView;
    private readonly ExportService _exportService;
    private readonly RiskAnalyzer _riskAnalyzer;

    [ObservableProperty]
    private bool _isExpertMode;

    [ObservableProperty]
    private string _searchText = string.Empty;

    [ObservableProperty]
    private StartupEntry? _selectedEntry;

    private List<StartupEntry> _selectedEntries = [];

    public int SelectedCount => _selectedEntries.Count;
    public bool HasMultipleSelection => _selectedEntries.Count > 1;

    [ObservableProperty]
    private string _selectedTypeFilter = "Tous";

    [ObservableProperty]
    private string _selectedStatusFilter = "Tous";

    public string[] TypeFilters { get; } = ["Tous", "Registre", "Dossier Démarrage", "Tâches", "Services", "Drivers", "Expert"];
    public string[] StatusFilters { get; } = ["Tous", "Actives", "Désactivées", "Suspectes"];

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
        ActionExecutor actionExecutor,
        ITransactionManager transactionManager)
    {
        _logger = logger;
        _providers = providers;
        _actionExecutor = actionExecutor;
        _transactionManager = transactionManager;
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

    partial void OnSelectedTypeFilterChanged(string value)
    {
        _entriesView.Refresh();
        OnPropertyChanged(nameof(VisibleEntriesCount));
    }

    partial void OnSelectedStatusFilterChanged(string value)
    {
        _entriesView.Refresh();
        OnPropertyChanged(nameof(VisibleEntriesCount));
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

            // Hide drivers and expert-only types in non-expert mode
            if (entry.Type == EntryType.Driver || entry.Type == EntryType.SessionManager ||
                entry.Type == EntryType.AppInitDlls || entry.Type == EntryType.PrintMonitor ||
                entry.Type == EntryType.ShellExtension || entry.Type == EntryType.BHO ||
                entry.Type == EntryType.WinsockLSP)
                return false;
        }

        // Apply type filter
        if (SelectedTypeFilter != "Tous")
        {
            var matchesType = SelectedTypeFilter switch
            {
                "Registre" => entry.Type is EntryType.RegistryRun or EntryType.RegistryRunOnce or EntryType.RegistryPolicies,
                "Dossier Démarrage" => entry.Type == EntryType.StartupFolder,
                "Tâches" => entry.Type == EntryType.ScheduledTask,
                "Services" => entry.Type == EntryType.Service,
                "Drivers" => entry.Type == EntryType.Driver,
                "Expert" => entry.Type is EntryType.IFEO or EntryType.Winlogon or EntryType.ShellExtension or
                            EntryType.BHO or EntryType.PrintMonitor or EntryType.SessionManager or
                            EntryType.AppInitDlls or EntryType.WinsockLSP,
                _ => true
            };
            if (!matchesType) return false;
        }

        // Apply status filter
        if (SelectedStatusFilter != "Tous")
        {
            var matchesStatus = SelectedStatusFilter switch
            {
                "Actives" => entry.Status == EntryStatus.Enabled,
                "Désactivées" => entry.Status == EntryStatus.Disabled,
                "Suspectes" => entry.RiskLevel is RiskLevel.Suspicious or RiskLevel.Critical,
                _ => true
            };
            if (!matchesStatus) return false;
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
        _selectedEntries.Clear();
        OnPropertyChanged(nameof(SelectedCount));
        OnPropertyChanged(nameof(HasMultipleSelection));
    }

    /// <summary>
    /// Updates the list of selected entries from the DataGrid.
    /// Called by the View when selection changes.
    /// </summary>
    public void UpdateSelectedEntries(List<StartupEntry> entries)
    {
        _selectedEntries = entries;
        OnPropertyChanged(nameof(SelectedCount));
        OnPropertyChanged(nameof(HasMultipleSelection));
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
    private async Task EnableBatchAsync()
    {
        if (_selectedEntries.Count == 0)
            return;

        var entriesToEnable = _selectedEntries.Where(e => e.Status == EntryStatus.Disabled).ToList();
        if (entriesToEnable.Count == 0)
        {
            MessageBox.Show(
                "Aucune entrée désactivée sélectionnée.",
                "Information",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
            return;
        }

        var result = MessageBox.Show(
            $"Voulez-vous activer {entriesToEnable.Count} entrée(s)?",
            "Confirmation",
            MessageBoxButton.YesNo,
            MessageBoxImage.Question);

        if (result != MessageBoxResult.Yes)
            return;

        _logger.LogInformation("Batch enabling {Count} entries", entriesToEnable.Count);
        StatusMessage = $"Activation de {entriesToEnable.Count} entrées...";

        var successCount = 0;
        var failCount = 0;

        foreach (var entry in entriesToEnable)
        {
            var actionResult = await _actionExecutor.EnableAsync(entry);
            if (actionResult.Success)
                successCount++;
            else
                failCount++;
        }

        _entriesView.Refresh();
        UpdateCounts();

        if (failCount == 0)
        {
            StatusMessage = $"{successCount} entrée(s) activée(s)";
        }
        else
        {
            StatusMessage = $"{successCount} activée(s), {failCount} échec(s)";
            MessageBox.Show(
                $"{successCount} entrée(s) activée(s) avec succès.\n{failCount} échec(s).",
                "Résultat",
                MessageBoxButton.OK,
                MessageBoxImage.Warning);
        }
    }

    [RelayCommand]
    private async Task DisableBatchAsync()
    {
        if (_selectedEntries.Count == 0)
            return;

        var entriesToDisable = _selectedEntries.Where(e => e.Status == EntryStatus.Enabled && !e.IsProtected).ToList();
        if (entriesToDisable.Count == 0)
        {
            MessageBox.Show(
                "Aucune entrée active non-protégée sélectionnée.",
                "Information",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
            return;
        }

        var result = MessageBox.Show(
            $"Voulez-vous désactiver {entriesToDisable.Count} entrée(s)?",
            "Confirmation",
            MessageBoxButton.YesNo,
            MessageBoxImage.Question);

        if (result != MessageBoxResult.Yes)
            return;

        _logger.LogInformation("Batch disabling {Count} entries", entriesToDisable.Count);
        StatusMessage = $"Désactivation de {entriesToDisable.Count} entrées...";

        var successCount = 0;
        var failCount = 0;

        foreach (var entry in entriesToDisable)
        {
            var actionResult = await _actionExecutor.DisableAsync(entry);
            if (actionResult.Success)
                successCount++;
            else
                failCount++;
        }

        _entriesView.Refresh();
        UpdateCounts();

        if (failCount == 0)
        {
            StatusMessage = $"{successCount} entrée(s) désactivée(s)";
        }
        else
        {
            StatusMessage = $"{successCount} désactivée(s), {failCount} échec(s)";
            MessageBox.Show(
                $"{successCount} entrée(s) désactivée(s) avec succès.\n{failCount} échec(s).",
                "Résultat",
                MessageBoxButton.OK,
                MessageBoxImage.Warning);
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
    private void OpenInRegedit()
    {
        if (SelectedEntry?.SourcePath == null)
            return;

        // Only works for registry entries
        if (!SelectedEntry.SourcePath.StartsWith("HK", StringComparison.OrdinalIgnoreCase))
        {
            MessageBox.Show(
                "Cette entrée n'est pas une clé de registre.",
                "Action impossible",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
            return;
        }

        try
        {
            // Convert short hive names to full names for regedit
            var regPath = SelectedEntry.SourcePath
                .Replace("HKCU\\", "HKEY_CURRENT_USER\\")
                .Replace("HKLM\\", "HKEY_LOCAL_MACHINE\\");

            // Set the last key in regedit's history
            using var key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(
                @"Software\Microsoft\Windows\CurrentVersion\Applets\Regedit");
            key?.SetValue("LastKey", regPath);

            Process.Start(new ProcessStartInfo("regedit.exe") { UseShellExecute = true });
            StatusMessage = "Regedit ouvert";
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error opening regedit");
            MessageBox.Show(
                $"Erreur lors de l'ouverture de Regedit: {ex.Message}",
                "Erreur",
                MessageBoxButton.OK,
                MessageBoxImage.Error);
        }
    }

    [RelayCommand]
    private void OpenInServices()
    {
        if (SelectedEntry?.Type != EntryType.Service)
        {
            MessageBox.Show(
                "Cette entrée n'est pas un service.",
                "Action impossible",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
            return;
        }

        try
        {
            Process.Start(new ProcessStartInfo("services.msc") { UseShellExecute = true });
            StatusMessage = "Services.msc ouvert";
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error opening services.msc");
        }
    }

    [RelayCommand]
    private void OpenInTaskScheduler()
    {
        if (SelectedEntry?.Type != EntryType.ScheduledTask)
        {
            MessageBox.Show(
                "Cette entrée n'est pas une tâche planifiée.",
                "Action impossible",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
            return;
        }

        try
        {
            Process.Start(new ProcessStartInfo("taskschd.msc") { UseShellExecute = true });
            StatusMessage = "Planificateur de tâches ouvert";
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error opening task scheduler");
        }
    }

    [RelayCommand]
    private async Task CalculateHashAsync()
    {
        if (SelectedEntry?.TargetPath == null || !SelectedEntry.FileExists)
        {
            MessageBox.Show(
                "Impossible de calculer le hash: fichier introuvable.",
                "Erreur",
                MessageBoxButton.OK,
                MessageBoxImage.Warning);
            return;
        }

        try
        {
            StatusMessage = "Calcul du hash SHA-256...";
            var hashCalculator = new HashCalculator();
            var hash = await hashCalculator.CalculateSha256Async(SelectedEntry.TargetPath);

            SelectedEntry.Sha256 = hash;
            OnPropertyChanged(nameof(SelectedEntry));

            // Copy to clipboard and show result
            Clipboard.SetText(hash);
            StatusMessage = $"Hash calculé et copié: {hash[..16]}...";

            MessageBox.Show(
                $"SHA-256:\n{hash}\n\nLe hash a été copié dans le presse-papiers.",
                "Hash calculé",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error calculating hash");
            StatusMessage = $"Erreur lors du calcul du hash: {ex.Message}";
            MessageBox.Show(
                $"Erreur lors du calcul du hash:\n{ex.Message}",
                "Erreur",
                MessageBoxButton.OK,
                MessageBoxImage.Error);
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
    private void Restore()
    {
        // Open history window for restoration
        ShowHistory();
    }

    [RelayCommand]
    private void SelectAll()
    {
        // This is handled by the DataGrid's built-in Ctrl+A, but we provide
        // a command for consistency. The actual selection is done in the View.
        StatusMessage = "Sélectionner tout via Ctrl+A";
    }

    [RelayCommand]
    private async Task UndoAsync()
    {
        try
        {
            // Get the latest transaction
            var transactions = await _transactionManager.GetTransactionsAsync(limit: 1);
            var lastTransaction = transactions.FirstOrDefault(t => t.CanRestore);

            if (lastTransaction == null)
            {
                MessageBox.Show(
                    "Aucune action à annuler.",
                    "Historique vide",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
                return;
            }

            var confirmResult = MessageBox.Show(
                $"Voulez-vous annuler l'action '{lastTransaction.ActionType}' sur '{lastTransaction.EntryDisplayName}'?",
                "Confirmation d'annulation",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (confirmResult != MessageBoxResult.Yes)
                return;

            _logger.LogInformation("Undoing transaction: {Id}", lastTransaction.Id);
            StatusMessage = $"Annulation de {lastTransaction.ActionType} sur {lastTransaction.EntryDisplayName}...";

            var result = await _transactionManager.RollbackAsync(lastTransaction.Id);

            if (result.Success)
            {
                StatusMessage = $"Action annulée: {lastTransaction.EntryDisplayName}";

                // Refresh the list to show the restored entry
                await RefreshAsync();
            }
            else
            {
                StatusMessage = $"Erreur: {result.ErrorMessage}";
                MessageBox.Show(
                    result.ErrorMessage,
                    "Erreur d'annulation",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error undoing last action");
            StatusMessage = $"Erreur: {ex.Message}";
            MessageBox.Show(
                $"Erreur lors de l'annulation: {ex.Message}",
                "Erreur",
                MessageBoxButton.OK,
                MessageBoxImage.Error);
        }
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
        var aboutDialog = new Views.AboutDialog
        {
            Owner = Application.Current.MainWindow
        };
        aboutDialog.ShowDialog();
    }

    [RelayCommand]
    private async Task ExportDiagnosticsAsync()
    {
        var dialog = new Microsoft.Win32.SaveFileDialog
        {
            Filter = "ZIP Files (*.zip)|*.zip",
            DefaultExt = ".zip",
            FileName = $"BootSentry_Diagnostics_{DateTime.Now:yyyyMMdd_HHmmss}"
        };

        if (dialog.ShowDialog() == true)
        {
            try
            {
                StatusMessage = "Création du fichier de diagnostics...";
                await _exportService.ExportDiagnosticsZipAsync(Entries, dialog.FileName);
                StatusMessage = $"Diagnostics exportés: {dialog.FileName}";

                var result = MessageBox.Show(
                    $"Le fichier de diagnostics a été créé:\n{dialog.FileName}\n\nVoulez-vous ouvrir le dossier contenant le fichier?",
                    "Export terminé",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Information);

                if (result == MessageBoxResult.Yes)
                {
                    Process.Start("explorer.exe", $"/select,\"{dialog.FileName}\"");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error exporting diagnostics");
                StatusMessage = $"Erreur d'export: {ex.Message}";
                MessageBox.Show(
                    $"Erreur lors de l'export: {ex.Message}",
                    "Erreur",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }
    }

    [RelayCommand]
    private void Exit()
    {
        Application.Current.Shutdown();
    }
}
