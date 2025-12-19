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
using BootSentry.Knowledge.Models;
using BootSentry.Knowledge.Services;
using BootSentry.Security;
using BootSentry.Security.Services;
using BootSentry.UI.Models;
using BootSentry.UI.Resources;

namespace BootSentry.UI.ViewModels;

/// <summary>
/// Main view model for the application.
/// </summary>
public partial class MainViewModel : ObservableObject, IDisposable
{
    private bool _disposed;
    private readonly ILogger<MainViewModel> _logger;
    private readonly IEnumerable<IStartupProvider> _providers;
    private readonly ActionExecutor _actionExecutor;
    private readonly ITransactionManager _transactionManager;
    private readonly ICollectionView _entriesView;
    private readonly ExportService _exportService;
    private readonly IRiskService _riskAnalyzer;
    private readonly KnowledgeService _knowledgeService;
    private readonly IMalwareScanner? _malwareScanner;
    private CancellationTokenSource? _cancellationTokenSource;

    // Smart Background Scan
    private readonly SemaphoreSlim _scanSemaphore = new(1, 1);
    private const int MaxAutoScanCount = 10;

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

    [ObservableProperty]
    private EntryCategory? _selectedCategory;

    public string[] TypeFilters { get; } = ["Tous", "Registre", "Dossier Démarrage", "Tâches", "Services", "Drivers", "Expert"];
    public string[] StatusFilters { get; } = ["Tous", "Actives", "Désactivées", "Suspectes"];

    [ObservableProperty]
    private string _statusMessage = "Prêt";

    [ObservableProperty]
    private bool _isLoading;

    [ObservableProperty]
    private string _progressText = string.Empty;

    [ObservableProperty]
    private bool _canCancel;

    [ObservableProperty]
    private bool _isAdmin;

    [ObservableProperty]
    private string _adminStatusText = string.Empty;

    [ObservableProperty]
    private LocalizedKnowledgeEntry? _knowledgeInfo;

    public ObservableCollection<StartupEntry> Entries { get; } = [];

    public ICollectionView FilteredEntries => _entriesView;

    public int VisibleEntriesCount => _entriesView.Cast<object>().Count();
    public int TotalEntriesCount => Entries.Count;
    public int EnabledCount => Entries.Count(e => e.Status == EntryStatus.Enabled);
    public int DisabledCount => Entries.Count(e => e.Status == EntryStatus.Disabled);
    public int SuspiciousCount => Entries.Count(e => e.RiskLevel == RiskLevel.Suspicious || e.RiskLevel == RiskLevel.Critical);
    public bool HasSuspiciousEntries => SuspiciousCount > 0;
    public bool HasNoVisibleEntries => VisibleEntriesCount == 0 && TotalEntriesCount > 0;

    // Category counts for tab headers
    public int StartupCount => Entries.Count(e => e.Category == EntryCategory.Startup && ShouldShowInNonExpertMode(e));
    public int TasksCount => Entries.Count(e => e.Category == EntryCategory.Tasks && ShouldShowInNonExpertMode(e));
    public int ServicesCount => Entries.Count(e => e.Category == EntryCategory.Services && ShouldShowInNonExpertMode(e));
    public int SystemCount => Entries.Count(e => e.Category == EntryCategory.System);
    public int ExtensionsCount => Entries.Count(e => e.Category == EntryCategory.Extensions);

    private bool ShouldShowInNonExpertMode(StartupEntry entry)
    {
        if (IsExpertMode) return true;
        if (entry.Publisher?.Contains("Microsoft", StringComparison.OrdinalIgnoreCase) == true
            && entry.SignatureStatus == SignatureStatus.SignedTrusted)
            return false;
        if (entry.IsProtected) return false;
        return true;
    }

    public MainViewModel(
        ILogger<MainViewModel> logger,
        IEnumerable<IStartupProvider> providers,
        ActionExecutor actionExecutor,
        ITransactionManager transactionManager,
        KnowledgeService knowledgeService,
        ExportService exportService,
        IRiskService riskAnalyzer,
        IMalwareScanner? malwareScanner = null)
    {
        _logger = logger;
        _providers = providers;
        _actionExecutor = actionExecutor;
        _transactionManager = transactionManager;
        _knowledgeService = knowledgeService;
        _malwareScanner = malwareScanner;
        _exportService = exportService;
        _riskAnalyzer = riskAnalyzer;

        _entriesView = CollectionViewSource.GetDefaultView(Entries);
        _entriesView.Filter = FilterEntries;

        // Check admin status
        IsAdmin = UacHelper.IsRunningAsAdmin();
        AdminStatusText = IsAdmin ? "Administrateur" : "Standard";

        // Load entries on startup
        _ = RefreshAsync();
    }

