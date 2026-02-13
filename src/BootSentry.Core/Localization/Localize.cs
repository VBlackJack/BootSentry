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

namespace BootSentry.Core.Localization;

/// <summary>
/// Lightweight localization accessor for non-UI layers.
/// The UI layer populates the resolver at startup.
/// </summary>
public static class Localize
{
    private static Func<string, string>? _resolver;

    /// <summary>
    /// Sets the localization resolver function (called by UI layer at startup).
    /// </summary>
    public static void SetResolver(Func<string, string> resolver)
    {
        _resolver = resolver;
    }

    /// <summary>
    /// Gets a localized string by key. Returns the key itself if no resolver is set.
    /// </summary>
    public static string Get(string key)
    {
        return _resolver?.Invoke(key) ?? key;
    }

    /// <summary>
    /// Gets a localized string and formats it with arguments.
    /// </summary>
    public static string Format(string key, params object[] args)
    {
        var template = Get(key);
        return string.Format(template, args);
    }
}
