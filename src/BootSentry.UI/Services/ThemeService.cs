using System.Windows;
using System.Windows.Media;
using Microsoft.Win32;

namespace BootSentry.UI.Services;

/// <summary>
/// Service for managing application themes.
/// </summary>
public class ThemeService
{
    private ThemeMode _currentTheme = ThemeMode.System;

    public event EventHandler? ThemeChanged;

    /// <summary>
    /// Gets or sets the current theme mode.
    /// </summary>
    public ThemeMode CurrentTheme
    {
        get => _currentTheme;
        set
        {
            if (_currentTheme != value)
            {
                _currentTheme = value;
                ApplyTheme();
                ThemeChanged?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    /// <summary>
    /// Gets whether the effective theme is dark.
    /// </summary>
    public bool IsDarkTheme
    {
        get
        {
            return _currentTheme switch
            {
                ThemeMode.Dark => true,
                ThemeMode.Light => false,
                _ => IsSystemDarkTheme()
            };
        }
    }

    /// <summary>
    /// Initializes the theme service.
    /// </summary>
    public void Initialize()
    {
        // Listen for system theme changes
        SystemEvents.UserPreferenceChanged += OnUserPreferenceChanged;
        ApplyTheme();
    }

    /// <summary>
    /// Applies the current theme to the application.
    /// </summary>
    public void ApplyTheme()
    {
        var isDark = IsDarkTheme;
        var resources = Application.Current.Resources;

        if (isDark)
        {
            // Dark theme colors
            resources["WindowBackground"] = new SolidColorBrush(Color.FromRgb(30, 30, 30));
            resources["WindowForeground"] = new SolidColorBrush(Color.FromRgb(240, 240, 240));
            resources["PanelBackground"] = new SolidColorBrush(Color.FromRgb(45, 45, 45));
            resources["BorderBrush"] = new SolidColorBrush(Color.FromRgb(60, 60, 60));
            resources["ControlBackground"] = new SolidColorBrush(Color.FromRgb(55, 55, 55));
            resources["ControlForeground"] = new SolidColorBrush(Color.FromRgb(240, 240, 240));
            resources["AccentBrush"] = new SolidColorBrush(Color.FromRgb(0, 120, 215));
            resources["AccentForeground"] = new SolidColorBrush(Colors.White);
            resources["SuccessBrush"] = new SolidColorBrush(Color.FromRgb(16, 185, 129));
            resources["WarningBrush"] = new SolidColorBrush(Color.FromRgb(245, 158, 11));
            resources["ErrorBrush"] = new SolidColorBrush(Color.FromRgb(239, 68, 68));
            resources["InfoBrush"] = new SolidColorBrush(Color.FromRgb(59, 130, 246));
            resources["DisabledForeground"] = new SolidColorBrush(Color.FromRgb(128, 128, 128));
            resources["RowAlternate"] = new SolidColorBrush(Color.FromRgb(38, 38, 38));
            resources["RowHover"] = new SolidColorBrush(Color.FromRgb(50, 50, 50));
            resources["RowSelected"] = new SolidColorBrush(Color.FromRgb(0, 90, 158));
        }
        else
        {
            // Light theme colors
            resources["WindowBackground"] = new SolidColorBrush(Color.FromRgb(249, 249, 249));
            resources["WindowForeground"] = new SolidColorBrush(Color.FromRgb(32, 32, 32));
            resources["PanelBackground"] = new SolidColorBrush(Colors.White);
            resources["BorderBrush"] = new SolidColorBrush(Color.FromRgb(220, 220, 220));
            resources["ControlBackground"] = new SolidColorBrush(Colors.White);
            resources["ControlForeground"] = new SolidColorBrush(Color.FromRgb(32, 32, 32));
            resources["AccentBrush"] = new SolidColorBrush(Color.FromRgb(0, 103, 192));
            resources["AccentForeground"] = new SolidColorBrush(Colors.White);
            resources["SuccessBrush"] = new SolidColorBrush(Color.FromRgb(16, 185, 129));
            resources["WarningBrush"] = new SolidColorBrush(Color.FromRgb(217, 119, 6));
            resources["ErrorBrush"] = new SolidColorBrush(Color.FromRgb(220, 38, 38));
            resources["InfoBrush"] = new SolidColorBrush(Color.FromRgb(37, 99, 235));
            resources["DisabledForeground"] = new SolidColorBrush(Color.FromRgb(160, 160, 160));
            resources["RowAlternate"] = new SolidColorBrush(Color.FromRgb(245, 245, 245));
            resources["RowHover"] = new SolidColorBrush(Color.FromRgb(235, 235, 235));
            resources["RowSelected"] = new SolidColorBrush(Color.FromRgb(0, 120, 215));
        }
    }

    private void OnUserPreferenceChanged(object sender, UserPreferenceChangedEventArgs e)
    {
        if (e.Category == UserPreferenceCategory.General && _currentTheme == ThemeMode.System)
        {
            Application.Current.Dispatcher.Invoke(ApplyTheme);
        }
    }

    private static bool IsSystemDarkTheme()
    {
        try
        {
            using var key = Registry.CurrentUser.OpenSubKey(
                @"Software\Microsoft\Windows\CurrentVersion\Themes\Personalize");

            if (key?.GetValue("AppsUseLightTheme") is int value)
            {
                return value == 0;
            }
        }
        catch
        {
            // Ignore errors
        }

        return false;
    }
}

/// <summary>
/// Theme mode options.
/// </summary>
public enum ThemeMode
{
    System,
    Light,
    Dark
}
