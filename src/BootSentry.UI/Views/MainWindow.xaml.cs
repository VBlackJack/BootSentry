using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Extensions.DependencyInjection;
using BootSentry.Core.Models;
using BootSentry.UI.Services;
using BootSentry.UI.ViewModels;

namespace BootSentry.UI.Views;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        DataContext = App.Services.GetRequiredService<MainViewModel>();

        Loaded += MainWindow_Loaded;

        // Handle Ctrl+F to focus search box
        PreviewKeyDown += MainWindow_PreviewKeyDown;
    }

    private void EntriesGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (DataContext is MainViewModel vm && sender is DataGrid dataGrid)
        {
            var selectedEntries = dataGrid.SelectedItems.Cast<StartupEntry>().ToList();
            vm.UpdateSelectedEntries(selectedEntries);
        }
    }

    private void MainWindow_PreviewKeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.F && Keyboard.Modifiers == ModifierKeys.Control)
        {
            SearchBox.Focus();
            SearchBox.SelectAll();
            e.Handled = true;
        }
    }

    private void MainWindow_Loaded(object sender, RoutedEventArgs e)
    {
        // Show onboarding if needed
        var settingsService = App.Services.GetRequiredService<SettingsService>();
        if (settingsService.Settings.ShowOnboarding)
        {
            var dialog = new OnboardingDialog { Owner = this };
            if (dialog.ShowDialog() == true)
            {
                // Apply expert mode if selected
                if (dialog.IsExpertModeSelected && DataContext is MainViewModel vm)
                {
                    vm.IsExpertMode = true;
                }
            }
        }
    }
}
