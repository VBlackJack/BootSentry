using System.Diagnostics;
using Microsoft.Extensions.Logging;
using Microsoft.Win32.TaskScheduler;
using BootSentry.Core.Enums;
using BootSentry.Core.Interfaces;
using BootSentry.Core.Models;
using BootSentry.Core.Parsing;
using Task = System.Threading.Tasks.Task;

namespace BootSentry.Providers;

/// <summary>
/// Provider that scans Windows Scheduled Tasks for startup-related triggers.
/// </summary>
public sealed class ScheduledTaskProvider : IStartupProvider
{
    private readonly ILogger<ScheduledTaskProvider> _logger;
    private readonly ISignatureVerifier? _signatureVerifier;

    public ScheduledTaskProvider(ILogger<ScheduledTaskProvider> logger, ISignatureVerifier? signatureVerifier = null)
    {
        _logger = logger;
        _signatureVerifier = signatureVerifier;
    }

    public EntryType EntryType => EntryType.ScheduledTask;
    public string DisplayName => "Scheduled Tasks";
    public bool RequiresAdminToRead => false;
    public bool RequiresAdminToModify => true;

    public bool IsAvailable()
    {
        try
        {
            using var ts = new TaskService();
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<IReadOnlyCollection<StartupEntry>> ScanAsync(CancellationToken cancellationToken = default)
    {
        var entries = new List<StartupEntry>();

        await Task.Run(() =>
        {
            try
            {
                using var ts = new TaskService();
                ScanFolder(ts.RootFolder, entries, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error scanning scheduled tasks");
            }
        }, cancellationToken);

        _logger.LogInformation("Found {Count} scheduled task entries", entries.Count);
        return entries;
    }

    private void ScanFolder(TaskFolder folder, List<StartupEntry> entries, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        // Scan tasks in this folder
        try
        {
            foreach (var task in folder.Tasks)
            {
                cancellationToken.ThrowIfCancellationRequested();

                try
                {
                    if (HasStartupTrigger(task))
                    {
                        var entry = CreateEntry(task, folder.Path);
                        if (entry != null)
                            entries.Add(entry);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Error processing task {Task}", task.Name);
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Error enumerating tasks in folder {Folder}", folder.Path);
        }

        // Recurse into subfolders
        try
        {
            foreach (var subFolder in folder.SubFolders)
            {
                ScanFolder(subFolder, entries, cancellationToken);
            }
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Error enumerating subfolders in {Folder}", folder.Path);
        }
    }

    private static bool HasStartupTrigger(Microsoft.Win32.TaskScheduler.Task task)
    {
        try
        {
            foreach (var trigger in task.Definition.Triggers)
            {
                if (trigger.TriggerType is TaskTriggerType.Boot or
                    TaskTriggerType.Logon or
                    TaskTriggerType.SessionStateChange)
                {
                    return true;
                }
            }
        }
        catch
        {
            // Ignore access errors
        }

        return false;
    }

    private StartupEntry? CreateEntry(Microsoft.Win32.TaskScheduler.Task task, string folderPath)
    {
        var definition = task.Definition;
        var actions = definition.Actions.OfType<ExecAction>().ToList();

        if (actions.Count == 0)
            return null;

        var primaryAction = actions[0];
        var commandLine = string.IsNullOrEmpty(primaryAction.Arguments)
            ? primaryAction.Path
            : $"\"{primaryAction.Path}\" {primaryAction.Arguments}";

        var targetPath = CommandLineParser.ResolvePath(primaryAction.Path);
        var fileExists = targetPath != null && File.Exists(targetPath);

        // Determine scope
        var scope = task.Definition.Principal.LogonType == TaskLogonType.Group ||
                    task.Definition.Principal.UserId?.Contains("S-1-5-18") == true || // SYSTEM
                    task.Definition.Principal.UserId?.Contains("S-1-5-19") == true || // LOCAL SERVICE
                    task.Definition.Principal.UserId?.Contains("S-1-5-20") == true    // NETWORK SERVICE
            ? EntryScope.Machine
            : EntryScope.User;

        var taskPath = string.IsNullOrEmpty(folderPath) || folderPath == "\\"
            ? task.Name
            : $"{folderPath.TrimStart('\\')}\\{task.Name}";

        var id = StartupEntry.GenerateId(EntryType.ScheduledTask, scope, "TaskScheduler", taskPath);

        // Get trigger description
        var triggerDesc = GetTriggerDescription(definition.Triggers);

        var entry = new StartupEntry
        {
            Id = id,
            Type = EntryType.ScheduledTask,
            Scope = scope,
            DisplayName = task.Name,
            SourcePath = taskPath,
            SourceName = task.Name,
            CommandLineRaw = commandLine,
            CommandLineNormalized = Environment.ExpandEnvironmentVariables(commandLine),
            TargetPath = targetPath,
            Arguments = primaryAction.Arguments,
            WorkingDirectory = primaryAction.WorkingDirectory,
            FileExists = fileExists,
            Status = task.Enabled ? EntryStatus.Enabled : EntryStatus.Disabled,
            TaskTrigger = triggerDesc,
            LastModified = task.LastRunTime == DateTime.MinValue ? null : task.LastRunTime
        };

        // Get file metadata if target exists
        if (fileExists && targetPath != null)
        {
            EnrichWithFileMetadata(entry, targetPath);
        }

        // Check for Microsoft tasks
        if (definition.RegistrationInfo.Author?.Contains("Microsoft", StringComparison.OrdinalIgnoreCase) == true ||
            entry.Publisher?.Contains("Microsoft", StringComparison.OrdinalIgnoreCase) == true)
        {
            entry.RiskLevel = RiskLevel.Safe;
        }

        return entry;
    }

    private static string GetTriggerDescription(TriggerCollection triggers)
    {
        var descriptions = new List<string>();

        foreach (var trigger in triggers)
        {
            var desc = trigger.TriggerType switch
            {
                TaskTriggerType.Boot => "Au démarrage",
                TaskTriggerType.Logon => "À l'ouverture de session",
                TaskTriggerType.SessionStateChange => "Changement de session",
                TaskTriggerType.Idle => "Au repos",
                TaskTriggerType.Event => "Sur événement",
                TaskTriggerType.Time => "Programmé",
                TaskTriggerType.Daily => "Quotidien",
                TaskTriggerType.Weekly => "Hebdomadaire",
                TaskTriggerType.Monthly => "Mensuel",
                _ => trigger.TriggerType.ToString()
            };
            descriptions.Add(desc);
        }

        return string.Join(", ", descriptions.Distinct());
    }

    private void EnrichWithFileMetadata(StartupEntry entry, string filePath)
    {
        try
        {
            var fileInfo = new FileInfo(filePath);
            entry.FileSize = fileInfo.Length;
            entry.LastModified ??= fileInfo.LastWriteTime;

            var versionInfo = FileVersionInfo.GetVersionInfo(filePath);
            entry.FileVersion = versionInfo.FileVersion;
            entry.ProductName = versionInfo.ProductName;
            entry.CompanyName = versionInfo.CompanyName;
            entry.FileDescription = versionInfo.FileDescription;
            entry.Publisher = versionInfo.CompanyName;
        }
        catch (Exception)
        {
            // Ignore metadata errors
        }
    }
}
