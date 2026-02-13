using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using Microsoft.Extensions.Logging;
using BootSentry.Core.Enums;
using BootSentry.Core.Interfaces;
using BootSentry.Core.Models;
using BootSentry.Core.Parsing;

namespace BootSentry.Providers;

/// <summary>
/// Provider that scans Windows Startup folders (user and common).
/// </summary>
public sealed class StartupFolderProvider : IStartupProvider
{
    private readonly ILogger<StartupFolderProvider> _logger;
    private readonly ISignatureVerifier? _signatureVerifier;

    public StartupFolderProvider(ILogger<StartupFolderProvider> logger, ISignatureVerifier? signatureVerifier = null)
    {
        _logger = logger;
        _signatureVerifier = signatureVerifier;
    }

    public EntryType EntryType => EntryType.StartupFolder;
    public string DisplayName => "Startup Folders";
    public bool RequiresAdminToRead => false;
    public bool RequiresAdminToModify => false; // User folder, true for common folder

    public bool IsAvailable() => true;

    public async Task<IReadOnlyCollection<StartupEntry>> ScanAsync(CancellationToken cancellationToken = default)
    {
        var entries = new List<StartupEntry>();

        // User startup folder
        var userStartup = Environment.GetFolderPath(Environment.SpecialFolder.Startup);
        if (Directory.Exists(userStartup))
        {
            var userEntries = await ScanFolderAsync(userStartup, EntryScope.User, cancellationToken).ConfigureAwait(false);
            entries.AddRange(userEntries);
        }

        // Common (All Users) startup folder
        var commonStartup = Environment.GetFolderPath(Environment.SpecialFolder.CommonStartup);
        if (Directory.Exists(commonStartup))
        {
            var commonEntries = await ScanFolderAsync(commonStartup, EntryScope.Machine, cancellationToken).ConfigureAwait(false);
            entries.AddRange(commonEntries);
        }

        _logger.LogInformation("Found {Count} startup folder entries", entries.Count);
        return entries;
    }

