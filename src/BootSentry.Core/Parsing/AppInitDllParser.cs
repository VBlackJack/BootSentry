using System.Text;

namespace BootSentry.Core.Parsing;

/// <summary>
/// Helpers for parsing and serializing AppInit_DLLs values safely.
/// </summary>
public static class AppInitDllParser
{
    /// <summary>
    /// Parses an AppInit_DLLs value into individual DLL entries.
    /// Handles quoted values, comma/semicolon separators and legacy whitespace-separated values.
    /// </summary>
    public static List<string> Parse(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return [];

        var input = value.Trim();
        var parts = new List<string>();
        var current = new StringBuilder();

        var inQuotes = false;
        var sawQuotes = false;
        var sawExplicitDelimiter = false;

        void Flush()
        {
            if (current.Length == 0)
                return;

            var token = current.ToString().Trim();
            current.Clear();

            if (!string.IsNullOrWhiteSpace(token))
                parts.Add(token);
        }

        foreach (var ch in input)
        {
            if (ch == '"')
            {
                inQuotes = !inQuotes;
                sawQuotes = true;
                continue;
            }

            if (!inQuotes && (ch == ',' || ch == ';'))
            {
                sawExplicitDelimiter = true;
                Flush();
                continue;
            }

            if (!inQuotes && char.IsWhiteSpace(ch))
            {
                Flush();
                continue;
            }

            current.Append(ch);
        }

        Flush();

        // Preserve unquoted paths with spaces (e.g. C:\Program Files\MyDll.dll)
        if (!sawQuotes && !sawExplicitDelimiter && parts.Count > 1 && !parts.All(LooksLikeDllToken))
        {
            return [TrimOuterQuotes(input)];
        }

        return parts
            .Select(TrimOuterQuotes)
            .Where(p => !string.IsNullOrWhiteSpace(p))
            .ToList();
    }

    /// <summary>
    /// Serializes DLL paths into an AppInit_DLLs value.
    /// Values containing whitespace or separators are quoted.
    /// </summary>
    public static string Serialize(IEnumerable<string> dllPaths)
    {
        return string.Join(" ", dllPaths
            .Where(p => !string.IsNullOrWhiteSpace(p))
            .Select(SerializeToken));
    }

    /// <summary>
    /// Compares two DLL paths in a tolerant, Windows-friendly way.
    /// </summary>
    public static bool AreEquivalent(string? left, string? right)
    {
        if (string.IsNullOrWhiteSpace(left) || string.IsNullOrWhiteSpace(right))
            return false;

        return string.Equals(
            NormalizeForComparison(left),
            NormalizeForComparison(right),
            StringComparison.OrdinalIgnoreCase);
    }

    private static string NormalizeForComparison(string value)
    {
        var normalized = TrimOuterQuotes(value).Trim();
        normalized = Environment.ExpandEnvironmentVariables(normalized);
        normalized = normalized.Replace('/', '\\');
        return normalized;
    }

    private static string SerializeToken(string value)
    {
        var token = TrimOuterQuotes(value).Trim();
        if (token.Length == 0)
            return token;

        var requiresQuotes = token.Any(ch => char.IsWhiteSpace(ch) || ch == ',' || ch == ';');
        if (!requiresQuotes)
            return token;

        return $"\"{token.Replace("\"", "\\\"")}\"";
    }

    private static bool LooksLikeDllToken(string token)
    {
        var value = token.Trim();
        return value.EndsWith(".dll", StringComparison.OrdinalIgnoreCase)
            || value.EndsWith(".ocx", StringComparison.OrdinalIgnoreCase);
    }

    private static string TrimOuterQuotes(string value)
    {
        var trimmed = value.Trim();
        if (trimmed.Length >= 2 && trimmed.StartsWith('"') && trimmed.EndsWith('"'))
            return trimmed[1..^1];
        return trimmed;
    }
}
