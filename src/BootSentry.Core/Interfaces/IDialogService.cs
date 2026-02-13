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
/// Abstraction for dialog interactions, decoupling ViewModels from WPF MessageBox and file dialogs.
/// </summary>
public interface IDialogService
{
    /// <summary>
    /// Shows a confirmation dialog with Yes/No buttons.
    /// </summary>
    /// <param name="message">The message to display.</param>
    /// <param name="title">The dialog title.</param>
    /// <returns>True if the user clicked Yes; otherwise false.</returns>
    bool Confirm(string message, string title);

    /// <summary>
    /// Shows an informational dialog.
    /// </summary>
    /// <param name="message">The message to display.</param>
    /// <param name="title">The dialog title.</param>
    void ShowInfo(string message, string title);

    /// <summary>
    /// Shows a warning dialog.
    /// </summary>
    /// <param name="message">The message to display.</param>
    /// <param name="title">The dialog title.</param>
    void ShowWarning(string message, string title);

    /// <summary>
    /// Shows an error dialog.
    /// </summary>
    /// <param name="message">The message to display.</param>
    /// <param name="title">The dialog title.</param>
    void ShowError(string message, string title);

    /// <summary>
    /// Shows a save file dialog and returns the selected file path, or null if cancelled.
    /// </summary>
    /// <param name="filter">The file type filter (e.g. "JSON Files (*.json)|*.json").</param>
    /// <param name="defaultExt">The default file extension (e.g. ".json").</param>
    /// <param name="fileName">The default file name.</param>
    /// <returns>The selected file path, or null if the user cancelled.</returns>
    string? ShowSaveFileDialog(string filter, string defaultExt, string fileName = "");

    /// <summary>
    /// Shows an open file dialog and returns the selected file path, or null if cancelled.
    /// </summary>
    /// <param name="filter">The file type filter.</param>
    /// <returns>The selected file path, or null if the user cancelled.</returns>
    string? ShowOpenFileDialog(string filter);
}
