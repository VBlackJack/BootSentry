using BootSentry.Knowledge.Models;
using BootSentry.UI.Resources;

namespace BootSentry.UI.Models;

/// <summary>
/// Wrapper around KnowledgeEntry that provides localized text based on current language.
/// </summary>
public class LocalizedKnowledgeEntry
{
    private readonly KnowledgeEntry _entry;

    public LocalizedKnowledgeEntry(KnowledgeEntry entry)
    {
        _entry = entry;
    }

    private static string CurrentLanguage => Strings.CurrentLanguage;

    public string Name => _entry.Name;
    public string? Publisher => _entry.Publisher;
    public KnowledgeCategory Category => _entry.Category;
    public SafetyLevel SafetyLevel => _entry.SafetyLevel;
    public string? InfoUrl => _entry.InfoUrl;

    public string ShortDescription => _entry.GetShortDescription(CurrentLanguage);
    public string? FullDescription => _entry.GetFullDescription(CurrentLanguage);
    public string? DisableImpact => _entry.GetDisableImpact(CurrentLanguage);
    public string? PerformanceImpact => _entry.GetPerformanceImpact(CurrentLanguage);
    public string? Recommendation => _entry.GetRecommendation(CurrentLanguage);

    /// <summary>
    /// Gets the underlying knowledge entry.
    /// </summary>
    public KnowledgeEntry Entry => _entry;
}
