using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Animation;
using BootSentry.UI.Resources;
using BootSentry.UI.Services;

namespace BootSentry.UI.Views;

/// <summary>
/// An error dialog with recovery options.
/// </summary>
public partial class ErrorDialog : Window
{
    public new string Title { get; set; } = Strings.Get("ErrorTitle");
    public string Message { get; set; } = string.Empty;
    public string? TechnicalDetails { get; set; }
    public string? Suggestion { get; set; }
    public bool CanRetry { get; set; }
    public bool HasDetails => !string.IsNullOrEmpty(TechnicalDetails);
    public bool HasSuggestion => !string.IsNullOrEmpty(Suggestion);

    /// <summary>
    /// Gets whether the user clicked Retry.
    /// </summary>
    public bool WantsRetry { get; private set; }

    public ErrorDialog()
    {
        InitializeComponent();
        DataContext = this;
    }

    /// <summary>
    /// Creates an error dialog for a general error.
    /// </summary>
    public static ErrorDialog Create(string message, Exception? ex = null, Window? owner = null)
    {
        return new ErrorDialog
        {
            Owner = owner,
            Title = Strings.Get("ErrorTitle"),
            Message = message,
            TechnicalDetails = ex?.ToString(),
            CanRetry = false
        };
    }

    /// <summary>
    /// Creates an error dialog for a registry access error.
    /// </summary>
    public static ErrorDialog ForRegistryError(Exception ex, Window? owner = null)
    {
        return new ErrorDialog
        {
            Owner = owner,
            Title = Strings.Get("ErrorTitle"),
            Message = "Impossible d'accéder au registre Windows.",
            TechnicalDetails = ex.Message,
            Suggestion = "Lancez l'application en tant qu'administrateur pour accéder au registre système.",
            CanRetry = true
        };
    }

    /// <summary>
    /// Creates an error dialog for a file access error.
    /// </summary>
    public static ErrorDialog ForFileError(string path, Exception ex, Window? owner = null)
    {
        return new ErrorDialog
        {
            Owner = owner,
            Title = Strings.Get("ErrorTitle"),
            Message = $"Impossible d'accéder au fichier:\n{path}",
            TechnicalDetails = ex.Message,
            Suggestion = "Le fichier est peut-être verrouillé par un autre programme ou vous n'avez pas les permissions nécessaires.",
            CanRetry = true
        };
    }

    /// <summary>
    /// Creates an error dialog for a network error.
    /// </summary>
    public static ErrorDialog ForNetworkError(Exception ex, Window? owner = null)
    {
        return new ErrorDialog
        {
            Owner = owner,
            Title = Strings.Get("ErrorTitle"),
            Message = "Erreur de connexion réseau.",
            TechnicalDetails = ex.Message,
            Suggestion = "Vérifiez votre connexion Internet et réessayez.",
            CanRetry = true
        };
    }

    /// <summary>
    /// Creates an error dialog for an antivirus scan error.
    /// </summary>
    public static ErrorDialog ForScanError(Exception ex, Window? owner = null)
    {
        return new ErrorDialog
        {
            Owner = owner,
            Title = Strings.Get("ErrorTitle"),
            Message = "Erreur lors du scan antivirus.",
            TechnicalDetails = ex.Message,
            Suggestion = "Assurez-vous que Windows Defender ou un autre antivirus compatible AMSI est actif.",
            CanRetry = true
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
    }

    private void Window_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Escape)
        {
            Close();
        }
    }

    private void RetryButton_Click(object sender, RoutedEventArgs e)
    {
        WantsRetry = true;
        DialogResult = true;
        Close();
    }

    private void CopyButton_Click(object sender, RoutedEventArgs e)
    {
        var errorText = $"{Title}\n\n{Message}";
        if (!string.IsNullOrEmpty(TechnicalDetails))
        {
            errorText += $"\n\nDétails techniques:\n{TechnicalDetails}";
        }
        Clipboard.SetText(errorText);
    }

    private void CloseButton_Click(object sender, RoutedEventArgs e)
    {
        DialogResult = false;
        Close();
    }
}
