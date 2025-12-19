using System.Drawing;
using System.IO;
using System.Windows.Forms;
using BootSentry.Core.Services;
using Microsoft.Extensions.Logging;

namespace BootSentry.UI.Services;

/// <summary>
/// Manages the system tray icon and notifications.
/// Integrates with the StartupWatchdog for real-time monitoring.
/// </summary>
public class TrayIconService : IDisposable
{
    private readonly ILogger<TrayIconService> _logger;
    private readonly StartupWatchdog _watchdog;
    private readonly SettingsService _settingsService;
    private NotifyIcon? _notifyIcon;
    private bool _disposed;

    public TrayIconService(
        ILogger<TrayIconService> logger,
        StartupWatchdog watchdog,
        SettingsService settingsService)
    {
        _logger = logger;
        _watchdog = watchdog;
        _settingsService = settingsService;
    }

    /// <summary>
    /// Initializes and shows the tray icon.
    /// </summary>
    public void Initialize()
    {
        if (_notifyIcon != null)
            return;

        _logger.LogInformation("Initializing tray icon service");

        _notifyIcon = new NotifyIcon
        {
            Text = "BootSentry",
            Visible = false,
            ContextMenuStrip = CreateContextMenu()
        };

        // Try to load the icon from resources
        try
        {
            var iconPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "bootsentry.ico");
            if (File.Exists(iconPath))
            {
                _notifyIcon.Icon = new Icon(iconPath);
            }
            else
            {
                // Use a system icon as fallback
                _notifyIcon.Icon = SystemIcons.Shield;
            }
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to load tray icon, using system default");
            _notifyIcon.Icon = SystemIcons.Shield;
        }

        // Subscribe to watchdog events
        _watchdog.EntryDetected += OnEntryDetected;
        _watchdog.EntryRemoved += OnEntryRemoved;

        _notifyIcon.DoubleClick += (s, e) => ShowMainWindow();
    }

    /// <summary>
    /// Shows the tray icon.
    /// </summary>
    public void Show()
    {
        if (_notifyIcon != null)
            _notifyIcon.Visible = true;
    }

    /// <summary>
    /// Hides the tray icon.
    /// </summary>
    public void Hide()
    {
        if (_notifyIcon != null)
            _notifyIcon.Visible = false;
    }

    /// <summary>
    /// Starts real-time monitoring mode.
    /// </summary>
    public void StartMonitoring()
    {
        if (!_watchdog.IsRunning)
        {
            _watchdog.Start();
            ShowNotification("BootSentry", "Surveillance en temps réel activée", ToolTipIcon.Info);
        }
    }

    /// <summary>
    /// Stops real-time monitoring mode.
    /// </summary>
    public void StopMonitoring()
    {
        if (_watchdog.IsRunning)
        {
            _watchdog.Stop();
            ShowNotification("BootSentry", "Surveillance en temps réel désactivée", ToolTipIcon.Info);
        }
    }

    private ContextMenuStrip CreateContextMenu()
    {
        var menu = new ContextMenuStrip();

        menu.Items.Add("Ouvrir BootSentry", null, (s, e) => ShowMainWindow());
        menu.Items.Add("-");

        var monitorItem = new ToolStripMenuItem("Surveillance temps réel")
        {
            CheckOnClick = true
        };
        monitorItem.CheckedChanged += (s, e) =>
        {
            if (monitorItem.Checked)
                StartMonitoring();
            else
                StopMonitoring();
        };
        menu.Items.Add(monitorItem);

        menu.Items.Add("-");
        menu.Items.Add("Quitter", null, (s, e) =>
        {
            Hide();
            Application.Current.Shutdown();
        });

        return menu;
    }

    private void OnEntryDetected(object? sender, StartupEntryDetectedEventArgs e)
    {
        var message = $"Nouvelle entrée de démarrage détectée:\n{e.Name}\nSource: {e.Source}";

        Application.Current.Dispatcher.Invoke(() =>
        {
            ShowNotification("⚠️ Nouvelle entrée détectée", message, ToolTipIcon.Warning);
        });
    }

    private void OnEntryRemoved(object? sender, StartupEntryDetectedEventArgs e)
    {
        var message = $"Entrée de démarrage supprimée:\n{e.Name}\nSource: {e.Source}";

        Application.Current.Dispatcher.Invoke(() =>
        {
            ShowNotification("Entrée supprimée", message, ToolTipIcon.Info);
        });
    }

    private void ShowNotification(string title, string message, ToolTipIcon icon)
    {
        _notifyIcon?.ShowBalloonTip(5000, title, message, icon);
    }

    private void ShowMainWindow()
    {
        Application.Current.Dispatcher.Invoke(() =>
        {
            var mainWindow = Application.Current.MainWindow;
            if (mainWindow != null)
            {
                mainWindow.Show();
                mainWindow.WindowState = System.Windows.WindowState.Normal;
                mainWindow.Activate();
            }
        });
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (_disposed)
            return;

        if (disposing)
        {
            _watchdog.EntryDetected -= OnEntryDetected;
            _watchdog.EntryRemoved -= OnEntryRemoved;

            if (_notifyIcon != null)
            {
                _notifyIcon.Visible = false;
                _notifyIcon.Dispose();
                _notifyIcon = null;
            }
        }

        _disposed = true;
    }
}
