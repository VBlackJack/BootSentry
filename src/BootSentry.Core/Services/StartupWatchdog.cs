using System.Management;
using Microsoft.Extensions.Logging;
using BootSentry.Core.Enums;

namespace BootSentry.Core.Services;

/// <summary>
/// Event args for when a new startup entry is detected.
/// </summary>
public class StartupEntryDetectedEventArgs : EventArgs
{
    public required string Name { get; init; }
    public required string Path { get; init; }
    public required string Source { get; init; }
    public required EntryType Type { get; init; }
    public DateTime DetectedAt { get; init; } = DateTime.Now;
}

/// <summary>
/// Monitors startup locations for new entries in real-time.
/// Uses FileSystemWatcher for folders and WMI for registry changes.
/// </summary>
public class StartupWatchdog : IDisposable
{
    private readonly ILogger<StartupWatchdog> _logger;
    private readonly List<FileSystemWatcher> _folderWatchers = new();
    private readonly List<ManagementEventWatcher> _registryWatchers = new();
    private bool _isRunning;
    private bool _disposed;

    /// <summary>
    /// Raised when a new startup entry is detected.
    /// </summary>
    public event EventHandler<StartupEntryDetectedEventArgs>? EntryDetected;

    /// <summary>
    /// Raised when a startup entry is removed.
    /// </summary>
    public event EventHandler<StartupEntryDetectedEventArgs>? EntryRemoved;

    public StartupWatchdog(ILogger<StartupWatchdog> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Gets whether the watchdog is currently running.
    /// </summary>
    public bool IsRunning => _isRunning;

    /// <summary>
    /// Starts monitoring startup locations.
    /// </summary>
    public void Start()
    {
        if (_isRunning)
            return;

        _logger.LogInformation("Starting startup watchdog");

        try
        {
            // Watch startup folders
            SetupFolderWatchers();

            // Watch registry keys (via WMI)
            SetupRegistryWatchers();

            _isRunning = true;
            _logger.LogInformation("Startup watchdog started successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to start startup watchdog");
            Stop();
            throw;
        }
    }

    /// <summary>
    /// Stops monitoring startup locations.
    /// </summary>
    public void Stop()
    {
        if (!_isRunning)
            return;

        _logger.LogInformation("Stopping startup watchdog");

        foreach (var watcher in _folderWatchers)
        {
            try
            {
                watcher.EnableRaisingEvents = false;
                watcher.Dispose();
            }
            catch { }
        }
        _folderWatchers.Clear();

        foreach (var watcher in _registryWatchers)
        {
            try
            {
                watcher.Stop();
                watcher.Dispose();
            }
            catch { }
        }
        _registryWatchers.Clear();

        _isRunning = false;
        _logger.LogInformation("Startup watchdog stopped");
    }

    private void SetupFolderWatchers()
    {
        // User startup folder
        var userStartup = Environment.GetFolderPath(Environment.SpecialFolder.Startup);
        if (Directory.Exists(userStartup))
        {
            var watcher = CreateFolderWatcher(userStartup, "User Startup Folder");
            _folderWatchers.Add(watcher);
        }

        // Common startup folder
        var commonStartup = Environment.GetFolderPath(Environment.SpecialFolder.CommonStartup);
        if (Directory.Exists(commonStartup))
        {
            var watcher = CreateFolderWatcher(commonStartup, "Common Startup Folder");
            _folderWatchers.Add(watcher);
        }
    }

    private FileSystemWatcher CreateFolderWatcher(string path, string sourceName)
    {
        var watcher = new FileSystemWatcher(path)
        {
            NotifyFilter = NotifyFilters.FileName | NotifyFilters.LastWrite | NotifyFilters.CreationTime,
            IncludeSubdirectories = false,
            EnableRaisingEvents = true
        };

        watcher.Created += (s, e) =>
        {
            _logger.LogWarning("New startup entry detected in {Source}: {File}", sourceName, e.Name);
            OnEntryDetected(new StartupEntryDetectedEventArgs
            {
                Name = Path.GetFileNameWithoutExtension(e.Name) ?? e.Name ?? "Unknown",
                Path = e.FullPath,
                Source = sourceName,
                Type = EntryType.StartupFolder
            });
        };

        watcher.Deleted += (s, e) =>
        {
            _logger.LogInformation("Startup entry removed from {Source}: {File}", sourceName, e.Name);
            OnEntryRemoved(new StartupEntryDetectedEventArgs
            {
                Name = Path.GetFileNameWithoutExtension(e.Name) ?? e.Name ?? "Unknown",
                Path = e.FullPath,
                Source = sourceName,
                Type = EntryType.StartupFolder
            });
        };

        _logger.LogDebug("Watching folder: {Path}", path);
        return watcher;
    }

    private void SetupRegistryWatchers()
    {
        // Watch HKCU\Software\Microsoft\Windows\CurrentVersion\Run
        var runKeys = new[]
        {
            (@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Run", "HKCU Run"),
            (@"HKEY_LOCAL_MACHINE\Software\Microsoft\Windows\CurrentVersion\Run", "HKLM Run"),
            (@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\RunOnce", "HKCU RunOnce"),
            (@"HKEY_LOCAL_MACHINE\Software\Microsoft\Windows\CurrentVersion\RunOnce", "HKLM RunOnce"),
        };

        foreach (var (keyPath, sourceName) in runKeys)
        {
            try
            {
                var watcher = CreateRegistryWatcher(keyPath, sourceName);
                if (watcher != null)
                    _registryWatchers.Add(watcher);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to create registry watcher for {Key}", keyPath);
            }
        }
    }

    private ManagementEventWatcher? CreateRegistryWatcher(string keyPath, string sourceName)
    {
        try
        {
            // Parse the key path for WMI
            var hive = keyPath.StartsWith("HKEY_LOCAL_MACHINE") ? "HKEY_LOCAL_MACHINE" : "HKEY_CURRENT_USER";
            var subKey = keyPath.Replace(hive + @"\", "").Replace(@"\", @"\\\\");

            var query = new WqlEventQuery(
                $"SELECT * FROM RegistryValueChangeEvent WHERE Hive='{hive}' AND KeyPath='{subKey}'");

            var watcher = new ManagementEventWatcher(query);

            watcher.EventArrived += (s, e) =>
            {
                var valueName = e.NewEvent.Properties["ValueName"]?.Value?.ToString() ?? "Unknown";
                _logger.LogWarning("Registry change detected in {Source}: {Value}", sourceName, valueName);

                OnEntryDetected(new StartupEntryDetectedEventArgs
                {
                    Name = valueName,
                    Path = keyPath,
                    Source = sourceName,
                    Type = keyPath.Contains("RunOnce") ? EntryType.RegistryRunOnce : EntryType.RegistryRun
                });
            };

            watcher.Start();
            _logger.LogDebug("Watching registry key: {Key}", keyPath);
            return watcher;
        }
        catch (ManagementException ex)
        {
            // WMI registry monitoring may not be available on all systems
            _logger.LogDebug(ex, "WMI registry monitoring not available for {Key}", keyPath);
            return null;
        }
    }

    protected virtual void OnEntryDetected(StartupEntryDetectedEventArgs e)
    {
        EntryDetected?.Invoke(this, e);
    }

    protected virtual void OnEntryRemoved(StartupEntryDetectedEventArgs e)
    {
        EntryRemoved?.Invoke(this, e);
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
            Stop();
        }

        _disposed = true;
    }
}
