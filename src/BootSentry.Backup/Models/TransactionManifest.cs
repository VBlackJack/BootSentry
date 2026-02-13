using System.Text.Json.Serialization;
using BootSentry.Core.Enums;

namespace BootSentry.Backup.Models;

/// <summary>
/// Serializable manifest for a backup transaction.
/// </summary>
public sealed class TransactionManifest
{
    [JsonPropertyName("id")]
    public required string Id { get; init; }

    [JsonPropertyName("timestamp")]
    public required DateTime Timestamp { get; init; }

    [JsonPropertyName("user")]
    public required string User { get; init; }

    [JsonPropertyName("userSid")]
    public string? UserSid { get; init; }

    [JsonPropertyName("machineName")]
    public string? MachineName { get; init; }

    [JsonPropertyName("actionType")]
    public required ActionType ActionType { get; init; }

    [JsonPropertyName("entryId")]
    public required string EntryId { get; init; }

    [JsonPropertyName("entryDisplayName")]
    public required string EntryDisplayName { get; init; }

    [JsonPropertyName("entryType")]
    public required EntryType EntryType { get; init; }

    [JsonPropertyName("entryScope")]
    public required EntryScope EntryScope { get; init; }

    [JsonPropertyName("sourcePath")]
    public required string SourcePath { get; init; }

    [JsonPropertyName("sourceName")]
    public string? SourceName { get; init; }

    [JsonPropertyName("originalValue")]
    public string? OriginalValue { get; init; }

    [JsonPropertyName("originalStatus")]
    public EntryStatus OriginalStatus { get; init; }

    [JsonPropertyName("payloadFiles")]
    public List<string> PayloadFiles { get; init; } = [];

    [JsonPropertyName("canRestore")]
    public bool CanRestore { get; init; } = true;

    [JsonPropertyName("notes")]
    public string? Notes { get; init; }

    [JsonPropertyName("status")]
    public TransactionStatus Status { get; set; } = TransactionStatus.Pending;

    [JsonPropertyName("completedAt")]
    public DateTime? CompletedAt { get; set; }

    [JsonPropertyName("errorMessage")]
    public string? ErrorMessage { get; set; }
}

/// <summary>
/// Status of a backup transaction.
/// </summary>
public enum TransactionStatus
{
    Pending,
    Committed,
    RolledBack,
    Failed
}
