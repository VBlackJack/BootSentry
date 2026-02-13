using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace BootSentry.UI.Converters;

/// <summary>
/// Converts a boolean value to Visibility.
/// True = Visible, False = Collapsed.
/// </summary>
public class BoolToVisibilityConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is bool boolValue)
        {
            return boolValue ? Visibility.Visible : Visibility.Collapsed;
        }
        return Visibility.Collapsed;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is Visibility visibility)
        {
            return visibility == Visibility.Visible;
        }
        return false;
    }
}

/// <summary>
/// Converts null to Visible, non-null to Collapsed.
/// </summary>
public class NullToVisibilityConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return value == null ? Visibility.Visible : Visibility.Collapsed;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

/// <summary>
/// Converts null or whitespace string values to Visible, others to Collapsed.
/// </summary>
public class NullOrWhiteSpaceToVisibilityConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return value is string text && !string.IsNullOrWhiteSpace(text)
            ? Visibility.Collapsed
            : Visibility.Visible;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

/// <summary>
/// Converts non-empty string values to Visible, null/whitespace to Collapsed.
/// </summary>
public class NotNullOrWhiteSpaceToVisibilityConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return value is string text && !string.IsNullOrWhiteSpace(text)
            ? Visibility.Visible
            : Visibility.Collapsed;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

/// <summary>
/// Converts non-null to Visible, null to Collapsed.
/// </summary>
public class NotNullToVisibilityConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return value != null ? Visibility.Visible : Visibility.Collapsed;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

/// <summary>
/// Inverts a boolean value.
/// </summary>
public class InverseBoolConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is bool boolValue)
        {
            return !boolValue;
        }
        return false;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is bool boolValue)
        {
            return !boolValue;
        }
        return false;
    }
}

/// <summary>
/// Converts boolean to inverted Visibility.
/// True = Collapsed, False = Visible.
/// </summary>
public class InverseBoolToVisibilityConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is bool boolValue)
        {
            return boolValue ? Visibility.Collapsed : Visibility.Visible;
        }
        return Visibility.Visible;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is Visibility visibility)
        {
            return visibility != Visibility.Visible;
        }
        return true;
    }
}

/// <summary>
/// Converts RiskLevel enum to a SolidColorBrush using theme-aware resources.
/// </summary>
public class RiskLevelToColorConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        var resources = Application.Current.Resources;
        var defaultBrush = resources["RiskUnknownBrush"] as System.Windows.Media.Brush
            ?? new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(107, 114, 128));

        if (value is BootSentry.Core.Enums.RiskLevel riskLevel)
        {
            return riskLevel switch
            {
                BootSentry.Core.Enums.RiskLevel.Safe => resources["RiskSafeBrush"] as System.Windows.Media.Brush ?? defaultBrush,
                BootSentry.Core.Enums.RiskLevel.Unknown => resources["RiskUnknownBrush"] as System.Windows.Media.Brush ?? defaultBrush,
                BootSentry.Core.Enums.RiskLevel.Suspicious => resources["RiskSuspiciousBrush"] as System.Windows.Media.Brush ?? defaultBrush,
                BootSentry.Core.Enums.RiskLevel.Critical => resources["RiskCriticalBrush"] as System.Windows.Media.Brush ?? defaultBrush,
                _ => defaultBrush
            };
        }
        return defaultBrush;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

/// <summary>
/// Converts RiskLevel enum to a localized string.
/// </summary>
public class RiskLevelToStringConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is BootSentry.Core.Enums.RiskLevel riskLevel)
        {
            return riskLevel switch
            {
                BootSentry.Core.Enums.RiskLevel.Safe => Resources.Strings.Get("RiskSafe"),
                BootSentry.Core.Enums.RiskLevel.Suspicious => Resources.Strings.Get("RiskSuspicious"),
                BootSentry.Core.Enums.RiskLevel.Critical => Resources.Strings.Get("RiskCritical"),
                _ => Resources.Strings.Get("RiskUnknown")
            };
        }

        return Resources.Strings.Get("RiskUnknown");
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

