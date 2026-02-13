using System.Collections.ObjectModel;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using BootSentry.Core.Interfaces;
using BootSentry.Core.Models;
using BootSentry.Core.Services;
using BootSentry.Core.Snapshots;
using BootSentry.UI.Resources;
using BootSentry.UI.Services;

namespace BootSentry.UI.ViewModels;

public partial class SnapshotViewModel : ObservableObject
{
    private readonly SnapshotManager _snapshotManager;
    private readonly IEnumerable<IStartupProvider> _providers;
    private readonly ToastService _toastService;

    [ObservableProperty]
    private ObservableCollection<StartupSnapshot> _snapshots = new();

    [ObservableProperty]
    private StartupSnapshot? _selectedSnapshot;

    [ObservableProperty]
    private SnapshotComparisonResult? _comparisonResult;

    [ObservableProperty]
    private bool _isLoading;

    [ObservableProperty]
    private bool _isComparing;

    public SnapshotViewModel(
        SnapshotManager snapshotManager,
        IEnumerable<IStartupProvider> providers,
        ToastService toastService)
    {
        _snapshotManager = snapshotManager;
        _providers = providers;
        _toastService = toastService;
        
        LoadSnapshotsCommand.Execute(null);
    }

    [RelayCommand]
    private async Task LoadSnapshotsAsync()
    {
        IsLoading = true;
        try
        {
            var list = await _snapshotManager.LoadSnapshotsAsync();
            Snapshots = new ObservableCollection<StartupSnapshot>(list);
        }
        catch (Exception ex)
        {
            _toastService.ShowError(Strings.Format("ErrorSnapshotLoad", ex.Message));
        }
        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand]
    private async Task CreateSnapshotAsync()
    {
        // In a real app, show a dialog to get name/description
        var name = Strings.Format("SnapshotAutoName", DateTime.Now);
        var description = Strings.Get("SnapshotManualDescription");

        IsLoading = true;
        try
        {
            // Collect current state
            var entries = new List<StartupEntry>();
            foreach (var provider in _providers)
            {
                if (provider.IsAvailable())
                {
                    var providerEntries = await provider.ScanAsync();
                    entries.AddRange(providerEntries);
                }
            }

            var snapshot = _snapshotManager.CreateSnapshot(name, description, entries);
            await _snapshotManager.SaveSnapshotAsync(snapshot);
            
            Snapshots.Insert(0, snapshot);
            _toastService.ShowSuccess(Strings.Format("NotifSnapshotCreated", name));
        }
        catch (Exception ex)
        {
            _toastService.ShowError(Strings.Format("ErrorSnapshotCreate", ex.Message));
        }
        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand]
    private async Task DeleteSnapshotAsync(StartupSnapshot? snapshot)
    {
        if (snapshot == null) return;

        try
        {
            _snapshotManager.DeleteSnapshot(snapshot); // This is synchronous in current impl
            Snapshots.Remove(snapshot);
            _toastService.ShowSuccess(Strings.Format("NotifSnapshotDeleted", snapshot.Name));
        }
        catch (Exception ex)
        {
            _toastService.ShowError(Strings.Format("ErrorSnapshotDelete", ex.Message));
        }
    }

    [RelayCommand]
    private async Task CompareWithCurrentAsync()
    {
        if (SelectedSnapshot == null) return;

        IsComparing = true;
        try
        {
            // Collect current state
            var currentEntries = new List<StartupEntry>();
            foreach (var provider in _providers)
            {
                if (provider.IsAvailable())
                {
                    var providerEntries = await provider.ScanAsync();
                    currentEntries.AddRange(providerEntries);
                }
            }

            var currentSnapshot = new StartupSnapshot
            {
                Name = Strings.Get("SnapshotCurrentState"),
                Entries = currentEntries
            };

            // Run comparison in background
            ComparisonResult = await Task.Run(() => _snapshotManager.Compare(SelectedSnapshot, currentSnapshot));
        }
        catch (Exception ex)
        {
            _toastService.ShowError(Strings.Format("ErrorComparisonFailed", ex.Message));
        }
        finally
        {
            IsComparing = false;
        }
    }
}
