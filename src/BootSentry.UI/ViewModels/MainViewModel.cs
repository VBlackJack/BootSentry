using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Data;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using BootSentry.Actions;
using BootSentry.Core;
using BootSentry.Core.Enums;
using BootSentry.Core.Helpers;
using BootSentry.Core.Interfaces;
using BootSentry.Core.Models;
using BootSentry.Core.Services;
using BootSentry.Core.Services.Integrations;
using BootSentry.Knowledge.Models;
using BootSentry.Knowledge.Services;
using BootSentry.Security;
using BootSentry.Security.Services;
using BootSentry.UI.Controls;
using BootSentry.UI.Enums;
using BootSentry.UI.Models;
using BootSentry.UI.Resources;
using BootSentry.UI.Services;

namespace BootSentry.UI.ViewModels;

/// <summary>
/// Main view model for the application.
/// </summary>
public partial class MainViewModel : ObservableObject, IDisposable
{
    private const string TypeFilterAll = "all";
    private const string TypeFilterRegistry = "registry";
    private const string TypeFilterStartupFolder = "startup_folder";
    private const string TypeFilterTasks = "tasks";
    private const string TypeFilterServices = "services";
    private const string TypeFilterDrivers = "drivers";
    private const string TypeFilterExpert = "expert";

    private const string StatusFilterAll = "all";
    private const string StatusFilterEnabled = "enabled";
    private const string StatusFilterDisabled = "disabled";
    private const string StatusFilterSuspicious = "suspicious";

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
    private readonly VirusTotalService _virusTotalService;
    private readonly ToastService _toastService;
    private readonly SettingsService _settingsService;
    private readonly IDialogService _dialogService;
    private readonly IProcessLauncher _processLauncher;
    private readonly IClipboardService _clipboardService;
    private CancellationTokenSource? _cancellationTokenSource;

    // Smart Background Scan
    private readonly SemaphoreSlim _scanSemaphore = new(1, 1);
    private const int MaxAutoScanCount = 10;

    [ObservableProperty]
    private bool _isExpertMode;

    /// <summary>
    /// Gets or sets whether to hide all Microsoft entries (applies to all entry types).
    /// </summary>
    public bool HideMicrosoftEntries
    {
        get => _settingsService.Settings.HideMicrosoftEntries;
        set
        {
            if (_settingsService.Settings.HideMicrosoftEntries != value)
            {
                _settingsService.Settings.HideMicrosoftEntries = value;
                _settingsService.Save();
                OnPropertyChanged();
                _entriesView.Refresh();
                UpdateCounts();
            }
        }
    }

    [ObservableProperty]
    private string _searchText = string.Empty;

    [ObservableProperty]
    private StartupEntry? _selectedEntry;

    private List<StartupEntry> _selectedEntries = [];

    public int SelectedCount => _selectedEntries.Count;
    public bool HasMultipleSelection => _selectedEntries.Count > 1;

    [ObservableProperty]
    private string _selectedTypeFilter = TypeFilterAll;

    [ObservableProperty]
    private string _selectedStatusFilter = StatusFilterAll;

    [ObservableProperty]
    private NavigationTab _selectedTab = NavigationTab.Applications;

    public ObservableCollection<FilterOption> TypeFilters { get; } = [];
    public ObservableCollection<FilterOption> StatusFilters { get; } = [];

    [ObservableProperty]
    private string _statusMessage = Strings.Get("StatusReady");

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

    // Navigation tab counts (grouped by user intention)
    public int ApplicationsCount => Entries.Count(e =>
        (e.Category == EntryCategory.Startup || e.Category == EntryCategory.Tasks) &&
        ShouldShowInNonExpertMode(e));

    public int BrowsersCount => Entries.Count(e =>
        (e.Category == EntryCategory.Extensions || e.Type == EntryType.BHO) &&
        ShouldShowInNonExpertMode(e));

    public int SystemTabCount => Entries.Count(e =>
        (e.Type == EntryType.Service || e.Type == EntryType.Driver || e.Type == EntryType.PrintMonitor) &&
        ShouldShowInNonExpertMode(e));

    public int AdvancedCount => Entries.Count(e =>
        e.Type is EntryType.Winlogon or EntryType.AppInitDlls or EntryType.IFEO or
        EntryType.ShellExtension or EntryType.SessionManager or EntryType.WinsockLSP);

    private bool ShouldShowInNonExpertMode(StartupEntry entry)
    {
        // Microsoft Noise Filter (Global - applies to ALL entry types)
        if (HideMicrosoftEntries)
        {
            bool isMicrosoft = entry.Publisher?.Contains("Microsoft", StringComparison.OrdinalIgnoreCase) == true ||
                               entry.CompanyName?.Contains("Microsoft", StringComparison.OrdinalIgnoreCase) == true ||
                               entry.ProtectionReason?.Contains("Microsoft", StringComparison.OrdinalIgnoreCase) == true;
            if (isMicrosoft)
                return false;
        }

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
        ToastService toastService,
        SettingsService settingsService,
        VirusTotalService virusTotalService,
        IDialogService dialogService,
        IProcessLauncher processLauncher,
        IClipboardService clipboardService,
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
        _toastService = toastService;
        _settingsService = settingsService;
        _virusTotalService = virusTotalService;
        _dialogService = dialogService;
        _processLauncher = processLauncher;
        _clipboardService = clipboardService;

        _entriesView = CollectionViewSource.GetDefaultView(Entries);
        _entriesView.Filter = FilterEntries;
        RebuildFilterOptions();
        Strings.LanguageChanged += OnLanguageChanged;

        // Check admin status
        IsAdmin = UacHelper.IsRunningAsAdmin();
        AdminStatusText = IsAdmin ? Strings.Get("AdminStatusAdmin") : Strings.Get("AdminStatusStandard");

        // Load entries on startup
        _ = RefreshAsync();
    }

