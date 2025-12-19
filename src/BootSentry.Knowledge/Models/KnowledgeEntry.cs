namespace BootSentry.Knowledge.Models;

/// <summary>
/// Represents a knowledge base entry for a startup item.
/// </summary>
public class KnowledgeEntry
{
    /// <summary>
    /// Unique identifier.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Display name of the program/service (e.g., "Steam Client Bootstrapper").
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// Alternative names or process names (comma-separated).
    /// </summary>
    public string? Aliases { get; set; }

    /// <summary>
    /// Publisher/Company name (e.g., "Valve Corporation").
    /// </summary>
    public string? Publisher { get; set; }

    /// <summary>
    /// Common executable names (comma-separated, e.g., "steam.exe,steamwebhelper.exe").
    /// </summary>
    public string? ExecutableNames { get; set; }

    /// <summary>
    /// Category of the entry.
    /// </summary>
    public KnowledgeCategory Category { get; set; }

    /// <summary>
    /// Safety recommendation.
    /// </summary>
    public SafetyLevel SafetyLevel { get; set; }

    /// <summary>
    /// Short description in French (1-2 sentences).
    /// </summary>
    public required string ShortDescription { get; set; }

    /// <summary>
    /// Short description in English.
    /// </summary>
    public string? ShortDescriptionEn { get; set; }

    /// <summary>
    /// Full detailed description in French (wiki-style).
    /// </summary>
    public string? FullDescription { get; set; }

    /// <summary>
    /// Full detailed description in English.
    /// </summary>
    public string? FullDescriptionEn { get; set; }

    /// <summary>
    /// What happens if disabled (French).
    /// </summary>
    public string? DisableImpact { get; set; }

    /// <summary>
    /// What happens if disabled (English).
    /// </summary>
    public string? DisableImpactEn { get; set; }

    /// <summary>
    /// Performance impact (CPU/RAM/Startup time) - French.
    /// </summary>
    public string? PerformanceImpact { get; set; }

    /// <summary>
    /// Performance impact (English).
    /// </summary>
    public string? PerformanceImpactEn { get; set; }

    /// <summary>
    /// Recommendation text in French.
    /// </summary>
    public string? Recommendation { get; set; }

    /// <summary>
    /// Recommendation text in English.
    /// </summary>
    public string? RecommendationEn { get; set; }

    /// <summary>
    /// Gets the localized short description.
    /// </summary>
    public string GetShortDescription(string language) =>
        language == "en" && !string.IsNullOrEmpty(ShortDescriptionEn) ? ShortDescriptionEn : ShortDescription;

    /// <summary>
    /// Gets the localized full description.
    /// </summary>
    public string? GetFullDescription(string language) =>
        language == "en" && !string.IsNullOrEmpty(FullDescriptionEn) ? FullDescriptionEn : FullDescription;

    /// <summary>
    /// Gets the localized disable impact.
    /// </summary>
    public string? GetDisableImpact(string language) =>
        language == "en" && !string.IsNullOrEmpty(DisableImpactEn) ? DisableImpactEn : DisableImpact;

    /// <summary>
    /// Gets the localized performance impact.
    /// </summary>
    public string? GetPerformanceImpact(string language) =>
        language == "en" && !string.IsNullOrEmpty(PerformanceImpactEn) ? PerformanceImpactEn : PerformanceImpact;

    /// <summary>
    /// Gets the localized recommendation.
    /// </summary>
    public string? GetRecommendation(string language) =>
        language == "en" && !string.IsNullOrEmpty(RecommendationEn) ? RecommendationEn : Recommendation;

    /// <summary>
    /// URL for more information.
    /// </summary>
    public string? InfoUrl { get; set; }

    /// <summary>
    /// Tags for search (comma-separated).
    /// </summary>
    public string? Tags { get; set; }

    /// <summary>
    /// When this entry was last updated.
    /// </summary>
    public DateTime LastUpdated { get; set; }
}

/// <summary>
/// Category of a knowledge entry.
/// </summary>
public enum KnowledgeCategory
{
    /// <summary>Windows system component</summary>
    WindowsSystem,

    /// <summary>Windows security feature</summary>
    WindowsSecurity,

    /// <summary>Hardware driver or utility</summary>
    Hardware,

    /// <summary>Antivirus/Security software</summary>
    Security,

    /// <summary>Gaming platform or launcher</summary>
    Gaming,

    /// <summary>Productivity software</summary>
    Productivity,

    /// <summary>Communication software</summary>
    Communication,

    /// <summary>Cloud storage/sync</summary>
    CloudStorage,

    /// <summary>Media/Entertainment</summary>
    Media,

    /// <summary>Browser-related</summary>
    Browser,

    /// <summary>System utility</summary>
    Utility,

    /// <summary>Manufacturer bloatware</summary>
    Bloatware,

    /// <summary>Potentially unwanted program</summary>
    PUP,

    /// <summary>Known malware</summary>
    Malware,

    /// <summary>Other/Unknown</summary>
    Other
}

/// <summary>
/// Safety level for disabling an entry.
/// </summary>
public enum SafetyLevel
{
    /// <summary>Critical - Never disable, required for system stability</summary>
    Critical,

    /// <summary>Important - Provides important functionality, disable with caution</summary>
    Important,

    /// <summary>Safe - Can be disabled if not needed</summary>
    Safe,

    /// <summary>Recommended to disable - Bloatware or unnecessary</summary>
    RecommendedDisable,

    /// <summary>Should be removed - PUP or malware</summary>
    ShouldRemove
}
