using System.Net.Http.Headers;
using System.Text.Json;
using BootSentry.Core.Interfaces;
using BootSentry.Core.Models.Integrations;
using Microsoft.Extensions.Logging;

namespace BootSentry.Core.Services.Integrations;

/// <summary>
/// Service to interact with VirusTotal API v3.
/// </summary>
public class VirusTotalService : IVirusTotalService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<VirusTotalService> _logger;
    private string? _apiKey;

    private const string BaseUrl = Constants.Urls.VirusTotalApi;

    public VirusTotalService(ILogger<VirusTotalService> logger)
    {
        _logger = logger;
        _httpClient = new HttpClient
        {
            BaseAddress = new Uri(BaseUrl)
        };
    }

    public void SetApiKey(string apiKey)
    {
        _apiKey = apiKey;
        _httpClient.DefaultRequestHeaders.Remove("x-apikey");
        
        if (!string.IsNullOrWhiteSpace(apiKey))
        {
            _httpClient.DefaultRequestHeaders.Add("x-apikey", apiKey);
        }
    }

    public bool IsConfigured => !string.IsNullOrWhiteSpace(_apiKey);

    /// <summary>
    /// Gets the file report for a given hash (MD5, SHA-1, or SHA-256).
    /// </summary>
    public async Task<VirusTotalData?> GetFileReportAsync(string hash, CancellationToken cancellationToken = default)
    {
        if (!IsConfigured)
        {
            throw new InvalidOperationException("VirusTotal API key is not configured.");
        }

        try
        {
            var response = await _httpClient.GetAsync($"files/{hash}", cancellationToken).ConfigureAwait(false);

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                _logger.LogInformation("Hash not found in VirusTotal: {Hash}", hash);
                return null; // File unknown to VirusTotal
            }

            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized) // 401
            {
                throw new UnauthorizedAccessException("Invalid VirusTotal API key.");
            }
            
            if (response.StatusCode == System.Net.HttpStatusCode.Forbidden) // 403
            {
                throw new UnauthorizedAccessException("Access forbidden. Check your API key permissions.");
            }

            if ((int)response.StatusCode == 429) // Too Many Requests
            {
                throw new InvalidOperationException("VirusTotal API quota exceeded.");
            }

            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
            var vtResponse = JsonSerializer.Deserialize<VirusTotalResponse>(json);

            return vtResponse?.Data;
        }
        catch (Exception ex) when (ex is not InvalidOperationException && ex is not UnauthorizedAccessException)
        {
            _logger.LogError(ex, "Error fetching VirusTotal report for hash {Hash}", hash);
            throw;
        }
    }

    public void Dispose()
    {
        _httpClient.Dispose();
        GC.SuppressFinalize(this);
    }
}
