namespace BootSentry.Backup.Models;

/// <summary>
/// Serializable registry value backup payload.
/// Stores typed value fields to preserve original registry semantics.
/// </summary>
public sealed class RegistryValueBackupData
{
    public required string KeyPath { get; init; }
    public required string ValueName { get; init; }
    public required string ValueKind { get; init; }
    public DateTime BackupTime { get; init; }

    public string? StringValue { get; init; }
    public int? DWordValue { get; init; }
    public long? QWordValue { get; init; }
    public string[]? MultiStringValue { get; init; }
    public string? BinaryBase64 { get; init; }
}
