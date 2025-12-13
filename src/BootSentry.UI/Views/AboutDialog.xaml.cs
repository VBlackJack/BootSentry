using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using BootSentry.Core.Services;

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
    }

    private void AboutDialog_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Escape || e.Key == Key.Enter)
        {
            Close();
            e.Handled = true;
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
            Process.Start(new ProcessStartInfo("https://github.com/julien-music/BootSentry")
            {
                UseShellExecute = true
            });
        }
        catch { }
    }

    private void LicenseButton_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            Process.Start(new ProcessStartInfo("https://github.com/julien-music/BootSentry/blob/main/LICENSE")
            {
                UseShellExecute = true
            });
        }
        catch { }
    }

    private void CloseButton_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }
}
