namespace BootSentry.Core.Enums;

/// <summary>
/// Authenticode signature verification status.
/// </summary>
public enum SignatureStatus
{
    /// <summary>File is signed with a trusted certificate</summary>
    SignedTrusted,

    /// <summary>File is signed but certificate is not trusted</summary>
    SignedUntrusted,

    /// <summary>File is not signed</summary>
    Unsigned,

    /// <summary>Signature status could not be determined (file inaccessible, etc.)</summary>
    Unknown
}
