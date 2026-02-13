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
/// Interface for managing application settings.
/// Provides persistence operations accessible from any layer.
/// The concrete <c>AppSettings</c> model lives in BootSentry.UI; consumers
/// that need typed access should resolve the concrete SettingsService.
/// </summary>
public interface ISettingsService
{
    /// <summary>
    /// Loads settings from disk.
    /// </summary>
    /// <returns>True if settings were loaded successfully.</returns>
    bool Load();

    /// <summary>
    /// Saves settings to disk.
    /// </summary>
    /// <returns>True if settings were saved successfully.</returns>
    bool Save();

    /// <summary>
    /// Resets settings to defaults.
    /// </summary>
    /// <returns>True if the reset and subsequent save succeeded.</returns>
    bool Reset();

    /// <summary>
    /// Purges all application data (settings, logs, backups, local data).
    /// </summary>
    /// <returns>True if all purge operations succeeded.</returns>
    bool PurgeAllData();
}