    partial void OnSelectedEntryChanged(StartupEntry? value)
    {
        // Look up knowledge info for the selected entry
        if (value != null)
        {
            // Try TargetPath first, then SourcePath (for extensions, the path contains the extension ID)
            var entry = _knowledgeService.FindEntry(
                value.DisplayName,
                value.TargetPath ?? value.SourcePath,
                value.Publisher);

            // If not found and SourcePath is different, try with SourcePath
            if (entry == null && !string.IsNullOrEmpty(value.SourcePath) && value.SourcePath != value.TargetPath)
            {
                entry = _knowledgeService.FindEntry(
                    value.DisplayName,
                    value.SourcePath,
                    value.Publisher);
            }

            KnowledgeInfo = entry != null ? new LocalizedKnowledgeEntry(entry) : null;
        }
        else
        {
            KnowledgeInfo = null;
        }

        // Notify commands that their CanExecute state may have changed
        EnableSelectedCommand.NotifyCanExecuteChanged();
        DisableSelectedCommand.NotifyCanExecuteChanged();
        OpenInRegeditCommand.NotifyCanExecuteChanged();
        OpenInServicesCommand.NotifyCanExecuteChanged();
        OpenInTaskSchedulerCommand.NotifyCanExecuteChanged();
    }

    partial void OnSearchTextChanged(string value)
    {
        _entriesView.Refresh();
        OnPropertyChanged(nameof(VisibleEntriesCount));
        OnPropertyChanged(nameof(HasNoVisibleEntries));
        OnPropertyChanged(nameof(CanResetFilters));
    }

    partial void OnIsExpertModeChanged(bool value)
    {
        _entriesView.Refresh();
        OnPropertyChanged(nameof(VisibleEntriesCount));
        OnPropertyChanged(nameof(HasNoVisibleEntries));
        StatusMessage = value ? Strings.Get("ExpertModeEnabled") : Strings.Get("ExpertModeDisabled");
        _logger.LogInformation("Expert mode: {Mode}", value ? "enabled" : "disabled");
    }

    partial void OnSelectedTypeFilterChanged(string value)
    {
        _entriesView.Refresh();
        OnPropertyChanged(nameof(VisibleEntriesCount));
        OnPropertyChanged(nameof(HasNoVisibleEntries));
        OnPropertyChanged(nameof(CanResetFilters));
    }

    partial void OnSelectedStatusFilterChanged(string value)
    {
        _entriesView.Refresh();
        OnPropertyChanged(nameof(VisibleEntriesCount));
        OnPropertyChanged(nameof(HasNoVisibleEntries));
        OnPropertyChanged(nameof(CanResetFilters));
    }

    partial void OnSelectedTabChanged(NavigationTab value)
    {
        // Sort Extensions by DisplayName to group by browser
        _entriesView.SortDescriptions.Clear();
        if (value == NavigationTab.Browsers)
        {
            _entriesView.SortDescriptions.Add(new System.ComponentModel.SortDescription(
                nameof(StartupEntry.DisplayName), System.ComponentModel.ListSortDirection.Ascending));
        }

        _entriesView.Refresh();
        OnPropertyChanged(nameof(VisibleEntriesCount));
        OnPropertyChanged(nameof(HasNoVisibleEntries));
    }

    public bool CanResetFilters =>
        !string.IsNullOrEmpty(SearchText)
        || SelectedStatusFilter != StatusFilterAll
        || SelectedTypeFilter != TypeFilterAll;

    [RelayCommand]
    private void ResetFilters()
    {
        SearchText = string.Empty;
        SelectedTypeFilter = TypeFilterAll;
        SelectedStatusFilter = StatusFilterAll;
        _entriesView.Refresh();
        OnPropertyChanged(nameof(VisibleEntriesCount));
        OnPropertyChanged(nameof(HasNoVisibleEntries));
        OnPropertyChanged(nameof(CanResetFilters));
    }

    private void OnLanguageChanged(object? sender, EventArgs e)
    {
        var dispatcher = Application.Current?.Dispatcher;
        if (dispatcher == null)
        {
            RebuildFilterOptions();
            return;
        }

        if (dispatcher.CheckAccess())
        {
            RebuildFilterOptions();
            return;
        }

        dispatcher.Invoke(RebuildFilterOptions);
    }

    private void RebuildFilterOptions()
    {
        var selectedType = SelectedTypeFilter;
        var selectedStatus = SelectedStatusFilter;

        TypeFilters.Clear();
        TypeFilters.Add(new FilterOption(TypeFilterAll, Strings.Get("FilterAll")));
        TypeFilters.Add(new FilterOption(TypeFilterRegistry, Strings.Get("FilterTypeRegistry")));
        TypeFilters.Add(new FilterOption(TypeFilterStartupFolder, Strings.Get("FilterTypeStartupFolder")));
        TypeFilters.Add(new FilterOption(TypeFilterTasks, Strings.Get("FilterTypeTasks")));
        TypeFilters.Add(new FilterOption(TypeFilterServices, Strings.Get("FilterTypeServices")));
        TypeFilters.Add(new FilterOption(TypeFilterDrivers, Strings.Get("FilterTypeDrivers")));
        TypeFilters.Add(new FilterOption(TypeFilterExpert, Strings.Get("FilterTypeExpert")));

        StatusFilters.Clear();
        StatusFilters.Add(new FilterOption(StatusFilterAll, Strings.Get("FilterAll")));
        StatusFilters.Add(new FilterOption(StatusFilterEnabled, Strings.Get("FilterStatusEnabled")));
        StatusFilters.Add(new FilterOption(StatusFilterDisabled, Strings.Get("FilterStatusDisabled")));
        StatusFilters.Add(new FilterOption(StatusFilterSuspicious, Strings.Get("FilterStatusSuspicious")));

        if (!TypeFilters.Any(option => option.Key == selectedType))
        {
            selectedType = TypeFilterAll;
        }

        if (!StatusFilters.Any(option => option.Key == selectedStatus))
        {
            selectedStatus = StatusFilterAll;
        }

        SelectedTypeFilter = selectedType;
        SelectedStatusFilter = selectedStatus;
        OnPropertyChanged(nameof(CanResetFilters));
    }

