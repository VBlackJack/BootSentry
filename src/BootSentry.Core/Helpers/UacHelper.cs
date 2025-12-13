using System.Diagnostics;
using System.Security.Principal;

namespace BootSentry.Core.Helpers;

/// <summary>
/// Helper class for User Account Control (UAC) operations.
/// </summary>
public static class UacHelper
{
    /// <summary>
    /// Checks if the current process is running with administrator privileges.
    /// </summary>
    public static bool IsRunningAsAdmin()
    {
        try
        {
            using var identity = WindowsIdentity.GetCurrent();
            var principal = new WindowsPrincipal(identity);
            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Gets the current user's SID.
    /// </summary>
    public static string GetCurrentUserSid()
    {
        try
        {
            using var identity = WindowsIdentity.GetCurrent();
            return identity.User?.Value ?? "Unknown";
        }
        catch
        {
            return "Unknown";
        }
    }

    /// <summary>
    /// Gets the current user's name.
    /// </summary>
    public static string GetCurrentUserName()
    {
        try
        {
            return Environment.UserName;
        }
        catch
        {
            return "Unknown";
        }
    }

    /// <summary>
    /// Restarts the current application with administrator privileges.
    /// </summary>
    /// <param name="arguments">Optional command line arguments.</param>
    /// <returns>True if the restart was initiated, false if cancelled or failed.</returns>
    public static bool RestartAsAdmin(string? arguments = null)
    {
        try
        {
            var processPath = Environment.ProcessPath;
            if (string.IsNullOrEmpty(processPath))
                return false;

            var startInfo = new ProcessStartInfo
            {
                FileName = processPath,
                Arguments = arguments ?? string.Empty,
                Verb = "runas",
                UseShellExecute = true
            };

            Process.Start(startInfo);
            return true;
        }
        catch (System.ComponentModel.Win32Exception)
        {
            // User cancelled UAC prompt
            return false;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Runs an external process with administrator privileges.
    /// </summary>
    /// <param name="fileName">The file to execute.</param>
    /// <param name="arguments">Optional arguments.</param>
    /// <returns>True if started successfully.</returns>
    public static bool RunAsAdmin(string fileName, string? arguments = null)
    {
        try
        {
            var startInfo = new ProcessStartInfo
            {
                FileName = fileName,
                Arguments = arguments ?? string.Empty,
                Verb = "runas",
                UseShellExecute = true
            };

            Process.Start(startInfo);
            return true;
        }
        catch
        {
            return false;
        }
    }
}
