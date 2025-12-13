using System.Net.Http.Json;
using System.Reflection;
using System.Text.Json.Serialization;

namespace BootSentry.Core.Services;

/// <summary>
/// Service for checking application updates from GitHub releases.
/// </summary>
public class UpdateChecker : IDisposable
{
    private readonly HttpClient _httpClient;
    private readonly string _repoOwner;
    private readonly string _repoName;
    private bool _disposed;

    public UpdateChecker(string repoOwner = "your-username", string repoName = "BootSentry")
    {
        _repoOwner = repoOwner;
        _repoName = repoName;

        _httpClient = new HttpClient();
        _httpClient.DefaultRequestHeaders.UserAgent.ParseAdd($"BootSentry/{CurrentVersion} UpdateCheck");
        _httpClient.DefaultRequestHeaders.Accept.ParseAdd("application/vnd.github.v3+json");
        _httpClient.Timeout = TimeSpan.FromSeconds(10);
    }

    /// <summary>
    /// Gets the current application version.
    /// </summary>
    public static string CurrentVersion
    {
        get
        {
            var version = Assembly.GetEntryAssembly()?.GetName().Version;
            return version != null ? $"{version.Major}.{version.Minor}.{version.Build}" : "1.0.0";
        }
    }

    /// <summary>
    /// Checks for updates from GitHub releases.
    /// </summary>
    /// <returns>Update info if available, null otherwise.</returns>
    public async Task<UpdateInfo?> CheckForUpdateAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var url = $"https://api.github.com/repos/{_repoOwner}/{_repoName}/releases/latest";
            var release = await _httpClient.GetFromJsonAsync<GitHubRelease>(url, cancellationToken);

            if (release == null || string.IsNullOrEmpty(release.TagName))
                return null;

            var latestVersion = ParseVersion(release.TagName);
            var currentVersion = ParseVersion(CurrentVersion);

            if (latestVersion > currentVersion)
            {
                return new UpdateInfo
                {
                    CurrentVersion = CurrentVersion,
                    LatestVersion = release.TagName.TrimStart('v'),
                    ReleaseUrl = release.HtmlUrl ?? $"https://github.com/{_repoOwner}/{_repoName}/releases/latest",
                    ReleaseNotes = release.Body,
                    PublishedAt = release.PublishedAt,
                    IsPrerelease = release.Prerelease
                };
            }

            return null;
        }
        catch (HttpRequestException)
        {
            // Network error - silently fail
            return null;
        }
        catch (TaskCanceledException)
        {
            // Timeout - silently fail
            return null;
        }
        catch
        {
            // Any other error - silently fail
            return null;
        }
    }

    private static Version ParseVersion(string versionString)
    {
        var cleaned = versionString.TrimStart('v', 'V');

        // Handle versions like "1.0.0-beta"
        var dashIndex = cleaned.IndexOf('-');
        if (dashIndex > 0)
            cleaned = cleaned[..dashIndex];

        if (Version.TryParse(cleaned, out var version))
            return version;

        return new Version(0, 0, 0);
    }

    public void Dispose()
    {
        if (!_disposed)
        {
            _httpClient.Dispose();
            _disposed = true;
        }
    }
}

/// <summary>
/// Information about an available update.
/// </summary>
public class UpdateInfo
{
    public string CurrentVersion { get; set; } = string.Empty;
    public string LatestVersion { get; set; } = string.Empty;
    public string ReleaseUrl { get; set; } = string.Empty;
    public string? ReleaseNotes { get; set; }
    public DateTime? PublishedAt { get; set; }
    public bool IsPrerelease { get; set; }
}

/// <summary>
/// GitHub release API response model.
/// </summary>
internal class GitHubRelease
{
    [JsonPropertyName("tag_name")]
    public string? TagName { get; set; }

    [JsonPropertyName("html_url")]
    public string? HtmlUrl { get; set; }

    [JsonPropertyName("body")]
    public string? Body { get; set; }

    [JsonPropertyName("published_at")]
    public DateTime? PublishedAt { get; set; }

    [JsonPropertyName("prerelease")]
    public bool Prerelease { get; set; }

    [JsonPropertyName("assets")]
    public List<GitHubAsset>? Assets { get; set; }
}

/// <summary>
/// GitHub release asset model.
/// </summary>
internal class GitHubAsset
{
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("browser_download_url")]
    public string? BrowserDownloadUrl { get; set; }

    [JsonPropertyName("size")]
    public long Size { get; set; }
}
