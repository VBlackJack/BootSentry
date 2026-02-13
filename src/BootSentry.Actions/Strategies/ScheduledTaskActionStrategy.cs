using Microsoft.Extensions.Logging;
using Microsoft.Win32.TaskScheduler;
using BootSentry.Core.Enums;
using BootSentry.Core.Interfaces;
using BootSentry.Core.Models;
using Task = System.Threading.Tasks.Task;

namespace BootSentry.Actions.Strategies;

/// <summary>
/// Action strategy for Windows Scheduled Tasks.
/// Uses Task Scheduler API to enable/disable tasks.
/// </summary>
public sealed class ScheduledTaskActionStrategy : IActionStrategy
{
    private readonly ILogger<ScheduledTaskActionStrategy> _logger;

    public ScheduledTaskActionStrategy(ILogger<ScheduledTaskActionStrategy> logger)
    {
        _logger = logger;
    }

    public EntryType EntryType => EntryType.ScheduledTask;
    public bool CanDisable => true;
    public bool CanDelete => false; // Deleting tasks is risky, disable only

    public bool RequiresAdmin(StartupEntry entry, ActionType action)
    {
        // Most task operations require admin
        return entry.Scope == EntryScope.Machine;
    }

    public async Task<ActionResult> DisableAsync(StartupEntry entry, CancellationToken cancellationToken = default)
    {
        if (entry.Type != EntryType.ScheduledTask)
        {
            return ActionResult.Fail("Invalid entry type for this strategy", "ERR_INVALID_TYPE");
        }

        return await Task.Run(() =>
        {
            try
            {
                using var ts = new TaskService();
                var task = ts.GetTask(entry.SourcePath);

                if (task == null)
                {
                    return ActionResult.Fail($"Task not found: {entry.SourcePath}", "ERR_TASK_NOT_FOUND");
                }

                if (!task.Enabled)
                {
                    _logger.LogInformation("Task already disabled: {Name}", entry.DisplayName);
                    return ActionResult.Ok(entry);
                }

                // Disable the task
                task.Definition.Settings.Enabled = false;
                task.RegisterChanges();

                _logger.LogInformation("Disabled scheduled task: {Name}", entry.DisplayName);

                entry.Status = EntryStatus.Disabled;
                return ActionResult.Ok(entry);
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogError(ex, "Access denied disabling scheduled task");
                return ActionResult.Fail("Access denied. Administrator privileges required.", "ERR_ACCESS_DENIED");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error disabling scheduled task");
                return ActionResult.Fail(ex.Message, "ERR_DISABLE_FAILED");
            }
        }, cancellationToken).ConfigureAwait(false);
    }

    public async Task<ActionResult> EnableAsync(StartupEntry entry, CancellationToken cancellationToken = default)
    {
        if (entry.Type != EntryType.ScheduledTask)
        {
            return ActionResult.Fail("Invalid entry type for this strategy", "ERR_INVALID_TYPE");
        }

        return await Task.Run(() =>
        {
            try
            {
                using var ts = new TaskService();
                var task = ts.GetTask(entry.SourcePath);

                if (task == null)
                {
                    return ActionResult.Fail($"Task not found: {entry.SourcePath}", "ERR_TASK_NOT_FOUND");
                }

                if (task.Enabled)
                {
                    _logger.LogInformation("Task already enabled: {Name}", entry.DisplayName);
                    return ActionResult.Ok(entry);
                }

                // Enable the task
                task.Definition.Settings.Enabled = true;
                task.RegisterChanges();

                _logger.LogInformation("Enabled scheduled task: {Name}", entry.DisplayName);

                entry.Status = EntryStatus.Enabled;
                return ActionResult.Ok(entry);
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogError(ex, "Access denied enabling scheduled task");
                return ActionResult.Fail("Access denied. Administrator privileges required.", "ERR_ACCESS_DENIED");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error enabling scheduled task");
                return ActionResult.Fail(ex.Message, "ERR_ENABLE_FAILED");
            }
        }, cancellationToken).ConfigureAwait(false);
    }

    public Task<ActionResult> DeleteAsync(StartupEntry entry, CancellationToken cancellationToken = default)
    {
        // We don't support deleting scheduled tasks for safety
        return Task.FromResult(ActionResult.Fail(
            "Deleting scheduled tasks is not supported. Use disable instead.",
            "ERR_NOT_SUPPORTED"));
    }
}
