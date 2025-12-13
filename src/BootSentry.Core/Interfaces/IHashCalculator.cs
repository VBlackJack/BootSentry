namespace BootSentry.Core.Interfaces;

/// <summary>
/// Interface for calculating file hashes.
/// </summary>
public interface IHashCalculator
{
    /// <summary>
    /// Calculates the SHA-256 hash of a file.
    /// </summary>
    /// <param name="filePath">Path to the file.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>SHA-256 hash as lowercase hex string.</returns>
    Task<string> CalculateSha256Async(string filePath, CancellationToken cancellationToken = default);

    /// <summary>
    /// Calculates the MD5 hash of a file (for compatibility with some services).
    /// </summary>
    /// <param name="filePath">Path to the file.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>MD5 hash as lowercase hex string.</returns>
    Task<string> CalculateMd5Async(string filePath, CancellationToken cancellationToken = default);
}
