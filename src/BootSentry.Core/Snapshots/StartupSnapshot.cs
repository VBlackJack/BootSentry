using System.Text.Json.Serialization;
using BootSentry.Core.Models;

namespace BootSentry.Core.Snapshots;

/// <summary>
/// Represents a point-in-time snapshot of all startup entries.
/// </summary>
public class StartupSnapshot
{
    public string Id { get; set; } = Guid.NewGuid().ToString("N");
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string MachineName { get; set; } = Environment.MachineName;
    public string UserName { get; set; } = Environment.UserName;
    
    public List<StartupEntry> Entries { get; set; } = new();

    [JsonIgnore]
    public int EntryCount => Entries.Count;
}
