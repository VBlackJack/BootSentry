/*
 * Copyright 2025 Julien Bombled
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using BootSentry.Core.Models.Integrations;

namespace BootSentry.Core.Interfaces;

/// <summary>
/// Interface for interacting with the VirusTotal API.
/// </summary>
public interface IVirusTotalService : IDisposable
{
    /// <summary>
    /// Sets the API key used for authentication with VirusTotal.
    /// </summary>
    /// <param name="apiKey">The VirusTotal API key.</param>
    void SetApiKey(string apiKey);

    /// <summary>
    /// Gets whether the service has a valid API key configured.
    /// </summary>
    bool IsConfigured { get; }

    /// <summary>
    /// Gets the file report for a given hash (MD5, SHA-1, or SHA-256).
    /// </summary>
    /// <param name="hash">The file hash to look up.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The VirusTotal file data, or null if the hash is unknown.</returns>
    Task<VirusTotalData?> GetFileReportAsync(string hash, CancellationToken cancellationToken = default);
}