    partial void OnSelectedEntryChanged(StartupEntry? value)
    {
        // Look up knowledge info for the selected entry
        if (value != null)
        {
            var entry = _knowledgeService.FindEntry(
                value.DisplayName,
                value.TargetPath,
                value.Publisher);

            // Debug: Show in status bar
            if (entry != null)
            {
                var lang = Resources.Strings.CurrentLanguage;
                var hasEn = !string.IsNullOrEmpty(entry.ShortDescriptionEn);
                StatusMessage = $"[DEBUG] Lang={lang}, HasEn={hasEn}";
            }

            KnowledgeInfo = entry != null ? new LocalizedKnowledgeEntry(entry) : null;
        }
        else
        {
            KnowledgeInfo = null;
        }
    }

    partial void OnSearchTextChanged(string value)
    {
        _entriesView.Refresh();
        OnPropertyChanged(nameof(VisibleEntriesCount));
        OnPropertyChanged(nameof(HasNoVisibleEntries));
    }

    partial void OnIsExpertModeChanged(bool value)
    {
        _entriesView.Refresh();
        OnPropertyChanged(nameof(VisibleEntriesCount));
        OnPropertyChanged(nameof(HasNoVisibleEntries));
        StatusMessage = value ? "Mode Expert activé" : "Mode Expert désactivé";
        _logger.LogInformation("Expert mode: {Mode}", value ? "enabled" : "disabled");
    }

    partial void OnSelectedTypeFilterChanged(string value)
    {
        _entriesView.Refresh();
        OnPropertyChanged(nameof(VisibleEntriesCount));
        OnPropertyChanged(nameof(HasNoVisibleEntries));
    }

    partial void OnSelectedStatusFilterChanged(string value)
    {
        _entriesView.Refresh();
        OnPropertyChanged(nameof(VisibleEntriesCount));
        OnPropertyChanged(nameof(HasNoVisibleEntries));
    }

    partial void OnSelectedCategoryChanged(EntryCategory? value)
    {
        _entriesView.Refresh();
        OnPropertyChanged(nameof(VisibleEntriesCount));
        OnPropertyChanged(nameof(HasNoVisibleEntries));
    }

    public bool CanResetFilters => !string.IsNullOrEmpty(SearchText) || SelectedStatusFilter != "Tous";

    [RelayCommand]
    private void ResetFilters()
    {
        SearchText = string.Empty;
        SelectedStatusFilter = "Tous";
        _entriesView.Refresh();
        OnPropertyChanged(nameof(VisibleEntriesCount));
        OnPropertyChanged(nameof(HasNoVisibleEntries));
        OnPropertyChanged(nameof(CanResetFilters));
    }

    private bool FilterEntries(object obj)
    {
        if (obj is not StartupEntry entry)
            return false;

        // Filter by category (tab) if one is selected
        if (SelectedCategory.HasValue && entry.Category != SelectedCategory.Value)
            return false;

        // In non-expert mode, hide Microsoft entries and critical items
        if (!IsExpertMode)
        {
            if (entry.Publisher?.Contains("Microsoft", StringComparison.OrdinalIgnoreCase) == true
                && entry.SignatureStatus == SignatureStatus.SignedTrusted)
                return false;

            if (entry.IsProtected)
                return false;

            // Hide expert-only categories in non-expert mode
            if (entry.Category == EntryCategory.System || entry.Category == EntryCategory.Extensions)
                return false;

            // Hide drivers in non-expert mode
            if (entry.Type == EntryType.Driver)
                return false;
        }

        // Apply type filter (for sub-filtering within category)
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
        _cancellationTokenSource?.Cancel();
        _cancellationTokenSource = new CancellationTokenSource();
        var token = _cancellationTokenSource.Token;

        IsLoading = true;
        CanCancel = true;
        ProgressText = string.Empty;
        StatusMessage = "Analyse en cours...";

        try
        {
            _logger.LogInformation("Starting scan with {Count} providers", _providers.Count());

            Entries.Clear();
            var providerList = _providers.Where(p => p.IsAvailable()).ToList();

            StatusMessage = Strings.Get("ScanningProviders");

            // Parallel scan of all providers
            var scanTasks = providerList.Select(async provider =>
            {
                try
                {
                    _logger.LogDebug("Starting scan for provider {Provider}", provider.DisplayName);
                    var entries = await provider.ScanAsync(token);
                    _logger.LogInformation("Provider {Provider} found {Count} entries",
                        provider.DisplayName, entries.Count);
                    return (Provider: provider, Entries: entries, Error: (Exception?)null);
                }
                catch (OperationCanceledException)
                {
                    throw;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error scanning with provider {Provider}", provider.DisplayName);
                    return (Provider: provider, Entries: new List<StartupEntry>(), Error: ex);
                }
            }).ToList();

            var results = await Task.WhenAll(scanTasks);

            token.ThrowIfCancellationRequested();

            // Process results on UI thread
            var totalEntries = 0;
            foreach (var result in results)
            {
                if (result.Error != null) continue;

                foreach (var entry in result.Entries)
                {
                    _riskAnalyzer.UpdateRiskLevel(entry);
                    Entries.Add(entry);
                    totalEntries++;
                }
            }

            _entriesView.Refresh();
            UpdateCounts();

            StatusMessage = $"Scan terminé - {totalEntries} entrées trouvées";
            _logger.LogInformation("Scan complete: {Count} entries found", totalEntries);
        }
        catch (OperationCanceledException)
        {
            StatusMessage = "Analyse annulée";
            _logger.LogInformation("Scan cancelled by user");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during scan");
            StatusMessage = $"Erreur: {ex.Message}";
        }
        finally
        {
            IsLoading = false;
            CanCancel = false;
            ProgressText = string.Empty;

            // Fire-and-forget: trigger smart background scan for risky entries
            _ = TriggerSmartScanAsync();
        }
    }

