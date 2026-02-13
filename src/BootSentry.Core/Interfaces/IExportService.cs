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

using BootSentry.Core.Models;
using BootSentry.Core.Services;

namespace BootSentry.Core.Interfaces;

/// <summary>
/// Interface for exporting startup entries to various formats.
/// </summary>
public interface IExportService
{
    /// <summary>
    /// Exports entries to JSON format.
    /// </summary>
    /// <param name="entries">The entries to export.</param>
    /// <param name="options">Export options controlling output.</param>
    /// <returns>A JSON string representation of the entries.</returns>
    string ExportToJson(IEnumerable<StartupEntry> entries, ExportOptions options);

    /// <summary>
    /// Exports entries to CSV format.
    /// </summary>
    /// <param name="entries">The entries to export.</param>
    /// <param name="options">Export options controlling output.</param>
    /// <returns>A CSV string representation of the entries.</returns>
    string ExportToCsv(IEnumerable<StartupEntry> entries, ExportOptions options);

    /// <summary>
    /// Exports entries to a file in the specified format.
    /// </summary>
    /// <param name="entries">The entries to export.</param>
    /// <param name="filePath">The destination file path.</param>
    /// <param name="format">The export format (JSON or CSV).</param>
    /// <param name="options">Export options controlling output.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    Task ExportToFileAsync(
        IEnumerable<StartupEntry> entries,
        string filePath,
        ExportFormat format,
        ExportOptions options,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates a diagnostics ZIP file containing logs, entries export, and system info.
    /// </summary>
    /// <param name="entries">The entries to include in the diagnostics.</param>
    /// <param name="zipFilePath">The destination ZIP file path.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    Task ExportDiagnosticsZipAsync(
        IEnumerable<StartupEntry> entries,
        string zipFilePath,
        CancellationToken cancellationToken = default);
}
