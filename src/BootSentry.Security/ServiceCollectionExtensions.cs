using BootSentry.Core.Interfaces;
using BootSentry.Security.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

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

        // Register AMSI scanner with graceful fallback if AMSI is not available
        services.AddSingleton<IMalwareScanner>(sp =>
        {
            var logger = sp.GetRequiredService<ILogger<AmsiMalwareScanner>>();
            try
            {
                return new AmsiMalwareScanner(logger);
            }
            catch (InvalidOperationException ex)
            {
                logger.LogWarning(ex, "AMSI is not available on this system. Malware scanning will be disabled.");
                return new NullMalwareScanner(logger);
            }
        });

        return services;
    }
}