/// <summary>
/// Converts SignatureStatus enum to a SolidColorBrush using theme-aware resources.
/// </summary>
public class SignatureStatusToColorConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        var resources = Application.Current.Resources;
        var defaultBrush = resources["NeutralBrush"] as System.Windows.Media.Brush
            ?? new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(107, 114, 128));

        if (value is BootSentry.Core.Enums.SignatureStatus status)
        {
            return status switch
            {
                BootSentry.Core.Enums.SignatureStatus.SignedTrusted => resources["SuccessBrush"] as System.Windows.Media.Brush ?? defaultBrush,
                BootSentry.Core.Enums.SignatureStatus.SignedUntrusted => resources["WarningBrush"] as System.Windows.Media.Brush ?? defaultBrush,
                BootSentry.Core.Enums.SignatureStatus.Unsigned => resources["DangerBrush"] as System.Windows.Media.Brush ?? defaultBrush,
                _ => defaultBrush
            };
        }
        return defaultBrush;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

/// <summary>
/// Converts EntryStatus enum to a SolidColorBrush using theme-aware resources.
/// </summary>
public class EntryStatusToColorConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        var resources = Application.Current.Resources;
        var defaultBrush = resources["NeutralBrush"] as System.Windows.Media.Brush
            ?? new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(107, 114, 128));

        if (value is BootSentry.Core.Enums.EntryStatus status)
        {
            return status switch
            {
                BootSentry.Core.Enums.EntryStatus.Enabled => resources["StatusEnabledBrush"] as System.Windows.Media.Brush ?? defaultBrush,
                BootSentry.Core.Enums.EntryStatus.Disabled => resources["StatusDisabledBrush"] as System.Windows.Media.Brush ?? defaultBrush,
                _ => defaultBrush
            };
        }
        return defaultBrush;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

/// <summary>
/// Converts ScanResult enum to a localized string.
/// </summary>
public class ScanResultToStringConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is BootSentry.Core.Enums.ScanResult result)
        {
            return result switch
            {
                BootSentry.Core.Enums.ScanResult.Clean => Resources.Strings.Get("ScanResultCleanShort"),
                BootSentry.Core.Enums.ScanResult.Malware => Resources.Strings.Get("ScanResultMalwareShort"),
                BootSentry.Core.Enums.ScanResult.Blocked => Resources.Strings.Get("ScanResultBlockedShort"),
                BootSentry.Core.Enums.ScanResult.NotScanned => Resources.Strings.Get("ScanResultNotScannedShort"),
                BootSentry.Core.Enums.ScanResult.TooLarge => Resources.Strings.Get("ScanResultTooLargeShort"),
                BootSentry.Core.Enums.ScanResult.Error => Resources.Strings.Get("ScanResultErrorShort"),
                BootSentry.Core.Enums.ScanResult.NoAntivirusProvider => Resources.Strings.Get("ScanResultNoAVShort"),
                _ => Resources.Strings.Get("ScanResultUnknown")
            };
        }
        return Resources.Strings.Get("ScanResultUnknown");
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

/// <summary>
/// Converts SafetyLevel enum to a localized string.
/// </summary>
public class SafetyLevelToStringConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is BootSentry.Knowledge.Models.SafetyLevel level)
        {
            return level switch
            {
                BootSentry.Knowledge.Models.SafetyLevel.Critical => Resources.Strings.Get("SafetyCritical"),
                BootSentry.Knowledge.Models.SafetyLevel.Important => Resources.Strings.Get("SafetyImportant"),
                BootSentry.Knowledge.Models.SafetyLevel.Safe => Resources.Strings.Get("SafetySafe"),
                BootSentry.Knowledge.Models.SafetyLevel.RecommendedDisable => Resources.Strings.Get("SafetyRecommendedDisable"),
                BootSentry.Knowledge.Models.SafetyLevel.ShouldRemove => Resources.Strings.Get("SafetyShouldRemove"),
                _ => Resources.Strings.Get("SafetyUnknown")
            };
        }
        return Resources.Strings.Get("SafetyUnknown");
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
