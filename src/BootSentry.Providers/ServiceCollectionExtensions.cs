using BootSentry.Core.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace BootSentry.Providers;

/// <summary>
/// Extension methods for registering provider services.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds all BootSentry providers to the service collection.
    /// </summary>
    public static IServiceCollection AddBootSentryProviders(this IServiceCollection services)
    {
        services.AddSingleton<IStartupProvider, RegistryRunProvider>();
        services.AddSingleton<IStartupProvider, StartupFolderProvider>();
        services.AddSingleton<IStartupProvider, ScheduledTaskProvider>();
        services.AddSingleton<IStartupProvider, ServiceProvider>();
        services.AddSingleton<IStartupProvider, WinlogonProvider>();

        return services;
    }

    /// <summary>
    /// Adds only the basic (MVP) providers to the service collection.
    /// </summary>
    public static IServiceCollection AddBootSentryBasicProviders(this IServiceCollection services)
    {
        services.AddSingleton<IStartupProvider, RegistryRunProvider>();
        services.AddSingleton<IStartupProvider, StartupFolderProvider>();

        return services;
    }
}
