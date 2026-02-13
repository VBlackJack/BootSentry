using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using BootSentry.Core.Interfaces;
using BootSentry.UI.Resources;

namespace BootSentry.UI.ViewModels;

/// <summary>
/// View model for the transaction history and rollback functionality.
/// </summary>
public partial class HistoryViewModel : ObservableObject
{
    private const int DefaultHistoryLimit = 500;

    private readonly ILogger<HistoryViewModel> _logger;
    private readonly ITransactionManager _transactionManager;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(HasNoTransactions))]
    private ObservableCollection<TransactionItem> _transactions = [];

    [ObservableProperty]
    private TransactionItem? _selectedTransaction;

    /// <summary>
    /// Gets whether there are no transactions to display.
    /// </summary>
    public bool HasNoTransactions => !IsLoading && Transactions.Count == 0;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(HasNoTransactions))]
    private bool _isLoading;

    [ObservableProperty]
    private string? _statusMessage;

    // Design-time constructor
    public HistoryViewModel() : this(null!, null!) { }

    public HistoryViewModel(
        ILogger<HistoryViewModel> logger,
        ITransactionManager transactionManager)
    {
        _logger = logger;
        _transactionManager = transactionManager;
    }

    /// <summary>
    /// Loads all transactions from storage.
    /// </summary>
    [RelayCommand]
    private async Task LoadTransactionsAsync()
    {
        try
        {
            IsLoading = true;
            StatusMessage = Strings.Get("HistoryLoading");

            var transactions = await _transactionManager.GetTransactionsAsync(DefaultHistoryLimit);

            Transactions.Clear();
            foreach (var tx in transactions.OrderByDescending(t => t.Timestamp))
            {
                Transactions.Add(new TransactionItem(tx));
            }

            StatusMessage = Strings.Format("HistoryCount", Transactions.Count);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to load transactions");
            StatusMessage = Strings.Get("HistoryLoadError");
        }
        finally
        {
            IsLoading = false;
        }
    }

    /// <summary>
    /// Rolls back the selected transaction.
    /// </summary>
    [RelayCommand]
    private async Task RollbackSelectedAsync()
    {
        if (SelectedTransaction == null || !SelectedTransaction.CanRestore)
            return;

        try
        {
            IsLoading = true;
            StatusMessage = Strings.Get("HistoryRestoring");

            var result = await _transactionManager.RollbackAsync(SelectedTransaction.Id);

            if (result.Success)
            {
                StatusMessage = Strings.Get("HistoryRestoreSuccess");
                SelectedTransaction.CanRestore = false;
                SelectedTransaction.StatusText = Strings.Get("HistoryRestored");
                _logger.LogInformation("Rolled back transaction {Id}", SelectedTransaction.Id);
            }
            else
            {
                StatusMessage = Strings.Format("HistoryRestoreError", result.ErrorMessage ?? Strings.Get("UnknownError"));
                _logger.LogWarning("Failed to rollback transaction {Id}: {Error}",
                    SelectedTransaction.Id, result.ErrorMessage);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during rollback of {Id}", SelectedTransaction?.Id);
            StatusMessage = Strings.Format("HistoryRestoreError", ex.Message);
        }
        finally
        {
            IsLoading = false;
        }
    }

    /// <summary>
    /// Purges old transactions based on retention policy.
    /// </summary>
    [RelayCommand]
    private async Task PurgeOldTransactionsAsync()
    {
        try
        {
            IsLoading = true;
            var purged = await _transactionManager.PurgeOldTransactionsAsync(
                maxAge: TimeSpan.FromDays(30),
                maxCount: 100);

            StatusMessage = Strings.Format("HistoryPurged", purged);
            _logger.LogInformation("Purged {Count} old transactions", purged);

            // Reload
            await LoadTransactionsAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to purge transactions");
            StatusMessage = Strings.Get("HistoryPurgeError");
        }
        finally
        {
            IsLoading = false;
        }
    }
}

/// <summary>
/// View model item for a single transaction.
/// </summary>
public partial class TransactionItem : ObservableObject
{
    public string Id { get; }
    public DateTime Timestamp { get; }
    public string User { get; }
    public string ActionType { get; }
    public string EntryDisplayName { get; }
    public string EntryType { get; }
    public string SourcePath { get; }

    [ObservableProperty]
    private bool _canRestore;

    [ObservableProperty]
    private string _statusText;

    public TransactionItem(Transaction transaction)
    {
        Id = transaction.Id;
        Timestamp = transaction.Timestamp.ToLocalTime();
        User = transaction.User;
        ActionType = transaction.ActionType.ToString();
        EntryDisplayName = transaction.EntryDisplayName;
        EntryType = transaction.EntrySnapshotBefore.Type.ToString();
        SourcePath = transaction.EntrySnapshotBefore.SourcePath;
        CanRestore = transaction.CanRestore;
        StatusText = transaction.CanRestore ? Strings.Get("HistoryCanRestore") : Strings.Get("HistoryCannotRestore");
    }
}
