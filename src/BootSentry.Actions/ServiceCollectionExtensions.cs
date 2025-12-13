using BootSentry.Actions.Strategies;
using BootSentry.Core.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace BootSentry.Actions;

/// <summary>
/// Extension methods for registering action services.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds BootSentry action services to the service collection.
    /// </summary>
    public static IServiceCollection AddBootSentryActions(this IServiceCollection services)
    {
        // Register strategies
        services.AddSingleton<IActionStrategy, RegistryActionStrategy>();
        services.AddSingleton<IActionStrategy, StartupFolderActionStrategy>();
        services.AddSingleton<IActionStrategy, ScheduledTaskActionStrategy>();
        services.AddSingleton<IActionStrategy, ServiceActionStrategy>();

        // Register executor
        services.AddSingleton<ActionExecutor>();

        return services;
    }
}
