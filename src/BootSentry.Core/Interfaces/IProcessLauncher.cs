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

namespace BootSentry.Core.Interfaces;

/// <summary>
/// Abstraction for launching external processes, decoupling ViewModels from System.Diagnostics.Process.
/// </summary>
public interface IProcessLauncher
{
    /// <summary>
    /// Opens a URL in the default browser.
    /// </summary>
    /// <param name="url">The URL to open.</param>
    void OpenUrl(string url);

    /// <summary>
    /// Opens a file with its associated application.
    /// </summary>
    /// <param name="filePath">The path to the file to open.</param>
    void OpenFile(string filePath);

    /// <summary>
    /// Opens a folder in Windows Explorer.
    /// </summary>
    /// <param name="folderPath">The path to the folder to open.</param>
    void OpenFolder(string folderPath);

    /// <summary>
    /// Opens Windows Explorer with the specified file selected.
    /// </summary>
    /// <param name="filePath">The path to the file to select in Explorer.</param>
    void OpenFolderAndSelectFile(string filePath);

    /// <summary>
    /// Starts an executable with shell execution.
    /// </summary>
    /// <param name="fileName">The executable file name or path.</param>
    void StartShellExecute(string fileName);
}
