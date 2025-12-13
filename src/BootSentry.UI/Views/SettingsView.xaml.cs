using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using Microsoft.Extensions.DependencyInjection;
using BootSentry.UI.ViewModels;

namespace BootSentry.UI.Views;

/// <summary>
/// Interaction logic for SettingsView.xaml
/// </summary>
public partial class SettingsView : Window
{
    private static readonly Regex NumericRegex = new(@"^[0-9]+$", RegexOptions.Compiled);

    public SettingsView()
    {
        InitializeComponent();
        DataContext = App.Services.GetRequiredService<SettingsViewModel>();
    }

    private void CloseButton_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }

    private void Window_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Escape)
        {
            Close();
            e.Handled = true;
        }
    }

    private void NumericTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
    {
        // Only allow numeric input
        e.Handled = !NumericRegex.IsMatch(e.Text);
    }
}
