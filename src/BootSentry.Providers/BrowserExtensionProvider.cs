using System.Text.Json;
using Microsoft.Extensions.Logging;
using BootSentry.Core.Enums;
using BootSentry.Core.Interfaces;
using BootSentry.Core.Models;

namespace BootSentry.Providers;

/// <summary>
/// Provider that scans browser extensions (Chrome, Edge, Firefox).
/// Browser extensions can be a vector for adware, spyware, and other malware.
/// </summary>
public sealed class BrowserExtensionProvider : IStartupProvider
{
    private readonly ILogger<BrowserExtensionProvider> _logger;

    public BrowserExtensionProvider(ILogger<BrowserExtensionProvider> logger)
    {
        _logger = logger;
    }

    public EntryType EntryType => EntryType.BrowserExtension;
    public string DisplayName => "Browser Extensions";
    public bool RequiresAdminToRead => false;
    public bool RequiresAdminToModify => false;

    public bool IsAvailable() => true;

    public async Task<IReadOnlyCollection<StartupEntry>> ScanAsync(CancellationToken cancellationToken = default)
    {
        var entries = new List<StartupEntry>();

        try
        {
            // Scan Chrome extensions
            var chromeEntries = await ScanChromiumExtensionsAsync(
                "Chrome",
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                    "Google", "Chrome", "User Data"),
                cancellationToken);
            entries.AddRange(chromeEntries);

            // Scan Edge extensions
            var edgeEntries = await ScanChromiumExtensionsAsync(
                "Edge",
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                    "Microsoft", "Edge", "User Data"),
                cancellationToken);
            entries.AddRange(edgeEntries);

            // Scan Firefox extensions
            var firefoxEntries = await ScanFirefoxExtensionsAsync(cancellationToken);
            entries.AddRange(firefoxEntries);

