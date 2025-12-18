using BootSentry.Core.Interfaces;
using BootSentry.Security.Services;
using Microsoft.Extensions.DependencyInjection;

namespace BootSentry.Security;

/// <summary>
/// Extension methods for registering security services.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds BootSentry security services to the service collection.
    /// </summary>
    public static IServiceCollection AddBootSentrySecurity(this IServiceCollection services)
    {
        services.AddSingleton<ISignatureVerifier, SignatureVerifier>();
        services.AddSingleton<IHashCalculator, HashCalculator>();
        services.AddSingleton<IMalwareScanner, AmsiMalwareScanner>();
        return services;
    }
}
