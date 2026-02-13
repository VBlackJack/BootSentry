using System.Diagnostics;
using Microsoft.Extensions.Logging;
using Microsoft.Win32;
using BootSentry.Core.Enums;
using BootSentry.Core.Interfaces;
using BootSentry.Core.Localization;
using BootSentry.Core.Models;

namespace BootSentry.Providers;

/// <summary>
/// Provider that scans Session Manager BootExecute entries.
/// These run before Windows starts and can be used for persistence.
/// Expert mode only - CRITICAL entries.
/// </summary>
public sealed class SessionManagerProvider : IStartupProvider
{
    private readonly ILogger<SessionManagerProvider> _logger;
    private readonly ISignatureVerifier? _signatureVerifier;

    private const string SessionManagerPath = @"SYSTEM\CurrentControlSet\Control\Session Manager";

    // Known safe BootExecute values
    private static readonly HashSet<string> KnownSafeValues = new(StringComparer.OrdinalIgnoreCase)
    {
        "autocheck autochk *",
        "autocheck autochk /m *",
        "autocheck autochk /p *",
    };

    public SessionManagerProvider(ILogger<SessionManagerProvider> logger, ISignatureVerifier? signatureVerifier = null)
    {
        _logger = logger;
        _signatureVerifier = signatureVerifier;
    }

    public EntryType EntryType => EntryType.SessionManager;
    public string DisplayName => "Session Manager (BootExecute)";
    public bool RequiresAdminToRead => false;
    public bool RequiresAdminToModify => true;

    public bool IsAvailable() => true;

    public async Task<IReadOnlyCollection<StartupEntry>> ScanAsync(CancellationToken cancellationToken = default)
    {
        var entries = new List<StartupEntry>();

        await Task.Run(() =>
        {
            cancellationToken.ThrowIfCancellationRequested();
            ScanBootExecute(entries);
        }, cancellationToken).ConfigureAwait(false);

        _logger.LogInformation("Found {Count} Session Manager entries", entries.Count);
        return entries;
    }

    private void ScanBootExecute(List<StartupEntry> entries)
    {
        try
        {
            using var key = Registry.LocalMachine.OpenSubKey(SessionManagerPath);
            if (key == null) return;

            // BootExecute is a REG_MULTI_SZ value
            var bootExecute = key.GetValue("BootExecute") as string[];
            if (bootExecute == null || bootExecute.Length == 0) return;

            var index = 0;
            foreach (var value in bootExecute)
            {
                if (string.IsNullOrWhiteSpace(value)) continue;

                var entry = CreateEntry(value.Trim(), index++);
                entries.Add(entry);
            }
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Error scanning Session Manager BootExecute");
        }
    }

    private StartupEntry CreateEntry(string commandLine, int index)
    {
        var sourcePath = $"HKLM\\{SessionManagerPath}";
        var id = StartupEntry.GenerateId(EntryType.SessionManager, EntryScope.Machine, sourcePath, $"BootExecute_{index}");

        // Parse the command to get the executable
        var parts = commandLine.Split(' ', 2);
        var executable = parts[0];
        var arguments = parts.Length > 1 ? parts[1] : null;

        // BootExecute programs are in System32
        var targetPath = Path.Combine(Environment.SystemDirectory, $"{executable}.exe");
        var fileExists = File.Exists(targetPath);

        var isSafe = KnownSafeValues.Contains(commandLine);

        var entry = new StartupEntry
        {
            Id = id,
            Type = EntryType.SessionManager,
            Scope = EntryScope.Machine,
            DisplayName = $"BootExecute: {executable}",
            SourcePath = sourcePath,
            SourceName = "BootExecute",
            CommandLineRaw = commandLine,
            TargetPath = fileExists ? targetPath : null,
            Arguments = arguments,
            FileExists = fileExists,
            Status = EntryStatus.Enabled,
            RiskLevel = isSafe ? RiskLevel.Safe : RiskLevel.Critical,
            IsProtected = true,
            ProtectionReason = Localize.Get("ProviderSessionCritical")
        };

        if (fileExists)
        {
            try
            {
                var fileInfo = new FileInfo(targetPath);
                entry.FileSize = fileInfo.Length;
                entry.LastModified = fileInfo.LastWriteTime;
                var versionInfo = FileVersionInfo.GetVersionInfo(targetPath);
                entry.FileVersion = versionInfo.FileVersion;
                entry.Publisher = versionInfo.CompanyName;
            }
            catch
            {
                // File info retrieval can fail for locked/inaccessible files
            }
        }

        if (!isSafe)
        {
            entry.Notes = Localize.Get("ProviderSessionNonStandard");
        }

        return entry;
    }
}