            _logger.LogInformation("Found {Count} browser extensions", entries.Count);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error scanning browser extensions");
        }

        return entries;
    }

    private async Task<List<StartupEntry>> ScanChromiumExtensionsAsync(
        string browserName,
        string userDataPath,
        CancellationToken cancellationToken)
    {
        var entries = new List<StartupEntry>();

        if (!Directory.Exists(userDataPath))
        {
            _logger.LogDebug("{Browser} not installed at {Path}", browserName, userDataPath);
            return entries;
        }

        try
        {
            // Check all profiles (Default, Profile 1, etc.)
            var profiles = Directory.GetDirectories(userDataPath)
                .Where(d => Path.GetFileName(d) == "Default" || Path.GetFileName(d).StartsWith("Profile "));

            foreach (var profilePath in profiles)
            {
                cancellationToken.ThrowIfCancellationRequested();

                var extensionsPath = Path.Combine(profilePath, "Extensions");
                if (!Directory.Exists(extensionsPath))
                    continue;

                var profileName = Path.GetFileName(profilePath);

                foreach (var extDir in Directory.GetDirectories(extensionsPath))
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    try
                    {
                        var entry = await ParseChromiumExtensionAsync(extDir, browserName, profileName, cancellationToken);
                        if (entry != null)
                            entries.Add(entry);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning(ex, "Error parsing extension at {Path}", extDir);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error scanning {Browser} extensions", browserName);
        }

        return entries;
    }

    private async Task<StartupEntry?> ParseChromiumExtensionAsync(
        string extensionPath,
        string browserName,
        string profileName,
        CancellationToken cancellationToken)
    {
        // Find the latest version folder
        var versionDirs = Directory.GetDirectories(extensionPath);
        if (versionDirs.Length == 0)
            return null;

        var latestVersion = versionDirs.OrderByDescending(v => v).First();
        var manifestPath = Path.Combine(latestVersion, "manifest.json");

        if (!File.Exists(manifestPath))
            return null;

        var manifestContent = await File.ReadAllTextAsync(manifestPath, cancellationToken);
        using var doc = JsonDocument.Parse(manifestContent);
        var root = doc.RootElement;

        var name = GetJsonString(root, "name") ?? Path.GetFileName(extensionPath);
        var version = GetJsonString(root, "version") ?? "Unknown";
        var description = GetJsonString(root, "description") ?? "";
        var author = GetJsonString(root, "author");

        // Clean up localized names (e.g., "__MSG_appName__")
        if (name.StartsWith("__MSG_"))
            name = TryGetLocalizedName(latestVersion, name) ?? name;

        var extensionId = Path.GetFileName(extensionPath);

        // Determine risk level based on permissions and origin
        var riskLevel = DetermineExtensionRisk(root, extensionId);

        return new StartupEntry
        {
            Id = $"BrowserExt_{browserName}_{extensionId}",
            DisplayName = $"[{browserName}] {name}",
            Type = EntryType.BrowserExtension,
            Scope = EntryScope.User,
            SourcePath = extensionPath,
            SourceName = extensionId,
            Publisher = author ?? "Unknown",
            Notes = $"v{version} - {description}".Trim(),
            TargetPath = extensionPath,
            CommandLineRaw = $"{browserName} Extension ({profileName})",
            Status = EntryStatus.Enabled,
            FileExists = true,
            RiskLevel = riskLevel,
            SignatureStatus = SignatureStatus.Unknown
        };
    }

    private string? TryGetLocalizedName(string versionPath, string msgKey)
    {
        try
        {
            // Try to find _locales/en/messages.json or _locales/en_US/messages.json
            var localesPath = Path.Combine(versionPath, "_locales");
            if (!Directory.Exists(localesPath))
                return null;

            var key = msgKey.Replace("__MSG_", "").Replace("__", "");

            foreach (var locale in new[] { "en", "en_US", "en_GB" })
            {
                var messagesPath = Path.Combine(localesPath, locale, "messages.json");
                if (!File.Exists(messagesPath))
                    continue;

                var content = File.ReadAllText(messagesPath);
                using var doc = JsonDocument.Parse(content);

                if (doc.RootElement.TryGetProperty(key, out var msgObj) &&
                    msgObj.TryGetProperty("message", out var message))
                {
                    return message.GetString();
                }
            }
        }
        catch
        {
            // Ignore errors in localization lookup
        }

        return null;
    }

    private RiskLevel DetermineExtensionRisk(JsonElement manifest, string extensionId)
    {
        // Known safe extension IDs (from Chrome Web Store)
        var knownSafeExtensions = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "nmmhkkegccagdldgiimedpiccmgmieda", // Google Wallet
            "aapbdbdomjkkjkaonfhkkikfgjllcleb", // Google Translate
            "ghbmnnjooekpmoecnnnilnnbdlolhkhi", // Google Docs Offline
            "cjpalhdlnbpafiamejdnhcphjbkeiagm", // uBlock Origin
            "cfhdojbkjhnklbpkdaibdccddilifddb", // Adblock Plus
            "gighmmpiobklfepjocnamgkkbiglidom", // AdBlock
            "hdokiejnpimakedhajhdlcegeplioahd", // LastPass
            "nkbihfbeogaeaoehlefnkodbefgpgknn", // MetaMask
        };

        if (knownSafeExtensions.Contains(extensionId))
            return RiskLevel.Safe;

        // Check for dangerous permissions
        var dangerousPermissions = new[]
        {
            "webRequest", "webRequestBlocking", "debugger", "proxy",
            "nativeMessaging", "contentSettings", "downloads", "history",
            "cookies", "<all_urls>", "*://*/*"
        };

        if (manifest.TryGetProperty("permissions", out var permissions))
        {
            foreach (var perm in permissions.EnumerateArray())
            {
                var permStr = perm.GetString();
                if (permStr != null && dangerousPermissions.Any(d =>
                    permStr.Contains(d, StringComparison.OrdinalIgnoreCase)))
                {
                    return RiskLevel.Suspicious;
                }
            }
        }

        // Check host_permissions for overly broad access
        if (manifest.TryGetProperty("host_permissions", out var hostPerms))
        {
            foreach (var perm in hostPerms.EnumerateArray())
            {
                var permStr = perm.GetString();
                if (permStr != null && (permStr == "<all_urls>" || permStr == "*://*/*"))
                {
                    return RiskLevel.Unknown;
                }
            }
        }

        return RiskLevel.Unknown;
    }

    private async Task<List<StartupEntry>> ScanFirefoxExtensionsAsync(CancellationToken cancellationToken)
    {
        var entries = new List<StartupEntry>();

        var firefoxPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "Mozilla", "Firefox", "Profiles");

        if (!Directory.Exists(firefoxPath))
        {
            _logger.LogDebug("Firefox not installed");
            return entries;
        }

        try
        {
            foreach (var profileDir in Directory.GetDirectories(firefoxPath))
            {
                cancellationToken.ThrowIfCancellationRequested();

                var extensionsJsonPath = Path.Combine(profileDir, "extensions.json");
                if (!File.Exists(extensionsJsonPath))
                    continue;

                var profileName = Path.GetFileName(profileDir);

                try
                {
                    var content = await File.ReadAllTextAsync(extensionsJsonPath, cancellationToken);
                    using var doc = JsonDocument.Parse(content);

                    if (doc.RootElement.TryGetProperty("addons", out var addons))
                    {
                        foreach (var addon in addons.EnumerateArray())
                        {
                            var entry = ParseFirefoxAddon(addon, profileName);
                            if (entry != null)
                                entries.Add(entry);
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Error parsing Firefox extensions.json at {Path}", extensionsJsonPath);
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error scanning Firefox extensions");
        }

        return entries;
    }

    private StartupEntry? ParseFirefoxAddon(JsonElement addon, string profileName)
    {
        var id = GetJsonString(addon, "id");
        var name = GetJsonString(addon, "name") ?? id ?? "Unknown";
        var version = GetJsonString(addon, "version") ?? "Unknown";
        var description = GetJsonString(addon, "description") ?? "";
        var creator = GetJsonString(addon, "creator");
        var type = GetJsonString(addon, "type");

        // Skip themes and language packs
        if (type == "theme" || type == "locale" || type == "dictionary")
            return null;

        // Skip system addons
        var location = GetJsonString(addon, "location");
        if (location == "app-system-defaults" || location == "app-builtin")
            return null;

        var active = addon.TryGetProperty("active", out var activeProp) && activeProp.GetBoolean();

        return new StartupEntry
        {
            Id = $"BrowserExt_Firefox_{id}",
            DisplayName = $"[Firefox] {name}",
            Type = EntryType.BrowserExtension,
            Scope = EntryScope.User,
            SourcePath = "Firefox Extensions",
            SourceName = id,
            Publisher = creator ?? "Unknown",
            Notes = $"v{version} - {description}".Trim(),
            TargetPath = null,
            CommandLineRaw = $"Firefox Extension ({profileName})",
            Status = active ? EntryStatus.Enabled : EntryStatus.Disabled,
            FileExists = true,
            RiskLevel = RiskLevel.Unknown,
            SignatureStatus = SignatureStatus.Unknown
        };
    }

    private static string? GetJsonString(JsonElement element, string propertyName)
    {
        if (element.TryGetProperty(propertyName, out var prop) && prop.ValueKind == JsonValueKind.String)
            return prop.GetString();
        return null;
    }
}
