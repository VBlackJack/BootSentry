using BootSentry.Core.Enums;
using BootSentry.Core.Models;

namespace BootSentry.Core.Interfaces;

/// <summary>
/// Result of an action operation.
/// </summary>
public record ActionResult
{
    /// <summary>Whether the action was successful.</summary>
    public required bool Success { get; init; }

    /// <summary>Error message if the action failed.</summary>
    public string? ErrorMessage { get; init; }

    /// <summary>Error code for programmatic handling.</summary>
    public string? ErrorCode { get; init; }

    /// <summary>The entry after the action (with updated status).</summary>
    public StartupEntry? UpdatedEntry { get; init; }

    /// <summary>ID of the backup transaction created (if any).</summary>
    public string? TransactionId { get; init; }

    public static ActionResult Ok(StartupEntry? updatedEntry = null, string? transactionId = null)
        => new() { Success = true, UpdatedEntry = updatedEntry, TransactionId = transactionId };

    public static ActionResult Fail(string message, string? code = null)
        => new() { Success = false, ErrorMessage = message, ErrorCode = code };
}

/// <summary>
/// Interface for strategies that perform actions on startup entries.
/// Each strategy handles a specific entry type.
/// </summary>
public interface IActionStrategy
{
    /// <summary>
    /// The entry type this strategy handles.
    /// </summary>
    EntryType EntryType { get; }

    /// <summary>
    /// Whether this strategy can disable entries of its type.
    /// </summary>
    bool CanDisable { get; }

    /// <summary>
    /// Whether this strategy can delete entries of its type.
    /// </summary>
    bool CanDelete { get; }

    /// <summary>
    /// Disables the specified entry (reversible operation).
    /// </summary>
    /// <param name="entry">The entry to disable.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Result of the operation.</returns>
    Task<ActionResult> DisableAsync(StartupEntry entry, CancellationToken cancellationToken = default);

    /// <summary>
    /// Re-enables a previously disabled entry.
    /// </summary>
    /// <param name="entry">The entry to enable.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Result of the operation.</returns>
    Task<ActionResult> EnableAsync(StartupEntry entry, CancellationToken cancellationToken = default);

    /// <summary>
    /// Permanently deletes the specified entry.
    /// </summary>
    /// <param name="entry">The entry to delete.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Result of the operation.</returns>
    Task<ActionResult> DeleteAsync(StartupEntry entry, CancellationToken cancellationToken = default);

    /// <summary>
    /// Checks if the action requires admin privileges for the given entry.
    /// </summary>
    /// <param name="entry">The entry to check.</param>
    /// <param name="action">The action to perform.</param>
    /// <returns>True if admin privileges are required.</returns>
    bool RequiresAdmin(StartupEntry entry, ActionType action);
}
