using BootSentry.Core.Enums;
using BootSentry.Core.Models;

namespace BootSentry.Core.Interfaces;

/// <summary>
/// Interface for providers that scan a specific startup source.
/// Each provider is responsible for one type of source (registry, folder, tasks, etc.).
/// </summary>
public interface IStartupProvider
{
    /// <summary>
    /// The type of entries this provider handles.
    /// </summary>
    EntryType EntryType { get; }

    /// <summary>
    /// Display name of this provider for logging/UI.
    /// </summary>
    string DisplayName { get; }

    /// <summary>
    /// Whether this provider requires administrative privileges to read.
    /// </summary>
    bool RequiresAdminToRead { get; }

    /// <summary>
    /// Whether this provider requires administrative privileges to modify.
    /// </summary>
    bool RequiresAdminToModify { get; }

    /// <summary>
    /// Scans and returns all startup entries from this source.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token for async operation.</param>
    /// <returns>Collection of startup entries found.</returns>
    Task<IReadOnlyCollection<StartupEntry>> ScanAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Checks if this provider is available on the current system.
    /// </summary>
    bool IsAvailable();
}