    private async Task<List<StartupEntry>> ScanFolderAsync(
        string folderPath,
        EntryScope scope,
        CancellationToken cancellationToken)
    {
        var entries = new List<StartupEntry>();

        try
        {
            var files = Directory.GetFiles(folderPath);

            foreach (var filePath in files)
            {
                cancellationToken.ThrowIfCancellationRequested();

                try
                {
                    var entry = await CreateEntryFromFileAsync(filePath, folderPath, scope, cancellationToken).ConfigureAwait(false);
                    if (entry != null)
                        entries.Add(entry);
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Error processing startup file {File}", filePath);
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Error scanning startup folder {Folder}", folderPath);
        }

        return entries;
    }

    private async Task<StartupEntry?> CreateEntryFromFileAsync(
        string filePath,
        string folderPath,
        EntryScope scope,
        CancellationToken cancellationToken)
    {
        var fileName = Path.GetFileName(filePath);
        var extension = Path.GetExtension(filePath).ToLowerInvariant();

        string? targetPath = null;
        string? arguments = null;
        string? workingDirectory = null;
        string? commandLine = null;

        if (extension == ".lnk")
        {
            // Resolve shortcut
            var shortcutInfo = ResolveShortcut(filePath);
            if (shortcutInfo != null)
            {
                targetPath = shortcutInfo.Value.TargetPath;
                arguments = shortcutInfo.Value.Arguments;
                workingDirectory = shortcutInfo.Value.WorkingDirectory;
                commandLine = string.IsNullOrEmpty(arguments)
                    ? $"\"{targetPath}\""
                    : $"\"{targetPath}\" {arguments}";
            }
        }
        else if (extension is ".exe" or ".bat" or ".cmd" or ".vbs" or ".ps1")
        {
            // Direct executable
            targetPath = filePath;
            commandLine = $"\"{filePath}\"";
        }
        else
        {
            // Skip other file types
            return null;
        }

        var fileExists = targetPath != null && File.Exists(targetPath);
        var id = StartupEntry.GenerateId(EntryType.StartupFolder, scope, folderPath, fileName);

        var entry = new StartupEntry
        {
            Id = id,
            Type = EntryType.StartupFolder,
            Scope = scope,
            DisplayName = Path.GetFileNameWithoutExtension(fileName),
            SourcePath = folderPath,
            SourceName = fileName,
            CommandLineRaw = commandLine,
            CommandLineNormalized = commandLine,
            TargetPath = targetPath,
            Arguments = arguments,
            WorkingDirectory = workingDirectory,
            FileExists = fileExists,
            Status = EntryStatus.Enabled
        };

        // Get file metadata if target exists
        if (fileExists && targetPath != null)
        {
            await EnrichWithFileMetadataAsync(entry, targetPath, cancellationToken).ConfigureAwait(false);
        }

        return entry;
    }

    private (string TargetPath, string? Arguments, string? WorkingDirectory)? ResolveShortcut(string lnkPath)
    {
        try
        {
            // Use COM interop to resolve shortcut
            var shellLink = (IShellLinkW)new ShellLink();
            var persistFile = (IPersistFile)shellLink;

            persistFile.Load(lnkPath, 0);

            var targetPath = new StringBuilder(260);
            var findData = new WIN32_FIND_DATAW();
            shellLink.GetPath(targetPath, targetPath.Capacity, ref findData, SLGP_FLAGS.SLGP_RAWPATH);

            var arguments = new StringBuilder(1024);
            shellLink.GetArguments(arguments, arguments.Capacity);

            var workingDir = new StringBuilder(260);
            shellLink.GetWorkingDirectory(workingDir, workingDir.Capacity);

            var target = targetPath.ToString();
            if (string.IsNullOrEmpty(target))
                return null;

            return (target,
                string.IsNullOrEmpty(arguments.ToString()) ? null : arguments.ToString(),
                string.IsNullOrEmpty(workingDir.ToString()) ? null : workingDir.ToString());
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to resolve shortcut {Path}", lnkPath);
            return null;
        }
    }

    #region Shell Link COM Interop

    [ComImport]
    [Guid("00021401-0000-0000-C000-000000000046")]
    private class ShellLink { }

    [ComImport]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid("000214F9-0000-0000-C000-000000000046")]
    private interface IShellLinkW
    {
        void GetPath([Out, MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszFile, int cchMaxPath, ref WIN32_FIND_DATAW pfd, SLGP_FLAGS fFlags);
        void GetIDList(out IntPtr ppidl);
        void SetIDList(IntPtr pidl);
        void GetDescription([Out, MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszName, int cchMaxName);
        void SetDescription([MarshalAs(UnmanagedType.LPWStr)] string pszName);
        void GetWorkingDirectory([Out, MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszDir, int cchMaxPath);
        void SetWorkingDirectory([MarshalAs(UnmanagedType.LPWStr)] string pszDir);
        void GetArguments([Out, MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszArgs, int cchMaxPath);
        void SetArguments([MarshalAs(UnmanagedType.LPWStr)] string pszArgs);
        void GetHotkey(out short pwHotkey);
        void SetHotkey(short wHotkey);
        void GetShowCmd(out int piShowCmd);
        void SetShowCmd(int iShowCmd);
        void GetIconLocation([Out, MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszIconPath, int cchIconPath, out int piIcon);
        void SetIconLocation([MarshalAs(UnmanagedType.LPWStr)] string pszIconPath, int iIcon);
        void SetRelativePath([MarshalAs(UnmanagedType.LPWStr)] string pszPathRel, int dwReserved);
        void Resolve(IntPtr hwnd, int fFlags);
        void SetPath([MarshalAs(UnmanagedType.LPWStr)] string pszFile);
    }

    [Flags]
    private enum SLGP_FLAGS
    {
        SLGP_SHORTPATH = 0x1,
        SLGP_UNCPRIORITY = 0x2,
        SLGP_RAWPATH = 0x4
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    private struct WIN32_FIND_DATAW
    {
        public uint dwFileAttributes;
        public System.Runtime.InteropServices.ComTypes.FILETIME ftCreationTime;
        public System.Runtime.InteropServices.ComTypes.FILETIME ftLastAccessTime;
        public System.Runtime.InteropServices.ComTypes.FILETIME ftLastWriteTime;
        public uint nFileSizeHigh;
        public uint nFileSizeLow;
        public uint dwReserved0;
        public uint dwReserved1;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
        public string cFileName;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 14)]
        public string cAlternateFileName;
    }

    #endregion

    private async Task EnrichWithFileMetadataAsync(StartupEntry entry, string filePath, CancellationToken cancellationToken)
    {
        try
        {
            var fileInfo = new FileInfo(filePath);
            entry.FileSize = fileInfo.Length;
            entry.LastModified = fileInfo.LastWriteTime;

            var versionInfo = FileVersionInfo.GetVersionInfo(filePath);
            entry.FileVersion = versionInfo.FileVersion;
            entry.ProductName = versionInfo.ProductName;
            entry.CompanyName = versionInfo.CompanyName;
            entry.FileDescription = versionInfo.FileDescription;
            entry.Publisher = versionInfo.CompanyName;

            // Check signature if verifier is available
            if (_signatureVerifier != null)
            {
                var sigInfo = await _signatureVerifier.VerifyAsync(filePath, cancellationToken).ConfigureAwait(false);
                entry.SignatureStatus = sigInfo.Status;

                if (!string.IsNullOrEmpty(sigInfo.SignerName))
                    entry.Publisher = sigInfo.SignerName;

                // Update risk level based on signature
                if (sigInfo.Status == SignatureStatus.SignedTrusted)
                    entry.RiskLevel = RiskLevel.Safe;
                else if (sigInfo.Status == SignatureStatus.Unsigned)
                    entry.RiskLevel = RiskLevel.Unknown;
                else if (sigInfo.Status == SignatureStatus.SignedUntrusted)
                    entry.RiskLevel = RiskLevel.Suspicious;
            }
        }
        catch (Exception)
        {
            // Ignore metadata errors
        }
    }
}
