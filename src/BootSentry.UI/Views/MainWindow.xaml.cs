using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Extensions.DependencyInjection;
using Wpf.Ui.Controls;
using BootSentry.Core.Enums;
using BootSentry.Core.Models;
using BootSentry.UI.Enums;
using BootSentry.UI.Services;
using BootSentry.UI.ViewModels;

namespace BootSentry.UI.Views;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : FluentWindow
{
    public MainWindow()
    {
        InitializeComponent();
        DataContext = App.Services.GetRequiredService<MainViewModel>();

        Loaded += MainWindow_Loaded;
        Closing += MainWindow_Closing;

        // Handle Ctrl+F to focus search box
        PreviewKeyDown += MainWindow_PreviewKeyDown;
    }

    private void MainWindow_Closing(object? sender, System.ComponentModel.CancelEventArgs e)
    {
        // Save window state before closing
        SaveWindowState();

        // Unsubscribe from events to prevent memory leaks
        Loaded -= MainWindow_Loaded;
        Closing -= MainWindow_Closing;
        PreviewKeyDown -= MainWindow_PreviewKeyDown;

        // Dispose the view model
        if (DataContext is IDisposable disposable)
        {
            disposable.Dispose();
        }
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

    private void SearchBox_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Return && DataContext is MainViewModel vm)
        {
            // Force refresh when Enter is pressed in search box
            vm.RefreshCommand.Execute(null);
            e.Handled = true;
        }
    }

    private void MainWindow_Loaded(object sender, RoutedEventArgs e)
    {
        // Apply title bar theme
        var themeService = App.Services.GetRequiredService<ThemeService>();
        ThemeService.ApplyTitleBarToWindow(this, themeService.IsDarkTheme);

        // Set up toast container
        var toastService = App.Services.GetRequiredService<ToastService>();
        toastService.SetContainer(ToastContainer);

        // Restore window size and position
        var settingsService = App.Services.GetRequiredService<SettingsService>();
        RestoreWindowState(settingsService);

        // Show onboarding if needed
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

    private void RestoreWindowState(SettingsService settingsService)
    {
        var windowState = settingsService.Settings.Window;
        if (windowState != null)
        {
            if (windowState.Width > 0 && windowState.Height > 0)
            {
                Width = windowState.Width;
                Height = windowState.Height;
            }

            // Ensure window is within screen bounds
            var screenWidth = SystemParameters.VirtualScreenWidth;
            var screenHeight = SystemParameters.VirtualScreenHeight;
            if (windowState.Left >= 0 && windowState.Top >= 0 &&
                windowState.Left < screenWidth && windowState.Top < screenHeight)
            {
                Left = windowState.Left;
                Top = windowState.Top;
            }

            if (windowState.IsMaximized)
            {
                WindowState = System.Windows.WindowState.Maximized;
            }
        }
    }

    private void SaveWindowState()
    {
        var settingsService = App.Services.GetRequiredService<SettingsService>();
        settingsService.Settings.Window = new Services.WindowState
        {
            Width = Width,
            Height = Height,
            Left = Left,
            Top = Top,
            IsMaximized = WindowState == System.Windows.WindowState.Maximized
        };
        settingsService.Save();
    }

    private void CategoryTabs_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (DataContext is not MainViewModel vm || sender is not TabControl tabControl)
            return;

        if (tabControl.SelectedItem is TabItem selectedTab && selectedTab.Tag is string tag)
        {
            vm.SelectedTab = tag switch
            {
                "Applications" => NavigationTab.Applications,
                "Browsers" => NavigationTab.Browsers,
                "System" => NavigationTab.System,
                "Advanced" => NavigationTab.Advanced,
                _ => NavigationTab.Applications
            };
        }
    }

    private void TitleBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        if (e.ClickCount == 2)
        {
            // Double-click to maximize/restore
            WindowState = WindowState == WindowState.Maximized
                ? WindowState.Normal
                : WindowState.Maximized;
        }
        else
        {
            DragMove();
        }
    }
}
