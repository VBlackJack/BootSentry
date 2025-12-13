using System.Text;
using System.Text.RegularExpressions;

namespace BootSentry.Core.Parsing;

/// <summary>
/// Result of parsing a command line string.
/// </summary>
public record ParsedCommandLine
{
    /// <summary>The executable path.</summary>
    public required string Executable { get; init; }

    /// <summary>The arguments (everything after the executable).</summary>
    public string? Arguments { get; init; }

    /// <summary>The original raw command line.</summary>
    public required string Raw { get; init; }

    /// <summary>The normalized command line with expanded variables.</summary>
    public required string Normalized { get; init; }
}

/// <summary>
/// Parses Windows command lines into executable and arguments.
/// Handles quoted paths, environment variables, and edge cases.
/// </summary>
public static partial class CommandLineParser
{
    /// <summary>
    /// Parses a command line string into its components.
    /// </summary>
    /// <param name="commandLine">The raw command line to parse.</param>
    /// <param name="expandEnvironmentVariables">Whether to expand environment variables.</param>
    /// <returns>Parsed command line, or null if input is empty.</returns>
    public static ParsedCommandLine? Parse(string? commandLine, bool expandEnvironmentVariables = true)
    {
        if (string.IsNullOrWhiteSpace(commandLine))
            return null;

        var raw = commandLine.Trim();
        var normalized = expandEnvironmentVariables
            ? Environment.ExpandEnvironmentVariables(raw)
            : raw;

        var (executable, arguments) = ExtractExecutableAndArguments(normalized);

        return new ParsedCommandLine
        {
            Executable = executable,
            Arguments = string.IsNullOrWhiteSpace(arguments) ? null : arguments,
            Raw = raw,
            Normalized = normalized
        };
    }

    /// <summary>
    /// Expands environment variables in a path.
    /// </summary>
    public static string ExpandEnvironmentVariables(string path)
    {
        return Environment.ExpandEnvironmentVariables(path);
    }

    /// <summary>
    /// Resolves a potentially relative or partial path to a full path.
    /// </summary>
    public static string? ResolvePath(string? path)
    {
        if (string.IsNullOrWhiteSpace(path))
            return null;

        var expanded = Environment.ExpandEnvironmentVariables(path);

        // If already absolute, return as-is
        if (Path.IsPathFullyQualified(expanded))
            return expanded;

        // Try to find in PATH
        var pathEnv = Environment.GetEnvironmentVariable("PATH");
        if (pathEnv != null)
        {
            foreach (var dir in pathEnv.Split(';', StringSplitOptions.RemoveEmptyEntries))
            {
                var fullPath = Path.Combine(dir.Trim(), expanded);
                if (File.Exists(fullPath))
                    return fullPath;

                // Try with .exe extension
                if (!expanded.EndsWith(".exe", StringComparison.OrdinalIgnoreCase))
                {
                    fullPath = Path.Combine(dir.Trim(), expanded + ".exe");
                    if (File.Exists(fullPath))
                        return fullPath;
                }
            }
        }

        // Try in System32
        var system32Path = Path.Combine(Environment.SystemDirectory, expanded);
        if (File.Exists(system32Path))
            return system32Path;

        // Return expanded path even if not found
        return expanded;
    }

    private static (string executable, string? arguments) ExtractExecutableAndArguments(string commandLine)
    {
        if (string.IsNullOrWhiteSpace(commandLine))
            return (string.Empty, null);

        commandLine = commandLine.Trim();

        // Case 1: Quoted executable
        if (commandLine.StartsWith('"'))
        {
            var endQuote = commandLine.IndexOf('"', 1);
            if (endQuote > 0)
            {
                var exe = commandLine[1..endQuote];
                var args = commandLine.Length > endQuote + 1
                    ? commandLine[(endQuote + 1)..].TrimStart()
                    : null;
                return (exe, args);
            }
        }

        // Case 2: Unquoted - find the executable by testing paths
        var parts = commandLine.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        var testPath = new StringBuilder();

        for (int i = 0; i < parts.Length; i++)
        {
            if (i > 0)
                testPath.Append(' ');
            testPath.Append(parts[i]);

            var currentPath = testPath.ToString();

            // Check if this path exists
            if (File.Exists(currentPath))
            {
                var args = i < parts.Length - 1
                    ? string.Join(' ', parts.Skip(i + 1))
                    : null;
                return (currentPath, args);
            }

            // Check with .exe extension
            if (!currentPath.EndsWith(".exe", StringComparison.OrdinalIgnoreCase))
            {
                if (File.Exists(currentPath + ".exe"))
                {
                    var args = i < parts.Length - 1
                        ? string.Join(' ', parts.Skip(i + 1))
                        : null;
                    return (currentPath + ".exe", args);
                }
            }
        }

        // Fallback: assume first space-separated part is executable
        var firstSpace = commandLine.IndexOf(' ');
        if (firstSpace > 0)
        {
            return (commandLine[..firstSpace], commandLine[(firstSpace + 1)..].TrimStart());
        }

        return (commandLine, null);
    }

    /// <summary>
    /// Checks if a command line appears to be obfuscated or suspicious.
    /// </summary>
    public static bool IsSuspiciousCommandLine(string? commandLine)
    {
        if (string.IsNullOrWhiteSpace(commandLine))
            return false;

        // Check for common obfuscation patterns
        var suspicious = new[]
        {
            // PowerShell encoded/obfuscated commands
            @"-enc\s+[A-Za-z0-9+/=]+",  // Encoded command
            @"-e\s+[A-Za-z0-9+/=]{20,}",  // Short form encoded (lowered threshold)
            @"-encodedcommand",  // Full parameter name
            @"frombase64string",  // Base64 decode
            @"invoke-expression",  // Dynamic execution
            @"\biex\b",  // Short IEX (word boundary)
            @"downloadstring",  // Download and execute
            @"webclient",  // Web download
            @"net\.webclient",  // .NET WebClient
            @"invoke-webrequest",  // PowerShell web request
            @"start-bitstransfer",  // BITS transfer

            // Suspicious PowerShell flags
            @"-windowstyle\s+hidden",  // Hidden window
            @"-w\s+hidden",  // Short form hidden
            @"-executionpolicy\s+bypass",  // Bypass execution policy
            @"-ep\s+bypass",  // Short form bypass
            @"-nop\b",  // No profile
            @"-noprofile",  // No profile full
            @"-sta\b",  // Single-threaded apartment
            @"-noninteractive",  // Non-interactive

            // CMD obfuscation
            @"\^",  // Caret obfuscation in cmd
            @"cmd\.exe.*/c.*powershell",  // CMD launching PowerShell

            // Other suspicious patterns
            @"regsvr32.*\/s.*\/u.*\/i:",  // Regsvr32 bypass
            @"mshta.*vbscript:",  // MSHTA VBScript
            @"certutil.*-urlcache",  // Certutil download
            @"bitsadmin.*/transfer",  // BITS download
        };

        var lower = commandLine.ToLowerInvariant();
        foreach (var pattern in suspicious)
        {
            if (Regex.IsMatch(lower, pattern, RegexOptions.IgnoreCase))
                return true;
        }

        return false;
    }
}
