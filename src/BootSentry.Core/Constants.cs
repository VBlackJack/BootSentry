/*
 * Copyright 2025 Julien Bombled
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

namespace BootSentry.Core;

/// <summary>
/// Centralized constants for the BootSentry application.
/// All hardcoded values (URLs, paths, magic numbers, etc.) must be defined here.
/// </summary>
public static class Constants
{
    /// <summary>
    /// Application identity constants.
    /// </summary>
    public const string AppName = "BootSentry";
    public const string MutexName = @"Global\BootSentryMutex";
    public const string FallbackVersion = "1.2.0";

    /// <summary>
    /// URL constants for external links and APIs.
    /// </summary>
    public static class Urls
    {
        public const string GitHubRepository = "https://github.com/VBlackJack/BootSentry";
        public const string GitHubLicense = GitHubRepository + "/blob/main/LICENSE";
        public const string GitHubDocumentation = GitHubRepository + "#documentation";
        public const string GitHubApiBase = "https://api.github.com";
        public const string VirusTotalApi = "https://www.virustotal.com/api/v3/";
        public const string VirusTotalGui = "https://www.virustotal.com/gui/file/";
        public const string WebSearchBase = "https://www.google.com/search?q=";
    }

    /// <summary>
    /// GitHub API constants.
    /// </summary>
    public static class GitHub
    {
        public const string RepoOwner = "VBlackJack";
        public const string RepoName = "BootSentry";
        public const string AcceptHeader = "application/vnd.github.v3+json";
    }

    /// <summary>
    /// Registry key path constants.
    /// </summary>
    public static class Registry
    {
        public const string DisabledKeyPath = @"Software\BootSentry\Disabled";

        // Startup locations
        public const string RunCurrentUser = @"Software\Microsoft\Windows\CurrentVersion\Run";
        public const string RunLocalMachine = @"Software\Microsoft\Windows\CurrentVersion\Run";
        public const string RunOnceCurrentUser = @"Software\Microsoft\Windows\CurrentVersion\RunOnce";
        public const string RunOnceLocalMachine = @"Software\Microsoft\Windows\CurrentVersion\RunOnce";

        // Policies
        public const string PoliciesExplorerRun = @"Software\Microsoft\Windows\CurrentVersion\Policies\Explorer\Run";
        public const string PoliciesSystem = @"Software\Microsoft\Windows\CurrentVersion\Policies\System";

        // System areas
        public const string WinlogonPath = @"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon";
        public const string IFEOPath = @"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Image File Execution Options";
        public const string ServicesPath = @"SYSTEM\CurrentControlSet\Services";
        public const string SessionManagerPath = @"SYSTEM\CurrentControlSet\Control\Session Manager";
        public const string AppInitDlls32 = @"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Windows";
        public const string AppInitDlls64 = @"SOFTWARE\WOW6432Node\Microsoft\Windows NT\CurrentVersion\Windows";

        // Print monitors
        public const string PrintMonitorsPath = @"SYSTEM\CurrentControlSet\Control\Print\Monitors";

        // Winsock
        public const string WinsockCatalogPath = @"SYSTEM\CurrentControlSet\Services\WinSock2\Parameters\Protocol_Catalog9\Catalog_Entries";
        public const string WinsockCatalog64Path = @"SYSTEM\CurrentControlSet\Services\WinSock2\Parameters\Protocol_Catalog9\Catalog_Entries64";

        // Theme / UI detection
        public const string ThemePersonalizePath = @"Software\Microsoft\Windows\CurrentVersion\Themes\Personalize";
        public const string DesktopWindowMetrics = @"Control Panel\Desktop\WindowMetrics";
        public const string VisualEffectsPath = @"Software\Microsoft\Windows\CurrentVersion\Explorer\VisualEffects";

        // Watchdog monitored paths (full hive prefix)
        public const string WatchdogHkcuRun = @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Run";
        public const string WatchdogHklmRun = @"HKEY_LOCAL_MACHINE\Software\Microsoft\Windows\CurrentVersion\Run";
        public const string WatchdogHkcuRunOnce = @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\RunOnce";
        public const string WatchdogHklmRunOnce = @"HKEY_LOCAL_MACHINE\Software\Microsoft\Windows\CurrentVersion\RunOnce";
    }

    /// <summary>
    /// Directory and file name constants.
    /// </summary>
    public static class Directories
    {
        public const string Logs = "Logs";
        public const string Backups = "Backups";
        public const string Quarantine = "Quarantine";
        public const string StartupFolder = "StartupFolder";
        public const string Snapshots = "Snapshots";
    }

    /// <summary>
    /// File name constants.
    /// </summary>
    public static class Files
    {
        public const string Settings = "settings.json";
        public const string Manifest = "manifest.json";
        public const string ManifestHmacSuffix = ".hmac";
        public const string LogFilePattern = "bootsentry-.log";
        public const string KnowledgeDb = "knowledge.db";
    }

    /// <summary>
    /// Timeout constants.
    /// </summary>
    public static class Timeouts
    {
        public const int HttpRequestSeconds = 30;
        public const int CertificateChainValidationSeconds = 3;
    }

    /// <summary>
    /// Toast notification duration constants (milliseconds).
    /// </summary>
    public static class Toast
    {
        public const int InfoDurationMs = 2500;
        public const int SuccessDurationMs = 3500;
        public const int WarningDurationMs = 5000;
        public const int ErrorDurationMs = 6000;
        public const int DefaultDurationMs = 3000;
        public const int BalloonTipDurationMs = 5000;
    }

    /// <summary>
    /// Performance threshold constants (milliseconds).
    /// </summary>
    public static class Performance
    {
        public const long AppStartupThresholdMs = 1500;
        public const long FullScanThresholdMs = 2000;
        public const long FullScanWithHashThresholdMs = 10000;
        public const long RegistryScanThresholdMs = 300;
        public const long DisableActionThresholdMs = 500;
        public const long EnableActionThresholdMs = 500;
        public const long DeleteActionThresholdMs = 500;
        public const long HashCalculationThresholdMs = 5000;
        public const long SignatureVerificationThresholdMs = 1000;
        public const long ListRenderThresholdMs = 100;
        public const long DefaultThresholdMs = 1000;
    }

    /// <summary>
    /// Security and risk analysis constants.
    /// </summary>
    public static class Security
    {
        public const long MaxAmsiFileSizeBytes = 250 * 1024 * 1024; // 250 MB
        public const int HashBufferSize = 1024 * 1024; // 1 MB
        public const string ManifestHmacKeySalt = "BootSentry";

        /// <summary>
        /// Risk score thresholds for determining risk levels.
        /// </summary>
        public static class RiskThresholds
        {
            public const int Safe = -20;
            public const int Unknown = 10;
            public const int Suspicious = 30;
        }

        /// <summary>
        /// Individual risk factor scores.
        /// </summary>
        public static class RiskScores
        {
            // Signature factors
            public const int SignedTrusted = -30;
            public const int SignedUntrusted = 20;
            public const int Unsigned = 10;

            // Publisher factors
            public const int TrustedPublisher = -20;
            public const int UnknownPublisher = 5;

            // Location factors
            public const int SuspiciousLocation = 25;
            public const int StandardLocation = -10;

            // Command line factors
            public const int SuspiciousCommandLine = 40;

            // File existence
            public const int FileNotFound = 15;

            // Entry type factors
            public const int IFEOEntryType = 30;
            public const int WinlogonEntryType = 20;

            // Name mimicking
            public const int NameMimicking = 50;
        }

        /// <summary>
        /// Heuristic rule scores.
        /// </summary>
        public static class HeuristicScores
        {
            public const int PowerShellEncoded = 30;
            public const int PowerShellWebRequest = 40;
            public const int PowerShellBypass = 15;
            public const int PowerShellHidden = 15;
            public const int SuspiciousScript = 20;
            public const int ScriptInTemp = 40;
            public const int Rundll32JavaScript = 50;
            public const int MshtaExecution = 25;
            public const int Regsvr32Remote = 50;
            public const int CertUtilDownload = 40;
            public const int BitsadminTransfer = 30;
            public const int HiddenCommandPrompt = 20;
            public const int IpAddressInCommand = 10;
        }

        /// <summary>
        /// Trusted software publishers.
        /// </summary>
        public static readonly HashSet<string> TrustedPublishers = new(StringComparer.OrdinalIgnoreCase)
        {
            "Microsoft Corporation",
            "Microsoft Windows",
            "Google LLC",
            "Google Inc.",
            "Mozilla Corporation",
            "Adobe Inc.",
            "Adobe Systems Incorporated",
            "Intel Corporation",
            "Intel(R) Corporation",
            "NVIDIA Corporation",
            "Advanced Micro Devices, Inc.",
            "AMD",
            "Oracle Corporation",
            "Apple Inc.",
            "Valve Corporation",
            "Realtek Semiconductor Corp.",
            "Logitech",
            "Synaptics Incorporated",
            "Dell Inc.",
            "HP Inc.",
            "Lenovo",
            "ASUS",
            "Zoom Video Communications, Inc.",
            "Slack Technologies, Inc.",
            "Dropbox, Inc.",
            "Spotify AB",
            "Discord Inc.",
            "GitHub, Inc.",
            "JetBrains s.r.o.",
            "Docker Inc",
            "VMware, Inc.",
            "Citrix Systems, Inc."
        };

        /// <summary>
        /// System files commonly mimicked by malware.
        /// </summary>
        public static readonly string[] MimickedSystemFiles =
        {
            "svchost.exe", "csrss.exe", "smss.exe", "lsass.exe",
            "services.exe", "winlogon.exe", "explorer.exe",
            "taskmgr.exe", "spoolsv.exe"
        };
    }

    /// <summary>
    /// Path analysis constants.
    /// </summary>
    public static class Paths
    {
        /// <summary>
        /// Suspicious file locations indicating potential malware.
        /// </summary>
        public static readonly string[] SuspiciousLocations =
        {
            @"\temp\", @"\tmp\", @"\appdata\local\temp",
            @"\downloads\", @"\desktop\", @"\public\", @"$recycle.bin"
        };

        /// <summary>
        /// Valid system paths for system files.
        /// </summary>
        public static readonly string[] ValidSystemPaths =
        {
            @"\windows\system32\",
            @"\windows\syswow64\"
        };

        /// <summary>
        /// Standard trusted locations.
        /// </summary>
        public static readonly string[] TrustedLocations =
        {
            @"\program files\",
            @"\program files (x86)\",
            @"\windows\"
        };

        /// <summary>
        /// Gets the default Userinit path using the system directory.
        /// </summary>
        public static string DefaultUserInitPath =>
            $@"{Environment.GetFolderPath(Environment.SpecialFolder.System)}\userinit.exe,";

        /// <summary>
        /// Gets the default Shell value.
        /// </summary>
        public const string DefaultShell = "explorer.exe";
    }

    /// <summary>
    /// Browser-related constants.
    /// </summary>
    public static class Browsers
    {
        // Browser identifiers
        public const string Chrome = "Chrome";
        public const string Edge = "Edge";
        public const string Brave = "Brave";
        public const string Opera = "Opera";
        public const string OperaGX = "Opera GX";
        public const string Vivaldi = "Vivaldi";
        public const string Firefox = "Firefox";

        // Browser profile paths (relative to LocalAppData)
        public static readonly string[] ChromeProfilePath = { "Google", "Chrome", "User Data" };
        public static readonly string[] EdgeProfilePath = { "Microsoft", "Edge", "User Data" };
        public static readonly string[] BraveProfilePath = { "BraveSoftware", "Brave-Browser", "User Data" };
        public static readonly string[] OperaProfilePath = { "Opera Software", "Opera Stable" };
        public static readonly string[] OperaGXProfilePath = { "Opera Software", "Opera GX Stable" };
        public static readonly string[] VivaldiProfilePath = { "Vivaldi", "User Data" };

        // Browser policy registry paths
        public const string ChromePolicyPath = @"SOFTWARE\Policies\Google\Chrome\ExtensionInstallBlocklist";
        public const string EdgePolicyPath = @"SOFTWARE\Policies\Microsoft\Edge\ExtensionInstallBlocklist";
        public const string BravePolicyPath = @"SOFTWARE\Policies\BraveSoftware\Brave\ExtensionInstallBlocklist";
    }

    /// <summary>
    /// File extension constants.
    /// </summary>
    public static class FileExtensions
    {
        public const string Shortcut = ".lnk";
        public static readonly string[] ExecutableTypes = { ".exe", ".bat", ".cmd", ".vbs", ".ps1" };
    }

    /// <summary>
    /// Logging constants.
    /// </summary>
    public static class Logging
    {
        public const int RetainedFileCountLimit = 30;
        public const string OutputTemplate = "[{Timestamp:yyyy-MM-dd HH:mm:ss.fff}] [{Level:u3}] [{SourceContext}] {Message:lj}{NewLine}{Exception}";
    }

    /// <summary>
    /// Backup and history constants.
    /// </summary>
    public static class Backup
    {
        public const int DefaultRetentionDays = 30;
        public const int DefaultPurgeMaxCount = 100;
        public const int DefaultHistoryLimit = 500;
    }

    /// <summary>
    /// UI constants.
    /// </summary>
    public static class UI
    {
        public const int MaxAutoScanCount = 10;
    }

    /// <summary>
    /// Default settings values.
    /// </summary>
    public static class Defaults
    {
        public const string Language = "fr";
        public const int BackupRetentionDays = 30;
        public const bool HideMicrosoftEntries = true;
    }

    /// <summary>
    /// Known safe values for various system components.
    /// </summary>
    public static class KnownSafe
    {
        /// <summary>
        /// Known safe Winsock DLLs.
        /// </summary>
        public static readonly HashSet<string> WinsockDlls = new(StringComparer.OrdinalIgnoreCase)
        {
            "mswsock.dll", "napinsp.dll", "pnrpnsp.dll", "NLAapi.dll",
            "winrnr.dll", "wshbth.dll", "nwprovau.dll"
        };

        /// <summary>
        /// Known safe BootExecute values.
        /// </summary>
        public static readonly HashSet<string> BootExecuteValues = new(StringComparer.OrdinalIgnoreCase)
        {
            "autocheck autochk *"
        };

        /// <summary>
        /// Legitimate debugger executables for IFEO entries.
        /// </summary>
        public static readonly string[] LegitimateDebuggers =
        {
            "vsjitdebugger.exe", "windbg.exe", "devenv.exe", "msvsmon.exe",
            "procdump.exe", "procdump64.exe", "x64dbg.exe", "x32dbg.exe",
            "ollydbg.exe", "idaq.exe", "idaq64.exe"
        };

        /// <summary>
        /// Suspicious IFEO targets (tools commonly hijacked by malware).
        /// </summary>
        public static readonly string[] SuspiciousIFEOTargets =
        {
            "taskmgr.exe", "regedit.exe", "cmd.exe", "powershell.exe",
            "mmc.exe", "msconfig.exe", "control.exe"
        };
    }

    /// <summary>
    /// Internal log/diagnostic messages (non-user-facing).
    /// </summary>
    public static class Messages
    {
        public const string SuspiciousCommandLine = "Suspicious command line detected";
    }
}