    private bool FilterEntries(object obj)
    {
        if (obj is not StartupEntry entry)
            return false;

        // Filter by navigation tab (grouped by user intention)
        bool matchesTab = SelectedTab switch
        {
            NavigationTab.Applications =>
                entry.Category == EntryCategory.Startup || entry.Category == EntryCategory.Tasks,

            NavigationTab.Browsers =>
                entry.Category == EntryCategory.Extensions || entry.Type == EntryType.BHO,

            NavigationTab.System =>
                entry.Type is EntryType.Service or EntryType.Driver or EntryType.PrintMonitor,

            NavigationTab.Advanced =>
                entry.Type is EntryType.Winlogon or EntryType.AppInitDlls or EntryType.IFEO or
                EntryType.ShellExtension or EntryType.SessionManager or EntryType.WinsockLSP,

            _ => true
        };
        if (!matchesTab) return false;

        // In non-expert mode, hide Microsoft entries and critical items
        if (!IsExpertMode)
        {
            if (entry.Publisher?.Contains("Microsoft", StringComparison.OrdinalIgnoreCase) == true
                && entry.SignatureStatus == SignatureStatus.SignedTrusted)
                return false;

            if (entry.IsProtected)
                return false;

            // Advanced tab is only visible in Expert mode (handled in XAML)
            // But also filter here as a safety measure
            if (SelectedTab == NavigationTab.Advanced)
                return false;
        }

        // Microsoft Noise Filter (Global - applies to ALL entry types)
        if (HideMicrosoftEntries)
        {
            bool isMicrosoft = entry.Publisher?.Contains("Microsoft", StringComparison.OrdinalIgnoreCase) == true ||
                               entry.CompanyName?.Contains("Microsoft", StringComparison.OrdinalIgnoreCase) == true ||
                               entry.ProtectionReason?.Contains("Microsoft", StringComparison.OrdinalIgnoreCase) == true;
            if (isMicrosoft)
                return false;
        }

        // Apply type filter (for sub-filtering within tab)
        if (SelectedTypeFilter != TypeFilterAll)
        {
            var matchesType = SelectedTypeFilter switch
            {
                TypeFilterRegistry => entry.Type is EntryType.RegistryRun or EntryType.RegistryRunOnce or EntryType.RegistryPolicies,
                TypeFilterStartupFolder => entry.Type == EntryType.StartupFolder,
                TypeFilterTasks => entry.Type == EntryType.ScheduledTask,
                TypeFilterServices => entry.Type == EntryType.Service,
                TypeFilterDrivers => entry.Type == EntryType.Driver,
                TypeFilterExpert => entry.Type is EntryType.IFEO or EntryType.Winlogon or EntryType.ShellExtension or
                                    EntryType.BHO or EntryType.PrintMonitor or EntryType.SessionManager or
                                    EntryType.AppInitDlls or EntryType.WinsockLSP,
                _ => true
            };
            if (!matchesType) return false;
        }

        // Apply status filter
        if (SelectedStatusFilter != StatusFilterAll)
        {
            var matchesStatus = SelectedStatusFilter switch
            {
                StatusFilterEnabled => entry.Status == EntryStatus.Enabled,
                StatusFilterDisabled => entry.Status == EntryStatus.Disabled,
                StatusFilterSuspicious => entry.RiskLevel is RiskLevel.Suspicious or RiskLevel.Critical,
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
        StatusMessage = Strings.Get("ProgressStep1");

        try
        {
            _logger.LogInformation("Starting scan with {Count} providers", _providers.Count());

            Entries.Clear();
            var providerList = _providers.Where(p => p.IsAvailable()).ToList();
            var totalSteps = providerList.Count + 2; // providers + risk analysis + finalization
            var currentStep = 0;

            // Step 1: Initialize
            ProgressText = $"{Strings.Get("Step")} 1/{totalSteps}";
            StatusMessage = Strings.Get("ProgressStep1");
            await Task.Delay(100, token); // Small delay for UI update

            // Step 2+: Scan each provider with progress
            var completedProviders = 0;
            var allEntries = new System.Collections.Concurrent.ConcurrentBag<(IStartupProvider Provider, List<StartupEntry> Entries, Exception? Error)>();

            var scanTasks = providerList.Select(async provider =>
            {
                try
                {
                    _logger.LogDebug("Starting scan for provider {Provider}", provider.DisplayName);
                    var entries = await provider.ScanAsync(token);
                    _logger.LogInformation("Provider {Provider} found {Count} entries",
                        provider.DisplayName, entries.Count);

                    // Update progress on completion
                    var completed = System.Threading.Interlocked.Increment(ref completedProviders);
                    await Application.Current.Dispatcher.InvokeAsync(() =>
                    {
                        currentStep = completed + 1;
                        ProgressText = $"{Strings.Get("Step")} {currentStep}/{totalSteps}";
                        StatusMessage = Strings.Format("ProgressScanning", provider.DisplayName, entries.Count);
                    });

                    return (Provider: provider, Entries: entries, Error: (Exception?)null);
                }
                catch (OperationCanceledException)
                {
                    throw;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error scanning with provider {Provider}", provider.DisplayName);
                    System.Threading.Interlocked.Increment(ref completedProviders);
                    return (Provider: provider, Entries: new List<StartupEntry>(), Error: ex);
                }
            }).ToList();

            var results = await Task.WhenAll(scanTasks);

            token.ThrowIfCancellationRequested();

            // Step N-1: Risk analysis
            currentStep = providerList.Count + 1;
            ProgressText = $"{Strings.Get("Step")} {currentStep}/{totalSteps}";
            StatusMessage = Strings.Get("ProgressAnalyzing");

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

            // Step N: Finalization
            currentStep = totalSteps;
            ProgressText = $"{Strings.Get("Step")} {currentStep}/{totalSteps}";
            StatusMessage = Strings.Get("ProgressFinalizing");

            _entriesView.Refresh();
            UpdateCounts();

            StatusMessage = Strings.Format("ProgressComplete", totalEntries);
            _logger.LogInformation("Scan complete: {Count} entries found", totalEntries);
        }
        catch (OperationCanceledException)
        {
            StatusMessage = Strings.Get("StatusScanCancelled");
            _logger.LogInformation("Scan cancelled by user");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during scan");
            StatusMessage = Strings.Format("StatusError", ex.Message);
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
                var scanOutcomes = new List<(StartupEntry Entry, ScanResult Result)>(riskyEntries.Count);

                foreach (var entry in riskyEntries)
                {
                    // Use semaphore to scan one at a time
                    await _scanSemaphore.WaitAsync();
                    try
                    {
                        _logger.LogDebug("Smart scan: scanning {Name}", entry.DisplayName);

                        var result = await _malwareScanner.ScanAsync(entry.TargetPath!);
                        scanOutcomes.Add((entry, result));
                    }
                    catch (Exception ex)
                    {
                        _logger.LogDebug(ex, "Smart scan: error scanning {Name}", entry.DisplayName);
                        scanOutcomes.Add((entry, ScanResult.Error));
                    }
                    finally
                    {
                        _scanSemaphore.Release();
                    }
                }

                // Apply bound-property updates on the UI thread.
                await Application.Current.Dispatcher.InvokeAsync(() =>
                {
                    var malwareDetected = new List<StartupEntry>();
                    foreach (var (entry, result) in scanOutcomes)
                    {
                        entry.MalwareScanResult = result;
                        entry.LastMalwareScan = DateTime.Now;

                        if (result != ScanResult.Malware)
                            continue;

                        entry.RiskLevel = RiskLevel.Critical;
                        malwareDetected.Add(entry);
                        _logger.LogWarning("Smart scan: MALWARE detected in {Path}", entry.TargetPath);
                    }

                    if (malwareDetected.Count == 0)
                        return;

                    _entriesView.Refresh();
                    UpdateCounts();

                    var names = string.Join(", ", malwareDetected.Select(e => e.DisplayName));
                    _logger.LogWarning("Smart scan completed: {Count} threat(s) detected: {Names}",
                        malwareDetected.Count, names);

                    StatusMessage = Strings.Format("StatusSmartScanThreats", malwareDetected.Count);
                });
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
        // Navigation tab counts
        OnPropertyChanged(nameof(ApplicationsCount));
        OnPropertyChanged(nameof(BrowsersCount));
        OnPropertyChanged(nameof(SystemTabCount));
        OnPropertyChanged(nameof(AdvancedCount));
    }

    [RelayCommand]
    private void CancelOperation()
    {
        _cancellationTokenSource?.Cancel();
        StatusMessage = Strings.Get("StatusCancelling");
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

    private bool CanDisableSelected() =>
        SelectedEntry != null &&
        SelectedEntry.Status == EntryStatus.Enabled &&
        !SelectedEntry.IsProtected &&
        _actionExecutor.CanPerformAction(SelectedEntry, ActionType.Disable);

    [RelayCommand(CanExecute = nameof(CanDisableSelected))]
    private async Task DisableSelectedAsync()
    {
        if (SelectedEntry == null)
            return;

        if (SelectedEntry.IsProtected)
        {
            _dialogService.ShowWarning(
                Strings.Format("ProtectedEntryMessage", SelectedEntry.ProtectionReason),
                Strings.Get("ErrorActionImpossible"));
            return;
        }

        // Check if admin is required
        if (_actionExecutor.RequiresAdmin(SelectedEntry, ActionType.Disable) && !IsAdmin)
        {
            if (_dialogService.Confirm(
                Strings.Get("AdminRequiredMessage"),
                Strings.Get("AdminRequiredTitle")))
            {
                if (UacHelper.RestartAsAdmin())
                {
                    Application.Current.Shutdown();
                }
            }
            return;
        }

        _logger.LogInformation("Disabling entry: {Id}", SelectedEntry.Id);
        StatusMessage = Strings.Format("StatusDisabling", SelectedEntry.DisplayName);

        var actionResult = await _actionExecutor.DisableAsync(SelectedEntry);

        if (actionResult.Success)
        {
            _entriesView.Refresh();
            UpdateCounts();
            StatusMessage = Strings.Format("NotifDisabled", SelectedEntry.DisplayName);
            _toastService.ShowSuccess(StatusMessage);

            // Show browser restart warning for browser extensions
            if (SelectedEntry.Type == EntryType.BrowserExtension)
            {
                _toastService.ShowWarning(Strings.Get("NotifBrowserRestartRequired"));
            }

            // Update command states after status change
            EnableSelectedCommand.NotifyCanExecuteChanged();
            DisableSelectedCommand.NotifyCanExecuteChanged();
        }
        else
        {
            StatusMessage = Strings.Format("StatusError", actionResult.ErrorMessage);
            _dialogService.ShowError(
                actionResult.ErrorMessage,
                Strings.Get("ErrorTitle"));
        }
    }

    private bool CanEnableSelected() =>
        SelectedEntry != null &&
        SelectedEntry.Status == EntryStatus.Disabled &&
        _actionExecutor.CanPerformAction(SelectedEntry, ActionType.Enable);

    [RelayCommand(CanExecute = nameof(CanEnableSelected))]
    private async Task EnableSelectedAsync()
    {
        if (SelectedEntry == null)
            return;

        // Check if admin is required
        if (_actionExecutor.RequiresAdmin(SelectedEntry, ActionType.Enable) && !IsAdmin)
        {
            if (_dialogService.Confirm(
                Strings.Get("AdminRequiredMessage"),
                Strings.Get("AdminRequiredTitle")))
            {
                if (UacHelper.RestartAsAdmin())
                {
                    Application.Current.Shutdown();
                }
            }
            return;
        }

        _logger.LogInformation("Enabling entry: {Id}", SelectedEntry.Id);
        StatusMessage = Strings.Format("StatusEnabling", SelectedEntry.DisplayName);

        var actionResult = await _actionExecutor.EnableAsync(SelectedEntry);

        if (actionResult.Success)
        {
            _entriesView.Refresh();
            UpdateCounts();
            StatusMessage = Strings.Format("NotifEnabled", SelectedEntry.DisplayName);
            _toastService.ShowSuccess(StatusMessage);

            // Show browser restart warning for browser extensions
            if (SelectedEntry.Type == EntryType.BrowserExtension)
            {
                _toastService.ShowWarning(Strings.Get("NotifBrowserRestartRequired"));
            }

            // Update command states after status change
            EnableSelectedCommand.NotifyCanExecuteChanged();
            DisableSelectedCommand.NotifyCanExecuteChanged();
        }
        else
        {
            StatusMessage = Strings.Format("StatusError", actionResult.ErrorMessage);
            _dialogService.ShowError(
                actionResult.ErrorMessage,
                Strings.Get("ErrorTitle"));
        }
    }

    [RelayCommand]
    private async Task DeleteSelectedAsync()
    {
        if (SelectedEntry == null || !IsExpertMode)
            return;

        if (SelectedEntry.IsProtected)
        {
            _dialogService.ShowWarning(
                Strings.Format("ProtectedEntryMessage", SelectedEntry.ProtectionReason),
                Strings.Get("ErrorActionImpossible"));
            return;
        }

        // Confirmation dialog with animation
        var confirmDialog = Views.ConfirmationDialog.ForDelete(SelectedEntry.DisplayName, Application.Current.MainWindow);
        if (confirmDialog.ShowDialog() != true)
            return;

        // Check if admin is required
        if (_actionExecutor.RequiresAdmin(SelectedEntry, ActionType.Delete) && !IsAdmin)
        {
            if (_dialogService.Confirm(
                Strings.Get("AdminRequiredMessage"),
                Strings.Get("AdminRequiredTitle")))
            {
                if (UacHelper.RestartAsAdmin())
                {
                    Application.Current.Shutdown();
                }
            }
            return;
        }

        _logger.LogInformation("Deleting entry: {Id}", SelectedEntry.Id);
        StatusMessage = Strings.Format("StatusDeleting", SelectedEntry.DisplayName);

        var actionResult = await _actionExecutor.DeleteAsync(SelectedEntry);

        if (actionResult.Success)
        {
            var entryName = SelectedEntry.DisplayName;
            Entries.Remove(SelectedEntry);
            SelectedEntry = null;

            _entriesView.Refresh();
            UpdateCounts();

            StatusMessage = Strings.Format("NotifDeleted", entryName);
            _toastService.ShowSuccess(StatusMessage);
        }
        else
        {
            StatusMessage = Strings.Format("StatusError", actionResult.ErrorMessage);
            _dialogService.ShowError(
                actionResult.ErrorMessage,
                Strings.Get("ErrorTitle"));
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
            _dialogService.ShowInfo(
                Strings.Get("NoDisabledEntrySelected"),
                Strings.Get("InfoTitle"));
            return;
        }

        if (!_dialogService.Confirm(
            Strings.Format("ConfirmEnableBatchMessage", entriesToEnable.Count),
            Strings.Get("ConfirmEnableBatchTitle")))
            return;

        _logger.LogInformation("Batch enabling {Count} entries", entriesToEnable.Count);
        IsLoading = true;
        StatusMessage = Strings.Format("StatusBatchEnabling", entriesToEnable.Count);

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
                StatusMessage = Strings.Format("StatusEnabling", entry.DisplayName);

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
                StatusMessage = Strings.Format("StatusBatchEnabled", successCount);
            }
            else
            {
                StatusMessage = Strings.Format("StatusBatchEnabledWithFailures", successCount, failCount);
                _dialogService.ShowWarning(
                    Strings.Format("BatchEnableResultMessage", successCount, failCount),
                    Strings.Get("BatchResultTitle"));
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
            _dialogService.ShowInfo(
                Strings.Get("NoEnabledEntrySelected"),
                Strings.Get("InfoTitle"));
            return;
        }

        if (!_dialogService.Confirm(
            Strings.Format("ConfirmDisableBatchMessage", entriesToDisable.Count),
            Strings.Get("ConfirmDisableBatchTitle")))
            return;

        _logger.LogInformation("Batch disabling {Count} entries", entriesToDisable.Count);
        IsLoading = true;
        StatusMessage = Strings.Format("StatusBatchDisabling", entriesToDisable.Count);

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
                StatusMessage = Strings.Format("StatusDisabling", entry.DisplayName);

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
                StatusMessage = Strings.Format("StatusBatchDisabled", successCount);
            }
            else
            {
                StatusMessage = Strings.Format("StatusBatchDisabledWithFailures", successCount, failCount);
                _dialogService.ShowWarning(
                    Strings.Format("BatchDisableResultMessage", successCount, failCount),
                    Strings.Get("BatchResultTitle"));
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
                    _processLauncher.OpenFolderAndSelectFile(SelectedEntry.TargetPath);
                }
                else
                {
                    _processLauncher.OpenFolder(directory);
                }
            }
            else
            {
                _dialogService.ShowInfo(
                    Strings.Get("FolderNotFound"),
                    Strings.Get("InfoTitle"));
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error opening file location");
            _dialogService.ShowError(
                Strings.Format("ErrorOpenLocation", ex.Message),
                Strings.Get("ErrorTitle"));
        }
    }

    [RelayCommand]
    private void CopyPath()
    {
        if (SelectedEntry?.TargetPath != null)
        {
            _clipboardService.SetText(SelectedEntry.TargetPath);
            StatusMessage = Strings.Get("NotifCopied");
        }
        else if (SelectedEntry?.CommandLineRaw != null)
        {
            _clipboardService.SetText(SelectedEntry.CommandLineRaw);
            StatusMessage = Strings.Get("NotifCopied");
        }
    }

    [RelayCommand]
    private void WebSearch()
    {
        if (SelectedEntry == null)
            return;

        var query = Uri.EscapeDataString($"{SelectedEntry.DisplayName} {SelectedEntry.Publisher ?? ""}");
        var url = $"{Constants.Urls.WebSearchBase}{query}";

        try
        {
            _processLauncher.OpenUrl(url);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error opening web search");
        }
    }

    private bool CanOpenInRegedit() =>
        SelectedEntry != null &&
        SelectedEntry.SourcePath != null &&
        SelectedEntry.SourcePath.StartsWith("HK", StringComparison.OrdinalIgnoreCase);

    [RelayCommand(CanExecute = nameof(CanOpenInRegedit))]
    private void OpenInRegedit()
    {
        if (SelectedEntry?.SourcePath == null)
            return;

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

            _processLauncher.StartShellExecute("regedit.exe");
            StatusMessage = Strings.Get("StatusRegeditOpened");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error opening regedit");
            _dialogService.ShowError(
                Strings.Format("ErrorOpeningRegedit", ex.Message),
                Strings.Get("ErrorTitle"));
        }
    }

