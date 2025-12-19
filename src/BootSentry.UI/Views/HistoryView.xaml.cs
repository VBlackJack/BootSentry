using System.Windows;
using System.Windows.Input;
using Microsoft.Extensions.DependencyInjection;
using BootSentry.UI.Services;
using BootSentry.UI.ViewModels;

namespace BootSentry.UI.Views;

/// <summary>
/// Interaction logic for HistoryView.xaml
/// </summary>
public partial class HistoryView : Window
{
    public HistoryView()
    {
        InitializeComponent();
        DataContext = App.Services.GetRequiredService<HistoryViewModel>();

        Loaded += async (s, e) =>
        {
            // Apply title bar theme
            var themeService = App.Services.GetRequiredService<ThemeService>();
            ThemeService.ApplyTitleBarToWindow(this, themeService.IsDarkTheme);

            if (DataContext is HistoryViewModel vm)
            {
                await vm.LoadTransactionsCommand.ExecuteAsync(null);
            }
        };
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
}
