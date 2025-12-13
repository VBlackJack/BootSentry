using Microsoft.Extensions.Logging;
using BootSentry.Core.Enums;
using BootSentry.Core.Interfaces;
using BootSentry.Core.Models;

namespace BootSentry.Actions;

/// <summary>
/// Coordinates action execution across different entry types.
/// </summary>
public sealed class ActionExecutor
{
    private readonly ILogger<ActionExecutor> _logger;
    private readonly IReadOnlyDictionary<EntryType, IActionStrategy> _strategies;

    public ActionExecutor(ILogger<ActionExecutor> logger, IEnumerable<IActionStrategy> strategies)
    {
        _logger = logger;
        _strategies = strategies.ToDictionary(s => s.EntryType);
    }

    /// <summary>
    /// Gets the strategy for a given entry type.
    /// </summary>
    public IActionStrategy? GetStrategy(EntryType entryType)
    {
        _strategies.TryGetValue(entryType, out var strategy);
        return strategy;
    }

    /// <summary>
    /// Checks if an action can be performed on an entry.
    /// </summary>
    public bool CanPerformAction(StartupEntry entry, ActionType action)
    {
        if (!_strategies.TryGetValue(entry.Type, out var strategy))
            return false;

        return action switch
        {
            ActionType.Disable => strategy.CanDisable,
            ActionType.Enable => strategy.CanDisable, // Enable uses same capability as disable
            ActionType.Delete => strategy.CanDelete,
            _ => false
        };
    }

    /// <summary>
    /// Checks if an action requires administrator privileges.
    /// </summary>
    public bool RequiresAdmin(StartupEntry entry, ActionType action)
    {
        if (!_strategies.TryGetValue(entry.Type, out var strategy))
            return true; // Assume admin required if no strategy

        return strategy.RequiresAdmin(entry, action);
    }

    /// <summary>
    /// Executes an action on an entry.
    /// </summary>
    public async Task<ActionResult> ExecuteAsync(
        StartupEntry entry,
        ActionType action,
        CancellationToken cancellationToken = default)
    {
        if (!_strategies.TryGetValue(entry.Type, out var strategy))
        {
            return ActionResult.Fail($"No strategy available for entry type: {entry.Type}", "ERR_NO_STRATEGY");
        }

        _logger.LogInformation("Executing {Action} on {Entry} ({Type})", action, entry.DisplayName, entry.Type);

        return action switch
        {
            ActionType.Disable => await strategy.DisableAsync(entry, cancellationToken),
            ActionType.Enable => await strategy.EnableAsync(entry, cancellationToken),
            ActionType.Delete => await strategy.DeleteAsync(entry, cancellationToken),
            _ => ActionResult.Fail($"Unknown action type: {action}", "ERR_UNKNOWN_ACTION")
        };
    }

    /// <summary>
    /// Disables an entry.
    /// </summary>
    public Task<ActionResult> DisableAsync(StartupEntry entry, CancellationToken cancellationToken = default)
        => ExecuteAsync(entry, ActionType.Disable, cancellationToken);

    /// <summary>
    /// Enables an entry.
    /// </summary>
    public Task<ActionResult> EnableAsync(StartupEntry entry, CancellationToken cancellationToken = default)
        => ExecuteAsync(entry, ActionType.Enable, cancellationToken);

    /// <summary>
    /// Deletes an entry.
    /// </summary>
    public Task<ActionResult> DeleteAsync(StartupEntry entry, CancellationToken cancellationToken = default)
        => ExecuteAsync(entry, ActionType.Delete, cancellationToken);
}
