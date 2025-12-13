using System.Windows;
using Microsoft.Extensions.DependencyInjection;
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
