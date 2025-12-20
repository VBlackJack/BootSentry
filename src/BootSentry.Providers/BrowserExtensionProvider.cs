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
    private readonly IBrowserPolicyManager? _policyManager;

    /// <summary>
    /// Database of known extension descriptions (ID -> Description).
    /// Used as fallback when manifest description is not available.
    /// </summary>
    private static readonly Dictionary<string, string> KnownExtensionDescriptions = new(StringComparer.OrdinalIgnoreCase)
    {
        // Ad blockers
        ["cjpalhdlnbpafiamejdnhcphjbkeiagm"] = "Bloqueur de publicités efficace et léger. Bloque les pubs, trackers et malwares.",
        ["cfhdojbkjhnklbpkdaibdccddilifddb"] = "Bloque les publicités intrusives sur le web.",
        ["gighmmpiobklfepjocnamgkkbiglidom"] = "Bloque les publicités sur YouTube, Facebook et partout sur le web.",
        ["epcnnfbjfcgphgdmggkamkmgojdagdnn"] = "Bloqueur de publicités ultra-rapide basé sur uBlock Origin.",

        // Password managers
        ["hdokiejnpimakedhajhdlcegeplioahd"] = "Gestionnaire de mots de passe sécurisé. Enregistre et remplit automatiquement vos identifiants.",
        ["nkbihfbeogaeaoehlefnkodbefgpgknn"] = "Portefeuille Ethereum pour interagir avec les applications blockchain.",
        ["oboonakemofpalcgghocfoadofidjkkk"] = "Gestionnaire de mots de passe open-source KeePassXC.",
        ["fdjamakpfbbddfjaooikfcpapjohcfmg"] = "Gestionnaire de mots de passe Dashlane.",
        ["naepdomgkenhinolocfifgehidddafch"] = "Gestionnaire de mots de passe Bitwarden gratuit et open-source.",
        ["eiaeiblijfjekdanodkjadfinkhbfgcd"] = "Proton Pass - Gestionnaire de mots de passe chiffré.",

        // Privacy & Security
        ["gcknhkkoolaabfmlnjonogaaifnjlfnp"] = "FoxyProxy - Configuration et gestion avancée des proxies.",
        ["pkehgijcmpdhfbdbbnkijodmdjhbjlgp"] = "Privacy Badger - Bloque automatiquement les trackers invisibles.",
        ["gcbommkclmclpchllfjekcdonpmejbdp"] = "HTTPS Everywhere - Force les connexions HTTPS sécurisées.",
        ["cmedhionkhpnakcndndgjdbohmhepckk"] = "Decentraleyes - Protège contre le tracking par les CDN.",
        ["fhcgjolkccmbidfldomjliifgaodjagh"] = "Cookie AutoDelete - Supprime automatiquement les cookies inutilisés.",
        ["ldpochfccmkkmhdbclfhpagapcfdljkj"] = "Decentraleyes - Émule les CDN localement pour la confidentialité.",
        ["bgnkhhnnamicmpeenaelnjfhikgbkllg"] = "AdGuard - Bloqueur de publicités et protection de la vie privée.",
        ["odfafepnkmbhccpbejgmiehpchacaeak"] = "uMatrix - Contrôle précis des requêtes web par type et origine.",

        // VPN & Proxy
        ["bihmplhobchoageeokmgbdihknkjbknd"] = "NordVPN - Proxy VPN rapide et sécurisé.",
        ["eppiocemhmnlbhjplcgkofciiegomcon"] = "Browsec VPN - VPN gratuit pour contourner les restrictions.",
        ["majdfhpaihoncoakbjgbdhglocklcgno"] = "Windscribe VPN - VPN et bloqueur de pubs.",
        ["ffbkglfijbcbgblgflchnbphjdllaogb"] = "Hotspot Shield VPN - Proxy VPN gratuit.",

        // Developer tools
        ["fmkadmapgofadopljbjfkapdkoienihi"] = "React Developer Tools - Outils de débogage pour React.",
        ["nhdogjmejiglipccpnnnanhbledajbpd"] = "Vue.js devtools - Outils de débogage pour Vue.js.",
        ["lmhkpmbekcpmknklioeibfkpmmfibljd"] = "Redux DevTools - Outils de débogage pour Redux.",
        ["bhlhnicpbhignbdhedgjhgdocnmhomnp"] = "ColorZilla - Pipette à couleurs et générateur de dégradés.",
        ["jdkknkkbebbapilgoeccciglkfbmbnfm"] = "Apollo Client DevTools - Outils pour GraphQL Apollo.",
        ["iaajmlceplecbljialhhkmedjlpdblhp"] = "JSON Viewer Pro - Affiche le JSON de manière formatée.",

        // Productivity
        ["aapbdbdomjkkjkaonfhkkikfgjllcleb"] = "Google Translate - Traduction instantanée de pages web.",
        ["ghbmnnjooekpmoecnnnilnnbdlolhkhi"] = "Google Docs Offline - Accès hors ligne aux documents Google.",
        ["lpcaedmchfhocbbapmcbpinfpgnhiddi"] = "Google Keep - Notes et listes de tâches.",
        ["efaidnbmnnnibpcajpcglclefindmkaj"] = "Adobe Acrobat - Lecteur et éditeur PDF.",
        ["pioclpoplcdbaefihamjohnefbikjilc"] = "Evernote Web Clipper - Sauvegarde de pages web dans Evernote.",
        ["nplieblnkhgboloddbbabmhbdakmekkh"] = "Grammarly - Correcteur orthographique et grammatical.",
        ["aomjjhallfgjeglblehebfpbcfeobpgk"] = "1Password - Gestionnaire de mots de passe.",
        ["hlepfoohegkhhmjieoechaddaejaokhf"] = "Refined GitHub - Interface GitHub améliorée.",
        ["kbfnbcaeplbcioakkpcpgfkobkghlhen"] = "Grammarly - Assistant d'écriture intelligent.",

        // Download & Media
        ["cjelfplplebdjjenllpjcblmjkfcffne"] = "Video DownloadHelper - Téléchargement de vidéos.",
        ["ajpgkpeckebdhofmmjfgcjjiiejpodla"] = "Xender - Transfert de fichiers.",
        ["ggbgaokmhkjligaokkfpojilddllkfkb"] = "Free Download Manager - Gestionnaire de téléchargements.",

        // Shopping
        ["chhjbpecpncaggjpdakmflnfcopglcmi"] = "Honey - Recherche automatique de codes promo.",
        ["hgmloofddffdnphfgcellkdfbfbjeloo"] = "RetailMeNot - Coupons et cashback.",
        ["pbjikboenpfhbbejgkoklgkhjpfogcam"] = "Amazon Assistant - Assistant shopping Amazon.",

        // Social & Communication
        ["edibdbjbnpennamipnfcpkbpgeocfceo"] = "Checker Plus for Gmail - Notifications Gmail.",
        ["oeopbcgkkoapgobdbedcemjljbihmemj"] = "Checker Plus for Google Calendar - Notifications agenda.",
        ["pnjaodmkngahhkoihejjehlcdlnohgmp"] = "Todoist - Gestionnaire de tâches et to-do list.",

        // Tab & Bookmark managers
        ["ophjlpahpchlmihnnnihgmmeilfjmjjc"] = "OneTab - Convertit tous les onglets en liste.",
        ["eggkanocgddhmamlbiijnphhppkpkmkl"] = "Tab Manager Plus - Gestion avancée des onglets.",
        ["klbibkeccnjlkjkiokjodocebajanakg"] = "The Great Suspender - Suspend les onglets inactifs.",
        ["chphlpgkkbolifaimnlloiipkdnihall"] = "OneNote Web Clipper - Sauvegarde dans OneNote.",

        // Screenshot & Screen recording
        ["liecbddmkiiihnedobmlmillhodjkdmb"] = "Loom - Enregistrement d'écran et vidéo.",
        ["mclkkofklkfljcocdinagocijmpgbhab"] = "Awesome Screenshot - Capture et annotation d'écran.",
        ["goficmpcgcnombioohjcgdhbaloknabb"] = "GoFullPage - Capture de page web complète.",
        ["alelhddbbhepgpmgidjdcjakblofbmce"] = "Full Page Screen Capture - Capture d'écran pleine page.",

        // YouTube enhancements
        ["mnjggcdmjocbbbhaepdhchncahnbgone"] = "SponsorBlock - Passe automatiquement les sponsors YouTube.",
        ["gebbhagfogifgggkldgodflihgfeippi"] = "Return YouTube Dislike - Restaure le compteur de dislikes.",
        ["cimiefiiaegbelhefglklhhakcgmhkai"] = "Plasma Integration - Intégration avec KDE Plasma.",
        ["enamippconapkdmgfgjchkhakpfinmaj"] = "DeArrow - Remplace les miniatures clickbait sur YouTube.",

        // Dark mode & Themes
        ["dmghijelimhndkbmpgbldicpogfkceaj"] = "Dark Mode - Mode sombre pour tous les sites.",
        ["pnakfcaoefokdmgdfmkolodnhifidemk"] = "Stylish - Thèmes personnalisés pour les sites web.",
        ["clngdbkpkpeebahjckkjfobafhncgmne"] = "Stylus - Gestionnaire de styles CSS personnalisés.",

        // Reading & Articles
        ["hipbfijinpcgfogaopmgehiegacbhmob"] = "Clearly - Mode lecture épuré pour les articles.",
        ["ecabifbgmdmgdllomnfinbmaellmclnh"] = "Reader View - Affichage lecture sans distractions.",
        ["gbkeegbaiigmenfmjfclcdgdpimamgkj"] = "Office Online - Word, Excel, PowerPoint dans le navigateur.",
    };

    public BrowserExtensionProvider(ILogger<BrowserExtensionProvider> logger, IBrowserPolicyManager? policyManager = null)
    {
        _logger = logger;
        _policyManager = policyManager;
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

            // Scan Brave extensions
            var braveEntries = await ScanChromiumExtensionsAsync(
                "Brave",
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                    "BraveSoftware", "Brave-Browser", "User Data"),
                cancellationToken);
            entries.AddRange(braveEntries);

            // Scan Opera extensions
            var operaEntries = await ScanChromiumExtensionsAsync(
                "Opera",
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                    "Opera Software", "Opera Stable"),
                cancellationToken);
            entries.AddRange(operaEntries);

            // Scan Opera GX extensions
            var operaGxEntries = await ScanChromiumExtensionsAsync(
                "Opera GX",
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                    "Opera Software", "Opera GX Stable"),
                cancellationToken);
            entries.AddRange(operaGxEntries);

            // Scan Vivaldi extensions
            var vivaldiEntries = await ScanChromiumExtensionsAsync(
                "Vivaldi",
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                    "Vivaldi", "User Data"),
                cancellationToken);
            entries.AddRange(vivaldiEntries);

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
            // Opera uses a flat structure, so we also check for Extensions directly in userDataPath
            var profiles = Directory.GetDirectories(userDataPath)
                .Where(d => Path.GetFileName(d) == "Default" || Path.GetFileName(d).StartsWith("Profile "))
                .ToList();

            // For Opera/Opera GX, Extensions folder is directly in the user data path
            if (profiles.Count == 0 && Directory.Exists(Path.Combine(userDataPath, "Extensions")))
            {
                profiles.Add(userDataPath);
            }

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
        var defaultLocale = GetJsonString(root, "default_locale");
        var shortName = GetJsonString(root, "short_name");
        var homepageUrl = GetJsonString(root, "homepage_url");

        // Clean up localized strings (e.g., "__MSG_appName__", "__MSG_appDesc__")
        if (name.StartsWith("__MSG_"))
            name = TryGetLocalizedString(latestVersion, name, defaultLocale) ?? name;
        if (description.StartsWith("__MSG_"))
        {
            var localizedDesc = TryGetLocalizedString(latestVersion, description, defaultLocale);
            _logger.LogDebug("Localization lookup for {Desc}: result={Result}, path={Path}",
                description, localizedDesc ?? "(null)", latestVersion);
            description = localizedDesc ?? description; // Keep original if not found
        }

        var extensionId = Path.GetFileName(extensionPath);

        // Fallback: use known descriptions database if description is empty or still localized
        if (string.IsNullOrWhiteSpace(description) || description.StartsWith("__MSG_"))
        {
            if (KnownExtensionDescriptions.TryGetValue(extensionId, out var knownDesc))
            {
                description = knownDesc;
            }
            else
            {
                // Last resort: use homepage URL or short name
                var parts = new List<string>();
                if (!string.IsNullOrEmpty(shortName) && shortName != name)
                    parts.Add(shortName);
                if (!string.IsNullOrEmpty(homepageUrl))
                    parts.Add(homepageUrl);
                if (parts.Count > 0)
                    description = string.Join(" - ", parts);
                else
                    description = $"Extension {browserName}"; // Debug fallback
            }
        }

        // Determine risk level based on permissions and origin
        var riskLevel = DetermineExtensionRisk(root, extensionId);

        // Check if extension is blocked by policy
        var isBlocked = _policyManager?.IsExtensionBlocked(extensionId, browserName) ?? false;
        var status = isBlocked ? EntryStatus.Disabled : EntryStatus.Enabled;

        return new StartupEntry
        {
            Id = $"BrowserExt_{browserName}_{extensionId}",
            DisplayName = $"[{browserName}] {name}",
            Type = EntryType.BrowserExtension,
            Scope = EntryScope.User,
            SourcePath = extensionPath,
            SourceName = extensionId,
            Publisher = author ?? "Unknown",
            Description = string.IsNullOrWhiteSpace(description) ? null : description,
            Notes = $"v{version}",
            TargetPath = extensionPath,
            CommandLineRaw = $"{browserName} Extension ({profileName})",
            Status = status,
            FileExists = true,
            RiskLevel = riskLevel,
            SignatureStatus = SignatureStatus.Unknown
        };
    }

    private string? TryGetLocalizedString(string versionPath, string msgKey, string? defaultLocale)
    {
        try
        {
            // Try to find localized messages in _locales folder
            var localesPath = Path.Combine(versionPath, "_locales");
            if (!Directory.Exists(localesPath))
            {
                _logger.LogDebug("Locales path not found: {Path}", localesPath);
                return null;
            }

            var key = msgKey.Replace("__MSG_", "").Replace("__", "");
            _logger.LogDebug("Looking for key '{Key}' in {Path}", key, localesPath);

            // Build locale priority list: default_locale first, then French, then English
            var locales = new List<string>();
            if (!string.IsNullOrEmpty(defaultLocale))
                locales.Add(defaultLocale);
            locales.AddRange(new[] { "fr", "fr_FR", "en", "en_US", "en_GB" });

            // Also try all available locales if nothing found
            var availableLocales = Directory.GetDirectories(localesPath)
                .Select(Path.GetFileName)
                .Where(l => l != null)
                .Cast<string>();

            foreach (var locale in locales.Concat(availableLocales).Distinct())
            {
                var messagesPath = Path.Combine(localesPath, locale, "messages.json");
                if (!File.Exists(messagesPath))
                    continue;

                var content = File.ReadAllText(messagesPath);
                using var doc = JsonDocument.Parse(content);

                // Try exact match first, then case-insensitive
                if (doc.RootElement.TryGetProperty(key, out var msgObj) &&
                    msgObj.TryGetProperty("message", out var message))
                {
                    _logger.LogDebug("Found '{Key}' in {Locale}: {Value}", key, locale, message.GetString());
                    return message.GetString();
                }

                // Case-insensitive search
                foreach (var prop in doc.RootElement.EnumerateObject())
                {
                    if (prop.Name.Equals(key, StringComparison.OrdinalIgnoreCase) &&
                        prop.Value.TryGetProperty("message", out var msg))
                    {
                        _logger.LogDebug("Found '{Key}' (case-insensitive) in {Locale}: {Value}", key, locale, msg.GetString());
                        return msg.GetString();
                    }
                }
            }

            _logger.LogDebug("Key '{Key}' not found in any locale", key);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Error in localization lookup for {Key}", msgKey);
        }

        return null;
    }

    private RiskLevel DetermineExtensionRisk(JsonElement manifest, string extensionId)
    {
        // Known safe extension IDs (from Chrome/Edge/Brave Web Stores)
        var knownSafeExtensions = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            // Google extensions
            "nmmhkkegccagdldgiimedpiccmgmieda", // Google Wallet
            "aapbdbdomjkkjkaonfhkkikfgjllcleb", // Google Translate
            "ghbmnnjooekpmoecnnnilnnbdlolhkhi", // Google Docs Offline
            "lpcaedmchfhocbbapmcbpinfpgnhiddi", // Google Keep

            // Ad blockers
            "cjpalhdlnbpafiamejdnhcphjbkeiagm", // uBlock Origin
            "cfhdojbkjhnklbpkdaibdccddilifddb", // Adblock Plus
            "gighmmpiobklfepjocnamgkkbiglidom", // AdBlock
            "epcnnfbjfcgphgdmggkamkmgojdagdnn", // uBlock Origin Lite
            "bgnkhhnnamicmpeenaelnjfhikgbkllg", // AdGuard

            // Password managers
            "hdokiejnpimakedhajhdlcegeplioahd", // LastPass
            "nkbihfbeogaeaoehlefnkodbefgpgknn", // MetaMask
            "oboonakemofpalcgghocfoadofidjkkk", // KeePassXC
            "fdjamakpfbbddfjaooikfcpapjohcfmg", // Dashlane
            "naepdomgkenhinolocfifgehidddafch", // Bitwarden
            "eiaeiblijfjekdanodkjadfinkhbfgcd", // Proton Pass
            "aomjjhallfgjeglblehebfpbcfeobpgk", // 1Password

            // Privacy & Security
            "pkehgijcmpdhfbdbbnkijodmdjhbjlgp", // Privacy Badger
            "gcbommkclmclpchllfjekcdonpmejbdp", // HTTPS Everywhere
            "cmedhionkhpnakcndndgjdbohmhepckk", // Decentraleyes
            "fhcgjolkccmbidfldomjliifgaodjagh", // Cookie AutoDelete

            // Productivity
            "efaidnbmnnnibpcajpcglclefindmkaj", // Adobe Acrobat
            "pioclpoplcdbaefihamjohnefbikjilc", // Evernote Web Clipper
            "nplieblnkhgboloddbbabmhbdakmekkh", // Grammarly
            "kbfnbcaeplbcioakkpcpgfkobkghlhen", // Grammarly (alt)
            "chphlpgkkbolifaimnlloiipkdnihall", // OneNote Web Clipper
            "ophjlpahpchlmihnnnihgmmeilfjmjjc", // OneTab

            // YouTube enhancements
            "mnjggcdmjocbbbhaepdhchncahnbgone", // SponsorBlock
            "gebbhagfogifgggkldgodflihgfeippi", // Return YouTube Dislike

            // Developer tools
            "fmkadmapgofadopljbjfkapdkoienihi", // React Developer Tools
            "nhdogjmejiglipccpnnnanhbledajbpd", // Vue.js devtools
            "lmhkpmbekcpmknklioeibfkpmmfibljd", // Redux DevTools
            "bhlhnicpbhignbdhedgjhgdocnmhomnp", // ColorZilla
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

        // Check if extension is blocked by policy (overrides JSON status)
        var isBlocked = id != null && (_policyManager?.IsExtensionBlocked(id, "Firefox") ?? false);
        var status = isBlocked ? EntryStatus.Disabled : (active ? EntryStatus.Enabled : EntryStatus.Disabled);

        return new StartupEntry
        {
            Id = $"BrowserExt_Firefox_{id}",
            DisplayName = $"[Firefox] {name}",
            Type = EntryType.BrowserExtension,
            Scope = EntryScope.User,
            SourcePath = "Firefox Extensions",
            SourceName = id,
            Publisher = creator ?? "Unknown",
            Description = string.IsNullOrWhiteSpace(description) ? null : description,
            Notes = $"v{version}",
            TargetPath = null,
            CommandLineRaw = $"Firefox Extension ({profileName})",
            Status = status,
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
