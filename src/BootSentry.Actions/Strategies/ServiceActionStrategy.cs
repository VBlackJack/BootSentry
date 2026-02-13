using System.ServiceProcess;
using Microsoft.Extensions.Logging;
using Microsoft.Win32;
using BootSentry.Core.Enums;
using BootSentry.Core.Interfaces;
using BootSentry.Core.Models;

namespace BootSentry.Actions.Strategies;

/// <summary>
/// Action strategy for Windows Services.
/// Changes service startup type between Automatic and Disabled.
/// </summary>
public sealed class ServiceActionStrategy : IActionStrategy
{
    private readonly ILogger<ServiceActionStrategy> _logger;
    private readonly ITransactionManager _transactionManager;

    public ServiceActionStrategy(ILogger<ServiceActionStrategy> logger, ITransactionManager transactionManager)
    {
        _logger = logger;
        _transactionManager = transactionManager;
    }

    public EntryType EntryType => EntryType.Service;
    public bool CanDisable => true;
    public bool CanDelete => false; // Never delete services

    public bool RequiresAdmin(StartupEntry entry, ActionType action)
    {
        return true; // Service modifications always require admin
    }

    public async Task<ActionResult> DisableAsync(StartupEntry entry, CancellationToken cancellationToken = default)
    {
        if (entry.Type != EntryType.Service)
        {
            return ActionResult.Fail("Invalid entry type for this strategy", "ERR_INVALID_TYPE");
        }

        if (entry.IsProtected)
        {
            return ActionResult.Fail(
                $"Cannot disable protected service: {entry.ProtectionReason ?? "System critical"}",
                "ERR_PROTECTED");
        }

        var serviceName = entry.SourceName;
        if (string.IsNullOrEmpty(serviceName))
        {
            return ActionResult.Fail("Service name not found", "ERR_NO_SERVICE_NAME");
        }

        try
        {
            // Get current start type for backup
            var currentStartConfig = GetServiceStartConfiguration(serviceName);
            if (currentStartConfig == null)
            {
                return ActionResult.Fail($"Service not found: {serviceName}", "ERR_SERVICE_NOT_FOUND");
            }

            // Create backup transaction (stores original start type)
            var transaction = await _transactionManager.CreateTransactionAsync(entry, ActionType.Disable, cancellationToken).ConfigureAwait(false);

            try
            {
                // Change start type to Disabled
                SetServiceStartConfiguration(serviceName, ServiceStartMode.Disabled);

                // Commit transaction
                await _transactionManager.CommitAsync(transaction.Id, cancellationToken).ConfigureAwait(false);

                var previousStartType = ToDisplayStartType(currentStartConfig.Value);
                _logger.LogInformation("Disabled service: {Name} (was {StartType})", entry.DisplayName, previousStartType);

                entry.PreviousServiceStartType = previousStartType;
                entry.Status = EntryStatus.Disabled;
                entry.ServiceStartType = "Disabled";
                return ActionResult.Ok(entry, transaction.Id);
            }
            catch
            {
                await _transactionManager.RollbackAsync(transaction.Id, cancellationToken).ConfigureAwait(false);
                throw;
            }
        }
        catch (UnauthorizedAccessException ex)
        {
            _logger.LogError(ex, "Access denied disabling service");
            return ActionResult.Fail("Access denied. Administrator privileges required.", "ERR_ACCESS_DENIED");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error disabling service");
            return ActionResult.Fail(ex.Message, "ERR_DISABLE_FAILED");
        }
    }

    public async Task<ActionResult> EnableAsync(StartupEntry entry, CancellationToken cancellationToken = default)
    {
        if (entry.Type != EntryType.Service)
        {
            return ActionResult.Fail("Invalid entry type for this strategy", "ERR_INVALID_TYPE");
        }

        var serviceName = entry.SourceName;
        if (string.IsNullOrEmpty(serviceName))
        {
            return ActionResult.Fail("Service name not found", "ERR_NO_SERVICE_NAME");
        }

        try
        {
            var targetStartType = entry.PreviousServiceStartType ?? "Automatic";
            var targetConfig = ParseStartConfiguration(targetStartType);

            // Restore previous start type when available.
            SetServiceStartConfiguration(serviceName, targetConfig.Mode, targetConfig.IsDelayedAutomatic);

            var appliedStartType = ToDisplayStartType(targetConfig);
            _logger.LogInformation("Enabled service: {Name} (start type: {StartType})", entry.DisplayName, appliedStartType);

            entry.Status = EntryStatus.Enabled;
            entry.ServiceStartType = appliedStartType;
            entry.PreviousServiceStartType = null;
            return ActionResult.Ok(entry);
        }
        catch (UnauthorizedAccessException ex)
        {
            _logger.LogError(ex, "Access denied enabling service");
            return ActionResult.Fail("Access denied. Administrator privileges required.", "ERR_ACCESS_DENIED");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error enabling service");
            return ActionResult.Fail(ex.Message, "ERR_ENABLE_FAILED");
        }
    }

