using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Microsoft.Extensions.DependencyInjection;
using BootSentry.UI.Services;

namespace BootSentry.UI.Views;

/// <summary>
/// Onboarding dialog shown on first launch.
/// </summary>
public partial class OnboardingDialog : Window
{
    private int _currentStep = 1;
    private const int TotalSteps = 3;

    public bool IsExpertModeSelected { get; private set; }
    public bool DontShowAgain { get; private set; }

    public OnboardingDialog()
    {
        InitializeComponent();
        UpdateUI();
        KeyDown += OnboardingDialog_KeyDown;
        Loaded += OnboardingDialog_Loaded;
        Closing += OnboardingDialog_Closing;
    }

    private void OnboardingDialog_Loaded(object sender, RoutedEventArgs e)
    {
        var themeService = App.Services.GetRequiredService<ThemeService>();
        ThemeService.ApplyTitleBarToWindow(this, themeService.IsDarkTheme);
    }

    private void OnboardingDialog_Closing(object? sender, System.ComponentModel.CancelEventArgs e)
    {
        // Unsubscribe from events to prevent memory leaks
        KeyDown -= OnboardingDialog_KeyDown;
        Loaded -= OnboardingDialog_Loaded;
        Closing -= OnboardingDialog_Closing;
    }

    private void OnboardingDialog_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Escape)
        {
            DialogResult = false;
            Close();
            e.Handled = true;
        }
        else if (e.Key == Key.Enter)
        {
            NextButton_Click(sender, e);
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

    private void UpdateUI()
    {
        Step1Panel.Visibility = _currentStep == 1 ? Visibility.Visible : Visibility.Collapsed;
        Step2Panel.Visibility = _currentStep == 2 ? Visibility.Visible : Visibility.Collapsed;
        Step3Panel.Visibility = _currentStep == 3 ? Visibility.Visible : Visibility.Collapsed;

        PreviousButton.Visibility = _currentStep > 1 ? Visibility.Visible : Visibility.Collapsed;
        NextButton.Content = _currentStep == TotalSteps
            ? UI.Resources.Strings.Get("OnboardingStart")
            : UI.Resources.Strings.Get("OnboardingNext");

        // Update step indicators
        var accentBrush = (Brush)FindResource("AccentBrush");
        var borderBrush = (Brush)FindResource("BorderBrush");
        Step1Indicator.Fill = _currentStep >= 1 ? accentBrush : borderBrush;
        Step2Indicator.Fill = _currentStep >= 2 ? accentBrush : borderBrush;
        Step3Indicator.Fill = _currentStep >= 3 ? accentBrush : borderBrush;
        StepText.Text = UI.Resources.Strings.Format("OnboardingStepN", _currentStep, TotalSteps);
    }

    private void NextButton_Click(object sender, RoutedEventArgs e)
    {
        if (_currentStep < TotalSteps)
        {
            _currentStep++;
            UpdateUI();
        }
        else
        {
            // Finish onboarding
            IsExpertModeSelected = ExpertModeRadio.IsChecked == true;
            DontShowAgain = DontShowAgainCheckBox.IsChecked == true;

            // Save settings
            var settingsService = App.Services.GetRequiredService<SettingsService>();
            settingsService.Settings.ExpertModeDefault = IsExpertModeSelected;
            if (DontShowAgain)
            {
                settingsService.Settings.ShowOnboarding = false;
            }
            settingsService.Save();

            DialogResult = true;
            Close();
        }
    }

    private void PreviousButton_Click(object sender, RoutedEventArgs e)
    {
        if (_currentStep > 1)
        {
            _currentStep--;
            UpdateUI();
        }
    }

    /// <summary>
    /// Shows the onboarding dialog if it should be shown.
    /// </summary>
    public static bool ShowIfNeeded()
    {
        var settingsService = App.Services.GetRequiredService<SettingsService>();
        if (!settingsService.Settings.ShowOnboarding)
        {
            return false;
        }

        var dialog = new OnboardingDialog();
        return dialog.ShowDialog() == true;
    }
}
