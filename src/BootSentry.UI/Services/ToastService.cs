using System.Windows;
using BootSentry.UI.Controls;

namespace BootSentry.UI.Services;

/// <summary>
/// Service for displaying toast notifications.
/// </summary>
public class ToastService
{
    private System.Windows.Controls.Panel? _container;
    private readonly Queue<(string message, ToastType type)> _queue = new();
    private bool _isShowing;

    /// <summary>
    /// Sets the container panel where toasts will be displayed.
    /// </summary>
    public void SetContainer(System.Windows.Controls.Panel container)
    {
        _container = container;
    }

    /// <summary>
    /// Shows an info toast.
    /// </summary>
    public void ShowInfo(string message) => Show(message, ToastType.Info);

    /// <summary>
    /// Shows a success toast.
    /// </summary>
    public void ShowSuccess(string message) => Show(message, ToastType.Success);

    /// <summary>
    /// Shows a warning toast.
    /// </summary>
    public void ShowWarning(string message) => Show(message, ToastType.Warning);

    /// <summary>
    /// Shows an error toast.
    /// </summary>
    public void ShowError(string message) => Show(message, ToastType.Error);

    /// <summary>
    /// Shows a toast notification with adaptive duration based on type.
    /// </summary>
    public void Show(string message, ToastType type = ToastType.Info, int? durationMs = null)
    {
        if (_container == null)
            return;

        // Adaptive duration based on toast type
        var duration = durationMs ?? type switch
        {
            ToastType.Info => 2500,
            ToastType.Success => 3500,
            ToastType.Warning => 5000,
            ToastType.Error => 6000,
            _ => 3000
        };

        Application.Current.Dispatcher.Invoke(async () =>
        {
            _queue.Enqueue((message, type));
            await ProcessQueueAsync(duration);
        });
    }

    private async Task ProcessQueueAsync(int durationMs)
    {
        if (_isShowing || _container == null)
            return;

        _isShowing = true;

        while (_queue.Count > 0)
        {
            var (message, type) = _queue.Dequeue();

            var toast = new ToastNotification
            {
                Message = message,
                Type = type,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Center,
                VerticalAlignment = System.Windows.VerticalAlignment.Top
            };

            _container.Children.Add(toast);

            try
            {
                await toast.ShowAsync(durationMs);
            }
            finally
            {
                _container.Children.Remove(toast);
            }
        }

        _isShowing = false;
    }
}
