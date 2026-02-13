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

using System.Globalization;
using System.Windows.Data;

namespace BootSentry.UI.Converters;

/// <summary>
/// Converts an enum value to a localized string using the i18n system.
/// The ConverterParameter specifies the enum type name (e.g., "EntryType").
/// The key is built as "Enum{TypeName}{Value}" and resolved via <see cref="Resources.Strings.Get"/>.
/// </summary>
/// <example>
/// XAML usage:
/// <code>
/// {Binding Type, Converter={StaticResource EnumToLocalizedStringConverter}, ConverterParameter=EntryType}
/// </code>
/// For EntryType.RegistryRun, this resolves to Strings.Get("EnumEntryTypeRegistryRun").
/// </example>
public class EnumToLocalizedStringConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value == null)
            return string.Empty;

        var typeName = parameter as string ?? value.GetType().Name;
        var enumValue = value.ToString();
        var key = $"Enum{typeName}{enumValue}";

        return Resources.Strings.Get(key);
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
