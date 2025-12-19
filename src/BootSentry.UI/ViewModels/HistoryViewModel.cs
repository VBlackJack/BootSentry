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
    private readonly ILogger<HistoryViewModel> _logger;
    private readonly ITransactionManager _transactionManager;

    [ObservableProperty]
    private ObservableCollection<TransactionItem> _transactions = [];

    [ObservableProperty]
    private TransactionItem? _selectedTransaction;

    [ObservableProperty]
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
            StatusMessage = Strings.Get("History_Loading");

            var transactions = await _transactionManager.GetTransactionsAsync(100);

            Transactions.Clear();
            foreach (var tx in transactions.OrderByDescending(t => t.Timestamp))
            {
                Transactions.Add(new TransactionItem(tx));
            }

            StatusMessage = Strings.Format("History_Count", Transactions.Count);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to load transactions");
            StatusMessage = Strings.Get("History_LoadError");
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
            StatusMessage = Strings.Get("History_Restoring");

            var result = await _transactionManager.RollbackAsync(SelectedTransaction.Id);

            if (result.Success)
            {
                StatusMessage = Strings.Get("History_RestoreSuccess");
                SelectedTransaction.CanRestore = false;
                SelectedTransaction.StatusText = Strings.Get("History_Restored");
                _logger.LogInformation("Rolled back transaction {Id}", SelectedTransaction.Id);
            }
            else
            {
                StatusMessage = Strings.Format("History_RestoreError", result.ErrorMessage ?? "Unknown error");
                _logger.LogWarning("Failed to rollback transaction {Id}: {Error}",
                    SelectedTransaction.Id, result.ErrorMessage);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during rollback of {Id}", SelectedTransaction?.Id);
            StatusMessage = Strings.Format("History_RestoreError", ex.Message);
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

            StatusMessage = Strings.Format("History_Purged", purged);
            _logger.LogInformation("Purged {Count} old transactions", purged);

            // Reload
            await LoadTransactionsAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to purge transactions");
            StatusMessage = Strings.Get("History_PurgeError");
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
        StatusText = transaction.CanRestore ? Strings.Get("History_CanRestore") : Strings.Get("History_CannotRestore");
    }
}