    /// <summary>
    /// Triggers an automatic background scan of risky entries.
    /// This method runs fire-and-forget and does not block the UI.
    /// </summary>
    private async Task TriggerSmartScanAsync()
    {
        // Skip if scanner is not available
        if (_malwareScanner == null)
            return;

        try
        {
            // Select risky entries that haven't been scanned yet
            var riskyEntries = Entries
                .Where(e =>
                    (e.RiskLevel == RiskLevel.Suspicious || e.RiskLevel == RiskLevel.Critical) &&
                    e.SignatureStatus != SignatureStatus.SignedTrusted &&
                    e.MalwareScanResult == null &&
                    e.FileExists &&
                    !string.IsNullOrEmpty(e.TargetPath))
                .Take(MaxAutoScanCount)
                .ToList();

            if (riskyEntries.Count == 0)
                return;

            _logger.LogInformation("Smart scan: {Count} risky entries to scan", riskyEntries.Count);

            // Run scanning in background
            await Task.Run(async () =>
            {
                var malwareDetected = new List<StartupEntry>();

                foreach (var entry in riskyEntries)
                {
                    // Use semaphore to scan one at a time
                    await _scanSemaphore.WaitAsync();
                    try
                    {
                        _logger.LogDebug("Smart scan: scanning {Name}", entry.DisplayName);

                        var result = await _malwareScanner.ScanAsync(entry.TargetPath!);

                        // Update entry properties (StartupEntry implements INotifyPropertyChanged)
                        entry.MalwareScanResult = result;
                        entry.LastMalwareScan = DateTime.Now;

                        if (result == ScanResult.Malware)
                        {
                            entry.RiskLevel = RiskLevel.Critical;
                            malwareDetected.Add(entry);
                            _logger.LogWarning("Smart scan: MALWARE detected in {Path}", entry.TargetPath);
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogDebug(ex, "Smart scan: error scanning {Name}", entry.DisplayName);
                        entry.MalwareScanResult = ScanResult.Error;
                    }
                    finally
                    {
                        _scanSemaphore.Release();
                    }
                }

                // Update UI on dispatcher thread if malware was found
                if (malwareDetected.Count > 0)
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        _entriesView.Refresh();
                        UpdateCounts();

                        // Log notification instead of blocking MessageBox
                        var names = string.Join(", ", malwareDetected.Select(e => e.DisplayName));
                        _logger.LogWarning("Smart scan completed: {Count} threat(s) detected: {Names}",
                            malwareDetected.Count, names);

                        StatusMessage = $"⚠ {malwareDetected.Count} menace(s) détectée(s) lors du scan automatique";
                    });
                }
            });

