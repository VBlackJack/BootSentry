using BootSentry.Core.Enums;

namespace BootSentry.Core.Interfaces;

/// <summary>
/// Result of signature verification.
/// </summary>
public record SignatureInfo
{
    /// <summary>Overall signature status.</summary>
    public required SignatureStatus Status { get; init; }

    /// <summary>Signer name (subject) from the certificate.</summary>
    public string? SignerName { get; init; }

    /// <summary>Issuer of the certificate.</summary>
    public string? Issuer { get; init; }

    /// <summary>Certificate serial number.</summary>
    public string? SerialNumber { get; init; }

    /// <summary>Certificate thumbprint (SHA-1).</summary>
    public string? Thumbprint { get; init; }

    /// <summary>Certificate validity start date.</summary>
    public DateTime? ValidFrom { get; init; }

    /// <summary>Certificate validity end date.</summary>
    public DateTime? ValidTo { get; init; }

    /// <summary>Whether the certificate chain is valid.</summary>
    public bool? ChainIsValid { get; init; }

    /// <summary>Error message if verification failed.</summary>
    public string? ErrorMessage { get; init; }
}

/// <summary>
/// Interface for verifying Authenticode signatures.
/// </summary>
public interface ISignatureVerifier
{
    /// <summary>
    /// Verifies the Authenticode signature of a file.
    /// </summary>
    /// <param name="filePath">Path to the file to verify.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Signature verification result.</returns>
    Task<SignatureInfo> VerifyAsync(string filePath, CancellationToken cancellationToken = default);

    /// <summary>
    /// Quickly checks if a file is signed (without full chain validation).
    /// </summary>
    /// <param name="filePath">Path to the file to check.</param>
    /// <returns>True if the file has a signature (valid or not).</returns>
    bool IsSigned(string filePath);
}
