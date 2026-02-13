using BootSentry.Core.Enums;

namespace BootSentry.Core.Models;

/// <summary>
/// Risk assessment result for a startup entry.
/// </summary>
public class RiskAssessment
{
    public string EntryId { get; set; } = string.Empty;
    public RiskLevel OriginalRiskLevel { get; set; }
    public RiskLevel RecommendedRiskLevel { get; set; }
    public int TotalScore { get; set; }
    public IReadOnlyList<RiskFactor> Factors { get; set; } = Array.Empty<RiskFactor>();
}

/// <summary>
/// Individual risk factor contributing to the assessment.
/// </summary>
public class RiskFactor
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int Score { get; set; }
    public RiskFactorType Type { get; set; }
}

/// <summary>
/// Type of risk factor.
/// </summary>
public enum RiskFactorType
{
    Positive,   // Reduces risk
    Neutral,    // No impact
    Warning,    // Minor concern
    Negative    // Increases risk
}