    private bool CanOpenInServices() =>
        SelectedEntry != null &&
        SelectedEntry.Type == EntryType.Service;

    [RelayCommand(CanExecute = nameof(CanOpenInServices))]
    private void OpenInServices()
    {
        if (SelectedEntry == null)
            return;

        try
        {
            _processLauncher.StartShellExecute("services.msc");
            StatusMessage = Strings.Get("StatusServicesOpened");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error opening services.msc");
            _dialogService.ShowError(
                $"{Strings.Get("ErrorOpeningServices")}: {ex.Message}",
                Strings.Get("ErrorTitle"));
        }
    }

    private bool CanOpenInTaskScheduler() =>
        SelectedEntry != null &&
        SelectedEntry.Type == EntryType.ScheduledTask;

    [RelayCommand(CanExecute = nameof(CanOpenInTaskScheduler))]
    private void OpenInTaskScheduler()
    {
        if (SelectedEntry == null)
            return;

        try
        {
            _processLauncher.StartShellExecute("taskschd.msc");
            StatusMessage = Strings.Get("StatusTaskSchedulerOpened");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error opening task scheduler");
            _dialogService.ShowError(
                $"{Strings.Get("ErrorOpeningTaskScheduler")}: {ex.Message}",
                Strings.Get("ErrorTitle"));
        }
    }

