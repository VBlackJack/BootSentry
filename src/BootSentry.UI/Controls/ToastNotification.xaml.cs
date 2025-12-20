using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;
using BootSentry.UI.Services;

namespace BootSentry.UI.Controls;

/// <summary>
/// Toast notification types.
/// </summary>
public enum ToastType
{
    Info,
    Success,
    Warning,
    Error
}

/// <summary>
/// A toast notification control.
/// </summary>
public partial class ToastNotification : System.Windows.Controls.UserControl
{
    public static readonly DependencyProperty MessageProperty =
        DependencyProperty.Register(nameof(Message), typeof(string), typeof(ToastNotification));

    public static readonly DependencyProperty ToastTypeProperty =
        DependencyProperty.Register(nameof(Type), typeof(ToastType), typeof(ToastNotification),
            new PropertyMetadata(ToastType.Info, OnTypeChanged));

    public string Message
    {
        get => (string)GetValue(MessageProperty);
        set => SetValue(MessageProperty, value);
    }

    public ToastType Type
    {
        get => (ToastType)GetValue(ToastTypeProperty);
        set => SetValue(ToastTypeProperty, value);
    }

    public event EventHandler? Closed;

    public ToastNotification()
    {
        InitializeComponent();
        Opacity = 0;
        UpdateAppearance();
    }

    private static void OnTypeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is ToastNotification toast)
        {
            toast.UpdateAppearance();
        }
    }

    private void UpdateAppearance()
    {
        var resources = Application.Current.Resources;

        (string icon, Brush color) = Type switch
        {
            ToastType.Success => ("\uE73E", resources["SuccessBrush"] as Brush ?? System.Windows.Media.Brushes.Green),
            ToastType.Warning => ("\uE7BA", resources["WarningBrush"] as Brush ?? System.Windows.Media.Brushes.Orange),
            ToastType.Error => ("\uE783", resources["ErrorBrush"] as Brush ?? System.Windows.Media.Brushes.Red),
            _ => ("\uE946", resources["InfoBrush"] as Brush ?? System.Windows.Media.Brushes.Blue)
        };

        IconText.Text = icon;
        IconText.Foreground = color;
        ToastBorder.BorderBrush = color;
    }

    public void Show()
    {
        Visibility = Visibility.Visible;
        if (!ThemeService.ShouldReduceMotion)
        {
            var storyboard = (Storyboard)Resources["ShowAnimation"];
            storyboard.Begin(this);
        }
        else
        {
            Opacity = 1;
        }
    }

    public async Task ShowAsync(int durationMs = 3000)
    {
        Show();
        await Task.Delay(durationMs);
        await HideAsync();
    }

    public async Task HideAsync()
    {
        if (!ThemeService.ShouldReduceMotion)
        {
            var storyboard = (Storyboard)Resources["HideAnimation"];
            storyboard.Begin(this);
            await Task.Delay(200);
        }
        Visibility = Visibility.Collapsed;
        Closed?.Invoke(this, EventArgs.Empty);
    }

    private async void CloseButton_Click(object sender, RoutedEventArgs e)
    {
        await HideAsync();
    }
}