            _logger.LogInformation("Smart scan completed");
        }
        catch (Exception ex)
        {
            // Silent failure - don't disturb the user for background scan errors
            _logger.LogError(ex, "Smart scan failed");
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
        OnPropertyChanged(nameof(HasNoVisibleEntries));
        // Category counts for tabs
        OnPropertyChanged(nameof(StartupCount));
        OnPropertyChanged(nameof(TasksCount));
        OnPropertyChanged(nameof(ServicesCount));
        OnPropertyChanged(nameof(SystemCount));
        OnPropertyChanged(nameof(ExtensionsCount));
    }

    [RelayCommand]
    private void CancelOperation()
    {
        _cancellationTokenSource?.Cancel();
        StatusMessage = "Annulation en cours...";
        _logger.LogInformation("Operation cancelled by user");
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
        IsLoading = true;
        StatusMessage = $"Activation de {entriesToEnable.Count} entrées...";

        var successCount = 0;
        var failCount = 0;
        var total = entriesToEnable.Count;
        var current = 0;

        try
        {
            foreach (var entry in entriesToEnable)
            {
                current++;
                ProgressText = $"{current}/{total}";
                StatusMessage = $"Activation: {entry.DisplayName}...";

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
        finally
        {
            IsLoading = false;
            ProgressText = string.Empty;
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
        IsLoading = true;
        StatusMessage = $"Désactivation de {entriesToDisable.Count} entrées...";

        var successCount = 0;
        var failCount = 0;
        var total = entriesToDisable.Count;
        var current = 0;

        try
        {
            foreach (var entry in entriesToDisable)
            {
                current++;
                ProgressText = $"{current}/{total}";
                StatusMessage = $"Désactivation: {entry.DisplayName}...";

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
        finally
        {
            IsLoading = false;
            ProgressText = string.Empty;
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
    private async Task ScanSelectedAsync()
    {
        if (SelectedEntry?.TargetPath == null || !SelectedEntry.FileExists)
        {
            MessageBox.Show(
                "Impossible de scanner: fichier introuvable.",
                "Erreur",
                MessageBoxButton.OK,
                MessageBoxImage.Warning);
            return;
        }

        if (_malwareScanner == null)
        {
            MessageBox.Show(
                "Le scanner antivirus n'est pas disponible.",
                "Erreur",
                MessageBoxButton.OK,
                MessageBoxImage.Warning);
            return;
        }

        try
        {
            StatusMessage = "Scan antivirus en cours...";
            _logger.LogInformation("Scanning file: {Path}", SelectedEntry.TargetPath);

            var result = await _malwareScanner.ScanAsync(SelectedEntry.TargetPath);

            SelectedEntry.MalwareScanResult = result;
            SelectedEntry.LastMalwareScan = DateTime.Now;
            OnPropertyChanged(nameof(SelectedEntry));

            switch (result)
            {
                case ScanResult.Clean:
                    StatusMessage = "Scan terminé: fichier sain";
                    break;

                case ScanResult.Malware:
                    StatusMessage = "ALERTE: Menace détectée!";
                    SelectedEntry.RiskLevel = RiskLevel.Critical;
                    _entriesView.Refresh();
                    UpdateCounts();

                    MessageBox.Show(
                        $"MENACE DÉTECTÉE!\n\nLe fichier suivant a été identifié comme malveillant:\n{SelectedEntry.TargetPath}\n\nIl est fortement recommandé de désactiver ou supprimer cette entrée.",
                        "Menace détectée",
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning);
                    break;

                case ScanResult.Blocked:
                    StatusMessage = "Scan terminé: fichier bloqué par la politique de sécurité";
                    break;

                case ScanResult.NotScanned:
                    StatusMessage = "Fichier non scanné (inaccessible ou verrouillé)";
                    break;

                case ScanResult.TooLarge:
                    StatusMessage = "Fichier trop volumineux pour le scan (max 250 Mo)";
                    break;

                case ScanResult.NoAntivirusProvider:
                    StatusMessage = "Aucun antivirus disponible";
                    MessageBox.Show(
                        "Aucun antivirus compatible n'est disponible pour le scan.\n\n" +
                        "Veuillez activer Windows Defender ou installer un antivirus compatible AMSI.",
                        "Antivirus non disponible",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information);
                    break;

                case ScanResult.Error:
                    StatusMessage = "Erreur lors du scan antivirus";
                    break;
            }

            _logger.LogInformation("Scan result for {Path}: {Result}", SelectedEntry.TargetPath, result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during malware scan");
            StatusMessage = $"Erreur lors du scan: {ex.Message}";
            MessageBox.Show(
                $"Erreur lors du scan antivirus:\n{ex.Message}",
                "Erreur",
                MessageBoxButton.OK,
                MessageBoxImage.Error);
        }
    }

    [ObservableProperty]
    private bool _isScanning;

    [ObservableProperty]
    private int _scanProgress;

    [ObservableProperty]
    private int _scanTotal;

    [ObservableProperty]
    private string _scanProgressText = string.Empty;

    private CancellationTokenSource? _scanCts;

    [RelayCommand]
    private async Task ScanAllAsync()
    {
        if (_malwareScanner == null)
        {
            MessageBox.Show(
                "Le scanner antivirus n'est pas disponible.",
                "Erreur",
                MessageBoxButton.OK,
                MessageBoxImage.Warning);
            return;
        }

        // Get all scannable entries (visible, with existing files)
        var entriesToScan = Entries
            .Where(e => e.FileExists && !string.IsNullOrEmpty(e.TargetPath))
            .ToList();

        if (entriesToScan.Count == 0)
        {
            MessageBox.Show(
                "Aucun fichier à scanner.",
                "Scan global",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
            return;
        }

        var result = MessageBox.Show(
            $"Lancer le scan antivirus sur {entriesToScan.Count} fichiers ?\n\nCette opération peut prendre plusieurs minutes.",
            "Scan global",
            MessageBoxButton.YesNo,
            MessageBoxImage.Question);

        if (result != MessageBoxResult.Yes)
            return;

        IsScanning = true;
        ScanProgress = 0;
        ScanTotal = entriesToScan.Count;
        _scanCts = new CancellationTokenSource();

        int scanned = 0;
        int clean = 0;
        int threats = 0;
        int errors = 0;

        _logger.LogInformation("Starting global scan of {Count} files", entriesToScan.Count);

        try
        {
            foreach (var entry in entriesToScan)
            {
                if (_scanCts.Token.IsCancellationRequested)
                    break;

                ScanProgress = scanned + 1;
                ScanProgressText = $"Scan {ScanProgress}/{ScanTotal}: {Path.GetFileName(entry.TargetPath)}";
                StatusMessage = ScanProgressText;

                try
                {
                    var scanResult = await _malwareScanner.ScanAsync(entry.TargetPath!, _scanCts.Token);
                    entry.MalwareScanResult = scanResult;
                    entry.LastMalwareScan = DateTime.Now;

                    switch (scanResult)
                    {
                        case ScanResult.Clean:
                            clean++;
                            break;
                        case ScanResult.Malware:
                            threats++;
                            entry.RiskLevel = RiskLevel.Critical;
                            break;
                        case ScanResult.Error:
                        case ScanResult.NoAntivirusProvider:
                            errors++;
                            break;
                    }
                }
                catch (OperationCanceledException)
                {
                    break;
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Error scanning {Path}", entry.TargetPath);
                    errors++;
                }

                scanned++;
            }

            _entriesView.Refresh();
            UpdateCounts();
            OnPropertyChanged(nameof(SelectedEntry));

            var message = _scanCts.Token.IsCancellationRequested
                ? $"Scan interrompu.\n\n"
                : $"Scan terminé.\n\n";

            message += $"Fichiers scannés: {scanned}\n";
            message += $"Fichiers sains: {clean}\n";
            if (threats > 0)
                message += $"MENACES DÉTECTÉES: {threats}\n";
            if (errors > 0)
                message += $"Erreurs/Non scannés: {errors}";

            StatusMessage = threats > 0
                ? $"Scan terminé: {threats} menace(s) détectée(s)!"
                : $"Scan terminé: {clean} fichiers sains";

            MessageBox.Show(
                message,
                threats > 0 ? "Menaces détectées!" : "Scan terminé",
                MessageBoxButton.OK,
                threats > 0 ? MessageBoxImage.Warning : MessageBoxImage.Information);

            _logger.LogInformation("Global scan completed: {Scanned} scanned, {Clean} clean, {Threats} threats, {Errors} errors",
                scanned, clean, threats, errors);
        }
        finally
        {
            IsScanning = false;
            ScanProgressText = string.Empty;
            _scanCts?.Dispose();
            _scanCts = null;
        }
    }

    [RelayCommand]
    private void CancelScan()
    {
        _scanCts?.Cancel();
        StatusMessage = "Annulation du scan...";
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
                var options = new ExportOptions { IncludeDetails = true, IncludeKnowledgeInfo = true };
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
                var options = new ExportOptions { IncludeDetails = true, IncludeKnowledgeInfo = true };
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
        Process.Start(new ProcessStartInfo("https://github.com/VBlackJack/BootSentry#documentation") { UseShellExecute = true });
    }

    [RelayCommand]
    private void GoToGitHub()
    {
        Process.Start(new ProcessStartInfo("https://github.com/VBlackJack/BootSentry") { UseShellExecute = true });
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

    /// <summary>
    /// Releases resources used by the view model.
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (_disposed)
            return;

        if (disposing)
        {
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource?.Dispose();
            _cancellationTokenSource = null;

            _scanCts?.Cancel();
            _scanCts?.Dispose();
            _scanCts = null;

            _scanSemaphore.Dispose();
        }

        _disposed = true;
    }
}
