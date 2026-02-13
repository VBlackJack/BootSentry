using System.Text.RegularExpressions;
using BootSentry.Core.Enums;

namespace BootSentry.Core.Services;

/// <summary>
/// Advanced heuristic analyzer for detecting suspicious command line patterns and behaviors.
/// Focuses on "Living off the Land" binaries (LOLBins) and script obfuscation.
/// </summary>
public sealed class HeuristicAnalyzer
{
    private readonly List<HeuristicRule> _rules = new()
    {
        // PowerShell Obfuscation & Downloaders
        new HeuristicRule(
            "PowerShell Encoded Command",
            @"(?i)powershell.*(?:-e\s+|-enco\s+|-encodedcommand\s+)[a-zA-Z0-9+/=]{20,}",
            Constants.Security.HeuristicScores.PowerShellEncoded,
            "Potential obfuscated PowerShell script execution."),

        new HeuristicRule(
            "PowerShell Web Request",
            @"(?i)powershell.*(?:iwr|wget|curl|invoke-webrequest|net\.webclient|downloadstring|downloadfile)",
            Constants.Security.HeuristicScores.PowerShellWebRequest,
            "PowerShell script attempting network connections/downloads."),

        new HeuristicRule(
            "PowerShell Execution Policy Bypass",
            @"(?i)powershell.*-ex\s+(?:bypass|unrestricted)",
            Constants.Security.HeuristicScores.PowerShellBypass,
            "PowerShell running with execution policy bypass."),

        new HeuristicRule(
            "PowerShell Window Hiding",
            @"(?i)powershell.*-w\s+(?:h|hidden)",
            Constants.Security.HeuristicScores.PowerShellHidden,
            "PowerShell attempting to hide its window."),

        // Scripting Engines (VBS, JS, WSF)
        new HeuristicRule(
            "Suspicious Script Execution",
            @"(?i)(?:wscript|cscript)\.exe.*(?:\.vbs|\.js|\.wsf|\.jse)",
            Constants.Security.HeuristicScores.SuspiciousScript,
            "Direct execution of Windows script files."),

        new HeuristicRule(
            "Script in Temp Folder",
            @"(?i)(?:wscript|cscript|cmd|powershell)\.exe.*(?:%temp%|\\appdata\\local\\temp\\).*\.(?:vbs|js|bat|ps1)",
            Constants.Security.HeuristicScores.ScriptInTemp,
            "Execution of scripts located in temporary folders."),

        // Living off the Land Binaries (LOLBins)
        new HeuristicRule(
            "Rundll32 JavaScript Execution",
            @"(?i)rundll32\.exe.*javascript:",
            Constants.Security.HeuristicScores.Rundll32JavaScript,
            "Rundll32 used to execute JavaScript code (often malicious)."),

        new HeuristicRule(
            "Mshta HTA Execution",
            @"(?i)mshta\.exe",
            Constants.Security.HeuristicScores.MshtaExecution,
            "Mshta used to execute HTML Applications (common malware vector)."),

        new HeuristicRule(
            "Regsvr32 Remote Script",
            @"(?i)regsvr32\.exe.*/u.*/n.*/i:http",
            Constants.Security.HeuristicScores.Regsvr32Remote,
            "Regsvr32 used to fetch and execute remote script object (Squiblydoo technique)."),

        new HeuristicRule(
            "CertUtil Download",
            @"(?i)certutil\.exe.*-urlcache.*-split.*-f",
            Constants.Security.HeuristicScores.CertUtilDownload,
            "CertUtil used to download files from the internet."),

        new HeuristicRule(
            "Bitsadmin Transfer",
            @"(?i)bitsadmin.*\/transfer",
            Constants.Security.HeuristicScores.BitsadminTransfer,
            "Bitsadmin used for file transfer/download."),

        // Command Prompt & General Obfuscation
        new HeuristicRule(
            "Hidden Command Prompt",
            @"(?i)cmd\.exe.*\/c.*(?:start|min|/min)",
            Constants.Security.HeuristicScores.HiddenCommandPrompt,
            "Command prompt executing commands in hidden/minimized mode."),

        // Network (Basic IP detection)
        new HeuristicRule(
            "IP Address in Command",
            @"(?:\d{1,3}\.){3}\d{1,3}",
            Constants.Security.HeuristicScores.IpAddressInCommand,
            "Direct IP address usage in command line.")
    };

    /// <summary>
    /// Analyzes the command line of a startup entry for suspicious patterns.
    /// </summary>
    /// <param name="commandLine">The raw command line string to analyze.</param>
    /// <returns>A list of matched heuristic rules.</returns>
    public List<HeuristicMatch> Analyze(string? commandLine)
    {
        var matches = new List<HeuristicMatch>();
        
        if (string.IsNullOrWhiteSpace(commandLine))
            return matches;

        // Normalize spaces for better matching
        var normalizedCmd = Regex.Replace(commandLine, @"\s+", " ");

        foreach (var rule in _rules)
        {
            if (Regex.IsMatch(normalizedCmd, rule.Pattern))
            {
                matches.Add(new HeuristicMatch(rule.Name, rule.Score, rule.Description));
            }
        }

        return matches;
    }
}

public record HeuristicRule(string Name, string Pattern, int Score, string Description);

public record HeuristicMatch(string Name, int Score, string Description);
