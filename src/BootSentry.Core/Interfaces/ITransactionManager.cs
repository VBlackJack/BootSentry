using BootSentry.Core.Enums;
using BootSentry.Core.Models;

namespace BootSentry.Core.Interfaces;

/// <summary>
/// Represents a backup transaction for rollback purposes.
/// </summary>
public record Transaction
{
    /// <summary>Unique transaction identifier.</summary>
    public required string Id { get; init; }

    /// <summary>When the transaction was created.</summary>
    public required DateTime Timestamp { get; init; }

    /// <summary>User who performed the action (SID or username).</summary>
    public required string User { get; init; }

    /// <summary>Type of action performed.</summary>
    public required ActionType ActionType { get; init; }

    /// <summary>ID of the affected entry.</summary>
    public required string EntryId { get; init; }

    /// <summary>Display name of the affected entry.</summary>
    public required string EntryDisplayName { get; init; }

    /// <summary>Snapshot of the entry before the action.</summary>
    public required StartupEntry EntrySnapshotBefore { get; init; }

    /// <summary>Paths to backup payload files.</summary>
    public IReadOnlyList<string> PayloadPaths { get; init; } = [];

    /// <summary>Whether this transaction can be restored.</summary>
    public bool CanRestore { get; init; } = true;

    /// <summary>Notes about the transaction.</summary>
    public string? Notes { get; init; }
}

/// <summary>
/// Interface for managing backup transactions and rollback.
/// </summary>
public interface ITransactionManager
{
    /// <summary>
    /// Creates a new transaction before performing an action.
    /// </summary>
    /// <param name="entry">The entry being modified.</param>
    /// <param name="actionType">The type of action to perform.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The created transaction.</returns>
    Task<Transaction> CreateTransactionAsync(
        StartupEntry entry,
        ActionType actionType,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Commits a transaction after successful action.
    /// </summary>
    /// <param name="transactionId">The transaction ID.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    Task CommitAsync(string transactionId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Rolls back a transaction (undo the action).
    /// </summary>
    /// <param name="transactionId">The transaction ID to rollback.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Result of the rollback operation.</returns>
    Task<ActionResult> RollbackAsync(string transactionId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all transactions (history).
    /// </summary>
    /// <param name="limit">Maximum number of transactions to return.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>List of transactions, most recent first.</returns>
    Task<IReadOnlyList<Transaction>> GetTransactionsAsync(
        int? limit = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a specific transaction by ID.
    /// </summary>
    /// <param name="transactionId">The transaction ID.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The transaction, or null if not found.</returns>
    Task<Transaction?> GetTransactionAsync(string transactionId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes old transactions based on retention policy.
    /// </summary>
    /// <param name="maxAge">Maximum age of transactions to keep.</param>
    /// <param name="maxCount">Maximum number of transactions to keep.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Number of transactions deleted.</returns>
    Task<int> PurgeOldTransactionsAsync(
        TimeSpan? maxAge = null,
        int? maxCount = null,
        CancellationToken cancellationToken = default);
}
