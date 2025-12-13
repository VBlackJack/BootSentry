using System.Diagnostics;
using Microsoft.Extensions.Logging;

namespace BootSentry.Core.Services;

/// <summary>
/// Monitors and logs performance metrics for BootSentry operations.
/// </summary>
public sealed class PerformanceMonitor
{
    private readonly ILogger<PerformanceMonitor> _logger;
    private readonly Stopwatch _appStartupWatch;
    private readonly Dictionary<string, List<long>> _operationMetrics = new();
    private DateTime _appStartTime;

    public PerformanceMonitor(ILogger<PerformanceMonitor> logger)
    {
        _logger = logger;
        _appStartupWatch = Stopwatch.StartNew();
        _appStartTime = DateTime.UtcNow;
    }

    /// <summary>
    /// Records the application startup time.
    /// </summary>
    public void RecordAppStartup()
    {
        _appStartupWatch.Stop();
        var startupMs = _appStartupWatch.ElapsedMilliseconds;

        _logger.LogInformation(
            "Application startup completed in {StartupMs}ms (Target: <1500ms) - {Status}",
            startupMs,
            startupMs < 1500 ? "OK" : "SLOW");

        RecordMetric("AppStartup", startupMs);
    }

    /// <summary>
    /// Measures the execution time of an operation.
    /// </summary>
    public MeasuredOperation Measure(string operationName)
    {
        return new MeasuredOperation(this, operationName, _logger);
    }

    /// <summary>
    /// Measures the execution time of an async operation.
    /// </summary>
    public async Task<T> MeasureAsync<T>(string operationName, Func<Task<T>> operation)
    {
        var sw = Stopwatch.StartNew();
        try
        {
            return await operation();
        }
        finally
        {
            sw.Stop();
            RecordMetric(operationName, sw.ElapsedMilliseconds);
            LogOperation(operationName, sw.ElapsedMilliseconds);
        }
    }

    /// <summary>
    /// Measures the execution time of an async operation.
    /// </summary>
    public async Task MeasureAsync(string operationName, Func<Task> operation)
    {
        var sw = Stopwatch.StartNew();
        try
        {
            await operation();
        }
        finally
        {
            sw.Stop();
            RecordMetric(operationName, sw.ElapsedMilliseconds);
            LogOperation(operationName, sw.ElapsedMilliseconds);
        }
    }

    private void RecordMetric(string operation, long elapsedMs)
    {
        if (!_operationMetrics.ContainsKey(operation))
            _operationMetrics[operation] = new List<long>();

        _operationMetrics[operation].Add(elapsedMs);
    }

    internal void RecordOperation(string operation, long elapsedMs)
    {
        RecordMetric(operation, elapsedMs);
        LogOperation(operation, elapsedMs);
    }

    private void LogOperation(string operation, long elapsedMs)
    {
        var threshold = GetThresholdForOperation(operation);
        var status = elapsedMs <= threshold ? "OK" : "SLOW";

        _logger.LogInformation(
            "Operation [{Operation}] completed in {ElapsedMs}ms (Target: <{Threshold}ms) - {Status}",
            operation, elapsedMs, threshold, status);
    }

    private static long GetThresholdForOperation(string operation) => operation switch
    {
        "AppStartup" => 1500,
        "FullScan" => 2000,
        "FullScanWithHash" => 10000,
        "RegistryScan" => 300,
        "DisableAction" => 500,
        "EnableAction" => 500,
        "DeleteAction" => 500,
        "HashCalculation" => 5000,
        "SignatureVerification" => 1000,
        "ListRender" => 100,
        _ => 1000
    };

    /// <summary>
    /// Gets a summary of all recorded metrics.
    /// </summary>
    public PerformanceSummary GetSummary()
    {
        var summary = new PerformanceSummary
        {
            AppStartTime = _appStartTime,
            Uptime = DateTime.UtcNow - _appStartTime
        };

        foreach (var (operation, times) in _operationMetrics)
        {
            if (times.Count == 0) continue;

            summary.Operations[operation] = new OperationMetrics
            {
                Count = times.Count,
                TotalMs = times.Sum(),
                AverageMs = times.Average(),
                MinMs = times.Min(),
                MaxMs = times.Max(),
                ThresholdMs = GetThresholdForOperation(operation)
            };
        }

        return summary;
    }

    /// <summary>
    /// Logs a performance summary.
    /// </summary>
    public void LogSummary()
    {
        var summary = GetSummary();

        _logger.LogInformation("=== Performance Summary ===");
        _logger.LogInformation("Uptime: {Uptime}", summary.Uptime);

        foreach (var (operation, metrics) in summary.Operations)
        {
            var status = metrics.AverageMs <= metrics.ThresholdMs ? "OK" : "NEEDS_OPTIMIZATION";
            _logger.LogInformation(
                "[{Operation}] Count={Count}, Avg={Avg:F1}ms, Min={Min}ms, Max={Max}ms - {Status}",
                operation, metrics.Count, metrics.AverageMs, metrics.MinMs, metrics.MaxMs, status);
        }
    }
}

/// <summary>
/// Represents a measured operation that records timing on disposal.
/// </summary>
public sealed class MeasuredOperation : IDisposable
{
    private readonly PerformanceMonitor _monitor;
    private readonly string _operationName;
    private readonly ILogger _logger;
    private readonly Stopwatch _stopwatch;

    internal MeasuredOperation(PerformanceMonitor monitor, string operationName, ILogger logger)
    {
        _monitor = monitor;
        _operationName = operationName;
        _logger = logger;
        _stopwatch = Stopwatch.StartNew();
    }

    public void Dispose()
    {
        _stopwatch.Stop();
        _monitor.RecordOperation(_operationName, _stopwatch.ElapsedMilliseconds);
    }
}

/// <summary>
/// Summary of performance metrics.
/// </summary>
public class PerformanceSummary
{
    public DateTime AppStartTime { get; set; }
    public TimeSpan Uptime { get; set; }
    public Dictionary<string, OperationMetrics> Operations { get; } = new();
}

/// <summary>
/// Metrics for a specific operation.
/// </summary>
public class OperationMetrics
{
    public int Count { get; set; }
    public long TotalMs { get; set; }
    public double AverageMs { get; set; }
    public long MinMs { get; set; }
    public long MaxMs { get; set; }
    public long ThresholdMs { get; set; }
}