    public Task<ActionResult> DeleteAsync(StartupEntry entry, CancellationToken cancellationToken = default)
    {
        // We don't support deleting services for safety
        return Task.FromResult(ActionResult.Fail(
            "Deleting services is not supported. Use disable instead.",
            "ERR_NOT_SUPPORTED"));
    }

    private static ServiceStartConfiguration? GetServiceStartConfiguration(string serviceName)
    {
        try
        {
            using var key = Registry.LocalMachine.OpenSubKey($@"SYSTEM\CurrentControlSet\Services\{serviceName}");
            if (key == null)
                return null;

            var start = key.GetValue("Start");
            if (start == null)
                return null;

            ServiceStartMode? startMode = Convert.ToInt32(start) switch
            {
                0 => ServiceStartMode.Boot,
                1 => ServiceStartMode.System,
                2 => ServiceStartMode.Automatic,
                3 => ServiceStartMode.Manual,
                4 => ServiceStartMode.Disabled,
                _ => null
            };

            if (startMode == null)
                return null;

            var isDelayed = false;
            if (startMode == ServiceStartMode.Automatic)
            {
                var delayed = key.GetValue("DelayedAutostart");
                isDelayed = delayed != null && Convert.ToInt32(delayed) == 1;
            }

            return new ServiceStartConfiguration(startMode.Value, isDelayed);
        }
        catch
        {
            return null;
        }
    }

    private static void SetServiceStartConfiguration(
        string serviceName,
        ServiceStartMode startMode,
        bool delayedAutomatic = false)
    {
        using var key = Registry.LocalMachine.OpenSubKey(
            $@"SYSTEM\CurrentControlSet\Services\{serviceName}",
            writable: true);

        if (key == null)
        {
            throw new InvalidOperationException($"Service not found: {serviceName}");
        }

        int startValue = startMode switch
        {
            ServiceStartMode.Boot => 0,
            ServiceStartMode.System => 1,
            ServiceStartMode.Automatic => 2,
            ServiceStartMode.Manual => 3,
            ServiceStartMode.Disabled => 4,
            _ => throw new ArgumentException($"Invalid start mode: {startMode}")
        };

        key.SetValue("Start", startValue, RegistryValueKind.DWord);

        if (startMode == ServiceStartMode.Automatic)
        {
            key.SetValue("DelayedAutostart", delayedAutomatic ? 1 : 0, RegistryValueKind.DWord);
        }
        else if (key.GetValueNames().Contains("DelayedAutostart", StringComparer.OrdinalIgnoreCase))
        {
            key.DeleteValue("DelayedAutostart", false);
        }
    }

    private static ServiceStartConfiguration ParseStartConfiguration(string value)
    {
        if (value.Equals("Automatic (Delayed)", StringComparison.OrdinalIgnoreCase))
            return new ServiceStartConfiguration(ServiceStartMode.Automatic, IsDelayedAutomatic: true);

        if (value.Equals("Automatic", StringComparison.OrdinalIgnoreCase)
            || value.Equals("Auto", StringComparison.OrdinalIgnoreCase))
            return new ServiceStartConfiguration(ServiceStartMode.Automatic, IsDelayedAutomatic: false);

        if (value.Equals("Manual", StringComparison.OrdinalIgnoreCase))
            return new ServiceStartConfiguration(ServiceStartMode.Manual, IsDelayedAutomatic: false);

        if (value.Equals("Disabled", StringComparison.OrdinalIgnoreCase))
            return new ServiceStartConfiguration(ServiceStartMode.Disabled, IsDelayedAutomatic: false);

        if (value.Equals("Boot", StringComparison.OrdinalIgnoreCase))
            return new ServiceStartConfiguration(ServiceStartMode.Boot, IsDelayedAutomatic: false);

        if (value.Equals("System", StringComparison.OrdinalIgnoreCase))
            return new ServiceStartConfiguration(ServiceStartMode.System, IsDelayedAutomatic: false);

        return new ServiceStartConfiguration(ServiceStartMode.Automatic, IsDelayedAutomatic: false);
    }

    private static string ToDisplayStartType(ServiceStartConfiguration configuration)
    {
        if (configuration.Mode == ServiceStartMode.Automatic && configuration.IsDelayedAutomatic)
            return "Automatic (Delayed)";

        return configuration.Mode.ToString();
    }

    private readonly record struct ServiceStartConfiguration(ServiceStartMode Mode, bool IsDelayedAutomatic);
}
