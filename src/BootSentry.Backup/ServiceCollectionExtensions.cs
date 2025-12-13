using BootSentry.Core.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace BootSentry.Backup;

/// <summary>
/// Extension methods for registering backup services.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds BootSentry backup services to the service collection.
    /// </summary>
    public static IServiceCollection AddBootSentryBackup(this IServiceCollection services)
    {
        services.AddSingleton<BackupStorage>();
        services.AddSingleton<ITransactionManager, TransactionManager>();
        return services;
    }
}
