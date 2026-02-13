using System.IO;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using BootSentry.Actions;
using BootSentry.Backup;
using BootSentry.Core;
using BootSentry.Core.Interfaces;
using BootSentry.Core.Localization;
using BootSentry.Core.Services;
using BootSentry.Core.Services.Integrations;
using BootSentry.Knowledge.Services;
using BootSentry.Providers;
using BootSentry.Security;
using BootSentry.UI.Resources;
using BootSentry.UI.Services;
using BootSentry.UI.ViewModels;

namespace BootSentry.UI;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    private static Mutex? _mutex;

    public static IServiceProvider Services { get; private set; } = null!;
    
    public bool IsExiting { get; set; }

    protected override void OnStartup(StartupEventArgs e)
    {
        // Single instance check
        if (!AcquireSingleInstance())
        {
            MessageBox.Show(
                Strings.Get("AppAlreadyRunning"),
                Constants.AppName,
                MessageBoxButton.OK,
                MessageBoxImage.Information);
            Shutdown();
            return;
        }

        // Ensure log directory exists
        var logDir = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
            Constants.AppName, Constants.Directories.Logs);
        Directory.CreateDirectory(logDir);

        var logPath = Path.Combine(logDir, Constants.Files.LogFilePattern);

        // Configure Serilog
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.File(logPath,
                rollingInterval: RollingInterval.Day,
                retainedFileCountLimit: Constants.Logging.RetainedFileCountLimit,
                outputTemplate: "[{Timestamp:yyyy-MM-dd HH:mm:ss.fff}] [{Level:u3}] [{SourceContext}] {Message:lj}{NewLine}{Exception}")
            .CreateLogger();

        // Configure DI
        var services = new ServiceCollection();
        ConfigureServices(services);
        Services = services.BuildServiceProvider();

        // Initialize services
        var settingsService = Services.GetRequiredService<SettingsService>();
        settingsService.Load();

        var themeService = Services.GetRequiredService<ThemeService>();
        themeService.CurrentTheme = settingsService.Settings.Theme;
        themeService.Initialize();

        // Initialize localization
        BootSentry.UI.Resources.Strings.CurrentLanguage = settingsService.Settings.Language;

        // Wire up localization resolver for non-UI layers (providers, etc.)
        Localize.SetResolver(Strings.Get);

        // Initialize tray icon service
        var trayService = Services.GetRequiredService<TrayIconService>();
        trayService.Initialize();

        Log.Information("BootSentry starting up");

        base.OnStartup(e);
    }

    protected override void OnExit(ExitEventArgs e)
    {
        Log.Information("BootSentry shutting down");

        // Dispose services
        var trayService = Services.GetService<TrayIconService>();
        trayService?.Dispose();

        var watchdog = Services.GetService<StartupWatchdog>();
        watchdog?.Dispose();

        var themeService = Services.GetService<ThemeService>();
        themeService?.Dispose();

        if (Services is IDisposable disposable)
            disposable.Dispose();

        Log.CloseAndFlush();

        _mutex?.ReleaseMutex();
        _mutex?.Dispose();

        base.OnExit(e);
    }

    private static bool AcquireSingleInstance()
    {
        _mutex = new Mutex(true, Constants.MutexName, out bool createdNew);
        if (!createdNew)
        {
            _mutex.Dispose();
            _mutex = null;
            return false;
        }
        return true;
    }

    private static void ConfigureServices(IServiceCollection services)
    {
        // Logging
        services.AddLogging(builder =>
        {
            builder.ClearProviders();
            builder.AddSerilog(dispose: true);
        });

        // Core services
        services.AddBootSentrySecurity();
        services.AddBootSentryBackup();
        services.AddBootSentryProviders();
        services.AddBootSentryActions();

        // UI Services
        services.AddSingleton<SettingsService>();
        services.AddSingleton<ISettingsService>(sp => sp.GetRequiredService<SettingsService>());
        services.AddSingleton<ThemeService>();
        services.AddSingleton<ToastService>();
        services.AddSingleton<IToastService>(sp => sp.GetRequiredService<ToastService>());
        services.AddSingleton<IDialogService, DialogService>();
        services.AddSingleton<IProcessLauncher, ProcessLauncher>();
        services.AddSingleton<IClipboardService, ClipboardService>();

        // Core services
        services.AddSingleton<IRiskService, RiskAnalyzer>();
        services.AddSingleton<SnapshotManager>();
        services.AddSingleton<ISnapshotManager>(sp => sp.GetRequiredService<SnapshotManager>());
        services.AddSingleton<StartupWatchdog>();
        services.AddSingleton<TrayIconService>();

        // Integrations
        services.AddSingleton<VirusTotalService>(sp =>
        {
            var logger = sp.GetRequiredService<ILogger<VirusTotalService>>();
            var service = new VirusTotalService(logger);

            // Configure API key if available
            var settings = sp.GetRequiredService<SettingsService>();
            if (!string.IsNullOrEmpty(settings.Settings.VirusTotalApiKey))
            {
                service.SetApiKey(settings.Settings.VirusTotalApiKey);
            }

            return service;
        });
        services.AddSingleton<IVirusTotalService>(sp => sp.GetRequiredService<VirusTotalService>());

        // Knowledge base
        services.AddSingleton<KnowledgeService>(sp =>
        {
            var service = new KnowledgeService();
            service.SeedIfEmpty();
            return service;
        });
        services.AddSingleton<IKnowledgeService>(sp => sp.GetRequiredService<KnowledgeService>());

        // ExportService (needs KnowledgeService for knowledge matching)
        services.AddSingleton<ExportService>(sp =>
        {
            var knowledgeService = sp.GetRequiredService<KnowledgeService>();
            return new ExportService((name, exe, pub) => knowledgeService.FindEntry(name, exe, pub));
        });
        services.AddSingleton<IExportService>(sp => sp.GetRequiredService<ExportService>());

        // ViewModels
        services.AddTransient<MainViewModel>();
        services.AddTransient<SnapshotViewModel>();
        services.AddTransient<EntryListViewModel>();
        services.AddTransient<SettingsViewModel>();
        services.AddTransient<HistoryViewModel>();
    }
}
