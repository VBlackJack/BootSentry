using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using Microsoft.Extensions.DependencyInjection;
using BootSentry.Core.Services;
using BootSentry.UI.Services;

namespace BootSentry.UI.Views;

/// <summary>
/// About dialog showing application information.
/// </summary>
public partial class AboutDialog : Window
{
    public AboutDialog()
    {
        InitializeComponent();
        LoadInfo();
        KeyDown += AboutDialog_KeyDown;
        Loaded += AboutDialog_Loaded;
    }

    private void AboutDialog_Loaded(object sender, RoutedEventArgs e)
    {
        var themeService = App.Services.GetRequiredService<ThemeService>();
        ThemeService.ApplyTitleBarToWindow(this, themeService.IsDarkTheme);
    }

    private void AboutDialog_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Escape || e.Key == Key.Enter)
        {
            Close();
            e.Handled = true;
        }
    }

    private void Header_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        if (e.ButtonState == MouseButtonState.Pressed)
        {
            DragMove();
        }
    }

    private void LoadInfo()
    {
        VersionText.Text = $"Version {UpdateChecker.CurrentVersion}";
        DotNetVersionText.Text = Environment.Version.ToString();
        OsVersionText.Text = Environment.OSVersion.ToString();
    }

    private void GitHubButton_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            Process.Start(new ProcessStartInfo("https://github.com/VBlackJack/BootSentry")
            {
                UseShellExecute = true
            });
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Failed to open GitHub URL: {ex.Message}");
        }
    }

    private void LicenseButton_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            Process.Start(new ProcessStartInfo("https://github.com/VBlackJack/BootSentry/blob/main/LICENSE")
            {
                UseShellExecute = true
            });
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Failed to open License URL: {ex.Message}");
        }
    }

    private void CloseButton_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }
}
