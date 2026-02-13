using System.Text.Json.Serialization;

namespace BootSentry.Core.Models.Integrations;

public class VirusTotalResponse
{
    [JsonPropertyName("data")]
    public VirusTotalData? Data { get; set; }
}

public class VirusTotalData
{
    [JsonPropertyName("attributes")]
    public VirusTotalAttributes? Attributes { get; set; }
    
    [JsonPropertyName("id")]
    public string? Id { get; set; }
    
    [JsonPropertyName("type")]
    public string? Type { get; set; }
}

public class VirusTotalAttributes
{
    [JsonPropertyName("last_analysis_stats")]
    public AnalysisStats? LastAnalysisStats { get; set; }
    
    [JsonPropertyName("last_analysis_date")]
    public long LastAnalysisDate { get; set; }
    
    [JsonPropertyName("meaningful_name")]
    public string? MeaningfulName { get; set; }
    
    [JsonPropertyName("reputation")]
    public int Reputation { get; set; }
}

public class AnalysisStats
{
    [JsonPropertyName("harmless")]
    public int Harmless { get; set; }
    
    [JsonPropertyName("type-unsupported")]
    public int TypeUnsupported { get; set; }
    
    [JsonPropertyName("suspicious")]
    public int Suspicious { get; set; }
    
    [JsonPropertyName("confirmed-timeout")]
    public int ConfirmedTimeout { get; set; }
    
    [JsonPropertyName("timeout")]
    public int Timeout { get; set; }
    
    [JsonPropertyName("failure")]
    public int Failure { get; set; }
    
    [JsonPropertyName("malicious")]
    public int Malicious { get; set; }
    
    [JsonPropertyName("undetected")]
    public int Undetected { get; set; }
}