    [RelayCommand]
    private async Task CalculateHashAsync()
    {
        if (SelectedEntry?.TargetPath == null || !SelectedEntry.FileExists)
        {
            _dialogService.ShowWarning(
                Strings.Get("ErrorHashFileMissing"),
                Strings.Get("ErrorTitle"));
            return;
        }

        try
        {
            StatusMessage = Strings.Get("StatusHashCalculating");
            var hashCalculator = new HashCalculator();
            var hash = await hashCalculator.CalculateSha256Async(SelectedEntry.TargetPath);

            SelectedEntry.Sha256 = hash;
            OnPropertyChanged(nameof(SelectedEntry));

            // Copy to clipboard and show result
            _clipboardService.SetText(hash);
            StatusMessage = Strings.Format("StatusHashCalculated", hash[..16]);

            _dialogService.ShowInfo(
                Strings.Format("HashCalculatedMessage", hash),
                Strings.Get("HashCalculatedTitle"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error calculating hash");
            StatusMessage = Strings.Format("ErrorHashCalculation", ex.Message);
            _dialogService.ShowError(
                Strings.Format("ErrorHashCalculationWithDetails", ex.Message),
                Strings.Get("ErrorTitle"));
        }
    }

    [RelayCommand]
    private async Task AnalyzeWithVirusTotalAsync()
    {
        if (SelectedEntry == null) return;
        
        if (!_virusTotalService.IsConfigured)
        {
             if (_dialogService.Confirm(
                Strings.Get("VTNotConfiguredMessage"),
                Strings.Get("VTConfigRequiredTitle")))
             {
                 OpenSettings();
             }
             return;
        }

        if (string.IsNullOrEmpty(SelectedEntry.Sha256))
        {
             await CalculateHashAsync();
             if (string.IsNullOrEmpty(SelectedEntry.Sha256)) return;
        }

        try
        {
            StatusMessage = Strings.Get("VTQuerying");
            IsLoading = true;
            var report = await _virusTotalService.GetFileReportAsync(SelectedEntry.Sha256);
            
            if (report == null)
            {
                StatusMessage = Strings.Get("VTUnknownFile");
                _toastService.ShowWarning(Strings.Get("VTUnknownFileToast"));
                return;
            }
            
            var stats = report.Attributes?.LastAnalysisStats;
            if (stats != null)
            {
                var total = stats.Harmless + stats.Suspicious + stats.Malicious + stats.Undetected;
                var score = $"{stats.Malicious}/{total}";
                
                SelectedEntry.VirusTotalScore = score;
                SelectedEntry.VirusTotalLink = $"{Constants.Urls.VirusTotalGui}{SelectedEntry.Sha256}";
                OnPropertyChanged(nameof(SelectedEntry));
                
                StatusMessage = Strings.Format("VTDetections", score);
                
                if (stats.Malicious > 0)
                {
                    _toastService.ShowWarning(Strings.Format("VTThreatDetected", stats.Malicious));
                    SelectedEntry.RiskLevel = RiskLevel.Critical;
                }
                else
                {
                    _toastService.ShowSuccess(Strings.Get("VTFileClean"));
                }
            }
        }
        catch (Exception ex)
        {
            StatusMessage = Strings.Get("VTError");
            _toastService.ShowError(Strings.Format("VTErrorDetail", ex.Message));
        }
        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand]
    private async Task ScanSelectedAsync()
    {
        if (SelectedEntry?.TargetPath == null || !SelectedEntry.FileExists)
        {
            _dialogService.ShowWarning(
                Strings.Get("ErrorScanFileMissing"),
                Strings.Get("ErrorTitle"));
            return;
        }

        if (_malwareScanner == null)
        {
            _dialogService.ShowWarning(
                Strings.Get("ScanNotAvailable"),
                Strings.Get("ErrorTitle"));
            return;
        }

        try
        {
            StatusMessage = Strings.Get("ScanInProgress");
            _logger.LogInformation("Scanning file: {Path}", SelectedEntry.TargetPath);

            var result = await _malwareScanner.ScanAsync(SelectedEntry.TargetPath);

            SelectedEntry.MalwareScanResult = result;
            SelectedEntry.LastMalwareScan = DateTime.Now;
            OnPropertyChanged(nameof(SelectedEntry));

            switch (result)
            {
                case ScanResult.Clean:
                    StatusMessage = Strings.Get("ScanResultClean");
                    break;

                case ScanResult.Malware:
                    StatusMessage = Strings.Get("ScanResultMalware");
                    SelectedEntry.RiskLevel = RiskLevel.Critical;
                    _entriesView.Refresh();
                    UpdateCounts();

                    _dialogService.ShowWarning(
                        Strings.Format("MalwareDetectedDetailedMessage", SelectedEntry.TargetPath),
                        Strings.Get("MalwareDetectedTitle"));
                    break;

                case ScanResult.Blocked:
                    StatusMessage = Strings.Get("ScanResultBlocked");
                    break;

                case ScanResult.NotScanned:
                    StatusMessage = Strings.Get("ScanResultNotScanned");
                    break;

                case ScanResult.TooLarge:
                    StatusMessage = Strings.Get("ScanResultTooLarge");
                    break;

                case ScanResult.NoAntivirusProvider:
                    StatusMessage = Strings.Get("ScanResultNoAntivirusProvider");
                    _dialogService.ShowInfo(
                        Strings.Get("ScanNoAntivirusProviderMessage"),
                        Strings.Get("ScanNoAntivirusProviderTitle"));
                    break;

                case ScanResult.Error:
                    StatusMessage = Strings.Get("ScanResultError");
                    break;
            }

            _logger.LogInformation("Scan result for {Path}: {Result}", SelectedEntry.TargetPath, result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during malware scan");
            StatusMessage = Strings.Format("ErrorScanStatus", ex.Message);
            _dialogService.ShowError(
                Strings.Format("ErrorScanMessageWithDetails", ex.Message),
                Strings.Get("ErrorTitle"));
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

    /// <summary>
    /// Gets the scan percentage for progress bar display.
    /// </summary>
    public int ScanPercentage => ScanTotal > 0 ? (int)((double)ScanProgress / ScanTotal * 100) : 0;

    partial void OnScanProgressChanged(int value)
    {
        OnPropertyChanged(nameof(ScanPercentage));
    }

    partial void OnScanTotalChanged(int value)
    {
        OnPropertyChanged(nameof(ScanPercentage));
    }

    private CancellationTokenSource? _scanCts;

    [RelayCommand]
    private async Task ScanAllAsync()
    {
        if (_malwareScanner == null)
        {
            _dialogService.ShowWarning(
                Strings.Get("ScanNotAvailable"),
                Strings.Get("ErrorTitle"));
            return;
        }

        // Get all scannable entries (visible, with existing files)
        var entriesToScan = Entries
            .Where(e => e.FileExists && !string.IsNullOrEmpty(e.TargetPath))
            .ToList();

        if (entriesToScan.Count == 0)
        {
            _dialogService.ShowInfo(
                Strings.Get("ScanGlobalNoFiles"),
                Strings.Get("ScanGlobalTitle"));
            return;
        }

        if (!_dialogService.Confirm(
            Strings.Format("ScanGlobalConfirm", entriesToScan.Count),
            Strings.Get("ScanGlobalTitle")))
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
                ScanProgressText = Strings.Format("ScanProgressFormat", ScanProgress, ScanTotal, Path.GetFileName(entry.TargetPath));
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
                ? Strings.Get("ScanGlobalInterruptedHeader")
                : Strings.Get("ScanGlobalCompletedHeader");

            message += Strings.Format("ScanGlobalScannedCount", scanned);
            message += Strings.Format("ScanGlobalCleanCount", clean);
            if (threats > 0)
                message += Strings.Format("ScanGlobalThreatsCount", threats);
            if (errors > 0)
                message += Strings.Format("ScanGlobalErrorsCount", errors);

            StatusMessage = threats > 0
                ? Strings.Format("ScanGlobalStatusThreats", threats)
                : Strings.Format("ScanGlobalStatusClean", clean);

            if (threats > 0)
            {
                _dialogService.ShowWarning(
                    message,
                    Strings.Get("ScanGlobalThreatsTitle"));
            }
            else
            {
                _dialogService.ShowInfo(
                    message,
                    Strings.Get("ScanComplete"));
            }

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
        StatusMessage = Strings.Get("ScanGlobalCancelling");
    }

    [RelayCommand]
    private async Task ExportJsonAsync()
    {
        var filePath = _dialogService.ShowSaveFileDialog(
            "JSON Files (*.json)|*.json",
            ".json",
            $"BootSentry_Export_{DateTime.Now:yyyyMMdd_HHmmss}");

        if (filePath != null)
        {
            try
            {
                StatusMessage = Strings.Get("StatusExporting");
                var options = new ExportOptions { IncludeDetails = true, IncludeKnowledgeInfo = true };
                await _exportService.ExportToFileAsync(Entries, filePath, ExportFormat.Json, options);
                StatusMessage = Strings.Format("StatusExportComplete", filePath);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error exporting to JSON");
                StatusMessage = Strings.Format("StatusExportError", ex.Message);
            }
        }
    }

    [RelayCommand]
    private async Task ExportCsvAsync()
    {
        var filePath = _dialogService.ShowSaveFileDialog(
            "CSV Files (*.csv)|*.csv",
            ".csv",
            $"BootSentry_Export_{DateTime.Now:yyyyMMdd_HHmmss}");

        if (filePath != null)
        {
            try
            {
                StatusMessage = Strings.Get("StatusExporting");
                var options = new ExportOptions { IncludeDetails = true, IncludeKnowledgeInfo = true };
                await _exportService.ExportToFileAsync(Entries, filePath, ExportFormat.Csv, options);
                StatusMessage = Strings.Format("StatusExportComplete", filePath);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error exporting to CSV");
                StatusMessage = Strings.Format("StatusExportError", ex.Message);
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
    private void OpenSnapshots()
    {
        var snapshotWindow = new Views.SnapshotWindow
        {
            Owner = Application.Current.MainWindow
        };
        snapshotWindow.ShowDialog();
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
        StatusMessage = Strings.Get("SelectAllHint");
    }

    [RelayCommand]
    private async Task UndoAsync()
    {
        try
        {
            // Find the latest transaction that is actually restorable.
            var transactions = await _transactionManager.GetTransactionsAsync();
            var lastTransaction = transactions.FirstOrDefault(t => t.CanRestore);

            if (lastTransaction == null)
            {
                _dialogService.ShowInfo(
                    Strings.Get("UndoNoActionMessage"),
                    Strings.Get("UndoNoActionTitle"));
                return;
            }

            if (!_dialogService.Confirm(
                Strings.Format("UndoConfirmMessage", lastTransaction.ActionType, lastTransaction.EntryDisplayName),
                Strings.Get("UndoConfirmTitle")))
                return;

            _logger.LogInformation("Undoing transaction: {Id}", lastTransaction.Id);
            StatusMessage = Strings.Format("UndoInProgress", lastTransaction.ActionType, lastTransaction.EntryDisplayName);

            var result = await _transactionManager.RollbackAsync(lastTransaction.Id);

            if (result.Success)
            {
                StatusMessage = Strings.Format("UndoSuccess", lastTransaction.EntryDisplayName);

                // Refresh the list to show the restored entry
                await RefreshAsync();
            }
            else
            {
                StatusMessage = Strings.Format("StatusError", result.ErrorMessage);
                _dialogService.ShowError(
                    result.ErrorMessage,
                    Strings.Get("UndoErrorTitle"));
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error undoing last action");
            StatusMessage = Strings.Format("StatusError", ex.Message);
            _dialogService.ShowError(
                Strings.Format("UndoErrorMessage", ex.Message),
                Strings.Get("ErrorTitle"));
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
        _processLauncher.OpenUrl(Constants.Urls.GitHubDocumentation);
    }

    [RelayCommand]
    private void GoToGitHub()
    {
        _processLauncher.OpenUrl(Constants.Urls.GitHubRepository);
    }

    [RelayCommand]
    private async Task CheckUpdatesAsync()
    {
        try
        {
            StatusMessage = Strings.Get("StatusCheckingUpdates");
            using var checker = new UpdateChecker();
            var update = await checker.CheckForUpdateAsync();

            if (update != null)
            {
                if (_dialogService.Confirm(
                    Strings.Format("UpdateMessage", update.LatestVersion),
                    Strings.Get("UpdateAvailable")))
                {
                    _processLauncher.OpenUrl(update.ReleaseUrl);
                }
            }
            else
            {
                _dialogService.ShowInfo(
                    Strings.Get("UpdateUpToDate"),
                    Strings.Get("InfoTitle"));
            }

            StatusMessage = Strings.Get("StatusReady");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking for updates");
            StatusMessage = Strings.Format("StatusError", ex.Message);
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
        var filePath = _dialogService.ShowSaveFileDialog(
            "ZIP Files (*.zip)|*.zip",
            ".zip",
            $"BootSentry_Diagnostics_{DateTime.Now:yyyyMMdd_HHmmss}");

        if (filePath != null)
        {
            try
            {
                StatusMessage = Strings.Get("StatusDiagnosticsCreating");
                await _exportService.ExportDiagnosticsZipAsync(Entries, filePath);
                StatusMessage = Strings.Format("StatusDiagnosticsExported", filePath);

                if (_dialogService.Confirm(
                    Strings.Format("DiagnosticsCreatedOpenPrompt", filePath),
                    Strings.Get("DiagnosticsCreatedTitle")))
                {
                    _processLauncher.OpenFolderAndSelectFile(filePath);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error exporting diagnostics");
                StatusMessage = Strings.Format("StatusExportError", ex.Message);
                _dialogService.ShowError(
                    Strings.Format("ErrorExportWithDetails", ex.Message),
                    Strings.Get("ErrorTitle"));
            }
        }
    }

    [RelayCommand]
    private void Exit()
    {
        if (Application.Current is App app)
        {
            app.IsExiting = true;
        }
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
            Strings.LanguageChanged -= OnLanguageChanged;

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

public sealed record FilterOption(string Key, string Label);
