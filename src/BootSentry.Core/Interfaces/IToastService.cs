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
/// Interface for displaying toast notifications to the user.
/// The ToastType enum lives in BootSentry.UI; this interface exposes
/// convenience methods that do not depend on UI-specific types.
/// </summary>
public interface IToastService
{
    /// <summary>
    /// Shows an informational toast notification.
    /// </summary>
    /// <param name="message">The message to display.</param>
    void ShowInfo(string message);

    /// <summary>
    /// Shows a success toast notification.
    /// </summary>
    /// <param name="message">The message to display.</param>
    void ShowSuccess(string message);

    /// <summary>
    /// Shows a warning toast notification.
    /// </summary>
    /// <param name="message">The message to display.</param>
    void ShowWarning(string message);

    /// <summary>
    /// Shows an error toast notification.
    /// </summary>
    /// <param name="message">The message to display.</param>
    void ShowError(string message);
}
