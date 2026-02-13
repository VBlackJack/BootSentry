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

using Microsoft.Win32;

namespace BootSentry.Core.Helpers;

/// <summary>
/// Shared utility for parsing Windows registry paths into their hive and sub-key components.
/// Supports both abbreviated (HKCU, HKLM, HKCR, HKU) and full-name forms
/// (HKEY_CURRENT_USER, HKEY_LOCAL_MACHINE, HKEY_CLASSES_ROOT, HKEY_USERS).
/// </summary>
public static class RegistryPathHelper
{
    /// <summary>
    /// Parses a full registry path (e.g. "HKLM\SOFTWARE\Microsoft") into its
    /// <see cref="RegistryHive"/> and the remaining sub-key path.
    /// </summary>
    /// <param name="fullPath">
    /// The full registry path. The first segment (before the first backslash)
    /// must be a recognised hive identifier.
    /// </param>
    /// <returns>A tuple containing the <see cref="RegistryHive"/> and the sub-key path.</returns>
    /// <exception cref="ArgumentException">Thrown when the hive prefix is not recognised.</exception>
    public static (RegistryHive Hive, string Path) ParseRegistryPath(string fullPath)
    {
        var parts = fullPath.Split('\\', 2);
        var hiveName = parts[0].ToUpperInvariant();
        var path = parts.Length > 1 ? parts[1] : string.Empty;

        var hive = hiveName switch
        {
            "HKCU" or "HKEY_CURRENT_USER" => RegistryHive.CurrentUser,
            "HKLM" or "HKEY_LOCAL_MACHINE" => RegistryHive.LocalMachine,
            "HKCR" or "HKEY_CLASSES_ROOT" => RegistryHive.ClassesRoot,
            "HKU" or "HKEY_USERS" => RegistryHive.Users,
            _ => throw new ArgumentException($"Unknown registry hive: {hiveName}")
        };

        return (hive, path);
    }
}
