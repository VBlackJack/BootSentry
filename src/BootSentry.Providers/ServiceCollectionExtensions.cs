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
        // Core providers (always included)
        services.AddSingleton<IStartupProvider, RegistryRunProvider>();
        services.AddSingleton<IStartupProvider, StartupFolderProvider>();
        services.AddSingleton<IStartupProvider, ScheduledTaskProvider>();
        services.AddSingleton<IStartupProvider, ServiceProvider>();
        services.AddSingleton<IStartupProvider, WinlogonProvider>();

        // Advanced providers
        services.AddSingleton<IStartupProvider, IFEOProvider>();
        services.AddSingleton<IStartupProvider, DriverProvider>();
        services.AddSingleton<IStartupProvider, RegistryPoliciesProvider>();
        services.AddSingleton<IStartupProvider, BrowserExtensionProvider>();

        // Expert mode providers
        services.AddSingleton<IStartupProvider, ShellExtensionProvider>();
        services.AddSingleton<IStartupProvider, BHOProvider>();
        services.AddSingleton<IStartupProvider, PrintMonitorProvider>();
        services.AddSingleton<IStartupProvider, SessionManagerProvider>();
        services.AddSingleton<IStartupProvider, AppInitDllsProvider>();
        services.AddSingleton<IStartupProvider, WinsockLSPProvider>();

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
