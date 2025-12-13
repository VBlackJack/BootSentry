using System.Windows;
using System.Windows.Input;
using Microsoft.Extensions.DependencyInjection;
using BootSentry.UI.Services;

namespace BootSentry.UI.Views;

/// <summary>
/// Onboarding dialog shown on first launch.
/// </summary>
public partial class OnboardingDialog : Window
{
    private int _currentStep = 1;
    private const int TotalSteps = 2;

    public bool IsExpertModeSelected { get; private set; }
    public bool DontShowAgain { get; private set; }

    public OnboardingDialog()
    {
        InitializeComponent();
        UpdateUI();
        KeyDown += OnboardingDialog_KeyDown;
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

    private void UpdateUI()
    {
        Step1Panel.Visibility = _currentStep == 1 ? Visibility.Visible : Visibility.Collapsed;
        Step2Panel.Visibility = _currentStep == 2 ? Visibility.Visible : Visibility.Collapsed;

        PreviousButton.Visibility = _currentStep > 1 ? Visibility.Visible : Visibility.Collapsed;
        NextButton.Content = _currentStep == TotalSteps ? "Commencer" : "Suivant";
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
