using System.Drawing;
using System.IO;
using System.Windows.Forms;
using BootSentry.Core;
using BootSentry.Core.Services;
using BootSentry.UI.Resources;
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
            Text = Constants.AppName,
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

        // Start monitoring if enabled in settings
        if (_settingsService.Settings.EnableRealTimeMonitoring)
        {
            StartMonitoring();
        }
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
            ShowNotification(Strings.Get("TrayTooltip"), Strings.Get("TrayMonitorActive"), ToolTipIcon.Info);
            
            _settingsService.Settings.EnableRealTimeMonitoring = true;
            _settingsService.Save();
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
            ShowNotification(Strings.Get("TrayTooltip"), Strings.Get("TrayMonitorInactive"), ToolTipIcon.Info);
            
            _settingsService.Settings.EnableRealTimeMonitoring = false;
            _settingsService.Save();
        }
    }

    private ContextMenuStrip CreateContextMenu()
    {
        var menu = new ContextMenuStrip();

        menu.Items.Add(Strings.Get("TrayOpen"), null, (s, e) => ShowMainWindow());
        menu.Items.Add("-");

        var monitorItem = new ToolStripMenuItem(Strings.Get("TrayMonitor"))
        {
            CheckOnClick = true,
            Checked = _settingsService.Settings.EnableRealTimeMonitoring
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
        menu.Items.Add(Strings.Get("TrayExit"), null, (s, e) =>
        {
            if (Application.Current is App app)
            {
                app.IsExiting = true;
            }
            Hide();
            Application.Current.Shutdown();
        });

        return menu;
    }

    private void OnEntryDetected(object? sender, StartupEntryDetectedEventArgs e)
    {
        var message = Strings.Format("NotifNewEntryBody", e.Name, e.Source);

        Application.Current.Dispatcher.Invoke(() =>
        {
            ShowNotification(Strings.Get("NotifNewEntry"), message, ToolTipIcon.Warning);
        });
    }

    private void OnEntryRemoved(object? sender, StartupEntryDetectedEventArgs e)
    {
        var message = Strings.Format("NotifEntryRemovedBody", e.Name, e.Source);

        Application.Current.Dispatcher.Invoke(() =>
        {
            ShowNotification(Strings.Get("NotifEntryRemoved"), message, ToolTipIcon.Info);
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
