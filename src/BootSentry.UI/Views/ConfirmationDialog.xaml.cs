using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using BootSentry.UI.Resources;
using BootSentry.UI.Services;

namespace BootSentry.UI.Views;

/// <summary>
/// Types of confirmation dialogs.
/// </summary>
public enum ConfirmationType
{
    Warning,
    Danger,
    Info
}

/// <summary>
/// A confirmation dialog with animations.
/// </summary>
public partial class ConfirmationDialog : Window
{
    private bool _isClosing;

    public new string Title { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public string? ItemName { get; set; }
    public bool HasItemName => !string.IsNullOrEmpty(ItemName);
    public new string Icon { get; set; } = "\uE7BA"; // Warning icon
    public Brush IconBrush { get; set; } = System.Windows.Media.Brushes.Orange;
    public string ConfirmText { get; set; } = "OK";
    public string CancelText { get; set; } = Strings.Get("Cancel");
    public Brush ConfirmButtonBackground { get; set; } = System.Windows.Media.Brushes.Orange;
    public Brush ConfirmButtonForeground { get; set; } = System.Windows.Media.Brushes.White;

    public ConfirmationDialog()
    {
        InitializeComponent();
        DataContext = this;
    }

    /// <summary>
    /// Creates a confirmation dialog for deleting an item.
    /// </summary>
    public static ConfirmationDialog ForDelete(string itemName, Window? owner = null)
    {
        var resources = Application.Current.Resources;
        var errorBrush = resources["ErrorBrush"] as Brush ?? System.Windows.Media.Brushes.Red;

        return new ConfirmationDialog
        {
            Owner = owner,
            Title = Strings.Get("ConfirmDeleteTitle"),
            Message = Strings.Get("ConfirmDeleteMessage"),
            ItemName = itemName,
            Icon = "\uE74D", // Delete icon
            IconBrush = errorBrush,
            ConfirmText = Strings.Get("Delete"),
            ConfirmButtonBackground = errorBrush,
            ConfirmButtonForeground = System.Windows.Media.Brushes.White
        };
    }

    /// <summary>
    /// Creates a confirmation dialog for deleting multiple items.
    /// </summary>
    public static ConfirmationDialog ForDeleteMultiple(int count, Window? owner = null)
    {
        var resources = Application.Current.Resources;
        var errorBrush = resources["ErrorBrush"] as Brush ?? System.Windows.Media.Brushes.Red;

        return new ConfirmationDialog
        {
            Owner = owner,
            Title = Strings.Get("ConfirmDeleteTitle"),
            Message = Strings.Format("ConfirmDeleteMultiple", count),
            Icon = "\uE74D", // Delete icon
            IconBrush = errorBrush,
            ConfirmText = Strings.Get("Delete"),
            ConfirmButtonBackground = errorBrush,
            ConfirmButtonForeground = System.Windows.Media.Brushes.White
        };
    }

    /// <summary>
    /// Creates a confirmation dialog for purging all data.
    /// </summary>
    public static ConfirmationDialog ForPurge(Window? owner = null)
    {
        var resources = Application.Current.Resources;
        var errorBrush = resources["ErrorBrush"] as Brush ?? System.Windows.Media.Brushes.Red;

        return new ConfirmationDialog
        {
            Owner = owner,
            Title = Strings.Get("ConfirmPurgeTitle"),
            Message = Strings.Get("ConfirmPurgeMessage"),
            Icon = "\uE74D", // Delete icon
            IconBrush = errorBrush,
            ConfirmText = Strings.Get("ConfirmPurgeButton"),
            ConfirmButtonBackground = errorBrush,
            ConfirmButtonForeground = System.Windows.Media.Brushes.White
        };
    }

    /// <summary>
    /// Creates a confirmation dialog for resetting settings.
    /// </summary>
    public static ConfirmationDialog ForReset(Window? owner = null)
    {
        var resources = Application.Current.Resources;
        var warningBrush = resources["WarningBrush"] as Brush ?? System.Windows.Media.Brushes.Orange;

        return new ConfirmationDialog
        {
            Owner = owner,
            Title = Strings.Get("ConfirmResetTitle"),
            Message = Strings.Get("ConfirmResetMessage"),
            Icon = "\uE777", // Sync icon
            IconBrush = warningBrush,
            ConfirmText = Strings.Get("ConfirmResetButton"),
            ConfirmButtonBackground = warningBrush,
            ConfirmButtonForeground = System.Windows.Media.Brushes.White
        };
    }

    /// <summary>
    /// Creates a generic warning confirmation dialog.
    /// </summary>
    public static ConfirmationDialog ForWarning(string title, string message, string confirmText, Window? owner = null)
    {
        var resources = Application.Current.Resources;
        var warningBrush = resources["WarningBrush"] as Brush ?? System.Windows.Media.Brushes.Orange;

        return new ConfirmationDialog
        {
            Owner = owner,
            Title = title,
            Message = message,
            Icon = "\uE7BA", // Warning icon
            IconBrush = warningBrush,
            ConfirmText = confirmText,
            ConfirmButtonBackground = warningBrush,
            ConfirmButtonForeground = System.Windows.Media.Brushes.White
        };
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
        if (!ThemeService.ShouldReduceMotion)
        {
            var storyboard = (Storyboard)Resources["ShowAnimation"];
            storyboard.Begin(this);
        }
        else
        {
            // Instant display without animation
            DialogBorder.Opacity = 1;
        }
        ConfirmButton.Focus();
    }

    private void Window_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Escape)
        {
            CloseWithResult(false);
        }
    }

    private void ConfirmButton_Click(object sender, RoutedEventArgs e)
    {
        CloseWithResult(true);
    }

    private void CancelButton_Click(object sender, RoutedEventArgs e)
    {
        CloseWithResult(false);
    }

    private async void CloseWithResult(bool result)
    {
        try
        {
            if (_isClosing) return;
            _isClosing = true;

            DialogResult = result;

            if (!ThemeService.ShouldReduceMotion)
            {
                var storyboard = (Storyboard)Resources["HideAnimation"];
                storyboard.Begin(this);
                await Task.Delay(150);
            }

            Close();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error in CloseWithResult: {ex}");
        }
    }
}
