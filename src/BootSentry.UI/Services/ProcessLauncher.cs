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

using System.Diagnostics;
using BootSentry.Core.Interfaces;

namespace BootSentry.UI.Services;

/// <summary>
/// Implementation of <see cref="IProcessLauncher"/> using System.Diagnostics.Process.
/// </summary>
public class ProcessLauncher : IProcessLauncher
{
    /// <inheritdoc />
    public void OpenUrl(string url)
    {
        Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
    }

    /// <inheritdoc />
    public void OpenFile(string filePath)
    {
        Process.Start(new ProcessStartInfo(filePath) { UseShellExecute = true });
    }

    /// <inheritdoc />
    public void OpenFolder(string folderPath)
    {
        Process.Start("explorer.exe", folderPath);
    }

    /// <inheritdoc />
    public void OpenFolderAndSelectFile(string filePath)
    {
        Process.Start("explorer.exe", $"/select,\"{filePath}\"");
    }

    /// <inheritdoc />
    public void StartShellExecute(string fileName)
    {
        Process.Start(new ProcessStartInfo(fileName) { UseShellExecute = true });
    }
}
