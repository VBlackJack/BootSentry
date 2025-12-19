using BootSentry.Knowledge.Models;

namespace BootSentry.Knowledge.Services;

/// <summary>
/// Seeds the knowledge database with common startup entries.
/// </summary>
internal class KnowledgeSeeder
{
    private readonly KnowledgeService _service;

    public KnowledgeSeeder(KnowledgeService service)
    {
        _service = service;
    }

    public void Seed()
    {
        SeedWindowsSystem();
        SeedWindowsSecurity();
        SeedWindowsServices();
        SeedScheduledTasks();
        SeedWindowsRunEntries();
        SeedHardware();
        SeedPeripherals();
        SeedPrintersAndScanners();
        SeedDrawingTablets();
        SeedWebcamsAndMicrophones();
        SeedSecuritySoftware();
        SeedVPN();
        SeedPasswordManagers();
        SeedBackupSoftware();
        SeedRemoteDesktop();
        SeedVirtualReality();
        SeedGaming();
        SeedGameLaunchers();
        SeedProductivity();
        SeedNotesApps();
        SeedPDFSoftware();
        SeedScreenshotTools();
        SeedDevelopment();
        SeedCommunication();
        SeedEmailClients();
        SeedCloudStorage();
        SeedMedia();
        SeedBrowsers();
        SeedUtilities();
        SeedSystemTools();
        SeedRGBAndFanControl();
        SeedNetworkTools();
        SeedDisplayUtilities();
        SeedBloatware();

        // Additional entries
        SeedGamingPlatforms();
        SeedAIApps();
        SeedAdditionalBrowsers();
        SeedDevelopmentTools();
        SeedProductivityApps();
        SeedMediaAndCreative();
        SeedHardwareTools();
        SeedCommunicationApps();
        SeedMiscApps();
    }

    private void SeedWindowsSystem()
    {
        Save(new KnowledgeEntry
        {
            Name = "Windows Security Health Service",
            Aliases = "SecurityHealthService,SecurityHealthSystray",
            Publisher = "Microsoft Corporation",
            ExecutableNames = "SecurityHealthSystray.exe,SecurityHealthService.exe",
            Category = KnowledgeCategory.WindowsSecurity,
            SafetyLevel = SafetyLevel.Critical,
            ShortDescription = "Service principal de Windows Security (anciennement Windows Defender).",
            ShortDescriptionEn = "Main Windows Security service (formerly Windows Defender).",
            FullDescription = "Windows Security Health Service surveille l'état de la sécurité de votre système. Il gère l'icône de la barre des tâches qui vous alerte des problèmes de sécurité, des mises à jour antivirus et de l'état du pare-feu. Ce service est intégré à Windows 10/11 et communique avec le Centre de sécurité Windows.",
            FullDescriptionEn = "Windows Security Health Service monitors your system's security status. It manages the taskbar icon that alerts you to security issues, antivirus updates, and firewall status. This service is built into Windows 10/11 and communicates with the Windows Security Center.",
            DisableImpact = "Vous ne recevrez plus d'alertes de sécurité. L'icône de Windows Security disparaîtra de la barre des tâches. Le système pourrait devenir vulnérable sans notifications appropriées.",
            DisableImpactEn = "You will no longer receive security alerts. The Windows Security icon will disappear from the taskbar. Your system may become vulnerable without proper notifications.",
            PerformanceImpact = "Très faible (~5 Mo RAM). Impact négligeable sur les performances.",
            PerformanceImpactEn = "Very low (~5 MB RAM). Negligible performance impact.",
            Recommendation = "Ne jamais désactiver sauf si vous utilisez un autre antivirus qui le remplace.",
            RecommendationEn = "Never disable unless you use another antivirus that replaces it.",
            Tags = "windows,security,defender,antivirus,systeme",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "Windows Shell Experience Host",
            Aliases = "ShellExperienceHost",
            Publisher = "Microsoft Corporation",
            ExecutableNames = "ShellExperienceHost.exe",
            Category = KnowledgeCategory.WindowsSystem,
            SafetyLevel = SafetyLevel.Critical,
            ShortDescription = "Gère l'interface utilisateur Windows moderne (tuiles, notifications, barre des tâches).",
            ShortDescriptionEn = "Manages modern Windows UI (tiles, notifications, taskbar).",
            FullDescription = "Shell Experience Host est responsable de l'affichage des éléments visuels de l'interface Windows moderne : les tuiles du menu Démarrer, la barre des tâches, le centre de notifications et les effets de transparence (Fluent Design). Il fait partie intégrante de l'expérience utilisateur Windows 10/11.",
            FullDescriptionEn = "Shell Experience Host is responsible for displaying visual elements of the modern Windows interface: Start menu tiles, taskbar, notification center, and transparency effects (Fluent Design). It is an integral part of the Windows 10/11 user experience.",
            DisableImpact = "L'interface Windows deviendra instable. Le menu Démarrer, la barre des tâches et les notifications pourraient ne plus fonctionner correctement.",
            DisableImpactEn = "Windows interface will become unstable. Start menu, taskbar, and notifications may not work properly.",
            PerformanceImpact = "Modéré (~50-100 Mo RAM selon les effets visuels activés).",
            PerformanceImpactEn = "Moderate (~50-100 MB RAM depending on visual effects enabled).",
            Recommendation = "Ne jamais désactiver. Composant essentiel de Windows.",
            RecommendationEn = "Never disable. Essential Windows component.",
            Tags = "windows,shell,ui,interface,systeme",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "Windows Search",
            Aliases = "SearchIndexer,SearchUI,SearchApp",
            Publisher = "Microsoft Corporation",
            ExecutableNames = "SearchIndexer.exe,SearchUI.exe,SearchApp.exe",
            Category = KnowledgeCategory.WindowsSystem,
            SafetyLevel = SafetyLevel.Important,
            ShortDescription = "Service d'indexation pour la recherche Windows.",
            ShortDescriptionEn = "Windows Search indexing service.",
            FullDescription = "Windows Search indexe vos fichiers, emails (Outlook) et autres contenus pour permettre une recherche rapide. Le service maintient une base de données de tous vos fichiers et leur contenu. L'indexation se fait généralement pendant les périodes d'inactivité.",
            FullDescriptionEn = "Windows Search indexes your files, emails (Outlook), and other content for quick searching. The service maintains a database of all your files and their content. Indexing typically occurs during idle periods.",
            DisableImpact = "La recherche Windows sera très lente ou ne fonctionnera pas. La recherche dans le menu Démarrer, l'Explorateur de fichiers et Outlook sera affectée.",
            DisableImpactEn = "Windows Search will be very slow or won't work. Search in Start menu, File Explorer, and Outlook will be affected.",
            PerformanceImpact = "Peut consommer des ressources significatives pendant l'indexation (~100-500 Mo RAM, utilisation CPU lors de l'indexation). Une fois l'index créé, l'impact est minimal.",
            PerformanceImpactEn = "Can consume significant resources during indexing (~100-500 MB RAM, CPU usage during indexing). Once the index is built, impact is minimal.",
            Recommendation = "Gardez activé si vous utilisez beaucoup la recherche. Peut être désactivé sur les PC avec SSD rapide si vous n'utilisez jamais la recherche Windows.",
            RecommendationEn = "Keep enabled if you use search frequently. Can be disabled on PCs with fast SSDs if you never use Windows Search.",
            Tags = "windows,recherche,search,indexation,systeme",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "Windows Update",
            Aliases = "wuauserv,UpdateOrchestrator",
            Publisher = "Microsoft Corporation",
            ExecutableNames = "wuauclt.exe,UsoClient.exe",
            Category = KnowledgeCategory.WindowsSystem,
            SafetyLevel = SafetyLevel.Critical,
            ShortDescription = "Service de mise à jour automatique de Windows.",
            ShortDescriptionEn = "Windows automatic update service.",
            FullDescription = "Windows Update télécharge et installe les mises à jour de sécurité, les correctifs et les nouvelles fonctionnalités pour Windows. Il vérifie régulièrement les nouvelles mises à jour disponibles et peut les installer automatiquement selon vos paramètres.",
            FullDescriptionEn = "Windows Update downloads and installs security updates, patches, and new features for Windows. It regularly checks for new available updates and can install them automatically based on your settings.",
            DisableImpact = "Votre système ne recevra plus de mises à jour de sécurité, vous exposant aux vulnérabilités connues. Les failles de sécurité ne seront pas corrigées.",
            DisableImpactEn = "Your system will no longer receive security updates, exposing you to known vulnerabilities. Security flaws will not be patched.",
            PerformanceImpact = "Variable. Peut utiliser beaucoup de bande passante et CPU lors des téléchargements/installations. Minimal au repos.",
            PerformanceImpactEn = "Variable. Can use significant bandwidth and CPU during downloads/installations. Minimal when idle.",
            Recommendation = "Ne jamais désactiver. Les mises à jour de sécurité sont essentielles.",
            RecommendationEn = "Never disable. Security updates are essential.",
            Tags = "windows,update,mise a jour,securite,systeme",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "Cortana",
            Aliases = "Cortana,SearchUI",
            Publisher = "Microsoft Corporation",
            ExecutableNames = "Cortana.exe,SearchUI.exe",
            Category = KnowledgeCategory.WindowsSystem,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Assistant vocal Microsoft intégré à Windows.",
            ShortDescriptionEn = "Microsoft voice assistant built into Windows.",
            FullDescription = "Cortana est l'assistant personnel de Microsoft qui répond aux commandes vocales et textuelles. Elle peut définir des rappels, effectuer des recherches web, ouvrir des applications et répondre à des questions. Dans Windows 11, Cortana est séparée de la recherche Windows.",
            FullDescriptionEn = "Cortana is Microsoft's personal assistant that responds to voice and text commands. It can set reminders, perform web searches, open apps, and answer questions. In Windows 11, Cortana is separate from Windows Search.",
            DisableImpact = "Perte de l'assistant vocal. La recherche Windows continuera de fonctionner normalement.",
            DisableImpactEn = "Loss of voice assistant. Windows Search will continue to work normally.",
            PerformanceImpact = "Modéré (~50-100 Mo RAM). Écoute en permanence si 'Hey Cortana' est activé.",
            PerformanceImpactEn = "Moderate (~50-100 MB RAM). Constantly listening if 'Hey Cortana' is enabled.",
            Recommendation = "Peut être désactivé en toute sécurité si vous n'utilisez pas l'assistant vocal.",
            RecommendationEn = "Can be safely disabled if you don't use the voice assistant.",
            Tags = "windows,cortana,assistant,vocal,microsoft",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "Windows Widgets",
            Aliases = "Widgets,Windows Widget Platform,WidgetService",
            Publisher = "Microsoft Corporation",
            ExecutableNames = "Widgets.exe,WidgetService.exe",
            Category = KnowledgeCategory.WindowsSystem,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Panneau de widgets Windows 11 (météo, actualités, etc.).",
            ShortDescriptionEn = "Windows 11 widgets panel (weather, news, etc.).",
            FullDescription = "Les widgets Windows 11 affichent des informations en un coup d'œil : météo, actualités, calendrier, tâches, photos, etc. Accessible via le bouton Widgets dans la barre des tâches ou Win+W.",
            FullDescriptionEn = "Windows 11 widgets display at-a-glance information: weather, news, calendar, tasks, photos, etc. Accessible via the Widgets button in the taskbar or Win+W.",
            DisableImpact = "Pas d'accès au panneau de widgets. Aucun impact sur les autres fonctionnalités.",
            DisableImpactEn = "No access to widgets panel. No impact on other features.",
            PerformanceImpact = "Modéré (~50-100 Mo RAM). Consomme des ressources même en arrière-plan.",
            PerformanceImpactEn = "Moderate (~50-100 MB RAM). Consumes resources even in background.",
            Recommendation = "Peut être désactivé si vous n'utilisez pas les widgets. Économie de RAM.",
            RecommendationEn = "Can be disabled if you don't use widgets. Saves RAM.",
            Tags = "windows,widgets,meteo,actualites,win11",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "Phone Link",
            Aliases = "Your Phone,Votre Téléphone,PhoneExperienceHost",
            Publisher = "Microsoft Corporation",
            ExecutableNames = "PhoneExperienceHost.exe,YourPhone.exe",
            Category = KnowledgeCategory.WindowsSystem,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Lien entre Windows et votre smartphone Android/iPhone.",
            ShortDescriptionEn = "Link between Windows and your Android/iPhone smartphone.",
            FullDescription = "Phone Link (anciennement Votre Téléphone) permet de voir les notifications, envoyer des SMS, passer des appels et accéder aux photos de votre smartphone depuis Windows. Support complet pour Android, limité pour iPhone.",
            FullDescriptionEn = "Phone Link (formerly Your Phone) lets you view notifications, send texts, make calls, and access photos from your smartphone on Windows. Full Android support, limited iPhone support.",
            DisableImpact = "Pas de synchronisation avec le téléphone. Les notifications téléphoniques n'apparaîtront pas sur PC.",
            DisableImpactEn = "No phone synchronization. Phone notifications won't appear on PC.",
            PerformanceImpact = "Modéré (~50-100 Mo RAM).",
            PerformanceImpactEn = "Moderate (~50-100 MB RAM).",
            Recommendation = "Peut être désactivé si vous n'utilisez pas la synchronisation téléphone-PC.",
            RecommendationEn = "Can be disabled if you don't use phone-PC synchronization.",
            Tags = "windows,phone,telephone,android,iphone,sms",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "Microsoft Store",
            Aliases = "Windows Store,WinStore,Store",
            Publisher = "Microsoft Corporation",
            ExecutableNames = "WinStore.App.exe,Microsoft.WindowsStore",
            Category = KnowledgeCategory.WindowsSystem,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Boutique d'applications Microsoft.",
            ShortDescriptionEn = "Microsoft app store.",
            FullDescription = "Le Microsoft Store permet d'installer des applications, jeux et mises à jour. Il met également à jour automatiquement les applications UWP installées en arrière-plan.",
            FullDescriptionEn = "Microsoft Store lets you install apps, games, and updates. It also automatically updates installed UWP apps in the background.",
            DisableImpact = "Les applications du Store ne se mettront plus à jour automatiquement.",
            DisableImpactEn = "Store apps will no longer update automatically.",
            PerformanceImpact = "Faible au repos. Peut utiliser de la bande passante lors des mises à jour.",
            PerformanceImpactEn = "Low when idle. May use bandwidth during updates.",
            Recommendation = "Le service de mise à jour peut être configuré dans les paramètres du Store.",
            RecommendationEn = "Update service can be configured in Store settings.",
            Tags = "windows,store,applications,mise a jour,uwp",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "Xbox Game Bar",
            Aliases = "GameBar,Xbox Game Overlay,GameBarFTServer",
            Publisher = "Microsoft Corporation",
            ExecutableNames = "GameBar.exe,GameBarFTServer.exe,XboxGameBarWidget",
            Category = KnowledgeCategory.Gaming,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Barre de jeu Xbox intégrée à Windows (Win+G).",
            ShortDescriptionEn = "Xbox Game Bar built into Windows (Win+G).",
            FullDescription = "La Xbox Game Bar offre un overlay en jeu pour l'enregistrement, les captures d'écran, le chat Xbox, le monitoring des performances et les widgets. Accessible avec Win+G.",
            FullDescriptionEn = "Xbox Game Bar provides an in-game overlay for recording, screenshots, Xbox chat, performance monitoring, and widgets. Accessible with Win+G.",
            DisableImpact = "Pas d'overlay de jeu. L'enregistrement rapide (Win+Alt+R) ne fonctionnera plus.",
            DisableImpactEn = "No game overlay. Quick recording (Win+Alt+R) won't work.",
            PerformanceImpact = "Faible au repos (~20-40 Mo RAM). L'enregistrement peut impacter les FPS.",
            PerformanceImpactEn = "Low when idle (~20-40 MB RAM). Recording may impact FPS.",
            Recommendation = "Peut être désactivé si vous utilisez OBS ou ShadowPlay pour l'enregistrement.",
            RecommendationEn = "Can be disabled if you use OBS or ShadowPlay for recording.",
            Tags = "xbox,gamebar,enregistrement,capture,overlay",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "Microsoft Copilot",
            Aliases = "Windows Copilot,Copilot",
            Publisher = "Microsoft Corporation",
            ExecutableNames = "Microsoft.Copilot",
            Category = KnowledgeCategory.WindowsSystem,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Assistant IA Microsoft intégré à Windows 11.",
            ShortDescriptionEn = "Microsoft AI assistant built into Windows 11.",
            FullDescription = "Copilot est l'assistant IA de Microsoft basé sur GPT-4. Il peut répondre à des questions, générer du texte, créer des images et aider avec les tâches Windows. Accessible via la barre des tâches ou Win+C.",
            FullDescriptionEn = "Copilot is Microsoft's AI assistant powered by GPT-4. It can answer questions, generate text, create images, and help with Windows tasks. Accessible via taskbar or Win+C.",
            DisableImpact = "Pas d'accès à l'assistant IA Copilot.",
            DisableImpactEn = "No access to Copilot AI assistant.",
            PerformanceImpact = "Faible au repos. Les requêtes utilisent le cloud Microsoft.",
            PerformanceImpactEn = "Low when idle. Queries use Microsoft cloud.",
            Recommendation = "Peut être désactivé si vous n'utilisez pas l'assistant IA.",
            RecommendationEn = "Can be disabled if you don't use the AI assistant.",
            Tags = "windows,copilot,ia,assistant,gpt",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "Microsoft To Do",
            Aliases = "To Do,Microsoft ToDo",
            Publisher = "Microsoft Corporation",
            ExecutableNames = "ToDo.exe,Microsoft.Todos",
            Category = KnowledgeCategory.Productivity,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Application de gestion de tâches Microsoft.",
            ShortDescriptionEn = "Microsoft task management application.",
            FullDescription = "Microsoft To Do permet de créer des listes de tâches, définir des rappels et synchroniser avec Outlook Tasks. Intégration avec l'écosystème Microsoft 365.",
            FullDescriptionEn = "Microsoft To Do lets you create task lists, set reminders, and sync with Outlook Tasks. Integrates with the Microsoft 365 ecosystem.",
            DisableImpact = "Pas de notifications de rappels au démarrage. Les tâches restent accessibles en ligne.",
            DisableImpactEn = "No reminder notifications at startup. Tasks remain accessible online.",
            PerformanceImpact = "Faible (~30-50 Mo RAM).",
            PerformanceImpactEn = "Low (~30-50 MB RAM).",
            Recommendation = "Peut être désactivé si vous n'utilisez pas les rappels.",
            RecommendationEn = "Can be disabled if you don't use reminders.",
            Tags = "microsoft,todo,taches,rappels,productivite",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "Microsoft Outlook",
            Aliases = "Outlook,New Outlook,Outlook for Windows",
            Publisher = "Microsoft Corporation",
            ExecutableNames = "OUTLOOK.EXE,olk.exe",
            Category = KnowledgeCategory.Communication,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Client email et calendrier Microsoft.",
            ShortDescriptionEn = "Microsoft email and calendar client.",
            FullDescription = "Microsoft Outlook gère vos emails, calendrier, contacts et tâches. Le nouveau Outlook pour Windows est une version modernisée avec interface web. Outlook classique fait partie de Microsoft 365.",
            FullDescriptionEn = "Microsoft Outlook manages your emails, calendar, contacts, and tasks. The new Outlook for Windows is a modernized version with web interface. Classic Outlook is part of Microsoft 365.",
            DisableImpact = "Pas de notifications d'emails au démarrage. Outlook peut être lancé manuellement.",
            DisableImpactEn = "No email notifications at startup. Outlook can be launched manually.",
            PerformanceImpact = "Modéré à élevé (~100-300 Mo RAM).",
            PerformanceImpactEn = "Moderate to high (~100-300 MB RAM).",
            Recommendation = "Peut être désactivé si vous préférez lancer Outlook manuellement.",
            RecommendationEn = "Can be disabled if you prefer launching Outlook manually.",
            Tags = "microsoft,outlook,email,calendrier,365",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "Microsoft OneNote",
            Aliases = "OneNote,OneNote for Windows",
            Publisher = "Microsoft Corporation",
            ExecutableNames = "ONENOTE.EXE,onenoteim.exe",
            Category = KnowledgeCategory.Productivity,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Application de prise de notes Microsoft.",
            ShortDescriptionEn = "Microsoft note-taking application.",
            FullDescription = "OneNote est un bloc-notes numérique pour capturer des notes, dessins, captures d'écran et enregistrements audio. Synchronise via OneDrive avec tous vos appareils.",
            FullDescriptionEn = "OneNote is a digital notebook for capturing notes, drawings, screenshots, and audio recordings. Syncs via OneDrive with all your devices.",
            DisableImpact = "Pas de raccourci 'Envoyer à OneNote' au démarrage. OneNote peut être lancé manuellement.",
            DisableImpactEn = "No 'Send to OneNote' shortcut at startup. OneNote can be launched manually.",
            PerformanceImpact = "Modéré (~80-150 Mo RAM).",
            PerformanceImpactEn = "Moderate (~80-150 MB RAM).",
            Recommendation = "Peut être désactivé si vous n'utilisez pas le raccourci de capture.",
            RecommendationEn = "Can be disabled if you don't use the capture shortcut.",
            Tags = "microsoft,onenote,notes,365,productivite",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "Microsoft PowerPoint",
            Aliases = "PowerPoint,POWERPNT",
            Publisher = "Microsoft Corporation",
            ExecutableNames = "POWERPNT.EXE",
            Category = KnowledgeCategory.Productivity,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Application de présentations Microsoft.",
            ShortDescriptionEn = "Microsoft presentation application.",
            FullDescription = "PowerPoint permet de créer des présentations professionnelles avec diapositives, animations et transitions. Fait partie de Microsoft 365.",
            FullDescriptionEn = "PowerPoint lets you create professional presentations with slides, animations, and transitions. Part of Microsoft 365.",
            DisableImpact = "Aucun impact. PowerPoint ne devrait pas être dans le démarrage automatique.",
            DisableImpactEn = "No impact. PowerPoint shouldn't be in startup.",
            PerformanceImpact = "Aucun au repos.",
            PerformanceImpactEn = "None when idle.",
            Recommendation = "Ne devrait pas être dans le démarrage. Peut être désactivé.",
            RecommendationEn = "Should not be in startup. Can be disabled.",
            Tags = "microsoft,powerpoint,presentation,365",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "Microsoft Word",
            Aliases = "Word,WINWORD",
            Publisher = "Microsoft Corporation",
            ExecutableNames = "WINWORD.EXE",
            Category = KnowledgeCategory.Productivity,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Traitement de texte Microsoft.",
            ShortDescriptionEn = "Microsoft word processor.",
            FullDescription = "Microsoft Word est le traitement de texte standard de l'industrie pour créer des documents professionnels. Fait partie de Microsoft 365.",
            FullDescriptionEn = "Microsoft Word is the industry standard word processor for creating professional documents. Part of Microsoft 365.",
            DisableImpact = "Aucun impact. Word ne devrait pas être dans le démarrage automatique.",
            DisableImpactEn = "No impact. Word shouldn't be in startup.",
            PerformanceImpact = "Aucun au repos.",
            PerformanceImpactEn = "None when idle.",
            Recommendation = "Ne devrait pas être dans le démarrage. Peut être désactivé.",
            RecommendationEn = "Shouldn't be in startup. Can be disabled.",
            Tags = "microsoft,word,document,365,texte",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "Microsoft Excel",
            Aliases = "Excel,EXCEL",
            Publisher = "Microsoft Corporation",
            ExecutableNames = "EXCEL.EXE",
            Category = KnowledgeCategory.Productivity,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Tableur Microsoft.",
            ShortDescriptionEn = "Microsoft spreadsheet application.",
            FullDescription = "Microsoft Excel est le tableur de référence pour l'analyse de données, les graphiques et les formules. Fait partie de Microsoft 365.",
            FullDescriptionEn = "Microsoft Excel is the reference spreadsheet for data analysis, charts, and formulas. Part of Microsoft 365.",
            DisableImpact = "Aucun impact. Excel ne devrait pas être dans le démarrage automatique.",
            DisableImpactEn = "No impact. Excel shouldn't be in startup.",
            PerformanceImpact = "Aucun au repos.",
            PerformanceImpactEn = "None when idle.",
            Recommendation = "Ne devrait pas être dans le démarrage. Peut être désactivé.",
            RecommendationEn = "Shouldn't be in startup. Can be disabled.",
            Tags = "microsoft,excel,tableur,365,donnees",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "Microsoft Access",
            Aliases = "Access,MSACCESS",
            Publisher = "Microsoft Corporation",
            ExecutableNames = "MSACCESS.EXE",
            Category = KnowledgeCategory.Productivity,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Système de gestion de base de données Microsoft.",
            ShortDescriptionEn = "Microsoft database management system.",
            FullDescription = "Microsoft Access permet de créer des bases de données relationnelles avec formulaires et rapports. Fait partie de certaines versions de Microsoft 365.",
            FullDescriptionEn = "Microsoft Access lets you create relational databases with forms and reports. Part of some Microsoft 365 versions.",
            DisableImpact = "Aucun impact. Access ne devrait pas être dans le démarrage automatique.",
            DisableImpactEn = "No impact. Access shouldn't be in startup.",
            PerformanceImpact = "Aucun au repos.",
            PerformanceImpactEn = "None at idle.",
            Recommendation = "Ne devrait pas être dans le démarrage. Peut être désactivé.",
            RecommendationEn = "Shouldn't be in startup. Can be disabled.",
            Tags = "microsoft,access,base de donnees,365",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "Power Automate Desktop",
            Aliases = "Power Automate,Microsoft Flow",
            Publisher = "Microsoft Corporation",
            ExecutableNames = "PAD.Robot.exe,PAD.Console.Host.exe",
            Category = KnowledgeCategory.Productivity,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Outil d'automatisation de tâches Microsoft.",
            ShortDescriptionEn = "Microsoft task automation tool.",
            FullDescription = "Power Automate Desktop permet de créer des flux de travail automatisés sans code : automatisation de clics, remplissage de formulaires, manipulation de fichiers, etc.",
            FullDescriptionEn = "Power Automate Desktop lets you create automated workflows without code: click automation, form filling, file manipulation, etc.",
            DisableImpact = "Les flux automatisés planifiés ne s'exécuteront pas.",
            DisableImpactEn = "Scheduled automated flows won't run.",
            PerformanceImpact = "Faible au repos (~30-50 Mo RAM).",
            PerformanceImpactEn = "Low at idle (~30-50 MB RAM).",
            Recommendation = "Gardez activé si vous utilisez des flux automatisés. Peut être désactivé sinon.",
            RecommendationEn = "Keep enabled if you use automated flows. Can be disabled otherwise.",
            Tags = "microsoft,power automate,automatisation,rpa,flux",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "OneDrive",
            Aliases = "OneDrive,OneDriveSync",
            Publisher = "Microsoft Corporation",
            ExecutableNames = "OneDrive.exe,OneDriveStandaloneUpdater.exe",
            Category = KnowledgeCategory.CloudStorage,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Service de synchronisation cloud Microsoft.",
            ShortDescriptionEn = "Microsoft cloud sync service.",
            FullDescription = "OneDrive synchronise automatiquement vos fichiers avec le cloud Microsoft. Il s'intègre à l'Explorateur de fichiers et permet d'accéder à vos documents depuis n'importe quel appareil. Inclut la fonctionnalité 'Fichiers à la demande' qui ne télécharge les fichiers que lorsque nécessaire.",
            FullDescriptionEn = "OneDrive automatically syncs your files with Microsoft cloud. It integrates with File Explorer and lets you access documents from any device. Includes 'Files On-Demand' feature that only downloads files when needed.",
            DisableImpact = "Vos fichiers ne seront plus synchronisés avec le cloud. Les fichiers 'à la demande' ne seront plus accessibles.",
            DisableImpactEn = "Files will no longer sync with cloud. 'On-Demand' files won't be accessible.",
            PerformanceImpact = "Faible à modéré (~30-80 Mo RAM). Peut utiliser de la bande passante lors de la synchronisation.",
            PerformanceImpactEn = "Low to moderate (~30-80 MB RAM). May use bandwidth during sync.",
            Recommendation = "Peut être désactivé si vous n'utilisez pas OneDrive ou préférez un autre service cloud.",
            RecommendationEn = "Can be disabled if you don't use OneDrive or prefer another cloud service.",
            Tags = "microsoft,cloud,synchronisation,stockage,onedrive",
            LastUpdated = DateTime.Now
        });
    }

    private void SeedWindowsSecurity()
    {
        Save(new KnowledgeEntry
        {
            Name = "Windows Defender",
            Aliases = "MsMpEng,WinDefend,Antimalware Service Executable",
            Publisher = "Microsoft Corporation",
            ExecutableNames = "MsMpEng.exe,NisSrv.exe,MpCmdRun.exe",
            Category = KnowledgeCategory.WindowsSecurity,
            SafetyLevel = SafetyLevel.Critical,
            ShortDescription = "Antivirus intégré à Windows.",
            ShortDescriptionEn = "Built-in Windows antivirus.",
            FullDescription = "Windows Defender (maintenant Windows Security) est l'antivirus intégré de Microsoft. Il offre une protection en temps réel contre les virus, malwares, ransomwares et autres menaces. Il inclut également une protection cloud et comportementale.",
            FullDescriptionEn = "Windows Defender (now Windows Security) is Microsoft's built-in antivirus. It provides real-time protection against viruses, malware, ransomware, and other threats. Also includes cloud and behavioral protection.",
            DisableImpact = "Votre PC sera sans protection antivirus. Fortement déconseillé sauf si vous installez un autre antivirus.",
            DisableImpactEn = "Your PC will have no antivirus protection. Strongly discouraged unless you install another antivirus.",
            PerformanceImpact = "Modéré (~100-200 Mo RAM). Peut utiliser beaucoup de CPU lors des analyses.",
            PerformanceImpactEn = "Moderate (~100-200 MB RAM). May use significant CPU during scans.",
            Recommendation = "Ne jamais désactiver sauf si vous utilisez un antivirus tiers.",
            RecommendationEn = "Never disable unless using third-party antivirus.",
            Tags = "windows,defender,antivirus,securite,malware",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "Windows Firewall",
            Aliases = "MpsSvc,Windows Defender Firewall",
            Publisher = "Microsoft Corporation",
            ExecutableNames = "mpssvc.dll",
            Category = KnowledgeCategory.WindowsSecurity,
            SafetyLevel = SafetyLevel.Critical,
            ShortDescription = "Pare-feu intégré de Windows.",
            ShortDescriptionEn = "Built-in Windows firewall.",
            FullDescription = "Le pare-feu Windows filtre le trafic réseau entrant et sortant selon des règles définies. Il protège votre PC contre les accès non autorisés depuis le réseau et Internet.",
            FullDescriptionEn = "Windows Firewall filters incoming and outgoing network traffic according to defined rules. Protects your PC against unauthorized access from network and Internet.",
            DisableImpact = "Votre PC sera exposé aux attaques réseau. Aucun filtrage du trafic entrant/sortant.",
            DisableImpactEn = "Your PC will be exposed to network attacks. No incoming/outgoing traffic filtering.",
            PerformanceImpact = "Très faible. Impact négligeable sur les performances.",
            PerformanceImpactEn = "Very low. Negligible performance impact.",
            Recommendation = "Ne jamais désactiver sauf si vous utilisez un pare-feu tiers.",
            RecommendationEn = "Never disable unless using third-party firewall.",
            Tags = "windows,firewall,pare-feu,securite,reseau",
            LastUpdated = DateTime.Now
        });
    }

    private void SeedHardware()
    {
        Save(new KnowledgeEntry
        {
            Name = "NVIDIA GeForce Experience",
            Aliases = "GFExperience,NVIDIA Share,ShadowPlay",
            Publisher = "NVIDIA Corporation",
            ExecutableNames = "NVIDIA GeForce Experience.exe,nvcontainer.exe,NVIDIA Share.exe,nvsphelper64.exe",
            Category = KnowledgeCategory.Hardware,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Suite logicielle NVIDIA pour optimiser les jeux et enregistrer des vidéos.",
            ShortDescriptionEn = "NVIDIA software suite for game optimization and video recording.",
            FullDescription = "GeForce Experience optimise automatiquement les paramètres de vos jeux, met à jour les pilotes graphiques, permet l'enregistrement vidéo (ShadowPlay) et le streaming. Inclut également des filtres de jeu et la fonctionnalité NVIDIA Highlights.",
            FullDescriptionEn = "GeForce Experience automatically optimizes game settings, updates graphics drivers, enables video recording (ShadowPlay), and streaming. Also includes game filters and NVIDIA Highlights feature.",
            DisableImpact = "Pas de mise à jour automatique des pilotes. Perte de ShadowPlay et des optimisations automatiques de jeux.",
            DisableImpactEn = "No automatic driver updates. Loss of ShadowPlay and automatic game optimizations.",
            PerformanceImpact = "Modéré (~100-200 Mo RAM avec tous les services). ShadowPlay peut impacter les FPS de 2-5%.",
            PerformanceImpactEn = "Moderate (~100-200 MB RAM with all services). ShadowPlay may impact FPS by 2-5%.",
            Recommendation = "Peut être désactivé si vous mettez à jour manuellement les pilotes et n'utilisez pas ShadowPlay.",
            RecommendationEn = "Can be disabled if you update drivers manually and don't use ShadowPlay.",
            Tags = "nvidia,gpu,graphique,gaming,driver,shadowplay",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "NVIDIA App",
            Aliases = "NVIDIA Control Panel,NVDisplay.Container",
            Publisher = "NVIDIA Corporation",
            ExecutableNames = "NVIDIA App.exe,NVDisplay.Container.exe,nvcplui.exe",
            Category = KnowledgeCategory.Hardware,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Nouvelle application NVIDIA unifiée (remplace GeForce Experience).",
            ShortDescriptionEn = "New unified NVIDIA app (replaces GeForce Experience).",
            FullDescription = "NVIDIA App est le nouveau logiciel unifié qui remplace progressivement GeForce Experience. Combine les paramètres du panneau de configuration NVIDIA avec les fonctionnalités de GFE : mise à jour des pilotes, optimisation des jeux, et enregistrement.",
            FullDescriptionEn = "NVIDIA App is the new unified software gradually replacing GeForce Experience. Combines NVIDIA Control Panel settings with GFE features: driver updates, game optimization, and recording.",
            DisableImpact = "Pas d'accès aux paramètres avancés NVIDIA. Pas de mise à jour automatique.",
            DisableImpactEn = "No access to advanced NVIDIA settings. No automatic updates.",
            PerformanceImpact = "Modéré (~80-150 Mo RAM).",
            PerformanceImpactEn = "Moderate (~80-150 MB RAM).",
            Recommendation = "Peut être désactivé si vous n'utilisez pas les fonctionnalités avancées.",
            RecommendationEn = "Can be disabled if you don't use advanced features.",
            Tags = "nvidia,app,gpu,graphique,panneau configuration",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "NVIDIA Container",
            Aliases = "NVDisplay.Container,nvcontainer",
            Publisher = "NVIDIA Corporation",
            ExecutableNames = "nvcontainer.exe,NVDisplay.Container.exe",
            Category = KnowledgeCategory.Hardware,
            SafetyLevel = SafetyLevel.Important,
            ShortDescription = "Service conteneur pour les fonctionnalités NVIDIA.",
            ShortDescriptionEn = "Container service for NVIDIA features.",
            FullDescription = "NVIDIA Container héberge divers services NVIDIA : télémétrie, GameStream, Shield, et autres fonctionnalités. Plusieurs instances peuvent tourner simultanément pour différents services.",
            FullDescriptionEn = "NVIDIA Container hosts various NVIDIA services: telemetry, GameStream, Shield, and other features. Multiple instances may run simultaneously for different services.",
            DisableImpact = "Certaines fonctionnalités NVIDIA pourraient ne plus fonctionner (ShadowPlay, GameStream, etc.).",
            DisableImpactEn = "Some NVIDIA features may stop working (ShadowPlay, GameStream, etc.).",
            PerformanceImpact = "Faible à modéré (~20-50 Mo RAM par instance).",
            PerformanceImpactEn = "Low to moderate (~20-50 MB RAM per instance).",
            Recommendation = "Peut être partiellement désactivé via les services NVIDIA. Gardez le service de base.",
            RecommendationEn = "Can be partially disabled via NVIDIA services. Keep the base service.",
            Tags = "nvidia,container,service,gpu",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "NVIDIA Broadcast",
            Aliases = "RTX Broadcast,NVIDIA Noise Removal",
            Publisher = "NVIDIA Corporation",
            ExecutableNames = "NVIDIA Broadcast.exe,RTXVoice.exe",
            Category = KnowledgeCategory.Hardware,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Suite IA pour streaming : suppression de bruit, arrière-plan virtuel.",
            ShortDescriptionEn = "AI suite for streaming: noise removal, virtual background.",
            FullDescription = "NVIDIA Broadcast utilise l'IA des GPU RTX pour la suppression de bruit du micro, le remplacement d'arrière-plan sans fond vert, le suivi automatique de la caméra et l'élimination de l'écho.",
            FullDescriptionEn = "NVIDIA Broadcast uses RTX GPU AI for microphone noise removal, background replacement without green screen, auto camera tracking, and echo removal.",
            DisableImpact = "Pas d'effets IA pour le streaming. Le micro et la webcam fonctionnent normalement.",
            DisableImpactEn = "No AI effects for streaming. Microphone and webcam work normally.",
            PerformanceImpact = "Modéré (~200-400 Mo VRAM, ~50 Mo RAM). Utilise ~5-10% du GPU RTX.",
            PerformanceImpactEn = "Moderate (~200-400 MB VRAM, ~50 MB RAM). Uses ~5-10% RTX GPU.",
            Recommendation = "Peut être désactivé si vous ne faites pas de streaming ou appels vidéo.",
            RecommendationEn = "Can be disabled if you don't stream or do video calls.",
            Tags = "nvidia,broadcast,rtx,streaming,bruit,ia",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "NVIDIA Telemetry",
            Aliases = "NvTelemetry,NVIDIA Telemetry Container",
            Publisher = "NVIDIA Corporation",
            ExecutableNames = "NvTelemetryContainer.exe",
            Category = KnowledgeCategory.Bloatware,
            SafetyLevel = SafetyLevel.RecommendedDisable,
            ShortDescription = "Service de télémétrie NVIDIA (collecte de données).",
            ShortDescriptionEn = "NVIDIA telemetry service (data collection).",
            FullDescription = "Le service de télémétrie NVIDIA collecte des données anonymes sur votre utilisation du GPU et des pilotes. Ces données sont envoyées à NVIDIA pour améliorer leurs produits.",
            FullDescriptionEn = "NVIDIA telemetry service collects anonymous data about your GPU and driver usage. This data is sent to NVIDIA to improve their products.",
            DisableImpact = "Aucun impact sur le fonctionnement. Moins de données envoyées à NVIDIA.",
            DisableImpactEn = "No impact on functionality. Less data sent to NVIDIA.",
            PerformanceImpact = "Très faible (~10-20 Mo RAM).",
            PerformanceImpactEn = "Very low (~10-20 MB RAM).",
            Recommendation = "Peut être désactivé pour la confidentialité. Aucun impact sur les performances ou fonctionnalités.",
            RecommendationEn = "Can be disabled for privacy. No impact on performance or features.",
            Tags = "nvidia,telemetrie,confidentialite,donnees",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "NVIDIA FrameView",
            Aliases = "FrameView,NVIDIA Performance Overlay",
            Publisher = "NVIDIA Corporation",
            ExecutableNames = "FrameView.exe",
            Category = KnowledgeCategory.Utility,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Outil de benchmarking et monitoring de performance.",
            ShortDescriptionEn = "Performance benchmarking and monitoring tool.",
            FullDescription = "FrameView affiche les FPS, frametime, consommation électrique du GPU et autres métriques de performance en temps réel. Permet aussi de capturer des benchmarks pour analyse.",
            FullDescriptionEn = "FrameView displays FPS, frametime, GPU power consumption and other real-time performance metrics. Also captures benchmarks for analysis.",
            DisableImpact = "Pas de monitoring de performance au démarrage.",
            DisableImpactEn = "No performance monitoring at startup.",
            PerformanceImpact = "Faible (~30-50 Mo RAM).",
            PerformanceImpactEn = "Low (~30-50 MB RAM).",
            Recommendation = "Peut être désactivé. Lancez-le manuellement pour les benchmarks.",
            RecommendationEn = "Can be disabled. Launch manually for benchmarks.",
            Tags = "nvidia,frameview,benchmark,fps,monitoring",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "AMD Radeon Software",
            Aliases = "AMD Software,Radeon Settings,RadeonSoftware,AMD Adrenalin",
            Publisher = "Advanced Micro Devices, Inc.",
            ExecutableNames = "RadeonSoftware.exe,AMDRSServ.exe,aaboreanHost.exe,AMD Software.exe",
            Category = KnowledgeCategory.Hardware,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Suite logicielle AMD pour les cartes graphiques Radeon.",
            ShortDescriptionEn = "AMD software suite for Radeon graphics cards.",
            FullDescription = "Radeon Software (Adrenalin Edition) permet de configurer votre carte graphique AMD, mettre à jour les pilotes, optimiser les jeux et enregistrer des vidéos avec ReLive. Inclut des fonctionnalités comme Radeon Anti-Lag, Radeon Boost, FSR et le streaming.",
            FullDescriptionEn = "Radeon Software (Adrenalin Edition) lets you configure your AMD graphics card, update drivers, optimize games, and record videos with ReLive. Includes features like Radeon Anti-Lag, Radeon Boost, FSR, and streaming.",
            DisableImpact = "Pas d'accès aux paramètres avancés de la carte graphique. Pas de mise à jour automatique.",
            DisableImpactEn = "No access to advanced graphics card settings. No automatic updates.",
            PerformanceImpact = "Modéré (~80-150 Mo RAM).",
            PerformanceImpactEn = "Moderate (~80-150 MB RAM).",
            Recommendation = "Peut être désactivé si vous n'utilisez pas les fonctionnalités avancées.",
            RecommendationEn = "Can be disabled if you don't use advanced features.",
            Tags = "amd,radeon,gpu,graphique,driver,gaming,adrenalin",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "AMD Chipset Software",
            Aliases = "AMD Chipset Drivers,AMD Ryzen Chipset",
            Publisher = "Advanced Micro Devices, Inc.",
            ExecutableNames = "AMDRyzenMasterService.exe",
            Category = KnowledgeCategory.Hardware,
            SafetyLevel = SafetyLevel.Important,
            ShortDescription = "Pilotes de chipset AMD pour cartes mères Ryzen.",
            ShortDescriptionEn = "AMD chipset drivers for Ryzen motherboards.",
            FullDescription = "Les pilotes de chipset AMD optimisent la communication entre le processeur Ryzen et les composants de la carte mère (USB, SATA, NVMe). Essentiels pour les performances optimales sur les systèmes AMD.",
            FullDescriptionEn = "AMD chipset drivers optimize communication between Ryzen processor and motherboard components (USB, SATA, NVMe). Essential for optimal performance on AMD systems.",
            DisableImpact = "Les services du chipset restent actifs même si l'interface est désactivée.",
            DisableImpactEn = "Chipset services remain active even if interface is disabled.",
            PerformanceImpact = "Très faible (~10-20 Mo RAM).",
            PerformanceImpactEn = "Very low (~10-20 MB RAM).",
            Recommendation = "Gardez les pilotes installés. Les services au démarrage peuvent être désactivés sans impact.",
            RecommendationEn = "Keep drivers installed. Startup services can be disabled without impact.",
            Tags = "amd,chipset,ryzen,carte mere,pilote",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "AMD Ryzen Master",
            Aliases = "Ryzen Master,AMDRyzenMaster",
            Publisher = "Advanced Micro Devices, Inc.",
            ExecutableNames = "AMD Ryzen Master.exe,AMDRyzenMasterDriver.sys",
            Category = KnowledgeCategory.Utility,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Outil d'overclocking et monitoring pour processeurs Ryzen.",
            ShortDescriptionEn = "Overclocking and monitoring tool for Ryzen processors.",
            FullDescription = "AMD Ryzen Master permet d'overclocker les processeurs Ryzen, ajuster les voltages, surveiller les températures et configurer les profils de performance (Precision Boost Overdrive, Curve Optimizer).",
            FullDescriptionEn = "AMD Ryzen Master lets you overclock Ryzen processors, adjust voltages, monitor temperatures and configure performance profiles (Precision Boost Overdrive, Curve Optimizer).",
            DisableImpact = "Les profils d'overclocking ne seront pas appliqués automatiquement. Le monitoring ne sera pas disponible.",
            DisableImpactEn = "Overclocking profiles won't be applied automatically. Monitoring won't be available.",
            PerformanceImpact = "Faible (~30-50 Mo RAM).",
            PerformanceImpactEn = "Low (~30-50 MB RAM).",
            Recommendation = "Peut être désactivé si vous n'utilisez pas l'overclocking. Lancez-le manuellement si besoin.",
            RecommendationEn = "Can be disabled if you don't use overclocking. Launch manually if needed.",
            Tags = "amd,ryzen,master,overclocking,temperature,pbo",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "AMD Link",
            Aliases = "Radeon Link,AMD Streaming",
            Publisher = "Advanced Micro Devices, Inc.",
            ExecutableNames = "AMDLink.exe,aaboreanHost.exe",
            Category = KnowledgeCategory.Gaming,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Streaming de jeux AMD vers appareils mobiles.",
            ShortDescriptionEn = "AMD game streaming to mobile devices.",
            FullDescription = "AMD Link permet de streamer vos jeux PC vers smartphone, tablette ou TV. Offre également un second écran avec stats de performance et le contrôle de Radeon Software à distance.",
            FullDescriptionEn = "AMD Link streams PC games to smartphone, tablet or TV. Also provides a second screen with performance stats and remote Radeon Software control.",
            DisableImpact = "Pas de streaming vers mobile. Les jeux PC ne sont pas affectés.",
            DisableImpactEn = "No mobile streaming. PC games are not affected.",
            PerformanceImpact = "Faible au repos (~20-30 Mo RAM).",
            PerformanceImpactEn = "Low at idle (~20-30 MB RAM).",
            Recommendation = "Peut être désactivé si vous n'utilisez pas le streaming mobile.",
            RecommendationEn = "Can be disabled if you don't use mobile streaming.",
            Tags = "amd,link,streaming,mobile,gaming",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "Intel Graphics Command Center",
            Aliases = "Intel Graphics,igfxEM,igfxHK",
            Publisher = "Intel Corporation",
            ExecutableNames = "igfxEM.exe,igfxHK.exe,igfxTray.exe",
            Category = KnowledgeCategory.Hardware,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Centre de contrôle pour les graphiques Intel intégrés.",
            ShortDescriptionEn = "Control center for Intel integrated graphics.",
            FullDescription = "Permet de configurer les paramètres d'affichage, la résolution, les profils de couleur et les raccourcis clavier pour les graphiques Intel intégrés.",
            FullDescriptionEn = "Configure display settings, resolution, color profiles and keyboard shortcuts for Intel integrated graphics.",
            DisableImpact = "Perte des raccourcis clavier Intel (rotation écran, etc.). Les paramètres de base restent accessibles via Windows.",
            DisableImpactEn = "Loss of Intel keyboard shortcuts (screen rotation, etc.). Basic settings remain accessible via Windows.",
            PerformanceImpact = "Faible (~20-40 Mo RAM).",
            PerformanceImpactEn = "Low (~20-40 MB RAM).",
            Recommendation = "Peut être désactivé si vous n'utilisez pas les raccourcis ou paramètres avancés.",
            RecommendationEn = "Can be disabled if you don't use shortcuts or advanced settings.",
            Tags = "intel,gpu,graphique,integrated,driver",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "Intel Management Engine",
            Aliases = "Intel ME,Intel MEI,jhi_service",
            Publisher = "Intel Corporation",
            ExecutableNames = "jhi_service.exe,LMS.exe,IntelMeFWService.exe",
            Category = KnowledgeCategory.Hardware,
            SafetyLevel = SafetyLevel.Important,
            ShortDescription = "Service de gestion matérielle Intel intégré au processeur.",
            ShortDescriptionEn = "Intel hardware management service built into processor.",
            FullDescription = "Intel Management Engine est un sous-système matériel intégré aux processeurs Intel. Il gère des fonctions comme Intel AMT (gestion à distance), la sécurité et la gestion d'alimentation. Fonctionne indépendamment du système d'exploitation.",
            FullDescriptionEn = "Intel Management Engine is a hardware subsystem built into Intel processors. Manages functions like Intel AMT (remote management), security and power management. Runs independently of the OS.",
            DisableImpact = "Certaines fonctionnalités système peuvent ne plus fonctionner. La désactivation complète n'est pas recommandée et peut causer des instabilités.",
            DisableImpactEn = "Some system features may stop working. Full disabling is not recommended and may cause instability.",
            PerformanceImpact = "Très faible (~10-20 Mo RAM).",
            PerformanceImpactEn = "Very low (~10-20 MB RAM).",
            Recommendation = "Ne pas désactiver sauf si vous savez ce que vous faites. Composant système important.",
            RecommendationEn = "Don't disable unless you know what you're doing. Important system component.",
            Tags = "intel,management engine,amt,securite,systeme",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "Intel Rapid Storage Technology",
            Aliases = "Intel RST,IRST,iaStorIcon",
            Publisher = "Intel Corporation",
            ExecutableNames = "IAStorIcon.exe,IAStorDataMgrSvc.exe",
            Category = KnowledgeCategory.Hardware,
            SafetyLevel = SafetyLevel.Important,
            ShortDescription = "Technologie de stockage rapide Intel pour RAID et SSD.",
            ShortDescriptionEn = "Intel Rapid Storage Technology for RAID and SSD.",
            FullDescription = "Intel RST gère les configurations RAID, le caching SSD (Intel Optane) et optimise les performances de stockage. Fournit également des notifications sur l'état des disques.",
            FullDescriptionEn = "Intel RST manages RAID configurations, SSD caching (Intel Optane) and optimizes storage performance. Also provides disk health notifications.",
            DisableImpact = "Perte des notifications d'état de disque. Les configurations RAID pourraient être affectées. Le caching Optane pourrait ne plus fonctionner.",
            DisableImpactEn = "Loss of disk health notifications. RAID configurations could be affected. Optane caching may stop working.",
            PerformanceImpact = "Faible (~20-40 Mo RAM).",
            PerformanceImpactEn = "Low (~20-40 MB RAM).",
            Recommendation = "Gardez activé si vous utilisez RAID ou Intel Optane. Peut être désactivé sur les systèmes simples.",
            RecommendationEn = "Keep enabled if you use RAID or Intel Optane. Can be disabled on simple systems.",
            Tags = "intel,rst,raid,optane,stockage,ssd",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "Intel Optane Memory",
            Aliases = "Intel Optane,OptaneMemory",
            Publisher = "Intel Corporation",
            ExecutableNames = "iastorafsservice.exe,OptaneMemory.exe",
            Category = KnowledgeCategory.Hardware,
            SafetyLevel = SafetyLevel.Critical,
            ShortDescription = "Service d'accélération de stockage Intel Optane.",
            ShortDescriptionEn = "Intel Optane storage acceleration service.",
            FullDescription = "Intel Optane Memory accélère les disques durs traditionnels en utilisant un SSD Optane comme cache. Si vous avez un module Optane installé, ce service est essentiel.",
            FullDescriptionEn = "Intel Optane Memory accelerates traditional hard drives using an Optane SSD as cache. If you have an Optane module installed, this service is essential.",
            DisableImpact = "CRITIQUE si vous utilisez Optane : votre disque dur sera considérablement ralenti. Pas d'impact si vous n'avez pas d'Optane.",
            DisableImpactEn = "CRITICAL if you use Optane: your hard drive will be significantly slowed down. No impact if you don't have Optane.",
            PerformanceImpact = "Très faible (~10-20 Mo RAM).",
            PerformanceImpactEn = "Very low (~10-20 MB RAM).",
            Recommendation = "NE PAS désactiver si vous avez Intel Optane. Vérifiez dans le BIOS si Optane est actif.",
            RecommendationEn = "DO NOT disable if you have Intel Optane. Check in BIOS if Optane is active.",
            Tags = "intel,optane,cache,ssd,acceleration",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "Intel Wireless Bluetooth",
            Aliases = "Intel Bluetooth,btmshellex,IntelBluetoothService",
            Publisher = "Intel Corporation",
            ExecutableNames = "btmshellex.exe,IntelBluetoothService.exe",
            Category = KnowledgeCategory.Hardware,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Service Bluetooth pour les cartes Wi-Fi Intel.",
            ShortDescriptionEn = "Bluetooth service for Intel Wi-Fi cards.",
            FullDescription = "Gère la fonctionnalité Bluetooth des cartes Wi-Fi Intel intégrées. Fournit des fonctionnalités avancées au-delà du Bluetooth Windows standard.",
            FullDescriptionEn = "Manages Bluetooth functionality of integrated Intel Wi-Fi cards. Provides advanced features beyond standard Windows Bluetooth.",
            DisableImpact = "Le Bluetooth fonctionnera toujours via Windows mais certaines fonctionnalités Intel pourraient manquer.",
            DisableImpactEn = "Bluetooth will still work via Windows but some Intel features may be missing.",
            PerformanceImpact = "Très faible (~5-15 Mo RAM).",
            PerformanceImpactEn = "Very low (~5-15 MB RAM).",
            Recommendation = "Peut être désactivé si vous n'utilisez pas le Bluetooth ou préférez le gestionnaire Windows.",
            RecommendationEn = "Can be disabled if you don't use Bluetooth or prefer the Windows manager.",
            Tags = "intel,bluetooth,wireless,wifi,sans fil",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "Intel PROSet Wireless",
            Aliases = "Intel WiFi,Intel Wireless,PROSet",
            Publisher = "Intel Corporation",
            ExecutableNames = "IntelWifiSettingx.exe,ZeroConfigService.exe",
            Category = KnowledgeCategory.Hardware,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Utilitaire de configuration Wi-Fi Intel.",
            ShortDescriptionEn = "Intel Wi-Fi configuration utility.",
            FullDescription = "Intel PROSet offre des options de configuration Wi-Fi avancées : profils de connexion, paramètres de sécurité avancés, et diagnostics réseau.",
            FullDescriptionEn = "Intel PROSet offers advanced Wi-Fi configuration options: connection profiles, advanced security settings, and network diagnostics.",
            DisableImpact = "Le Wi-Fi fonctionne normalement via Windows. Perte des fonctionnalités avancées Intel.",
            DisableImpactEn = "Wi-Fi works normally via Windows. Loss of Intel advanced features.",
            PerformanceImpact = "Faible (~15-30 Mo RAM).",
            PerformanceImpactEn = "Low (~15-30 MB RAM).",
            Recommendation = "Peut être désactivé. Windows gère très bien le Wi-Fi sans cet outil.",
            RecommendationEn = "Can be disabled. Windows handles Wi-Fi very well without this tool.",
            Tags = "intel,wifi,wireless,proset,reseau",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "Intel Extreme Tuning Utility",
            Aliases = "Intel XTU,XTU",
            Publisher = "Intel Corporation",
            ExecutableNames = "XTU3.exe,XtuService.exe",
            Category = KnowledgeCategory.Utility,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Outil d'overclocking et de monitoring Intel.",
            ShortDescriptionEn = "Intel overclocking and monitoring tool.",
            FullDescription = "Intel XTU permet d'overclocker les processeurs Intel débloqués (série K), de surveiller les températures et voltages, et d'optimiser les performances.",
            FullDescriptionEn = "Intel XTU allows overclocking unlocked Intel processors (K series), monitoring temperatures and voltages, and optimizing performance.",
            DisableImpact = "Les profils d'overclocking ne seront pas appliqués au démarrage. Le monitoring ne sera pas disponible.",
            DisableImpactEn = "Overclocking profiles won't be applied at startup. Monitoring won't be available.",
            PerformanceImpact = "Faible (~30-50 Mo RAM).",
            PerformanceImpactEn = "Low (~30-50 MB RAM).",
            Recommendation = "Gardez activé si vous avez des profils d'overclocking. Sinon, peut être désactivé.",
            RecommendationEn = "Keep enabled if you have overclocking profiles. Otherwise, can be disabled.",
            Tags = "intel,xtu,overclocking,monitoring,temperature",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "Realtek HD Audio Manager",
            Aliases = "Realtek Audio,RtkAudioService,RAVCpl64",
            Publisher = "Realtek Semiconductor Corp.",
            ExecutableNames = "RAVCpl64.exe,RtkAudioService64.exe,RtkNGUI64.exe",
            Category = KnowledgeCategory.Hardware,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Gestionnaire audio pour les puces Realtek.",
            ShortDescriptionEn = "Audio manager for Realtek chips.",
            FullDescription = "Permet de configurer les paramètres audio avancés : égaliseur, effets sonores, configuration des haut-parleurs, détection des prises jack.",
            FullDescriptionEn = "Allows configuring advanced audio settings: equalizer, sound effects, speaker configuration, jack detection.",
            DisableImpact = "Perte de l'interface de configuration avancée. L'audio fonctionnera toujours via les paramètres Windows standards.",
            DisableImpactEn = "Loss of advanced configuration interface. Audio will still work via standard Windows settings.",
            PerformanceImpact = "Faible (~15-30 Mo RAM).",
            PerformanceImpactEn = "Low (~15-30 MB RAM).",
            Recommendation = "Peut être désactivé si vous n'utilisez que les paramètres audio Windows.",
            RecommendationEn = "Can be disabled if you only use Windows audio settings.",
            Tags = "realtek,audio,son,driver,hardware",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "Synaptics Touchpad",
            Aliases = "SynTPEnh,Synaptics Pointing Device Driver",
            Publisher = "Synaptics Incorporated",
            ExecutableNames = "SynTPEnh.exe,SynTPHelper.exe",
            Category = KnowledgeCategory.Hardware,
            SafetyLevel = SafetyLevel.Important,
            ShortDescription = "Pilote et utilitaire pour les touchpads Synaptics.",
            ShortDescriptionEn = "Driver and utility for Synaptics touchpads.",
            FullDescription = "Gère les gestes multi-touch, le défilement, le zoom et autres fonctionnalités avancées des touchpads Synaptics présents sur de nombreux laptops.",
            FullDescriptionEn = "Manages multi-touch gestures, scrolling, zoom and other advanced features of Synaptics touchpads found on many laptops.",
            DisableImpact = "Le touchpad fonctionnera mais avec des fonctionnalités réduites. Les gestes avancés pourraient ne plus fonctionner.",
            DisableImpactEn = "Touchpad will work but with reduced functionality. Advanced gestures may stop working.",
            PerformanceImpact = "Faible (~10-20 Mo RAM).",
            PerformanceImpactEn = "Low (~10-20 MB RAM).",
            Recommendation = "Gardez activé sur les laptops. Non nécessaire sur les PC de bureau.",
            RecommendationEn = "Keep enabled on laptops. Not needed on desktop PCs.",
            Tags = "synaptics,touchpad,laptop,gestes,driver",
            LastUpdated = DateTime.Now
        });
    }

    private void SeedSecuritySoftware()
    {
        Save(new KnowledgeEntry
        {
            Name = "Norton Security",
            Aliases = "Norton 360,Norton AntiVirus,NortonSecurity",
            Publisher = "NortonLifeLock Inc.",
            ExecutableNames = "Norton.exe,NortonSecurity.exe,ccSvcHst.exe",
            Category = KnowledgeCategory.Security,
            SafetyLevel = SafetyLevel.Important,
            ShortDescription = "Suite de sécurité complète Norton.",
            ShortDescriptionEn = "Complete Norton security suite.",
            FullDescription = "Norton offre une protection antivirus, un pare-feu, un VPN, un gestionnaire de mots de passe et une protection de l'identité. C'est une suite de sécurité complète payante.",
            FullDescriptionEn = "Norton offers antivirus protection, firewall, VPN, password manager and identity protection. It's a complete paid security suite.",
            DisableImpact = "Perte de la protection antivirus en temps réel. Windows Defender prendra le relais si Norton est complètement désactivé.",
            DisableImpactEn = "Loss of real-time antivirus protection. Windows Defender will take over if Norton is completely disabled.",
            PerformanceImpact = "Modéré à élevé (~150-300 Mo RAM). Peut ralentir les analyses de fichiers.",
            PerformanceImpactEn = "Moderate to high (~150-300 MB RAM). May slow down file scans.",
            Recommendation = "Gardez activé si vous avez payé pour Norton. Sinon, désinstallez complètement et utilisez Windows Defender.",
            RecommendationEn = "Keep enabled if you paid for Norton. Otherwise, uninstall completely and use Windows Defender.",
            Tags = "norton,antivirus,securite,firewall,vpn",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "McAfee",
            Aliases = "McAfee Security,McAfee Total Protection,McAfee LiveSafe",
            Publisher = "McAfee, LLC",
            ExecutableNames = "mcshield.exe,McUICnt.exe,mfemms.exe",
            Category = KnowledgeCategory.Security,
            SafetyLevel = SafetyLevel.Important,
            ShortDescription = "Suite de sécurité McAfee.",
            ShortDescriptionEn = "McAfee security suite.",
            FullDescription = "McAfee offre une protection antivirus, un pare-feu et diverses fonctionnalités de sécurité. Souvent préinstallé sur les PC de marque.",
            FullDescriptionEn = "McAfee offers antivirus protection, firewall and various security features. Often pre-installed on brand PCs.",
            DisableImpact = "Perte de la protection en temps réel.",
            DisableImpactEn = "Loss of real-time protection.",
            PerformanceImpact = "Modéré à élevé (~200-400 Mo RAM). Connu pour impacter les performances.",
            PerformanceImpactEn = "Moderate to high (~200-400 MB RAM). Known to impact performance.",
            Recommendation = "Si vous ne l'avez pas acheté volontairement, envisagez de le désinstaller et d'utiliser Windows Defender.",
            RecommendationEn = "If you didn't buy it voluntarily, consider uninstalling and using Windows Defender.",
            Tags = "mcafee,antivirus,securite,bloatware",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "Malwarebytes",
            Aliases = "Malwarebytes Anti-Malware,mbam",
            Publisher = "Malwarebytes Inc",
            ExecutableNames = "mbam.exe,MBAMService.exe,mbamtray.exe",
            Category = KnowledgeCategory.Security,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Outil anti-malware complémentaire populaire.",
            ShortDescriptionEn = "Popular complementary anti-malware tool.",
            FullDescription = "Malwarebytes détecte et supprime les malwares, adwares et PUP que les antivirus traditionnels peuvent manquer. La version gratuite est un scanner à la demande, la version premium offre une protection en temps réel.",
            FullDescriptionEn = "Malwarebytes detects and removes malware, adware and PUPs that traditional antivirus may miss. Free version is an on-demand scanner, premium version offers real-time protection.",
            DisableImpact = "Version gratuite : aucun impact (pas de protection temps réel). Version premium : perte de la protection temps réel.",
            DisableImpactEn = "Free version: no impact (no real-time protection). Premium version: loss of real-time protection.",
            PerformanceImpact = "Faible à modéré (~50-100 Mo RAM en version premium).",
            PerformanceImpactEn = "Low to moderate (~50-100 MB RAM in premium version).",
            Recommendation = "Gardez activé si vous avez la version premium. La version gratuite peut être lancée manuellement.",
            RecommendationEn = "Keep enabled if you have premium version. Free version can be launched manually.",
            Tags = "malwarebytes,antimalware,securite,pup,adware",
            LastUpdated = DateTime.Now
        });
    }

    private void SeedGaming()
    {
        Save(new KnowledgeEntry
        {
            Name = "Steam",
            Aliases = "Steam Client,Steam Client Bootstrapper",
            Publisher = "Valve Corporation",
            ExecutableNames = "steam.exe,steamwebhelper.exe,SteamService.exe",
            Category = KnowledgeCategory.Gaming,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Plateforme de distribution de jeux vidéo de Valve.",
            ShortDescriptionEn = "Valve's video game distribution platform.",
            FullDescription = "Steam est la plus grande plateforme de distribution de jeux PC. Elle gère vos achats, téléchargements, mises à jour de jeux, le cloud save, les succès et les fonctionnalités communautaires. Le démarrage automatique permet de recevoir les notifications et de mettre à jour les jeux en arrière-plan.",
            FullDescriptionEn = "Steam is the largest PC game distribution platform. It manages purchases, downloads, game updates, cloud saves, achievements, and community features. Auto-start enables notifications and background game updates.",
            DisableImpact = "Pas de mise à jour automatique des jeux. Pas de notifications d'amis. Les jeux fonctionneront toujours si Steam est lancé manuellement.",
            DisableImpactEn = "No automatic game updates. No friend notifications. Games still work if Steam is launched manually.",
            PerformanceImpact = "Modéré (~100-200 Mo RAM avec le navigateur intégré).",
            PerformanceImpactEn = "Moderate (~100-200 MB RAM with built-in browser).",
            Recommendation = "Peut être désactivé si vous préférez lancer Steam manuellement quand vous voulez jouer.",
            RecommendationEn = "Can be disabled if you prefer launching Steam manually when you want to play.",
            Tags = "steam,valve,jeux,gaming,store,plateforme",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "Epic Games Launcher",
            Aliases = "Epic Games,EpicGamesLauncher",
            Publisher = "Epic Games, Inc.",
            ExecutableNames = "EpicGamesLauncher.exe,EpicWebHelper.exe",
            Category = KnowledgeCategory.Gaming,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Lanceur de jeux d'Epic Games.",
            ShortDescriptionEn = "Epic Games game launcher.",
            FullDescription = "Le lanceur Epic Games permet d'accéder aux jeux achetés sur l'Epic Games Store, dont les jeux gratuits hebdomadaires. Nécessaire pour Fortnite et les jeux Unreal Engine.",
            FullDescriptionEn = "Epic Games Launcher provides access to games purchased on Epic Games Store, including weekly free games. Required for Fortnite and Unreal Engine games.",
            DisableImpact = "Pas de récupération automatique des jeux gratuits. Les jeux fonctionneront si le lanceur est démarré manuellement.",
            DisableImpactEn = "No automatic free game claims. Games work if launcher is started manually.",
            PerformanceImpact = "Modéré (~80-150 Mo RAM).",
            PerformanceImpactEn = "Moderate (~80-150 MB RAM).",
            Recommendation = "Peut être désactivé. Pensez juste à le lancer régulièrement pour récupérer les jeux gratuits.",
            RecommendationEn = "Can be disabled. Just remember to launch regularly to claim free games.",
            Tags = "epic,fortnite,jeux,gaming,store,unreal",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "GOG Galaxy",
            Aliases = "GOG Galaxy Client,GalaxyClient",
            Publisher = "GOG sp. z o.o.",
            ExecutableNames = "GalaxyClient.exe,GalaxyClientService.exe",
            Category = KnowledgeCategory.Gaming,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Client GOG pour jeux sans DRM.",
            ShortDescriptionEn = "DRM-free gaming client.",
            FullDescription = "GOG Galaxy est le client de la plateforme GOG.com, spécialisée dans les jeux sans DRM. Il peut également intégrer vos bibliothèques Steam, Epic, etc. en une seule interface.",
            FullDescriptionEn = "GOG Galaxy is the client for the GOG.com platform, offering DRM-free games and integration with other game libraries.",
            DisableImpact = "Aucun impact sur les jeux GOG (sans DRM). Perte de l'intégration des bibliothèques.",
            DisableImpactEn = "GOG Galaxy won't start automatically.",
            PerformanceImpact = "Faible à modéré (~50-100 Mo RAM).",
            PerformanceImpactEn = "Low to moderate.",
            Recommendation = "Peut être désactivé. Les jeux GOG fonctionnent sans le client.",
            RecommendationEn = "Can be disabled from automatic startup.",
            Tags = "gog,galaxy,jeux,gaming,drm-free,cdprojekt",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "EA App",
            Aliases = "EA Desktop,Origin,Electronic Arts,EADM",
            Publisher = "Electronic Arts, Inc.",
            ExecutableNames = "EADesktop.exe,EABackgroundService.exe,Origin.exe,EALauncher.exe",
            Category = KnowledgeCategory.Gaming,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Lanceur de jeux Electronic Arts.",
            ShortDescriptionEn = "Electronic Arts game launcher.",
            FullDescription = "L'EA App (anciennement Origin) est nécessaire pour jouer aux jeux EA (FIFA, Battlefield, The Sims, etc.). Gère les téléchargements, mises à jour et la connexion EA.",
            FullDescriptionEn = "EA App (formerly Origin) is required to play EA games (FIFA, Battlefield, The Sims, etc.). Manages downloads, updates and EA account connection.",
            DisableImpact = "Les jeux EA ne pourront pas se lancer sans le client. Démarrez-le manuellement avant de jouer.",
            DisableImpactEn = "EA games won't launch without the client. Start it manually before playing.",
            PerformanceImpact = "Modéré (~70-120 Mo RAM).",
            PerformanceImpactEn = "Moderate (~70-120 MB RAM).",
            Recommendation = "Peut être désactivé si vous ne jouez pas souvent aux jeux EA.",
            RecommendationEn = "Can be disabled if you don't play EA games often.",
            Tags = "ea,origin,jeux,gaming,electronic arts,fifa,battlefield",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "Ubisoft Connect",
            Aliases = "Uplay,UbisoftConnect",
            Publisher = "Ubisoft Entertainment",
            ExecutableNames = "UbisoftConnect.exe,upc.exe,UplayService.exe",
            Category = KnowledgeCategory.Gaming,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Lanceur de jeux Ubisoft.",
            ShortDescriptionEn = "Ubisoft gaming platform.",
            FullDescription = "Ubisoft Connect (anciennement Uplay) est requis pour les jeux Ubisoft (Assassin's Creed, Far Cry, etc.). Gère les succès, récompenses et connexion en ligne.",
            FullDescriptionEn = "Ubisoft Connect is Ubisoft's platform for games and services, replacing Uplay. Manages games, rewards and friends.",
            DisableImpact = "Les jeux Ubisoft ne pourront pas se lancer. Même les jeux Steam Ubisoft nécessitent ce client.",
            DisableImpactEn = "Ubisoft Connect won't start automatically.",
            PerformanceImpact = "Modéré (~60-100 Mo RAM).",
            PerformanceImpactEn = "Moderate.",
            Recommendation = "Peut être désactivé si vous ne jouez pas régulièrement aux jeux Ubisoft.",
            RecommendationEn = "Can be disabled from automatic startup.",
            Tags = "ubisoft,uplay,jeux,gaming,assassins creed",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "Xbox App",
            Aliases = "Xbox,XboxApp,Gaming Services",
            Publisher = "Microsoft Corporation",
            ExecutableNames = "XboxApp.exe,gamingservices.exe,XboxGameBarFTServer.exe",
            Category = KnowledgeCategory.Gaming,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Application Xbox et services de jeu Microsoft.",
            ShortDescriptionEn = "Xbox app and Microsoft gaming services.",
            FullDescription = "L'app Xbox permet d'accéder au Xbox Game Pass PC, aux jeux Xbox Play Anywhere, au chat Xbox et aux fonctionnalités sociales Xbox. La Game Bar (Win+G) fait également partie de cet écosystème.",
            FullDescriptionEn = "Xbox app provides access to Xbox Game Pass PC, Xbox Play Anywhere games, Xbox chat and Xbox social features. Game Bar (Win+G) is also part of this ecosystem.",
            DisableImpact = "Perte de la Game Bar et des fonctionnalités Xbox. Les jeux Game Pass pourraient ne plus fonctionner.",
            DisableImpactEn = "Loss of Game Bar and Xbox features. Game Pass games may stop working.",
            PerformanceImpact = "Faible à modéré (~40-80 Mo RAM).",
            PerformanceImpactEn = "Low to moderate (~40-80 MB RAM).",
            Recommendation = "Gardez activé si vous utilisez le Xbox Game Pass ou la Game Bar.",
            RecommendationEn = "Keep enabled if you use Xbox Game Pass or Game Bar.",
            Tags = "xbox,microsoft,gamepass,gaming,game bar",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "Battle.net",
            Aliases = "Blizzard Battle.net,Blizzard App",
            Publisher = "Blizzard Entertainment, Inc.",
            ExecutableNames = "Battle.net.exe,Agent.exe,BlizzardError.exe",
            Category = KnowledgeCategory.Gaming,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Lanceur de jeux Blizzard Entertainment.",
            ShortDescriptionEn = "Blizzard Entertainment game launcher.",
            FullDescription = "Battle.net est le client de Blizzard pour ses jeux (World of Warcraft, Diablo, Overwatch, Hearthstone, StarCraft, etc.). Gère les téléchargements, mises à jour, le chat avec les amis et les fonctionnalités sociales Blizzard.",
            FullDescriptionEn = "Battle.net is Blizzard's client for its games (World of Warcraft, Diablo, Overwatch, Hearthstone, StarCraft, etc.). Manages downloads, updates, friend chat and Blizzard social features.",
            DisableImpact = "Pas de notifications d'amis. Les jeux Blizzard fonctionneront si Battle.net est lancé manuellement.",
            DisableImpactEn = "No friend notifications. Blizzard games will work if Battle.net is launched manually.",
            PerformanceImpact = "Modéré (~80-150 Mo RAM).",
            PerformanceImpactEn = "Moderate (~80-150 MB RAM).",
            Recommendation = "Peut être désactivé si vous ne jouez pas souvent aux jeux Blizzard.",
            RecommendationEn = "Can be disabled if you don't play Blizzard games often.",
            Tags = "blizzard,battle.net,wow,diablo,overwatch,gaming",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "Discord",
            Aliases = "Discord Client,DiscordPTB,DiscordCanary",
            Publisher = "Discord Inc.",
            ExecutableNames = "Discord.exe,Update.exe",
            Category = KnowledgeCategory.Communication,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Application de chat vocal et textuel pour gamers.",
            ShortDescriptionEn = "Voice and text chat application for gamers.",
            FullDescription = "Discord permet la communication vocale et textuelle, principalement utilisé par les gamers. Offre des serveurs communautaires, le partage d'écran, le streaming et l'intégration avec de nombreux jeux.",
            FullDescriptionEn = "Discord enables voice and text communication, primarily used by gamers. Offers community servers, screen sharing, streaming, and integration with many games.",
            DisableImpact = "Pas de connexion automatique. Vous devrez lancer Discord manuellement pour rejoindre vos serveurs.",
            DisableImpactEn = "No automatic login. You'll need to launch Discord manually to join your servers.",
            PerformanceImpact = "Modéré (~80-150 Mo RAM, plus si plusieurs serveurs sont ouverts).",
            PerformanceImpactEn = "Moderate (~80-150 MB RAM, more if multiple servers are open).",
            Recommendation = "Peut être désactivé si vous n'avez pas besoin d'être connecté en permanence.",
            RecommendationEn = "Can be disabled if you don't need to be connected all the time.",
            Tags = "discord,chat,vocal,gaming,communaute,streaming",
            LastUpdated = DateTime.Now
        });
    }

    private void SeedProductivity()
    {
        Save(new KnowledgeEntry
        {
            Name = "Microsoft Office",
            Aliases = "Office Click-to-Run,OfficeClickToRun,Microsoft 365",
            Publisher = "Microsoft Corporation",
            ExecutableNames = "OfficeClickToRun.exe,MSOSYNC.exe,OfficeC2RClient.exe",
            Category = KnowledgeCategory.Productivity,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Service de mise à jour et synchronisation Microsoft Office.",
            ShortDescriptionEn = "Microsoft Office update and sync service.",
            FullDescription = "Le service Click-to-Run gère les mises à jour de Microsoft Office/365 et la synchronisation des paramètres. Il permet également le démarrage rapide des applications Office.",
            FullDescriptionEn = "The Click-to-Run service manages Microsoft Office/365 updates and settings sync. Also enables faster Office app startup.",
            DisableImpact = "Mises à jour Office manuelles uniquement. Démarrage des applications Office légèrement plus lent.",
            DisableImpactEn = "Manual Office updates only. Slightly slower Office app startup.",
            PerformanceImpact = "Faible (~30-60 Mo RAM).",
            PerformanceImpactEn = "Low (~30-60 MB RAM).",
            Recommendation = "Peut être désactivé si vous préférez des mises à jour manuelles.",
            RecommendationEn = "Can be disabled if you prefer manual updates.",
            Tags = "microsoft,office,365,word,excel,outlook,productivite",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "Adobe Creative Cloud",
            Aliases = "Adobe CC,Creative Cloud,CCLibrary,AdobeGCClient",
            Publisher = "Adobe Inc.",
            ExecutableNames = "Creative Cloud.exe,CCLibrary.exe,AdobeGCClient.exe,CCXProcess.exe,AdobeIPCBroker.exe",
            Category = KnowledgeCategory.Productivity,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Gestionnaire d'applications Adobe Creative Cloud.",
            ShortDescriptionEn = "Adobe Creative Cloud application manager.",
            FullDescription = "Creative Cloud gère les installations, mises à jour et licences des applications Adobe (Photoshop, Illustrator, Premiere, etc.). Synchronise également les fichiers et polices Creative Cloud.",
            FullDescriptionEn = "Creative Cloud manages installations, updates and licenses for Adobe applications (Photoshop, Illustrator, Premiere, etc.). Also syncs Creative Cloud files and fonts.",
            DisableImpact = "Pas de mise à jour automatique. Les applications Adobe fonctionnent toujours mais vous devrez gérer les mises à jour manuellement.",
            DisableImpactEn = "No automatic updates. Adobe applications still work but you'll need to manage updates manually.",
            PerformanceImpact = "Modéré à élevé (~100-250 Mo RAM avec tous les processus).",
            PerformanceImpactEn = "Moderate to high (~100-250 MB RAM with all processes).",
            Recommendation = "Peut être désactivé si vous n'avez pas besoin des mises à jour automatiques. Attention : certaines fonctionnalités cloud pourraient ne plus fonctionner.",
            RecommendationEn = "Can be disabled if you don't need automatic updates. Warning: some cloud features may stop working.",
            Tags = "adobe,creative cloud,photoshop,illustrator,premiere,design",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "Java Update Scheduler",
            Aliases = "jusched,Java Update",
            Publisher = "Oracle Corporation",
            ExecutableNames = "jusched.exe,jucheck.exe",
            Category = KnowledgeCategory.Utility,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Planificateur de mises à jour Java.",
            ShortDescriptionEn = "Java update scheduler.",
            FullDescription = "Vérifie et notifie les mises à jour disponibles pour Java Runtime Environment. Java est utilisé par certaines applications et sites web.",
            FullDescriptionEn = "Checks and notifies available updates for Java Runtime Environment. Java is used by some applications and websites.",
            DisableImpact = "Pas de notification des mises à jour Java. Vous devrez vérifier manuellement.",
            DisableImpactEn = "No Java update notifications. You'll need to check manually.",
            PerformanceImpact = "Très faible (~5-10 Mo RAM).",
            PerformanceImpactEn = "Very low (~5-10 MB RAM).",
            Recommendation = "Peut être désactivé. Vérifiez manuellement les mises à jour Java de temps en temps pour la sécurité.",
            RecommendationEn = "Can be disabled. Check Java updates manually from time to time for security.",
            Tags = "java,oracle,update,runtime",
            LastUpdated = DateTime.Now
        });
    }

    private void SeedCommunication()
    {
        Save(new KnowledgeEntry
        {
            Name = "Skype",
            Aliases = "Skype for Desktop,SkypeApp,Skype for Business",
            Publisher = "Microsoft Corporation",
            ExecutableNames = "Skype.exe,SkypeApp.exe,SkypeBridge.exe",
            Category = KnowledgeCategory.Communication,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Application d'appels vidéo et messagerie Microsoft.",
            ShortDescriptionEn = "Microsoft video calling and messaging app.",
            FullDescription = "Skype permet les appels vidéo/audio, la messagerie instantanée et le partage d'écran. Peut aussi appeler des numéros de téléphone (payant).",
            FullDescriptionEn = "Skype enables video/audio calls, instant messaging and screen sharing. Can also call phone numbers (paid).",
            DisableImpact = "Pas de connexion automatique. Vous ne recevrez pas les appels/messages si Skype n'est pas lancé.",
            DisableImpactEn = "No automatic connection. You won't receive calls/messages if Skype isn't running.",
            PerformanceImpact = "Modéré (~80-150 Mo RAM).",
            PerformanceImpactEn = "Moderate (~80-150 MB RAM).",
            Recommendation = "Peut être désactivé si vous utilisez Teams, Zoom ou d'autres alternatives.",
            RecommendationEn = "Can be disabled if you use Teams, Zoom or other alternatives.",
            Tags = "skype,microsoft,appel,video,chat,communication",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "Microsoft Teams",
            Aliases = "Teams,Microsoft Teams personal",
            Publisher = "Microsoft Corporation",
            ExecutableNames = "Teams.exe,ms-teams.exe",
            Category = KnowledgeCategory.Communication,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Plateforme de collaboration Microsoft.",
            ShortDescriptionEn = "Microsoft collaboration platform.",
            FullDescription = "Teams combine chat, visioconférence, stockage de fichiers et intégration Office 365. Utilisé massivement en entreprise pour le travail collaboratif.",
            FullDescriptionEn = "Teams combines chat, video conferencing, file storage, and Office 365 integration. Widely used in enterprise for collaborative work.",
            DisableImpact = "Pas de notifications de messages/réunions si Teams n'est pas lancé.",
            DisableImpactEn = "No message/meeting notifications if Teams isn't running.",
            PerformanceImpact = "Élevé (~200-400 Mo RAM). Connu pour sa consommation mémoire.",
            PerformanceImpactEn = "High (~200-400 MB RAM). Known for memory consumption.",
            Recommendation = "Peut être désactivé si non utilisé pour le travail. Attention aux réunions manquées.",
            RecommendationEn = "Can be disabled if not used for work. Watch for missed meetings.",
            Tags = "teams,microsoft,collaboration,chat,video,entreprise",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "Zoom",
            Aliases = "Zoom Client,Zoom Meetings",
            Publisher = "Zoom Video Communications, Inc.",
            ExecutableNames = "Zoom.exe,ZoomInstaller.exe",
            Category = KnowledgeCategory.Communication,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Application de visioconférence populaire.",
            ShortDescriptionEn = "Popular video conferencing application.",
            FullDescription = "Zoom permet d'organiser et rejoindre des réunions vidéo, webinaires et conférences. Très utilisé pour le télétravail et l'enseignement à distance.",
            FullDescriptionEn = "Zoom lets you organize and join video meetings, webinars, and conferences. Widely used for remote work and distance learning.",
            DisableImpact = "Vous devrez lancer Zoom manuellement avant les réunions.",
            DisableImpactEn = "You'll need to launch Zoom manually before meetings.",
            PerformanceImpact = "Faible au repos (~20 Mo RAM). Élevé pendant les appels.",
            PerformanceImpactEn = "Low when idle (~20 MB RAM). High during calls.",
            Recommendation = "Peut être désactivé. Zoom se lance généralement via des liens de réunion.",
            RecommendationEn = "Can be disabled. Zoom typically launches via meeting links.",
            Tags = "zoom,video,reunion,conference,teletravail",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "Slack",
            Aliases = "Slack Desktop",
            Publisher = "Slack Technologies, LLC",
            ExecutableNames = "slack.exe",
            Category = KnowledgeCategory.Communication,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Plateforme de messagerie d'entreprise.",
            ShortDescriptionEn = "Enterprise messaging platform.",
            FullDescription = "Slack est une plateforme de communication en équipe avec des canaux, messages directs, intégrations d'apps et partage de fichiers.",
            FullDescriptionEn = "Slack is a team communication platform with channels, direct messages, app integrations, and file sharing.",
            DisableImpact = "Pas de notifications si Slack n'est pas lancé. Messages manqués.",
            DisableImpactEn = "No notifications if Slack isn't running. Missed messages.",
            PerformanceImpact = "Modéré à élevé (~150-300 Mo RAM).",
            PerformanceImpactEn = "Moderate to high (~150-300 MB RAM).",
            Recommendation = "Gardez activé si utilisé pour le travail. Peut être désactivé sinon.",
            RecommendationEn = "Keep enabled if used for work. Can be disabled otherwise.",
            Tags = "slack,chat,entreprise,collaboration,communication",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "Telegram Desktop",
            Aliases = "Telegram",
            Publisher = "Telegram FZ-LLC",
            ExecutableNames = "Telegram.exe",
            Category = KnowledgeCategory.Communication,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Application de messagerie instantanée sécurisée.",
            ShortDescriptionEn = "Secure instant messaging app.",
            FullDescription = "Telegram est une app de messagerie axée sur la vitesse et la sécurité. Offre des chats secrets chiffrés, des groupes jusqu'à 200 000 membres et des canaux.",
            FullDescriptionEn = "Telegram is a messaging app focused on speed and security. Offers encrypted secret chats, groups up to 200,000 members and channels.",
            DisableImpact = "Pas de notifications de messages si Telegram n'est pas lancé.",
            DisableImpactEn = "No message notifications if Telegram is not running.",
            PerformanceImpact = "Faible (~30-60 Mo RAM).",
            PerformanceImpactEn = "Low (~30-60 MB RAM).",
            Recommendation = "Peut être désactivé si vous préférez utiliser la version web ou mobile.",
            RecommendationEn = "Can be disabled if you prefer using the web or mobile version.",
            Tags = "telegram,messagerie,chat,securite,chiffrement",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "Signal",
            Aliases = "Signal Desktop,signal-desktop,org.whispersystems.signal-desktop",
            Publisher = "Signal Messenger, LLC",
            ExecutableNames = "Signal.exe",
            Category = KnowledgeCategory.Communication,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Messagerie chiffrée de bout en bout.",
            ShortDescriptionEn = "End-to-end encrypted messenger.",
            FullDescription = "Signal est une application de messagerie ultra-sécurisée recommandée par des experts en sécurité. Tous les messages, appels et fichiers sont chiffrés de bout en bout. Open source et sans publicité.",
            FullDescriptionEn = "Signal is an ultra-secure messaging app recommended by security experts. All messages, calls, and files are end-to-end encrypted. Open source and ad-free.",
            DisableImpact = "Pas de notifications de messages Signal si l'application n'est pas lancée.",
            DisableImpactEn = "No Signal message notifications if the app is not running.",
            PerformanceImpact = "Faible (~40-80 Mo RAM).",
            PerformanceImpactEn = "Low (~40-80 MB RAM).",
            Recommendation = "Peut être désactivé si vous préférez lancer Signal manuellement.",
            RecommendationEn = "Can be disabled if you prefer to launch Signal manually.",
            Tags = "signal,messagerie,chiffrement,securite,vie privee",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "WhatsApp",
            Aliases = "WhatsApp Desktop",
            Publisher = "WhatsApp Inc.",
            ExecutableNames = "WhatsApp.exe",
            Category = KnowledgeCategory.Communication,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Application de messagerie populaire de Meta.",
            ShortDescriptionEn = "Popular Meta messaging app.",
            FullDescription = "WhatsApp Desktop permet d'envoyer des messages et passer des appels depuis votre PC, synchronisé avec votre téléphone. Chiffrement de bout en bout pour les messages.",
            FullDescriptionEn = "WhatsApp Desktop lets you send messages and make calls from your PC, synced with your phone. End-to-end encryption for messages.",
            DisableImpact = "Pas de notifications WhatsApp sur le PC.",
            DisableImpactEn = "No WhatsApp notifications on PC.",
            PerformanceImpact = "Modéré (~80-150 Mo RAM).",
            PerformanceImpactEn = "Moderate (~80-150 MB RAM).",
            Recommendation = "Peut être désactivé si vous préférez utiliser WhatsApp Web ou uniquement sur mobile.",
            RecommendationEn = "Can be disabled if you prefer using WhatsApp Web or mobile only.",
            Tags = "whatsapp,messagerie,meta,facebook,chat",
            LastUpdated = DateTime.Now
        });
    }

    private void SeedCloudStorage()
    {
        Save(new KnowledgeEntry
        {
            Name = "Google Drive",
            Aliases = "Google Drive for Desktop,DriveFS,GoogleDriveFS",
            Publisher = "Google LLC",
            ExecutableNames = "GoogleDriveFS.exe,GoogleDriveSync.exe",
            Category = KnowledgeCategory.CloudStorage,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Client de synchronisation Google Drive.",
            ShortDescriptionEn = "Google Drive sync client.",
            FullDescription = "Google Drive synchronise vos fichiers avec le cloud Google. Intègre Google Docs, Sheets et Slides. Peut streamer les fichiers sans les télécharger complètement.",
            FullDescriptionEn = "Google Drive syncs your files with Google cloud. Integrates Google Docs, Sheets and Slides. Can stream files without downloading them completely.",
            DisableImpact = "Fichiers non synchronisés. Les fichiers locaux restent accessibles mais pas mis à jour avec le cloud.",
            DisableImpactEn = "Files not synced. Local files remain accessible but not updated with cloud.",
            PerformanceImpact = "Faible à modéré (~40-80 Mo RAM).",
            PerformanceImpactEn = "Low to moderate (~40-80 MB RAM).",
            Recommendation = "Peut être désactivé si vous n'utilisez pas activement Google Drive.",
            RecommendationEn = "Can be disabled if you don't actively use Google Drive.",
            Tags = "google,drive,cloud,synchronisation,stockage",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "Dropbox",
            Aliases = "Dropbox Client",
            Publisher = "Dropbox, Inc.",
            ExecutableNames = "Dropbox.exe,DropboxUpdate.exe",
            Category = KnowledgeCategory.CloudStorage,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Service de stockage cloud et synchronisation.",
            ShortDescriptionEn = "Cloud storage service.",
            FullDescription = "Dropbox synchronise vos fichiers entre vos appareils et le cloud. Offre le partage de fichiers, la collaboration et l'historique des versions.",
            FullDescriptionEn = "Dropbox syncs your files to the cloud and makes them accessible on all your devices. Offers sharing and collaboration.",
            DisableImpact = "Fichiers non synchronisés automatiquement.",
            DisableImpactEn = "Your files won't be synced automatically.",
            PerformanceImpact = "Modéré (~70-120 Mo RAM).",
            PerformanceImpactEn = "Low when idle, moderate during sync.",
            Recommendation = "Peut être désactivé si vous préférez synchroniser manuellement.",
            RecommendationEn = "Keep enabled if you use Dropbox regularly.",
            Tags = "dropbox,cloud,synchronisation,stockage,partage",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "pCloud",
            Aliases = "pCloud Drive,pCloud Sync",
            Publisher = "pCloud International AG",
            ExecutableNames = "pCloud.exe,pCloudSync.exe",
            Category = KnowledgeCategory.CloudStorage,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Service de stockage cloud suisse sécurisé.",
            ShortDescriptionEn = "Secure Swiss cloud storage service.",
            FullDescription = "pCloud est un service de stockage cloud basé en Suisse, offrant un chiffrement côté client optionnel (pCloud Crypto). Propose un stockage à vie avec un seul paiement et synchronise vos fichiers entre appareils.",
            FullDescriptionEn = "pCloud is a Swiss-based cloud storage service offering optional client-side encryption (pCloud Crypto). Offers lifetime storage with a single payment and syncs files between devices.",
            DisableImpact = "Vos fichiers ne seront plus synchronisés automatiquement avec le cloud pCloud.",
            DisableImpactEn = "Your files will no longer sync automatically with pCloud.",
            PerformanceImpact = "Faible à modéré (~40-80 Mo RAM).",
            PerformanceImpactEn = "Low to moderate (~40-80 MB RAM).",
            Recommendation = "Gardez activé si vous utilisez pCloud pour la synchronisation. Peut être désactivé sinon.",
            RecommendationEn = "Keep enabled if you use pCloud for sync. Can be disabled otherwise.",
            Tags = "pcloud,cloud,synchronisation,stockage,suisse,chiffrement",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "Proton Drive",
            Aliases = "ProtonDrive",
            Publisher = "Proton AG",
            ExecutableNames = "ProtonDrive.exe",
            Category = KnowledgeCategory.CloudStorage,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Stockage cloud chiffré de bout en bout par Proton.",
            ShortDescriptionEn = "End-to-end encrypted cloud storage by Proton.",
            FullDescription = "Proton Drive est un service de stockage cloud chiffré de bout en bout, créé par les développeurs de ProtonMail. Toutes les données sont chiffrées avant de quitter votre appareil.",
            FullDescriptionEn = "Proton Drive is an end-to-end encrypted cloud storage service created by the developers of ProtonMail. All data is encrypted before leaving your device.",
            DisableImpact = "Les fichiers ne seront plus synchronisés avec Proton Drive.",
            DisableImpactEn = "Files will no longer sync with Proton Drive.",
            PerformanceImpact = "Faible (~30-60 Mo RAM).",
            PerformanceImpactEn = "Low (~30-60 MB RAM).",
            Recommendation = "Gardez activé si vous utilisez Proton Drive pour la synchronisation sécurisée.",
            RecommendationEn = "Keep enabled if you use Proton Drive for secure sync.",
            Tags = "proton,drive,cloud,chiffrement,securite,stockage",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "Proton Mail Bridge",
            Aliases = "ProtonMail Bridge,Bridge",
            Publisher = "Proton AG",
            ExecutableNames = "protonmail-bridge.exe,bridge.exe",
            Category = KnowledgeCategory.Communication,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Pont IMAP/SMTP pour utiliser ProtonMail avec des clients email.",
            ShortDescriptionEn = "IMAP/SMTP bridge to use ProtonMail with email clients.",
            FullDescription = "Proton Mail Bridge permet d'utiliser votre compte ProtonMail avec des clients email classiques (Outlook, Thunderbird, etc.) en créant un serveur local IMAP/SMTP qui déchiffre vos emails.",
            FullDescriptionEn = "Proton Mail Bridge allows using your ProtonMail account with traditional email clients (Outlook, Thunderbird, etc.) by creating a local IMAP/SMTP server that decrypts your emails.",
            DisableImpact = "Votre client email ne pourra plus accéder à ProtonMail. Les emails ne seront plus synchronisés.",
            DisableImpactEn = "Your email client won't be able to access ProtonMail. Emails won't sync.",
            PerformanceImpact = "Faible (~40-70 Mo RAM).",
            PerformanceImpactEn = "Low (~40-70 MB RAM).",
            Recommendation = "Gardez activé si vous utilisez ProtonMail avec un client email de bureau.",
            RecommendationEn = "Keep enabled if you use ProtonMail with a desktop email client.",
            Tags = "proton,protonmail,email,bridge,imap,chiffrement",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "iCloud",
            Aliases = "iCloud Drive,iCloud Photos,ApplePhotoStreams",
            Publisher = "Apple Inc.",
            ExecutableNames = "iCloudServices.exe,iCloudDrive.exe,iCloudPhotos.exe",
            Category = KnowledgeCategory.CloudStorage,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Services cloud Apple pour Windows.",
            ShortDescriptionEn = "Apple cloud services for Windows.",
            FullDescription = "iCloud pour Windows synchronise vos photos, documents, favoris et mots de passe avec vos appareils Apple.",
            FullDescriptionEn = "iCloud for Windows syncs your photos, documents, bookmarks and passwords with your Apple devices.",
            DisableImpact = "Pas de synchronisation avec les appareils Apple. Photos et fichiers iCloud non accessibles localement.",
            DisableImpactEn = "No sync with Apple devices. iCloud photos and files not accessible locally.",
            PerformanceImpact = "Modéré (~50-100 Mo RAM).",
            PerformanceImpactEn = "Moderate (~50-100 MB RAM).",
            Recommendation = "Gardez activé si vous utilisez des appareils Apple. Peut être désactivé sinon.",
            RecommendationEn = "Keep enabled if you use Apple devices. Can be disabled otherwise.",
            Tags = "apple,icloud,synchronisation,photos,ios,mac",
            LastUpdated = DateTime.Now
        });
    }

    private void SeedMedia()
    {
        Save(new KnowledgeEntry
        {
            Name = "Spotify",
            Aliases = "Spotify Music",
            Publisher = "Spotify AB",
            ExecutableNames = "Spotify.exe",
            Category = KnowledgeCategory.Media,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Service de streaming musical.",
            ShortDescriptionEn = "Music streaming service.",
            FullDescription = "Spotify permet d'écouter de la musique en streaming, créer des playlists et découvrir de nouveaux artistes. Le démarrage automatique permet une lecture rapide.",
            FullDescriptionEn = "Spotify lets you stream music, create playlists, and discover new artists. Auto-start enables quick playback.",
            DisableImpact = "Spotify ne démarrera pas automatiquement. Vous devrez le lancer manuellement.",
            DisableImpactEn = "Spotify won't start automatically. You'll need to launch it manually.",
            PerformanceImpact = "Modéré (~100-150 Mo RAM).",
            PerformanceImpactEn = "Moderate (~100-150 MB RAM).",
            Recommendation = "Peut être désactivé si vous ne voulez pas que Spotify démarre automatiquement.",
            RecommendationEn = "Can be disabled if you don't want Spotify to start automatically.",
            Tags = "spotify,musique,streaming,audio,playlist",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "iTunes",
            Aliases = "iTunesHelper,Apple Mobile Device Service",
            Publisher = "Apple Inc.",
            ExecutableNames = "iTunesHelper.exe,iTunes.exe,AppleMobileDeviceService.exe",
            Category = KnowledgeCategory.Media,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Lecteur multimédia et gestionnaire d'appareils Apple.",
            ShortDescriptionEn = "Media player and Apple device manager.",
            FullDescription = "iTunes gère votre bibliothèque musicale, synchronise les appareils iOS et permet l'achat de musique/films. iTunesHelper détecte les connexions d'iPhone/iPad.",
            FullDescriptionEn = "iTunes manages your music library, syncs iOS devices and enables music/movie purchases. iTunesHelper detects iPhone/iPad connections.",
            DisableImpact = "La détection automatique des iPhones/iPads ne fonctionnera pas. iTunes devra être lancé manuellement.",
            DisableImpactEn = "Automatic iPhone/iPad detection won't work. iTunes will need to be launched manually.",
            PerformanceImpact = "Faible pour iTunesHelper (~10 Mo RAM). iTunes complet : ~100-200 Mo RAM.",
            PerformanceImpactEn = "Low for iTunesHelper (~10 MB RAM). Full iTunes: ~100-200 MB RAM.",
            Recommendation = "Peut être désactivé si vous ne connectez pas régulièrement d'appareils Apple.",
            RecommendationEn = "Can be disabled if you don't regularly connect Apple devices.",
            Tags = "apple,itunes,musique,iphone,ipad,synchronisation",
            LastUpdated = DateTime.Now
        });
    }

    private void SeedBrowsers()
    {
        Save(new KnowledgeEntry
        {
            Name = "Google Chrome",
            Aliases = "Chrome,GoogleChromeAutoLaunch",
            Publisher = "Google LLC",
            ExecutableNames = "chrome.exe,GoogleUpdate.exe",
            Category = KnowledgeCategory.Browser,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Navigateur web de Google.",
            ShortDescriptionEn = "Google's web browser.",
            FullDescription = "Chrome peut être configuré pour démarrer automatiquement et restaurer vos onglets. Google Update vérifie les mises à jour du navigateur.",
            FullDescriptionEn = "Chrome can be configured to start automatically and restore tabs. Google Update checks for browser updates.",
            DisableImpact = "Chrome ne s'ouvrira pas automatiquement. Les mises à jour pourraient être retardées.",
            DisableImpactEn = "Chrome won't open automatically. Updates may be delayed.",
            PerformanceImpact = "Très variable selon les extensions et onglets (~100-500+ Mo RAM).",
            PerformanceImpactEn = "Highly variable depending on extensions and tabs (~100-500+ MB RAM).",
            Recommendation = "Peut être désactivé sauf si vous voulez restaurer automatiquement vos onglets.",
            RecommendationEn = "Can be disabled unless you want to automatically restore tabs.",
            Tags = "google,chrome,navigateur,browser,web",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "Microsoft Edge",
            Aliases = "Edge,MSEdge",
            Publisher = "Microsoft Corporation",
            ExecutableNames = "msedge.exe,MicrosoftEdgeUpdate.exe",
            Category = KnowledgeCategory.Browser,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Navigateur web de Microsoft basé sur Chromium.",
            ShortDescriptionEn = "Microsoft's Chromium-based web browser.",
            FullDescription = "Edge est le navigateur par défaut de Windows, basé sur Chromium. Intègre des fonctionnalités comme Collections, le lecteur PDF et l'intégration Microsoft.",
            FullDescriptionEn = "Edge is Windows' default browser, based on Chromium. Includes features like Collections, PDF reader, and Microsoft integration.",
            DisableImpact = "Edge ne démarrera pas automatiquement.",
            DisableImpactEn = "Edge won't start automatically.",
            PerformanceImpact = "Variable (~80-400+ Mo RAM).",
            PerformanceImpactEn = "Variable (~80-400+ MB RAM).",
            Recommendation = "Peut être désactivé si vous utilisez un autre navigateur.",
            RecommendationEn = "Can be disabled if you use another browser.",
            Tags = "microsoft,edge,navigateur,browser,chromium",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "Brave",
            Aliases = "Brave Browser",
            Publisher = "Brave Software Inc",
            ExecutableNames = "brave.exe",
            Category = KnowledgeCategory.Browser,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Navigateur axé sur la confidentialité avec bloqueur de pubs intégré.",
            ShortDescriptionEn = "Privacy-focused browser with built-in ad blocker.",
            FullDescription = "Brave est un navigateur basé sur Chromium qui bloque automatiquement les publicités et trackers. Offre des récompenses en crypto (BAT) pour les publicités opt-in et une protection renforcée de la vie privée.",
            FullDescriptionEn = "Brave is a Chromium-based browser that automatically blocks ads and trackers. Offers crypto rewards (BAT) for opt-in ads and enhanced privacy protection.",
            DisableImpact = "Brave ne démarrera pas automatiquement.",
            DisableImpactEn = "Brave won't start automatically.",
            PerformanceImpact = "Variable (~80-400+ Mo RAM).",
            PerformanceImpactEn = "Variable (~80-400+ MB RAM).",
            Recommendation = "Peut être désactivé si vous ne voulez pas le démarrage automatique.",
            RecommendationEn = "Can be disabled if you don't want auto-start.",
            Tags = "brave,navigateur,vie privee,crypto,bat,chromium",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "Mozilla Firefox",
            Aliases = "Firefox,Mozilla Maintenance Service",
            Publisher = "Mozilla Corporation",
            ExecutableNames = "firefox.exe,maintenanceservice.exe",
            Category = KnowledgeCategory.Browser,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Navigateur web open source de Mozilla.",
            ShortDescriptionEn = "Mozilla's open source web browser.",
            FullDescription = "Firefox est un navigateur axé sur la vie privée avec un bloqueur de trackers intégré. Le service de maintenance gère les mises à jour en arrière-plan.",
            FullDescriptionEn = "Firefox is a privacy-focused browser with built-in tracker blocker. Maintenance service handles background updates.",
            DisableImpact = "Firefox ne démarrera pas automatiquement. Les mises à jour nécessiteront une élévation admin.",
            DisableImpactEn = "Firefox won't start automatically. Updates will require admin elevation.",
            PerformanceImpact = "Variable (~80-400+ Mo RAM).",
            PerformanceImpactEn = "Variable (~80-400+ MB RAM).",
            Recommendation = "Peut être désactivé. Gardez le service de maintenance si vous voulez des mises à jour silencieuses.",
            RecommendationEn = "Can be disabled. Keep maintenance service if you want silent updates.",
            Tags = "mozilla,firefox,navigateur,browser,vie privee",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "Opera",
            Aliases = "Opera Browser,Opera GX",
            Publisher = "Opera Norway AS",
            ExecutableNames = "opera.exe,opera_autoupdate.exe",
            Category = KnowledgeCategory.Browser,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Navigateur web avec VPN intégré et sidebar.",
            ShortDescriptionEn = "Web browser with built-in VPN and sidebar.",
            FullDescription = "Opera est un navigateur avec des fonctionnalités uniques : VPN gratuit intégré, sidebar pour les réseaux sociaux, bloqueur de pubs, et Workspaces pour organiser les onglets. Opera GX est la version gaming.",
            FullDescriptionEn = "Opera is a browser with unique features: free built-in VPN, social media sidebar, ad blocker, and Workspaces to organize tabs. Opera GX is the gaming version.",
            DisableImpact = "Opera ne démarrera pas automatiquement.",
            DisableImpactEn = "Opera won't start automatically.",
            PerformanceImpact = "Variable (~80-400+ Mo RAM).",
            PerformanceImpactEn = "Variable (~80-400+ MB RAM).",
            Recommendation = "Peut être désactivé si vous ne voulez pas le démarrage automatique.",
            RecommendationEn = "Can be disabled if you don't want auto-start.",
            Tags = "opera,navigateur,browser,vpn,sidebar",
            LastUpdated = DateTime.Now
        });
    }

    private void SeedUtilities()
    {
        Save(new KnowledgeEntry
        {
            Name = "CCleaner",
            Aliases = "CCleaner Browser Monitor,CCleaner Monitoring",
            Publisher = "Piriform Software Ltd",
            ExecutableNames = "CCleaner.exe,CCleaner64.exe,CCleanerBrowser.exe",
            Category = KnowledgeCategory.Utility,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Outil de nettoyage système populaire.",
            ShortDescriptionEn = "System cleaning utility.",
            FullDescription = "CCleaner nettoie les fichiers temporaires, le cache des navigateurs et les entrées de registre inutiles. La surveillance active optimise le système en arrière-plan.",
            FullDescriptionEn = "CCleaner cleans temporary files, registry and browsing data. The Browser Monitor monitors browser settings.",
            DisableImpact = "Pas de nettoyage automatique. Vous devrez lancer CCleaner manuellement.",
            DisableImpactEn = "Scheduled cleaning and monitoring won't work.",
            PerformanceImpact = "Faible (~15-30 Mo RAM).",
            PerformanceImpactEn = "Low.",
            Recommendation = "La surveillance active peut être désactivée. Lancez CCleaner manuellement de temps en temps.",
            RecommendationEn = "Browser Monitor can be disabled.",
            Tags = "ccleaner,nettoyage,optimisation,registre,cache",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "WinRAR",
            Aliases = "WinRAR Startup",
            Publisher = "win.rar GmbH",
            ExecutableNames = "WinRAR.exe",
            Category = KnowledgeCategory.Utility,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Utilitaire de compression de fichiers.",
            ShortDescriptionEn = "File compression utility.",
            FullDescription = "WinRAR permet de créer et extraire des archives RAR, ZIP et autres formats. Le démarrage automatique n'est généralement pas nécessaire.",
            FullDescriptionEn = "WinRAR creates and extracts RAR, ZIP and other archive formats. Auto-start is generally not needed.",
            DisableImpact = "Aucun impact. WinRAR fonctionne via le menu contextuel ou en ouvrant les archives.",
            DisableImpactEn = "No impact. WinRAR works via context menu or by opening archives.",
            PerformanceImpact = "Très faible au démarrage.",
            PerformanceImpactEn = "Very low at startup.",
            Recommendation = "Peut être désactivé. Le démarrage automatique est inutile pour WinRAR.",
            RecommendationEn = "Can be disabled. Auto-start is unnecessary for WinRAR.",
            Tags = "winrar,compression,archive,rar,zip",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "7-Zip",
            Aliases = "7-Zip File Manager",
            Publisher = "Igor Pavlov",
            ExecutableNames = "7zFM.exe,7zG.exe",
            Category = KnowledgeCategory.Utility,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Utilitaire de compression open source.",
            ShortDescriptionEn = "Open source compression utility.",
            FullDescription = "7-Zip est une alternative gratuite à WinRAR, supportant de nombreux formats d'archives. Ne nécessite pas de démarrage automatique.",
            FullDescriptionEn = "7-Zip is a free alternative to WinRAR, supporting many archive formats. Doesn't need auto-start.",
            DisableImpact = "Aucun impact. 7-Zip fonctionne via le menu contextuel.",
            DisableImpactEn = "No impact. 7-Zip works via context menu.",
            PerformanceImpact = "Aucun si non démarré.",
            PerformanceImpactEn = "None if not started.",
            Recommendation = "Devrait être désactivé s'il est dans le démarrage automatique.",
            RecommendationEn = "Should be disabled if in auto-start.",
            Tags = "7zip,compression,archive,open source",
            LastUpdated = DateTime.Now
        });
    }

    private void SeedPeripherals()
    {
        Save(new KnowledgeEntry
        {
            Name = "Logitech G HUB",
            Aliases = "LGHUB,Logitech Gaming Software",
            Publisher = "Logitech",
            ExecutableNames = "lghub.exe,lghub_agent.exe,lghub_updater.exe",
            Category = KnowledgeCategory.Hardware,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Logiciel de configuration pour périphériques gaming Logitech.",
            ShortDescriptionEn = "Logitech gaming peripheral manager.",
            FullDescription = "G HUB permet de configurer les souris, claviers, casques et volants gaming Logitech. Gère les profils de jeu, l'éclairage RGB LIGHTSYNC, les macros et les paramètres DPI.",
            FullDescriptionEn = "Logitech G HUB manages Logitech gaming peripherals (mice, keyboards, headsets). Configures RGB lighting, macros and profiles.",
            DisableImpact = "Les périphériques fonctionneront avec les paramètres par défaut. Les profils personnalisés et l'éclairage RGB ne seront pas appliqués.",
            DisableImpactEn = "Custom profiles and lighting won't be loaded.",
            PerformanceImpact = "Modéré (~80-150 Mo RAM).",
            PerformanceImpactEn = "Moderate.",
            Recommendation = "Gardez activé si vous utilisez des périphériques Logitech avec des profils personnalisés.",
            RecommendationEn = "Keep enabled if you use Logitech G peripherals.",
            Tags = "logitech,gaming,souris,clavier,rgb,peripheriques",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "Corsair iCUE",
            Aliases = "iCUE,Corsair Utility Engine",
            Publisher = "Corsair",
            ExecutableNames = "iCUE.exe,Corsair.Service.CpuIdRemote64.exe,CorsairGamingAudioCfgService64.exe",
            Category = KnowledgeCategory.Hardware,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Logiciel de contrôle pour périphériques Corsair.",
            ShortDescriptionEn = "Corsair peripheral manager.",
            FullDescription = "iCUE gère l'éclairage RGB, les profils de ventilateurs, les macros clavier/souris et les paramètres audio pour tous les périphériques Corsair (claviers, souris, casques, RAM RGB, refroidissement).",
            FullDescriptionEn = "Corsair iCUE controls RGB lighting, performance and macros for all Corsair peripherals (keyboards, mice, headsets, RAM, fans).",
            DisableImpact = "L'éclairage RGB reviendra aux effets par défaut. Les profils de ventilateurs et macros ne fonctionneront pas.",
            DisableImpactEn = "RGB lighting and custom profiles won't be loaded.",
            PerformanceImpact = "Modéré à élevé (~100-200 Mo RAM).",
            PerformanceImpactEn = "Moderate to high.",
            Recommendation = "Gardez activé si vous utilisez l'éclairage RGB ou des profils de ventilateurs personnalisés.",
            RecommendationEn = "Keep enabled if you have Corsair RGB peripherals.",
            Tags = "corsair,icue,rgb,ventilateurs,gaming,peripheriques",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "SteelSeries GG",
            Aliases = "SteelSeries Engine,SteelSeriesGG",
            Publisher = "SteelSeries ApS",
            ExecutableNames = "SteelSeriesGG.exe,SteelSeriesEngine3.exe",
            Category = KnowledgeCategory.Hardware,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Logiciel pour périphériques gaming SteelSeries.",
            ShortDescriptionEn = "SteelSeries peripheral manager.",
            FullDescription = "SteelSeries GG configure les souris, claviers et casques SteelSeries. Inclut Sonar pour l'audio gaming et Moments pour l'enregistrement de clips.",
            FullDescriptionEn = "SteelSeries GG configures SteelSeries peripherals (headsets, mice, keyboards). Includes Moments for gaming clips and Sonar for audio.",
            DisableImpact = "Les périphériques utiliseront les paramètres par défaut. Pas de personnalisation RGB ou audio Sonar.",
            DisableImpactEn = "Lighting and custom profiles won't be loaded.",
            PerformanceImpact = "Modéré (~70-120 Mo RAM).",
            PerformanceImpactEn = "Moderate.",
            Recommendation = "Gardez activé si vous utilisez des périphériques SteelSeries avec des profils personnalisés.",
            RecommendationEn = "Keep enabled if you have SteelSeries peripherals.",
            Tags = "steelseries,gaming,audio,sonar,peripheriques",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "Razer Synapse",
            Aliases = "Synapse3,Razer Synapse 3,RazerSynapse",
            Publisher = "Razer USA Ltd.",
            ExecutableNames = "Razer Synapse 3.exe,RazerCentralService.exe,Razer Synapse Service.exe",
            Category = KnowledgeCategory.Hardware,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Suite logicielle pour périphériques gaming Razer.",
            ShortDescriptionEn = "Razer peripheral manager.",
            FullDescription = "Razer Synapse 3 configure tous les périphériques Razer (souris, claviers, casques, tapis de souris RGB). Gère l'éclairage Chroma RGB, les macros, les profils de jeu et la synchronisation cloud des paramètres.",
            FullDescriptionEn = "Razer Synapse configures Chroma RGB lighting, macros and profiles for all Razer peripherals. Syncs settings to the cloud.",
            DisableImpact = "Les périphériques Razer fonctionneront avec les paramètres par défaut. Pas d'éclairage Chroma personnalisé ni de macros.",
            DisableImpactEn = "Chroma lighting and profiles won't be loaded.",
            PerformanceImpact = "Modéré à élevé (~100-200 Mo RAM avec tous les modules).",
            PerformanceImpactEn = "Moderate to high.",
            Recommendation = "Gardez activé si vous utilisez des périphériques Razer. Peut être désactivé sinon.",
            RecommendationEn = "Keep enabled if you have Razer peripherals.",
            Tags = "razer,synapse,chroma,rgb,gaming,souris,clavier",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "Waves Audio",
            Aliases = "WavesSvc,Waves MaxxAudio,MaxxAudioPro",
            Publisher = "Waves Audio Ltd.",
            ExecutableNames = "WavesSvc64.exe,WavesSysSvc64.exe",
            Category = KnowledgeCategory.Hardware,
            SafetyLevel = SafetyLevel.Important,
            ShortDescription = "Service de traitement audio DSP Waves.",
            ShortDescriptionEn = "Waves DSP audio processing service.",
            FullDescription = "Waves Audio fournit le traitement audio avancé (DSP) sur de nombreux PC de marque (Dell, HP, Lenovo). Offre des améliorations sonores, l'égalisation et les effets MaxxAudio.",
            FullDescriptionEn = "Waves Audio provides advanced audio processing (DSP) on many brand-name PCs (Dell, HP, Lenovo). Offers sound enhancements, equalization and MaxxAudio effects.",
            DisableImpact = "L'audio fonctionnera mais sans les améliorations Waves. Le son pourrait sembler plus plat.",
            DisableImpactEn = "Audio will work but without Waves enhancements. Sound may seem flatter.",
            PerformanceImpact = "Faible (~20-40 Mo RAM).",
            PerformanceImpactEn = "Low (~20-40 MB RAM).",
            Recommendation = "Gardez activé si vous appréciez les améliorations audio. Peut être désactivé si vous préférez un son neutre.",
            RecommendationEn = "Keep enabled if you appreciate audio enhancements. Can be disabled if you prefer neutral sound.",
            Tags = "waves,audio,maxxaudio,dsp,son,amelioration",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "AMD Noise Suppression",
            Aliases = "AMDNoiseSuppression,AMD ANR",
            Publisher = "Advanced Micro Devices Inc.",
            ExecutableNames = "AMDNoiseSuppression.exe",
            Category = KnowledgeCategory.Hardware,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Réduction de bruit IA pour microphones AMD.",
            ShortDescriptionEn = "AI noise reduction for AMD microphones.",
            FullDescription = "AMD Noise Suppression utilise l'IA pour filtrer le bruit de fond de votre microphone en temps réel. Similaire à NVIDIA RTX Voice mais pour les GPU AMD. Élimine les bruits de clavier, ventilateurs, etc.",
            FullDescriptionEn = "AMD Noise Suppression uses AI to filter background noise from your microphone in real-time. Similar to NVIDIA RTX Voice but for AMD GPUs. Removes keyboard, fan noise, etc.",
            DisableImpact = "Pas de réduction de bruit IA sur votre microphone.",
            DisableImpactEn = "No AI noise reduction on your microphone.",
            PerformanceImpact = "Faible à modéré (~1-3% GPU selon le modèle).",
            PerformanceImpactEn = "Low to moderate (~1-3% GPU depending on model).",
            Recommendation = "Gardez activé si vous faites des appels vocaux ou du streaming. Peut être désactivé sinon.",
            RecommendationEn = "Keep enabled for voice calls or streaming. Can be disabled otherwise.",
            Tags = "amd,bruit,microphone,ia,voice,reduction",
            LastUpdated = DateTime.Now
        });
    }

    private void SeedDevelopment()
    {
        Save(new KnowledgeEntry
        {
            Name = "Visual Studio Code",
            Aliases = "VS Code,Code,VSCode",
            Publisher = "Microsoft Corporation",
            ExecutableNames = "Code.exe,code.cmd",
            Category = KnowledgeCategory.Productivity,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Éditeur de code léger et extensible de Microsoft.",
            ShortDescriptionEn = "Microsoft's lightweight and extensible code editor.",
            FullDescription = "Visual Studio Code est un éditeur de code gratuit très populaire chez les développeurs. Supporte de nombreux langages via extensions, le débogage intégré et l'intégration Git.",
            FullDescriptionEn = "Visual Studio Code is a free code editor very popular among developers. Supports many languages via extensions, integrated debugging and Git integration.",
            DisableImpact = "VS Code ne démarrera pas automatiquement. Aucun impact sur le fonctionnement.",
            DisableImpactEn = "VS Code won't start automatically. No impact on functionality.",
            PerformanceImpact = "Variable selon les extensions (~150-400 Mo RAM).",
            PerformanceImpactEn = "Variable depending on extensions (~150-400 MB RAM).",
            Recommendation = "Peut être désactivé. Le démarrage automatique est rarement nécessaire.",
            RecommendationEn = "Can be disabled. Auto-start is rarely needed.",
            Tags = "vscode,developpement,editeur,microsoft,programmation",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "Docker Desktop",
            Aliases = "Docker,Docker Engine",
            Publisher = "Docker Inc.",
            ExecutableNames = "Docker Desktop.exe,dockerd.exe,com.docker.backend.exe",
            Category = KnowledgeCategory.Utility,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Plateforme de conteneurisation pour développeurs.",
            ShortDescriptionEn = "Container platform for developers.",
            FullDescription = "Docker Desktop permet d'exécuter des conteneurs Linux et Windows sur votre PC. Utilisé massivement pour le développement, les tests et le déploiement d'applications.",
            FullDescriptionEn = "Docker Desktop lets you run Linux and Windows containers on your PC. Widely used for development, testing and application deployment.",
            DisableImpact = "Les conteneurs Docker ne démarreront pas automatiquement. Vous devrez lancer Docker manuellement.",
            DisableImpactEn = "Docker containers won't start automatically. You'll need to launch Docker manually.",
            PerformanceImpact = "Élevé (~500 Mo - 2 Go RAM selon les conteneurs actifs). Utilise WSL2 ou Hyper-V.",
            PerformanceImpactEn = "High (~500 MB - 2 GB RAM depending on active containers). Uses WSL2 or Hyper-V.",
            Recommendation = "Peut être désactivé si vous n'utilisez pas Docker quotidiennement. Lance-le manuellement quand nécessaire.",
            RecommendationEn = "Can be disabled if you don't use Docker daily. Launch manually when needed.",
            Tags = "docker,conteneur,developpement,devops,wsl",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "Git",
            Aliases = "Git for Windows,Git Bash",
            Publisher = "The Git Development Community",
            ExecutableNames = "git.exe,git-bash.exe,git-cmd.exe",
            Category = KnowledgeCategory.Utility,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Système de contrôle de version distribué.",
            ShortDescriptionEn = "Distributed version control system.",
            FullDescription = "Git est le système de gestion de version le plus utilisé au monde. Il ne démarre normalement pas automatiquement, c'est un outil en ligne de commande.",
            FullDescriptionEn = "Git is the world's most used version control system. It doesn't normally start automatically, it's a command-line tool.",
            DisableImpact = "Aucun impact. Git est un outil à la demande.",
            DisableImpactEn = "No impact. Git is an on-demand tool.",
            PerformanceImpact = "Aucun au repos.",
            PerformanceImpactEn = "None when idle.",
            Recommendation = "Si Git apparaît dans le démarrage, il peut être désactivé en toute sécurité.",
            RecommendationEn = "If Git appears in startup, it can be safely disabled.",
            Tags = "git,developpement,version,github,code",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "Android Studio",
            Aliases = "Android Studio IDE",
            Publisher = "Google LLC",
            ExecutableNames = "studio64.exe,adb.exe,emulator.exe",
            Category = KnowledgeCategory.Productivity,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Environnement de développement Android officiel.",
            ShortDescriptionEn = "Official Android development environment.",
            FullDescription = "Android Studio est l'IDE officiel de Google pour développer des applications Android. Inclut un émulateur Android et les outils SDK.",
            FullDescriptionEn = "Android Studio is Google's official IDE for developing Android applications. Includes an Android emulator and SDK tools.",
            DisableImpact = "Aucun impact. Android Studio devrait être lancé manuellement.",
            DisableImpactEn = "No impact. Android Studio should be launched manually.",
            PerformanceImpact = "Très élevé quand actif (~1-4 Go RAM avec émulateur).",
            PerformanceImpactEn = "Very high when active (~1-4 GB RAM with emulator).",
            Recommendation = "Si des services Android apparaissent au démarrage (ADB, émulateur), ils peuvent être désactivés.",
            RecommendationEn = "If Android services appear at startup (ADB, emulator), they can be disabled.",
            Tags = "android,developpement,google,mobile,ide",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "JetBrains Toolbox",
            Aliases = "Toolbox App,JetBrains",
            Publisher = "JetBrains s.r.o.",
            ExecutableNames = "jetbrains-toolbox.exe",
            Category = KnowledgeCategory.Productivity,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Gestionnaire d'IDE JetBrains.",
            ShortDescriptionEn = "JetBrains IDE manager.",
            FullDescription = "Toolbox gère l'installation et les mises à jour des IDE JetBrains (IntelliJ, PyCharm, WebStorm, etc.). Permet de lancer rapidement les projets récents.",
            FullDescriptionEn = "Toolbox manages installation and updates of JetBrains IDEs (IntelliJ, PyCharm, WebStorm, etc.). Allows quick launching of recent projects.",
            DisableImpact = "Pas de mises à jour automatiques des IDE. Les IDE fonctionnent toujours.",
            DisableImpactEn = "No automatic IDE updates. IDEs still work.",
            PerformanceImpact = "Faible (~30-50 Mo RAM).",
            PerformanceImpactEn = "Low (~30-50 MB RAM).",
            Recommendation = "Peut être désactivé si vous préférez lancer les IDE manuellement.",
            RecommendationEn = "Can be disabled if you prefer launching IDEs manually.",
            Tags = "jetbrains,ide,developpement,intellij,pycharm",
            LastUpdated = DateTime.Now
        });
    }

    private void SeedSystemTools()
    {
        Save(new KnowledgeEntry
        {
            Name = "Everything",
            Aliases = "Everything Search,voidtools",
            Publisher = "voidtools",
            ExecutableNames = "Everything.exe,Everything64.exe",
            Category = KnowledgeCategory.Utility,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Moteur de recherche de fichiers ultra-rapide.",
            ShortDescriptionEn = "Ultra-fast file search engine.",
            FullDescription = "Everything indexe instantanément tous les fichiers de vos disques NTFS. La recherche est quasi instantanée, bien plus rapide que Windows Search. L'indexation est légère car elle utilise la MFT du système de fichiers.",
            FullDescriptionEn = "Everything instantly indexes all files on your NTFS drives. Search is nearly instant, much faster than Windows Search. Indexing is lightweight as it uses the file system's MFT.",
            DisableImpact = "La recherche ne sera pas disponible immédiatement. L'index devra être reconstruit au lancement.",
            DisableImpactEn = "Search won't be immediately available. Index will need to rebuild at launch.",
            PerformanceImpact = "Très faible (~15-30 Mo RAM). Indexation quasi instantanée.",
            PerformanceImpactEn = "Very low (~15-30 MB RAM). Nearly instant indexing.",
            Recommendation = "Gardez activé pour une recherche instantanée. Excellente alternative à Windows Search.",
            RecommendationEn = "Keep enabled for instant search. Excellent alternative to Windows Search.",
            Tags = "everything,recherche,fichiers,voidtools,productivite",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "HWiNFO",
            Aliases = "HWiNFO64,HWiNFO32",
            Publisher = "Martin Malík - REALiX",
            ExecutableNames = "HWiNFO64.exe,HWiNFO32.exe",
            Category = KnowledgeCategory.Utility,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Outil de diagnostic et monitoring matériel complet.",
            ShortDescriptionEn = "Hardware information and monitoring tool.",
            FullDescription = "HWiNFO fournit des informations détaillées sur le matériel et surveille les capteurs (températures, voltages, vitesses de ventilateurs). Très utilisé pour le monitoring en temps réel.",
            FullDescriptionEn = "HWiNFO provides detailed information about hardware and monitors sensors (temperature, voltage, fan speed).",
            DisableImpact = "Pas de monitoring des températures au démarrage. Les widgets de monitoring ne fonctionneront pas.",
            DisableImpactEn = "Background monitoring won't be active.",
            PerformanceImpact = "Faible (~20-40 Mo RAM).",
            PerformanceImpactEn = "Low.",
            Recommendation = "Peut être désactivé si vous ne surveillez pas les températures en permanence.",
            RecommendationEn = "Can be disabled from startup if used occasionally.",
            Tags = "hwinfo,monitoring,temperature,capteurs,diagnostic",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "Core Temp",
            Aliases = "CoreTemp",
            Publisher = "ALCPU",
            ExecutableNames = "Core Temp.exe",
            Category = KnowledgeCategory.Utility,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Moniteur de température CPU léger.",
            ShortDescriptionEn = "Lightweight CPU temperature monitor.",
            FullDescription = "Core Temp affiche la température de chaque cœur du processeur. Peut afficher la température dans la barre des tâches et alerter en cas de surchauffe.",
            FullDescriptionEn = "Core Temp displays the temperature of each processor core. Can show temperature in taskbar and alert on overheating.",
            DisableImpact = "Pas de surveillance de température CPU au démarrage.",
            DisableImpactEn = "No CPU temperature monitoring at startup.",
            PerformanceImpact = "Très faible (~5-10 Mo RAM).",
            PerformanceImpactEn = "Very low (~5-10 MB RAM).",
            Recommendation = "Peut être désactivé. Utile pour surveiller la température pendant les jeux ou le travail intensif.",
            RecommendationEn = "Can be disabled. Useful for monitoring temperature during gaming or intensive work.",
            Tags = "coretemp,temperature,cpu,monitoring,processeur",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "CrystalDiskInfo",
            Aliases = "DiskInfo",
            Publisher = "Crystal Dew World",
            ExecutableNames = "DiskInfo64.exe,DiskInfo32.exe",
            Category = KnowledgeCategory.Utility,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Outil de surveillance de santé des disques.",
            ShortDescriptionEn = "Disk health monitoring tool.",
            FullDescription = "CrystalDiskInfo lit les données S.M.A.R.T. de vos disques durs et SSD pour évaluer leur santé. Peut alerter en cas de problème détecté sur un disque.",
            FullDescriptionEn = "CrystalDiskInfo reads S.M.A.R.T. data from your hard drives and SSDs to assess their health. Can alert on detected disk problems.",
            DisableImpact = "Pas de surveillance de la santé des disques au démarrage.",
            DisableImpactEn = "No disk health monitoring at startup.",
            PerformanceImpact = "Très faible (~10-20 Mo RAM).",
            PerformanceImpactEn = "Very low (~10-20 MB RAM).",
            Recommendation = "Gardez activé pour être alerté des problèmes de disque. Peut aider à prévenir les pertes de données.",
            RecommendationEn = "Keep enabled to be alerted of disk issues. Can help prevent data loss.",
            Tags = "crystaldiskinfo,disque,ssd,hdd,smart,sante",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "Intel Driver & Support Assistant",
            Aliases = "Intel DSA,Intel Driver Update Utility",
            Publisher = "Intel Corporation",
            ExecutableNames = "DSATray.exe,DSAService.exe",
            Category = KnowledgeCategory.Hardware,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Assistant de mise à jour des pilotes Intel.",
            ShortDescriptionEn = "Intel driver update assistant.",
            FullDescription = "Intel DSA détecte les composants Intel de votre système et propose les dernières mises à jour de pilotes. Inclut un service en arrière-plan qui vérifie régulièrement les mises à jour.",
            FullDescriptionEn = "Intel DSA detects Intel components in your system and offers latest driver updates. Includes a background service that regularly checks for updates.",
            DisableImpact = "Pas de notification des mises à jour Intel. Les pilotes peuvent être mis à jour manuellement sur le site Intel.",
            DisableImpactEn = "No Intel update notifications. Drivers can be manually updated from Intel's website.",
            PerformanceImpact = "Faible (~20-40 Mo RAM).",
            PerformanceImpactEn = "Low (~20-40 MB RAM).",
            Recommendation = "Peut être désactivé. Les mises à jour importantes arrivent généralement via Windows Update.",
            RecommendationEn = "Can be disabled. Important updates usually come via Windows Update.",
            Tags = "intel,pilotes,driver,mise a jour,support",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "Samsung Magician",
            Aliases = "Samsung SSD Magician",
            Publisher = "Samsung Electronics Co., Ltd.",
            ExecutableNames = "Samsung Magician.exe",
            Category = KnowledgeCategory.Utility,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Outil de gestion pour SSD Samsung.",
            ShortDescriptionEn = "Management tool for Samsung SSDs.",
            FullDescription = "Samsung Magician permet de surveiller la santé des SSD Samsung, mettre à jour le firmware, activer les modes de performance et gérer le surapprovisionnement.",
            FullDescriptionEn = "Samsung Magician monitors Samsung SSD health, updates firmware, enables performance modes and manages over-provisioning.",
            DisableImpact = "Pas de surveillance automatique du SSD. Les mises à jour de firmware devront être faites manuellement.",
            DisableImpactEn = "No automatic SSD monitoring. Firmware updates must be done manually.",
            PerformanceImpact = "Faible (~30-50 Mo RAM).",
            PerformanceImpactEn = "Low (~30-50 MB RAM).",
            Recommendation = "Peut être désactivé. Lancez-le occasionnellement pour vérifier les mises à jour de firmware.",
            RecommendationEn = "Can be disabled. Run it occasionally to check for firmware updates.",
            Tags = "samsung,ssd,magician,firmware,stockage",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "Audacity",
            Aliases = "Audacity Audio Editor",
            Publisher = "Audacity Team",
            ExecutableNames = "audacity.exe",
            Category = KnowledgeCategory.Media,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Éditeur audio open source.",
            ShortDescriptionEn = "Open source audio editor.",
            FullDescription = "Audacity est un éditeur audio gratuit permettant d'enregistrer, éditer et mixer des pistes audio. Ne nécessite pas de démarrage automatique.",
            FullDescriptionEn = "Audacity is a free audio editor for recording, editing and mixing audio tracks. Doesn't need auto-start.",
            DisableImpact = "Aucun impact. Audacity est une application à lancer manuellement.",
            DisableImpactEn = "No impact. Audacity is a manually launched application.",
            PerformanceImpact = "Aucun au repos.",
            PerformanceImpactEn = "None when idle.",
            Recommendation = "Si Audacity est dans le démarrage, il peut être désactivé en toute sécurité.",
            RecommendationEn = "If Audacity is in startup, it can be safely disabled.",
            Tags = "audacity,audio,editeur,enregistrement,musique",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "OBS Studio",
            Aliases = "OBS,Open Broadcaster Software",
            Publisher = "OBS Project",
            ExecutableNames = "obs64.exe,obs32.exe",
            Category = KnowledgeCategory.Media,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Logiciel de streaming et d'enregistrement vidéo.",
            ShortDescriptionEn = "Video streaming and recording software.",
            FullDescription = "OBS Studio permet le streaming en direct vers Twitch, YouTube, etc., ainsi que l'enregistrement local de vidéos. Très populaire chez les streamers et créateurs de contenu.",
            FullDescriptionEn = "OBS Studio enables live streaming to Twitch, YouTube, etc., and local video recording. Very popular among streamers and content creators.",
            DisableImpact = "OBS ne démarrera pas automatiquement. Aucun impact sur le streaming.",
            DisableImpactEn = "OBS won't start automatically. No impact on streaming.",
            PerformanceImpact = "Élevé pendant l'utilisation (encodage). Aucun impact au repos.",
            PerformanceImpactEn = "High during use (encoding). No impact when idle.",
            Recommendation = "Ne devrait pas démarrer automatiquement. Peut être désactivé en toute sécurité.",
            RecommendationEn = "Shouldn't auto-start. Can be safely disabled.",
            Tags = "obs,streaming,twitch,youtube,enregistrement,video",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "VLC media player",
            Aliases = "VLC,VideoLAN",
            Publisher = "VideoLAN",
            ExecutableNames = "vlc.exe",
            Category = KnowledgeCategory.Media,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Lecteur multimédia universel open source.",
            ShortDescriptionEn = "Universal open source media player.",
            FullDescription = "VLC peut lire pratiquement tous les formats audio et vidéo sans codecs supplémentaires. Inclut des fonctionnalités de streaming et de conversion.",
            FullDescriptionEn = "VLC can play virtually all audio and video formats without additional codecs. Includes streaming and conversion features.",
            DisableImpact = "Aucun impact. VLC est un lecteur à lancer manuellement.",
            DisableImpactEn = "No impact. VLC is a manually launched player.",
            PerformanceImpact = "Aucun au repos.",
            PerformanceImpactEn = "None when idle.",
            Recommendation = "Ne devrait pas être dans le démarrage automatique. Peut être désactivé.",
            RecommendationEn = "Shouldn't be in auto-start. Can be disabled.",
            Tags = "vlc,video,lecteur,multimedia,codec",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "qBittorrent",
            Aliases = "qBittorrent-nox",
            Publisher = "The qBittorrent project",
            ExecutableNames = "qbittorrent.exe",
            Category = KnowledgeCategory.Utility,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Client BitTorrent open source.",
            ShortDescriptionEn = "Open source BitTorrent client.",
            FullDescription = "qBittorrent est un client torrent gratuit et sans publicité. Permet de télécharger et partager des fichiers via le protocole BitTorrent.",
            FullDescriptionEn = "qBittorrent is a free, ad-free torrent client. Downloads and shares files via BitTorrent protocol.",
            DisableImpact = "Les téléchargements torrents ne reprendront pas automatiquement.",
            DisableImpactEn = "Torrent downloads won't resume automatically.",
            PerformanceImpact = "Variable selon l'activité (~30-100 Mo RAM, bande passante selon téléchargements).",
            PerformanceImpactEn = "Variable depending on activity (~30-100 MB RAM, bandwidth according to downloads).",
            Recommendation = "Peut être désactivé si vous ne téléchargez pas régulièrement des torrents.",
            RecommendationEn = "Can be disabled if you don't regularly download torrents.",
            Tags = "qbittorrent,torrent,telechargement,p2p",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "Internet Download Manager",
            Aliases = "IDM,IDMan",
            Publisher = "Tonec Inc.",
            ExecutableNames = "IDMan.exe,IEMonitor.exe",
            Category = KnowledgeCategory.Utility,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Gestionnaire de téléchargements avancé.",
            ShortDescriptionEn = "Advanced download manager.",
            FullDescription = "IDM accélère les téléchargements en les divisant en segments. S'intègre aux navigateurs pour capturer les téléchargements et les vidéos.",
            FullDescriptionEn = "IDM accelerates downloads by splitting them into segments. Integrates with browsers to capture downloads and videos.",
            DisableImpact = "L'intégration avec les navigateurs ne fonctionnera pas. Les téléchargements devront être ajoutés manuellement.",
            DisableImpactEn = "Browser integration won't work. Downloads will need to be added manually.",
            PerformanceImpact = "Faible (~20-30 Mo RAM).",
            PerformanceImpactEn = "Low (~20-30 MB RAM).",
            Recommendation = "Gardez activé si vous téléchargez fréquemment des fichiers volumineux.",
            RecommendationEn = "Keep enabled if you frequently download large files.",
            Tags = "idm,telechargement,gestionnaire,download,accelerateur",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "AutoHotkey",
            Aliases = "AHK,AutoHotkey Script",
            Publisher = "AutoHotkey Foundation",
            ExecutableNames = "AutoHotkey.exe,AutoHotkeyU64.exe",
            Category = KnowledgeCategory.Utility,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Langage de script pour automatisation Windows.",
            ShortDescriptionEn = "Scripting language for Windows automation.",
            FullDescription = "AutoHotkey permet de créer des raccourcis clavier personnalisés, des macros et d'automatiser des tâches répétitives. Les scripts peuvent démarrer automatiquement.",
            FullDescriptionEn = "AutoHotkey creates custom keyboard shortcuts, macros and automates repetitive tasks. Scripts can auto-start.",
            DisableImpact = "Vos scripts AutoHotkey ne s'exécuteront pas au démarrage. Les raccourcis personnalisés ne fonctionneront pas.",
            DisableImpactEn = "Your AutoHotkey scripts won't run at startup. Custom shortcuts won't work.",
            PerformanceImpact = "Très faible (~5-15 Mo RAM par script).",
            PerformanceImpactEn = "Very low (~5-15 MB RAM per script).",
            Recommendation = "Gardez activé si vous utilisez des scripts AHK. Vérifiez quels scripts sont lancés.",
            RecommendationEn = "Keep enabled if you use AHK scripts. Check which scripts are launched.",
            Tags = "autohotkey,automatisation,macro,raccourci,script",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "PowerToys",
            Aliases = "Microsoft PowerToys",
            Publisher = "Microsoft Corporation",
            ExecutableNames = "PowerToys.exe,PowerToys.Settings.exe",
            Category = KnowledgeCategory.Utility,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Suite d'utilitaires avancés Microsoft pour Windows.",
            ShortDescriptionEn = "Microsoft's advanced Windows utilities suite.",
            FullDescription = "PowerToys est une collection d'outils pour utilisateurs avancés : FancyZones (gestion des fenêtres), PowerRename, Color Picker, File Explorer add-ons, Keyboard Manager, PowerToys Run (lanceur), et plus encore.",
            FullDescriptionEn = "PowerToys is a collection of power user tools: FancyZones (window management), PowerRename, Color Picker, File Explorer add-ons, Keyboard Manager, PowerToys Run (launcher), and more.",
            DisableImpact = "Toutes les fonctionnalités PowerToys seront indisponibles : FancyZones, PowerToys Run (Alt+Space), raccourcis personnalisés, etc.",
            DisableImpactEn = "All PowerToys features will be unavailable: FancyZones, PowerToys Run (Alt+Space), custom shortcuts, etc.",
            PerformanceImpact = "Faible à modéré (~50-100 Mo RAM selon les modules activés).",
            PerformanceImpactEn = "Low to moderate (~50-100 MB RAM depending on enabled modules).",
            Recommendation = "Gardez activé si vous utilisez FancyZones ou PowerToys Run. Excellent outil de productivité.",
            RecommendationEn = "Keep enabled if you use FancyZones or PowerToys Run. Excellent productivity tool.",
            Tags = "powertoys,microsoft,productivite,fancyzones,utilitaire",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "Textify",
            Aliases = "Textify Tool",
            Publisher = "Michael Maltsev",
            ExecutableNames = "Textify.exe",
            Category = KnowledgeCategory.Utility,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Outil pour copier du texte depuis n'importe quelle fenêtre Windows.",
            ShortDescriptionEn = "Tool to copy text from any Windows window.",
            FullDescription = "Textify permet de sélectionner et copier du texte depuis des boîtes de dialogue, messages d'erreur et autres éléments Windows normalement non sélectionnables. Très utile pour copier des messages d'erreur.",
            FullDescriptionEn = "Textify lets you select and copy text from dialog boxes, error messages and other Windows elements that are normally not selectable. Very useful for copying error messages.",
            DisableImpact = "Vous ne pourrez plus extraire le texte des fenêtres non-sélectionnables avec le raccourci Textify.",
            DisableImpactEn = "You won't be able to extract text from non-selectable windows with Textify shortcut.",
            PerformanceImpact = "Très faible (~5-10 Mo RAM).",
            PerformanceImpactEn = "Very low (~5-10 MB RAM).",
            Recommendation = "Peut être désactivé si vous ne l'utilisez pas régulièrement.",
            RecommendationEn = "Can be disabled if you don't use it regularly.",
            Tags = "textify,texte,copier,utilitaire,accessibilite",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "YoloMouse",
            Aliases = "Yolo Mouse",
            Publisher = "Dragonflame Software",
            ExecutableNames = "YoloMouse.exe",
            Category = KnowledgeCategory.Gaming,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Curseur de souris personnalisé pour les jeux.",
            ShortDescriptionEn = "Custom mouse cursor for games.",
            FullDescription = "YoloMouse remplace le curseur de souris dans les jeux par un curseur plus visible et personnalisable. Utile quand le curseur du jeu est trop petit ou se confond avec le décor.",
            FullDescriptionEn = "YoloMouse replaces mouse cursor in games with a more visible and customizable one. Useful when the game cursor is too small or blends with the background.",
            DisableImpact = "Le curseur personnalisé ne sera pas disponible dans les jeux.",
            DisableImpactEn = "Custom cursor won't be available in games.",
            PerformanceImpact = "Très faible (~10-20 Mo RAM).",
            PerformanceImpactEn = "Very low (~10-20 MB RAM).",
            Recommendation = "Peut être désactivé si vous n'avez pas de problème à voir votre curseur dans les jeux.",
            RecommendationEn = "Can be disabled if you have no trouble seeing your cursor in games.",
            Tags = "yolomouse,curseur,souris,gaming,accessibilite",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "RoboForm",
            Aliases = "AI RoboForm,RoboForm Password Manager",
            Publisher = "Siber Systems Inc",
            ExecutableNames = "RoboTaskBarIcon.exe,RoboForm.exe",
            Category = KnowledgeCategory.Security,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Gestionnaire de mots de passe et remplissage de formulaires.",
            ShortDescriptionEn = "Password manager and form filler.",
            FullDescription = "RoboForm stocke vos mots de passe de manière sécurisée et les remplit automatiquement sur les sites web. Offre aussi le remplissage de formulaires et la génération de mots de passe forts.",
            FullDescriptionEn = "RoboForm securely stores your passwords and auto-fills them on websites. Also offers form filling and strong password generation.",
            DisableImpact = "Pas de remplissage automatique des mots de passe. Vous devrez lancer RoboForm manuellement.",
            DisableImpactEn = "No automatic password filling. You'll need to launch RoboForm manually.",
            PerformanceImpact = "Faible (~30-50 Mo RAM).",
            PerformanceImpactEn = "Low (~30-50 MB RAM).",
            Recommendation = "Gardez activé si vous utilisez RoboForm comme gestionnaire de mots de passe principal.",
            RecommendationEn = "Keep enabled if you use RoboForm as your main password manager.",
            Tags = "roboform,password,mot de passe,securite,formulaire",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "VMware Workstation",
            Aliases = "VMware Player,vmware-tray",
            Publisher = "VMware, Inc.",
            ExecutableNames = "vmware-tray.exe,vmware.exe,vmnat.exe,vmnetdhcp.exe",
            Category = KnowledgeCategory.Utility,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Logiciel de virtualisation pour exécuter d'autres OS.",
            ShortDescriptionEn = "Professional virtualization software.",
            FullDescription = "VMware Workstation permet d'exécuter des machines virtuelles (Windows, Linux, etc.) sur votre PC. L'icône de la barre des tâches donne un accès rapide aux VMs et aux paramètres réseau virtuels.",
            FullDescriptionEn = "VMware Workstation is a professional virtualization solution to run multiple operating systems on a single PC.",
            DisableImpact = "Les VMs partagées ne démarreront pas automatiquement. Pas d'icône dans la barre des tâches.",
            DisableImpactEn = "VMware services will not be available immediately.",
            PerformanceImpact = "Les services réseau utilisent ~20-40 Mo RAM même sans VM active.",
            PerformanceImpactEn = "High while running VMs.",
            Recommendation = "Peut être désactivé si vous n'utilisez pas de VMs partagées. Les VMs fonctionneront toujours.",
            RecommendationEn = "Services can start automatically if needed.",
            Tags = "vmware,virtualisation,vm,machine virtuelle,hyperviseur",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "Brother Software Update",
            Aliases = "BrotherSoftwareUpdateNotification,Brother Update",
            Publisher = "Brother Industries, Ltd.",
            ExecutableNames = "SoftwareUpdateNotificationService.exe",
            Category = KnowledgeCategory.Hardware,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Service de notification des mises à jour Brother.",
            ShortDescriptionEn = "Brother update notification service.",
            FullDescription = "Vérifie et notifie les mises à jour disponibles pour les pilotes et logiciels de vos imprimantes/scanners Brother.",
            FullDescriptionEn = "Checks and notifies available updates for Brother printer/scanner drivers and software.",
            DisableImpact = "Pas de notification des mises à jour Brother. Vous devrez vérifier manuellement sur le site Brother.",
            DisableImpactEn = "No Brother update notifications. You'll need to check manually on Brother's website.",
            PerformanceImpact = "Très faible (~10-20 Mo RAM).",
            PerformanceImpactEn = "Very low (~10-20 MB RAM).",
            Recommendation = "Peut être désactivé. Vérifiez occasionnellement les mises à jour sur le site Brother.",
            RecommendationEn = "Can be disabled. Check updates occasionally on Brother's website.",
            Tags = "brother,imprimante,scanner,mise a jour,pilote",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "Brother iPrint&Scan",
            Aliases = "BrIPSScan,Brother ControlCenter",
            Publisher = "Brother Industries, Ltd.",
            ExecutableNames = "Brother iPrint&Scan.exe,ControlCenter4.exe",
            Category = KnowledgeCategory.Hardware,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Application de numérisation et impression Brother.",
            ShortDescriptionEn = "Brother scanning and printing application.",
            FullDescription = "Brother iPrint&Scan permet de numériser des documents, d'imprimer des photos et de gérer vos imprimantes Brother. ControlCenter offre un accès rapide aux fonctions de numérisation.",
            FullDescriptionEn = "Brother iPrint&Scan scans documents, prints photos and manages your Brother printers. ControlCenter offers quick access to scanning functions.",
            DisableImpact = "Pas de raccourci rapide pour la numérisation. L'imprimante fonctionne toujours normalement.",
            DisableImpactEn = "No quick shortcut for scanning. Printer still works normally.",
            PerformanceImpact = "Faible (~20-40 Mo RAM).",
            PerformanceImpactEn = "Low (~20-40 MB RAM).",
            Recommendation = "Peut être désactivé si vous ne numérisez pas régulièrement.",
            RecommendationEn = "Can be disabled if you don't scan regularly.",
            Tags = "brother,scanner,numerisation,imprimante,controlcenter",
            LastUpdated = DateTime.Now
        });
    }

    private void SeedPrintersAndScanners()
    {
        Save(new KnowledgeEntry
        {
            Name = "HP Smart",
            Aliases = "HP Printer Assistant,HP Solution Center",
            Publisher = "HP Inc.",
            ExecutableNames = "HPSmart.exe,HPPrinterAssistant.exe",
            Category = KnowledgeCategory.Hardware,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Application de gestion des imprimantes HP.",
            ShortDescriptionEn = "HP printer management app.",
            FullDescription = "HP Smart permet de configurer, surveiller et gérer vos imprimantes HP. Offre la numérisation, l'impression mobile, la commande de cartouches et le suivi de l'état de l'imprimante.",
            FullDescriptionEn = "HP Smart allows configuring, printing, scanning and managing HP printers. Includes diagnostic features.",
            DisableImpact = "Pas de notifications d'encre faible. L'imprimante fonctionne toujours normalement.",
            DisableImpactEn = "Printing remains possible, but some advanced features won't be available.",
            PerformanceImpact = "Modéré (~50-100 Mo RAM).",
            PerformanceImpactEn = "Low.",
            Recommendation = "Peut être désactivé. Lancez HP Smart manuellement quand nécessaire.",
            RecommendationEn = "Can be disabled if you don't use advanced features.",
            Tags = "hp,imprimante,smart,scanner,cartouche",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "HP Scan",
            Aliases = "HP Scan Software,HPQTRA08",
            Publisher = "HP Inc.",
            ExecutableNames = "HPScan.exe,hpqtra08.exe",
            Category = KnowledgeCategory.Hardware,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Logiciel de numérisation HP.",
            ShortDescriptionEn = "HP scanning software.",
            FullDescription = "HP Scan fournit une interface pour numériser des documents et photos avec les scanners HP. Offre des préréglages de numérisation et l'OCR.",
            FullDescriptionEn = "HP Scan provides an interface for scanning documents and photos with HP scanners. Offers scan presets and OCR.",
            DisableImpact = "Pas d'icône de raccourci. La numérisation est toujours possible via HP Smart ou Windows.",
            DisableImpactEn = "No shortcut icon. Scanning still possible via HP Smart or Windows.",
            PerformanceImpact = "Faible (~20-30 Mo RAM).",
            PerformanceImpactEn = "Low (~20-30 MB RAM).",
            Recommendation = "Peut être désactivé si vous n'utilisez pas le scanner fréquemment.",
            RecommendationEn = "Can be disabled if you don't use the scanner frequently.",
            Tags = "hp,scanner,numerisation,ocr",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "Canon My Printer",
            Aliases = "Canon IJ Network Tool,CNMNSUT",
            Publisher = "Canon Inc.",
            ExecutableNames = "CNMNSUT.exe,MyPrinter.exe,CaptSvcApp.exe",
            Category = KnowledgeCategory.Hardware,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Utilitaire de configuration imprimante Canon.",
            ShortDescriptionEn = "Canon printer configuration utility.",
            FullDescription = "Canon My Printer permet de configurer les paramètres d'imprimante par défaut, vérifier les niveaux d'encre et diagnostiquer les problèmes d'impression Canon.",
            FullDescriptionEn = "Canon My Printer configures default printer settings, checks ink levels and diagnoses Canon printing issues.",
            DisableImpact = "Pas d'accès rapide aux réglages. L'imprimante fonctionne normalement.",
            DisableImpactEn = "No quick access to settings. Printer works normally.",
            PerformanceImpact = "Faible (~15-30 Mo RAM).",
            PerformanceImpactEn = "Low (~15-30 MB RAM).",
            Recommendation = "Peut être désactivé. Lancez-le manuellement si besoin.",
            RecommendationEn = "Can be disabled. Launch manually if needed.",
            Tags = "canon,imprimante,configuration,encre",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "Canon IJ Scan Utility",
            Aliases = "Canon Scanner,IJ Scan Utility",
            Publisher = "Canon Inc.",
            ExecutableNames = "Canon IJ Scan Utility.exe,IJPLMUI.exe",
            Category = KnowledgeCategory.Hardware,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Utilitaire de numérisation Canon.",
            ShortDescriptionEn = "Canon scanning utility.",
            FullDescription = "Canon IJ Scan Utility offre une interface de numérisation complète pour les multifonctions Canon. Permet de numériser en PDF, JPEG, avec OCR et détection automatique du type de document.",
            FullDescriptionEn = "Canon IJ Scan Utility offers a complete scanning interface for Canon all-in-ones. Scans to PDF, JPEG, with OCR and automatic document type detection.",
            DisableImpact = "Pas de raccourci de numérisation. La numérisation reste possible via Windows.",
            DisableImpactEn = "No scanning shortcut. Scanning still possible via Windows.",
            PerformanceImpact = "Faible (~20-40 Mo RAM).",
            PerformanceImpactEn = "Low (~20-40 MB RAM).",
            Recommendation = "Peut être désactivé si vous n'utilisez pas souvent le scanner.",
            RecommendationEn = "Can be disabled if you don't use the scanner often.",
            Tags = "canon,scanner,numerisation,ocr,pdf",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "Epson Scan",
            Aliases = "Epson Scan 2,EPSON Scanner",
            Publisher = "Seiko Epson Corporation",
            ExecutableNames = "Epson Scan 2.exe,escndv.exe,eslogsvc.exe",
            Category = KnowledgeCategory.Hardware,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Logiciel de numérisation Epson.",
            ShortDescriptionEn = "Epson scanning software.",
            FullDescription = "Epson Scan 2 offre des options de numérisation avancées : correction des couleurs, suppression de la poussière, numérisation de films/négatifs, et préréglages personnalisables.",
            FullDescriptionEn = "Epson Scan 2 offers advanced scanning options: color correction, dust removal, film/negative scanning, and customizable presets.",
            DisableImpact = "Pas de raccourci de numérisation. Utilisez Windows Scan ou lancez Epson Scan manuellement.",
            DisableImpactEn = "No scanning shortcut. Use Windows Scan or launch Epson Scan manually.",
            PerformanceImpact = "Faible (~20-40 Mo RAM).",
            PerformanceImpactEn = "Low (~20-40 MB RAM).",
            Recommendation = "Peut être désactivé si vous ne numérisez pas régulièrement.",
            RecommendationEn = "Can be disabled if you don't scan regularly.",
            Tags = "epson,scanner,numerisation,photo,film",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "Epson Status Monitor",
            Aliases = "Epson Printer Utility,EPSONSSM",
            Publisher = "Seiko Epson Corporation",
            ExecutableNames = "E_YATIHVE.EXE,EPSONSSM.exe",
            Category = KnowledgeCategory.Hardware,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Moniteur d'état pour imprimantes Epson.",
            ShortDescriptionEn = "Status monitor for Epson printers.",
            FullDescription = "Epson Status Monitor surveille l'état de l'imprimante : niveaux d'encre, erreurs, et progression des travaux d'impression. Affiche des alertes quand l'encre est faible.",
            FullDescriptionEn = "Epson Status Monitor tracks printer status: ink levels, errors, and print job progress. Displays alerts when ink is low.",
            DisableImpact = "Pas d'alertes d'encre faible ou d'erreurs. L'impression fonctionne normalement.",
            DisableImpactEn = "No low ink or error alerts. Printing works normally.",
            PerformanceImpact = "Très faible (~10-20 Mo RAM).",
            PerformanceImpactEn = "Very low (~10-20 MB RAM).",
            Recommendation = "Peut être désactivé. Vérifiez manuellement les niveaux d'encre.",
            RecommendationEn = "Can be disabled. Check ink levels manually.",
            Tags = "epson,imprimante,encre,monitoring,status",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "Epson Software Updater",
            Aliases = "Epson Update,EPSON Software Updater",
            Publisher = "Seiko Epson Corporation",
            ExecutableNames = "EPSONUS.exe,ESUPDATE.EXE",
            Category = KnowledgeCategory.Hardware,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Service de mise à jour Epson.",
            ShortDescriptionEn = "Epson printer software.",
            FullDescription = "Epson Software Updater vérifie et télécharge les mises à jour de pilotes et logiciels pour vos produits Epson.",
            FullDescriptionEn = "Epson software suite including updater, event manager and scanning tools.",
            DisableImpact = "Pas de notification des mises à jour Epson.",
            DisableImpactEn = "Automatic updates and some scanning features might not work.",
            PerformanceImpact = "Très faible (~10-15 Mo RAM).",
            PerformanceImpactEn = "Low.",
            Recommendation = "Peut être désactivé. Vérifiez les mises à jour sur le site Epson occasionnellement.",
            RecommendationEn = "Software updater can be disabled.",
            Tags = "epson,mise a jour,pilote,update",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "Samsung Printer Experience",
            Aliases = "Samsung Easy Printer Manager,Samsung Scan",
            Publisher = "Samsung Electronics Co., Ltd.",
            ExecutableNames = "Samsung Printer Experience.exe,SPPrintMon.exe",
            Category = KnowledgeCategory.Hardware,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Gestionnaire d'imprimantes Samsung (maintenant HP).",
            ShortDescriptionEn = "Samsung printer manager (now HP).",
            FullDescription = "Samsung Printer Experience (maintenant repris par HP) gère les imprimantes Samsung : numérisation, paramètres, niveaux de toner et diagnostics.",
            FullDescriptionEn = "Samsung Printer Experience (now taken over by HP) manages Samsung printers: scanning, settings, toner levels and diagnostics.",
            DisableImpact = "Pas de notifications de toner faible. L'imprimante fonctionne normalement.",
            DisableImpactEn = "No low toner notifications. Printer works normally.",
            PerformanceImpact = "Faible (~20-40 Mo RAM).",
            PerformanceImpactEn = "Low (~20-40 MB RAM).",
            Recommendation = "Peut être désactivé. Samsung a été racheté par HP, utilisez HP Smart pour les nouvelles imprimantes.",
            RecommendationEn = "Can be disabled. Samsung was acquired by HP, use HP Smart for new printers.",
            Tags = "samsung,imprimante,toner,scanner",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "Lexmark Printer Software",
            Aliases = "Lexmark Printer Home,LexmarkPrintAgent",
            Publisher = "Lexmark International, Inc.",
            ExecutableNames = "LexPrintAgent.exe,lxdpmon.exe",
            Category = KnowledgeCategory.Hardware,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Logiciel de gestion d'imprimantes Lexmark.",
            ShortDescriptionEn = "Lexmark printer management software.",
            FullDescription = "Le logiciel Lexmark permet de surveiller l'état de l'imprimante, les niveaux de toner/encre et de configurer les paramètres d'impression.",
            FullDescriptionEn = "Lexmark software monitors printer status, toner/ink levels and configures print settings.",
            DisableImpact = "Pas de surveillance d'état. L'impression fonctionne normalement.",
            DisableImpactEn = "No status monitoring. Printing works normally.",
            PerformanceImpact = "Faible (~15-30 Mo RAM).",
            PerformanceImpactEn = "Low (~15-30 MB RAM).",
            Recommendation = "Peut être désactivé si vous n'avez pas besoin des alertes.",
            RecommendationEn = "Can be disabled if you don't need alerts.",
            Tags = "lexmark,imprimante,toner,monitoring",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "Xerox Print Experience",
            Aliases = "Xerox Desktop Print Experience,Xerox Scan",
            Publisher = "Xerox Corporation",
            ExecutableNames = "XeroxPrintExperience.exe,XeroxScanApp.exe",
            Category = KnowledgeCategory.Hardware,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Application de gestion d'imprimantes Xerox.",
            ShortDescriptionEn = "Xerox printer management application.",
            FullDescription = "Xerox Print Experience offre une interface moderne pour imprimer, numériser et gérer vos imprimantes Xerox. Inclut des fonctionnalités de workflow et de conversion.",
            FullDescriptionEn = "Xerox Print Experience offers a modern interface to print, scan and manage your Xerox printers. Includes workflow and conversion features.",
            DisableImpact = "Pas de raccourcis Xerox. L'impression via Windows fonctionne normalement.",
            DisableImpactEn = "No Xerox shortcuts. Printing via Windows works normally.",
            PerformanceImpact = "Modéré (~40-70 Mo RAM).",
            PerformanceImpactEn = "Moderate (~40-70 MB RAM).",
            Recommendation = "Peut être désactivé si vous n'utilisez pas les fonctionnalités avancées.",
            RecommendationEn = "Can be disabled if you don't use advanced features.",
            Tags = "xerox,imprimante,scanner,entreprise",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "Kofax PaperPort",
            Aliases = "PaperPort,Nuance PaperPort",
            Publisher = "Kofax, Inc.",
            ExecutableNames = "PPLaunch.exe,PaperPort.exe",
            Category = KnowledgeCategory.Productivity,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Logiciel de gestion documentaire et numérisation.",
            ShortDescriptionEn = "Document management and scanning software.",
            FullDescription = "PaperPort est un logiciel de gestion documentaire qui permet de numériser, organiser et convertir des documents. Offre l'OCR et la création de PDF recherchables. Souvent fourni avec les scanners.",
            FullDescriptionEn = "PaperPort is document management software that scans, organizes and converts documents. Offers OCR and searchable PDF creation. Often bundled with scanners.",
            DisableImpact = "Pas de lancement automatique de PaperPort. La numérisation est toujours possible.",
            DisableImpactEn = "No automatic PaperPort launch. Scanning still possible.",
            PerformanceImpact = "Modéré (~50-100 Mo RAM).",
            PerformanceImpactEn = "Moderate (~50-100 MB RAM).",
            Recommendation = "Peut être désactivé. Lancez PaperPort manuellement quand vous numérisez.",
            RecommendationEn = "Can be disabled. Launch PaperPort manually when scanning.",
            Tags = "paperport,scanner,ocr,pdf,document",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "ABBYY FineReader",
            Aliases = "FineReader,ABBYY Screenshot Reader",
            Publisher = "ABBYY",
            ExecutableNames = "FineReader.exe,ScreenshotReader.exe",
            Category = KnowledgeCategory.Productivity,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Logiciel OCR professionnel pour reconnaissance de texte.",
            ShortDescriptionEn = "Professional OCR software for text recognition.",
            FullDescription = "ABBYY FineReader est l'un des meilleurs logiciels OCR. Convertit les documents numérisés et PDF en fichiers éditables avec une grande précision, même pour les documents complexes.",
            FullDescriptionEn = "ABBYY FineReader is one of the best OCR software. Converts scanned documents and PDFs to editable files with high accuracy, even for complex documents.",
            DisableImpact = "Pas de raccourcis OCR. Lancez FineReader manuellement.",
            DisableImpactEn = "No OCR shortcuts. Launch FineReader manually.",
            PerformanceImpact = "Faible au repos (~20-40 Mo RAM).",
            PerformanceImpactEn = "Low when idle (~20-40 MB RAM).",
            Recommendation = "Peut être désactivé si vous n'utilisez pas l'OCR fréquemment.",
            RecommendationEn = "Can be disabled if you don't use OCR frequently.",
            Tags = "abbyy,ocr,finereader,pdf,reconnaissance",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "NAPS2",
            Aliases = "Not Another PDF Scanner,NAPS2 Scanner",
            Publisher = "NAPS2",
            ExecutableNames = "NAPS2.exe",
            Category = KnowledgeCategory.Utility,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Logiciel de numérisation gratuit et open source.",
            ShortDescriptionEn = "Free and open source scanning software.",
            FullDescription = "NAPS2 (Not Another PDF Scanner) est un outil gratuit pour numériser vers PDF, TIFF ou images. Simple mais puissant avec OCR intégré et support de tous les scanners TWAIN/WIA.",
            FullDescriptionEn = "NAPS2 (Not Another PDF Scanner) is a free tool to scan to PDF, TIFF or images. Simple yet powerful with built-in OCR and support for all TWAIN/WIA scanners.",
            DisableImpact = "Aucun impact. NAPS2 est un outil à lancer manuellement.",
            DisableImpactEn = "No impact. NAPS2 is a manually launched tool.",
            PerformanceImpact = "Aucun au repos.",
            PerformanceImpactEn = "None when idle.",
            Recommendation = "Ne devrait pas être dans le démarrage automatique.",
            RecommendationEn = "Shouldn't be in auto-start.",
            Tags = "naps2,scanner,pdf,gratuit,open source",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "VueScan",
            Aliases = "VueScan Scanner",
            Publisher = "Hamrick Software",
            ExecutableNames = "vuescan.exe",
            Category = KnowledgeCategory.Utility,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Logiciel de numérisation universel.",
            ShortDescriptionEn = "Universal scanning software.",
            FullDescription = "VueScan est un logiciel de numérisation qui supporte plus de 7000 scanners, y compris les anciens modèles sans pilotes Windows récents. Excellent pour la numérisation de films et photos.",
            FullDescriptionEn = "VueScan is scanning software that supports over 7000 scanners, including old models without recent Windows drivers. Excellent for film and photo scanning.",
            DisableImpact = "Aucun impact. VueScan est lancé manuellement.",
            DisableImpactEn = "No impact. VueScan is launched manually.",
            PerformanceImpact = "Aucun au repos.",
            PerformanceImpactEn = "None when idle.",
            Recommendation = "Ne devrait pas être dans le démarrage automatique.",
            RecommendationEn = "Shouldn't be in auto-start.",
            Tags = "vuescan,scanner,film,photo,universel",
            LastUpdated = DateTime.Now
        });
    }

    private void SeedDrawingTablets()
    {
        Save(new KnowledgeEntry
        {
            Name = "Wacom Tablet",
            Aliases = "Wacom Desktop Center,Wacom Tablet Driver,WacomTablet",
            Publisher = "Wacom Technology Corp.",
            ExecutableNames = "WacomDesktopCenter.exe,Wacom_Tablet.exe,WTabletServicePro.exe",
            Category = KnowledgeCategory.Hardware,
            SafetyLevel = SafetyLevel.Important,
            ShortDescription = "Pilote et utilitaire pour tablettes graphiques Wacom.",
            ShortDescriptionEn = "Driver and utility for Wacom graphics tablets.",
            FullDescription = "Le pilote Wacom est essentiel pour les tablettes graphiques Wacom (Intuos, Cintiq, etc.). Gère la pression du stylet, les boutons personnalisés, les raccourcis et les paramètres par application.",
            FullDescriptionEn = "Wacom driver is essential for Wacom graphics tablets (Intuos, Cintiq, etc.). Manages pen pressure, custom buttons, shortcuts and per-application settings.",
            DisableImpact = "La tablette graphique pourrait ne pas fonctionner correctement. Perte de la sensibilité à la pression et des boutons personnalisés.",
            DisableImpactEn = "Graphics tablet may not work correctly. Loss of pressure sensitivity and custom buttons.",
            PerformanceImpact = "Faible (~30-50 Mo RAM).",
            PerformanceImpactEn = "Low (~30-50 MB RAM).",
            Recommendation = "Gardez activé si vous utilisez une tablette Wacom. Essentiel pour les artistes et designers.",
            RecommendationEn = "Keep enabled if you use a Wacom tablet. Essential for artists and designers.",
            Tags = "wacom,tablette,graphique,stylet,dessin,intuos,cintiq",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "XP-Pen",
            Aliases = "XP-Pen Tablet,PenTablet",
            Publisher = "XP-Pen Technology Co.",
            ExecutableNames = "PenTablet.exe,XPPenTablet.exe",
            Category = KnowledgeCategory.Hardware,
            SafetyLevel = SafetyLevel.Important,
            ShortDescription = "Pilote pour tablettes graphiques XP-Pen.",
            ShortDescriptionEn = "Driver for XP-Pen graphics tablets.",
            FullDescription = "Le pilote XP-Pen configure les tablettes graphiques XP-Pen : sensibilité à la pression, touches express, paramètres de zone de travail et raccourcis par application.",
            FullDescriptionEn = "XP-Pen driver configures XP-Pen graphics tablets: pressure sensitivity, express keys, work area settings and per-application shortcuts.",
            DisableImpact = "La tablette pourrait ne pas fonctionner correctement. Perte de la pression et des boutons.",
            DisableImpactEn = "Tablet may not work correctly. Loss of pressure and buttons.",
            PerformanceImpact = "Faible (~20-40 Mo RAM).",
            PerformanceImpactEn = "Low (~20-40 MB RAM).",
            Recommendation = "Gardez activé si vous utilisez une tablette XP-Pen.",
            RecommendationEn = "Keep enabled if you use an XP-Pen tablet.",
            Tags = "xppen,tablette,graphique,stylet,dessin",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "Huion Tablet",
            Aliases = "HuionTablet,Huion Driver",
            Publisher = "Huion Animation Technology Co., Ltd.",
            ExecutableNames = "HuionTablet.exe,HuionDriverUI.exe",
            Category = KnowledgeCategory.Hardware,
            SafetyLevel = SafetyLevel.Important,
            ShortDescription = "Pilote pour tablettes graphiques Huion.",
            ShortDescriptionEn = "Driver for Huion graphics tablets.",
            FullDescription = "Le pilote Huion gère les tablettes graphiques Huion : sensibilité à la pression (8192+ niveaux), touches programmables et paramètres d'affichage pour les pen displays.",
            FullDescriptionEn = "Huion driver manages Huion graphics tablets: pressure sensitivity (8192+ levels), programmable keys and display settings for pen displays.",
            DisableImpact = "La tablette ne fonctionnera pas correctement sans le pilote actif.",
            DisableImpactEn = "Tablet won't work correctly without active driver.",
            PerformanceImpact = "Faible (~20-40 Mo RAM).",
            PerformanceImpactEn = "Low (~20-40 MB RAM).",
            Recommendation = "Gardez activé si vous utilisez une tablette Huion.",
            RecommendationEn = "Keep enabled if you use a Huion tablet.",
            Tags = "huion,tablette,graphique,stylet,dessin",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "Gaomon Tablet",
            Aliases = "Gaomon Driver,GaomonTablet",
            Publisher = "Gaomon Technology Corporation",
            ExecutableNames = "GaomonTablet.exe,Gaomon.exe",
            Category = KnowledgeCategory.Hardware,
            SafetyLevel = SafetyLevel.Important,
            ShortDescription = "Pilote pour tablettes graphiques Gaomon.",
            ShortDescriptionEn = "Driver for Gaomon graphics tablets.",
            FullDescription = "Le pilote Gaomon configure les tablettes graphiques Gaomon avec les paramètres de pression, les touches express et la zone de travail.",
            FullDescriptionEn = "Gaomon driver configures Gaomon graphics tablets with pressure settings, express keys and work area.",
            DisableImpact = "La tablette ne fonctionnera pas correctement.",
            DisableImpactEn = "Tablet won't work correctly.",
            PerformanceImpact = "Faible (~15-30 Mo RAM).",
            PerformanceImpactEn = "Low (~15-30 MB RAM).",
            Recommendation = "Gardez activé si vous utilisez une tablette Gaomon.",
            RecommendationEn = "Keep enabled if you use a Gaomon tablet.",
            Tags = "gaomon,tablette,graphique,stylet,dessin",
            LastUpdated = DateTime.Now
        });
    }

    private void SeedWebcamsAndMicrophones()
    {
        Save(new KnowledgeEntry
        {
            Name = "Logitech Capture",
            Aliases = "Logi Capture",
            Publisher = "Logitech",
            ExecutableNames = "LogiCapture.exe",
            Category = KnowledgeCategory.Hardware,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Logiciel de capture vidéo pour webcams Logitech.",
            ShortDescriptionEn = "Video capture software for Logitech webcams.",
            FullDescription = "Logitech Capture permet de configurer les webcams Logitech (Brio, C920, StreamCam, etc.), de capturer des vidéos, d'ajouter des effets et de diffuser en direct.",
            FullDescriptionEn = "Logitech Capture configures Logitech webcams (Brio, C920, StreamCam, etc.), captures videos, adds effects and streams live.",
            DisableImpact = "Pas de configuration avancée de la webcam. La webcam fonctionne toujours avec les paramètres par défaut.",
            DisableImpactEn = "No advanced webcam configuration. Webcam still works with default settings.",
            PerformanceImpact = "Modéré (~60-100 Mo RAM).",
            PerformanceImpactEn = "Moderate (~60-100 MB RAM).",
            Recommendation = "Peut être désactivé si vous n'utilisez pas les fonctionnalités avancées de capture.",
            RecommendationEn = "Can be disabled if you don't use advanced capture features.",
            Tags = "logitech,webcam,capture,streaming,video",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "Elgato Camera Hub",
            Aliases = "Camera Hub,Elgato Facecam",
            Publisher = "Corsair Memory, Inc.",
            ExecutableNames = "Camera Hub.exe,CameraHub.exe",
            Category = KnowledgeCategory.Hardware,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Logiciel de configuration pour webcams Elgato.",
            ShortDescriptionEn = "Configuration software for Elgato webcams.",
            FullDescription = "Elgato Camera Hub configure les paramètres avancés des webcams Elgato Facecam : exposition, balance des blancs, HDR, zoom et cadrage.",
            FullDescriptionEn = "Elgato Camera Hub configures advanced Elgato Facecam webcam settings: exposure, white balance, HDR, zoom and framing.",
            DisableImpact = "Les paramètres personnalisés de la webcam ne seront pas appliqués.",
            DisableImpactEn = "Custom webcam settings won't be applied.",
            PerformanceImpact = "Faible (~30-50 Mo RAM).",
            PerformanceImpactEn = "Low (~30-50 MB RAM).",
            Recommendation = "Gardez activé si vous utilisez une Elgato Facecam avec des paramètres personnalisés.",
            RecommendationEn = "Keep enabled if you use an Elgato Facecam with custom settings.",
            Tags = "elgato,webcam,facecam,streaming,configuration",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "Elgato Stream Deck",
            Aliases = "Stream Deck,StreamDeck",
            Publisher = "Corsair Memory, Inc.",
            ExecutableNames = "StreamDeck.exe",
            Category = KnowledgeCategory.Hardware,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Logiciel pour contrôleurs Stream Deck.",
            ShortDescriptionEn = "Elgato Stream Deck manager.",
            FullDescription = "Stream Deck configure les boutons LCD programmables du Stream Deck. Permet de créer des raccourcis, contrôler OBS, gérer les scènes de stream et automatiser des actions.",
            FullDescriptionEn = "Elgato Stream Deck configures the Stream Deck, a control panel with customizable LCD keys for streaming and productivity.",
            DisableImpact = "Le Stream Deck n'affichera pas les boutons personnalisés et les actions ne fonctionneront pas.",
            DisableImpactEn = "Stream Deck won't work correctly.",
            PerformanceImpact = "Modéré (~50-80 Mo RAM).",
            PerformanceImpactEn = "Low.",
            Recommendation = "Gardez activé si vous utilisez un Stream Deck. Essentiel pour le streaming.",
            RecommendationEn = "Keep enabled if you use a Stream Deck.",
            Tags = "elgato,streamdeck,streaming,raccourci,obs",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "Elgato Wave Link",
            Aliases = "Wave Link,WaveLink",
            Publisher = "Corsair Memory, Inc.",
            ExecutableNames = "WaveLink.exe",
            Category = KnowledgeCategory.Hardware,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Mixeur audio logiciel pour microphones Elgato Wave.",
            ShortDescriptionEn = "Software audio mixer for Elgato Wave microphones.",
            FullDescription = "Wave Link est un mixeur audio virtuel pour les microphones Elgato Wave. Permet de mixer plusieurs sources audio, d'appliquer des effets et de gérer le monitoring pour le streaming.",
            FullDescriptionEn = "Wave Link is a virtual audio mixer for Elgato Wave microphones. Mixes multiple audio sources, applies effects and manages monitoring for streaming.",
            DisableImpact = "Le mixage audio Wave Link ne sera pas disponible. Le micro fonctionnera comme un périphérique USB standard.",
            DisableImpactEn = "Wave Link audio mixing won't be available. Mic will work as standard USB device.",
            PerformanceImpact = "Modéré (~50-80 Mo RAM).",
            PerformanceImpactEn = "Moderate (~50-80 MB RAM).",
            Recommendation = "Gardez activé si vous utilisez un micro Elgato Wave pour le streaming.",
            RecommendationEn = "Keep enabled if you use an Elgato Wave mic for streaming.",
            Tags = "elgato,wave,microphone,audio,mixeur,streaming",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "Blue Sherpa",
            Aliases = "Blue VO!CE,Logitech G Hub Blue",
            Publisher = "Logitech",
            ExecutableNames = "Blue Sherpa.exe,BlueSherpa.exe",
            Category = KnowledgeCategory.Hardware,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Logiciel pour microphones Blue (Yeti, Snowball).",
            ShortDescriptionEn = "Software for Blue microphones (Yeti, Snowball).",
            FullDescription = "Blue Sherpa configure les microphones Blue (maintenant Logitech) : gain, pattern de capture, monitoring. Blue VO!CE ajoute des effets vocaux et la suppression de bruit.",
            FullDescriptionEn = "Blue Sherpa configures Blue microphones (now Logitech): gain, pickup pattern, monitoring. Blue VO!CE adds vocal effects and noise suppression.",
            DisableImpact = "Pas d'accès aux paramètres avancés du micro. Les paramètres de base restent via Windows.",
            DisableImpactEn = "No access to advanced mic settings. Basic settings remain via Windows.",
            PerformanceImpact = "Faible (~30-50 Mo RAM).",
            PerformanceImpactEn = "Low (~30-50 MB RAM).",
            Recommendation = "Peut être désactivé si vous n'utilisez pas Blue VO!CE ou les paramètres avancés.",
            RecommendationEn = "Can be disabled if you don't use Blue VO!CE or advanced settings.",
            Tags = "blue,yeti,microphone,audio,logitech",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "RØDE Central",
            Aliases = "Rode Central,RODE Connect",
            Publisher = "RØDE Microphones",
            ExecutableNames = "RØDE Central.exe,RODE Central.exe",
            Category = KnowledgeCategory.Hardware,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Logiciel pour microphones et périphériques RØDE.",
            ShortDescriptionEn = "Software for RØDE microphones and devices.",
            FullDescription = "RØDE Central configure les microphones RØDE (NT-USB, PodMic USB, etc.) et le RØDECaster. Permet les mises à jour firmware, la configuration audio et l'enregistrement.",
            FullDescriptionEn = "RØDE Central configures RØDE microphones (NT-USB, PodMic USB, etc.) and RØDECaster. Enables firmware updates, audio configuration and recording.",
            DisableImpact = "Pas d'accès aux paramètres RØDE. Le micro fonctionne avec les paramètres par défaut.",
            DisableImpactEn = "No access to RØDE settings. Mic works with default settings.",
            PerformanceImpact = "Faible (~30-50 Mo RAM).",
            PerformanceImpactEn = "Low (~30-50 MB RAM).",
            Recommendation = "Peut être désactivé si vous n'avez pas besoin des fonctionnalités avancées.",
            RecommendationEn = "Can be disabled if you don't need advanced features.",
            Tags = "rode,microphone,audio,podcast,usb",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "GoXLR App",
            Aliases = "GoXLR,TC-Helicon GoXLR",
            Publisher = "TC-Helicon",
            ExecutableNames = "GoXLR App.exe,GoXLRApp.exe",
            Category = KnowledgeCategory.Hardware,
            SafetyLevel = SafetyLevel.Important,
            ShortDescription = "Logiciel de contrôle pour mixeur GoXLR.",
            ShortDescriptionEn = "Control software for GoXLR mixer.",
            FullDescription = "L'app GoXLR contrôle le mixeur audio GoXLR/GoXLR Mini : routage audio, effets vocaux, sampler, et intégration streaming. Essentiel pour utiliser le matériel.",
            FullDescriptionEn = "GoXLR app controls the GoXLR/GoXLR Mini audio mixer: audio routing, vocal effects, sampler, and streaming integration. Essential for using the hardware.",
            DisableImpact = "Le GoXLR ne fonctionnera pas correctement sans l'application.",
            DisableImpactEn = "GoXLR won't work correctly without the application.",
            PerformanceImpact = "Modéré (~60-100 Mo RAM).",
            PerformanceImpactEn = "Moderate (~60-100 MB RAM).",
            Recommendation = "Gardez activé si vous possédez un GoXLR. Essentiel pour son fonctionnement.",
            RecommendationEn = "Keep enabled if you own a GoXLR. Essential for its operation.",
            Tags = "goxlr,mixeur,audio,streaming,effets",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "VoiceMeeter",
            Aliases = "VoiceMeeter Banana,VoiceMeeter Potato,VBAN",
            Publisher = "VB-Audio Software",
            ExecutableNames = "voicemeeter.exe,voicemeeterpro.exe",
            Category = KnowledgeCategory.Utility,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Mixeur audio virtuel gratuit.",
            ShortDescriptionEn = "Free virtual audio mixer.",
            FullDescription = "VoiceMeeter est un mixeur audio virtuel qui permet de router, mixer et traiter plusieurs sources audio. Très populaire pour le streaming et le podcasting. Versions Banana et Potato pour plus de canaux.",
            FullDescriptionEn = "VoiceMeeter is a virtual audio mixer that routes, mixes and processes multiple audio sources. Very popular for streaming and podcasting. Banana and Potato versions for more channels.",
            DisableImpact = "Tout le routage audio VoiceMeeter sera désactivé. Les applications utilisant les entrées/sorties virtuelles n'auront plus d'audio.",
            DisableImpactEn = "All VoiceMeeter audio routing will be disabled. Applications using virtual inputs/outputs will have no audio.",
            PerformanceImpact = "Faible (~20-40 Mo RAM).",
            PerformanceImpactEn = "Low (~20-40 MB RAM).",
            Recommendation = "Gardez activé si votre configuration audio dépend de VoiceMeeter.",
            RecommendationEn = "Keep enabled if your audio setup depends on VoiceMeeter.",
            Tags = "voicemeeter,audio,mixeur,virtuel,streaming",
            LastUpdated = DateTime.Now
        });
    }

    private void SeedVPN()
    {
        Save(new KnowledgeEntry
        {
            Name = "NordVPN",
            Aliases = "Nord VPN,NordVPN Service",
            Publisher = "nordvpn s.a.",
            ExecutableNames = "NordVPN.exe,nordvpn-service.exe",
            Category = KnowledgeCategory.Security,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Service VPN populaire pour la confidentialité en ligne.",
            ShortDescriptionEn = "Popular VPN service for online privacy.",
            FullDescription = "NordVPN chiffre votre connexion internet et masque votre adresse IP. Offre des serveurs dans 60+ pays, le double VPN, et la protection contre les menaces (CyberSec).",
            FullDescriptionEn = "NordVPN encrypts your internet connection and hides your IP address. Offers servers in 60+ countries, double VPN, and threat protection (CyberSec).",
            DisableImpact = "Le VPN ne se connectera pas automatiquement au démarrage. Vous devrez le lancer manuellement.",
            DisableImpactEn = "VPN won't connect automatically at startup. You'll need to launch it manually.",
            PerformanceImpact = "Faible (~40-60 Mo RAM). Peut légèrement réduire la vitesse internet.",
            PerformanceImpactEn = "Low (~40-60 MB RAM). May slightly reduce internet speed.",
            Recommendation = "Gardez activé si vous voulez une connexion VPN permanente. Peut être désactivé sinon.",
            RecommendationEn = "Keep enabled if you want permanent VPN connection. Can be disabled otherwise.",
            Tags = "nordvpn,vpn,confidentialite,securite,chiffrement",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "ExpressVPN",
            Aliases = "Express VPN",
            Publisher = "ExpressVPN International Ltd.",
            ExecutableNames = "ExpressVPN.exe,expressvpnd.exe",
            Category = KnowledgeCategory.Security,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "VPN premium reconnu pour sa vitesse.",
            ShortDescriptionEn = "Fast premium VPN service.",
            FullDescription = "ExpressVPN offre des connexions rapides avec serveurs dans 94 pays. Connu pour sa fiabilité, son support du streaming et sa politique no-logs.",
            FullDescriptionEn = "ExpressVPN is a premium VPN service recognized for its speed and reliability, with servers in 94 countries.",
            DisableImpact = "Le VPN ne se connectera pas automatiquement.",
            DisableImpactEn = "VPN connection won't be established automatically.",
            PerformanceImpact = "Faible (~40-60 Mo RAM).",
            PerformanceImpactEn = "Low to moderate.",
            Recommendation = "Gardez activé pour une protection VPN permanente.",
            RecommendationEn = "Keep enabled if you need permanent VPN protection.",
            Tags = "expressvpn,vpn,confidentialite,streaming,vitesse",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "Surfshark",
            Aliases = "Surfshark VPN",
            Publisher = "Surfshark Ltd.",
            ExecutableNames = "Surfshark.exe,Surfshark.Service.exe",
            Category = KnowledgeCategory.Security,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "VPN abordable avec connexions illimitées.",
            ShortDescriptionEn = "Affordable VPN with unlimited connections.",
            FullDescription = "Surfshark permet des connexions simultanées illimitées. Inclut CleanWeb (bloqueur de pubs), MultiHop et mode camouflage.",
            FullDescriptionEn = "Surfshark allows unlimited simultaneous connections. Includes CleanWeb (ad blocker), MultiHop and camouflage mode.",
            DisableImpact = "Le VPN ne démarrera pas automatiquement.",
            DisableImpactEn = "VPN won't start automatically.",
            PerformanceImpact = "Faible (~30-50 Mo RAM).",
            PerformanceImpactEn = "Low (~30-50 MB RAM).",
            Recommendation = "Peut être désactivé si vous préférez lancer le VPN manuellement.",
            RecommendationEn = "Can be disabled if you prefer launching VPN manually.",
            Tags = "surfshark,vpn,confidentialite,illimite",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "CyberGhost VPN",
            Aliases = "CyberGhost",
            Publisher = "CyberGhost S.R.L.",
            ExecutableNames = "CyberGhost.exe,CyberGhost.Service.exe",
            Category = KnowledgeCategory.Security,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "VPN convivial avec profils optimisés.",
            ShortDescriptionEn = "User-friendly VPN with optimized profiles.",
            FullDescription = "CyberGhost offre des profils préconfigurés pour le streaming, le torrent et la navigation. Interface simple avec 7000+ serveurs.",
            FullDescriptionEn = "CyberGhost offers preconfigured profiles for streaming, torrenting and browsing. Simple interface with 7000+ servers.",
            DisableImpact = "Le VPN ne se connectera pas au démarrage.",
            DisableImpactEn = "VPN won't connect at startup.",
            PerformanceImpact = "Faible (~40-60 Mo RAM).",
            PerformanceImpactEn = "Low (~40-60 MB RAM).",
            Recommendation = "Peut être désactivé si vous n'avez pas besoin d'un VPN permanent.",
            RecommendationEn = "Can be disabled if you don't need a permanent VPN.",
            Tags = "cyberghost,vpn,confidentialite,streaming",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "ProtonVPN",
            Aliases = "Proton VPN",
            Publisher = "Proton AG",
            ExecutableNames = "ProtonVPN.exe,ProtonVPNService.exe",
            Category = KnowledgeCategory.Security,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "VPN suisse axé sur la confidentialité.",
            ShortDescriptionEn = "Secure VPN from ProtonMail creators.",
            FullDescription = "ProtonVPN est créé par les développeurs de ProtonMail. Offre un niveau gratuit, Secure Core (double VPN via pays sûrs) et une politique stricte no-logs.",
            FullDescriptionEn = "ProtonVPN is a Swiss VPN service focused on privacy, created by the founders of ProtonMail. Offers a free tier.",
            DisableImpact = "Le VPN ne se connectera pas automatiquement.",
            DisableImpactEn = "VPN connection won't be established automatically.",
            PerformanceImpact = "Faible (~30-50 Mo RAM).",
            PerformanceImpactEn = "Low to moderate.",
            Recommendation = "Gardez activé pour une protection permanente.",
            RecommendationEn = "Keep enabled if you need permanent VPN protection.",
            Tags = "protonvpn,vpn,proton,suisse,confidentialite",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "Private Internet Access",
            Aliases = "PIA,PIA VPN",
            Publisher = "Private Internet Access, Inc.",
            ExecutableNames = "pia-client.exe,pia-service.exe",
            Category = KnowledgeCategory.Security,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "VPN avec politique no-logs prouvée en justice.",
            ShortDescriptionEn = "VPN with no-logs policy proven in court.",
            FullDescription = "PIA est un VPN qui a prouvé sa politique no-logs devant les tribunaux. Offre le port forwarding, un bloqueur de pubs (MACE) et WireGuard.",
            FullDescriptionEn = "PIA is a VPN that proved its no-logs policy in court. Offers port forwarding, ad blocker (MACE) and WireGuard.",
            DisableImpact = "Le VPN ne démarrera pas automatiquement.",
            DisableImpactEn = "VPN won't start automatically.",
            PerformanceImpact = "Faible (~30-50 Mo RAM).",
            PerformanceImpactEn = "Low (~30-50 MB RAM).",
            Recommendation = "Peut être désactivé si vous lancez le VPN manuellement.",
            RecommendationEn = "Can be disabled if you launch VPN manually.",
            Tags = "pia,vpn,confidentialite,nologs",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "Windscribe",
            Aliases = "Windscribe VPN",
            Publisher = "Windscribe Limited",
            ExecutableNames = "Windscribe.exe,WindscribeService.exe",
            Category = KnowledgeCategory.Security,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "VPN avec niveau gratuit généreux.",
            ShortDescriptionEn = "VPN with generous free tier.",
            FullDescription = "Windscribe offre 10 Go/mois gratuits. Inclut R.O.B.E.R.T. (bloqueur de pubs/malware), split tunneling et le mode pont pour contourner les censures.",
            FullDescriptionEn = "Windscribe offers 10 GB/month free. Includes R.O.B.E.R.T. (ad/malware blocker), split tunneling and bridge mode to bypass censorship.",
            DisableImpact = "Le VPN ne se connectera pas au démarrage.",
            DisableImpactEn = "VPN won't connect at startup.",
            PerformanceImpact = "Faible (~30-50 Mo RAM).",
            PerformanceImpactEn = "Low (~30-50 MB RAM).",
            Recommendation = "Peut être désactivé si vous n'utilisez pas le VPN en permanence.",
            RecommendationEn = "Can be disabled if you don't use VPN permanently.",
            Tags = "windscribe,vpn,gratuit,confidentialite",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "Mullvad VPN",
            Aliases = "Mullvad",
            Publisher = "Mullvad VPN AB",
            ExecutableNames = "mullvad-vpn.exe,mullvad-daemon.exe",
            Category = KnowledgeCategory.Security,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "VPN ultra-privé avec paiement anonyme.",
            ShortDescriptionEn = "Ultra-private VPN with anonymous payment.",
            FullDescription = "Mullvad est réputé pour sa confidentialité maximale : pas d'email requis, paiement en cash/crypto possible, numéro de compte anonyme. Recommandé par les experts en sécurité.",
            FullDescriptionEn = "Mullvad is known for maximum privacy: no email required, cash/crypto payment possible, anonymous account number. Recommended by security experts.",
            DisableImpact = "Le VPN ne démarrera pas automatiquement.",
            DisableImpactEn = "VPN won't start automatically.",
            PerformanceImpact = "Faible (~20-40 Mo RAM).",
            PerformanceImpactEn = "Low (~20-40 MB RAM).",
            Recommendation = "Gardez activé pour une protection VPN permanente.",
            RecommendationEn = "Keep enabled for permanent VPN protection.",
            Tags = "mullvad,vpn,confidentialite,anonyme,securite",
            LastUpdated = DateTime.Now
        });
    }

    private void SeedPasswordManagers()
    {
        Save(new KnowledgeEntry
        {
            Name = "1Password",
            Aliases = "1Password 7,1Password 8",
            Publisher = "AgileBits Inc.",
            ExecutableNames = "1Password.exe,1Password-BrowserSupport.exe",
            Category = KnowledgeCategory.Security,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Gestionnaire de mots de passe premium.",
            ShortDescriptionEn = "Premium password manager.",
            FullDescription = "1Password stocke vos mots de passe, cartes de crédit et notes sécurisées. Offre Watchtower (surveillance des fuites), le partage familial et l'intégration navigateur.",
            FullDescriptionEn = "1Password stores your passwords, credit cards and secure notes. Offers Watchtower (breach monitoring), family sharing and browser integration.",
            DisableImpact = "Pas de remplissage automatique au démarrage. Vous devrez lancer 1Password manuellement.",
            DisableImpactEn = "No autofill at startup. You will need to launch 1Password manually.",
            PerformanceImpact = "Faible (~50-80 Mo RAM).",
            PerformanceImpactEn = "Low (~50-80 MB RAM).",
            Recommendation = "Gardez activé pour un remplissage automatique des mots de passe.",
            RecommendationEn = "Keep enabled for automatic password filling.",
            Tags = "1password,password,mot de passe,securite,coffre",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "Bitwarden",
            Aliases = "Bitwarden Desktop",
            Publisher = "Bitwarden Inc.",
            ExecutableNames = "Bitwarden.exe",
            Category = KnowledgeCategory.Security,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Gestionnaire de mots de passe open source.",
            ShortDescriptionEn = "Open source password manager.",
            FullDescription = "Bitwarden est un gestionnaire de mots de passe gratuit et open source. Offre le stockage illimité, la synchronisation multi-appareils et peut être auto-hébergé.",
            FullDescriptionEn = "Bitwarden is a free and open source password manager. Offers unlimited storage, multi-device sync and can be self-hosted.",
            DisableImpact = "Pas de raccourci rapide. L'extension navigateur fonctionne indépendamment.",
            DisableImpactEn = "No quick shortcut. Browser extension works independently.",
            PerformanceImpact = "Faible (~40-60 Mo RAM).",
            PerformanceImpactEn = "Low (~40-60 MB RAM).",
            Recommendation = "Peut être désactivé si vous utilisez principalement l'extension navigateur.",
            RecommendationEn = "Can be disabled if you mainly use the browser extension.",
            Tags = "bitwarden,password,mot de passe,open source,gratuit",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "LastPass",
            Aliases = "LastPass Password Manager",
            Publisher = "LastPass US LP",
            ExecutableNames = "LastPass.exe,LastPassBroker.exe",
            Category = KnowledgeCategory.Security,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Gestionnaire de mots de passe populaire.",
            ShortDescriptionEn = "Popular password manager.",
            FullDescription = "LastPass stocke vos mots de passe dans le cloud avec chiffrement local. Offre le remplissage automatique, le générateur de mots de passe et le partage sécurisé.",
            FullDescriptionEn = "LastPass stores your passwords in the cloud with local encryption. Offers autofill, password generator and secure sharing.",
            DisableImpact = "Pas de remplissage automatique hors navigateur.",
            DisableImpactEn = "No autofill outside browser.",
            PerformanceImpact = "Faible (~40-60 Mo RAM).",
            PerformanceImpactEn = "Low (~40-60 MB RAM).",
            Recommendation = "L'extension navigateur fonctionne sans l'application desktop.",
            RecommendationEn = "Browser extension works without the desktop app.",
            Tags = "lastpass,password,mot de passe,cloud",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "Dashlane",
            Aliases = "Dashlane Password Manager",
            Publisher = "Dashlane, Inc.",
            ExecutableNames = "Dashlane.exe,DashlanePlugin.exe",
            Category = KnowledgeCategory.Security,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Gestionnaire de mots de passe avec VPN intégré.",
            ShortDescriptionEn = "Password manager with built-in VPN.",
            FullDescription = "Dashlane offre la gestion des mots de passe, la surveillance du dark web, et un VPN intégré dans les plans premium.",
            FullDescriptionEn = "Dashlane offers password management, dark web monitoring, and a built-in VPN in premium plans.",
            DisableImpact = "Pas de remplissage automatique hors navigateur.",
            DisableImpactEn = "No autofill outside browser.",
            PerformanceImpact = "Faible (~50-70 Mo RAM).",
            PerformanceImpactEn = "Low (~50-70 MB RAM).",
            Recommendation = "Peut être désactivé si vous utilisez l'extension navigateur.",
            RecommendationEn = "Can be disabled if you use the browser extension.",
            Tags = "dashlane,password,mot de passe,vpn",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "KeePass",
            Aliases = "KeePass Password Safe,KeePassXC",
            Publisher = "Dominik Reichl / KeePassXC Team",
            ExecutableNames = "KeePass.exe,KeePassXC.exe",
            Category = KnowledgeCategory.Security,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Gestionnaire de mots de passe local open source.",
            ShortDescriptionEn = "Local open source password manager.",
            FullDescription = "KeePass stocke vos mots de passe localement dans un fichier chiffré. Pas de cloud, contrôle total. KeePassXC est une version cross-platform moderne.",
            FullDescriptionEn = "KeePass stores your passwords locally in an encrypted file. No cloud, full control. KeePassXC is a modern cross-platform version.",
            DisableImpact = "La base de données ne sera pas ouverte automatiquement.",
            DisableImpactEn = "Database will not open automatically.",
            PerformanceImpact = "Très faible (~20-40 Mo RAM).",
            PerformanceImpactEn = "Very low (~20-40 MB RAM).",
            Recommendation = "Peut être désactivé si vous ouvrez KeePass manuellement.",
            RecommendationEn = "Can be disabled if you open KeePass manually.",
            Tags = "keepass,keepassxc,password,local,open source",
            LastUpdated = DateTime.Now
        });
    }

    private void SeedBackupSoftware()
    {
        Save(new KnowledgeEntry
        {
            Name = "Acronis True Image",
            Aliases = "Acronis Cyber Protect,Acronis Backup",
            Publisher = "Acronis International GmbH",
            ExecutableNames = "TrueImage.exe,TrueImageMonitor.exe,acronis_drive.exe",
            Category = KnowledgeCategory.Utility,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Logiciel de sauvegarde et clonage de disque.",
            ShortDescriptionEn = "Backup and recovery software.",
            FullDescription = "Acronis True Image offre la sauvegarde complète du système, le clonage de disque, la sauvegarde cloud et la protection anti-ransomware. Permet de restaurer un système complet.",
            FullDescriptionEn = "Acronis True Image offers full system backup, disk imaging and cloud synchronization.",
            DisableImpact = "Les sauvegardes planifiées ne s'exécuteront pas automatiquement.",
            DisableImpactEn = "Scheduled backups won't run.",
            PerformanceImpact = "Modéré (~80-150 Mo RAM avec les services).",
            PerformanceImpactEn = "Low when idle, high during backups.",
            Recommendation = "Gardez activé si vous avez des sauvegardes planifiées.",
            RecommendationEn = "Keep enabled to keep your backups up to date.",
            Tags = "acronis,backup,sauvegarde,clonage,image",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "EaseUS Todo Backup",
            Aliases = "EaseUS Backup,Todo Backup",
            Publisher = "CHENGDU YIWO Tech Development Co., Ltd.",
            ExecutableNames = "TbService.exe,TodoBackup.exe",
            Category = KnowledgeCategory.Utility,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Logiciel de sauvegarde et récupération.",
            ShortDescriptionEn = "Backup and recovery software.",
            FullDescription = "EaseUS Todo Backup permet de sauvegarder fichiers, partitions ou disques entiers. Offre le clonage de disque et la création de médias de récupération.",
            FullDescriptionEn = "EaseUS Todo Backup allows backing up files, partitions or entire disks. Offers disk cloning and recovery media creation.",
            DisableImpact = "Les sauvegardes planifiées ne s'exécuteront pas.",
            DisableImpactEn = "Scheduled backups will not run.",
            PerformanceImpact = "Faible (~30-60 Mo RAM).",
            PerformanceImpactEn = "Low (~30-60 MB RAM).",
            Recommendation = "Gardez activé pour les sauvegardes automatiques.",
            RecommendationEn = "Keep enabled for automatic backups.",
            Tags = "easeus,backup,sauvegarde,recuperation",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "Macrium Reflect",
            Aliases = "Macrium Reflect Free",
            Publisher = "Paramount Software UK Ltd",
            ExecutableNames = "Reflect.exe,ReflectService.exe",
            Category = KnowledgeCategory.Utility,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Logiciel de clonage et sauvegarde d'images disque.",
            ShortDescriptionEn = "Disk cloning and imaging backup software.",
            FullDescription = "Macrium Reflect crée des images de disque pour sauvegarde et restauration. Connu pour sa fiabilité et sa version gratuite fonctionnelle. Excellent pour cloner vers SSD.",
            FullDescriptionEn = "Macrium Reflect creates disk images for backup and restoration. Known for reliability and functional free version. Excellent for cloning to SSD.",
            DisableImpact = "Les sauvegardes planifiées ne fonctionneront pas.",
            DisableImpactEn = "Scheduled backups will not work.",
            PerformanceImpact = "Faible (~30-50 Mo RAM).",
            PerformanceImpactEn = "Low (~30-50 MB RAM).",
            Recommendation = "Gardez activé si vous utilisez les sauvegardes planifiées.",
            RecommendationEn = "Keep enabled if you use scheduled backups.",
            Tags = "macrium,reflect,backup,clonage,image disque",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "Backblaze",
            Aliases = "Backblaze Backup",
            Publisher = "Backblaze, Inc.",
            ExecutableNames = "bzbui.exe,bzserv.exe",
            Category = KnowledgeCategory.CloudStorage,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Sauvegarde cloud illimitée automatique.",
            ShortDescriptionEn = "Automatic unlimited cloud backup.",
            FullDescription = "Backblaze sauvegarde automatiquement tous vos fichiers vers le cloud pour un prix fixe. Stockage illimité, restauration par envoi de disque dur possible.",
            FullDescriptionEn = "Backblaze automatically backs up all your files to the cloud for a flat fee. Unlimited storage, hard drive shipment restore available.",
            DisableImpact = "Vos fichiers ne seront plus sauvegardés automatiquement vers le cloud.",
            DisableImpactEn = "Your files will no longer be automatically backed up to the cloud.",
            PerformanceImpact = "Faible à modéré (~30-60 Mo RAM). Utilise la bande passante en arrière-plan.",
            PerformanceImpactEn = "Low to moderate (~30-60 MB RAM). Uses bandwidth in the background.",
            Recommendation = "Gardez activé pour une sauvegarde continue. C'est son but.",
            RecommendationEn = "Keep enabled for continuous backup. That's its purpose.",
            Tags = "backblaze,cloud,backup,sauvegarde,illimite",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "Carbonite",
            Aliases = "Carbonite Backup",
            Publisher = "Carbonite, Inc.",
            ExecutableNames = "CarboniteUI.exe,CarboniteService.exe",
            Category = KnowledgeCategory.CloudStorage,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Service de sauvegarde cloud automatique.",
            ShortDescriptionEn = "Automatic cloud backup service.",
            FullDescription = "Carbonite sauvegarde automatiquement vos fichiers vers le cloud. Offre la restauration de fichiers, la protection contre les ransomwares et le support technique.",
            FullDescriptionEn = "Carbonite automatically backs up your files to the cloud. Offers file restoration, ransomware protection and technical support.",
            DisableImpact = "Les sauvegardes cloud s'arrêteront.",
            DisableImpactEn = "Cloud backups will stop.",
            PerformanceImpact = "Faible (~30-50 Mo RAM).",
            PerformanceImpactEn = "Low (~30-50 MB RAM).",
            Recommendation = "Gardez activé pour la sauvegarde continue.",
            RecommendationEn = "Keep enabled for continuous backup.",
            Tags = "carbonite,cloud,backup,sauvegarde",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "Veeam Agent",
            Aliases = "Veeam Agent for Windows,Veeam Backup",
            Publisher = "Veeam Software",
            ExecutableNames = "Veeam.EndPoint.Tray.exe,Veeam.EndPoint.Service.exe",
            Category = KnowledgeCategory.Utility,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Agent de sauvegarde gratuit pour Windows.",
            ShortDescriptionEn = "Veeam backup agent.",
            FullDescription = "Veeam Agent offre une sauvegarde gratuite de niveau entreprise pour les particuliers. Sauvegarde vers disque local, NAS ou cloud avec restauration bare-metal.",
            FullDescriptionEn = "Veeam Agent for Windows offers image-level backup for Windows workstations and servers.",
            DisableImpact = "Les sauvegardes planifiées ne s'exécuteront pas.",
            DisableImpactEn = "Scheduled backups won't run.",
            PerformanceImpact = "Faible (~30-50 Mo RAM).",
            PerformanceImpactEn = "Low when idle, high during backups.",
            Recommendation = "Gardez activé pour les sauvegardes automatiques.",
            RecommendationEn = "Keep enabled to keep your backups up to date.",
            Tags = "veeam,backup,sauvegarde,entreprise,gratuit",
            LastUpdated = DateTime.Now
        });
    }

    private void SeedRemoteDesktop()
    {
        Save(new KnowledgeEntry
        {
            Name = "TeamViewer",
            Aliases = "TeamViewer Host",
            Publisher = "TeamViewer Germany GmbH",
            ExecutableNames = "TeamViewer.exe,TeamViewer_Service.exe",
            Category = KnowledgeCategory.Utility,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Logiciel de contrôle à distance et support.",
            ShortDescriptionEn = "Remote control software.",
            FullDescription = "TeamViewer permet de contrôler des PC à distance, transférer des fichiers, faire des réunions en ligne et fournir du support technique. Gratuit pour usage personnel.",
            FullDescriptionEn = "TeamViewer allows remote control of computers, file transfer and online meetings. Widely used for technical support.",
            DisableImpact = "Les connexions entrantes ne seront pas possibles. Le PC ne sera pas accessible à distance.",
            DisableImpactEn = "Remote access to this computer won't be available.",
            PerformanceImpact = "Faible au repos (~30-50 Mo RAM).",
            PerformanceImpactEn = "Low when idle.",
            Recommendation = "Désactivez si vous n'avez pas besoin d'accès distant entrant. Peut être lancé manuellement.",
            RecommendationEn = "Disable if you don't need permanent remote access.",
            Tags = "teamviewer,remote,distance,support,controle",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "AnyDesk",
            Aliases = "AnyDesk Remote Desktop",
            Publisher = "AnyDesk Software GmbH",
            ExecutableNames = "AnyDesk.exe",
            Category = KnowledgeCategory.Utility,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Alternative légère à TeamViewer.",
            ShortDescriptionEn = "Lightweight remote desktop software.",
            FullDescription = "AnyDesk est un logiciel de bureau à distance léger et rapide. Connu pour sa faible latence et sa fluidité, même sur des connexions lentes.",
            FullDescriptionEn = "AnyDesk is a lightweight alternative to TeamViewer for remote control with low latency and good image quality.",
            DisableImpact = "Pas d'accès distant entrant sans lancer AnyDesk.",
            DisableImpactEn = "Remote access to this computer won't be available.",
            PerformanceImpact = "Très faible (~15-30 Mo RAM).",
            PerformanceImpactEn = "Very low when idle.",
            Recommendation = "Peut être désactivé si vous n'avez pas besoin d'accès permanent.",
            RecommendationEn = "Disable if you don't need permanent remote access.",
            Tags = "anydesk,remote,distance,bureau",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "Parsec",
            Aliases = "Parsec Gaming",
            Publisher = "Parsec Cloud, Inc.",
            ExecutableNames = "parsecd.exe,pservice.exe",
            Category = KnowledgeCategory.Gaming,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Streaming de jeux et bureau à distance pour gamers.",
            ShortDescriptionEn = "Game streaming and remote desktop.",
            FullDescription = "Parsec est optimisé pour le streaming de jeux avec une latence ultra-faible. Permet de jouer à vos jeux PC depuis n'importe où ou d'héberger des sessions multijoueur locales à distance.",
            FullDescriptionEn = "Parsec allows remote game streaming with very low latency. Ideal for playing on a remote PC or cloud gaming.",
            DisableImpact = "Le PC ne sera pas accessible pour le streaming de jeux.",
            DisableImpactEn = "Parsec streaming won't be available.",
            PerformanceImpact = "Faible au repos (~20-40 Mo RAM). Élevé pendant le streaming.",
            PerformanceImpactEn = "Low when idle, high during streaming.",
            Recommendation = "Gardez activé si vous utilisez Parsec pour le cloud gaming.",
            RecommendationEn = "Disable if you don't use it regularly.",
            Tags = "parsec,streaming,gaming,remote,jeux",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "Chrome Remote Desktop",
            Aliases = "Chrome Remote Desktop Host",
            Publisher = "Google LLC",
            ExecutableNames = "remoting_host.exe",
            Category = KnowledgeCategory.Utility,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Bureau à distance via Chrome.",
            ShortDescriptionEn = "Remote desktop via Chrome.",
            FullDescription = "Chrome Remote Desktop permet d'accéder à votre PC depuis n'importe quel navigateur Chrome ou l'application mobile. Gratuit et simple à configurer.",
            FullDescriptionEn = "Chrome Remote Desktop lets you access your PC from any Chrome browser or mobile app. Free and simple to set up.",
            DisableImpact = "Le PC ne sera plus accessible via Chrome Remote Desktop.",
            DisableImpactEn = "PC will no longer be accessible via Chrome Remote Desktop.",
            PerformanceImpact = "Très faible (~10-20 Mo RAM).",
            PerformanceImpactEn = "Very low (~10-20 MB RAM).",
            Recommendation = "Peut être désactivé si vous n'utilisez pas l'accès distant Chrome.",
            RecommendationEn = "Can be disabled if you don't use Chrome remote access.",
            Tags = "chrome,remote,google,bureau,distance",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "RustDesk",
            Aliases = "RustDesk Remote Desktop",
            Publisher = "RustDesk",
            ExecutableNames = "rustdesk.exe",
            Category = KnowledgeCategory.Utility,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Alternative open source à TeamViewer.",
            ShortDescriptionEn = "Open source TeamViewer alternative.",
            FullDescription = "RustDesk est une solution de bureau à distance open source. Peut être auto-hébergé pour un contrôle total. Alternative gratuite aux solutions propriétaires.",
            FullDescriptionEn = "RustDesk is an open source remote desktop solution. Can be self-hosted for full control. Free alternative to proprietary solutions.",
            DisableImpact = "Le PC ne sera pas accessible à distance.",
            DisableImpactEn = "PC will not be remotely accessible.",
            PerformanceImpact = "Très faible (~15-25 Mo RAM).",
            PerformanceImpactEn = "Very low (~15-25 MB RAM).",
            Recommendation = "Peut être désactivé si vous n'avez pas besoin d'accès distant.",
            RecommendationEn = "Can be disabled if you don't need remote access.",
            Tags = "rustdesk,remote,open source,bureau,distance",
            LastUpdated = DateTime.Now
        });
    }

    private void SeedVirtualReality()
    {
        Save(new KnowledgeEntry
        {
            Name = "Oculus",
            Aliases = "Meta Quest,Oculus App,OculusClient",
            Publisher = "Meta Platforms, Inc.",
            ExecutableNames = "OculusClient.exe,OVRServer_x64.exe,OVRServiceLauncher.exe",
            Category = KnowledgeCategory.Gaming,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Application pour casques VR Meta Quest et Rift.",
            ShortDescriptionEn = "Application for Meta Quest and Rift VR headsets.",
            FullDescription = "L'application Oculus (Meta Quest) gère vos casques VR Meta, la bibliothèque de jeux, le Link/Air Link pour jouer aux jeux PC VR et les paramètres du Guardian.",
            FullDescriptionEn = "Oculus app (Meta Quest) manages your Meta VR headsets, game library, Link/Air Link for PC VR gaming and Guardian settings.",
            DisableImpact = "Les services VR ne démarreront pas automatiquement. Vous devrez lancer l'app avant d'utiliser le casque.",
            DisableImpactEn = "VR services will not start automatically. You will need to launch the app before using the headset.",
            PerformanceImpact = "Modéré à élevé (~150-300 Mo RAM avec les services).",
            PerformanceImpactEn = "Moderate to high (~150-300 MB RAM with services).",
            Recommendation = "Peut être désactivé si vous n'utilisez pas la VR quotidiennement.",
            RecommendationEn = "Can be disabled if you don't use VR daily.",
            Tags = "oculus,meta,quest,vr,realite virtuelle,rift",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "SteamVR",
            Aliases = "Steam VR,OpenVR",
            Publisher = "Valve Corporation",
            ExecutableNames = "vrserver.exe,vrmonitor.exe,vrstartup.exe",
            Category = KnowledgeCategory.Gaming,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Plateforme VR de Steam pour tous les casques.",
            ShortDescriptionEn = "Steam VR platform for all headsets.",
            FullDescription = "SteamVR est la plateforme VR universelle de Valve. Compatible avec la plupart des casques (Valve Index, HTC Vive, Oculus, WMR). Gère le tracking, les contrôleurs et la bibliothèque VR.",
            FullDescriptionEn = "SteamVR is Valve's universal VR platform. Compatible with most headsets (Valve Index, HTC Vive, Oculus, WMR). Manages tracking, controllers and VR library.",
            DisableImpact = "SteamVR ne démarrera pas automatiquement. Lancé par Steam quand nécessaire.",
            DisableImpactEn = "SteamVR will not start automatically. Launched by Steam when needed.",
            PerformanceImpact = "Élevé en utilisation (~200-400 Mo RAM).",
            PerformanceImpactEn = "High when in use (~200-400 MB RAM).",
            Recommendation = "Peut être désactivé du démarrage. Steam le lance automatiquement.",
            RecommendationEn = "Can be disabled from startup. Steam launches it automatically.",
            Tags = "steamvr,valve,vr,realite virtuelle,index,vive",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "HTC Vive Console",
            Aliases = "Vive Console,VIVE,Viveport",
            Publisher = "HTC Corporation",
            ExecutableNames = "ViveConsole.exe,Viveport.exe,ViveVRRuntime.exe",
            Category = KnowledgeCategory.Gaming,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Logiciel pour casques VR HTC Vive.",
            ShortDescriptionEn = "Software for HTC Vive VR headsets.",
            FullDescription = "Vive Console gère les casques HTC Vive : configuration du tracking, des contrôleurs, mise à jour du firmware. Viveport est la boutique de jeux VR HTC.",
            FullDescriptionEn = "Vive Console manages HTC Vive headsets: tracking setup, controller configuration, firmware updates. Viveport is the HTC VR game store.",
            DisableImpact = "Le casque Vive devra être configuré manuellement avant utilisation.",
            DisableImpactEn = "Vive headset will need to be configured manually before use.",
            PerformanceImpact = "Modéré (~100-200 Mo RAM).",
            PerformanceImpactEn = "Moderate (~100-200 MB RAM).",
            Recommendation = "Peut être désactivé si vous n'utilisez pas fréquemment la VR.",
            RecommendationEn = "Can be disabled if you don't use VR frequently.",
            Tags = "htc,vive,vr,realite virtuelle,viveport",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "Windows Mixed Reality",
            Aliases = "WMR,Mixed Reality Portal",
            Publisher = "Microsoft Corporation",
            ExecutableNames = "MixedRealityPortal.exe,PerceptionSimulationService.exe",
            Category = KnowledgeCategory.Gaming,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Plateforme VR de Microsoft pour casques WMR.",
            ShortDescriptionEn = "Microsoft VR platform for WMR headsets.",
            FullDescription = "Windows Mixed Reality gère les casques VR WMR (HP Reverb, Samsung Odyssey, etc.). Intégré à Windows 10/11 avec tracking inside-out.",
            FullDescriptionEn = "Windows Mixed Reality manages WMR VR headsets (HP Reverb, Samsung Odyssey, etc.). Integrated into Windows 10/11 with inside-out tracking.",
            DisableImpact = "Le portail MR ne démarrera pas automatiquement.",
            DisableImpactEn = "MR portal will not start automatically.",
            PerformanceImpact = "Modéré (~100-150 Mo RAM).",
            PerformanceImpactEn = "Moderate (~100-150 MB RAM).",
            Recommendation = "Peut être désactivé si vous n'utilisez pas la VR fréquemment.",
            RecommendationEn = "Can be disabled if you don't use VR frequently.",
            Tags = "wmr,mixed reality,microsoft,vr,reverb",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "Virtual Desktop",
            Aliases = "Virtual Desktop Streamer",
            Publisher = "Virtual Desktop, Inc.",
            ExecutableNames = "VirtualDesktop.Streamer.exe",
            Category = KnowledgeCategory.Gaming,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Streaming PC vers casque VR standalone.",
            ShortDescriptionEn = "PC streaming to standalone VR headset.",
            FullDescription = "Virtual Desktop permet de streamer votre bureau Windows et vos jeux VR vers un casque standalone (Quest) en Wi-Fi. Alternative à Oculus Link/Air Link.",
            FullDescriptionEn = "Virtual Desktop lets you stream your Windows desktop and VR games to a standalone headset (Quest) over Wi-Fi. Alternative to Oculus Link/Air Link.",
            DisableImpact = "Le streaming vers le casque Quest ne sera pas disponible immédiatement.",
            DisableImpactEn = "Streaming to Quest headset will not be immediately available.",
            PerformanceImpact = "Faible au repos (~30-50 Mo RAM).",
            PerformanceImpactEn = "Low when idle (~30-50 MB RAM).",
            Recommendation = "Peut être désactivé si vous n'utilisez pas le streaming VR.",
            RecommendationEn = "Can be disabled if you don't use VR streaming.",
            Tags = "virtual desktop,vr,streaming,quest,wireless",
            LastUpdated = DateTime.Now
        });
    }

    private void SeedGameLaunchers()
    {
        Save(new KnowledgeEntry
        {
            Name = "Rockstar Games Launcher",
            Aliases = "Rockstar Launcher,Social Club",
            Publisher = "Rockstar Games",
            ExecutableNames = "Launcher.exe,SocialClubHelper.exe",
            Category = KnowledgeCategory.Gaming,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Lanceur pour les jeux Rockstar (GTA, Red Dead).",
            ShortDescriptionEn = "Rockstar games launcher.",
            FullDescription = "Le Rockstar Games Launcher est requis pour jouer aux jeux Rockstar (GTA V, Red Dead Redemption 2, etc.). Gère les téléchargements, mises à jour et le Social Club.",
            FullDescriptionEn = "The Rockstar Games Launcher is required to play Rockstar games (GTA, Red Dead Redemption) on PC.",
            DisableImpact = "Les jeux Rockstar ne pourront pas se lancer sans le lanceur. Démarrez-le manuellement.",
            DisableImpactEn = "The launcher won't start automatically.",
            PerformanceImpact = "Modéré (~60-100 Mo RAM).",
            PerformanceImpactEn = "Low.",
            Recommendation = "Peut être désactivé si vous ne jouez pas souvent aux jeux Rockstar.",
            RecommendationEn = "Can be disabled from automatic startup.",
            Tags = "rockstar,gta,red dead,launcher,social club",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "Riot Client",
            Aliases = "Riot Games,Riot Vanguard",
            Publisher = "Riot Games, Inc.",
            ExecutableNames = "RiotClientServices.exe,vgc.exe,vgtray.exe",
            Category = KnowledgeCategory.Gaming,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Lanceur pour League of Legends et Valorant.",
            ShortDescriptionEn = "Launcher for League of Legends and Valorant.",
            FullDescription = "Le Riot Client gère vos jeux Riot (LoL, Valorant, Legends of Runeterra, etc.). Vanguard est l'anti-cheat de Valorant qui fonctionne au niveau kernel.",
            FullDescriptionEn = "Riot Client manages your Riot games (LoL, Valorant, Legends of Runeterra, etc.). Vanguard is Valorant's anti-cheat running at kernel level.",
            DisableImpact = "Les jeux Riot ne pourront pas se lancer. Vanguard doit tourner pour jouer à Valorant.",
            DisableImpactEn = "Riot games cannot launch. Vanguard must run to play Valorant.",
            PerformanceImpact = "Vanguard : ~50 Mo RAM, toujours actif. Riot Client : ~80-120 Mo RAM.",
            PerformanceImpactEn = "Vanguard: ~50 MB RAM, always active. Riot Client: ~80-120 MB RAM.",
            Recommendation = "Vanguard peut être désactivé si vous ne jouez pas à Valorant, mais il redémarrera le PC.",
            RecommendationEn = "Vanguard can be disabled if you don't play Valorant, but it will restart the PC.",
            Tags = "riot,lol,valorant,vanguard,anticheat",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "Amazon Games",
            Aliases = "Amazon Games App,Prime Gaming",
            Publisher = "Amazon.com, Inc.",
            ExecutableNames = "Amazon Games.exe,AmazonGamesService.exe",
            Category = KnowledgeCategory.Gaming,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Lanceur de jeux Amazon et Prime Gaming.",
            ShortDescriptionEn = "Amazon gaming client.",
            FullDescription = "Amazon Games gère les jeux Amazon (New World, Lost Ark) et les jeux gratuits Prime Gaming. Télécharge et met à jour les jeux.",
            FullDescriptionEn = "Amazon Games is the client for Amazon games, free Prime Gaming games and New World and Lost Ark.",
            DisableImpact = "Les jeux Amazon devront être lancés manuellement.",
            DisableImpactEn = "Amazon Games won't start automatically.",
            PerformanceImpact = "Faible (~40-70 Mo RAM).",
            PerformanceImpactEn = "Low to moderate.",
            Recommendation = "Peut être désactivé si vous ne récupérez pas souvent les jeux Prime.",
            RecommendationEn = "Can be disabled from automatic startup.",
            Tags = "amazon,games,prime gaming,new world",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "Playnite",
            Aliases = "Playnite Game Library",
            Publisher = "Josef Nemec",
            ExecutableNames = "Playnite.DesktopApp.exe,Playnite.FullscreenApp.exe",
            Category = KnowledgeCategory.Gaming,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Bibliothèque de jeux unifiée open source.",
            ShortDescriptionEn = "Unified open source game library.",
            FullDescription = "Playnite agrège tous vos jeux de Steam, Epic, GOG, Origin, etc. en une seule bibliothèque. Offre un mode Big Picture et de nombreuses personnalisations.",
            FullDescriptionEn = "Playnite aggregates all your games from Steam, Epic, GOG, Origin, etc. into one library. Offers Big Picture mode and extensive customization.",
            DisableImpact = "Playnite ne s'ouvrira pas au démarrage. Aucun impact sur les launchers individuels.",
            DisableImpactEn = "Playnite will not open at startup. No impact on individual launchers.",
            PerformanceImpact = "Faible (~40-80 Mo RAM).",
            PerformanceImpactEn = "Low (~40-80 MB RAM).",
            Recommendation = "Peut être désactivé si vous préférez lancer Playnite manuellement.",
            RecommendationEn = "Can be disabled if you prefer launching Playnite manually.",
            Tags = "playnite,bibliotheque,jeux,unifiee,open source",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "Bethesda Launcher",
            Aliases = "Bethesda.net Launcher",
            Publisher = "Bethesda Softworks",
            ExecutableNames = "BethesdaNetLauncher.exe",
            Category = KnowledgeCategory.Gaming,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Lanceur pour les jeux Bethesda (en migration vers Steam).",
            ShortDescriptionEn = "Launcher for Bethesda games (migrating to Steam).",
            FullDescription = "Le Bethesda Launcher gérait les jeux Bethesda (Fallout, Elder Scrolls, DOOM). Note : Bethesda a migré vers Steam en 2022, ce lanceur est obsolète.",
            FullDescriptionEn = "Bethesda Launcher managed Bethesda games (Fallout, Elder Scrolls, DOOM). Note: Bethesda migrated to Steam in 2022, this launcher is obsolete.",
            DisableImpact = "Aucun impact car les jeux ont été migrés vers Steam.",
            DisableImpactEn = "No impact as games have been migrated to Steam.",
            PerformanceImpact = "Faible (~30-50 Mo RAM).",
            PerformanceImpactEn = "Low (~30-50 MB RAM).",
            Recommendation = "Peut être désinstallé car obsolète. Les jeux sont maintenant sur Steam.",
            RecommendationEn = "Can be uninstalled as obsolete. Games are now on Steam.",
            Tags = "bethesda,fallout,elder scrolls,obsolete",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "itch.io",
            Aliases = "itch Desktop App",
            Publisher = "itch corp.",
            ExecutableNames = "itch.exe,butler.exe",
            Category = KnowledgeCategory.Gaming,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Plateforme de jeux indépendants.",
            ShortDescriptionEn = "Indie games platform.",
            FullDescription = "itch.io est une plateforme pour les jeux indépendants avec un modèle 'payez ce que vous voulez'. L'app desktop facilite le téléchargement et la mise à jour des jeux.",
            FullDescriptionEn = "itch.io is a platform for indie games with a 'pay what you want' model. The desktop app makes downloading and updating games easier.",
            DisableImpact = "L'app itch.io ne démarrera pas automatiquement.",
            DisableImpactEn = "itch.io app will not start automatically.",
            PerformanceImpact = "Faible (~40-60 Mo RAM).",
            PerformanceImpactEn = "Low (~40-60 MB RAM).",
            Recommendation = "Peut être désactivé si vous n'achetez pas souvent sur itch.io.",
            RecommendationEn = "Can be disabled if you don't often buy from itch.io.",
            Tags = "itch,indie,jeux,independant",
            LastUpdated = DateTime.Now
        });
    }

    private void SeedNotesApps()
    {
        Save(new KnowledgeEntry
        {
            Name = "Notion",
            Aliases = "Notion Desktop",
            Publisher = "Notion Labs, Inc.",
            ExecutableNames = "Notion.exe",
            Category = KnowledgeCategory.Productivity,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Espace de travail tout-en-un pour notes et projets.",
            ShortDescriptionEn = "All-in-one workspace.",
            FullDescription = "Notion combine notes, documents, bases de données, kanban et wikis. Très flexible pour la gestion de projets personnels ou en équipe.",
            FullDescriptionEn = "Notion combines notes, databases, wikis and project management in a single application. Popular for personal and team productivity.",
            DisableImpact = "Notion ne s'ouvrira pas automatiquement. Accès toujours possible via navigateur.",
            DisableImpactEn = "Notion won't be immediately accessible.",
            PerformanceImpact = "Modéré (~100-200 Mo RAM basé sur Electron).",
            PerformanceImpactEn = "Moderate (Electron application).",
            Recommendation = "Peut être désactivé si vous préférez la version web.",
            RecommendationEn = "Can be disabled from automatic startup.",
            Tags = "notion,notes,productivite,wiki,projet",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "Obsidian",
            Aliases = "Obsidian Notes",
            Publisher = "Dynalist Inc.",
            ExecutableNames = "Obsidian.exe",
            Category = KnowledgeCategory.Productivity,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Application de notes en Markdown avec liens bidirectionnels.",
            ShortDescriptionEn = "Markdown note-taking application.",
            FullDescription = "Obsidian stocke vos notes en fichiers Markdown locaux avec des liens bidirectionnels pour créer un 'second cerveau'. Hautement personnalisable avec des plugins.",
            FullDescriptionEn = "Obsidian is a personal knowledge base with Markdown support, bi-directional links and knowledge graph.",
            DisableImpact = "Obsidian ne démarrera pas automatiquement.",
            DisableImpactEn = "Obsidian won't be immediately accessible.",
            PerformanceImpact = "Modéré (~80-150 Mo RAM).",
            PerformanceImpactEn = "Moderate.",
            Recommendation = "Peut être désactivé si vous n'avez pas besoin d'y accéder au démarrage.",
            RecommendationEn = "Can be disabled from automatic startup.",
            Tags = "obsidian,notes,markdown,zettelkasten,local",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "Evernote",
            Aliases = "Evernote Client",
            Publisher = "Evernote Corporation",
            ExecutableNames = "Evernote.exe,EvernoteClipper.exe",
            Category = KnowledgeCategory.Productivity,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Application de prise de notes cloud populaire.",
            ShortDescriptionEn = "Note-taking app.",
            FullDescription = "Evernote synchronise vos notes entre appareils avec OCR sur les images, Web Clipper et recherche puissante. Un des pionniers de la prise de notes numériques.",
            FullDescriptionEn = "Evernote allows capturing and organizing notes, tasks, and schedules.",
            DisableImpact = "Pas de capture rapide au démarrage. Les notes restent accessibles via l'app ou le web.",
            DisableImpactEn = "Quick note features won't be available.",
            PerformanceImpact = "Modéré (~100-200 Mo RAM).",
            PerformanceImpactEn = "Moderate.",
            Recommendation = "Peut être désactivé si vous n'utilisez pas le raccourci de capture.",
            RecommendationEn = "Can be disabled if not used frequently.",
            Tags = "evernote,notes,cloud,ocr,clipper",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "Joplin",
            Aliases = "Joplin Notes",
            Publisher = "Laurent Cozic",
            ExecutableNames = "Joplin.exe",
            Category = KnowledgeCategory.Productivity,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Application de notes open source avec chiffrement.",
            ShortDescriptionEn = "Note-taking app.",
            FullDescription = "Joplin est une alternative open source à Evernote. Supporte Markdown, le chiffrement bout en bout et la synchronisation via Dropbox, OneDrive ou Nextcloud.",
            FullDescriptionEn = "Open-source note-taking and to-do application with synchronization capabilities.",
            DisableImpact = "Joplin ne démarrera pas automatiquement.",
            DisableImpactEn = "Sync and reminders won't run at startup.",
            PerformanceImpact = "Modéré (~80-150 Mo RAM).",
            PerformanceImpactEn = "Low.",
            Recommendation = "Peut être désactivé si vous n'avez pas besoin d'accès immédiat.",
            RecommendationEn = "Can be disabled from startup.",
            Tags = "joplin,notes,open source,markdown,chiffrement",
            LastUpdated = DateTime.Now
        });
    }

    private void SeedPDFSoftware()
    {
        Save(new KnowledgeEntry
        {
            Name = "Adobe Acrobat Reader",
            Aliases = "Acrobat Reader DC,AcroRd32,Adobe Reader",
            Publisher = "Adobe Inc.",
            ExecutableNames = "Acrobat.exe,AcroRd32.exe,AdobeARM.exe",
            Category = KnowledgeCategory.Productivity,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Lecteur PDF gratuit d'Adobe.",
            ShortDescriptionEn = "Free PDF viewer.",
            FullDescription = "Adobe Acrobat Reader est le lecteur PDF standard. Permet de lire, annoter, signer et remplir des formulaires PDF. AdobeARM gère les mises à jour.",
            FullDescriptionEn = "Adobe Acrobat Reader is the standard software for viewing, printing, and commenting on PDF documents.",
            DisableImpact = "Pas de mise à jour automatique. Les PDF s'ouvrent toujours normalement.",
            DisableImpactEn = "Update checks won't run automatically.",
            PerformanceImpact = "Faible (~30-50 Mo RAM pour le service de mise à jour).",
            PerformanceImpactEn = "Low.",
            Recommendation = "Le service de mise à jour peut être désactivé. Mettez à jour manuellement.",
            RecommendationEn = "Can be disabled from startup.",
            Tags = "adobe,acrobat,reader,pdf,lecture",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "Adobe Acrobat Pro",
            Aliases = "Acrobat Pro DC,Acrobat Pro",
            Publisher = "Adobe Inc.",
            ExecutableNames = "Acrobat.exe,AdobeCollabSync.exe",
            Category = KnowledgeCategory.Productivity,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Éditeur PDF professionnel d'Adobe.",
            ShortDescriptionEn = "Professional PDF editor.",
            FullDescription = "Acrobat Pro permet de créer, modifier, convertir et signer des PDF. Inclut l'OCR, la fusion de documents et les formulaires avancés.",
            FullDescriptionEn = "Adobe Acrobat Pro allows creating, editing, signing, and converting PDF documents.",
            DisableImpact = "Les services de synchronisation et mise à jour ne démarreront pas.",
            DisableImpactEn = "Background update and licensing services won't run.",
            PerformanceImpact = "Modéré (~50-100 Mo RAM avec les services).",
            PerformanceImpactEn = "Low.",
            Recommendation = "Les services de démarrage peuvent être désactivés sans impact sur l'édition PDF.",
            RecommendationEn = "Can be disabled from startup.",
            Tags = "adobe,acrobat,pro,pdf,edition,ocr",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "Foxit PDF Reader",
            Aliases = "Foxit Reader,Foxit PhantomPDF",
            Publisher = "Foxit Software Inc.",
            ExecutableNames = "FoxitPDFReader.exe,FoxitPhantomPDF.exe",
            Category = KnowledgeCategory.Productivity,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Lecteur PDF alternatif léger.",
            ShortDescriptionEn = "Fast PDF reader.",
            FullDescription = "Foxit PDF Reader est une alternative légère à Adobe Reader. Offre lecture, annotations, signatures et formulaires. PhantomPDF ajoute l'édition complète.",
            FullDescriptionEn = "Foxit PDF Reader is a popular alternative to Adobe Reader with a small footprint.",
            DisableImpact = "Aucun impact. Foxit ne devrait pas être dans le démarrage automatique.",
            DisableImpactEn = "Update service won't run.",
            PerformanceImpact = "Aucun au repos.",
            PerformanceImpactEn = "Low.",
            Recommendation = "Si dans le démarrage, peut être désactivé en toute sécurité.",
            RecommendationEn = "Can be disabled from startup.",
            Tags = "foxit,pdf,reader,leger,alternatif",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "PDF-XChange Editor",
            Aliases = "PDF-XChange,Tracker PDF",
            Publisher = "Tracker Software Products Ltd",
            ExecutableNames = "PDFXEdit.exe",
            Category = KnowledgeCategory.Productivity,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Éditeur PDF puissant avec version gratuite.",
            ShortDescriptionEn = "PDF editor.",
            FullDescription = "PDF-XChange Editor offre de nombreuses fonctionnalités gratuites : édition de texte, annotations, OCR, comparaison de documents. Alternative puissante à Acrobat.",
            FullDescriptionEn = "Feature-rich PDF editor and viewer.",
            DisableImpact = "Aucun impact. L'application devrait être lancée manuellement.",
            DisableImpactEn = "Quick launch/updates won't run.",
            PerformanceImpact = "Aucun au repos.",
            PerformanceImpactEn = "Low.",
            Recommendation = "Ne devrait pas être dans le démarrage automatique.",
            RecommendationEn = "Disable from startup.",
            Tags = "pdfxchange,pdf,editeur,ocr,gratuit",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "Nitro PDF",
            Aliases = "Nitro Pro,Nitro PDF Pro",
            Publisher = "Nitro Software, Inc.",
            ExecutableNames = "NitroPDF.exe,NitroService.exe",
            Category = KnowledgeCategory.Productivity,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Suite PDF professionnelle alternative.",
            ShortDescriptionEn = "PDF productivity tool.",
            FullDescription = "Nitro Pro est une alternative à Acrobat Pro pour créer, éditer et convertir des PDF. Interface familière style Office et intégration cloud.",
            FullDescriptionEn = "Software to create, edit, sign, and share PDF files.",
            DisableImpact = "Les services Nitro ne démarreront pas automatiquement.",
            DisableImpactEn = "Agent won't load.",
            PerformanceImpact = "Faible (~20-40 Mo RAM).",
            PerformanceImpactEn = "Low.",
            Recommendation = "Les services de démarrage peuvent être désactivés.",
            RecommendationEn = "Disable from startup.",
            Tags = "nitro,pdf,pro,edition,conversion",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "SumatraPDF",
            Aliases = "Sumatra PDF Reader",
            Publisher = "Krzysztof Kowalczyk",
            ExecutableNames = "SumatraPDF.exe",
            Category = KnowledgeCategory.Utility,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Lecteur PDF ultra-léger et rapide.",
            ShortDescriptionEn = "Lightweight PDF reader.",
            FullDescription = "SumatraPDF est un lecteur PDF minimaliste et rapide. Supporte aussi ePub, Mobi, XPS et autres formats. Portable, pas d'installation requise.",
            FullDescriptionEn = "Small, fast, free, open-source PDF, eBook (ePub, Mobi), XPS, DjVu, CHM, Comic Book (CBZ and CBR) reader.",
            DisableImpact = "Aucun impact. SumatraPDF ne devrait pas être dans le démarrage.",
            DisableImpactEn = "No impact.",
            PerformanceImpact = "Aucun au repos.",
            PerformanceImpactEn = "None.",
            Recommendation = "Ne devrait pas être dans le démarrage automatique.",
            RecommendationEn = "Should not be in startup.",
            Tags = "sumatra,pdf,leger,rapide,portable",
            LastUpdated = DateTime.Now
        });
    }

    private void SeedScreenshotTools()
    {
        Save(new KnowledgeEntry
        {
            Name = "ShareX",
            Aliases = "ShareX Screenshot",
            Publisher = "ShareX Team",
            ExecutableNames = "ShareX.exe",
            Category = KnowledgeCategory.Utility,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Outil de capture d'écran et partage gratuit.",
            ShortDescriptionEn = "Free screenshot and sharing tool.",
            FullDescription = "ShareX est un outil de capture d'écran open source très complet : capture de région, fenêtre, scroll, GIF, vidéo. Upload automatique vers de nombreux services. Éditeur d'image intégré.",
            FullDescriptionEn = "ShareX is a comprehensive open-source screenshot tool: region, window, scroll, GIF, video capture. Automatic upload to many services. Built-in image editor.",
            DisableImpact = "Les raccourcis de capture ne fonctionneront pas au démarrage.",
            DisableImpactEn = "Capture shortcuts won't work at startup.",
            PerformanceImpact = "Faible (~30-50 Mo RAM).",
            PerformanceImpactEn = "Low (~30-50 MB RAM).",
            Recommendation = "Gardez activé si vous utilisez régulièrement les captures d'écran.",
            RecommendationEn = "Keep enabled if you regularly use screenshots.",
            Tags = "sharex,screenshot,capture,gif,upload",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "Greenshot",
            Aliases = "Greenshot Screenshot",
            Publisher = "Greenshot",
            ExecutableNames = "Greenshot.exe",
            Category = KnowledgeCategory.Utility,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Outil de capture d'écran léger et gratuit.",
            ShortDescriptionEn = "Lightweight and free screenshot tool.",
            FullDescription = "Greenshot est un outil de capture d'écran simple mais efficace. Capture de région, fenêtre, plein écran avec éditeur intégré et export direct vers applications.",
            FullDescriptionEn = "Greenshot is a simple but effective screenshot tool. Region, window, fullscreen capture with built-in editor and direct export to applications.",
            DisableImpact = "Les raccourcis de capture ne seront pas disponibles.",
            DisableImpactEn = "Capture shortcuts won't be available.",
            PerformanceImpact = "Très faible (~15-25 Mo RAM).",
            PerformanceImpactEn = "Very low (~15-25 MB RAM).",
            Recommendation = "Gardez activé si vous utilisez les raccourcis de capture.",
            RecommendationEn = "Keep enabled if you use capture shortcuts.",
            Tags = "greenshot,screenshot,capture,gratuit,leger",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "Lightshot",
            Aliases = "Lightshot Screenshot",
            Publisher = "Skillbrains",
            ExecutableNames = "Lightshot.exe",
            Category = KnowledgeCategory.Utility,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Outil de capture d'écran rapide avec partage.",
            ShortDescriptionEn = "Screenshot tool.",
            FullDescription = "Lightshot permet de capturer une zone de l'écran rapidement, d'annoter et de partager via un lien. Simple et efficace pour le partage rapide.",
            FullDescriptionEn = "Fast and customizable screenshot tool.",
            DisableImpact = "Le raccourci Print Screen modifié ne fonctionnera plus.",
            DisableImpactEn = "PrintScreen key shortcut won't work.",
            PerformanceImpact = "Très faible (~10-20 Mo RAM).",
            PerformanceImpactEn = "Very low.",
            Recommendation = "Gardez activé si vous utilisez Print Screen pour Lightshot.",
            RecommendationEn = "Keep enabled if you use it.",
            Tags = "lightshot,screenshot,capture,partage",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "Snagit",
            Aliases = "TechSmith Snagit",
            Publisher = "TechSmith Corporation",
            ExecutableNames = "Snagit32.exe,Snagit64.exe,SnagitEditor.exe",
            Category = KnowledgeCategory.Productivity,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Outil de capture d'écran professionnel.",
            ShortDescriptionEn = "Screen capture software.",
            FullDescription = "Snagit est un outil de capture premium avec éditeur puissant, capture vidéo, scrolling capture, templates et intégrations professionnelles.",
            FullDescriptionEn = "Screen capture and recording software with built-in editing.",
            DisableImpact = "Les raccourcis de capture Snagit ne seront pas disponibles.",
            DisableImpactEn = "Capture shortcuts won't work.",
            PerformanceImpact = "Modéré (~50-100 Mo RAM).",
            PerformanceImpactEn = "Low.",
            Recommendation = "Gardez activé si vous utilisez Snagit régulièrement.",
            RecommendationEn = "Keep enabled if used frequently.",
            Tags = "snagit,screenshot,capture,professionnel,video",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "Flameshot",
            Aliases = "Flameshot Screenshot",
            Publisher = "Flameshot",
            ExecutableNames = "flameshot.exe",
            Category = KnowledgeCategory.Utility,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Outil de capture d'écran open source.",
            ShortDescriptionEn = "Powerful screenshot tool.",
            FullDescription = "Flameshot est un outil de capture open source avec annotations en temps réel. Interface simple avec upload vers Imgur et autres services.",
            FullDescriptionEn = "Open-source screenshot software with drawing and annotation tools.",
            DisableImpact = "Les raccourcis de capture ne fonctionneront pas.",
            DisableImpactEn = "Screenshot shortcuts won't work.",
            PerformanceImpact = "Très faible (~15-25 Mo RAM).",
            PerformanceImpactEn = "Very low.",
            Recommendation = "Gardez activé si vous utilisez les raccourcis de capture.",
            RecommendationEn = "Keep enabled if it's your main screenshot tool.",
            Tags = "flameshot,screenshot,capture,open source",
            LastUpdated = DateTime.Now
        });
    }

    private void SeedEmailClients()
    {
        Save(new KnowledgeEntry
        {
            Name = "Mozilla Thunderbird",
            Aliases = "Thunderbird,Thunderbird Mail",
            Publisher = "Mozilla Foundation",
            ExecutableNames = "thunderbird.exe",
            Category = KnowledgeCategory.Communication,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Client email gratuit et open source de Mozilla.",
            ShortDescriptionEn = "Free and open source email client by Mozilla.",
            FullDescription = "Thunderbird est un client email complet supportant IMAP, POP3, Exchange. Inclut calendrier, contacts, chat et de nombreuses extensions. Alternative gratuite à Outlook.",
            FullDescriptionEn = "Thunderbird is a complete email client supporting IMAP, POP3, Exchange. Includes calendar, contacts, chat and many extensions. Free alternative to Outlook.",
            DisableImpact = "Pas de notifications email au démarrage. Lancez Thunderbird manuellement.",
            DisableImpactEn = "No email notifications at startup. Launch Thunderbird manually.",
            PerformanceImpact = "Modéré (~100-200 Mo RAM).",
            PerformanceImpactEn = "Moderate (~100-200 MB RAM).",
            Recommendation = "Peut être désactivé si vous préférez lancer Thunderbird manuellement.",
            RecommendationEn = "Can be disabled if you prefer to launch Thunderbird manually.",
            Tags = "thunderbird,email,mozilla,imap,open source",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "Mailbird",
            Aliases = "Mailbird Email Client",
            Publisher = "Mailbird",
            ExecutableNames = "Mailbird.exe",
            Category = KnowledgeCategory.Communication,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Client email moderne pour Windows.",
            ShortDescriptionEn = "Modern email client for Windows.",
            FullDescription = "Mailbird est un client email élégant avec boîte de réception unifiée, intégrations apps (WhatsApp, Slack, Todoist) et recherche rapide.",
            FullDescriptionEn = "Mailbird is an elegant email client with unified inbox, app integrations (WhatsApp, Slack, Todoist) and fast search.",
            DisableImpact = "Pas de notifications email au démarrage.",
            DisableImpactEn = "No email notifications at startup.",
            PerformanceImpact = "Modéré (~80-150 Mo RAM).",
            PerformanceImpactEn = "Moderate (~80-150 MB RAM).",
            Recommendation = "Peut être désactivé si vous ne voulez pas de notifications email.",
            RecommendationEn = "Can be disabled if you don't want email notifications.",
            Tags = "mailbird,email,client,moderne,unifie",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "eM Client",
            Aliases = "eM Client Email",
            Publisher = "eM Client s.r.o.",
            ExecutableNames = "MailClient.exe",
            Category = KnowledgeCategory.Communication,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Client email avec calendrier et contacts intégrés.",
            ShortDescriptionEn = "Email client with integrated calendar and contacts.",
            FullDescription = "eM Client offre email, calendrier, contacts et tâches dans une interface unifiée. Supporte Gmail, Outlook.com, Exchange et autres services.",
            FullDescriptionEn = "eM Client offers email, calendar, contacts and tasks in a unified interface. Supports Gmail, Outlook.com, Exchange and other services.",
            DisableImpact = "Pas de notifications au démarrage.",
            DisableImpactEn = "No notifications at startup.",
            PerformanceImpact = "Modéré (~100-180 Mo RAM).",
            PerformanceImpactEn = "Moderate (~100-180 MB RAM).",
            Recommendation = "Peut être désactivé si vous préférez le lancer manuellement.",
            RecommendationEn = "Can be disabled if you prefer to launch it manually.",
            Tags = "emclient,email,calendrier,contacts,client",
            LastUpdated = DateTime.Now
        });
    }

    private void SeedRGBAndFanControl()
    {
        Save(new KnowledgeEntry
        {
            Name = "OpenRGB",
            Aliases = "Open RGB",
            Publisher = "OpenRGB",
            ExecutableNames = "OpenRGB.exe",
            Category = KnowledgeCategory.Utility,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Contrôleur RGB universel open source.",
            ShortDescriptionEn = "Open source universal RGB controller.",
            FullDescription = "OpenRGB unifie le contrôle de l'éclairage RGB de tous vos périphériques (cartes mères, RAM, GPU, périphériques) sans bloatware constructeur. Remplace Aura, iCUE, etc.",
            FullDescriptionEn = "OpenRGB unifies RGB lighting control for all your devices (motherboards, RAM, GPU, peripherals) without manufacturer bloatware. Replaces Aura, iCUE, etc.",
            DisableImpact = "L'éclairage RGB reviendra aux effets par défaut ou restera éteint.",
            DisableImpactEn = "RGB lighting will return to default effects or stay off.",
            PerformanceImpact = "Très faible (~20-40 Mo RAM).",
            PerformanceImpactEn = "Very low (~20-40 MB RAM).",
            Recommendation = "Gardez activé pour appliquer vos profils RGB au démarrage.",
            RecommendationEn = "Keep enabled to apply your RGB profiles at startup.",
            Tags = "openrgb,rgb,eclairage,open source,universel",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "SignalRGB",
            Aliases = "Signal RGB",
            Publisher = "WhirlwindFX",
            ExecutableNames = "SignalRGB.exe",
            Category = KnowledgeCategory.Utility,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Contrôleur RGB avec effets avancés.",
            ShortDescriptionEn = "RGB controller with advanced effects.",
            FullDescription = "SignalRGB offre le contrôle RGB unifié avec des effets visuels avancés, des intégrations de jeux et du matériel de streaming. Alternative plus visuelle à OpenRGB.",
            FullDescriptionEn = "SignalRGB offers unified RGB control with advanced visual effects, game integrations and streaming hardware support. More visual alternative to OpenRGB.",
            DisableImpact = "Les effets RGB personnalisés ne s'appliqueront pas au démarrage.",
            DisableImpactEn = "Custom RGB effects will not apply at startup.",
            PerformanceImpact = "Modéré (~80-150 Mo RAM).",
            PerformanceImpactEn = "Moderate (~80-150 MB RAM).",
            Recommendation = "Gardez activé pour les effets RGB automatiques.",
            RecommendationEn = "Keep enabled for automatic RGB effects.",
            Tags = "signalrgb,rgb,eclairage,effets,gaming",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "FanControl",
            Aliases = "Fan Control",
            Publisher = "Rémi Mercier",
            ExecutableNames = "FanControl.exe",
            Category = KnowledgeCategory.Utility,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Contrôleur de ventilateurs open source.",
            ShortDescriptionEn = "Open source fan controller.",
            FullDescription = "FanControl permet de créer des courbes de ventilateurs personnalisées basées sur les températures CPU, GPU ou autres capteurs. Interface moderne et flexible.",
            FullDescriptionEn = "FanControl lets you create custom fan curves based on CPU, GPU or other sensor temperatures. Modern and flexible interface.",
            DisableImpact = "Les ventilateurs reviendront aux courbes par défaut du BIOS.",
            DisableImpactEn = "Fans will return to default BIOS curves.",
            PerformanceImpact = "Très faible (~15-30 Mo RAM).",
            PerformanceImpactEn = "Very low (~15-30 MB RAM).",
            Recommendation = "Gardez activé pour vos courbes de ventilateurs personnalisées.",
            RecommendationEn = "Keep enabled for your custom fan curves.",
            Tags = "fancontrol,ventilateurs,temperature,courbe,refroidissement",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "Argus Monitor",
            Aliases = "ArgusMonitor",
            Publisher = "Argotronic GmbH",
            ExecutableNames = "ArgusMonitor.exe",
            Category = KnowledgeCategory.Utility,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Monitoring et contrôle de ventilateurs.",
            ShortDescriptionEn = "Monitoring and fan control.",
            FullDescription = "Argus Monitor surveille les températures CPU, GPU, disques et contrôle les ventilateurs avec des courbes personnalisées. Affiche les données dans la barre des tâches.",
            FullDescriptionEn = "Argus Monitor monitors CPU, GPU, disk temperatures and controls fans with custom curves. Displays data in the taskbar.",
            DisableImpact = "Pas de monitoring au démarrage. Courbes de ventilateurs par défaut.",
            DisableImpactEn = "No monitoring at startup. Default fan curves.",
            PerformanceImpact = "Faible (~20-40 Mo RAM).",
            PerformanceImpactEn = "Low (~20-40 MB RAM).",
            Recommendation = "Gardez activé pour le contrôle des ventilateurs.",
            RecommendationEn = "Keep enabled for fan control.",
            Tags = "argus,monitor,ventilateurs,temperature,monitoring",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "SpeedFan",
            Aliases = "Speed Fan",
            Publisher = "Alfredo Milani Comparetti",
            ExecutableNames = "speedfan.exe",
            Category = KnowledgeCategory.Utility,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Utilitaire classique de contrôle de ventilateurs.",
            ShortDescriptionEn = "Fan control software.",
            FullDescription = "SpeedFan est un outil historique pour surveiller les températures et contrôler les ventilateurs. Moins maintenu mais toujours fonctionnel sur du matériel plus ancien.",
            FullDescriptionEn = "Monitors voltages, fan speeds and temperatures.",
            DisableImpact = "Pas de contrôle automatique des ventilateurs.",
            DisableImpactEn = "Fan control rules won't apply.",
            PerformanceImpact = "Très faible (~10-20 Mo RAM).",
            PerformanceImpactEn = "Low.",
            Recommendation = "Peut être remplacé par FanControl ou Argus Monitor sur le matériel récent.",
            RecommendationEn = "Keep enabled for thermal management.",
            Tags = "speedfan,ventilateurs,temperature,classique",
            LastUpdated = DateTime.Now
        });
    }

    private void SeedNetworkTools()
    {
        Save(new KnowledgeEntry
        {
            Name = "GlassWire",
            Aliases = "GlassWire Firewall",
            Publisher = "GlassWire",
            ExecutableNames = "GlassWire.exe,GWCtlSrv.exe",
            Category = KnowledgeCategory.Security,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Moniteur réseau et pare-feu visuel.",
            ShortDescriptionEn = "Network monitor and firewall.",
            FullDescription = "GlassWire surveille votre trafic réseau avec des graphiques visuels, détecte les connexions suspectes et peut bloquer les applications. Alerte sur les nouvelles connexions réseau.",
            FullDescriptionEn = "GlassWire visualizes network activity and provides a firewall to block connections.",
            DisableImpact = "Pas de surveillance réseau au démarrage. Le pare-feu Windows reste actif.",
            DisableImpactEn = "Network monitoring and alerts won't work.",
            PerformanceImpact = "Faible à modéré (~40-80 Mo RAM).",
            PerformanceImpactEn = "Low to moderate.",
            Recommendation = "Gardez activé pour la surveillance continue du réseau.",
            RecommendationEn = "Keep enabled for network security.",
            Tags = "glasswire,reseau,firewall,monitoring,securite",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "NetLimiter",
            Aliases = "NetLimiter 4",
            Publisher = "Locktime Software",
            ExecutableNames = "NLClientApp.exe,nlsvc.exe",
            Category = KnowledgeCategory.Utility,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Contrôleur de bande passante par application.",
            ShortDescriptionEn = "Internet traffic control.",
            FullDescription = "NetLimiter permet de limiter la bande passante utilisée par chaque application, de définir des priorités et de surveiller le trafic réseau en détail.",
            FullDescriptionEn = "Tool to monitor and control internet traffic for applications.",
            DisableImpact = "Les limites de bande passante ne seront pas appliquées au démarrage.",
            DisableImpactEn = "Traffic limits won't be enforced.",
            PerformanceImpact = "Faible (~30-50 Mo RAM).",
            PerformanceImpactEn = "Low.",
            Recommendation = "Gardez activé si vous limitez la bande passante de certaines apps.",
            RecommendationEn = "Keep enabled to enforce rules.",
            Tags = "netlimiter,bande passante,reseau,limite,priorite",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "Wireshark",
            Aliases = "Wireshark Network Analyzer",
            Publisher = "Wireshark Foundation",
            ExecutableNames = "Wireshark.exe,dumpcap.exe",
            Category = KnowledgeCategory.Utility,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Analyseur de protocoles réseau.",
            ShortDescriptionEn = "Network protocol analyzer.",
            FullDescription = "Wireshark capture et analyse le trafic réseau en détail. Outil essentiel pour le diagnostic réseau et la sécurité. Utilisé par les professionnels IT.",
            FullDescriptionEn = "Tool to capture and interactively browse the traffic running on a computer network.",
            DisableImpact = "Aucun impact. Wireshark ne devrait pas être dans le démarrage.",
            DisableImpactEn = "No impact.",
            PerformanceImpact = "Aucun au repos.",
            PerformanceImpactEn = "None.",
            Recommendation = "Ne devrait pas être dans le démarrage automatique.",
            RecommendationEn = "Should not be in startup.",
            Tags = "wireshark,reseau,analyse,paquets,diagnostic",
            LastUpdated = DateTime.Now
        });
    }

    private void SeedDisplayUtilities()
    {
        Save(new KnowledgeEntry
        {
            Name = "f.lux",
            Aliases = "Flux",
            Publisher = "f.lux Software LLC",
            ExecutableNames = "flux.exe",
            Category = KnowledgeCategory.Utility,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Filtre de lumière bleue automatique.",
            ShortDescriptionEn = "Screen color adapter.",
            FullDescription = "f.lux ajuste automatiquement la température des couleurs de votre écran selon l'heure du jour. Réduit la lumière bleue le soir pour améliorer le sommeil.",
            FullDescriptionEn = "Adjusts display color temperature according to time of day.",
            DisableImpact = "Pas de filtrage automatique de la lumière bleue.",
            DisableImpactEn = "Screen won't adjust colors.",
            PerformanceImpact = "Très faible (~10-20 Mo RAM).",
            PerformanceImpactEn = "Very low.",
            Recommendation = "Gardez activé pour la protection des yeux le soir. Windows 10/11 a une fonction similaire 'Éclairage nocturne'.",
            RecommendationEn = "Keep enabled if you use it.",
            Tags = "flux,lumiere bleue,yeux,sommeil,ecran",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "DisplayFusion",
            Aliases = "Display Fusion",
            Publisher = "Binary Fortress Software",
            ExecutableNames = "DisplayFusion.exe,DisplayFusionHookx64.exe",
            Category = KnowledgeCategory.Utility,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Gestionnaire multi-écrans avancé.",
            ShortDescriptionEn = "Multi-monitor tool.",
            FullDescription = "DisplayFusion améliore la gestion multi-écrans : barre des tâches sur chaque écran, fonds d'écran par moniteur, fenêtres accrochables, raccourcis clavier et profils.",
            FullDescriptionEn = "DisplayFusion makes multi-monitor life easier with taskbars, wallpapers, and window management.",
            DisableImpact = "Les fonctionnalités multi-écrans avancées ne seront pas disponibles.",
            DisableImpactEn = "Multi-monitor features won't be active.",
            PerformanceImpact = "Faible (~30-60 Mo RAM).",
            PerformanceImpactEn = "Low to moderate.",
            Recommendation = "Gardez activé si vous utilisez plusieurs moniteurs.",
            RecommendationEn = "Keep enabled if you use multiple monitors.",
            Tags = "displayfusion,multi ecran,moniteur,barre des taches",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "Rainmeter",
            Aliases = "Rainmeter Desktop",
            Publisher = "Rainmeter",
            ExecutableNames = "Rainmeter.exe",
            Category = KnowledgeCategory.Utility,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Widgets et personnalisation du bureau.",
            ShortDescriptionEn = "Desktop customization tool.",
            FullDescription = "Rainmeter affiche des widgets personnalisables sur le bureau : horloge, météo, monitoring système, lecteur audio, etc. Des milliers de skins disponibles.",
            FullDescriptionEn = "Allows displaying customizable skins on your desktop, like memory usage, battery, RSS feeds.",
            DisableImpact = "Les widgets Rainmeter ne s'afficheront pas sur le bureau.",
            DisableImpactEn = "Desktop skins won't appear.",
            PerformanceImpact = "Variable selon les skins (~20-100+ Mo RAM).",
            PerformanceImpactEn = "Low to moderate.",
            Recommendation = "Gardez activé pour vos widgets de bureau personnalisés.",
            RecommendationEn = "Keep enabled for custom desktop.",
            Tags = "rainmeter,widgets,bureau,personnalisation,skins",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "Wallpaper Engine",
            Aliases = "Wallpaper Engine Service",
            Publisher = "Kristjan Skutta",
            ExecutableNames = "wallpaper64.exe,wallpaper32.exe",
            Category = KnowledgeCategory.Utility,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Fonds d'écran animés et interactifs.",
            ShortDescriptionEn = "Animated wallpapers.",
            FullDescription = "Wallpaper Engine permet d'utiliser des fonds d'écran animés, vidéo ou interactifs. Steam Workshop avec des milliers de créations. Peut utiliser des pages web comme fond.",
            FullDescriptionEn = "Enables using live wallpapers on your Windows desktop.",
            DisableImpact = "Les fonds d'écran animés ne s'afficheront pas.",
            DisableImpactEn = "Live wallpapers won't load.",
            PerformanceImpact = "Variable (~50-200 Mo RAM, utilisation GPU selon le fond).",
            PerformanceImpactEn = "Moderate to High.",
            Recommendation = "Peut être désactivé si vous n'utilisez pas de fonds animés ou pour économiser des ressources.",
            RecommendationEn = "Keep enabled for live wallpapers.",
            Tags = "wallpaper engine,fond ecran,anime,video,steam",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "Lively Wallpaper",
            Aliases = "Lively",
            Publisher = "rocksdanister",
            ExecutableNames = "Lively.exe,livelywpf.exe",
            Category = KnowledgeCategory.Utility,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Fonds d'écran animés gratuit et open source.",
            ShortDescriptionEn = "Animated wallpaper.",
            FullDescription = "Lively est une alternative gratuite à Wallpaper Engine. Supporte les vidéos, GIFs, pages web et shaders comme fonds d'écran animés.",
            FullDescriptionEn = "Free and open-source software to set videos, webpages and GIFs as desktop wallpaper.",
            DisableImpact = "Les fonds d'écran animés ne s'afficheront pas.",
            DisableImpactEn = "Animated wallpaper won't load.",
            PerformanceImpact = "Variable (~30-150 Mo RAM).",
            PerformanceImpactEn = "Moderate to High.",
            Recommendation = "Peut être désactivé si vous n'utilisez pas de fonds animés.",
            RecommendationEn = "Keep enabled for animated desktop.",
            Tags = "lively,wallpaper,anime,gratuit,open source",
            LastUpdated = DateTime.Now
        });
    }

    private void SeedBloatware()
    {
        Save(new KnowledgeEntry
        {
            Name = "HP Support Assistant",
            Aliases = "HPSA,HP Support Solutions Framework",
            Publisher = "HP Inc.",
            ExecutableNames = "HPSF.exe,HPSupportSolutionsFrameworkService.exe",
            Category = KnowledgeCategory.Bloatware,
            SafetyLevel = SafetyLevel.RecommendedDisable,
            ShortDescription = "Utilitaire de support HP préinstallé.",
            ShortDescriptionEn = "HP support tool.",
            FullDescription = "HP Support Assistant vérifie les mises à jour de pilotes HP et l'état de la garantie. Souvent considéré comme bloatware car il consomme des ressources et affiche des publicités HP.",
            FullDescriptionEn = "Provides automated support, updates, and fixes for HP PCs and printers.",
            DisableImpact = "Pas de notification des mises à jour HP. Les pilotes peuvent être mis à jour via Windows Update ou manuellement.",
            DisableImpactEn = "Automatic HP updates won't be checked.",
            PerformanceImpact = "Modéré (~50-100 Mo RAM avec les services).",
            PerformanceImpactEn = "Low.",
            Recommendation = "Peut être désactivé ou désinstallé. Windows Update fournit les pilotes essentiels.",
            RecommendationEn = "Can be disabled, check updates manually.",
            Tags = "hp,bloatware,support,pilotes,preinstalle",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "Dell SupportAssist",
            Aliases = "Dell SupportAssist Agent,SupportAssistAgent",
            Publisher = "Dell Inc.",
            ExecutableNames = "SupportAssistAgent.exe,DellSupportAssistRemedationService.exe",
            Category = KnowledgeCategory.Bloatware,
            SafetyLevel = SafetyLevel.RecommendedDisable,
            ShortDescription = "Utilitaire de support Dell préinstallé.",
            ShortDescriptionEn = "Preinstalled Dell support utility.",
            FullDescription = "Dell SupportAssist surveille l'état de votre PC Dell, vérifie les mises à jour et peut collecter des données de diagnostic.",
            FullDescriptionEn = "Dell SupportAssist monitors your Dell PC health, checks for updates and may collect diagnostic data.",
            DisableImpact = "Pas de diagnostics automatiques ni de mises à jour Dell. Les pilotes peuvent être téléchargés manuellement.",
            DisableImpactEn = "No automatic diagnostics or Dell updates. Drivers can be downloaded manually.",
            PerformanceImpact = "Modéré (~40-80 Mo RAM).",
            PerformanceImpactEn = "Moderate (~40-80 MB RAM).",
            Recommendation = "Peut être désactivé. Utilisez Windows Update ou le site Dell pour les pilotes.",
            RecommendationEn = "Can be disabled. Use Windows Update or Dell website for drivers.",
            Tags = "dell,bloatware,support,pilotes,preinstalle",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "Dell Command | Update",
            Aliases = "Dell Command Update,DCU",
            Publisher = "Dell Inc.",
            ExecutableNames = "DellCommandUpdate.exe",
            Category = KnowledgeCategory.Bloatware,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Outil de mise à jour des pilotes Dell.",
            ShortDescriptionEn = "Dell driver update tool.",
            FullDescription = "Dell Command | Update scanne votre système Dell et propose les derniers pilotes et BIOS. C'est l'outil recommandé par Dell pour maintenir les pilotes à jour.",
            FullDescriptionEn = "Dell Command | Update scans your Dell system and offers latest drivers and BIOS. It's Dell's recommended tool for keeping drivers up to date.",
            DisableImpact = "Pas de vérification automatique des mises à jour Dell. Lancez-le manuellement occasionnellement.",
            DisableImpactEn = "No automatic Dell update checks. Run it manually occasionally.",
            PerformanceImpact = "Faible au repos (~20-30 Mo RAM).",
            PerformanceImpactEn = "Low at rest (~20-30 MB RAM).",
            Recommendation = "Peut être désactivé du démarrage. Lancez-le manuellement pour vérifier les mises à jour.",
            RecommendationEn = "Can be disabled from startup. Run it manually to check for updates.",
            Tags = "dell,pilotes,bios,mise a jour,driver",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "Dell Peripheral Manager",
            Aliases = "Dell Display Manager,DDM",
            Publisher = "Dell Inc.",
            ExecutableNames = "DellDisplayManager.exe,Dell.PeripheralManager.exe",
            Category = KnowledgeCategory.Hardware,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Gestionnaire de périphériques Dell (moniteurs, claviers, souris).",
            ShortDescriptionEn = "Dell peripheral manager (monitors, keyboards, mice).",
            FullDescription = "Dell Peripheral Manager permet de configurer les moniteurs Dell (luminosité, contraste, profils), les claviers et souris Dell. Offre des raccourcis de productivité pour les moniteurs multi-écrans.",
            FullDescriptionEn = "Dell Peripheral Manager allows configuring Dell monitors (brightness, contrast, profiles), Dell keyboards and mice. Offers productivity shortcuts for multi-monitor setups.",
            DisableImpact = "Les paramètres par défaut seront utilisés pour les moniteurs Dell. Pas de profils personnalisés.",
            DisableImpactEn = "Default settings will be used for Dell monitors. No custom profiles.",
            PerformanceImpact = "Faible (~20-40 Mo RAM).",
            PerformanceImpactEn = "Low (~20-40 MB RAM).",
            Recommendation = "Gardez activé si vous utilisez des moniteurs Dell avec des profils personnalisés.",
            RecommendationEn = "Keep enabled if you use Dell monitors with custom profiles.",
            Tags = "dell,moniteur,peripherique,display,ecran",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "Lenovo Vantage",
            Aliases = "Lenovo Vantage Service,ImController",
            Publisher = "Lenovo",
            ExecutableNames = "LenovoVantage.exe,ImController.exe",
            Category = KnowledgeCategory.Bloatware,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Utilitaire de gestion Lenovo.",
            ShortDescriptionEn = "Lenovo management utility.",
            FullDescription = "Lenovo Vantage permet de gérer les paramètres spécifiques Lenovo, les mises à jour de pilotes, les paramètres de batterie et les fonctionnalités ThinkPad.",
            FullDescriptionEn = "Lenovo Vantage allows managing Lenovo-specific settings, driver updates, battery settings and ThinkPad features.",
            DisableImpact = "Perte des fonctionnalités Lenovo spécifiques comme les modes de performance personnalisés.",
            DisableImpactEn = "Loss of Lenovo-specific features like custom performance modes.",
            PerformanceImpact = "Modéré (~50-100 Mo RAM).",
            PerformanceImpactEn = "Moderate (~50-100 MB RAM).",
            Recommendation = "Gardez si vous utilisez les fonctionnalités Lenovo. Peut être désactivé sinon.",
            RecommendationEn = "Keep if you use Lenovo features. Can be disabled otherwise.",
            Tags = "lenovo,thinkpad,vantage,pilotes,batterie",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "ASUS Armoury Crate",
            Aliases = "ArmouryCrate,ROG,ASUS Optimization",
            Publisher = "ASUSTeK COMPUTER INC.",
            ExecutableNames = "ArmouryCrate.exe,AsusCertService.exe,ROGLiveService.exe",
            Category = KnowledgeCategory.Hardware,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Centre de contrôle ASUS pour gaming.",
            ShortDescriptionEn = "ASUS gaming control center.",
            FullDescription = "Armoury Crate gère l'éclairage RGB Aura Sync, les profils de ventilateurs, les modes de performance et les mises à jour ASUS sur les PC gaming ROG/TUF.",
            FullDescriptionEn = "Armoury Crate manages Aura Sync RGB lighting, fan profiles, performance modes and ASUS updates on ROG/TUF gaming PCs.",
            DisableImpact = "Perte du contrôle RGB et des profils de performance personnalisés.",
            DisableImpactEn = "Loss of RGB control and custom performance profiles.",
            PerformanceImpact = "Modéré à élevé (~80-150 Mo RAM).",
            PerformanceImpactEn = "Moderate to high (~80-150 MB RAM).",
            Recommendation = "Gardez si vous utilisez les fonctionnalités RGB ou gaming. Peut être désactivé sur les PC non-gaming.",
            RecommendationEn = "Keep if you use RGB or gaming features. Can be disabled on non-gaming PCs.",
            Tags = "asus,rog,armoury,rgb,gaming,aura",
            LastUpdated = DateTime.Now
        });
    }

    private void SeedWindowsServices()
    {
        // Core Windows Services
        Save(new KnowledgeEntry
        {
            Name = "Print Spooler",
            Aliases = "Spooler,spoolsv",
            Publisher = "Microsoft Corporation",
            ExecutableNames = "spoolsv.exe",
            Category = KnowledgeCategory.WindowsSystem,
            SafetyLevel = SafetyLevel.Important,
            ShortDescription = "Gère les travaux d'impression et les files d'attente.",
            ShortDescriptionEn = "Manages print jobs and queues.",
            FullDescription = "Le service Spouleur d'impression gère toutes les imprimantes locales et réseau, met en file d'attente les travaux d'impression et communique avec les pilotes d'imprimante. Essentiel si vous utilisez une imprimante.",
            FullDescriptionEn = "The Print Spooler service manages all local and network printers, queues print jobs and communicates with printer drivers. Essential if you use a printer.",
            DisableImpact = "Aucune impression possible. Les imprimantes ne seront pas détectées.",
            DisableImpactEn = "No printing possible. Printers will not be detected.",
            PerformanceImpact = "Faible (~5-15 Mo RAM). Impact minimal sauf lors d'impressions actives.",
            PerformanceImpactEn = "Low (~5-15 MB RAM). Minimal impact except during active printing.",
            Recommendation = "Gardez activé si vous avez une imprimante. Peut être désactivé sur les PC sans imprimante.",
            RecommendationEn = "Keep enabled if you have a printer. Can be disabled on PCs without a printer.",
            Tags = "impression,printer,spooler,windows,service",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "Background Intelligent Transfer Service",
            Aliases = "BITS",
            Publisher = "Microsoft Corporation",
            ExecutableNames = "svchost.exe",
            Category = KnowledgeCategory.WindowsSystem,
            SafetyLevel = SafetyLevel.Critical,
            ShortDescription = "Service de transfert de fichiers en arrière-plan pour Windows Update.",
            ShortDescriptionEn = "Background file transfer service for Windows Update.",
            FullDescription = "BITS transfère des fichiers en arrière-plan en utilisant la bande passante inutilisée. Il est utilisé par Windows Update, Microsoft Store et d'autres services Microsoft pour télécharger les mises à jour sans impacter votre connexion.",
            FullDescriptionEn = "BITS transfers files in the background using unused bandwidth. It's used by Windows Update, Microsoft Store and other Microsoft services to download updates without impacting your connection.",
            DisableImpact = "Windows Update ne fonctionnera plus. Le Microsoft Store ne pourra pas télécharger d'applications.",
            DisableImpactEn = "Windows Update will no longer work. Microsoft Store will not be able to download applications.",
            PerformanceImpact = "Variable selon les téléchargements. Conçu pour être non-intrusif.",
            PerformanceImpactEn = "Variable depending on downloads. Designed to be non-intrusive.",
            Recommendation = "Ne jamais désactiver. Essentiel pour les mises à jour Windows.",
            RecommendationEn = "Never disable. Essential for Windows updates.",
            Tags = "bits,update,windows,transfert,telechargement",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "Windows Audio",
            Aliases = "AudioSrv,AudioEndpointBuilder",
            Publisher = "Microsoft Corporation",
            ExecutableNames = "svchost.exe",
            Category = KnowledgeCategory.WindowsSystem,
            SafetyLevel = SafetyLevel.Critical,
            ShortDescription = "Gère le son sur Windows.",
            ShortDescriptionEn = "Manages sound on Windows.",
            FullDescription = "Windows Audio gère le son pour les programmes Windows. AudioEndpointBuilder détecte les périphériques audio et permet la commutation dynamique.",
            FullDescriptionEn = "Windows Audio manages sound for Windows programs. AudioEndpointBuilder detects audio devices and enables dynamic switching.",
            DisableImpact = "Aucun son sur le système. Ni haut-parleurs, ni casque ne fonctionneront.",
            DisableImpactEn = "No sound on the system. Neither speakers nor headphones will work.",
            PerformanceImpact = "Très faible (~5-10 Mo RAM).",
            PerformanceImpactEn = "Very low (~5-10 MB RAM).",
            Recommendation = "Ne jamais désactiver sauf sur des serveurs sans audio.",
            RecommendationEn = "Never disable except on servers without audio.",
            Tags = "audio,son,windows,service,haut-parleur",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "Themes",
            Aliases = "Themes Service",
            Publisher = "Microsoft Corporation",
            ExecutableNames = "svchost.exe",
            Category = KnowledgeCategory.WindowsSystem,
            SafetyLevel = SafetyLevel.Important,
            ShortDescription = "Gère les thèmes visuels Windows.",
            ShortDescriptionEn = "Manages Windows visual themes.",
            FullDescription = "Le service Thèmes fournit la gestion des thèmes de l'expérience utilisateur. Il permet les effets visuels, les couleurs d'accentuation et les thèmes personnalisés.",
            FullDescriptionEn = "The Themes service provides user experience theme management. It enables visual effects, accent colors and custom themes.",
            DisableImpact = "L'interface revient au thème Windows classique. Perte des effets visuels et transparences.",
            DisableImpactEn = "Interface reverts to Windows classic theme. Loss of visual effects and transparency.",
            PerformanceImpact = "Faible (~10-20 Mo RAM).",
            PerformanceImpactEn = "Low (~10-20 MB RAM).",
            Recommendation = "Gardez activé pour une interface moderne. Peut être désactivé pour économiser des ressources.",
            RecommendationEn = "Keep enabled for a modern interface. Can be disabled to save resources.",
            Tags = "themes,visuel,interface,windows,apparence",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "Windows Event Log",
            Aliases = "EventLog",
            Publisher = "Microsoft Corporation",
            ExecutableNames = "svchost.exe",
            Category = KnowledgeCategory.WindowsSystem,
            SafetyLevel = SafetyLevel.Critical,
            ShortDescription = "Enregistre les événements système et application.",
            ShortDescriptionEn = "Records system and application events.",
            FullDescription = "Le service Journal des événements Windows gère les événements et journaux d'événements. Il permet le diagnostic des problèmes, la surveillance de sécurité et l'audit du système.",
            FullDescriptionEn = "The Windows Event Log service manages events and event logs. It enables problem diagnostics, security monitoring and system auditing.",
            DisableImpact = "Impossible de diagnostiquer les problèmes. Journaux de sécurité non enregistrés. Certaines applications peuvent échouer.",
            DisableImpactEn = "Unable to diagnose problems. Security logs not recorded. Some applications may fail.",
            PerformanceImpact = "Faible (~10-20 Mo RAM). Écriture disque périodique.",
            PerformanceImpactEn = "Low (~10-20 MB RAM). Periodic disk writes.",
            Recommendation = "Ne jamais désactiver. Essentiel pour le dépannage et la sécurité.",
            RecommendationEn = "Never disable. Essential for troubleshooting and security.",
            Tags = "eventlog,journal,evenements,diagnostic,windows",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "SysMain",
            Aliases = "Superfetch,SysMain",
            Publisher = "Microsoft Corporation",
            ExecutableNames = "svchost.exe",
            Category = KnowledgeCategory.WindowsSystem,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Optimise les performances en préchargeant les applications fréquentes.",
            ShortDescriptionEn = "Optimizes performance by preloading frequently used applications.",
            FullDescription = "SysMain (anciennement Superfetch) analyse vos habitudes d'utilisation et précharge les applications fréquemment utilisées en RAM pour un démarrage plus rapide.",
            FullDescriptionEn = "SysMain (formerly Superfetch) analyzes your usage patterns and preloads frequently used applications in RAM for faster startup.",
            DisableImpact = "Les applications mettront plus de temps à démarrer au premier lancement.",
            DisableImpactEn = "Applications will take longer to start on first launch.",
            PerformanceImpact = "Modéré (~50-200 Mo RAM utilisée pour le cache). Peut causer de l'activité disque sur les HDD.",
            PerformanceImpactEn = "Moderate (~50-200 MB RAM used for cache). May cause disk activity on HDDs.",
            Recommendation = "Gardez activé sur les PC avec SSD. Peut être désactivé sur les PC avec HDD si cela cause des ralentissements.",
            RecommendationEn = "Keep enabled on PCs with SSD. Can be disabled on PCs with HDD if it causes slowdowns.",
            Tags = "sysmain,superfetch,performance,cache,windows",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "Windows Time",
            Aliases = "W32Time",
            Publisher = "Microsoft Corporation",
            ExecutableNames = "svchost.exe",
            Category = KnowledgeCategory.WindowsSystem,
            SafetyLevel = SafetyLevel.Important,
            ShortDescription = "Synchronise l'horloge système avec les serveurs de temps.",
            ShortDescriptionEn = "Synchronizes system clock with time servers.",
            FullDescription = "Le service Temps Windows maintient la synchronisation de la date et l'heure sur tous les clients et serveurs du réseau. Utilise NTP pour la synchronisation.",
            FullDescriptionEn = "The Windows Time service maintains date and time synchronization on all network clients and servers. Uses NTP for synchronization.",
            DisableImpact = "L'heure du système peut dériver. Problèmes de certificats SSL et authentification possibles.",
            DisableImpactEn = "System time may drift. Possible SSL certificate and authentication issues.",
            PerformanceImpact = "Négligeable. Synchronisation périodique uniquement.",
            PerformanceImpactEn = "Negligible. Periodic synchronization only.",
            Recommendation = "Gardez activé pour une heure précise. Essentiel pour les environnements d'entreprise.",
            RecommendationEn = "Keep enabled for accurate time. Essential for enterprise environments.",
            Tags = "time,heure,ntp,synchronisation,windows",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "DNS Client",
            Aliases = "Dnscache",
            Publisher = "Microsoft Corporation",
            ExecutableNames = "svchost.exe",
            Category = KnowledgeCategory.WindowsSystem,
            SafetyLevel = SafetyLevel.Critical,
            ShortDescription = "Met en cache les requêtes DNS pour améliorer les performances réseau.",
            ShortDescriptionEn = "Caches DNS queries to improve network performance.",
            FullDescription = "Le service Client DNS résout et met en cache les noms de domaine DNS. Il accélère l'accès aux sites web en évitant de refaire les requêtes DNS.",
            FullDescriptionEn = "The DNS Client service resolves and caches DNS domain names. It speeds up access to websites by avoiding repeated DNS queries.",
            DisableImpact = "Chaque requête DNS sera effectuée sans cache. Navigation web plus lente.",
            DisableImpactEn = "Every DNS query will be performed without cache. Slower web browsing.",
            PerformanceImpact = "Faible (~5-10 Mo RAM pour le cache).",
            PerformanceImpactEn = "Low (~5-10 MB RAM for cache).",
            Recommendation = "Ne jamais désactiver. Essentiel pour le réseau.",
            RecommendationEn = "Never disable. Essential for networking.",
            Tags = "dns,reseau,cache,internet,windows",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "DHCP Client",
            Aliases = "Dhcp",
            Publisher = "Microsoft Corporation",
            ExecutableNames = "svchost.exe",
            Category = KnowledgeCategory.WindowsSystem,
            SafetyLevel = SafetyLevel.Critical,
            ShortDescription = "Obtient automatiquement l'adresse IP du réseau.",
            ShortDescriptionEn = "Automatically obtains network IP address.",
            FullDescription = "Le service Client DHCP enregistre et met à jour les adresses IP et les enregistrements DNS. Sans lui, vous devez configurer manuellement l'adresse IP.",
            FullDescriptionEn = "The DHCP Client service registers and updates IP addresses and DNS records. Without it, you must manually configure the IP address.",
            DisableImpact = "Pas de connexion réseau automatique. Configuration IP manuelle requise.",
            DisableImpactEn = "No automatic network connection. Manual IP configuration required.",
            PerformanceImpact = "Négligeable.",
            PerformanceImpactEn = "Negligible.",
            Recommendation = "Ne jamais désactiver sauf si vous utilisez des IP statiques.",
            RecommendationEn = "Never disable unless you use static IPs.",
            Tags = "dhcp,reseau,ip,adresse,windows",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "Cryptographic Services",
            Aliases = "CryptSvc",
            Publisher = "Microsoft Corporation",
            ExecutableNames = "svchost.exe",
            Category = KnowledgeCategory.WindowsSecurity,
            SafetyLevel = SafetyLevel.Critical,
            ShortDescription = "Fournit les services de cryptographie Windows.",
            ShortDescriptionEn = "Provides Windows cryptography services.",
            FullDescription = "Les services de chiffrement fournissent la gestion des clés, la signature de code et la vérification des certificats. Utilisé par Windows Update et les applications sécurisées.",
            FullDescriptionEn = "Cryptographic Services provide key management, code signing and certificate verification. Used by Windows Update and secure applications.",
            DisableImpact = "Windows Update échouera. Problèmes avec les sites HTTPS et les applications signées.",
            DisableImpactEn = "Windows Update will fail. Issues with HTTPS sites and signed applications.",
            PerformanceImpact = "Faible (~10-20 Mo RAM).",
            PerformanceImpactEn = "Low (~10-20 MB RAM).",
            Recommendation = "Ne jamais désactiver. Essentiel pour la sécurité.",
            RecommendationEn = "Never disable. Essential for security.",
            Tags = "crypto,certificats,securite,windows,chiffrement",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "Windows Font Cache Service",
            Aliases = "FontCache",
            Publisher = "Microsoft Corporation",
            ExecutableNames = "svchost.exe",
            Category = KnowledgeCategory.WindowsSystem,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Optimise les performances des polices.",
            ShortDescriptionEn = "Optimizes font performance.",
            FullDescription = "Le service Cache de polices Windows optimise les performances des applications en mettant en cache les données de polices courantes.",
            FullDescriptionEn = "The Windows Font Cache Service optimizes application performance by caching common font data.",
            DisableImpact = "Les applications peuvent démarrer plus lentement. Rendu des polices moins performant.",
            DisableImpactEn = "Applications may start slower. Less performant font rendering.",
            PerformanceImpact = "Faible (~10-30 Mo RAM).",
            PerformanceImpactEn = "Low (~10-30 MB RAM).",
            Recommendation = "Gardez activé pour de meilleures performances d'affichage.",
            RecommendationEn = "Keep enabled for better display performance.",
            Tags = "fonts,polices,cache,windows,performance",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "Task Scheduler",
            Aliases = "Schedule",
            Publisher = "Microsoft Corporation",
            ExecutableNames = "svchost.exe",
            Category = KnowledgeCategory.WindowsSystem,
            SafetyLevel = SafetyLevel.Critical,
            ShortDescription = "Exécute les tâches planifiées.",
            ShortDescriptionEn = "Executes scheduled tasks.",
            FullDescription = "Le Planificateur de tâches permet de programmer des tâches automatiques. Windows et de nombreuses applications l'utilisent pour la maintenance, les mises à jour et les sauvegardes.",
            FullDescriptionEn = "Task Scheduler allows scheduling automated tasks. Windows and many applications use it for maintenance, updates and backups.",
            DisableImpact = "Aucune tâche planifiée ne s'exécutera. Maintenance Windows compromise. Beaucoup d'applications ne fonctionneront pas correctement.",
            DisableImpactEn = "No scheduled tasks will run. Windows maintenance compromised. Many applications will not work properly.",
            PerformanceImpact = "Faible (~10-20 Mo RAM).",
            PerformanceImpactEn = "Low (~10-20 MB RAM).",
            Recommendation = "Ne jamais désactiver. Composant essentiel de Windows.",
            RecommendationEn = "Never disable. Essential Windows component.",
            Tags = "scheduler,taches,planificateur,automatisation,windows",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "Windows Management Instrumentation",
            Aliases = "WMI,Winmgmt",
            Publisher = "Microsoft Corporation",
            ExecutableNames = "svchost.exe,WmiPrvSE.exe",
            Category = KnowledgeCategory.WindowsSystem,
            SafetyLevel = SafetyLevel.Critical,
            ShortDescription = "Interface de gestion système Windows.",
            ShortDescriptionEn = "System management infrastructure.",
            FullDescription = "WMI fournit une interface commune pour accéder aux informations de gestion du système. Utilisé par les outils de monitoring, les scripts et les applications de gestion.",
            FullDescriptionEn = "Provides a common interface and object model to access management information about operating system.",
            DisableImpact = "De nombreuses applications de gestion échoueront. Monitoring système impossible. Scripts PowerShell affectés.",
            DisableImpactEn = "Many applications will fail.",
            PerformanceImpact = "Modéré (~20-50 Mo RAM).",
            PerformanceImpactEn = "Low.",
            Recommendation = "Ne jamais désactiver. Essentiel pour la gestion du système.",
            RecommendationEn = "Critical. Do not disable.",
            Tags = "wmi,management,gestion,monitoring,windows",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "Remote Procedure Call",
            Aliases = "RpcSs,RPC",
            Publisher = "Microsoft Corporation",
            ExecutableNames = "svchost.exe",
            Category = KnowledgeCategory.WindowsSystem,
            SafetyLevel = SafetyLevel.Critical,
            ShortDescription = "Service fondamental pour la communication entre processus.",
            ShortDescriptionEn = "System communication service.",
            FullDescription = "RPC est le service de base qui permet la communication entre les processus Windows. Pratiquement tous les services Windows en dépendent.",
            FullDescriptionEn = "Serves as the endpoint mapper for RPC services.",
            DisableImpact = "Windows ne fonctionnera pas. Le système sera instable ou ne démarrera pas.",
            DisableImpactEn = "System will fail to function.",
            PerformanceImpact = "Faible (partie intégrante de Windows).",
            PerformanceImpactEn = "Low.",
            Recommendation = "Ne jamais désactiver. Composant critique de Windows.",
            RecommendationEn = "Critical system service. Do not disable.",
            Tags = "rpc,communication,processus,windows,systeme",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "Windows Installer",
            Aliases = "msiserver",
            Publisher = "Microsoft Corporation",
            ExecutableNames = "msiexec.exe",
            Category = KnowledgeCategory.WindowsSystem,
            SafetyLevel = SafetyLevel.Important,
            ShortDescription = "Gère l'installation et la désinstallation des programmes.",
            ShortDescriptionEn = "Installation service.",
            FullDescription = "Windows Installer gère l'installation, la modification et la suppression des applications utilisant le format MSI. De nombreux programmes professionnels utilisent ce format.",
            FullDescriptionEn = "Adds, modifies, and removes applications provided as a Windows Installer package (*.msi).",
            DisableImpact = "Impossible d'installer ou désinstaller les programmes MSI.",
            DisableImpactEn = "Software installation will fail.",
            PerformanceImpact = "Faible. S'exécute uniquement lors des installations.",
            PerformanceImpactEn = "Low.",
            Recommendation = "Gardez activé. Peut être en démarrage manuel.",
            RecommendationEn = "Critical. Do not disable.",
            Tags = "installer,msi,installation,programmes,windows",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "Windows Update",
            Aliases = "wuauserv,Windows Update Service",
            Publisher = "Microsoft Corporation",
            ExecutableNames = "svchost.exe",
            Category = KnowledgeCategory.WindowsSystem,
            SafetyLevel = SafetyLevel.Critical,
            ShortDescription = "Détecte, télécharge et installe les mises à jour Windows.",
            ShortDescriptionEn = "Detects, downloads and installs Windows updates.",
            FullDescription = "Le service Windows Update permet la détection, le téléchargement et l'installation des mises à jour pour Windows et les autres produits Microsoft.",
            FullDescriptionEn = "Windows Update service enables detection, download and installation of updates for Windows and other Microsoft products.",
            DisableImpact = "Aucune mise à jour de sécurité. Le système devient vulnérable aux failles de sécurité.",
            DisableImpactEn = "No security updates. System becomes vulnerable to security flaws.",
            PerformanceImpact = "Variable. Peut utiliser CPU et réseau lors des mises à jour.",
            PerformanceImpactEn = "Variable. May use CPU and network during updates.",
            Recommendation = "Ne jamais désactiver. Les mises à jour de sécurité sont essentielles.",
            RecommendationEn = "Never disable. Security updates are essential.",
            Tags = "update,mises à jour,securite,windows,patches",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "User Profile Service",
            Aliases = "ProfSvc",
            Publisher = "Microsoft Corporation",
            ExecutableNames = "svchost.exe",
            Category = KnowledgeCategory.WindowsSystem,
            SafetyLevel = SafetyLevel.Critical,
            ShortDescription = "Gère le chargement et déchargement des profils utilisateur.",
            ShortDescriptionEn = "User profile manager.",
            FullDescription = "Ce service est responsable du chargement et déchargement des profils utilisateur. Il gère les dossiers Documents, Bureau et les paramètres utilisateur.",
            FullDescriptionEn = "Responsible for loading and unloading user profiles.",
            DisableImpact = "Impossible de se connecter à Windows. Les profils utilisateur ne se chargeront pas.",
            DisableImpactEn = "Users will be unable to log on.",
            PerformanceImpact = "Faible.",
            PerformanceImpactEn = "Low.",
            Recommendation = "Ne jamais désactiver. Essentiel pour l'ouverture de session.",
            RecommendationEn = "Critical system service. Do not disable.",
            Tags = "profil,utilisateur,session,windows,connexion",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "Windows Firewall",
            Aliases = "mpssvc,Windows Defender Firewall",
            Publisher = "Microsoft Corporation",
            ExecutableNames = "svchost.exe",
            Category = KnowledgeCategory.WindowsSecurity,
            SafetyLevel = SafetyLevel.Critical,
            ShortDescription = "Protège le PC contre les accès réseau non autorisés.",
            ShortDescriptionEn = "Protects PC against unauthorized network access.",
            FullDescription = "Le Pare-feu Windows Defender filtre le trafic réseau entrant et sortant selon des règles de sécurité. Protection de base contre les attaques réseau.",
            FullDescriptionEn = "Windows Defender Firewall filters incoming and outgoing network traffic according to security rules. Basic protection against network attacks.",
            DisableImpact = "Le PC sera vulnérable aux attaques réseau. Aucun filtrage du trafic.",
            DisableImpactEn = "PC will be vulnerable to network attacks. No traffic filtering.",
            PerformanceImpact = "Très faible.",
            PerformanceImpactEn = "Very low.",
            Recommendation = "Ne jamais désactiver sauf si un autre pare-feu est installé.",
            RecommendationEn = "Never disable unless another firewall is installed.",
            Tags = "firewall,pare-feu,securite,reseau,windows",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "Windows Defender",
            Aliases = "WinDefend,Windows Defender Antivirus Service",
            Publisher = "Microsoft Corporation",
            ExecutableNames = "MsMpEng.exe,NisSrv.exe",
            Category = KnowledgeCategory.WindowsSecurity,
            SafetyLevel = SafetyLevel.Critical,
            ShortDescription = "Protection antivirus intégrée à Windows.",
            ShortDescriptionEn = "Built-in Windows antivirus protection.",
            FullDescription = "Windows Defender fournit une protection en temps réel contre les virus, malwares, spywares et autres menaces. Intégré à Windows 10/11.",
            FullDescriptionEn = "Windows Defender provides real-time protection against viruses, malware, spyware and other threats. Built into Windows 10/11.",
            DisableImpact = "Le PC sera vulnérable aux malwares. Protection en temps réel désactivée.",
            DisableImpactEn = "PC will be vulnerable to malware. Real-time protection disabled.",
            PerformanceImpact = "Modéré (~100-200 Mo RAM). Peut utiliser du CPU lors des analyses.",
            PerformanceImpactEn = "Moderate (~100-200 MB RAM). May use CPU during scans.",
            Recommendation = "Ne jamais désactiver sauf si un autre antivirus est installé.",
            RecommendationEn = "Never disable unless another antivirus is installed.",
            Tags = "defender,antivirus,securite,malware,windows",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "Network Location Awareness",
            Aliases = "NlaSvc,NLA",
            Publisher = "Microsoft Corporation",
            ExecutableNames = "svchost.exe",
            Category = KnowledgeCategory.WindowsSystem,
            SafetyLevel = SafetyLevel.Important,
            ShortDescription = "Détecte le type de réseau (public, privé, domaine).",
            ShortDescriptionEn = "Network info service.",
            FullDescription = "Ce service collecte les informations sur le réseau et notifie les applications des changements. Il détermine si vous êtes sur un réseau public ou privé.",
            FullDescriptionEn = "Collects and stores configuration information for the network.",
            DisableImpact = "Windows ne détectera pas le type de réseau. Les paramètres de pare-feu peuvent être incorrects.",
            DisableImpactEn = "Network connectivity issues may occur.",
            PerformanceImpact = "Négligeable.",
            PerformanceImpactEn = "Low.",
            Recommendation = "Gardez activé pour une configuration réseau correcte.",
            RecommendationEn = "Do not disable.",
            Tags = "reseau,nla,detection,firewall,windows",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "Server",
            Aliases = "LanmanServer",
            Publisher = "Microsoft Corporation",
            ExecutableNames = "svchost.exe",
            Category = KnowledgeCategory.WindowsSystem,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Permet le partage de fichiers et d'imprimantes.",
            ShortDescriptionEn = "File and print sharing.",
            FullDescription = "Le service Serveur prend en charge le partage de fichiers, d'imprimantes et de canaux nommés sur le réseau. Essentiel pour le partage réseau.",
            FullDescriptionEn = "Supports file, print, and named-pipe sharing over the network.",
            DisableImpact = "Impossible de partager des fichiers ou imprimantes. Les autres PC ne pourront pas accéder à vos partages.",
            DisableImpactEn = "File sharing won't work.",
            PerformanceImpact = "Faible sauf lors de transferts actifs.",
            PerformanceImpactEn = "Low.",
            Recommendation = "Gardez activé si vous partagez des fichiers. Peut être désactivé sur les PC isolés.",
            RecommendationEn = "Keep enabled if sharing files.",
            Tags = "serveur,partage,fichiers,reseau,smb",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "Workstation",
            Aliases = "LanmanWorkstation",
            Publisher = "Microsoft Corporation",
            ExecutableNames = "svchost.exe",
            Category = KnowledgeCategory.WindowsSystem,
            SafetyLevel = SafetyLevel.Important,
            ShortDescription = "Permet la connexion aux partages réseau.",
            ShortDescriptionEn = "Network connections service.",
            FullDescription = "Le service Station de travail crée et maintient les connexions aux serveurs distants via le protocole SMB. Permet d'accéder aux partages réseau.",
            FullDescriptionEn = "Creates and maintains client network connections to remote servers.",
            DisableImpact = "Impossible d'accéder aux dossiers partagés sur le réseau.",
            DisableImpactEn = "Network access will be lost.",
            PerformanceImpact = "Faible.",
            PerformanceImpactEn = "Low.",
            Recommendation = "Gardez activé pour accéder aux ressources réseau.",
            RecommendationEn = "Critical. Do not disable.",
            Tags = "workstation,reseau,partage,smb,windows",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "Plug and Play",
            Aliases = "PlugPlay,PnP",
            Publisher = "Microsoft Corporation",
            ExecutableNames = "svchost.exe",
            Category = KnowledgeCategory.WindowsSystem,
            SafetyLevel = SafetyLevel.Critical,
            ShortDescription = "Détecte et configure automatiquement le matériel.",
            ShortDescriptionEn = "Hardware recognition service.",
            FullDescription = "Plug and Play permet à Windows de reconnaître et configurer automatiquement les périphériques sans intervention manuelle. Essentiel pour l'USB.",
            FullDescriptionEn = "Enables a computer to recognize and adapt to hardware changes with little or no user intervention.",
            DisableImpact = "Les nouveaux périphériques ne seront pas détectés. L'USB pourrait ne pas fonctionner.",
            DisableImpactEn = "System instability and hardware failure.",
            PerformanceImpact = "Très faible.",
            PerformanceImpactEn = "Low.",
            Recommendation = "Ne jamais désactiver. Composant essentiel de Windows.",
            RecommendationEn = "Critical system service. Do not disable.",
            Tags = "pnp,plug and play,materiel,usb,windows",
            LastUpdated = DateTime.Now
        });
    }

    private void SeedScheduledTasks()
    {
        Save(new KnowledgeEntry
        {
            Name = "GoogleUpdateTaskMachine",
            Aliases = "Google Update,GoogleUpdateTaskMachineCore,GoogleUpdateTaskMachineUA",
            Publisher = "Google LLC",
            ExecutableNames = "GoogleUpdate.exe",
            Category = KnowledgeCategory.Utility,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Met à jour les produits Google (Chrome, Drive, Earth...).",
            ShortDescriptionEn = "Google software updater.",
            FullDescription = "Google Update vérifie et installe les mises à jour pour Chrome, Google Drive, Google Earth et autres produits Google. S'exécute périodiquement en arrière-plan.",
            FullDescriptionEn = "System-wide update task for Google software (Chrome, Earth, etc.).",
            DisableImpact = "Les produits Google ne se mettront plus à jour automatiquement. Vulnérabilités de sécurité possibles.",
            DisableImpactEn = "Google software won't update automatically.",
            PerformanceImpact = "Très faible. S'exécute brièvement à intervalles.",
            PerformanceImpactEn = "Very low.",
            Recommendation = "Gardez activé pour les mises à jour de sécurité Chrome.",
            RecommendationEn = "Keep enabled for security updates.",
            Tags = "google,update,chrome,tache planifiee",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "MicrosoftEdgeUpdateTask",
            Aliases = "Microsoft Edge Update,MicrosoftEdgeUpdateTaskMachineCore,MicrosoftEdgeUpdateTaskMachineUA",
            Publisher = "Microsoft Corporation",
            ExecutableNames = "MicrosoftEdgeUpdate.exe",
            Category = KnowledgeCategory.WindowsSystem,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Met à jour Microsoft Edge.",
            ShortDescriptionEn = "Edge browser updater.",
            FullDescription = "Cette tâche vérifie et installe les mises à jour pour le navigateur Microsoft Edge. Importante pour la sécurité du navigateur.",
            FullDescriptionEn = "Scheduled task to keep Microsoft Edge up to date.",
            DisableImpact = "Edge ne se mettra plus à jour automatiquement.",
            DisableImpactEn = "Edge won't update automatically.",
            PerformanceImpact = "Très faible.",
            PerformanceImpactEn = "Very low.",
            Recommendation = "Gardez activé si vous utilisez Edge.",
            RecommendationEn = "Keep enabled for security.",
            Tags = "edge,microsoft,update,navigateur,tache",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "OneDrive Standalone Update Task",
            Aliases = "OneDrive Per-Machine Standalone Update Task",
            Publisher = "Microsoft Corporation",
            ExecutableNames = "OneDriveSetup.exe",
            Category = KnowledgeCategory.CloudStorage,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Met à jour OneDrive.",
            ShortDescriptionEn = "OneDrive updater.",
            FullDescription = "Cette tâche maintient OneDrive à jour avec les dernières fonctionnalités et correctifs de sécurité.",
            FullDescriptionEn = "Task to keep the OneDrive client up to date.",
            DisableImpact = "OneDrive ne se mettra plus à jour automatiquement.",
            DisableImpactEn = "OneDrive might become outdated.",
            PerformanceImpact = "Très faible.",
            PerformanceImpactEn = "Very low.",
            Recommendation = "Gardez activé si vous utilisez OneDrive.",
            RecommendationEn = "Keep enabled.",
            Tags = "onedrive,microsoft,cloud,update,tache",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "Office Automatic Updates",
            Aliases = "Office Automatic Updates 2.0",
            Publisher = "Microsoft Corporation",
            ExecutableNames = "OfficeC2RClient.exe",
            Category = KnowledgeCategory.Productivity,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Met à jour Microsoft Office.",
            ShortDescriptionEn = "Microsoft Office updates.",
            FullDescription = "Cette tâche télécharge et installe les mises à jour pour Microsoft Office (Word, Excel, PowerPoint, Outlook...). Importante pour la sécurité et les nouvelles fonctionnalités.",
            FullDescriptionEn = "Scheduled task to check for updates for Microsoft Office.",
            DisableImpact = "Office ne recevra plus de mises à jour automatiques. Vulnérabilités possibles.",
            DisableImpactEn = "Office won't update automatically.",
            PerformanceImpact = "Modéré lors des mises à jour. Faible au repos.",
            PerformanceImpactEn = "Very low.",
            Recommendation = "Gardez activé pour la sécurité d'Office.",
            RecommendationEn = "Keep enabled for security.",
            Tags = "office,microsoft,update,word,excel,tache",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "MpIdleTask",
            Aliases = "Windows Defender Scheduled Scan",
            Publisher = "Microsoft Corporation",
            ExecutableNames = "MpCmdRun.exe",
            Category = KnowledgeCategory.WindowsSecurity,
            SafetyLevel = SafetyLevel.Important,
            ShortDescription = "Analyse antivirus planifiée de Windows Defender.",
            ShortDescriptionEn = "Windows Defender maintenance.",
            FullDescription = "Cette tâche effectue des analyses antivirus pendant les périodes d'inactivité du système. Partie intégrante de la protection Windows Defender.",
            FullDescriptionEn = "Scheduled maintenance task for Windows Defender Antivirus.",
            DisableImpact = "Pas d'analyses automatiques. Dépendance à la protection en temps réel uniquement.",
            DisableImpactEn = "Antivirus maintenance won't run idle.",
            PerformanceImpact = "Peut utiliser des ressources significatives pendant l'analyse. Conçu pour s'exécuter en période d'inactivité.",
            PerformanceImpactEn = "Low.",
            Recommendation = "Gardez activé pour une protection complète.",
            RecommendationEn = "Do not disable.",
            Tags = "defender,antivirus,scan,analyse,securite,tache",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "ScanForUpdates",
            Aliases = "Windows Update ScanForUpdates",
            Publisher = "Microsoft Corporation",
            ExecutableNames = "UsoClient.exe,svchost.exe",
            Category = KnowledgeCategory.WindowsSystem,
            SafetyLevel = SafetyLevel.Important,
            ShortDescription = "Recherche les mises à jour Windows disponibles.",
            ShortDescriptionEn = "Update scanner task.",
            FullDescription = "Cette tâche vérifie régulièrement si de nouvelles mises à jour Windows sont disponibles. Prépare les téléchargements et installations.",
            FullDescriptionEn = "Generic task usually associated with software update checks (e.g., Mozilla, Google).",
            DisableImpact = "Windows ne recherchera plus automatiquement les mises à jour.",
            DisableImpactEn = "Software updates won't be detected.",
            PerformanceImpact = "Faible. Utilise le réseau brièvement.",
            PerformanceImpactEn = "Very low.",
            Recommendation = "Gardez activé pour recevoir les mises à jour de sécurité.",
            RecommendationEn = "Keep enabled.",
            Tags = "update,windows,recherche,mises a jour,tache",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "Consolidator",
            Aliases = "Microsoft Compatibility Appraiser",
            Publisher = "Microsoft Corporation",
            ExecutableNames = "CompatTelRunner.exe",
            Category = KnowledgeCategory.WindowsSystem,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Collecte les données de télémétrie Windows.",
            ShortDescriptionEn = "Windows Customer Experience Improvement.",
            FullDescription = "Cette tâche collecte des informations sur la compatibilité du système pour les mises à jour Windows. Fait partie du programme d'amélioration de l'expérience Windows.",
            FullDescriptionEn = "Collects usage data for the Customer Experience Improvement Program (CEIP).",
            DisableImpact = "Microsoft ne recevra plus de données de compatibilité. Aucun impact fonctionnel.",
            DisableImpactEn = "Usage data won't be sent to Microsoft.",
            PerformanceImpact = "Peut utiliser du CPU périodiquement. Parfois gourmand sur les anciens systèmes.",
            PerformanceImpactEn = "Very low.",
            Recommendation = "Peut être désactivé pour la confidentialité ou les performances.",
            RecommendationEn = "Can be safely disabled.",
            Tags = "telemetrie,compatibilite,microsoft,confidentialite,tache",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "Disk Cleanup",
            Aliases = "SilentCleanup",
            Publisher = "Microsoft Corporation",
            ExecutableNames = "cleanmgr.exe",
            Category = KnowledgeCategory.WindowsSystem,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Nettoie automatiquement les fichiers temporaires.",
            ShortDescriptionEn = "Windows disk cleaning tool.",
            FullDescription = "Cette tâche exécute le nettoyage de disque silencieux pour supprimer les fichiers temporaires, le cache et autres fichiers inutiles.",
            FullDescriptionEn = "Utility to free up disk space by removing unnecessary files.",
            DisableImpact = "Les fichiers temporaires s'accumuleront. Nettoyage manuel nécessaire.",
            DisableImpactEn = "Scheduled cleanups won't run.",
            PerformanceImpact = "Faible. Libère de l'espace disque.",
            PerformanceImpactEn = "Low.",
            Recommendation = "Gardez activé pour maintenir l'espace disque.",
            RecommendationEn = "Can be disabled from automatic startup.",
            Tags = "nettoyage,disque,temporaire,cache,maintenance",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "StartupAppTask",
            Aliases = "Startup App Task",
            Publisher = "Microsoft Corporation",
            ExecutableNames = "svchost.exe",
            Category = KnowledgeCategory.WindowsSystem,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Surveille l'impact des applications au démarrage.",
            ShortDescriptionEn = "Startup app manager.",
            FullDescription = "Cette tâche analyse l'impact des applications de démarrage et fournit des recommandations dans le Gestionnaire des tâches.",
            FullDescriptionEn = "Task that handles the launching of startup apps for the user.",
            DisableImpact = "Pas de mesure d'impact au démarrage. Fonctionnalité informative uniquement.",
            DisableImpactEn = "Startup apps might not launch.",
            PerformanceImpact = "Négligeable.",
            PerformanceImpactEn = "Low.",
            Recommendation = "Gardez activé pour les informations de démarrage.",
            RecommendationEn = "Do not disable.",
            Tags = "demarrage,startup,performance,analyse,tache",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "CCleaner Scheduled Task",
            Aliases = "CCleaner,CCleanerSkipUAC",
            Publisher = "Piriform",
            ExecutableNames = "CCleaner64.exe,CCleaner.exe",
            Category = KnowledgeCategory.Utility,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Nettoyage automatique planifié par CCleaner.",
            ShortDescriptionEn = "CCleaner scheduled maintenance.",
            FullDescription = "Exécute CCleaner automatiquement pour nettoyer les fichiers temporaires, le cache des navigateurs et autres fichiers inutiles.",
            FullDescriptionEn = "Task to run CCleaner cleaning or updates automatically.",
            DisableImpact = "Pas de nettoyage automatique. CCleaner devra être lancé manuellement.",
            DisableImpactEn = "Automatic cleaning won't run.",
            PerformanceImpact = "Modéré pendant le nettoyage.",
            PerformanceImpactEn = "Low.",
            Recommendation = "Optionnel. Utile pour un nettoyage régulier automatique.",
            RecommendationEn = "Can be disabled if you prefer manual cleaning.",
            Tags = "ccleaner,nettoyage,cache,piriform,tache",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "Adobe Acrobat Update Task",
            Aliases = "AdobeAcrobatUpdateTask",
            Publisher = "Adobe Inc.",
            ExecutableNames = "armsvc.exe",
            Category = KnowledgeCategory.Productivity,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Met à jour Adobe Acrobat et Reader.",
            ShortDescriptionEn = "Adobe Acrobat update task.",
            FullDescription = "Cette tâche vérifie et installe les mises à jour pour Adobe Acrobat et Reader. Important car les PDF peuvent contenir des vulnérabilités.",
            FullDescriptionEn = "Scheduled task to keep Adobe Acrobat products up to date.",
            DisableImpact = "Acrobat/Reader ne se mettra plus à jour automatiquement.",
            DisableImpactEn = "Adobe software won't update automatically.",
            PerformanceImpact = "Très faible.",
            PerformanceImpactEn = "Very low.",
            Recommendation = "Gardez activé pour les correctifs de sécurité PDF.",
            RecommendationEn = "Keep enabled for security updates.",
            Tags = "adobe,acrobat,reader,pdf,update,tache",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "NvTmRepOnLogon",
            Aliases = "NVIDIA Telemetry,NvTmRep,NvTmMon",
            Publisher = "NVIDIA Corporation",
            ExecutableNames = "nvtmrep.exe",
            Category = KnowledgeCategory.Hardware,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Télémétrie NVIDIA.",
            ShortDescriptionEn = "NVIDIA telemetry.",
            FullDescription = "Collecte des données d'utilisation pour NVIDIA. Aide NVIDIA à améliorer ses pilotes et logiciels.",
            FullDescriptionEn = "Task related to NVIDIA telemetry and crash reporting.",
            DisableImpact = "NVIDIA ne recevra plus de données d'utilisation. Aucun impact fonctionnel.",
            DisableImpactEn = "Crash reports won't be sent to NVIDIA.",
            PerformanceImpact = "Très faible.",
            PerformanceImpactEn = "Very low.",
            Recommendation = "Peut être désactivé pour la confidentialité.",
            RecommendationEn = "Can be disabled.",
            Tags = "nvidia,telemetrie,gpu,confidentialite,tache",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "CreateExplorerShellUnelevatedTask",
            Aliases = "CreateExplorerShellUnelevatedTask",
            Publisher = "Microsoft Corporation",
            ExecutableNames = "explorer.exe",
            Category = KnowledgeCategory.WindowsSystem,
            SafetyLevel = SafetyLevel.Important,
            ShortDescription = "Lance l'Explorateur Windows au démarrage.",
            ShortDescriptionEn = "Explorer shell task.",
            FullDescription = "Cette tâche s'assure que l'Explorateur Windows (le bureau et la barre des tâches) démarre correctement avec les droits utilisateur normaux.",
            FullDescriptionEn = "Task related to launching Explorer shell components with specific privileges.",
            DisableImpact = "Peut causer des problèmes avec le bureau et l'Explorateur.",
            DisableImpactEn = "May affect Explorer stability or features.",
            PerformanceImpact = "Aucun (s'exécute une fois au démarrage).",
            PerformanceImpactEn = "Very low.",
            Recommendation = "Ne jamais désactiver.",
            RecommendationEn = "Keep enabled.",
            Tags = "explorateur,bureau,shell,windows,tache",
            LastUpdated = DateTime.Now
        });
    }

    private void SeedWindowsRunEntries()
    {
        Save(new KnowledgeEntry
        {
            Name = "ctfmon",
            Aliases = "CTF Loader,TextInputHost",
            Publisher = "Microsoft Corporation",
            ExecutableNames = "ctfmon.exe,TextInputHost.exe",
            Category = KnowledgeCategory.WindowsSystem,
            SafetyLevel = SafetyLevel.Important,
            ShortDescription = "Gère la saisie de texte et les langues.",
            ShortDescriptionEn = "Text input loader.",
            FullDescription = "CTF Loader (ctfmon.exe) active le processeur de texte alternatif et la barre de langue Microsoft Office. Gère les méthodes d'entrée et la reconnaissance d'écriture manuscrite.",
            FullDescriptionEn = "Activates the Alternative User Input Text Processor (TIP) and Microsoft Office Language Bar.",
            DisableImpact = "La barre de langue et certaines fonctionnalités de saisie de texte peuvent ne pas fonctionner.",
            DisableImpactEn = "Language bar/input methods issues.",
            PerformanceImpact = "Très faible (~5-10 Mo RAM).",
            PerformanceImpactEn = "Very low.",
            Recommendation = "Gardez activé si vous utilisez plusieurs langues ou la saisie spéciale.",
            RecommendationEn = "Do not disable.",
            Tags = "ctfmon,langue,saisie,clavier,windows",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "VMware User Process",
            Aliases = "vmtoolsd,VMware Tools",
            Publisher = "VMware, Inc.",
            ExecutableNames = "vmtoolsd.exe",
            Category = KnowledgeCategory.Utility,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Outils d'intégration pour les machines virtuelles VMware.",
            ShortDescriptionEn = "VMware tools user process.",
            FullDescription = "VMware Tools améliore les performances et la gestion des machines virtuelles. Permet le copier-coller entre hôte et VM, le redimensionnement de l'écran et la synchronisation de l'heure.",
            FullDescriptionEn = "Enables features like copy-paste and drag-drop between host and guest in VMware.",
            DisableImpact = "Perte du copier-coller hôte/VM, performances graphiques réduites dans la VM.",
            DisableImpactEn = "Integration features won't work.",
            PerformanceImpact = "Faible (~20-40 Mo RAM).",
            PerformanceImpactEn = "Low.",
            Recommendation = "Gardez activé si vous êtes dans une VM VMware.",
            RecommendationEn = "Keep enabled in a VM.",
            Tags = "vmware,virtualisation,tools,vm,machine virtuelle",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "VirtualBox Guest Additions",
            Aliases = "VBoxTray,VirtualBox",
            Publisher = "Oracle Corporation",
            ExecutableNames = "VBoxTray.exe",
            Category = KnowledgeCategory.Utility,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Outils d'intégration pour VirtualBox.",
            ShortDescriptionEn = "VirtualBox guest integration.",
            FullDescription = "Les Guest Additions VirtualBox permettent les dossiers partagés, le copier-coller, le redimensionnement de l'écran et l'intégration du pointeur souris.",
            FullDescriptionEn = "Provides better integration between host and guest systems (mouse, screen, file sharing).",
            DisableImpact = "Perte des fonctionnalités d'intégration VirtualBox.",
            DisableImpactEn = "Integration features won't work.",
            PerformanceImpact = "Faible (~15-30 Mo RAM).",
            PerformanceImpactEn = "Low.",
            Recommendation = "Gardez activé si vous êtes dans une VM VirtualBox.",
            RecommendationEn = "Keep enabled in a VM.",
            Tags = "virtualbox,oracle,vm,guest additions,virtualisation",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "igfxtray",
            Aliases = "Intel Graphics Tray,igfxTray",
            Publisher = "Intel Corporation",
            ExecutableNames = "igfxtray.exe,igfxEM.exe,igfxHK.exe",
            Category = KnowledgeCategory.Hardware,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Icône de la barre des tâches pour les graphiques Intel.",
            ShortDescriptionEn = "Intel Graphics Tray.",
            FullDescription = "Fournit un accès rapide aux paramètres des graphiques Intel intégrés depuis la barre des tâches. Permet de changer la résolution et les paramètres d'affichage.",
            FullDescriptionEn = "System tray icon for Intel Graphics settings.",
            DisableImpact = "Pas d'icône Intel dans la barre des tâches. Les paramètres restent accessibles via le Panneau de configuration Intel.",
            DisableImpactEn = "Quick access to graphics settings hidden.",
            PerformanceImpact = "Très faible.",
            PerformanceImpactEn = "Very low.",
            Recommendation = "Peut être désactivé si vous n'utilisez pas les raccourcis Intel.",
            RecommendationEn = "Can be disabled.",
            Tags = "intel,graphiques,gpu,tray,affichage",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "Bluetooth Swift Pair",
            Aliases = "BluetoothUserService,bthserv",
            Publisher = "Microsoft Corporation",
            ExecutableNames = "svchost.exe",
            Category = KnowledgeCategory.WindowsSystem,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Facilite l'appairage rapide des périphériques Bluetooth.",
            ShortDescriptionEn = "Quick Bluetooth pairing service.",
            FullDescription = "Swift Pair affiche des notifications lorsqu'un nouveau périphérique Bluetooth est détecté à proximité, permettant un appairage en un clic.",
            FullDescriptionEn = "Facilitates quick and easy pairing of supported Bluetooth devices when they are nearby.",
            DisableImpact = "Pas de notifications d'appairage rapide. Appairage manuel toujours possible.",
            DisableImpactEn = "Fast pairing notifications won't appear.",
            PerformanceImpact = "Négligeable.",
            PerformanceImpactEn = "Very low.",
            Recommendation = "Gardez activé si vous utilisez régulièrement le Bluetooth.",
            RecommendationEn = "Keep enabled for convenience.",
            Tags = "bluetooth,swift pair,appairage,peripherique,windows",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "Windows Terminal",
            Aliases = "WindowsTerminal,wt",
            Publisher = "Microsoft Corporation",
            ExecutableNames = "WindowsTerminal.exe,wt.exe",
            Category = KnowledgeCategory.Utility,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Terminal moderne pour Windows.",
            ShortDescriptionEn = "Terminal emulator.",
            FullDescription = "Windows Terminal est l'application de terminal moderne de Microsoft qui combine PowerShell, Command Prompt, WSL et d'autres shells dans une interface à onglets.",
            FullDescriptionEn = "Modern terminal application for command-line tools like Command Prompt, PowerShell, and WSL.",
            DisableImpact = "Le terminal ne s'ouvrira pas au démarrage (comportement normal).",
            DisableImpactEn = "Quake mode/global hotkey won't work.",
            PerformanceImpact = "Aucun au démarrage (lance à la demande).",
            PerformanceImpactEn = "Low.",
            Recommendation = "N'a généralement pas besoin de démarrer avec Windows.",
            RecommendationEn = "Keep enabled if using global hotkey.",
            Tags = "terminal,powershell,cmd,wsl,developpement",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "Windows Sandbox",
            Aliases = "WindowsSandbox",
            Publisher = "Microsoft Corporation",
            ExecutableNames = "WindowsSandbox.exe",
            Category = KnowledgeCategory.Security,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Environnement Windows isolé jetable.",
            ShortDescriptionEn = "Temporary desktop environment.",
            FullDescription = "Windows Sandbox crée un environnement de bureau léger et temporaire pour exécuter des applications en isolation. Tout est supprimé à la fermeture.",
            FullDescriptionEn = "Lightweight desktop environment to safely run applications in isolation.",
            DisableImpact = "N'a pas besoin de démarrer avec Windows.",
            DisableImpactEn = "Sandbox features won't work.",
            PerformanceImpact = "Aucun au démarrage.",
            PerformanceImpactEn = "Low.",
            Recommendation = "Ne devrait pas être dans les programmes de démarrage.",
            RecommendationEn = "Keep enabled if used.",
            Tags = "sandbox,isolation,securite,test,windows",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "PowerShell",
            Aliases = "pwsh,powershell.exe",
            Publisher = "Microsoft Corporation",
            ExecutableNames = "powershell.exe,pwsh.exe",
            Category = KnowledgeCategory.Utility,
            SafetyLevel = SafetyLevel.Important,
            ShortDescription = "Shell de ligne de commande et langage de script.",
            ShortDescriptionEn = "Command-line shell.",
            FullDescription = "PowerShell est un shell de ligne de commande et un langage de script puissant. Sa présence au démarrage peut être normale (scripts) ou suspecte (malware).",
            FullDescriptionEn = "Task automation and configuration management framework.",
            DisableImpact = "Les scripts PowerShell au démarrage ne s'exécuteront pas.",
            DisableImpactEn = "Scripts won't run.",
            PerformanceImpact = "Variable selon le script.",
            PerformanceImpactEn = "Varies.",
            Recommendation = "Vérifiez le script exécuté. Les malwares utilisent souvent PowerShell.",
            RecommendationEn = "Depends on the specific script.",
            Tags = "powershell,script,shell,commande,attention",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "cmd",
            Aliases = "Command Prompt,cmd.exe",
            Publisher = "Microsoft Corporation",
            ExecutableNames = "cmd.exe",
            Category = KnowledgeCategory.WindowsSystem,
            SafetyLevel = SafetyLevel.Important,
            ShortDescription = "Invite de commandes Windows classique.",
            ShortDescriptionEn = "Command prompt.",
            FullDescription = "L'invite de commandes Windows au démarrage exécute généralement un script batch. Peut être légitime ou suspect selon le script.",
            FullDescriptionEn = "Windows command line interpreter.",
            DisableImpact = "Les scripts batch au démarrage ne s'exécuteront pas.",
            DisableImpactEn = "Scripts won't run.",
            PerformanceImpact = "Variable selon le script.",
            PerformanceImpactEn = "Very low.",
            Recommendation = "Examinez la commande complète. Peut indiquer un malware.",
            RecommendationEn = "Check why it starts.",
            Tags = "cmd,batch,script,commande,attention",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "wscript",
            Aliases = "Windows Script Host,wscript.exe,cscript.exe",
            Publisher = "Microsoft Corporation",
            ExecutableNames = "wscript.exe,cscript.exe",
            Category = KnowledgeCategory.WindowsSystem,
            SafetyLevel = SafetyLevel.Important,
            ShortDescription = "Hôte de scripts Windows pour VBScript et JScript.",
            ShortDescriptionEn = "Windows Script Host.",
            FullDescription = "Windows Script Host exécute des scripts VBS et JS. Souvent utilisé par les malwares pour leurs capacités de scripting.",
            FullDescriptionEn = "Provides an environment in which users can execute scripts in a variety of languages.",
            DisableImpact = "Les scripts VBS/JS au démarrage ne s'exécuteront pas.",
            DisableImpactEn = "Scripts won't run.",
            PerformanceImpact = "Variable selon le script.",
            PerformanceImpactEn = "Very low.",
            Recommendation = "Inspectez le script. Vecteur de malware courant.",
            RecommendationEn = "Investigate the script.",
            Tags = "wscript,vbscript,jscript,script,attention,malware",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "mshta",
            Aliases = "Microsoft HTML Application Host",
            Publisher = "Microsoft Corporation",
            ExecutableNames = "mshta.exe",
            Category = KnowledgeCategory.WindowsSystem,
            SafetyLevel = SafetyLevel.Critical,
            ShortDescription = "Exécute les applications HTML.",
            ShortDescriptionEn = "HTML Application host.",
            FullDescription = "MSHTA exécute les fichiers HTA (HTML Application). Très rarement légitime au démarrage. Fréquemment utilisé par les malwares.",
            FullDescriptionEn = "Executes HTML Applications (.hta). Often used by malware but also legitimate scripts.",
            DisableImpact = "Les applications HTA au démarrage ne s'exécuteront pas.",
            DisableImpactEn = "HTA apps won't run.",
            PerformanceImpact = "Variable.",
            PerformanceImpactEn = "Low.",
            Recommendation = "SUSPECT : Vérifiez immédiatement. Vecteur de malware très courant.",
            RecommendationEn = "Investigate why it starts.",
            Tags = "mshta,hta,html,script,malware,suspect,danger",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "rundll32",
            Aliases = "rundll32.exe",
            Publisher = "Microsoft Corporation",
            ExecutableNames = "rundll32.exe",
            Category = KnowledgeCategory.WindowsSystem,
            SafetyLevel = SafetyLevel.Important,
            ShortDescription = "Exécute des fonctions dans les DLL.",
            ShortDescriptionEn = "DLL runner.",
            FullDescription = "Rundll32 exécute des fonctions exportées depuis des fichiers DLL. Utilisé légitimement par Windows et les applications, mais aussi par les malwares.",
            FullDescriptionEn = "Loads and runs 32-bit dynamic-link libraries (DLLs).",
            DisableImpact = "Variable selon la DLL appelée.",
            DisableImpactEn = "Functionality provided by DLL won't work.",
            PerformanceImpact = "Variable.",
            PerformanceImpactEn = "Low.",
            Recommendation = "Examinez la DLL et la fonction appelées. Peut être légitime ou malveillant.",
            RecommendationEn = "Investigate the target DLL.",
            Tags = "rundll32,dll,windows,attention",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "regsvr32",
            Aliases = "regsvr32.exe",
            Publisher = "Microsoft Corporation",
            ExecutableNames = "regsvr32.exe",
            Category = KnowledgeCategory.WindowsSystem,
            SafetyLevel = SafetyLevel.Important,
            ShortDescription = "Enregistre les DLL et contrôles ActiveX.",
            ShortDescriptionEn = "DLL registrar.",
            FullDescription = "Regsvr32 enregistre et désenregistre les DLL et contrôles OLE dans le registre. Peut être utilisé pour exécuter du code malveillant.",
            FullDescriptionEn = "Command-line tool to register and unregister OLE controls (DLLs and ActiveX).",
            DisableImpact = "L'enregistrement de DLL au démarrage ne se fera pas.",
            DisableImpactEn = "Depends on the DLL registered.",
            PerformanceImpact = "Faible.",
            PerformanceImpactEn = "Low.",
            Recommendation = "Rare au démarrage. Vérifiez la DLL concernée.",
            RecommendationEn = "Investigate why it starts.",
            Tags = "regsvr32,dll,registre,activex,attention",
            LastUpdated = DateTime.Now
        });
    }

    /// <summary>
    /// Seeds additional gaming platforms and overlays.
    /// </summary>
    private void SeedGamingPlatforms()
    {
        Save(new KnowledgeEntry
        {
            Name = "Overwolf",
            Aliases = "OverwolfLauncher",
            Publisher = "Overwolf Ltd",
            ExecutableNames = "Overwolf.exe,OverwolfLauncher.exe",
            Category = KnowledgeCategory.Gaming,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Plateforme d'applications et overlays pour les jeux.",
            ShortDescriptionEn = "Platform for gaming apps and overlays.",
            FullDescription = "Overwolf est une plateforme qui permet d'exécuter des applications et overlays pendant les jeux. Héberge des apps comme CurseForge pour les mods.",
            FullDescriptionEn = "Overwolf is a platform for running apps and overlays during games. Hosts apps like CurseForge for mods.",
            DisableImpact = "Les overlays et apps Overwolf ne seront pas disponibles en jeu.",
            DisableImpactEn = "Overwolf overlays and apps won't be available in-game.",
            PerformanceImpact = "Modéré. Consomme des ressources pour les overlays.",
            PerformanceImpactEn = "Moderate. Uses resources for overlays.",
            Recommendation = "Peut être désactivé si non utilisé. Relancez manuellement si besoin.",
            RecommendationEn = "Can be disabled if not used. Relaunch manually if needed.",
            Tags = "overwolf,gaming,overlay,mods,curseforge",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "CurseForge",
            Aliases = "CurseForge App",
            Publisher = "Overwolf Ltd.",
            ExecutableNames = "CurseForge.exe",
            Category = KnowledgeCategory.Gaming,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Gestionnaire de mods pour Minecraft, WoW et autres jeux.",
            ShortDescriptionEn = "Mod manager for games.",
            FullDescription = "CurseForge est un gestionnaire de mods populaire pour Minecraft, World of Warcraft, et de nombreux autres jeux. Fait partie de l'écosystème Overwolf.",
            FullDescriptionEn = "Platform to download and manage mods for games like WoW, Minecraft, etc.",
            DisableImpact = "Les mods ne seront pas mis à jour automatiquement.",
            DisableImpactEn = "Updates and mod management won't start automatically.",
            PerformanceImpact = "Faible au démarrage.",
            PerformanceImpactEn = "Moderate.",
            Recommendation = "Peut être désactivé. Lancez manuellement pour gérer les mods.",
            RecommendationEn = "Disable if you don't play modded games daily.",
            Tags = "curseforge,mods,minecraft,wow,gaming",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "itch.io",
            Aliases = "itch,itch Desktop App",
            Publisher = "itch corp",
            ExecutableNames = "itch.exe,itch-setup.exe",
            Category = KnowledgeCategory.Gaming,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Plateforme de distribution de jeux indépendants.",
            ShortDescriptionEn = "Indie game distribution platform.",
            FullDescription = "itch.io est une plateforme de distribution de jeux indépendants. L'application desktop permet de télécharger et gérer les jeux achetés.",
            FullDescriptionEn = "itch.io is an indie game distribution platform. The desktop app allows downloading and managing purchased games.",
            DisableImpact = "Pas de mise à jour automatique des jeux itch.io.",
            DisableImpactEn = "No automatic updates for itch.io games.",
            PerformanceImpact = "Faible.",
            PerformanceImpactEn = "Low.",
            Recommendation = "Peut être désactivé. Lancez manuellement quand nécessaire.",
            RecommendationEn = "Can be disabled. Launch manually when needed.",
            Tags = "itch,indie,games,distribution",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "Humble App",
            Aliases = "Humble Bundle",
            Publisher = "Humble Bundle, Inc.",
            ExecutableNames = "Humble App.exe,HumbleApp.exe",
            Category = KnowledgeCategory.Gaming,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Application Humble Bundle pour gérer les jeux.",
            ShortDescriptionEn = "Humble Bundle game launcher.",
            FullDescription = "L'application Humble permet de télécharger et gérer les jeux achetés sur Humble Bundle, ainsi que les jeux du Humble Choice.",
            FullDescriptionEn = "Launcher for games from the Humble Choice membership and Trove.",
            DisableImpact = "Pas de mise à jour automatique des jeux Humble.",
            DisableImpactEn = "App won't start automatically.",
            PerformanceImpact = "Faible.",
            PerformanceImpactEn = "Low.",
            Recommendation = "Peut être désactivé si vous n'utilisez pas souvent Humble.",
            RecommendationEn = "Disable from startup.",
            Tags = "humble,bundle,games,distribution",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "Twitch",
            Aliases = "Twitch App,Twitch Desktop",
            Publisher = "Amazon/Twitch",
            ExecutableNames = "Twitch.exe",
            Category = KnowledgeCategory.Media,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Application de streaming Twitch.",
            ShortDescriptionEn = "Twitch desktop app.",
            FullDescription = "L'application desktop Twitch permet de regarder des streams, gérer les abonnements et recevoir des notifications.",
            FullDescriptionEn = "Desktop application for Twitch streaming and viewing.",
            DisableImpact = "Pas de notifications de streams en direct.",
            DisableImpactEn = "Notifications won't appear.",
            PerformanceImpact = "Modéré si actif en arrière-plan.",
            PerformanceImpactEn = "Low to moderate.",
            Recommendation = "Peut être désactivé. Utilisez le site web comme alternative.",
            RecommendationEn = "Disable from startup.",
            Tags = "twitch,streaming,amazon,gaming",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "Guilded",
            Aliases = "Guilded App",
            Publisher = "Guilded, Inc. (Roblox)",
            ExecutableNames = "Guilded.exe",
            Category = KnowledgeCategory.Communication,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Alternative à Discord pour les communautés gaming.",
            ShortDescriptionEn = "Gaming chat platform.",
            FullDescription = "Guilded est une plateforme de communication pour les communautés gaming, offrant chat, forums, calendriers et streaming intégrés.",
            FullDescriptionEn = "Chat and team organization platform for gaming communities, similar to Discord.",
            DisableImpact = "Pas de notifications ni messages instantanés.",
            DisableImpactEn = "Notifications won't appear.",
            PerformanceImpact = "Modéré. Similaire à Discord.",
            PerformanceImpactEn = "Moderate.",
            Recommendation = "Peut être désactivé si utilisé occasionnellement.",
            RecommendationEn = "Keep enabled if you use it for communication.",
            Tags = "guilded,chat,gaming,community,roblox",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "Prism Launcher",
            Aliases = "PrismLauncher,PolyMC,MultiMC",
            Publisher = "Prism Launcher Contributors",
            ExecutableNames = "prismlauncher.exe",
            Category = KnowledgeCategory.Gaming,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Lanceur Minecraft open source avec gestion d'instances.",
            ShortDescriptionEn = "Minecraft launcher.",
            FullDescription = "Prism Launcher est un lanceur Minecraft open source permettant de gérer plusieurs instances avec différentes versions et mods.",
            FullDescriptionEn = "Open-source launcher for Minecraft, managing multiple instances/accounts.",
            DisableImpact = "Minecraft ne sera pas lancé automatiquement.",
            DisableImpactEn = "Launcher won't start automatically.",
            PerformanceImpact = "Aucun si pas utilisé.",
            PerformanceImpactEn = "Low.",
            Recommendation = "N'a pas besoin de démarrer avec Windows.",
            RecommendationEn = "Disable from startup.",
            Tags = "minecraft,launcher,prism,mods,gaming",
            LastUpdated = DateTime.Now
        });
    }

    /// <summary>
    /// Seeds AI assistant applications.
    /// </summary>
    private void SeedAIApps()
    {
        Save(new KnowledgeEntry
        {
            Name = "Perplexity",
            Aliases = "Perplexity AI,Perplexity Desktop,com.todesktop.25020447d4kq915",
            Publisher = "PERPLEXITY AI, INC.",
            ExecutableNames = "Perplexity.exe",
            Category = KnowledgeCategory.Productivity,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Assistant de recherche IA avec sources citées.",
            ShortDescriptionEn = "AI research assistant with cited sources.",
            FullDescription = "Perplexity est un assistant de recherche alimenté par l'IA qui fournit des réponses avec des sources citées. L'application desktop offre un accès rapide.",
            FullDescriptionEn = "Perplexity is an AI-powered research assistant that provides answers with cited sources. The desktop app offers quick access.",
            DisableImpact = "Pas d'accès rapide via raccourci global.",
            DisableImpactEn = "No quick access via global shortcut.",
            PerformanceImpact = "Faible à modéré.",
            PerformanceImpactEn = "Low to moderate.",
            Recommendation = "Peut être désactivé si utilisé via le navigateur.",
            RecommendationEn = "Can be disabled if you use the browser version.",
            Tags = "perplexity,ia,ai,recherche,search,assistant",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "Claude",
            Aliases = "Claude Desktop,Claude AI",
            Publisher = "Anthropic, PBC",
            ExecutableNames = "Claude.exe,claude.exe",
            Category = KnowledgeCategory.Productivity,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Application desktop de l'assistant IA Claude.",
            ShortDescriptionEn = "Desktop app for Claude AI assistant.",
            FullDescription = "Claude est un assistant IA développé par Anthropic. L'application desktop offre un accès direct sans navigateur.",
            FullDescriptionEn = "Claude is an AI assistant developed by Anthropic. The desktop app provides direct access without a browser.",
            DisableImpact = "Claude ne sera pas accessible via raccourci global.",
            DisableImpactEn = "Claude won't be accessible via global shortcut.",
            PerformanceImpact = "Faible.",
            PerformanceImpactEn = "Low.",
            Recommendation = "Peut être désactivé. Lancez manuellement si besoin.",
            RecommendationEn = "Can be disabled. Launch manually if needed.",
            Tags = "claude,anthropic,ia,ai,assistant,llm",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "ChatGPT",
            Aliases = "ChatGPT Desktop,OpenAI",
            Publisher = "OpenAI",
            ExecutableNames = "ChatGPT.exe",
            Category = KnowledgeCategory.Productivity,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Application desktop ChatGPT d'OpenAI.",
            ShortDescriptionEn = "AI desktop application.",
            FullDescription = "ChatGPT est l'assistant IA conversationnel d'OpenAI. L'application desktop permet un accès rapide avec raccourcis globaux.",
            FullDescriptionEn = "Desktop client for OpenAI's ChatGPT.",
            DisableImpact = "Pas de raccourci global pour accéder à ChatGPT.",
            DisableImpactEn = "Quick access shortcut won't be available.",
            PerformanceImpact = "Faible.",
            PerformanceImpactEn = "Low.",
            Recommendation = "Peut être désactivé. Utilisez le site web comme alternative.",
            RecommendationEn = "Can be disabled from startup.",
            Tags = "chatgpt,openai,ia,ai,assistant,gpt,llm",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "Pieces",
            Aliases = "Pieces for Developers,Pieces OS",
            Publisher = "Mesh Intelligent Technologies, Inc.",
            ExecutableNames = "Pieces.exe,Pieces OS.exe",
            Category = KnowledgeCategory.Productivity,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Assistant IA pour développeurs avec gestion de snippets.",
            ShortDescriptionEn = "AI code snippet manager.",
            FullDescription = "Pieces est un outil IA pour développeurs qui aide à sauvegarder, enrichir et réutiliser des snippets de code avec contexte.",
            FullDescriptionEn = "Tool to save, generate, and enrich code snippets using AI.",
            DisableImpact = "Les fonctionnalités d'IA et snippets ne seront pas disponibles.",
            DisableImpactEn = "OS integration won't be active.",
            PerformanceImpact = "Modéré. Exécute un modèle IA local.",
            PerformanceImpactEn = "Low.",
            Recommendation = "Désactivez si non utilisé régulièrement.",
            RecommendationEn = "Disable if not used constantly.",
            Tags = "pieces,developpeur,code,snippets,ia,ai",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "Codeium",
            Aliases = "Codeium AI,Windsurf",
            Publisher = "Exafunction, Inc.",
            ExecutableNames = "Codeium.exe,Windsurf.exe",
            Category = KnowledgeCategory.Productivity,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Assistant de code IA gratuit.",
            ShortDescriptionEn = "AI coding assistant.",
            FullDescription = "Codeium est un outil d'autocomplétion de code alimenté par l'IA, gratuit pour les développeurs individuels.",
            FullDescriptionEn = "Codeium provides AI-powered code completion and chat for developers.",
            DisableImpact = "L'autocomplétion IA dans les IDE ne fonctionnera pas.",
            DisableImpactEn = "Background update service won't run.",
            PerformanceImpact = "Faible.",
            PerformanceImpactEn = "Low.",
            Recommendation = "Peut être désactivé si non utilisé.",
            RecommendationEn = "Can be disabled from startup.",
            Tags = "codeium,windsurf,code,ia,ai,autocompletion",
            LastUpdated = DateTime.Now
        });
    }

    /// <summary>
    /// Seeds additional browsers.
    /// </summary>
    private void SeedAdditionalBrowsers()
    {
        Save(new KnowledgeEntry
        {
            Name = "Vivaldi",
            Aliases = "Vivaldi Browser",
            Publisher = "Vivaldi Technologies AS",
            ExecutableNames = "vivaldi.exe",
            Category = KnowledgeCategory.Browser,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Navigateur personnalisable basé sur Chromium.",
            ShortDescriptionEn = "Customizable web browser.",
            FullDescription = "Vivaldi est un navigateur hautement personnalisable basé sur Chromium, créé par les anciens fondateurs d'Opera. Offre de nombreuses fonctionnalités intégrées.",
            FullDescriptionEn = "Vivaldi is a feature-rich, highly customizable web browser.",
            DisableImpact = "Vivaldi ne s'ouvrira pas au démarrage.",
            DisableImpactEn = "Update notifier won't run.",
            PerformanceImpact = "Modéré si lancé au démarrage.",
            PerformanceImpactEn = "Low.",
            Recommendation = "Peut être désactivé si non utilisé comme navigateur principal.",
            RecommendationEn = "Disable from startup.",
            Tags = "vivaldi,browser,chromium,navigateur",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "Arc",
            Aliases = "Arc Browser",
            Publisher = "The Browser Company",
            ExecutableNames = "Arc.exe",
            Category = KnowledgeCategory.Browser,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Navigateur moderne avec interface innovante.",
            ShortDescriptionEn = "Modern web browser.",
            FullDescription = "Arc est un navigateur repensé avec une interface utilisateur innovante, des espaces de travail et des fonctionnalités IA intégrées.",
            FullDescriptionEn = "Arc is a browser designed for better organization and focus, based on Chromium.",
            DisableImpact = "Arc ne s'ouvrira pas au démarrage.",
            DisableImpactEn = "Arc update and background processes won't run.",
            PerformanceImpact = "Modéré.",
            PerformanceImpactEn = "Low.",
            Recommendation = "Peut être désactivé. Lancez manuellement quand nécessaire.",
            RecommendationEn = "Can be disabled from startup.",
            Tags = "arc,browser,navigateur,moderne",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "Waterfox",
            Aliases = "Waterfox Browser",
            Publisher = "WaterfoxLimited",
            ExecutableNames = "waterfox.exe",
            Category = KnowledgeCategory.Browser,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Navigateur Firefox axé sur la vie privée.",
            ShortDescriptionEn = "Privacy-focused browser.",
            FullDescription = "Waterfox est un navigateur basé sur Firefox avec un focus sur la vie privée et la suppression de la télémétrie.",
            FullDescriptionEn = "Firefox fork focused on privacy and speed.",
            DisableImpact = "Waterfox ne s'ouvrira pas au démarrage.",
            DisableImpactEn = "No impact.",
            PerformanceImpact = "Modéré.",
            PerformanceImpactEn = "None.",
            Recommendation = "Peut être désactivé.",
            RecommendationEn = "Should not be in startup.",
            Tags = "waterfox,browser,firefox,privacy,navigateur",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "Tor Browser",
            Aliases = "Tor,TorBrowser",
            Publisher = "The Tor Project",
            ExecutableNames = "firefox.exe,tor.exe",
            Category = KnowledgeCategory.Browser,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Navigateur pour navigation anonyme via le réseau Tor.",
            ShortDescriptionEn = "Anonymous web browser.",
            FullDescription = "Tor Browser est un navigateur basé sur Firefox qui route le trafic via le réseau Tor pour l'anonymat. Ne devrait généralement pas démarrer automatiquement.",
            FullDescriptionEn = "Web browser that anonymizes your web traffic using the Tor network.",
            DisableImpact = "N'affecte pas la navigation normale.",
            DisableImpactEn = "No impact.",
            PerformanceImpact = "Aucun si désactivé.",
            PerformanceImpactEn = "None.",
            Recommendation = "Ne devrait pas être au démarrage. Vérifiez si c'est intentionnel.",
            RecommendationEn = "Should not be in startup.",
            Tags = "tor,browser,privacy,anonyme,navigateur",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "Zen Browser",
            Aliases = "Zen",
            Publisher = "Zen Browser",
            ExecutableNames = "zen.exe",
            Category = KnowledgeCategory.Browser,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Navigateur Firefox avec interface verticale.",
            ShortDescriptionEn = "Web browser.",
            FullDescription = "Zen est un navigateur basé sur Firefox avec une interface à onglets verticaux et un design moderne et minimaliste.",
            FullDescriptionEn = "Web browser focused on tranquility and focus.",
            DisableImpact = "Zen ne s'ouvrira pas au démarrage.",
            DisableImpactEn = "No impact.",
            PerformanceImpact = "Modéré.",
            PerformanceImpactEn = "None.",
            Recommendation = "Peut être désactivé.",
            RecommendationEn = "Should not be in startup.",
            Tags = "zen,browser,firefox,navigateur,onglets",
            LastUpdated = DateTime.Now
        });
    }

    /// <summary>
    /// Seeds development tools.
    /// </summary>
    private void SeedDevelopmentTools()
    {
        Save(new KnowledgeEntry
        {
            Name = "Cursor",
            Aliases = "Cursor AI,Cursor IDE",
            Publisher = "Anysphere, Inc.",
            ExecutableNames = "Cursor.exe",
            Category = KnowledgeCategory.Productivity,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Éditeur de code avec IA intégrée basé sur VS Code.",
            ShortDescriptionEn = "AI-powered code editor based on VS Code.",
            FullDescription = "Cursor est un éditeur de code basé sur VS Code avec des fonctionnalités d'IA avancées pour l'autocomplétion et la génération de code.",
            FullDescriptionEn = "Cursor is a VS Code-based code editor with advanced AI features for code completion and generation.",
            DisableImpact = "Cursor ne s'ouvrira pas au démarrage.",
            DisableImpactEn = "Cursor will not open at startup.",
            PerformanceImpact = "Faible au démarrage.",
            PerformanceImpactEn = "Low at startup.",
            Recommendation = "N'a pas besoin de démarrer avec Windows.",
            RecommendationEn = "Does not need to start with Windows.",
            Tags = "cursor,ide,code,ia,ai,vscode,developpement",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "JetBrains Toolbox",
            Aliases = "Toolbox App",
            Publisher = "JetBrains s.r.o.",
            ExecutableNames = "jetbrains-toolbox.exe",
            Category = KnowledgeCategory.Productivity,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Gestionnaire des IDE JetBrains.",
            ShortDescriptionEn = "JetBrains IDE manager.",
            FullDescription = "JetBrains Toolbox gère l'installation et les mises à jour de tous les IDE JetBrains (IntelliJ, PyCharm, WebStorm, etc.).",
            FullDescriptionEn = "JetBrains Toolbox manages installation and updates for all JetBrains IDEs (IntelliJ, PyCharm, WebStorm, etc.).",
            DisableImpact = "Pas de mises à jour automatiques des IDE JetBrains.",
            DisableImpactEn = "No automatic updates for JetBrains IDEs.",
            PerformanceImpact = "Faible.",
            PerformanceImpactEn = "Low.",
            Recommendation = "Peut être désactivé. Les mises à jour se feront manuellement.",
            RecommendationEn = "Can be disabled. Updates will be done manually.",
            Tags = "jetbrains,toolbox,ide,intellij,pycharm,developpement",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "Sublime Text",
            Aliases = "SublimeText",
            Publisher = "Sublime HQ Pty Ltd",
            ExecutableNames = "sublime_text.exe,subl.exe",
            Category = KnowledgeCategory.Productivity,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Éditeur de texte rapide et léger.",
            ShortDescriptionEn = "Text editor.",
            FullDescription = "Sublime Text est un éditeur de texte sophistiqué pour le code et le texte, connu pour sa rapidité et son interface élégante.",
            FullDescriptionEn = "Sophisticated text editor for code, markup and prose.",
            DisableImpact = "Sublime Text ne s'ouvrira pas au démarrage.",
            DisableImpactEn = "Update check won't run.",
            PerformanceImpact = "Très faible.",
            PerformanceImpactEn = "Very low.",
            Recommendation = "N'a pas besoin de démarrer avec Windows.",
            RecommendationEn = "Disable from startup.",
            Tags = "sublime,text,editor,code,developpement",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "Notepad++",
            Aliases = "NotepadPlusPlus,npp",
            Publisher = "Don Ho",
            ExecutableNames = "notepad++.exe",
            Category = KnowledgeCategory.Productivity,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Éditeur de texte open source pour Windows.",
            ShortDescriptionEn = "Source code editor.",
            FullDescription = "Notepad++ est un éditeur de texte et de code source gratuit et open source. Léger et rapide avec support de nombreux langages.",
            FullDescriptionEn = "Free source code editor and Notepad replacement.",
            DisableImpact = "Notepad++ ne s'ouvrira pas au démarrage.",
            DisableImpactEn = "Auto-updater won't run.",
            PerformanceImpact = "Très faible.",
            PerformanceImpactEn = "Very low.",
            Recommendation = "N'a pas besoin de démarrer avec Windows.",
            RecommendationEn = "Disable from startup.",
            Tags = "notepad++,editor,text,code,opensource",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "GitHub Desktop",
            Aliases = "GitHubDesktop",
            Publisher = "GitHub, Inc.",
            ExecutableNames = "GitHubDesktop.exe",
            Category = KnowledgeCategory.Productivity,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Client Git graphique de GitHub.",
            ShortDescriptionEn = "GitHub GUI client.",
            FullDescription = "GitHub Desktop est une application graphique pour gérer les dépôts Git et GitHub sans ligne de commande.",
            FullDescriptionEn = "Graphical interface for managing GitHub repositories.",
            DisableImpact = "GitHub Desktop ne s'ouvrira pas au démarrage.",
            DisableImpactEn = "No impact.",
            PerformanceImpact = "Faible.",
            PerformanceImpactEn = "None.",
            Recommendation = "N'a pas besoin de démarrer avec Windows. Lancez manuellement.",
            RecommendationEn = "Should not be in startup.",
            Tags = "github,git,desktop,versioncontrol",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "GitKraken",
            Aliases = "GitKraken Client",
            Publisher = "Axosoft, LLC",
            ExecutableNames = "gitkraken.exe",
            Category = KnowledgeCategory.Productivity,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Client Git graphique multiplateforme.",
            ShortDescriptionEn = "Git GUI client.",
            FullDescription = "GitKraken est un client Git graphique puissant avec visualisation des branches, intégrations et fonctionnalités de collaboration.",
            FullDescriptionEn = "Visual Git client with graph view and integrations.",
            DisableImpact = "GitKraken ne s'ouvrira pas au démarrage.",
            DisableImpactEn = "Background updates won't run.",
            PerformanceImpact = "Modéré.",
            PerformanceImpactEn = "Low.",
            Recommendation = "N'a pas besoin de démarrer avec Windows.",
            RecommendationEn = "Disable from startup.",
            Tags = "gitkraken,git,client,versioncontrol",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "Sourcetree",
            Aliases = "Atlassian Sourcetree",
            Publisher = "Atlassian",
            ExecutableNames = "SourceTree.exe",
            Category = KnowledgeCategory.Productivity,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Client Git et Mercurial gratuit d'Atlassian.",
            ShortDescriptionEn = "Git GUI.",
            FullDescription = "Sourcetree est un client Git et Mercurial gratuit avec interface graphique pour visualiser et gérer les dépôts.",
            FullDescriptionEn = "Free Git client for Windows.",
            DisableImpact = "Sourcetree ne s'ouvrira pas au démarrage.",
            DisableImpactEn = "No impact.",
            PerformanceImpact = "Faible.",
            PerformanceImpactEn = "None.",
            Recommendation = "N'a pas besoin de démarrer avec Windows.",
            RecommendationEn = "Should not be in startup.",
            Tags = "sourcetree,git,atlassian,mercurial",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "Postman",
            Aliases = "Postman App",
            Publisher = "Postman, Inc.",
            ExecutableNames = "Postman.exe",
            Category = KnowledgeCategory.Productivity,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Plateforme de développement et test d'API.",
            ShortDescriptionEn = "API platform.",
            FullDescription = "Postman est une plateforme collaborative pour le développement d'API permettant de créer, tester et documenter les APIs.",
            FullDescriptionEn = "Platform for building and using APIs.",
            DisableImpact = "Postman ne s'ouvrira pas au démarrage.",
            DisableImpactEn = "Update check/agent won't run.",
            PerformanceImpact = "Modéré.",
            PerformanceImpactEn = "Low.",
            Recommendation = "N'a pas besoin de démarrer avec Windows.",
            RecommendationEn = "Disable from startup.",
            Tags = "postman,api,developpement,test,rest",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "Insomnia",
            Aliases = "Insomnia REST Client",
            Publisher = "Kong Inc.",
            ExecutableNames = "Insomnia.exe",
            Category = KnowledgeCategory.Productivity,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Client API REST/GraphQL open source.",
            ShortDescriptionEn = "API design tool.",
            FullDescription = "Insomnia est un client API open source pour REST, GraphQL et gRPC avec une interface moderne.",
            FullDescriptionEn = "Platform for API design, debugging, and testing.",
            DisableImpact = "Insomnia ne s'ouvrira pas au démarrage.",
            DisableImpactEn = "Updates won't run automatically.",
            PerformanceImpact = "Faible.",
            PerformanceImpactEn = "Low.",
            Recommendation = "N'a pas besoin de démarrer avec Windows.",
            RecommendationEn = "Disable from startup.",
            Tags = "insomnia,api,rest,graphql,developpement",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "WSL",
            Aliases = "Windows Subsystem for Linux,wsl.exe",
            Publisher = "Microsoft Corporation",
            ExecutableNames = "wsl.exe,wslhost.exe",
            Category = KnowledgeCategory.Utility,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Sous-système Windows pour Linux.",
            ShortDescriptionEn = "Windows Subsystem for Linux.",
            FullDescription = "WSL permet d'exécuter un environnement Linux directement sur Windows sans machine virtuelle traditionnelle.",
            FullDescriptionEn = "Compatibility layer for running Linux binaries natively on Windows.",
            DisableImpact = "Les distributions Linux WSL ne démarreront pas automatiquement.",
            DisableImpactEn = "Linux services won't run in background.",
            PerformanceImpact = "Variable selon l'utilisation.",
            PerformanceImpactEn = "Varies.",
            Recommendation = "Généralement n'a pas besoin de démarrer automatiquement.",
            RecommendationEn = "Keep enabled if you use WSL services.",
            Tags = "wsl,linux,windows,developpement,microsoft",
            LastUpdated = DateTime.Now
        });
    }

    /// <summary>
    /// Seeds productivity applications.
    /// </summary>
    private void SeedProductivityApps()
    {
        Save(new KnowledgeEntry
        {
            Name = "Grammarly",
            Aliases = "Grammarly Desktop,Grammarly for Windows",
            Publisher = "Grammarly, Inc.",
            ExecutableNames = "Grammarly.Desktop.exe,Grammarly.exe",
            Category = KnowledgeCategory.Productivity,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Assistant d'écriture et correcteur grammatical IA.",
            ShortDescriptionEn = "Writing assistant.",
            FullDescription = "Grammarly est un assistant d'écriture qui vérifie la grammaire, l'orthographe et le style dans toutes les applications.",
            FullDescriptionEn = "Grammarly helps check spelling, grammar, and tone in desktop applications.",
            DisableImpact = "Pas de corrections Grammarly en temps réel.",
            DisableImpactEn = "Writing suggestions won't appear.",
            PerformanceImpact = "Faible à modéré.",
            PerformanceImpactEn = "Low to moderate.",
            Recommendation = "Peut être désactivé si utilisé principalement via extension navigateur.",
            RecommendationEn = "Keep enabled if you write frequently.",
            Tags = "grammarly,writing,grammar,orthographe,ia",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "Todoist",
            Aliases = "Todoist Desktop",
            Publisher = "Doist Inc.",
            ExecutableNames = "Todoist.exe",
            Category = KnowledgeCategory.Productivity,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Application de gestion de tâches et productivité.",
            ShortDescriptionEn = "Task management application.",
            FullDescription = "Todoist est un gestionnaire de tâches multiplateforme avec projets, étiquettes, filtres et rappels.",
            FullDescriptionEn = "Todoist is a task and to-do list management application with multi-device synchronization and reminders.",
            DisableImpact = "Pas de rappels ni accès rapide au démarrage.",
            DisableImpactEn = "Task reminders won't be displayed at startup.",
            PerformanceImpact = "Faible.",
            PerformanceImpactEn = "Low.",
            Recommendation = "Peut être désactivé. Utilisez l'app web ou mobile.",
            RecommendationEn = "Keep enabled if you depend on reminders.",
            Tags = "todoist,tasks,productivity,todo,gtd",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "ClickUp",
            Aliases = "ClickUp Desktop",
            Publisher = "ClickUp",
            ExecutableNames = "ClickUp.exe",
            Category = KnowledgeCategory.Productivity,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Plateforme de productivité et gestion de projet.",
            ShortDescriptionEn = "Productivity platform.",
            FullDescription = "ClickUp est une plateforme tout-en-un pour la gestion de projets, tâches, documents, objectifs et temps.",
            FullDescriptionEn = "All-in-one productivity tool for tasks, docs, chat, goals, and more.",
            DisableImpact = "Pas de notifications desktop ClickUp.",
            DisableImpactEn = "Notifications won't appear at startup.",
            PerformanceImpact = "Modéré.",
            PerformanceImpactEn = "Moderate.",
            Recommendation = "Peut être désactivé. L'app web est fonctionnellement équivalente.",
            RecommendationEn = "Disable if not used heavily.",
            Tags = "clickup,productivity,project,management",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "Linear",
            Aliases = "Linear App",
            Publisher = "Linear Orbit, Inc.",
            ExecutableNames = "Linear.exe",
            Category = KnowledgeCategory.Productivity,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Outil de suivi des issues pour équipes produit.",
            ShortDescriptionEn = "Issue tracking tool.",
            FullDescription = "Linear est un outil de suivi des issues moderne et rapide, populaire auprès des équipes de développement produit.",
            FullDescriptionEn = "Project and issue tracking tool for software teams.",
            DisableImpact = "Pas de notifications Linear au démarrage.",
            DisableImpactEn = "Notifications won't appear.",
            PerformanceImpact = "Faible.",
            PerformanceImpactEn = "Low.",
            Recommendation = "Peut être désactivé. Lancez manuellement ou utilisez le web.",
            RecommendationEn = "Disable if you check it manually.",
            Tags = "linear,issues,tracking,product,agile",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "Figma",
            Aliases = "Figma Desktop",
            Publisher = "Figma, Inc.",
            ExecutableNames = "Figma.exe",
            Category = KnowledgeCategory.Productivity,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Outil de design collaboratif.",
            ShortDescriptionEn = "Interface design tool.",
            FullDescription = "Figma est un outil de design d'interface utilisateur collaboratif basé sur le cloud avec édition en temps réel.",
            FullDescriptionEn = "Collaborative interface design tool. Desktop app wraps the web experience.",
            DisableImpact = "Figma ne s'ouvrira pas au démarrage.",
            DisableImpactEn = "Font helper/updates won't run.",
            PerformanceImpact = "Modéré.",
            PerformanceImpactEn = "Low.",
            Recommendation = "N'a pas besoin de démarrer avec Windows. L'app web est identique.",
            RecommendationEn = "Disable from startup.",
            Tags = "figma,design,ui,ux,collaborative",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "Loom",
            Aliases = "Loom Desktop",
            Publisher = "Loom, Inc.",
            ExecutableNames = "Loom.exe",
            Category = KnowledgeCategory.Productivity,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Outil d'enregistrement vidéo et partage d'écran.",
            ShortDescriptionEn = "Screen recording tool.",
            FullDescription = "Loom permet d'enregistrer rapidement des vidéos de son écran avec webcam pour partager des messages asynchrones.",
            FullDescriptionEn = "Video messaging tool to record screen, camera, and microphone.",
            DisableImpact = "Pas d'accès rapide pour l'enregistrement Loom.",
            DisableImpactEn = "Quick record shortcut won't work.",
            PerformanceImpact = "Faible au repos.",
            PerformanceImpactEn = "Low.",
            Recommendation = "Peut être désactivé si utilisé occasionnellement.",
            RecommendationEn = "Disable if you don't record often.",
            Tags = "loom,video,screen,recording,async",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "Raycast",
            Aliases = "Raycast for Windows",
            Publisher = "Raycast Technologies Inc.",
            ExecutableNames = "Raycast.exe",
            Category = KnowledgeCategory.Utility,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Lanceur de productivité extensible.",
            ShortDescriptionEn = "Productivity launcher.",
            FullDescription = "Raycast est un lanceur de productivité extensible qui remplace Spotlight/PowerToys Run avec des extensions communautaires.",
            FullDescriptionEn = "Extensible launcher to control your tools, usually on macOS but relevant if ported/similar.",
            DisableImpact = "Raycast ne sera pas disponible via raccourci.",
            DisableImpactEn = "Launcher shortcuts won't work.",
            PerformanceImpact = "Faible.",
            PerformanceImpactEn = "Low.",
            Recommendation = "Doit rester actif si utilisé comme lanceur principal.",
            RecommendationEn = "Keep enabled for productivity.",
            Tags = "raycast,launcher,productivity,spotlight",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "Flow Launcher",
            Aliases = "FlowLauncher",
            Publisher = "Flow Launcher Team",
            ExecutableNames = "Flow.Launcher.exe",
            Category = KnowledgeCategory.Utility,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Lanceur de productivité open source pour Windows.",
            ShortDescriptionEn = "Quick file and app launcher.",
            FullDescription = "Flow Launcher est un lanceur d'applications rapide et extensible pour Windows, alternative open source à Alfred/Raycast.",
            FullDescriptionEn = "Flow Launcher is a productivity tool to search files, apps, and perform system actions quickly.",
            DisableImpact = "Flow Launcher ne sera pas disponible via raccourci.",
            DisableImpactEn = "Search bar won't be available via shortcut.",
            PerformanceImpact = "Très faible.",
            PerformanceImpactEn = "Low.",
            Recommendation = "Doit rester actif si utilisé comme lanceur principal.",
            RecommendationEn = "Keep enabled for productivity.",
            Tags = "flow,launcher,productivity,search,windows",
            LastUpdated = DateTime.Now
        });
    }

    /// <summary>
    /// Seeds media players and creative applications.
    /// </summary>
    private void SeedMediaAndCreative()
    {
        Save(new KnowledgeEntry
        {
            Name = "Plex",
            Aliases = "Plex Media Player,Plex HTPC",
            Publisher = "Plex, Inc.",
            ExecutableNames = "Plex.exe,Plex HTPC.exe",
            Category = KnowledgeCategory.Media,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Client media center Plex.",
            ShortDescriptionEn = "Media player app.",
            FullDescription = "Plex est un client pour le système de media center Plex, permettant de streamer films, séries et musique depuis un serveur Plex.",
            FullDescriptionEn = "Client application for Plex Media Server.",
            DisableImpact = "Plex ne s'ouvrira pas au démarrage.",
            DisableImpactEn = "App won't start automatically.",
            PerformanceImpact = "Faible au repos.",
            PerformanceImpactEn = "Low.",
            Recommendation = "Peut être désactivé sur les postes non-HTPC.",
            RecommendationEn = "Disable from startup.",
            Tags = "plex,media,streaming,htpc,films",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "Plex Media Server",
            Aliases = "PlexMediaServer",
            Publisher = "Plex, Inc.",
            ExecutableNames = "Plex Media Server.exe,PlexMediaServer.exe",
            Category = KnowledgeCategory.Media,
            SafetyLevel = SafetyLevel.Important,
            ShortDescription = "Serveur média Plex.",
            ShortDescriptionEn = "Personal media server.",
            FullDescription = "Plex Media Server organise et streame votre bibliothèque multimédia vers tous les appareils. Doit tourner pour que Plex fonctionne.",
            FullDescriptionEn = "Organizes your video, music, and photo collections and streams them to all your devices.",
            DisableImpact = "Votre bibliothèque Plex ne sera pas accessible.",
            DisableImpactEn = "Media won't be accessible to other devices.",
            PerformanceImpact = "Modéré à élevé lors du transcodage.",
            PerformanceImpactEn = "Low idle, high streaming.",
            Recommendation = "Gardez actif si vous utilisez Plex comme serveur.",
            RecommendationEn = "Keep enabled if used as a server.",
            Tags = "plex,server,media,streaming,transcoding",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "foobar2000",
            Aliases = "foobar",
            Publisher = "Peter Pawlowski",
            ExecutableNames = "foobar2000.exe",
            Category = KnowledgeCategory.Media,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Lecteur audio léger et personnalisable.",
            ShortDescriptionEn = "Advanced audio player.",
            FullDescription = "foobar2000 est un lecteur audio gratuit pour Windows, réputé pour sa légèreté, sa qualité sonore et sa personnalisation.",
            FullDescriptionEn = "Freeware audio player with modular design and rich configuration.",
            DisableImpact = "foobar2000 ne s'ouvrira pas au démarrage.",
            DisableImpactEn = "No impact.",
            PerformanceImpact = "Très faible.",
            PerformanceImpactEn = "None.",
            Recommendation = "N'a pas besoin de démarrer avec Windows.",
            RecommendationEn = "Should not be in startup.",
            Tags = "foobar2000,audio,music,player,flac",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "AIMP",
            Aliases = "AIMP Player",
            Publisher = "AIMP DevTeam",
            ExecutableNames = "AIMP.exe",
            Category = KnowledgeCategory.Media,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Lecteur audio russe gratuit.",
            ShortDescriptionEn = "Versatile audio player.",
            FullDescription = "AIMP est un lecteur audio gratuit avec une interface élégante, support de nombreux formats et fonctionnalités avancées.",
            FullDescriptionEn = "AIMP is a lightweight and powerful music player with support for many formats and playlists.",
            DisableImpact = "AIMP ne s'ouvrira pas au démarrage.",
            DisableImpactEn = "AIMP agent won't start.",
            PerformanceImpact = "Très faible.",
            PerformanceImpactEn = "Low.",
            Recommendation = "N'a pas besoin de démarrer avec Windows.",
            RecommendationEn = "Can be disabled from startup.",
            Tags = "aimp,audio,music,player",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "MusicBee",
            Aliases = "Music Bee",
            Publisher = "Steven Mayall",
            ExecutableNames = "MusicBee.exe",
            Category = KnowledgeCategory.Media,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Gestionnaire et lecteur de musique.",
            ShortDescriptionEn = "Music manager and player.",
            FullDescription = "MusicBee est un gestionnaire de musique puissant avec bibliothèque, podcasts, radio et synchronisation d'appareils.",
            FullDescriptionEn = "Music player to manage large collections with broad format support.",
            DisableImpact = "MusicBee ne s'ouvrira pas au démarrage.",
            DisableImpactEn = "Player won't start automatically.",
            PerformanceImpact = "Faible.",
            PerformanceImpactEn = "Low.",
            Recommendation = "N'a pas besoin de démarrer avec Windows.",
            RecommendationEn = "Disable from startup.",
            Tags = "musicbee,audio,music,library,player",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "PotPlayer",
            Aliases = "Daum PotPlayer",
            Publisher = "Kakao Corp.",
            ExecutableNames = "PotPlayer.exe,PotPlayerMini64.exe",
            Category = KnowledgeCategory.Media,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Lecteur multimédia puissant et léger.",
            ShortDescriptionEn = "Multimedia player.",
            FullDescription = "PotPlayer est un lecteur multimédia coréen gratuit supportant de nombreux formats et codecs avec une interface personnalisable.",
            FullDescriptionEn = "Multimedia player with vast codecs support.",
            DisableImpact = "PotPlayer ne s'ouvrira pas au démarrage.",
            DisableImpactEn = "No impact.",
            PerformanceImpact = "Très faible.",
            PerformanceImpactEn = "None.",
            Recommendation = "N'a pas besoin de démarrer avec Windows.",
            RecommendationEn = "Should not be in startup.",
            Tags = "potplayer,video,audio,player,codec",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "Kodi",
            Aliases = "XBMC",
            Publisher = "XBMC Foundation",
            ExecutableNames = "Kodi.exe",
            Category = KnowledgeCategory.Media,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Centre multimédia open source.",
            ShortDescriptionEn = "Media center.",
            FullDescription = "Kodi est un centre multimédia open source pour organiser et lire films, séries, musique, photos et plus.",
            FullDescriptionEn = "Free and open-source home theater software.",
            DisableImpact = "Kodi ne s'ouvrira pas au démarrage.",
            DisableImpactEn = "Media center won't start automatically.",
            PerformanceImpact = "Faible au repos.",
            PerformanceImpactEn = "Low when idle.",
            Recommendation = "Utile au démarrage uniquement sur les HTPC dédiés.",
            RecommendationEn = "Disable unless dedicated media PC.",
            Tags = "kodi,xbmc,media,htpc,center",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "Kindle",
            Aliases = "Kindle for PC,Amazon Kindle",
            Publisher = "Amazon",
            ExecutableNames = "Kindle.exe",
            Category = KnowledgeCategory.Media,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Application de lecture Kindle d'Amazon.",
            ShortDescriptionEn = "Amazon Kindle reader.",
            FullDescription = "Kindle for PC permet de lire les ebooks achetés sur Amazon et synchronise la progression avec les autres appareils.",
            FullDescriptionEn = "Desktop app to read Kindle books.",
            DisableImpact = "Kindle ne s'ouvrira pas au démarrage.",
            DisableImpactEn = "App won't start automatically.",
            PerformanceImpact = "Faible.",
            PerformanceImpactEn = "Low.",
            Recommendation = "N'a pas besoin de démarrer avec Windows.",
            RecommendationEn = "Disable from startup.",
            Tags = "kindle,amazon,ebook,reading",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "calibre",
            Aliases = "calibre ebook",
            Publisher = "Kovid Goyal",
            ExecutableNames = "calibre.exe,calibre-parallel.exe",
            Category = KnowledgeCategory.Media,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Gestionnaire et convertisseur d'ebooks.",
            ShortDescriptionEn = "E-book management.",
            FullDescription = "calibre est un gestionnaire d'ebooks gratuit et open source pour organiser, convertir et transférer des ebooks.",
            FullDescriptionEn = "E-book manager to view, convert, and catalog e-books.",
            DisableImpact = "calibre ne s'ouvrira pas au démarrage.",
            DisableImpactEn = "Device detection/server won't run.",
            PerformanceImpact = "Faible.",
            PerformanceImpactEn = "Low.",
            Recommendation = "N'a pas besoin de démarrer avec Windows.",
            RecommendationEn = "Disable from startup.",
            Tags = "calibre,ebook,library,converter",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "DaVinci Resolve",
            Aliases = "DaVinci Resolve Studio,Blackmagic Design",
            Publisher = "Blackmagic Design",
            ExecutableNames = "Resolve.exe",
            Category = KnowledgeCategory.Media,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Suite de montage vidéo professionnelle.",
            ShortDescriptionEn = "Professional video editing.",
            FullDescription = "DaVinci Resolve est une suite professionnelle de montage vidéo, étalonnage, effets visuels et post-production audio.",
            FullDescriptionEn = "DaVinci Resolve combines editing, color correction, visual effects, motion graphics and audio post production.",
            DisableImpact = "DaVinci Resolve ne s'ouvrira pas au démarrage.",
            DisableImpactEn = "Database server/panels won't connect automatically.",
            PerformanceImpact = "Très faible au repos.",
            PerformanceImpactEn = "Low when idle.",
            Recommendation = "N'a pas besoin de démarrer avec Windows.",
            RecommendationEn = "Disable from startup unless used daily.",
            Tags = "davinci,resolve,video,editing,color",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "HandBrake",
            Aliases = "HandBrake Video Transcoder",
            Publisher = "HandBrake Team",
            ExecutableNames = "HandBrake.exe",
            Category = KnowledgeCategory.Media,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Convertisseur vidéo open source.",
            ShortDescriptionEn = "Video transcoder.",
            FullDescription = "HandBrake est un transcodeur vidéo open source multiplateforme pour convertir des vidéos en formats modernes.",
            FullDescriptionEn = "Open-source tool for converting video from nearly any format.",
            DisableImpact = "HandBrake ne s'ouvrira pas au démarrage.",
            DisableImpactEn = "No impact.",
            PerformanceImpact = "Aucun au repos.",
            PerformanceImpactEn = "None.",
            Recommendation = "N'a pas besoin de démarrer avec Windows.",
            RecommendationEn = "Should not be in startup.",
            Tags = "handbrake,video,transcoding,converter",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "GIMP",
            Aliases = "GNU Image Manipulation Program",
            Publisher = "GIMP Team",
            ExecutableNames = "gimp.exe,gimp-2.10.exe",
            Category = KnowledgeCategory.Media,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Éditeur d'images open source.",
            ShortDescriptionEn = "Image editor.",
            FullDescription = "GIMP est un éditeur d'images libre et gratuit, alternative open source à Photoshop.",
            FullDescriptionEn = "GNU Image Manipulation Program, a free and open-source image editor.",
            DisableImpact = "GIMP ne s'ouvrira pas au démarrage.",
            DisableImpactEn = "No impact.",
            PerformanceImpact = "Aucun au repos.",
            PerformanceImpactEn = "None.",
            Recommendation = "N'a pas besoin de démarrer avec Windows.",
            RecommendationEn = "Should not be in startup.",
            Tags = "gimp,image,editor,photo,opensource",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "Inkscape",
            Aliases = "Inkscape Vector Graphics",
            Publisher = "Inkscape Project",
            ExecutableNames = "inkscape.exe",
            Category = KnowledgeCategory.Media,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Éditeur de graphiques vectoriels open source.",
            ShortDescriptionEn = "Vector graphics editor.",
            FullDescription = "Inkscape est un éditeur de graphiques vectoriels libre, alternative open source à Illustrator.",
            FullDescriptionEn = "Free and open-source vector graphics editor.",
            DisableImpact = "Inkscape ne s'ouvrira pas au démarrage.",
            DisableImpactEn = "No impact.",
            PerformanceImpact = "Aucun au repos.",
            PerformanceImpactEn = "None.",
            Recommendation = "N'a pas besoin de démarrer avec Windows.",
            RecommendationEn = "Should not be in startup.",
            Tags = "inkscape,vector,svg,graphics,opensource",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "Krita",
            Aliases = "Krita Painting",
            Publisher = "Krita Foundation",
            ExecutableNames = "krita.exe",
            Category = KnowledgeCategory.Media,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Application de peinture numérique open source.",
            ShortDescriptionEn = "Digital painting app.",
            FullDescription = "Krita est une application de peinture numérique gratuite et open source pour illustrateurs, artistes concept et peintres.",
            FullDescriptionEn = "Professional free and open-source painting program.",
            DisableImpact = "Krita ne s'ouvrira pas au démarrage.",
            DisableImpactEn = "No impact.",
            PerformanceImpact = "Aucun au repos.",
            PerformanceImpactEn = "None.",
            Recommendation = "N'a pas besoin de démarrer avec Windows.",
            RecommendationEn = "Should not be in startup.",
            Tags = "krita,painting,digital,art,opensource",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "Blender",
            Aliases = "Blender 3D",
            Publisher = "Blender Foundation",
            ExecutableNames = "blender.exe,blender-launcher.exe",
            Category = KnowledgeCategory.Media,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Suite de création 3D open source.",
            ShortDescriptionEn = "3D creation suite.",
            FullDescription = "Blender est une suite de création 3D open source pour modélisation, animation, rendu, compositing et montage vidéo.",
            FullDescriptionEn = "Blender is an open-source 3D creation suite supporting modeling, rigging, animation, simulation, rendering, and compositing.",
            DisableImpact = "Blender ne s'ouvrira pas au démarrage.",
            DisableImpactEn = "No impact on startup.",
            PerformanceImpact = "Aucun au repos.",
            PerformanceImpactEn = "None.",
            Recommendation = "N'a pas besoin de démarrer avec Windows.",
            RecommendationEn = "Should not be in automatic startup.",
            Tags = "blender,3d,modeling,animation,opensource",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "Unity Hub",
            Aliases = "Unity Editor",
            Publisher = "Unity Technologies",
            ExecutableNames = "Unity Hub.exe,Unity.exe",
            Category = KnowledgeCategory.Productivity,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Gestionnaire et lanceur Unity Engine.",
            ShortDescriptionEn = "Unity project manager.",
            FullDescription = "Unity Hub gère les installations de l'éditeur Unity, les projets et les licences pour le développement de jeux et applications.",
            FullDescriptionEn = "Management tool for Unity projects and installations.",
            DisableImpact = "Unity Hub ne s'ouvrira pas au démarrage.",
            DisableImpactEn = "Hub won't open automatically.",
            PerformanceImpact = "Faible.",
            PerformanceImpactEn = "Low.",
            Recommendation = "N'a pas besoin de démarrer avec Windows.",
            RecommendationEn = "Disable from startup.",
            Tags = "unity,game,development,3d,engine",
            LastUpdated = DateTime.Now
        });
    }

    /// <summary>
    /// Seeds hardware monitoring and overclocking tools.
    /// </summary>
    private void SeedHardwareTools()
    {
        Save(new KnowledgeEntry
        {
            Name = "MSI Afterburner",
            Aliases = "Afterburner",
            Publisher = "MSI / Guru3D",
            ExecutableNames = "MSIAfterburner.exe",
            Category = KnowledgeCategory.Hardware,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Utilitaire d'overclocking et monitoring GPU.",
            ShortDescriptionEn = "GPU overclocking utility.",
            FullDescription = "MSI Afterburner est un utilitaire d'overclocking GPU populaire avec monitoring en temps réel, profils et statistiques en jeu.",
            FullDescriptionEn = "MSI Afterburner allows graphics card overclocking, monitoring and OSD display in games via RivaTuner.",
            DisableImpact = "Pas d'overclocking GPU ni de monitoring au démarrage.",
            DisableImpactEn = "Custom overclocking and OSD won't be active.",
            PerformanceImpact = "Faible.",
            PerformanceImpactEn = "Low.",
            Recommendation = "Gardez actif si vous utilisez l'overclocking ou le monitoring.",
            RecommendationEn = "Keep enabled if you use overclocking or OSD.",
            Tags = "msi,afterburner,gpu,overclocking,monitoring",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "NZXT CAM",
            Aliases = "CAM",
            Publisher = "NZXT, Inc.",
            ExecutableNames = "NZXT CAM.exe,CAM.exe",
            Category = KnowledgeCategory.Hardware,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Logiciel de monitoring et contrôle NZXT.",
            ShortDescriptionEn = "NZXT monitoring and control software.",
            FullDescription = "NZXT CAM surveille les performances du PC, contrôle l'éclairage RGB et les ventilateurs des produits NZXT.",
            FullDescriptionEn = "NZXT CAM controls NZXT products (cases, cooling, lighting) and offers detailed system monitoring.",
            DisableImpact = "Pas de contrôle RGB/ventilateurs NZXT, pas de monitoring.",
            DisableImpactEn = "NZXT product control and lighting won't be managed.",
            PerformanceImpact = "Faible à modéré.",
            PerformanceImpactEn = "Moderate.",
            Recommendation = "Gardez actif si vous avez du matériel NZXT ou voulez le monitoring.",
            RecommendationEn = "Keep enabled if you have NZXT products.",
            Tags = "nzxt,cam,monitoring,rgb,cooling",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "Voicemod",
            Aliases = "Voicemod Desktop",
            Publisher = "Voicemod S.L.",
            ExecutableNames = "Voicemod.exe,VoicemodDesktop.exe",
            Category = KnowledgeCategory.Media,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Modificateur de voix en temps réel.",
            ShortDescriptionEn = "Real-time voice changer.",
            FullDescription = "Voicemod est un modificateur de voix en temps réel pour Discord, Twitch et autres applications de communication.",
            FullDescriptionEn = "Voice changer and soundboard software for gamers and streamers.",
            DisableImpact = "Les effets de voix ne seront pas disponibles.",
            DisableImpactEn = "Voice effects won't be ready.",
            PerformanceImpact = "Modéré.",
            PerformanceImpactEn = "Moderate.",
            Recommendation = "Peut être désactivé si non utilisé régulièrement.",
            RecommendationEn = "Keep enabled if used frequently.",
            Tags = "voicemod,voice,changer,streaming,discord",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "Krisp",
            Aliases = "Krisp AI",
            Publisher = "Krisp Technologies, Inc.",
            ExecutableNames = "krisp.exe",
            Category = KnowledgeCategory.Productivity,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Suppression de bruit IA pour les appels.",
            ShortDescriptionEn = "Noise cancellation app.",
            FullDescription = "Krisp utilise l'IA pour supprimer le bruit de fond et les échos lors des appels vidéo et audio.",
            FullDescriptionEn = "AI-powered noise cancellation for microphone and speaker.",
            DisableImpact = "Pas de suppression de bruit automatique.",
            DisableImpactEn = "Noise cancellation won't be active.",
            PerformanceImpact = "Modéré (traitement IA).",
            PerformanceImpactEn = "Low.",
            Recommendation = "Utile si vous faites beaucoup d'appels. Sinon désactivable.",
            RecommendationEn = "Keep enabled if you do frequent calls.",
            Tags = "krisp,noise,cancellation,ai,calls",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "EarTrumpet",
            Aliases = "EarTrumpet Volume Control",
            Publisher = "File-New-Project",
            ExecutableNames = "EarTrumpet.exe",
            Category = KnowledgeCategory.Utility,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Contrôle de volume par application pour Windows.",
            ShortDescriptionEn = "Advanced volume control.",
            FullDescription = "EarTrumpet remplace l'icône de volume Windows avec un contrôle avancé du volume par application.",
            FullDescriptionEn = "EarTrumpet offers per-app volume control and better audio management for Windows.",
            DisableImpact = "Pas de contrôle de volume par app via EarTrumpet.",
            DisableImpactEn = "Advanced volume mixer won't be available.",
            PerformanceImpact = "Très faible.",
            PerformanceImpactEn = "Low.",
            Recommendation = "Gardez actif si vous l'utilisez pour le contrôle du volume.",
            RecommendationEn = "Keep enabled if you use it for audio control.",
            Tags = "eartrumpet,volume,audio,control,windows",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "Streamlabs",
            Aliases = "Streamlabs OBS,Streamlabs Desktop",
            Publisher = "Streamlabs",
            ExecutableNames = "Streamlabs OBS.exe,Streamlabs Desktop.exe",
            Category = KnowledgeCategory.Media,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Logiciel de streaming tout-en-un.",
            ShortDescriptionEn = "Streaming software.",
            FullDescription = "Streamlabs est une version enrichie d'OBS avec alertes, widgets et outils de streaming intégrés.",
            FullDescriptionEn = "Streamlabs Desktop is a free professional broadcasting software for live streaming.",
            DisableImpact = "Streamlabs ne s'ouvrira pas au démarrage.",
            DisableImpactEn = "Streaming features won't be ready.",
            PerformanceImpact = "Faible au repos.",
            PerformanceImpactEn = "Moderate.",
            Recommendation = "N'a pas besoin de démarrer avec Windows.",
            RecommendationEn = "Disable unless dedicated streaming PC.",
            Tags = "streamlabs,obs,streaming,twitch,youtube",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "XSplit",
            Aliases = "XSplit Broadcaster,XSplit VCam",
            Publisher = "SplitmediaLabs",
            ExecutableNames = "XSplit.Core.exe,XSplitBroadcaster.exe",
            Category = KnowledgeCategory.Media,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Logiciel de streaming et diffusion.",
            ShortDescriptionEn = "Streaming and recording software.",
            FullDescription = "XSplit est une suite de diffusion en direct incluant le streaming, la caméra virtuelle et l'enregistrement.",
            FullDescriptionEn = "Software for live streaming and video mixing.",
            DisableImpact = "XSplit ne s'ouvrira pas au démarrage.",
            DisableImpactEn = "Streaming features won't be ready.",
            PerformanceImpact = "Faible au repos.",
            PerformanceImpactEn = "Moderate.",
            Recommendation = "N'a pas besoin de démarrer avec Windows.",
            RecommendationEn = "Disable unless dedicated streaming PC.",
            Tags = "xsplit,streaming,broadcast,camera",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "GPU-Z",
            Aliases = "TechPowerUp GPU-Z",
            Publisher = "TechPowerUp",
            ExecutableNames = "GPU-Z.exe",
            Category = KnowledgeCategory.Hardware,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Utilitaire d'information GPU.",
            ShortDescriptionEn = "Graphics card information.",
            FullDescription = "GPU-Z affiche les informations détaillées sur la carte graphique, les capteurs et les fréquences.",
            FullDescriptionEn = "Lightweight utility to provide vital information about your video card and GPU.",
            DisableImpact = "GPU-Z ne s'ouvrira pas au démarrage.",
            DisableImpactEn = "No impact.",
            PerformanceImpact = "Très faible.",
            PerformanceImpactEn = "None.",
            Recommendation = "N'a pas besoin de démarrer avec Windows.",
            RecommendationEn = "Should not be in startup.",
            Tags = "gpu-z,gpu,monitoring,info,techpowerup",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "CPU-Z",
            Aliases = "CPUID CPU-Z",
            Publisher = "CPUID",
            ExecutableNames = "cpuz.exe,cpuz_x64.exe",
            Category = KnowledgeCategory.Hardware,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Utilitaire d'information CPU et système.",
            ShortDescriptionEn = "CPU information tool.",
            FullDescription = "CPU-Z affiche les informations détaillées sur le processeur, la carte mère, la mémoire et le cache.",
            FullDescriptionEn = "CPU-Z gathers information on some of the main devices of your system (CPU, Mainboard, Memory).",
            DisableImpact = "CPU-Z ne s'ouvrira pas au démarrage.",
            DisableImpactEn = "No impact, usually not a startup item.",
            PerformanceImpact = "Très faible.",
            PerformanceImpactEn = "None.",
            Recommendation = "N'a pas besoin de démarrer avec Windows.",
            RecommendationEn = "Should not be in startup.",
            Tags = "cpu-z,cpu,monitoring,info,cpuid",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "AIDA64",
            Aliases = "AIDA64 Extreme,AIDA64 Engineer",
            Publisher = "FinalWire Ltd.",
            ExecutableNames = "aida64.exe",
            Category = KnowledgeCategory.Hardware,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Outil de diagnostic et benchmark système.",
            ShortDescriptionEn = "System diagnostics and benchmarking tool.",
            FullDescription = "AIDA64 est un outil complet de diagnostic système, benchmarking et monitoring avec des informations détaillées sur tout le matériel.",
            FullDescriptionEn = "AIDA64 is a comprehensive system diagnostics, benchmarking and monitoring tool with detailed information about all hardware.",
            DisableImpact = "Pas de monitoring AIDA64 au démarrage.",
            DisableImpactEn = "No AIDA64 monitoring at startup.",
            PerformanceImpact = "Faible.",
            PerformanceImpactEn = "Low.",
            Recommendation = "Peut être désactivé si le monitoring n'est pas nécessaire.",
            RecommendationEn = "Can be disabled if monitoring is not needed.",
            Tags = "aida64,benchmark,diagnostic,monitoring",
            LastUpdated = DateTime.Now
        });
    }

    /// <summary>
    /// Seeds communication applications.
    /// </summary>
    private void SeedCommunicationApps()
    {
        Save(new KnowledgeEntry
        {
            Name = "Element",
            Aliases = "Element Desktop,Riot,Matrix",
            Publisher = "Element",
            ExecutableNames = "Element.exe",
            Category = KnowledgeCategory.Communication,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Client Matrix décentralisé et sécurisé.",
            ShortDescriptionEn = "Secure collaboration app.",
            FullDescription = "Element est un client de messagerie basé sur le protocole Matrix, offrant une communication décentralisée et chiffrée.",
            FullDescriptionEn = "Element is a Matrix-based end-to-end encrypted messenger and collaboration tool.",
            DisableImpact = "Pas de notifications Element au démarrage.",
            DisableImpactEn = "Messages notifications won't appear.",
            PerformanceImpact = "Faible à modéré.",
            PerformanceImpactEn = "Low to moderate.",
            Recommendation = "Peut être désactivé si non utilisé régulièrement.",
            RecommendationEn = "Keep enabled for communication.",
            Tags = "element,matrix,chat,encrypted,decentralized",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "Mumble",
            Aliases = "Mumble Client",
            Publisher = "Mumble Contributors",
            ExecutableNames = "mumble.exe",
            Category = KnowledgeCategory.Communication,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Chat vocal open source basse latence.",
            ShortDescriptionEn = "VoIP chat software.",
            FullDescription = "Mumble est une application de chat vocal open source à faible latence, populaire pour les jeux et les équipes.",
            FullDescriptionEn = "Open-source, low-latency, high-quality voice chat software primarily for gaming.",
            DisableImpact = "Mumble ne se connectera pas automatiquement.",
            DisableImpactEn = "Voice chat won't be ready.",
            PerformanceImpact = "Faible.",
            PerformanceImpactEn = "Low.",
            Recommendation = "Peut être désactivé. Lancez manuellement avant les sessions.",
            RecommendationEn = "Disable unless you need it always on.",
            Tags = "mumble,voice,chat,gaming,opensource",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "TeamSpeak",
            Aliases = "TeamSpeak 3,TeamSpeak 5,TS3",
            Publisher = "TeamSpeak Systems GmbH",
            ExecutableNames = "ts3client_win64.exe,TeamSpeak.exe",
            Category = KnowledgeCategory.Communication,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Application de communication vocale pour les équipes.",
            ShortDescriptionEn = "VoIP communication system.",
            FullDescription = "TeamSpeak est une application VoIP pour la communication d'équipe, populaire dans le gaming et le milieu professionnel.",
            FullDescriptionEn = "Voice-over-Internet Protocol (VoIP) software for audio communication between users on a chat channel.",
            DisableImpact = "TeamSpeak ne se connectera pas automatiquement.",
            DisableImpactEn = "Voice communication won't be ready.",
            PerformanceImpact = "Faible.",
            PerformanceImpactEn = "Low.",
            Recommendation = "Peut être désactivé si non utilisé quotidiennement.",
            RecommendationEn = "Disable unless needed always on.",
            Tags = "teamspeak,voice,chat,gaming,voip",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "Webex",
            Aliases = "Cisco Webex,Webex Meetings",
            Publisher = "Cisco Systems, Inc.",
            ExecutableNames = "CiscoWebexStart.exe,webex.exe",
            Category = KnowledgeCategory.Communication,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Plateforme de réunions vidéo Cisco.",
            ShortDescriptionEn = "Video conferencing.",
            FullDescription = "Webex est une plateforme de visioconférence et collaboration d'entreprise de Cisco.",
            FullDescriptionEn = "Cisco Webex is an enterprise solution for video conferencing and webinars.",
            DisableImpact = "Pas de connexion automatique aux réunions.",
            DisableImpactEn = "Meeting reminders won't show.",
            PerformanceImpact = "Faible au repos.",
            PerformanceImpactEn = "Low.",
            Recommendation = "Peut être désactivé si utilisé occasionnellement.",
            RecommendationEn = "Disable if you don't use it frequently.",
            Tags = "webex,cisco,meetings,video,conferencing",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "BlueJeans",
            Aliases = "BlueJeans Meetings",
            Publisher = "Verizon",
            ExecutableNames = "BlueJeans.exe",
            Category = KnowledgeCategory.Communication,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Plateforme de réunions vidéo.",
            ShortDescriptionEn = "Video conferencing platform.",
            FullDescription = "BlueJeans est une plateforme de visioconférence d'entreprise appartenant à Verizon.",
            FullDescriptionEn = "BlueJeans provides interoperable cloud-based video conferencing services.",
            DisableImpact = "BlueJeans ne démarrera pas automatiquement.",
            DisableImpactEn = "Meeting reminders won't show.",
            PerformanceImpact = "Faible au repos.",
            PerformanceImpactEn = "Low.",
            Recommendation = "Peut être désactivé. Lancez manuellement pour les réunions.",
            RecommendationEn = "Disable if you don't use it frequently.",
            Tags = "bluejeans,verizon,meetings,video",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "GoTo Meeting",
            Aliases = "GoToMeeting,LogMeIn",
            Publisher = "GoTo Technologies",
            ExecutableNames = "g2mstart.exe,GoToMeeting.exe",
            Category = KnowledgeCategory.Communication,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Plateforme de réunions en ligne.",
            ShortDescriptionEn = "Online meeting software.",
            FullDescription = "GoTo Meeting est un service de visioconférence et webinaires pour les entreprises.",
            FullDescriptionEn = "Video conferencing and collaboration tool for businesses.",
            DisableImpact = "GoTo Meeting ne démarrera pas automatiquement.",
            DisableImpactEn = "Meeting reminders won't show.",
            PerformanceImpact = "Faible au repos.",
            PerformanceImpactEn = "Low.",
            Recommendation = "Peut être désactivé. Lancez via lien de réunion.",
            RecommendationEn = "Disable if you don't use it frequently.",
            Tags = "gotomeeting,meetings,webinar,conferencing",
            LastUpdated = DateTime.Now
        });
    }

    /// <summary>
    /// Seeds miscellaneous applications and services.
    /// </summary>
    private void SeedMiscApps()
    {
        Save(new KnowledgeEntry
        {
            Name = "LM Studio",
            Aliases = "electron.app.LM Studio,LMStudio",
            Publisher = "LM Studio",
            ExecutableNames = "LM Studio.exe",
            Category = KnowledgeCategory.Productivity,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Application pour exécuter des LLMs localement.",
            ShortDescriptionEn = "App for running LLMs locally.",
            FullDescription = "LM Studio permet de télécharger et exécuter des modèles de langage (LLMs) localement sur votre machine. Supporte de nombreux modèles open source.",
            FullDescriptionEn = "LM Studio lets you download and run language models (LLMs) locally on your machine. Supports many open source models.",
            DisableImpact = "LM Studio ne s'ouvrira pas au démarrage.",
            DisableImpactEn = "LM Studio won't open at startup.",
            PerformanceImpact = "Élevé lors de l'utilisation (GPU/CPU intensif).",
            PerformanceImpactEn = "High during use (GPU/CPU intensive).",
            Recommendation = "N'a pas besoin de démarrer avec Windows.",
            RecommendationEn = "Doesn't need to start with Windows.",
            Tags = "lmstudio,llm,ai,local,models",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "Sandboxie Plus",
            Aliases = "SandboxiePlus,SandboxiePlus_AutoRun,SandMan",
            Publisher = "Tonalio GmbH",
            ExecutableNames = "SandMan.exe,SbieSvc.exe",
            Category = KnowledgeCategory.Security,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Bac à sable pour exécuter des programmes isolés.",
            ShortDescriptionEn = "Sandbox for running isolated programs.",
            FullDescription = "Sandboxie Plus permet d'exécuter des programmes dans un environnement isolé pour protéger votre système. Idéal pour tester des logiciels suspects.",
            FullDescriptionEn = "Sandboxie Plus runs programs in an isolated environment to protect your system. Ideal for testing suspicious software.",
            DisableImpact = "Les programmes ne seront pas automatiquement sandboxés.",
            DisableImpactEn = "Programs won't be automatically sandboxed.",
            PerformanceImpact = "Faible.",
            PerformanceImpactEn = "Low.",
            Recommendation = "Gardez actif si vous utilisez régulièrement le sandboxing.",
            RecommendationEn = "Keep enabled if you regularly use sandboxing.",
            Tags = "sandboxie,sandbox,security,isolation",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "Synology Image Assistant",
            Aliases = "Synology Assistant",
            Publisher = "Synology Inc.",
            ExecutableNames = "Synology Image Assistant.exe",
            Category = KnowledgeCategory.Utility,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Outil de gestion des NAS Synology.",
            ShortDescriptionEn = "Synology NAS management tool.",
            FullDescription = "Synology Image Assistant aide à gérer et configurer les NAS Synology depuis votre PC.",
            FullDescriptionEn = "Synology Image Assistant helps manage and configure Synology NAS from your PC.",
            DisableImpact = "L'assistant Synology ne sera pas disponible immédiatement.",
            DisableImpactEn = "Synology Assistant won't be immediately available.",
            PerformanceImpact = "Très faible.",
            PerformanceImpactEn = "Very low.",
            Recommendation = "Peut être désactivé si utilisé occasionnellement.",
            RecommendationEn = "Can be disabled if used occasionally.",
            Tags = "synology,nas,storage,assistant",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "YoloMouse",
            Aliases = "Yolo Mouse,YoloLauncher",
            Publisher = "Dragonrise Games",
            ExecutableNames = "YoloLauncher.exe,YoloMouse.exe",
            Category = KnowledgeCategory.Gaming,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Remplace le curseur dans les jeux par un curseur personnalisé.",
            ShortDescriptionEn = "Replaces game cursors with custom cursors.",
            FullDescription = "YoloMouse permet de remplacer le curseur par défaut dans les jeux par un curseur plus visible et personnalisé. Utile pour les jeux avec curseurs difficiles à voir.",
            FullDescriptionEn = "YoloMouse replaces the default cursor in games with a more visible and customizable cursor. Useful for games with hard-to-see cursors.",
            DisableImpact = "Les curseurs personnalisés ne seront pas appliqués automatiquement.",
            DisableImpactEn = "Custom cursors won't be applied automatically.",
            PerformanceImpact = "Très faible.",
            PerformanceImpactEn = "Very low.",
            Recommendation = "Peut être désactivé. Lancez manuellement avant de jouer.",
            RecommendationEn = "Can be disabled. Launch manually before gaming.",
            Tags = "yolomouse,cursor,gaming,visibility",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "RoboForm",
            Aliases = "RoboForm Password Manager",
            Publisher = "Siber Systems",
            ExecutableNames = "RoboTaskBarIcon.exe,rf-updater.exe",
            Category = KnowledgeCategory.Security,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Gestionnaire de mots de passe.",
            ShortDescriptionEn = "Password manager.",
            FullDescription = "RoboForm est un gestionnaire de mots de passe qui stocke et remplit automatiquement vos identifiants de connexion.",
            FullDescriptionEn = "RoboForm is a password manager that stores and auto-fills your login credentials.",
            DisableImpact = "Le remplissage automatique des mots de passe ne sera pas disponible.",
            DisableImpactEn = "Password auto-fill won't be available.",
            PerformanceImpact = "Faible.",
            PerformanceImpactEn = "Low.",
            Recommendation = "Gardez actif si vous l'utilisez comme gestionnaire de mots de passe principal.",
            RecommendationEn = "Keep enabled if you use it as your main password manager.",
            Tags = "roboform,password,manager,security",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "Waves MaxxAudio",
            Aliases = "WavesSvc,Waves Audio,WavesSvc64",
            Publisher = "Waves Inc",
            ExecutableNames = "WavesSvc64.exe,WavesSvc.exe",
            Category = KnowledgeCategory.Hardware,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Service d'amélioration audio Waves.",
            ShortDescriptionEn = "Waves audio enhancement service.",
            FullDescription = "Waves MaxxAudio améliore la qualité audio sur les PC Dell et autres marques. Fournit des effets audio et égalisation.",
            FullDescriptionEn = "Waves MaxxAudio enhances audio quality on Dell and other brand PCs. Provides audio effects and equalization.",
            DisableImpact = "Les améliorations audio Waves ne seront pas appliquées.",
            DisableImpactEn = "Waves audio enhancements won't be applied.",
            PerformanceImpact = "Faible.",
            PerformanceImpactEn = "Low.",
            Recommendation = "Gardez actif si vous utilisez les effets audio Waves.",
            RecommendationEn = "Keep enabled if you use Waves audio effects.",
            Tags = "waves,maxxaudio,audio,dell,enhancement",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "Brother Software Update",
            Aliases = "BrotherSoftwareUpdateNotification",
            Publisher = "Brother Industries, Ltd.",
            ExecutableNames = "SoftwareUpdateNotificationService.exe",
            Category = KnowledgeCategory.Hardware,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Service de mise à jour pour imprimantes Brother.",
            ShortDescriptionEn = "Update service for Brother printers.",
            FullDescription = "Ce service vérifie et notifie les mises à jour disponibles pour les pilotes et logiciels d'imprimantes Brother.",
            FullDescriptionEn = "This service checks and notifies available updates for Brother printer drivers and software.",
            DisableImpact = "Pas de notifications de mises à jour Brother.",
            DisableImpactEn = "No Brother update notifications.",
            PerformanceImpact = "Très faible.",
            PerformanceImpactEn = "Very low.",
            Recommendation = "Peut être désactivé. Vérifiez manuellement les mises à jour.",
            RecommendationEn = "Can be disabled. Check for updates manually.",
            Tags = "brother,printer,update,notification",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "Opera Browser Assistant",
            Aliases = "Opera Assistant,browser_assistant",
            Publisher = "Opera Norway AS",
            ExecutableNames = "browser_assistant.exe",
            Category = KnowledgeCategory.Browser,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Assistant Opera pour les mises à jour et promotions.",
            ShortDescriptionEn = "Opera Assistant for updates and promotions.",
            FullDescription = "Opera Browser Assistant gère les mises à jour automatiques d'Opera et affiche des notifications promotionnelles.",
            FullDescriptionEn = "Opera Browser Assistant manages automatic Opera updates and displays promotional notifications.",
            DisableImpact = "Pas de mises à jour automatiques d'Opera ni de notifications.",
            DisableImpactEn = "No automatic Opera updates or notifications.",
            PerformanceImpact = "Très faible.",
            PerformanceImpactEn = "Very low.",
            Recommendation = "Peut être désactivé. Les mises à jour se feront via le navigateur.",
            RecommendationEn = "Can be disabled. Updates will happen through the browser.",
            Tags = "opera,browser,assistant,update",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "Opera Stable",
            Aliases = "Opera Browser",
            Publisher = "Opera Norway AS",
            ExecutableNames = "opera.exe",
            Category = KnowledgeCategory.Browser,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Navigateur web Opera.",
            ShortDescriptionEn = "Opera web browser.",
            FullDescription = "Opera est un navigateur web avec VPN intégré, bloqueur de publicités et sidebar de messagerie.",
            FullDescriptionEn = "Opera is a web browser with built-in VPN, ad blocker, and messaging sidebar.",
            DisableImpact = "Opera ne s'ouvrira pas au démarrage.",
            DisableImpactEn = "Opera won't open at startup.",
            PerformanceImpact = "Modéré si lancé au démarrage.",
            PerformanceImpactEn = "Moderate if launched at startup.",
            Recommendation = "Peut être désactivé si vous lancez Opera manuellement.",
            RecommendationEn = "Can be disabled if you launch Opera manually.",
            Tags = "opera,browser,navigateur,vpn",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "Brave Update",
            Aliases = "BraveSoftwareUpdateTaskMachineCore,Brave Service",
            Publisher = "BraveSoftware Inc.",
            ExecutableNames = "BraveUpdate.exe",
            Category = KnowledgeCategory.Browser,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Service de mise à jour du navigateur Brave.",
            ShortDescriptionEn = "Brave browser update service.",
            FullDescription = "Ce service gère les mises à jour automatiques du navigateur Brave.",
            FullDescriptionEn = "This service manages automatic updates for the Brave browser.",
            DisableImpact = "Brave ne se mettra pas à jour automatiquement.",
            DisableImpactEn = "Brave won't update automatically.",
            PerformanceImpact = "Très faible.",
            PerformanceImpactEn = "Very low.",
            Recommendation = "Gardez actif pour les mises à jour de sécurité automatiques.",
            RecommendationEn = "Keep enabled for automatic security updates.",
            Tags = "brave,browser,update,service",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "VMware Services",
            Aliases = "vmware-tray,VMware Authorization Service,VMware DHCP Service,VMware NAT Service,VMware USB Arbitration Service",
            Publisher = "VMware, Inc.",
            ExecutableNames = "vmware-tray.exe,vmware-authd.exe,vmnetdhcp.exe,vmnat.exe,vmware-usbarbitrator64.exe",
            Category = KnowledgeCategory.Utility,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Services VMware pour la virtualisation.",
            ShortDescriptionEn = "VMware services for virtualization.",
            FullDescription = "Services nécessaires au fonctionnement de VMware Workstation pour les machines virtuelles, réseau et périphériques USB.",
            FullDescriptionEn = "Services required for VMware Workstation to run virtual machines, networking, and USB devices.",
            DisableImpact = "Les machines virtuelles VMware ne fonctionneront pas correctement.",
            DisableImpactEn = "VMware virtual machines won't work properly.",
            PerformanceImpact = "Modéré.",
            PerformanceImpactEn = "Moderate.",
            Recommendation = "Gardez actif si vous utilisez VMware Workstation.",
            RecommendationEn = "Keep enabled if you use VMware Workstation.",
            Tags = "vmware,virtualization,vm,services",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "Corsair Services",
            Aliases = "Corsair CpuIdService,Corsair LLA Service,Corsair Service",
            Publisher = "Corsair Memory, Inc.",
            ExecutableNames = "CorsairCpuIdService.exe,CueLLAccessService.exe,Corsair.Service.exe",
            Category = KnowledgeCategory.Hardware,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Services pour les périphériques Corsair.",
            ShortDescriptionEn = "Services for Corsair peripherals.",
            FullDescription = "Services nécessaires au fonctionnement de Corsair iCUE pour contrôler l'éclairage RGB et les fonctionnalités des périphériques Corsair.",
            FullDescriptionEn = "Services required for Corsair iCUE to control RGB lighting and Corsair peripheral features.",
            DisableImpact = "L'éclairage RGB Corsair et les fonctionnalités iCUE ne fonctionneront pas.",
            DisableImpactEn = "Corsair RGB lighting and iCUE features won't work.",
            PerformanceImpact = "Faible.",
            PerformanceImpactEn = "Low.",
            Recommendation = "Gardez actif si vous avez des périphériques Corsair.",
            RecommendationEn = "Keep enabled if you have Corsair peripherals.",
            Tags = "corsair,icue,rgb,services,peripherals",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "Razer Services",
            Aliases = "Razer Chroma SDK Server,Razer Chroma SDK Service,Razer Game Manager,Razer Central Service",
            Publisher = "Razer Inc.",
            ExecutableNames = "RzSDKServer.exe,RzSDKService.exe,GameManagerService.exe,RazerCentralService.exe,RzChromaStreamServer.exe",
            Category = KnowledgeCategory.Hardware,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Services pour l'écosystème Razer.",
            ShortDescriptionEn = "Services for the Razer ecosystem.",
            FullDescription = "Services nécessaires au fonctionnement de Razer Synapse et Chroma RGB pour les périphériques Razer.",
            FullDescriptionEn = "Services required for Razer Synapse and Chroma RGB to work with Razer peripherals.",
            DisableImpact = "L'éclairage Chroma et les fonctionnalités Synapse ne fonctionneront pas.",
            DisableImpactEn = "Chroma lighting and Synapse features won't work.",
            PerformanceImpact = "Modéré.",
            PerformanceImpactEn = "Moderate.",
            Recommendation = "Gardez actif si vous avez des périphériques Razer.",
            RecommendationEn = "Keep enabled if you have Razer peripherals.",
            Tags = "razer,chroma,synapse,services,rgb",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "Oculus VR Runtime",
            Aliases = "Oculus VR Runtime Service,OVRServiceLauncher",
            Publisher = "Facebook Technologies, LLC",
            ExecutableNames = "OVRServiceLauncher.exe,OVRServer_x64.exe",
            Category = KnowledgeCategory.Gaming,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Service runtime pour casques Oculus/Meta Quest.",
            ShortDescriptionEn = "Runtime service for Oculus/Meta Quest headsets.",
            FullDescription = "Service nécessaire pour utiliser les casques VR Oculus Rift et Meta Quest avec Link sur PC.",
            FullDescriptionEn = "Service required to use Oculus Rift and Meta Quest VR headsets with Link on PC.",
            DisableImpact = "Les casques Oculus/Meta Quest ne fonctionneront pas avec le PC.",
            DisableImpactEn = "Oculus/Meta Quest headsets won't work with the PC.",
            PerformanceImpact = "Modéré.",
            PerformanceImpactEn = "Moderate.",
            Recommendation = "Désactivez si vous n'utilisez pas de casque VR Oculus.",
            RecommendationEn = "Disable if you don't use an Oculus VR headset.",
            Tags = "oculus,meta,quest,vr,runtime",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "WD Drive Manager",
            Aliases = "Western Digital Drive Manager,WDDriveService",
            Publisher = "Western Digital Technologies, Inc.",
            ExecutableNames = "WDDriveService.exe",
            Category = KnowledgeCategory.Hardware,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Service pour les disques Western Digital.",
            ShortDescriptionEn = "Service for Western Digital drives.",
            FullDescription = "Service de gestion pour les disques externes Western Digital. Peut inclure des fonctionnalités de sauvegarde et sécurité.",
            FullDescriptionEn = "Management service for Western Digital external drives. May include backup and security features.",
            DisableImpact = "Certaines fonctionnalités WD peuvent ne pas fonctionner.",
            DisableImpactEn = "Some WD features may not work.",
            PerformanceImpact = "Très faible.",
            PerformanceImpactEn = "Very low.",
            Recommendation = "Peut être désactivé si vous n'utilisez pas les fonctionnalités WD avancées.",
            RecommendationEn = "Can be disabled if you don't use advanced WD features.",
            Tags = "western,digital,wd,storage,drive",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "DTS Audio",
            Aliases = "DTS APO3 Service,DTSAPO3Service",
            Publisher = "DTS, Inc.",
            ExecutableNames = "DTSAPO3Service.exe",
            Category = KnowledgeCategory.Hardware,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Service d'amélioration audio DTS.",
            ShortDescriptionEn = "DTS audio enhancement service.",
            FullDescription = "DTS Audio Processing Object améliore la qualité audio avec des effets surround et égalisation sur les PC compatibles.",
            FullDescriptionEn = "DTS Audio Processing Object enhances audio quality with surround effects and equalization on compatible PCs.",
            DisableImpact = "Les effets audio DTS ne seront pas appliqués.",
            DisableImpactEn = "DTS audio effects won't be applied.",
            PerformanceImpact = "Très faible.",
            PerformanceImpactEn = "Very low.",
            Recommendation = "Gardez actif si vous utilisez les améliorations audio DTS.",
            RecommendationEn = "Keep enabled if you use DTS audio enhancements.",
            Tags = "dts,audio,surround,enhancement",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "Logitech LampArray Service",
            Aliases = "Logitech Lamp Array",
            Publisher = "Logitech, Inc.",
            ExecutableNames = "logi_lamparray_service.exe",
            Category = KnowledgeCategory.Hardware,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Service d'éclairage Logitech.",
            ShortDescriptionEn = "Logitech lighting service.",
            FullDescription = "Service pour contrôler l'éclairage RGB dynamique sur les périphériques Logitech compatibles LampArray.",
            FullDescriptionEn = "Service to control dynamic RGB lighting on LampArray-compatible Logitech peripherals.",
            DisableImpact = "L'éclairage dynamique Logitech ne fonctionnera pas.",
            DisableImpactEn = "Logitech dynamic lighting won't work.",
            PerformanceImpact = "Très faible.",
            PerformanceImpactEn = "Very low.",
            Recommendation = "Gardez actif si vous utilisez l'éclairage RGB Logitech.",
            RecommendationEn = "Keep enabled if you use Logitech RGB lighting.",
            Tags = "logitech,lamparray,rgb,lighting",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "PLAUD",
            Aliases = "electron.app.PLAUD,PLAUD Note",
            Publisher = "PLAUD",
            ExecutableNames = "PLAUD.exe",
            Category = KnowledgeCategory.Productivity,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Application pour l'enregistreur vocal PLAUD.",
            ShortDescriptionEn = "App for PLAUD voice recorder.",
            FullDescription = "PLAUD est l'application compagnon pour les enregistreurs vocaux PLAUD. Permet de transcrire et gérer les enregistrements audio.",
            FullDescriptionEn = "PLAUD is the companion app for PLAUD voice recorders. Allows transcribing and managing audio recordings.",
            DisableImpact = "L'application PLAUD ne s'ouvrira pas au démarrage.",
            DisableImpactEn = "PLAUD app won't open at startup.",
            PerformanceImpact = "Faible.",
            PerformanceImpactEn = "Low.",
            Recommendation = "Peut être désactivé si vous n'utilisez pas régulièrement l'enregistreur.",
            RecommendationEn = "Can be disabled if you don't regularly use the recorder.",
            Tags = "plaud,recorder,voice,transcription,audio",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "AMD External Events Utility",
            Aliases = "atiesrxx,AMD External Events",
            Publisher = "AMD",
            ExecutableNames = "atiesrxx.exe",
            Category = KnowledgeCategory.Hardware,
            SafetyLevel = SafetyLevel.Important,
            ShortDescription = "Service AMD pour les événements externes.",
            ShortDescriptionEn = "AMD service for external events.",
            FullDescription = "Ce service AMD gère les événements externes comme les changements d'affichage, les hotkeys et la communication avec les pilotes graphiques.",
            FullDescriptionEn = "This AMD service manages external events like display changes, hotkeys, and communication with graphics drivers.",
            DisableImpact = "Certaines fonctionnalités AMD (hotkeys, détection d'affichage) peuvent ne pas fonctionner.",
            DisableImpactEn = "Some AMD features (hotkeys, display detection) may not work.",
            PerformanceImpact = "Très faible.",
            PerformanceImpactEn = "Very low.",
            Recommendation = "Gardez actif si vous avez une carte graphique AMD.",
            RecommendationEn = "Keep enabled if you have an AMD graphics card.",
            Tags = "amd,radeon,events,graphics,driver",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "ASUS Update Check",
            Aliases = "AsusUpdateCheck",
            Publisher = "ASUSTeK Computer Inc.",
            ExecutableNames = "AsusUpdateCheck.exe",
            Category = KnowledgeCategory.Hardware,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Service de vérification des mises à jour ASUS.",
            ShortDescriptionEn = "ASUS update check service.",
            FullDescription = "Ce service vérifie les mises à jour disponibles pour les logiciels et pilotes ASUS sur votre système.",
            FullDescriptionEn = "This service checks for available updates for ASUS software and drivers on your system.",
            DisableImpact = "Pas de notifications de mises à jour ASUS.",
            DisableImpactEn = "No ASUS update notifications.",
            PerformanceImpact = "Très faible.",
            PerformanceImpactEn = "Very low.",
            Recommendation = "Peut être désactivé. Vérifiez manuellement via MyASUS.",
            RecommendationEn = "Can be disabled. Check manually via MyASUS.",
            Tags = "asus,update,check,drivers",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "ASUS GameSDK Service",
            Aliases = "GameSDK Service,ASUS GameSDK",
            Publisher = "ASUS Inc.",
            ExecutableNames = "GameSDK.exe",
            Category = KnowledgeCategory.Gaming,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Service ASUS pour l'optimisation gaming.",
            ShortDescriptionEn = "ASUS service for gaming optimization.",
            FullDescription = "ASUS GameSDK optimise les performances système pour les jeux sur les appareils ASUS. Fait partie de l'écosystème Armoury Crate.",
            FullDescriptionEn = "ASUS GameSDK optimizes system performance for gaming on ASUS devices. Part of the Armoury Crate ecosystem.",
            DisableImpact = "Les optimisations gaming automatiques ASUS ne s'appliqueront pas.",
            DisableImpactEn = "ASUS automatic gaming optimizations won't apply.",
            PerformanceImpact = "Faible.",
            PerformanceImpactEn = "Low.",
            Recommendation = "Gardez actif si vous utilisez Armoury Crate pour le gaming.",
            RecommendationEn = "Keep enabled if you use Armoury Crate for gaming.",
            Tags = "asus,gaming,sdk,armoury,crate,optimization",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "PDF-XChange Pro",
            Aliases = "PDFProFiltSrvPP,PDF-XChange",
            Publisher = "Tracker Software Products",
            ExecutableNames = "PDFProFiltSrvPP.exe",
            Category = KnowledgeCategory.Productivity,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Service pour PDF-XChange Editor/Pro.",
            ShortDescriptionEn = "Service for PDF-XChange Editor/Pro.",
            FullDescription = "Service de filtrage pour PDF-XChange Editor Pro, permettant l'intégration système et les fonctionnalités avancées d'édition PDF.",
            FullDescriptionEn = "Filtering service for PDF-XChange Editor Pro, enabling system integration and advanced PDF editing features.",
            DisableImpact = "Certaines fonctionnalités d'intégration PDF-XChange peuvent ne pas fonctionner.",
            DisableImpactEn = "Some PDF-XChange integration features may not work.",
            PerformanceImpact = "Très faible.",
            PerformanceImpactEn = "Very low.",
            Recommendation = "Gardez actif si vous utilisez PDF-XChange régulièrement.",
            RecommendationEn = "Keep enabled if you use PDF-XChange regularly.",
            Tags = "pdf,xchange,editor,tracker,document",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "Intel System Usage Report",
            Aliases = "SystemUsageReportSvc_QUEENCREEK,IntelSURQC,SurSvc",
            Publisher = "Intel Corporation",
            ExecutableNames = "SurSvc.exe,IntelSoftwareAssetManagerService.exe",
            Category = KnowledgeCategory.Hardware,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Service de télémétrie Intel.",
            ShortDescriptionEn = "Intel telemetry service.",
            FullDescription = "Service Intel qui collecte des données d'utilisation système pour l'amélioration des produits. Peut être désactivé sans impact.",
            FullDescriptionEn = "Intel service that collects system usage data for product improvement. Can be disabled without impact.",
            DisableImpact = "Pas de collecte de données d'utilisation Intel.",
            DisableImpactEn = "No Intel usage data collection.",
            PerformanceImpact = "Très faible.",
            PerformanceImpactEn = "Very low.",
            Recommendation = "Peut être désactivé pour la vie privée.",
            RecommendationEn = "Can be disabled for privacy.",
            Tags = "intel,telemetry,usage,report,privacy",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "Intel Energy Server",
            Aliases = "Energy Server Service queencreek,esrv_svc",
            Publisher = "Intel Corporation",
            ExecutableNames = "esrv_svc.exe",
            Category = KnowledgeCategory.Hardware,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Service de gestion d'énergie Intel.",
            ShortDescriptionEn = "Intel energy management service.",
            FullDescription = "Service Intel pour la gestion de l'énergie et l'optimisation des performances sur les systèmes Intel.",
            FullDescriptionEn = "Intel service for energy management and performance optimization on Intel systems.",
            DisableImpact = "Les optimisations d'énergie Intel peuvent ne pas fonctionner.",
            DisableImpactEn = "Intel energy optimizations may not work.",
            PerformanceImpact = "Très faible.",
            PerformanceImpactEn = "Very low.",
            Recommendation = "Peut être désactivé sur les PC de bureau.",
            RecommendationEn = "Can be disabled on desktop PCs.",
            Tags = "intel,energy,power,management",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "Google Updater",
            Aliases = "GoogleUpdaterTaskSystem,Google Update",
            Publisher = "Google LLC",
            ExecutableNames = "updater.exe,GoogleUpdate.exe",
            Category = KnowledgeCategory.Utility,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Service de mise à jour des applications Google.",
            ShortDescriptionEn = "Google applications update service.",
            FullDescription = "Google Updater maintient à jour les applications Google installées (Chrome, Drive, Earth, etc.).",
            FullDescriptionEn = "Google Updater keeps installed Google applications up to date (Chrome, Drive, Earth, etc.).",
            DisableImpact = "Les applications Google ne se mettront pas à jour automatiquement.",
            DisableImpactEn = "Google applications won't update automatically.",
            PerformanceImpact = "Très faible.",
            PerformanceImpactEn = "Very low.",
            Recommendation = "Gardez actif pour les mises à jour de sécurité automatiques.",
            RecommendationEn = "Keep enabled for automatic security updates.",
            Tags = "google,update,chrome,drive,updater",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "Twain Scanner Service",
            Aliases = "S18A,TwDsUiLaunch",
            Publisher = "Microsoft Windows Hardware Compatibility Publisher",
            ExecutableNames = "TwDsUiLaunch.exe",
            Category = KnowledgeCategory.Hardware,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Service TWAIN pour scanners.",
            ShortDescriptionEn = "TWAIN service for scanners.",
            FullDescription = "Service qui gère l'interface TWAIN pour les scanners et appareils d'imagerie.",
            FullDescriptionEn = "Service that manages the TWAIN interface for scanners and imaging devices.",
            DisableImpact = "Les scanners TWAIN peuvent ne pas fonctionner correctement.",
            DisableImpactEn = "TWAIN scanners may not work properly.",
            PerformanceImpact = "Très faible.",
            PerformanceImpactEn = "Very low.",
            Recommendation = "Gardez actif si vous utilisez un scanner.",
            RecommendationEn = "Keep enabled if you use a scanner.",
            Tags = "twain,scanner,imaging,driver",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "Radeon Software Service",
            Aliases = "StartCN,cncmd",
            Publisher = "AMD",
            ExecutableNames = "cncmd.exe",
            Category = KnowledgeCategory.Hardware,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Service Radeon Software d'AMD.",
            ShortDescriptionEn = "AMD Radeon Software service.",
            FullDescription = "Service de commande pour Radeon Software, gérant les fonctionnalités avancées des cartes graphiques AMD.",
            FullDescriptionEn = "Command service for Radeon Software, managing advanced features of AMD graphics cards.",
            DisableImpact = "Certaines fonctionnalités Radeon Software peuvent ne pas fonctionner.",
            DisableImpactEn = "Some Radeon Software features may not work.",
            PerformanceImpact = "Faible.",
            PerformanceImpactEn = "Low.",
            Recommendation = "Gardez actif si vous utilisez Radeon Software.",
            RecommendationEn = "Keep enabled if you use Radeon Software.",
            Tags = "amd,radeon,software,graphics",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "AMD ReLive DVR",
            Aliases = "StartDVR,RSServCmd,ReLive",
            Publisher = "AMD",
            ExecutableNames = "RSServCmd.exe",
            Category = KnowledgeCategory.Gaming,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Service d'enregistrement AMD ReLive.",
            ShortDescriptionEn = "AMD ReLive recording service.",
            FullDescription = "AMD ReLive permet d'enregistrer et de streamer vos sessions de jeu avec les cartes graphiques AMD.",
            FullDescriptionEn = "AMD ReLive allows recording and streaming your gaming sessions with AMD graphics cards.",
            DisableImpact = "L'enregistrement et le streaming ReLive ne seront pas disponibles.",
            DisableImpactEn = "ReLive recording and streaming won't be available.",
            PerformanceImpact = "Faible au repos, modéré lors de l'enregistrement.",
            PerformanceImpactEn = "Low at idle, moderate during recording.",
            Recommendation = "Peut être désactivé si vous n'utilisez pas ReLive.",
            RecommendationEn = "Can be disabled if you don't use ReLive.",
            Tags = "amd,relive,recording,streaming,gaming",
            LastUpdated = DateTime.Now
        });

        // Shell Extensions and Context Menu Handlers
        Save(new KnowledgeEntry
        {
            Name = "Synology Assistant",
            Aliases = "Synology USB Client,UsbClientService",
            Publisher = "Synology Inc.",
            ExecutableNames = "UsbClientService.exe,Synology Assistant.exe",
            Category = KnowledgeCategory.Utility,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Service Synology pour la connexion USB directe au NAS.",
            ShortDescriptionEn = "Synology service for direct USB connection to NAS.",
            FullDescription = "Synology Assistant permet de découvrir et gérer les NAS Synology sur le réseau local. Le service USB permet la connexion directe d'un NAS via USB.",
            FullDescriptionEn = "Synology Assistant allows discovering and managing Synology NAS on the local network. The USB service enables direct NAS connection via USB.",
            DisableImpact = "La connexion USB directe au NAS ne fonctionnera pas.",
            DisableImpactEn = "Direct USB connection to NAS won't work.",
            PerformanceImpact = "Très faible.",
            PerformanceImpactEn = "Very low.",
            Recommendation = "Peut être désactivé si vous n'utilisez pas la connexion USB de votre NAS.",
            RecommendationEn = "Can be disabled if you don't use your NAS USB connection.",
            Tags = "synology,nas,storage,usb,network",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "LockHunter",
            Aliases = "LHShellExt,LockHunterShellExtension",
            Publisher = "Crystal Rich Ltd",
            ExecutableNames = "LockHunter.exe,LHShellExt64.dll",
            Category = KnowledgeCategory.Utility,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Outil pour débloquer et supprimer les fichiers verrouillés.",
            ShortDescriptionEn = "Tool to unlock and delete locked files.",
            FullDescription = "LockHunter permet de voir quels processus verrouillent un fichier et de le débloquer. L'extension shell ajoute une option au menu contextuel.",
            FullDescriptionEn = "LockHunter shows which processes are locking a file and allows unlocking it. The shell extension adds a context menu option.",
            DisableImpact = "L'option 'What is locking this file?' ne sera pas disponible dans le menu contextuel.",
            DisableImpactEn = "The 'What is locking this file?' option won't be available in the context menu.",
            PerformanceImpact = "Négligeable (extension shell uniquement).",
            PerformanceImpactEn = "Negligible (shell extension only).",
            Recommendation = "Peut rester actif, n'impacte pas les performances.",
            RecommendationEn = "Can remain enabled, doesn't impact performance.",
            Tags = "lockhunter,unlock,file,locked,delete",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "Tracker Software PDF-XChange",
            Aliases = "PDF-XChange Editor,PDF Tools,PDFXToolsShellMenu,XCShellMenu",
            Publisher = "Tracker Software Products (Canada) Ltd.",
            ExecutableNames = "PDFXEdit.exe,PDFXTools.exe,PDFXToolsShellMenu.x64.dll,XCShellMenu.x64.dll",
            Category = KnowledgeCategory.Productivity,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Éditeur PDF professionnel avec outils avancés.",
            ShortDescriptionEn = "PDF-XChange updater.",
            FullDescription = "PDF-XChange Editor est un éditeur PDF complet permettant l'annotation, la modification et la création de PDF. Les extensions shell ajoutent des options au menu contextuel.",
            FullDescriptionEn = "Update agent for PDF-XChange software.",
            DisableImpact = "Les options PDF dans le menu contextuel ne seront pas disponibles.",
            DisableImpactEn = "Updates won't be detected.",
            PerformanceImpact = "Négligeable (extension shell uniquement).",
            PerformanceImpactEn = "Very low.",
            Recommendation = "Peut rester actif pour un accès rapide aux outils PDF.",
            RecommendationEn = "Can be disabled.",
            Tags = "pdf,editor,tracker,annotation,document",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "Internet Download Manager",
            Aliases = "IDM,IDMIEHlprObj,IDMIECC",
            Publisher = "Tonec Inc.",
            ExecutableNames = "IDMan.exe,IDMIECC64.dll,IEMonitor.exe",
            Category = KnowledgeCategory.Utility,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Gestionnaire de téléchargements avec accélération.",
            ShortDescriptionEn = "Download manager with acceleration.",
            FullDescription = "Internet Download Manager (IDM) accélère les téléchargements en utilisant plusieurs connexions simultanées. S'intègre aux navigateurs pour capturer les téléchargements.",
            FullDescriptionEn = "Internet Download Manager (IDM) accelerates downloads using multiple simultaneous connections. Integrates with browsers to capture downloads.",
            DisableImpact = "L'intégration IDM dans les navigateurs ne fonctionnera pas automatiquement.",
            DisableImpactEn = "IDM browser integration won't work automatically.",
            PerformanceImpact = "Faible (~20-40 Mo RAM).",
            PerformanceImpactEn = "Low (~20-40 MB RAM).",
            Recommendation = "Gardez actif si vous utilisez IDM régulièrement.",
            RecommendationEn = "Keep enabled if you use IDM regularly.",
            Tags = "idm,download,manager,accelerator,internet",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "Adobe CoreSync",
            Aliases = "AccExt,CoreSync_x64,Adobe Creative Cloud Files",
            Publisher = "Adobe Inc.",
            ExecutableNames = "CoreSync.exe,CoreSync_x64.dll",
            Category = KnowledgeCategory.Productivity,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Synchronisation des fichiers Adobe Creative Cloud.",
            ShortDescriptionEn = "Adobe Creative Cloud synchronization.",
            FullDescription = "CoreSync gère la synchronisation des fichiers Creative Cloud entre votre ordinateur et le cloud Adobe. L'extension shell ajoute des icônes d'état de synchronisation.",
            FullDescriptionEn = "CoreSync synchronizes files, fonts, and settings with Adobe Creative Cloud.",
            DisableImpact = "Les fichiers Creative Cloud ne se synchroniseront pas automatiquement.",
            DisableImpactEn = "Cloud syncing won't work.",
            PerformanceImpact = "Faible au repos, modéré lors de la synchronisation.",
            PerformanceImpactEn = "Low to moderate.",
            Recommendation = "Gardez actif si vous utilisez les fichiers Creative Cloud.",
            RecommendationEn = "Keep enabled if you use Creative Cloud features.",
            Tags = "adobe,creative,cloud,sync,files",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "WinRAR",
            Aliases = "WinRAR32,RAR,WinRAR Shell Extension",
            Publisher = "Alexander Roshal",
            ExecutableNames = "WinRAR.exe,Rar.exe,UnRAR.exe",
            Category = KnowledgeCategory.Utility,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Utilitaire de compression et décompression d'archives.",
            ShortDescriptionEn = "Archive compression and extraction utility.",
            FullDescription = "WinRAR est un des compresseurs d'archives les plus populaires, supportant RAR, ZIP, 7Z et de nombreux autres formats. L'extension shell permet d'extraire directement depuis l'explorateur.",
            FullDescriptionEn = "WinRAR is one of the most popular archive compressors, supporting RAR, ZIP, 7Z and many other formats. The shell extension allows direct extraction from explorer.",
            DisableImpact = "Les options WinRAR dans le menu contextuel ne seront pas disponibles.",
            DisableImpactEn = "WinRAR options in context menu won't be available.",
            PerformanceImpact = "Négligeable (extension shell uniquement).",
            PerformanceImpactEn = "Negligible (shell extension only).",
            Recommendation = "Peut rester actif pour un accès rapide à la compression.",
            RecommendationEn = "Can stay enabled for quick access to compression.",
            Tags = "winrar,compression,archive,zip,rar",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "LDPlayer",
            Aliases = "ldplayer,LdVBox,ldplayerbox,ldplayer9box",
            Publisher = "Shanghai Chang Zhi Network Technology",
            ExecutableNames = "dnplayer.exe,LdVBoxDrv.sys,Ld9BoxSup.sys",
            Category = KnowledgeCategory.Gaming,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Émulateur Android pour PC.",
            ShortDescriptionEn = "Android emulator.",
            FullDescription = "LDPlayer est un émulateur Android optimisé pour les jeux. Permet de jouer aux jeux Android sur PC avec de meilleures performances et contrôles.",
            FullDescriptionEn = "Lightweight Android emulator for gaming on PC.",
            DisableImpact = "L'émulateur devra être lancé manuellement.",
            DisableImpactEn = "Emulator won't start automatically.",
            PerformanceImpact = "Élevé quand actif (~1-2 Go RAM).",
            PerformanceImpactEn = "High.",
            Recommendation = "Les services peuvent être désactivés. Lancez LDPlayer manuellement.",
            RecommendationEn = "Disable from startup.",
            Tags = "ldplayer,android,emulator,gaming,mobile",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "Kofax Power PDF",
            Aliases = "PDF Create,Nuance PDF,SDirectShellExt",
            Publisher = "Kofax,Zeon Corporation,Nuance",
            ExecutableNames = "PDFCreate!.exe,SDirectShellExt.dll",
            Category = KnowledgeCategory.Productivity,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Suite de création et édition PDF professionnelle.",
            ShortDescriptionEn = "PDF editor.",
            FullDescription = "Kofax Power PDF (anciennement Nuance) permet de créer, éditer et convertir des documents PDF. L'extension shell ajoute des options de conversion au menu contextuel.",
            FullDescriptionEn = "Software to create, convert, edit and share PDF files.",
            DisableImpact = "Les options de conversion PDF dans le menu contextuel ne seront pas disponibles.",
            DisableImpactEn = "Quick launch features won't be active.",
            PerformanceImpact = "Négligeable (extension shell uniquement).",
            PerformanceImpactEn = "Low.",
            Recommendation = "Peut rester actif pour un accès rapide à la conversion PDF.",
            RecommendationEn = "Disable from startup.",
            Tags = "kofax,nuance,pdf,create,convert",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "Autodesk Shell Extension",
            Aliases = "AcContextMenuHandler,AcShellExtension",
            Publisher = "Autodesk, Inc.",
            ExecutableNames = "AcShellExtension.dll",
            Category = KnowledgeCategory.Productivity,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Extension shell pour fichiers AutoCAD.",
            ShortDescriptionEn = "Autodesk file integration.",
            FullDescription = "Extension Autodesk qui ajoute des options au menu contextuel pour les fichiers DWG et autres formats AutoCAD.",
            FullDescriptionEn = "Provides thumbnail previews and property details for Autodesk files in Windows Explorer.",
            DisableImpact = "Les options Autodesk dans le menu contextuel ne seront pas disponibles.",
            DisableImpactEn = "Previews for CAD files might be missing.",
            PerformanceImpact = "Négligeable.",
            PerformanceImpactEn = "Very low.",
            Recommendation = "Peut rester actif si vous travaillez avec des fichiers AutoCAD.",
            RecommendationEn = "Keep enabled if you work with Autodesk files.",
            Tags = "autodesk,autocad,dwg,shell,extension",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "Google Drive File Stream",
            Aliases = "DriveFS,googledrivefs,DriveFS ContextMenu",
            Publisher = "Google LLC",
            ExecutableNames = "GoogleDriveFS.exe,drivefsext.dll,googledrivefs*.sys",
            Category = KnowledgeCategory.CloudStorage,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Client de synchronisation Google Drive pour bureau.",
            ShortDescriptionEn = "Google Drive for desktop.",
            FullDescription = "Google Drive pour bureau synchronise vos fichiers Google Drive avec votre ordinateur. Permet d'accéder aux fichiers en streaming sans les télécharger tous.",
            FullDescriptionEn = "Streams My Drive and Team Drives files directly from the cloud to your PC.",
            DisableImpact = "Google Drive ne sera pas accessible comme lecteur virtuel.",
            DisableImpactEn = "Drive files won't be accessible in Explorer.",
            PerformanceImpact = "Faible au repos, modéré lors de la synchronisation.",
            PerformanceImpactEn = "Low to moderate.",
            Recommendation = "Gardez actif si vous utilisez Google Drive régulièrement.",
            RecommendationEn = "Keep enabled if you use Google Drive.",
            Tags = "google,drive,cloud,sync,storage",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "RUXIM",
            Aliases = "PLUGScheduler,Windows Update Health Tools",
            Publisher = "Microsoft Corporation",
            ExecutableNames = "PLUGScheduler.exe",
            Category = KnowledgeCategory.WindowsSystem,
            SafetyLevel = SafetyLevel.Critical,
            ShortDescription = "Composant Windows Update pour la maintenance système.",
            ShortDescriptionEn = "Update interaction manager.",
            FullDescription = "RUXIM (Remediation Update eXecutor IMproved) est un composant Microsoft qui aide à maintenir Windows à jour et en bon état. Fait partie des outils de santé Windows Update.",
            FullDescriptionEn = "Reusable UX Interaction Manager, used by Windows Update to show interaction campaigns.",
            DisableImpact = "Certaines mises à jour Windows et corrections automatiques peuvent ne pas fonctionner.",
            DisableImpactEn = "Update notifications might be missed.",
            PerformanceImpact = "Très faible.",
            PerformanceImpactEn = "Very low.",
            Recommendation = "Ne pas désactiver. Composant système important.",
            RecommendationEn = "Leave enabled.",
            Tags = "microsoft,windows,update,health,remediation",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "Windows Clipboard Service",
            Aliases = "ClipESU,Clipboard Enhanced Service",
            Publisher = "Microsoft Corporation",
            ExecutableNames = "clipesu.exe",
            Category = KnowledgeCategory.WindowsSystem,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Service amélioré du presse-papiers Windows.",
            ShortDescriptionEn = "Clipboard manager.",
            FullDescription = "Service lié aux fonctionnalités avancées du presse-papiers Windows comme l'historique du presse-papiers et la synchronisation entre appareils.",
            FullDescriptionEn = "Enables clipboard scenarios like history and cloud sync.",
            DisableImpact = "L'historique du presse-papiers et la synchronisation peuvent ne pas fonctionner.",
            DisableImpactEn = "Clipboard history won't work.",
            PerformanceImpact = "Très faible.",
            PerformanceImpactEn = "Very low.",
            Recommendation = "Gardez actif si vous utilisez l'historique du presse-papiers (Win+V).",
            RecommendationEn = "Keep enabled.",
            Tags = "clipboard,windows,history,sync,paste",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "ASUS ROG Peripherals",
            Aliases = "P508PowerAgent,ROG STRIX CARRY,ASUS Mouse",
            Publisher = "ASUSTeK Computer Inc.",
            ExecutableNames = "P508PowerAgent.exe,ArmouryDevice*.exe",
            Category = KnowledgeCategory.Hardware,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Agent de gestion des périphériques gaming ROG.",
            ShortDescriptionEn = "ASUS ROG peripheral software.",
            FullDescription = "Service ASUS pour la gestion des périphériques gaming ROG (souris, claviers). Gère les profils, l'éclairage RGB et la batterie.",
            FullDescriptionEn = "Software to manage ASUS ROG peripherals (mice, keyboards) and Aura Sync lighting.",
            DisableImpact = "Les profils et l'éclairage RGB des périphériques ROG ne seront pas gérés automatiquement.",
            DisableImpactEn = "Custom settings and lighting won't apply.",
            PerformanceImpact = "Faible.",
            PerformanceImpactEn = "Moderate.",
            Recommendation = "Gardez actif si vous utilisez des périphériques ROG avec des profils personnalisés.",
            RecommendationEn = "Keep enabled if you use ASUS peripherals.",
            Tags = "asus,rog,mouse,keyboard,gaming,rgb",
            LastUpdated = DateTime.Now
        });

        // ============================================================
        // MEDIA PLAYERS & EDITORS
        // ============================================================

        Save(new KnowledgeEntry
        {
            Name = "VLC Media Player",
            Aliases = "VLC,VideoLAN",
            Publisher = "VideoLAN",
            ExecutableNames = "vlc.exe",
            Category = KnowledgeCategory.Media,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Lecteur multimédia open-source universel.",
            ShortDescriptionEn = "Multimedia player.",
            FullDescription = "VLC est un lecteur multimédia gratuit capable de lire pratiquement tous les formats audio et vidéo. Il peut aussi servir de serveur de streaming.",
            FullDescriptionEn = "VLC is a free and open source cross-platform multimedia player.",
            DisableImpact = "Aucun impact au démarrage - VLC ne démarre normalement pas automatiquement.",
            DisableImpactEn = "No impact.",
            PerformanceImpact = "Nul au repos.",
            PerformanceImpactEn = "None.",
            Recommendation = "Peut être désactivé du démarrage sans problème.",
            RecommendationEn = "Should not be in startup.",
            Tags = "vlc,video,audio,player,multimedia,opensource",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "OBS Studio",
            Aliases = "OBS,Open Broadcaster Software,obs64",
            Publisher = "OBS Project",
            ExecutableNames = "obs64.exe,obs32.exe,obs-browser-page.exe",
            Category = KnowledgeCategory.Media,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Logiciel de streaming et d'enregistrement vidéo.",
            ShortDescriptionEn = "Streaming and video recording software.",
            FullDescription = "OBS Studio est un logiciel gratuit et open-source pour l'enregistrement vidéo et le streaming en direct. Très populaire chez les créateurs de contenu.",
            FullDescriptionEn = "OBS Studio is free and open-source software for video recording and live streaming. Very popular among content creators.",
            DisableImpact = "Aucun impact - OBS ne doit pas démarrer automatiquement.",
            DisableImpactEn = "No impact - OBS should not start automatically.",
            PerformanceImpact = "Élevé uniquement pendant l'enregistrement/streaming.",
            PerformanceImpactEn = "High only during recording/streaming.",
            Recommendation = "Désactivez du démarrage automatique.",
            RecommendationEn = "Disable from automatic startup.",
            Tags = "obs,streaming,recording,video,twitch,youtube",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "Audacity",
            Aliases = "Audacity Audio Editor",
            Publisher = "Audacity Team",
            ExecutableNames = "audacity.exe",
            Category = KnowledgeCategory.Media,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Éditeur audio open-source.",
            ShortDescriptionEn = "Open-source audio editor.",
            FullDescription = "Audacity est un éditeur et enregistreur audio multipiste gratuit et open-source. Permet l'édition, le mixage et l'application d'effets audio.",
            FullDescriptionEn = "Audacity is a free and open-source multitrack audio editor and recorder. Allows editing, mixing and applying audio effects.",
            DisableImpact = "Aucun - ne devrait pas démarrer automatiquement.",
            DisableImpactEn = "None - should not start automatically.",
            PerformanceImpact = "Nul au repos.",
            PerformanceImpactEn = "None when idle.",
            Recommendation = "Désactivez du démarrage automatique.",
            RecommendationEn = "Disable from automatic startup.",
            Tags = "audacity,audio,editor,recording,music,opensource",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "HandBrake",
            Aliases = "HandBrake Video Transcoder",
            Publisher = "The HandBrake Team",
            ExecutableNames = "HandBrake.exe,HandBrake.Worker.exe",
            Category = KnowledgeCategory.Media,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Convertisseur vidéo open-source.",
            ShortDescriptionEn = "Video transcoder.",
            FullDescription = "HandBrake est un transcodeur vidéo gratuit permettant de convertir des vidéos dans différents formats avec un contrôle précis de la qualité.",
            FullDescriptionEn = "Open-source tool for converting video from nearly any format.",
            DisableImpact = "Aucun - ne devrait pas démarrer automatiquement.",
            DisableImpactEn = "No impact.",
            PerformanceImpact = "Très élevé pendant la conversion uniquement.",
            PerformanceImpactEn = "None.",
            Recommendation = "Désactivez du démarrage automatique.",
            RecommendationEn = "Should not be in startup.",
            Tags = "handbrake,video,converter,transcoder,encoding",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "GIMP",
            Aliases = "GNU Image Manipulation Program,gimp-2",
            Publisher = "The GIMP Team",
            ExecutableNames = "gimp*.exe",
            Category = KnowledgeCategory.Media,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Éditeur d'images open-source.",
            ShortDescriptionEn = "Image editor.",
            FullDescription = "GIMP est un éditeur d'images gratuit et puissant, alternative open-source à Photoshop. Supporte les calques, filtres et plugins.",
            FullDescriptionEn = "GNU Image Manipulation Program, a free and open-source image editor.",
            DisableImpact = "Aucun - ne devrait pas démarrer automatiquement.",
            DisableImpactEn = "No impact.",
            PerformanceImpact = "Nul au repos.",
            PerformanceImpactEn = "None.",
            Recommendation = "Désactivez du démarrage automatique.",
            RecommendationEn = "Should not be in startup.",
            Tags = "gimp,image,editor,photo,graphics,opensource",
            LastUpdated = DateTime.Now
        });

        // ============================================================
        // DEVELOPMENT TOOLS
        // ============================================================

        Save(new KnowledgeEntry
        {
            Name = "Visual Studio Code",
            Aliases = "VSCode,Code,VS Code",
            Publisher = "Microsoft Corporation",
            ExecutableNames = "Code.exe,code.cmd",
            Category = KnowledgeCategory.Utility,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Éditeur de code source léger et extensible.",
            ShortDescriptionEn = "Lightweight and extensible source code editor.",
            FullDescription = "Visual Studio Code est un éditeur de code gratuit de Microsoft avec support du débogage, contrôle Git intégré, coloration syntaxique et extensions.",
            FullDescriptionEn = "Visual Studio Code is a free code editor from Microsoft with debugging support, integrated Git control, syntax highlighting and extensions.",
            DisableImpact = "VS Code ne s'ouvrira pas automatiquement au démarrage.",
            DisableImpactEn = "VS Code won't open automatically at startup.",
            PerformanceImpact = "Modéré quand actif, nul au repos.",
            PerformanceImpactEn = "Moderate when active, none when idle.",
            Recommendation = "Peut être désactivé du démarrage automatique.",
            RecommendationEn = "Can be disabled from automatic startup.",
            Tags = "vscode,code,editor,development,microsoft,programming",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "Git for Windows",
            Aliases = "Git,git-bash,git-cmd",
            Publisher = "The Git Development Community",
            ExecutableNames = "git.exe,git-bash.exe,git-cmd.exe,bash.exe",
            Category = KnowledgeCategory.Utility,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Système de contrôle de version distribué.",
            ShortDescriptionEn = "Git version control.",
            FullDescription = "Git est le système de contrôle de version le plus utilisé au monde. Git for Windows inclut Git Bash pour une expérience en ligne de commande Unix-like.",
            FullDescriptionEn = "Provides Git command line and GUI tools for Windows.",
            DisableImpact = "Git reste utilisable, seuls les helpers d'authentification peuvent être affectés.",
            DisableImpactEn = "Git Bash integration/agents won't load.",
            PerformanceImpact = "Nul.",
            PerformanceImpactEn = "Very low.",
            Recommendation = "Le credential manager peut démarrer automatiquement - généralement utile.",
            RecommendationEn = "Keep agents enabled if you use SSH keys.",
            Tags = "git,version,control,development,github,source",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "Node.js",
            Aliases = "Node,nodejs,npm",
            Publisher = "OpenJS Foundation",
            ExecutableNames = "node.exe,npm.cmd,npx.cmd",
            Category = KnowledgeCategory.Utility,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Runtime JavaScript côté serveur.",
            ShortDescriptionEn = "JavaScript runtime.",
            FullDescription = "Node.js est un environnement d'exécution JavaScript construit sur le moteur V8 de Chrome. Inclut npm, le gestionnaire de paquets JavaScript.",
            FullDescriptionEn = "JavaScript runtime built on Chrome's V8 engine.",
            DisableImpact = "Aucun - Node.js ne démarre pas automatiquement.",
            DisableImpactEn = "Node services/updates won't run.",
            PerformanceImpact = "Nul au repos.",
            PerformanceImpactEn = "Varies.",
            Recommendation = "Ne devrait pas être dans le démarrage automatique.",
            RecommendationEn = "Disable unless running a specific server.",
            Tags = "node,nodejs,javascript,npm,development,runtime",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "Python",
            Aliases = "Python3,pythonw,py",
            Publisher = "Python Software Foundation",
            ExecutableNames = "python.exe,pythonw.exe,python3*.exe,py.exe",
            Category = KnowledgeCategory.Utility,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Langage de programmation Python.",
            ShortDescriptionEn = "Programming language.",
            FullDescription = "Python est un langage de programmation interprété, polyvalent et populaire pour le scripting, le développement web, la data science et l'automatisation.",
            FullDescriptionEn = "Python interpreter often used for scripts and automation.",
            DisableImpact = "Aucun - Python ne démarre pas automatiquement.",
            DisableImpactEn = "Scripts won't run.",
            PerformanceImpact = "Nul au repos.",
            PerformanceImpactEn = "Varies.",
            Recommendation = "Ne devrait pas être dans le démarrage automatique.",
            RecommendationEn = "Depends on the specific script.",
            Tags = "python,programming,scripting,development,language",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "Java Runtime",
            Aliases = "Java,JRE,JDK,OpenJDK,javaw",
            Publisher = "Oracle Corporation",
            ExecutableNames = "java.exe,javaw.exe,javaws.exe,jusched.exe",
            Category = KnowledgeCategory.Utility,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Environnement d'exécution Java.",
            ShortDescriptionEn = "Java update scheduler.",
            FullDescription = "Java Runtime Environment permet d'exécuter des applications Java. jusched.exe vérifie les mises à jour Java automatiquement.",
            FullDescriptionEn = "Checks for updates to the Java Runtime Environment.",
            DisableImpact = "Les mises à jour automatiques de Java ne seront plus vérifiées.",
            DisableImpactEn = "Java won't update automatically.",
            PerformanceImpact = "Faible pour jusched, variable pour les applications Java.",
            PerformanceImpactEn = "Very low.",
            Recommendation = "jusched peut être désactivé si vous gérez les mises à jour manuellement.",
            RecommendationEn = "Keep enabled for security.",
            Tags = "java,jre,jdk,oracle,runtime,programming",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "Docker Desktop",
            Aliases = "Docker,Docker for Windows,com.docker",
            Publisher = "Docker Inc.",
            ExecutableNames = "Docker Desktop.exe,dockerd.exe,com.docker.backend.exe,com.docker.proxy.exe",
            Category = KnowledgeCategory.Utility,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Plateforme de conteneurisation.",
            ShortDescriptionEn = "Containerization platform.",
            FullDescription = "Docker Desktop permet de créer, déployer et exécuter des applications dans des conteneurs. Utilise WSL2 ou Hyper-V sur Windows.",
            FullDescriptionEn = "Docker Desktop allows you to create, deploy and run applications in containers. Uses WSL2 or Hyper-V on Windows.",
            DisableImpact = "Docker ne sera pas disponible au démarrage. Les conteneurs ne démarreront pas automatiquement.",
            DisableImpactEn = "Docker won't be available at startup. Containers won't start automatically.",
            PerformanceImpact = "Modéré à élevé (mémoire et CPU).",
            PerformanceImpactEn = "Moderate to high (memory and CPU).",
            Recommendation = "Désactivez du démarrage si vous n'utilisez pas Docker quotidiennement.",
            RecommendationEn = "Disable from startup if you don't use Docker daily.",
            Tags = "docker,container,virtualization,development,devops",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "Notepad++",
            Aliases = "NotepadPlusPlus,npp",
            Publisher = "Don HO",
            ExecutableNames = "notepad++.exe",
            Category = KnowledgeCategory.Productivity,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Éditeur de texte et de code avancé.",
            ShortDescriptionEn = "Source code editor.",
            FullDescription = "Notepad++ est un éditeur de texte et de code source gratuit avec coloration syntaxique, auto-complétion et support de plugins.",
            FullDescriptionEn = "Free source code editor and Notepad replacement.",
            DisableImpact = "Aucun - ne devrait pas démarrer automatiquement.",
            DisableImpactEn = "Auto-updater won't run.",
            PerformanceImpact = "Nul au repos.",
            PerformanceImpactEn = "Very low.",
            Recommendation = "Désactivez du démarrage automatique.",
            RecommendationEn = "Disable from startup.",
            Tags = "notepad,editor,text,code,programming",
            LastUpdated = DateTime.Now
        });

        // ============================================================
        // COMMUNICATION & COLLABORATION
        // ============================================================

        Save(new KnowledgeEntry
        {
            Name = "Slack",
            Aliases = "Slack Technologies",
            Publisher = "Slack Technologies, LLC",
            ExecutableNames = "slack.exe",
            Category = KnowledgeCategory.Communication,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Plateforme de messagerie professionnelle.",
            ShortDescriptionEn = "Professional messaging platform.",
            FullDescription = "Slack est une plateforme de communication d'équipe avec messagerie instantanée, canaux, appels et intégrations tierces.",
            FullDescriptionEn = "Slack is a team communication platform with instant messaging, channels, calls and third-party integrations.",
            DisableImpact = "Vous ne recevrez pas les notifications Slack au démarrage.",
            DisableImpactEn = "You won't receive Slack notifications at startup.",
            PerformanceImpact = "Modéré (application Electron).",
            PerformanceImpactEn = "Moderate (Electron app).",
            Recommendation = "Gardez actif si vous l'utilisez professionnellement.",
            RecommendationEn = "Keep enabled if you use it professionally.",
            Tags = "slack,messaging,team,communication,work,chat",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "Microsoft Teams",
            Aliases = "Teams,ms-teams",
            Publisher = "Microsoft Corporation",
            ExecutableNames = "Teams.exe,ms-teams.exe,msteams.exe",
            Category = KnowledgeCategory.Communication,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Plateforme de collaboration Microsoft.",
            ShortDescriptionEn = "Microsoft collaboration platform.",
            FullDescription = "Microsoft Teams combine messagerie, visioconférence, stockage de fichiers et intégration avec les apps Microsoft 365.",
            FullDescriptionEn = "Microsoft Teams combines messaging, video conferencing, file storage and integration with Microsoft 365 apps.",
            DisableImpact = "Vous ne recevrez pas les notifications Teams au démarrage.",
            DisableImpactEn = "You won't receive Teams notifications at startup.",
            PerformanceImpact = "Modéré à élevé.",
            PerformanceImpactEn = "Moderate to high.",
            Recommendation = "Gardez actif si vous l'utilisez professionnellement.",
            RecommendationEn = "Keep enabled if you use it professionally.",
            Tags = "teams,microsoft,communication,video,chat,office365",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "Zoom",
            Aliases = "Zoom Meetings,ZoomOpener",
            Publisher = "Zoom Video Communications, Inc.",
            ExecutableNames = "Zoom.exe,ZoomOpener.exe,ZoomOutlookIMPlugin.exe",
            Category = KnowledgeCategory.Communication,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Application de visioconférence.",
            ShortDescriptionEn = "Video conferencing application.",
            FullDescription = "Zoom est une plateforme de visioconférence populaire pour les réunions en ligne, webinaires et collaboration à distance.",
            FullDescriptionEn = "Zoom is a popular video conferencing platform for online meetings, webinars and remote collaboration.",
            DisableImpact = "Zoom ne démarrera pas automatiquement. Vous devrez le lancer manuellement.",
            DisableImpactEn = "Zoom won't start automatically. You'll need to launch it manually.",
            PerformanceImpact = "Faible au repos, élevé pendant les appels.",
            PerformanceImpactEn = "Low when idle, high during calls.",
            Recommendation = "Peut être désactivé du démarrage automatique.",
            RecommendationEn = "Can be disabled from automatic startup.",
            Tags = "zoom,video,conference,meeting,communication",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "WhatsApp Desktop",
            Aliases = "WhatsApp,WhatsApp for Windows",
            Publisher = "WhatsApp LLC",
            ExecutableNames = "WhatsApp.exe",
            Category = KnowledgeCategory.Communication,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Application de messagerie WhatsApp pour bureau.",
            ShortDescriptionEn = "WhatsApp desktop messaging application.",
            FullDescription = "Version desktop de WhatsApp permettant d'envoyer des messages, passer des appels et partager des fichiers depuis votre ordinateur.",
            FullDescriptionEn = "Desktop version of WhatsApp allowing you to send messages, make calls and share files from your computer.",
            DisableImpact = "Vous ne recevrez pas les notifications WhatsApp au démarrage.",
            DisableImpactEn = "You won't receive WhatsApp notifications at startup.",
            PerformanceImpact = "Faible à modéré.",
            PerformanceImpactEn = "Low to moderate.",
            Recommendation = "Selon vos préférences de communication.",
            RecommendationEn = "Depends on your communication preferences.",
            Tags = "whatsapp,messaging,chat,communication,meta",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "Telegram Desktop",
            Aliases = "Telegram,Telegram Messenger",
            Publisher = "Telegram FZ-LLC",
            ExecutableNames = "Telegram.exe,Updater.exe",
            Category = KnowledgeCategory.Communication,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Application de messagerie sécurisée.",
            ShortDescriptionEn = "Secure messaging application.",
            FullDescription = "Telegram est une application de messagerie instantanée axée sur la vitesse et la sécurité, avec support des groupes, canaux et bots.",
            FullDescriptionEn = "Telegram is an instant messaging app focused on speed and security, with support for groups, channels and bots.",
            DisableImpact = "Vous ne recevrez pas les notifications Telegram au démarrage.",
            DisableImpactEn = "You won't receive Telegram notifications at startup.",
            PerformanceImpact = "Faible.",
            PerformanceImpactEn = "Low.",
            Recommendation = "Selon vos préférences de communication.",
            RecommendationEn = "Depends on your communication preferences.",
            Tags = "telegram,messaging,chat,secure,communication",
            LastUpdated = DateTime.Now
        });

        // ============================================================
        // BROWSERS
        // ============================================================

        Save(new KnowledgeEntry
        {
            Name = "Mozilla Firefox",
            Aliases = "Firefox,Firefox Browser",
            Publisher = "Mozilla Corporation",
            ExecutableNames = "firefox.exe,updater.exe,pingsender.exe,default-browser-agent.exe",
            Category = KnowledgeCategory.Browser,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Navigateur web open-source de Mozilla.",
            ShortDescriptionEn = "Mozilla's open-source web browser.",
            FullDescription = "Firefox est un navigateur web rapide et respectueux de la vie privée développé par Mozilla. default-browser-agent vérifie le navigateur par défaut.",
            FullDescriptionEn = "Firefox is a fast and privacy-respecting web browser developed by Mozilla. default-browser-agent checks the default browser.",
            DisableImpact = "Firefox ne s'ouvrira pas automatiquement. L'agent de navigateur par défaut peut être désactivé.",
            DisableImpactEn = "Firefox won't open automatically. The default browser agent can be disabled.",
            PerformanceImpact = "Faible pour l'agent, modéré pour le navigateur.",
            PerformanceImpactEn = "Low for the agent, moderate for the browser.",
            Recommendation = "L'agent de navigateur par défaut peut être désactivé.",
            RecommendationEn = "The default browser agent can be disabled.",
            Tags = "firefox,browser,mozilla,web,internet,privacy",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "Mozilla Thunderbird",
            Aliases = "Thunderbird,Thunderbird Mail",
            Publisher = "Mozilla Corporation",
            ExecutableNames = "thunderbird.exe",
            Category = KnowledgeCategory.Communication,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Client email open-source de Mozilla.",
            ShortDescriptionEn = "Mozilla's open-source email client.",
            FullDescription = "Thunderbird est un client de messagerie gratuit et open-source avec support des emails, calendrier, contacts et flux RSS.",
            FullDescriptionEn = "Thunderbird is a free and open-source email client with support for emails, calendar, contacts and RSS feeds.",
            DisableImpact = "Vous ne recevrez pas les notifications email au démarrage.",
            DisableImpactEn = "You won't receive email notifications at startup.",
            PerformanceImpact = "Faible à modéré.",
            PerformanceImpactEn = "Low to moderate.",
            Recommendation = "Gardez actif si c'est votre client email principal.",
            RecommendationEn = "Keep enabled if this is your main email client.",
            Tags = "thunderbird,email,mail,mozilla,calendar",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "Opera Browser",
            Aliases = "Opera,Opera GX,opera_autoupdate",
            Publisher = "Opera Norway AS",
            ExecutableNames = "opera.exe,opera_autoupdate.exe,launcher.exe,opera_crashreporter.exe",
            Category = KnowledgeCategory.Browser,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Navigateur web avec VPN intégré.",
            ShortDescriptionEn = "Web browser with built-in VPN.",
            FullDescription = "Opera est un navigateur web avec VPN gratuit intégré, bloqueur de pubs, et fonctionnalités uniques comme les espaces de travail et Flow.",
            FullDescriptionEn = "Opera is a web browser with built-in free VPN, ad blocker, and unique features like workspaces and Flow.",
            DisableImpact = "Opera ne démarrera pas automatiquement.",
            DisableImpactEn = "Opera won't start automatically.",
            PerformanceImpact = "Faible pour l'updater.",
            PerformanceImpactEn = "Low for the updater.",
            Recommendation = "L'updater automatique peut être désactivé.",
            RecommendationEn = "The automatic updater can be disabled.",
            Tags = "opera,browser,vpn,web,internet",
            LastUpdated = DateTime.Now
        });

        // ============================================================
        // UTILITIES
        // ============================================================

        Save(new KnowledgeEntry
        {
            Name = "7-Zip",
            Aliases = "7z,SevenZip",
            Publisher = "Igor Pavlov",
            ExecutableNames = "7zFM.exe,7zG.exe,7z.exe",
            Category = KnowledgeCategory.Utility,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Archiveur de fichiers open-source.",
            ShortDescriptionEn = "Open-source file archiver.",
            FullDescription = "7-Zip est un utilitaire de compression de fichiers gratuit avec un taux de compression élevé et support de nombreux formats (7z, ZIP, RAR, etc.).",
            FullDescriptionEn = "7-Zip is a free file compression utility with high compression ratio and support for many formats (7z, ZIP, RAR, etc.).",
            DisableImpact = "Aucun - ne devrait pas démarrer automatiquement.",
            DisableImpactEn = "None - should not start automatically.",
            PerformanceImpact = "Nul au repos.",
            PerformanceImpactEn = "None when idle.",
            Recommendation = "Ne devrait pas être dans le démarrage automatique.",
            RecommendationEn = "Should not be in automatic startup.",
            Tags = "7zip,compression,archive,zip,rar,utility",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "WinRAR",
            Aliases = "RARLAB WinRAR",
            Publisher = "Alexander Roshal",
            ExecutableNames = "WinRAR.exe,Rar.exe,UnRAR.exe",
            Category = KnowledgeCategory.Utility,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Archiveur de fichiers RAR.",
            ShortDescriptionEn = "RAR file archiver.",
            FullDescription = "WinRAR est un utilitaire de compression populaire spécialisé dans le format RAR mais supportant aussi ZIP, 7Z et d'autres formats.",
            FullDescriptionEn = "WinRAR is a popular compression utility specialized in RAR format but also supporting ZIP, 7Z and other formats.",
            DisableImpact = "Aucun - ne devrait pas démarrer automatiquement.",
            DisableImpactEn = "None - should not start automatically.",
            PerformanceImpact = "Nul au repos.",
            PerformanceImpactEn = "None when idle.",
            Recommendation = "Ne devrait pas être dans le démarrage automatique.",
            RecommendationEn = "Should not be in automatic startup.",
            Tags = "winrar,compression,archive,rar,zip,utility",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "Everything Search",
            Aliases = "Everything,voidtools",
            Publisher = "voidtools",
            ExecutableNames = "Everything.exe,Everything64.exe",
            Category = KnowledgeCategory.Utility,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Moteur de recherche de fichiers ultra-rapide.",
            ShortDescriptionEn = "Ultra-fast file search engine.",
            FullDescription = "Everything indexe tous les fichiers NTFS et permet une recherche instantanée. Beaucoup plus rapide que la recherche Windows native.",
            FullDescriptionEn = "Everything indexes all NTFS files and enables instant search. Much faster than native Windows search.",
            DisableImpact = "L'index ne sera pas maintenu à jour. La recherche instantanée ne sera pas disponible.",
            DisableImpactEn = "Index won't be kept up to date. Instant search won't be available.",
            PerformanceImpact = "Très faible (index léger en mémoire).",
            PerformanceImpactEn = "Very low (lightweight index in memory).",
            Recommendation = "Gardez actif si vous l'utilisez régulièrement.",
            RecommendationEn = "Keep enabled if you use it regularly.",
            Tags = "everything,search,files,fast,index,utility",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "ShareX",
            Aliases = "ShareX Screen Capture",
            Publisher = "ShareX Team",
            ExecutableNames = "ShareX.exe",
            Category = KnowledgeCategory.Utility,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Outil de capture d'écran avancé.",
            ShortDescriptionEn = "Advanced screenshot tool.",
            FullDescription = "ShareX est un outil gratuit et open-source pour la capture d'écran, l'enregistrement vidéo et le partage de fichiers avec de nombreuses options.",
            FullDescriptionEn = "ShareX is a free and open-source tool for screenshots, video recording and file sharing with many options.",
            DisableImpact = "Les raccourcis de capture d'écran ne seront pas disponibles.",
            DisableImpactEn = "Screenshot shortcuts won't be available.",
            PerformanceImpact = "Faible.",
            PerformanceImpactEn = "Low.",
            Recommendation = "Gardez actif si vous faites des captures régulièrement.",
            RecommendationEn = "Keep enabled if you take screenshots regularly.",
            Tags = "sharex,screenshot,capture,screen,recording",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "Greenshot",
            Aliases = "Greenshot Screenshot",
            Publisher = "Greenshot",
            ExecutableNames = "Greenshot.exe",
            Category = KnowledgeCategory.Utility,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Outil de capture d'écran léger.",
            ShortDescriptionEn = "Lightweight screenshot tool.",
            FullDescription = "Greenshot est un outil de capture d'écran léger et gratuit avec éditeur intégré et options d'export multiples.",
            FullDescriptionEn = "Greenshot is a lightweight and free screenshot tool with built-in editor and multiple export options.",
            DisableImpact = "Les raccourcis de capture d'écran Greenshot ne seront pas disponibles.",
            DisableImpactEn = "Greenshot screenshot shortcuts won't be available.",
            PerformanceImpact = "Très faible.",
            PerformanceImpactEn = "Very low.",
            Recommendation = "Gardez actif si vous l'utilisez régulièrement.",
            RecommendationEn = "Keep enabled if you use it regularly.",
            Tags = "greenshot,screenshot,capture,screen,image",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "Microsoft PowerToys",
            Aliases = "PowerToys,PowerToys.exe",
            Publisher = "Microsoft Corporation",
            ExecutableNames = "PowerToys.exe,PowerToys.*.exe",
            Category = KnowledgeCategory.Utility,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Suite d'utilitaires Windows de Microsoft.",
            ShortDescriptionEn = "Microsoft Windows utilities suite.",
            FullDescription = "PowerToys est une collection d'utilitaires incluant FancyZones, PowerRename, Color Picker, et d'autres outils pour power users.",
            FullDescriptionEn = "PowerToys is a collection of utilities including FancyZones, PowerRename, Color Picker, and other tools for power users.",
            DisableImpact = "Les fonctionnalités PowerToys (FancyZones, etc.) ne seront pas disponibles.",
            DisableImpactEn = "PowerToys features (FancyZones, etc.) won't be available.",
            PerformanceImpact = "Faible à modéré selon les modules activés.",
            PerformanceImpactEn = "Low to moderate depending on enabled modules.",
            Recommendation = "Gardez actif si vous utilisez ses fonctionnalités.",
            RecommendationEn = "Keep enabled if you use its features.",
            Tags = "powertoys,microsoft,utility,productivity,windows",
            LastUpdated = DateTime.Now
        });

        // ============================================================
        // SECURITY & VPN
        // ============================================================

        Save(new KnowledgeEntry
        {
            Name = "KeePass",
            Aliases = "KeePass Password Safe,KeePassXC",
            Publisher = "Dominik Reichl",
            ExecutableNames = "KeePass.exe,KeePassXC.exe",
            Category = KnowledgeCategory.Security,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Gestionnaire de mots de passe open-source.",
            ShortDescriptionEn = "Open-source password manager.",
            FullDescription = "KeePass est un gestionnaire de mots de passe gratuit et open-source qui stocke vos mots de passe dans une base de données chiffrée.",
            FullDescriptionEn = "KeePass is a free and open-source password manager that stores your passwords in an encrypted database.",
            DisableImpact = "KeePass ne sera pas disponible immédiatement pour l'auto-remplissage.",
            DisableImpactEn = "KeePass won't be available immediately for auto-fill.",
            PerformanceImpact = "Très faible.",
            PerformanceImpactEn = "Very low.",
            Recommendation = "Gardez actif pour un accès facile aux mots de passe.",
            RecommendationEn = "Keep enabled for easy password access.",
            Tags = "keepass,password,manager,security,encryption",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "Bitwarden",
            Aliases = "Bitwarden Desktop",
            Publisher = "Bitwarden Inc.",
            ExecutableNames = "Bitwarden.exe",
            Category = KnowledgeCategory.Security,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Gestionnaire de mots de passe cloud.",
            ShortDescriptionEn = "Cloud password manager.",
            FullDescription = "Bitwarden est un gestionnaire de mots de passe open-source avec synchronisation cloud, disponible sur toutes les plateformes.",
            FullDescriptionEn = "Bitwarden is an open-source password manager with cloud synchronization, available on all platforms.",
            DisableImpact = "Bitwarden ne sera pas disponible pour l'auto-remplissage au démarrage.",
            DisableImpactEn = "Bitwarden won't be available for auto-fill at startup.",
            PerformanceImpact = "Faible.",
            PerformanceImpactEn = "Low.",
            Recommendation = "Gardez actif pour un accès facile aux mots de passe.",
            RecommendationEn = "Keep enabled for easy password access.",
            Tags = "bitwarden,password,manager,security,cloud",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "1Password",
            Aliases = "1Password 7,1Password 8",
            Publisher = "AgileBits Inc.",
            ExecutableNames = "1Password.exe,1Password-BrowserSupport.exe",
            Category = KnowledgeCategory.Security,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Gestionnaire de mots de passe premium.",
            ShortDescriptionEn = "Premium password manager.",
            FullDescription = "1Password est un gestionnaire de mots de passe populaire avec fonctionnalités avancées, partage familial/équipe et intégration navigateur.",
            FullDescriptionEn = "1Password is a popular password manager with advanced features, family/team sharing and browser integration.",
            DisableImpact = "1Password ne sera pas disponible pour l'auto-remplissage au démarrage.",
            DisableImpactEn = "1Password won't be available for auto-fill at startup.",
            PerformanceImpact = "Faible.",
            PerformanceImpactEn = "Low.",
            Recommendation = "Gardez actif pour un accès facile aux mots de passe.",
            RecommendationEn = "Keep enabled for easy password access.",
            Tags = "1password,password,manager,security,vault",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "NordVPN",
            Aliases = "Nord VPN,NordVPN Service",
            Publisher = "NORD SECURITY",
            ExecutableNames = "NordVPN.exe,nordvpn-service.exe,NordLynx.exe",
            Category = KnowledgeCategory.Security,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Service VPN commercial populaire.",
            ShortDescriptionEn = "Popular commercial VPN service.",
            FullDescription = "NordVPN est l'un des services VPN les plus populaires, offrant une connexion sécurisée et anonyme à Internet avec des serveurs dans le monde entier.",
            FullDescriptionEn = "NordVPN is one of the most popular VPN services, offering secure and anonymous Internet connection with servers worldwide.",
            DisableImpact = "La connexion VPN ne sera pas établie automatiquement.",
            DisableImpactEn = "VPN connection won't be established automatically.",
            PerformanceImpact = "Faible à modéré.",
            PerformanceImpactEn = "Low to moderate.",
            Recommendation = "Gardez actif si vous avez besoin d'une protection VPN permanente.",
            RecommendationEn = "Keep enabled if you need permanent VPN protection.",
            Tags = "nordvpn,vpn,security,privacy,network",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "ExpressVPN",
            Aliases = "Express VPN,expressvpnd",
            Publisher = "Express VPN International Ltd.",
            ExecutableNames = "ExpressVPN.exe,expressvpnd.exe",
            Category = KnowledgeCategory.Security,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Service VPN premium rapide.",
            ShortDescriptionEn = "Fast premium VPN service.",
            FullDescription = "ExpressVPN est un service VPN premium reconnu pour sa vitesse et sa fiabilité, avec des serveurs dans 94 pays.",
            FullDescriptionEn = "ExpressVPN is a premium VPN service recognized for its speed and reliability, with servers in 94 countries.",
            DisableImpact = "La connexion VPN ne sera pas établie automatiquement.",
            DisableImpactEn = "VPN connection won't be established automatically.",
            PerformanceImpact = "Faible à modéré.",
            PerformanceImpactEn = "Low to moderate.",
            Recommendation = "Gardez actif si vous avez besoin d'une protection VPN permanente.",
            RecommendationEn = "Keep enabled if you need permanent VPN protection.",
            Tags = "expressvpn,vpn,security,privacy,network",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "ProtonVPN",
            Aliases = "Proton VPN,ProtonVPN Service",
            Publisher = "Proton AG",
            ExecutableNames = "ProtonVPN.exe,protonvpn-service.exe",
            Category = KnowledgeCategory.Security,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "VPN sécurisé des créateurs de ProtonMail.",
            ShortDescriptionEn = "Secure VPN from ProtonMail creators.",
            FullDescription = "ProtonVPN est un service VPN suisse axé sur la vie privée, créé par les fondateurs de ProtonMail. Offre un niveau gratuit.",
            FullDescriptionEn = "ProtonVPN is a Swiss VPN service focused on privacy, created by the founders of ProtonMail. Offers a free tier.",
            DisableImpact = "La connexion VPN ne sera pas établie automatiquement.",
            DisableImpactEn = "VPN connection won't be established automatically.",
            PerformanceImpact = "Faible à modéré.",
            PerformanceImpactEn = "Low to moderate.",
            Recommendation = "Gardez actif si vous avez besoin d'une protection VPN permanente.",
            RecommendationEn = "Keep enabled if you need permanent VPN protection.",
            Tags = "protonvpn,vpn,security,privacy,swiss",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "WireGuard",
            Aliases = "WireGuard VPN",
            Publisher = "WireGuard LLC",
            ExecutableNames = "wireguard.exe,wg.exe",
            Category = KnowledgeCategory.Security,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Protocole VPN moderne et rapide.",
            ShortDescriptionEn = "Modern and fast VPN protocol.",
            FullDescription = "WireGuard est un protocole VPN moderne, simple et extrêmement performant. Open-source et intégré au noyau Linux.",
            FullDescriptionEn = "WireGuard is a modern, simple and extremely performant VPN protocol. Open-source and integrated into Linux kernel.",
            DisableImpact = "Le tunnel VPN WireGuard ne sera pas établi automatiquement.",
            DisableImpactEn = "WireGuard VPN tunnel won't be established automatically.",
            PerformanceImpact = "Très faible.",
            PerformanceImpactEn = "Very low.",
            Recommendation = "Gardez actif si vous utilisez WireGuard comme VPN principal.",
            RecommendationEn = "Keep enabled if you use WireGuard as main VPN.",
            Tags = "wireguard,vpn,security,tunnel,network,opensource",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "Tailscale",
            Aliases = "Tailscale VPN,tailscaled",
            Publisher = "Tailscale Inc.",
            ExecutableNames = "tailscale.exe,tailscaled.exe,tailscale-ipn.exe",
            Category = KnowledgeCategory.Utility,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "VPN mesh basé sur WireGuard.",
            ShortDescriptionEn = "Mesh VPN based on WireGuard.",
            FullDescription = "Tailscale crée un réseau privé mesh entre vos appareils en utilisant WireGuard. Simplifie la connexion entre machines distantes.",
            FullDescriptionEn = "Tailscale creates a private mesh network between your devices using WireGuard. Simplifies connection between remote machines.",
            DisableImpact = "Votre appareil ne sera pas connecté au réseau Tailscale.",
            DisableImpactEn = "Your device won't be connected to Tailscale network.",
            PerformanceImpact = "Très faible.",
            PerformanceImpactEn = "Very low.",
            Recommendation = "Gardez actif si vous utilisez Tailscale pour accéder à des ressources distantes.",
            RecommendationEn = "Keep enabled if you use Tailscale to access remote resources.",
            Tags = "tailscale,vpn,wireguard,mesh,network",
            LastUpdated = DateTime.Now
        });

        // ============================================================
        // REMOTE ACCESS
        // ============================================================

        Save(new KnowledgeEntry
        {
            Name = "TeamViewer",
            Aliases = "TeamViewer Host,TeamViewer Service",
            Publisher = "TeamViewer Germany GmbH",
            ExecutableNames = "TeamViewer.exe,TeamViewer_Service.exe,tv_w32.exe,tv_x64.exe",
            Category = KnowledgeCategory.Communication,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Logiciel de contrôle à distance.",
            ShortDescriptionEn = "Remote control software.",
            FullDescription = "TeamViewer permet le contrôle à distance d'ordinateurs, le transfert de fichiers et les réunions en ligne. Très utilisé pour le support technique.",
            FullDescriptionEn = "TeamViewer allows remote control of computers, file transfer and online meetings. Widely used for technical support.",
            DisableImpact = "L'accès à distance à cet ordinateur ne sera pas disponible.",
            DisableImpactEn = "Remote access to this computer won't be available.",
            PerformanceImpact = "Faible au repos.",
            PerformanceImpactEn = "Low when idle.",
            Recommendation = "Désactivez si vous n'avez pas besoin d'accès distant permanent.",
            RecommendationEn = "Disable if you don't need permanent remote access.",
            Tags = "teamviewer,remote,desktop,support,control",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "AnyDesk",
            Aliases = "AnyDesk Service",
            Publisher = "AnyDesk Software GmbH",
            ExecutableNames = "AnyDesk.exe,AnyDesk_Service.exe",
            Category = KnowledgeCategory.Communication,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Logiciel de bureau à distance léger.",
            ShortDescriptionEn = "Lightweight remote desktop software.",
            FullDescription = "AnyDesk est une alternative légère à TeamViewer pour le contrôle à distance avec une faible latence et une bonne qualité d'image.",
            FullDescriptionEn = "AnyDesk is a lightweight alternative to TeamViewer for remote control with low latency and good image quality.",
            DisableImpact = "L'accès à distance à cet ordinateur ne sera pas disponible.",
            DisableImpactEn = "Remote access to this computer won't be available.",
            PerformanceImpact = "Très faible au repos.",
            PerformanceImpactEn = "Very low when idle.",
            Recommendation = "Désactivez si vous n'avez pas besoin d'accès distant permanent.",
            RecommendationEn = "Disable if you don't need permanent remote access.",
            Tags = "anydesk,remote,desktop,control,support",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "Parsec",
            Aliases = "Parsec Gaming",
            Publisher = "Parsec Cloud, Inc.",
            ExecutableNames = "parsecd.exe,pservice.exe",
            Category = KnowledgeCategory.Communication,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Streaming de jeux et bureau à distance.",
            ShortDescriptionEn = "Game streaming and remote desktop.",
            FullDescription = "Parsec permet le streaming de jeux à distance avec une latence très faible. Idéal pour jouer sur un PC distant ou le cloud gaming.",
            FullDescriptionEn = "Parsec allows remote game streaming with very low latency. Ideal for playing on a remote PC or cloud gaming.",
            DisableImpact = "Le streaming Parsec ne sera pas disponible.",
            DisableImpactEn = "Parsec streaming won't be available.",
            PerformanceImpact = "Faible au repos, élevé pendant le streaming.",
            PerformanceImpactEn = "Low when idle, high during streaming.",
            Recommendation = "Désactivez si vous ne l'utilisez pas régulièrement.",
            RecommendationEn = "Disable if you don't use it regularly.",
            Tags = "parsec,gaming,streaming,remote,lowlatency",
            LastUpdated = DateTime.Now
        });

        // ============================================================
        // GAMING PERIPHERALS
        // ============================================================

        Save(new KnowledgeEntry
        {
            Name = "Logitech G HUB",
            Aliases = "LGHUB,Logitech G Hub,lghub_agent",
            Publisher = "Logitech",
            ExecutableNames = "lghub.exe,lghub_agent.exe,lghub_updater.exe,lghub_system_tray.exe",
            Category = KnowledgeCategory.Hardware,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Gestionnaire de périphériques gaming Logitech.",
            ShortDescriptionEn = "Logitech gaming peripheral manager.",
            FullDescription = "Logitech G HUB gère les périphériques gaming Logitech (souris, claviers, casques). Configure l'éclairage RGB, macros et profils.",
            FullDescriptionEn = "Logitech G HUB manages Logitech gaming peripherals (mice, keyboards, headsets). Configures RGB lighting, macros and profiles.",
            DisableImpact = "Les profils et l'éclairage personnalisés ne seront pas chargés.",
            DisableImpactEn = "Custom profiles and lighting won't be loaded.",
            PerformanceImpact = "Modéré.",
            PerformanceImpactEn = "Moderate.",
            Recommendation = "Gardez actif si vous utilisez des périphériques Logitech G.",
            RecommendationEn = "Keep enabled if you use Logitech G peripherals.",
            Tags = "logitech,ghub,gaming,mouse,keyboard,rgb",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "Logitech Options",
            Aliases = "Logi Options,Logi Options+,LogiOptions",
            Publisher = "Logitech",
            ExecutableNames = "LogiOptions.exe,LogiOptionsMgr.exe,logioptionsplus_agent.exe",
            Category = KnowledgeCategory.Hardware,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Gestionnaire de périphériques Logitech.",
            ShortDescriptionEn = "Logitech peripheral manager.",
            FullDescription = "Logitech Options configure les périphériques Logitech non-gaming (souris MX, claviers, webcams). Gère les gestes, boutons et paramètres.",
            FullDescriptionEn = "Logitech Options configures non-gaming Logitech peripherals (MX mice, keyboards, webcams). Manages gestures, buttons and settings.",
            DisableImpact = "Les configurations personnalisées des périphériques ne seront pas appliquées.",
            DisableImpactEn = "Custom peripheral configurations won't be applied.",
            PerformanceImpact = "Faible.",
            PerformanceImpactEn = "Low.",
            Recommendation = "Gardez actif si vous utilisez des périphériques Logitech avec des configurations personnalisées.",
            RecommendationEn = "Keep enabled if you use Logitech peripherals with custom configurations.",
            Tags = "logitech,options,mouse,keyboard,productivity",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "Corsair iCUE",
            Aliases = "iCUE,Corsair Utility Engine,CorsairGamingAudioConfig",
            Publisher = "Corsair",
            ExecutableNames = "iCUE.exe,Corsair.Service.exe,CorsairGamingAudioConfig*.exe",
            Category = KnowledgeCategory.Hardware,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Gestionnaire de périphériques Corsair.",
            ShortDescriptionEn = "Corsair peripheral manager.",
            FullDescription = "Corsair iCUE contrôle l'éclairage RGB, les performances et les macros de tous les périphériques Corsair (claviers, souris, casques, RAM, ventilateurs).",
            FullDescriptionEn = "Corsair iCUE controls RGB lighting, performance and macros for all Corsair peripherals (keyboards, mice, headsets, RAM, fans).",
            DisableImpact = "L'éclairage RGB et les profils personnalisés ne seront pas chargés.",
            DisableImpactEn = "RGB lighting and custom profiles won't be loaded.",
            PerformanceImpact = "Modéré à élevé.",
            PerformanceImpactEn = "Moderate to high.",
            Recommendation = "Gardez actif si vous avez des périphériques Corsair RGB.",
            RecommendationEn = "Keep enabled if you have Corsair RGB peripherals.",
            Tags = "corsair,icue,rgb,gaming,keyboard,mouse,cooling",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "Razer Synapse",
            Aliases = "Synapse,Razer Synapse 3,RazerCentralService",
            Publisher = "Razer Inc.",
            ExecutableNames = "Razer Synapse.exe,RazerCentralService.exe,RzSynapse.exe,Razer Synapse Service.exe",
            Category = KnowledgeCategory.Hardware,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Gestionnaire de périphériques Razer.",
            ShortDescriptionEn = "Razer peripheral manager.",
            FullDescription = "Razer Synapse configure l'éclairage Chroma RGB, les macros et profils pour tous les périphériques Razer. Synchronise les paramètres dans le cloud.",
            FullDescriptionEn = "Razer Synapse configures Chroma RGB lighting, macros and profiles for all Razer peripherals. Syncs settings to the cloud.",
            DisableImpact = "L'éclairage Chroma et les profils ne seront pas chargés.",
            DisableImpactEn = "Chroma lighting and profiles won't be loaded.",
            PerformanceImpact = "Modéré à élevé.",
            PerformanceImpactEn = "Moderate to high.",
            Recommendation = "Gardez actif si vous avez des périphériques Razer.",
            RecommendationEn = "Keep enabled if you have Razer peripherals.",
            Tags = "razer,synapse,chroma,rgb,gaming,mouse,keyboard",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "SteelSeries GG",
            Aliases = "SteelSeries Engine,SteelSeriesGG,SteelSeriesEngine3",
            Publisher = "SteelSeries ApS",
            ExecutableNames = "SteelSeriesGG.exe,SteelSeriesEngine3.exe,SteelSeriesGGClient.exe",
            Category = KnowledgeCategory.Hardware,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Gestionnaire de périphériques SteelSeries.",
            ShortDescriptionEn = "SteelSeries peripheral manager.",
            FullDescription = "SteelSeries GG configure les périphériques SteelSeries (casques, souris, claviers). Inclut Moments pour les clips gaming et Sonar pour l'audio.",
            FullDescriptionEn = "SteelSeries GG configures SteelSeries peripherals (headsets, mice, keyboards). Includes Moments for gaming clips and Sonar for audio.",
            DisableImpact = "L'éclairage et les profils personnalisés ne seront pas chargés.",
            DisableImpactEn = "Lighting and custom profiles won't be loaded.",
            PerformanceImpact = "Modéré.",
            PerformanceImpactEn = "Moderate.",
            Recommendation = "Gardez actif si vous avez des périphériques SteelSeries.",
            RecommendationEn = "Keep enabled if you have SteelSeries peripherals.",
            Tags = "steelseries,gg,gaming,headset,mouse,keyboard",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "HyperX NGENUITY",
            Aliases = "NGENUITY,HyperX Ngenuity",
            Publisher = "HP Inc.",
            ExecutableNames = "HyperX NGENUITY.exe,NGenuity.exe",
            Category = KnowledgeCategory.Hardware,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Gestionnaire de périphériques HyperX.",
            ShortDescriptionEn = "HyperX peripheral manager.",
            FullDescription = "HyperX NGENUITY configure les périphériques HyperX (claviers, casques, souris). Gère l'éclairage RGB, les macros et l'égaliseur audio.",
            FullDescriptionEn = "HyperX NGENUITY configures HyperX peripherals (keyboards, headsets, mice). Manages RGB lighting, macros and audio equalizer.",
            DisableImpact = "Les profils et l'éclairage personnalisés ne seront pas chargés.",
            DisableImpactEn = "Custom profiles and lighting won't be loaded.",
            PerformanceImpact = "Faible à modéré.",
            PerformanceImpactEn = "Low to moderate.",
            Recommendation = "Gardez actif si vous avez des périphériques HyperX.",
            RecommendationEn = "Keep enabled if you have HyperX peripherals.",
            Tags = "hyperx,ngenuity,gaming,keyboard,headset,rgb",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "Elgato Stream Deck",
            Aliases = "Stream Deck,Elgato StreamDeck",
            Publisher = "Corsair Memory, Inc.",
            ExecutableNames = "StreamDeck.exe",
            Category = KnowledgeCategory.Hardware,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Gestionnaire de Stream Deck Elgato.",
            ShortDescriptionEn = "Elgato Stream Deck manager.",
            FullDescription = "Elgato Stream Deck configure le Stream Deck, un panneau de contrôle avec touches LCD personnalisables pour le streaming et la productivité.",
            FullDescriptionEn = "Elgato Stream Deck configures the Stream Deck, a control panel with customizable LCD keys for streaming and productivity.",
            DisableImpact = "Le Stream Deck ne fonctionnera pas correctement.",
            DisableImpactEn = "Stream Deck won't work correctly.",
            PerformanceImpact = "Faible.",
            PerformanceImpactEn = "Low.",
            Recommendation = "Gardez actif si vous utilisez un Stream Deck.",
            RecommendationEn = "Keep enabled if you use a Stream Deck.",
            Tags = "elgato,streamdeck,streaming,macro,buttons",
            LastUpdated = DateTime.Now
        });

        // ============================================================
        // AUDIO
        // ============================================================

        Save(new KnowledgeEntry
        {
            Name = "Voicemeeter",
            Aliases = "VB-Audio Voicemeeter,Voicemeeter Banana,Voicemeeter Potato",
            Publisher = "VB-Audio Software",
            ExecutableNames = "voicemeeter.exe,voicemeeter8.exe,voicemeeterpro.exe",
            Category = KnowledgeCategory.Media,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Mixeur audio virtuel.",
            ShortDescriptionEn = "Virtual audio mixer.",
            FullDescription = "Voicemeeter est un mixeur audio virtuel permettant de router et mixer plusieurs sources audio. Populaire chez les streamers et podcasteurs.",
            FullDescriptionEn = "Voicemeeter is a virtual audio mixer allowing to route and mix multiple audio sources. Popular among streamers and podcasters.",
            DisableImpact = "Le routage audio virtuel ne sera pas disponible.",
            DisableImpactEn = "Virtual audio routing won't be available.",
            PerformanceImpact = "Faible à modéré.",
            PerformanceImpactEn = "Low to moderate.",
            Recommendation = "Gardez actif si vous l'utilisez pour le streaming ou le podcast.",
            RecommendationEn = "Keep enabled if you use it for streaming or podcast.",
            Tags = "voicemeeter,audio,mixer,virtual,streaming,podcast",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "Focusrite Control",
            Aliases = "Focusrite Notifier,Focusrite Control 2",
            Publisher = "Focusrite Audio Engineering Ltd.",
            ExecutableNames = "Focusrite Control*.exe,Focusrite Notifier.exe",
            Category = KnowledgeCategory.Media,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Contrôleur d'interface audio Focusrite.",
            ShortDescriptionEn = "Focusrite audio interface controller.",
            FullDescription = "Focusrite Control configure les interfaces audio Focusrite Scarlett et Clarett. Gère le routage, le monitoring et les paramètres de préampli.",
            FullDescriptionEn = "Focusrite Control configures Focusrite Scarlett and Clarett audio interfaces. Manages routing, monitoring and preamp settings.",
            DisableImpact = "L'interface Focusrite utilisera les paramètres par défaut.",
            DisableImpactEn = "Focusrite interface will use default settings.",
            PerformanceImpact = "Très faible.",
            PerformanceImpactEn = "Very low.",
            Recommendation = "Gardez actif si vous utilisez une interface Focusrite.",
            RecommendationEn = "Keep enabled if you use a Focusrite interface.",
            Tags = "focusrite,audio,interface,scarlett,recording",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "Dolby Access",
            Aliases = "Dolby Atmos,DolbyDAX2API",
            Publisher = "Dolby Laboratories",
            ExecutableNames = "DolbyDAX2API.exe,DolbyDAX3*.exe",
            Category = KnowledgeCategory.Media,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Gestionnaire audio Dolby Atmos.",
            ShortDescriptionEn = "Dolby Atmos audio manager.",
            FullDescription = "Dolby Access active et configure Dolby Atmos pour casque et home cinéma. Offre un son surround immersif pour les jeux et films.",
            FullDescriptionEn = "Dolby Access enables and configures Dolby Atmos for headphones and home theater. Offers immersive surround sound for games and movies.",
            DisableImpact = "Dolby Atmos ne sera pas disponible.",
            DisableImpactEn = "Dolby Atmos won't be available.",
            PerformanceImpact = "Faible.",
            PerformanceImpactEn = "Low.",
            Recommendation = "Gardez actif si vous utilisez Dolby Atmos.",
            RecommendationEn = "Keep enabled if you use Dolby Atmos.",
            Tags = "dolby,atmos,audio,surround,spatial",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "Nahimic",
            Aliases = "Nahimic Service,A-Volute,NahimicSvc",
            Publisher = "A-Volute",
            ExecutableNames = "NahimicSvc64.exe,NahimicSvc32.exe,Nahimic*.exe",
            Category = KnowledgeCategory.Media,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Suite audio gaming.",
            ShortDescriptionEn = "Gaming audio suite.",
            FullDescription = "Nahimic est un logiciel audio gaming pré-installé sur certains PC (MSI, ASUS). Offre des améliorations audio 3D et des effets pour les jeux.",
            FullDescriptionEn = "Nahimic is gaming audio software pre-installed on some PCs (MSI, ASUS). Offers 3D audio enhancements and effects for games.",
            DisableImpact = "Les améliorations audio Nahimic ne seront pas actives.",
            DisableImpactEn = "Nahimic audio enhancements won't be active.",
            PerformanceImpact = "Faible.",
            PerformanceImpactEn = "Low.",
            Recommendation = "Peut être désactivé si vous n'utilisez pas ses fonctionnalités.",
            RecommendationEn = "Can be disabled if you don't use its features.",
            Tags = "nahimic,audio,gaming,3d,enhancement",
            LastUpdated = DateTime.Now
        });

        // ============================================================
        // VIRTUALIZATION
        // ============================================================

        Save(new KnowledgeEntry
        {
            Name = "VirtualBox",
            Aliases = "Oracle VM VirtualBox,VBoxSVC,VBoxHeadless",
            Publisher = "Oracle Corporation",
            ExecutableNames = "VirtualBox.exe,VBoxSVC.exe,VBoxHeadless.exe,VBoxManage.exe",
            Category = KnowledgeCategory.Utility,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Logiciel de virtualisation open-source.",
            ShortDescriptionEn = "Open-source virtualization software.",
            FullDescription = "VirtualBox permet de créer et exécuter des machines virtuelles. Supporte Windows, Linux, macOS et d'autres systèmes en tant qu'invités.",
            FullDescriptionEn = "VirtualBox allows creating and running virtual machines. Supports Windows, Linux, macOS and other systems as guests.",
            DisableImpact = "Les VMs ne démarreront pas automatiquement.",
            DisableImpactEn = "VMs will not start automatically.",
            PerformanceImpact = "Élevé pendant l'exécution de VMs.",
            PerformanceImpactEn = "High while running VMs.",
            Recommendation = "Désactivez du démarrage sauf si vous avez besoin de VMs au boot.",
            RecommendationEn = "Disable from startup unless you need VMs at boot.",
            Tags = "virtualbox,oracle,vm,virtualization,opensource",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "VMware Workstation",
            Aliases = "VMware Player,VMware Workstation Pro,vmware-authd",
            Publisher = "Broadcom Inc.",
            ExecutableNames = "vmware.exe,vmware-authd.exe,vmware-vmx.exe,vmnat.exe",
            Category = KnowledgeCategory.Utility,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Logiciel de virtualisation professionnel.",
            ShortDescriptionEn = "Professional virtualization software.",
            FullDescription = "VMware Workstation est une solution de virtualisation professionnelle pour exécuter plusieurs systèmes d'exploitation sur un seul PC.",
            FullDescriptionEn = "VMware Workstation is a professional virtualization solution to run multiple operating systems on a single PC.",
            DisableImpact = "Les services VMware ne seront pas disponibles immédiatement.",
            DisableImpactEn = "VMware services will not be available immediately.",
            PerformanceImpact = "Élevé pendant l'exécution de VMs.",
            PerformanceImpactEn = "High while running VMs.",
            Recommendation = "Les services peuvent démarrer automatiquement si nécessaire.",
            RecommendationEn = "Services can start automatically if needed.",
            Tags = "vmware,workstation,vm,virtualization,professional",
            LastUpdated = DateTime.Now
        });

        // ============================================================
        // FILE TRANSFER
        // ============================================================

        Save(new KnowledgeEntry
        {
            Name = "FileZilla",
            Aliases = "FileZilla Client,FileZilla FTP",
            Publisher = "FileZilla Project",
            ExecutableNames = "filezilla.exe",
            Category = KnowledgeCategory.Utility,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Client FTP open-source.",
            ShortDescriptionEn = "Open-source FTP client.",
            FullDescription = "FileZilla est un client FTP/SFTP/FTPS gratuit et open-source pour transférer des fichiers entre votre ordinateur et un serveur.",
            FullDescriptionEn = "FileZilla is a free and open-source FTP/SFTP/FTPS client for transferring files between your computer and a server.",
            DisableImpact = "Aucun - ne devrait pas démarrer automatiquement.",
            DisableImpactEn = "None - shouldn't start automatically.",
            PerformanceImpact = "Nul au repos.",
            PerformanceImpactEn = "Zero when idle.",
            Recommendation = "Ne devrait pas être dans le démarrage automatique.",
            RecommendationEn = "Should not be in automatic startup.",
            Tags = "filezilla,ftp,sftp,transfer,files,opensource",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "WinSCP",
            Aliases = "WinSCP SFTP",
            Publisher = "Martin Prikryl",
            ExecutableNames = "WinSCP.exe",
            Category = KnowledgeCategory.Utility,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Client SFTP/SCP pour Windows.",
            ShortDescriptionEn = "SFTP/SCP client for Windows.",
            FullDescription = "WinSCP est un client SFTP/SCP/FTP gratuit avec une interface graphique pour transférer des fichiers de manière sécurisée.",
            FullDescriptionEn = "WinSCP is a free SFTP/SCP/FTP client with a graphical interface for secure file transfer.",
            DisableImpact = "Aucun - ne devrait pas démarrer automatiquement.",
            DisableImpactEn = "None - shouldn't start automatically.",
            PerformanceImpact = "Nul au repos.",
            PerformanceImpactEn = "Zero when idle.",
            Recommendation = "Ne devrait pas être dans le démarrage automatique.",
            RecommendationEn = "Should not be in automatic startup.",
            Tags = "winscp,sftp,scp,transfer,secure,files",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "PuTTY",
            Aliases = "PuTTY SSH,pageant",
            Publisher = "Simon Tatham",
            ExecutableNames = "putty.exe,pageant.exe,plink.exe,pscp.exe",
            Category = KnowledgeCategory.Utility,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Client SSH/Telnet pour Windows.",
            ShortDescriptionEn = "SSH/Telnet client for Windows.",
            FullDescription = "PuTTY est un émulateur de terminal et client SSH gratuit. Pageant est l'agent d'authentification SSH associé.",
            FullDescriptionEn = "PuTTY is a free terminal emulator and SSH client. Pageant is the associated SSH authentication agent.",
            DisableImpact = "Pageant ne chargera pas les clés SSH automatiquement.",
            DisableImpactEn = "Pageant won't load SSH keys automatically.",
            PerformanceImpact = "Très faible.",
            PerformanceImpactEn = "Very low.",
            Recommendation = "Gardez Pageant actif si vous utilisez l'authentification par clé SSH.",
            RecommendationEn = "Keep Pageant enabled if you use SSH key authentication.",
            Tags = "putty,ssh,telnet,terminal,pageant,keys",
            LastUpdated = DateTime.Now
        });

        // ============================================================
        // CLOUD SYNC
        // ============================================================

        Save(new KnowledgeEntry
        {
            Name = "Dropbox",
            Aliases = "Dropbox Update,DropboxClient",
            Publisher = "Dropbox, Inc.",
            ExecutableNames = "Dropbox.exe,DropboxUpdate.exe",
            Category = KnowledgeCategory.CloudStorage,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Service de stockage cloud.",
            ShortDescriptionEn = "Cloud storage service.",
            FullDescription = "Dropbox synchronise vos fichiers dans le cloud et les rend accessibles sur tous vos appareils. Offre partage et collaboration.",
            FullDescriptionEn = "Dropbox syncs your files to the cloud and makes them accessible on all your devices. Offers sharing and collaboration.",
            DisableImpact = "Vos fichiers ne seront pas synchronisés automatiquement.",
            DisableImpactEn = "Your files won't be synced automatically.",
            PerformanceImpact = "Faible au repos, modéré lors de la synchronisation.",
            PerformanceImpactEn = "Low when idle, moderate during sync.",
            Recommendation = "Gardez actif si vous utilisez Dropbox régulièrement.",
            RecommendationEn = "Keep enabled if you use Dropbox regularly.",
            Tags = "dropbox,cloud,sync,storage,backup",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "Box Drive",
            Aliases = "Box,Box Sync",
            Publisher = "Box, Inc.",
            ExecutableNames = "Box.exe,BoxUI.exe",
            Category = KnowledgeCategory.CloudStorage,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Service de stockage cloud entreprise.",
            ShortDescriptionEn = "Enterprise cloud storage service.",
            FullDescription = "Box Drive synchronise les fichiers Box avec votre ordinateur. Populaire dans les environnements professionnels pour la collaboration.",
            FullDescriptionEn = "Box Drive syncs Box files with your computer. Popular in professional environments for collaboration.",
            DisableImpact = "Les fichiers Box ne seront pas accessibles localement.",
            DisableImpactEn = "Box files won't be accessible locally.",
            PerformanceImpact = "Faible au repos.",
            PerformanceImpactEn = "Low when idle.",
            Recommendation = "Gardez actif si vous utilisez Box pour le travail.",
            RecommendationEn = "Keep enabled if you use Box for work.",
            Tags = "box,cloud,sync,enterprise,storage",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "iCloud for Windows",
            Aliases = "iCloud Drive,iCloud Photos,ApplePhotoStreams",
            Publisher = "Apple Inc.",
            ExecutableNames = "iCloudDrive.exe,iCloud.exe,ApplePhotoStreams.exe,iCloudPhotos.exe",
            Category = KnowledgeCategory.CloudStorage,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Client iCloud pour Windows.",
            ShortDescriptionEn = "iCloud client for Windows.",
            FullDescription = "iCloud pour Windows synchronise vos photos, documents, signets et mots de passe iCloud avec votre PC Windows.",
            FullDescriptionEn = "iCloud for Windows syncs your iCloud photos, documents, bookmarks and passwords with your Windows PC.",
            DisableImpact = "Les fichiers iCloud ne seront pas synchronisés.",
            DisableImpactEn = "iCloud files won't be synced.",
            PerformanceImpact = "Faible à modéré.",
            PerformanceImpactEn = "Low to moderate.",
            Recommendation = "Gardez actif si vous utilisez l'écosystème Apple.",
            RecommendationEn = "Keep enabled if you use the Apple ecosystem.",
            Tags = "icloud,apple,cloud,sync,photos,drive",
            LastUpdated = DateTime.Now
        });

        // ============================================================
        // BACKUP SOFTWARE
        // ============================================================

        Save(new KnowledgeEntry
        {
            Name = "Acronis True Image",
            Aliases = "Acronis Backup,Acronis Cyber Protect",
            Publisher = "Acronis",
            ExecutableNames = "TrueImage*.exe,AcronisTrueImage*.exe,schedmgr.exe",
            Category = KnowledgeCategory.Utility,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Logiciel de sauvegarde et récupération.",
            ShortDescriptionEn = "Backup and recovery software.",
            FullDescription = "Acronis True Image offre une sauvegarde complète du système, la création d'images disque et la synchronisation cloud.",
            FullDescriptionEn = "Acronis True Image offers full system backup, disk imaging and cloud synchronization.",
            DisableImpact = "Les sauvegardes programmées ne s'exécuteront pas.",
            DisableImpactEn = "Scheduled backups won't run.",
            PerformanceImpact = "Faible au repos, élevé pendant les sauvegardes.",
            PerformanceImpactEn = "Low when idle, high during backups.",
            Recommendation = "Gardez actif pour maintenir vos sauvegardes à jour.",
            RecommendationEn = "Keep enabled to keep your backups up to date.",
            Tags = "acronis,backup,image,recovery,clone",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "Veeam Agent",
            Aliases = "Veeam Backup,Veeam Agent for Windows",
            Publisher = "Veeam Software",
            ExecutableNames = "Veeam.EndPoint*.exe,VeeamAgent*.exe",
            Category = KnowledgeCategory.Utility,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Agent de sauvegarde Veeam.",
            ShortDescriptionEn = "Veeam backup agent.",
            FullDescription = "Veeam Agent for Windows offre une sauvegarde gratuite au niveau de l'image pour les postes de travail et serveurs Windows.",
            FullDescriptionEn = "Veeam Agent for Windows offers image-level backup for Windows workstations and servers.",
            DisableImpact = "Les sauvegardes programmées ne s'exécuteront pas.",
            DisableImpactEn = "Scheduled backups won't run.",
            PerformanceImpact = "Faible au repos, élevé pendant les sauvegardes.",
            PerformanceImpactEn = "Low when idle, high during backups.",
            Recommendation = "Gardez actif pour maintenir vos sauvegardes à jour.",
            RecommendationEn = "Keep enabled to keep your backups up to date.",
            Tags = "veeam,backup,image,recovery,enterprise",
            LastUpdated = DateTime.Now
        });

        // ============================================================
        // GAMING PLATFORMS
        // ============================================================

        Save(new KnowledgeEntry
        {
            Name = "GOG Galaxy",
            Aliases = "GOG Galaxy 2.0,GalaxyClient",
            Publisher = "GOG sp. z o.o.",
            ExecutableNames = "GalaxyClient.exe,GOG Galaxy.exe,GalaxyClientService.exe",
            Category = KnowledgeCategory.Gaming,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Client gaming GOG sans DRM.",
            ShortDescriptionEn = "DRM-free gaming client.",
            FullDescription = "GOG Galaxy est le client de la plateforme GOG.com, offrant des jeux sans DRM et l'intégration avec d'autres bibliothèques de jeux.",
            FullDescriptionEn = "GOG Galaxy is the client for the GOG.com platform, offering DRM-free games and integration with other game libraries.",
            DisableImpact = "GOG Galaxy ne démarrera pas automatiquement.",
            DisableImpactEn = "GOG Galaxy won't start automatically.",
            PerformanceImpact = "Faible à modéré.",
            PerformanceImpactEn = "Low to moderate.",
            Recommendation = "Peut être désactivé du démarrage automatique.",
            RecommendationEn = "Can be disabled from automatic startup.",
            Tags = "gog,galaxy,gaming,drm-free,launcher",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "Ubisoft Connect",
            Aliases = "Uplay,UbisoftConnect,UbisoftGameLauncher",
            Publisher = "Ubisoft Entertainment",
            ExecutableNames = "UbisoftConnect.exe,upc.exe,UbisoftGameLauncher.exe",
            Category = KnowledgeCategory.Gaming,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Plateforme de jeux Ubisoft.",
            ShortDescriptionEn = "Ubisoft gaming platform.",
            FullDescription = "Ubisoft Connect est la plateforme de jeux et de services d'Ubisoft, remplaçant Uplay. Gère les jeux, récompenses et amis.",
            FullDescriptionEn = "Ubisoft Connect is Ubisoft's platform for games and services, replacing Uplay. Manages games, rewards and friends.",
            DisableImpact = "Ubisoft Connect ne démarrera pas automatiquement.",
            DisableImpactEn = "Ubisoft Connect won't start automatically.",
            PerformanceImpact = "Modéré.",
            PerformanceImpactEn = "Moderate.",
            Recommendation = "Peut être désactivé du démarrage automatique.",
            RecommendationEn = "Can be disabled from automatic startup.",
            Tags = "ubisoft,uplay,gaming,launcher,connect",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "Rockstar Games Launcher",
            Aliases = "Rockstar Launcher,RockstarService",
            Publisher = "Rockstar Games",
            ExecutableNames = "Launcher.exe,RockstarService.exe,SocialClubHelper.exe",
            Category = KnowledgeCategory.Gaming,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Lanceur de jeux Rockstar.",
            ShortDescriptionEn = "Rockstar games launcher.",
            FullDescription = "Le Rockstar Games Launcher est nécessaire pour jouer aux jeux Rockstar (GTA, Red Dead Redemption) sur PC.",
            FullDescriptionEn = "The Rockstar Games Launcher is required to play Rockstar games (GTA, Red Dead Redemption) on PC.",
            DisableImpact = "Le lanceur ne démarrera pas automatiquement.",
            DisableImpactEn = "The launcher won't start automatically.",
            PerformanceImpact = "Faible.",
            PerformanceImpactEn = "Low.",
            Recommendation = "Peut être désactivé du démarrage automatique.",
            RecommendationEn = "Can be disabled from automatic startup.",
            Tags = "rockstar,gta,gaming,launcher,rdr",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "Amazon Games",
            Aliases = "Amazon Games App,Amazon Game Studios",
            Publisher = "Amazon.com Services LLC",
            ExecutableNames = "Amazon Games.exe,Amazon Games UI.exe",
            Category = KnowledgeCategory.Gaming,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Client gaming Amazon.",
            ShortDescriptionEn = "Amazon gaming client.",
            FullDescription = "Amazon Games est le client pour les jeux Amazon, les jeux Prime Gaming gratuits et les jeux New World et Lost Ark.",
            FullDescriptionEn = "Amazon Games is the client for Amazon games, free Prime Gaming games and New World and Lost Ark.",
            DisableImpact = "Amazon Games ne démarrera pas automatiquement.",
            DisableImpactEn = "Amazon Games won't start automatically.",
            PerformanceImpact = "Faible à modéré.",
            PerformanceImpactEn = "Low to moderate.",
            Recommendation = "Peut être désactivé du démarrage automatique.",
            RecommendationEn = "Can be disabled from automatic startup.",
            Tags = "amazon,gaming,prime,launcher,twitch",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "Bethesda.net Launcher",
            Aliases = "Bethesda Launcher,BethesdaNetLauncher",
            Publisher = "Bethesda Softworks",
            ExecutableNames = "BethesdaNetLauncher.exe,BethesdaNetUpdater.exe",
            Category = KnowledgeCategory.Gaming,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Lanceur de jeux Bethesda (obsolète).",
            ShortDescriptionEn = "Bethesda games launcher (deprecated).",
            FullDescription = "Le Bethesda.net Launcher était le client pour les jeux Bethesda. Il a été abandonné et migré vers Steam en 2022.",
            FullDescriptionEn = "The Bethesda.net Launcher was the client for Bethesda games. It has been discontinued and migrated to Steam in 2022.",
            DisableImpact = "Le lanceur ne démarrera pas automatiquement.",
            DisableImpactEn = "The launcher won't start automatically.",
            PerformanceImpact = "Faible.",
            PerformanceImpactEn = "Low.",
            Recommendation = "Peut être désinstallé - les jeux sont maintenant sur Steam.",
            RecommendationEn = "Can be uninstalled - games are now on Steam.",
            Tags = "bethesda,gaming,launcher,legacy,fallout,elder scrolls",
            LastUpdated = DateTime.Now
        });

        // ============================================================
        // SYSTEM OPTIMIZATION
        // ============================================================

        Save(new KnowledgeEntry
        {
            Name = "CCleaner",
            Aliases = "Piriform CCleaner,CCleaner Browser Monitor",
            Publisher = "Piriform Software Ltd",
            ExecutableNames = "CCleaner.exe,CCleaner64.exe,CCleanerBrowserMonitor.exe",
            Category = KnowledgeCategory.Utility,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Utilitaire de nettoyage système.",
            ShortDescriptionEn = "System cleaning utility.",
            FullDescription = "CCleaner nettoie les fichiers temporaires, le registre et les données de navigation. Le Browser Monitor surveille les paramètres du navigateur.",
            FullDescriptionEn = "CCleaner cleans temporary files, registry and browsing data. The Browser Monitor monitors browser settings.",
            DisableImpact = "Le nettoyage programmé et la surveillance ne fonctionneront pas.",
            DisableImpactEn = "Scheduled cleaning and monitoring won't work.",
            PerformanceImpact = "Faible.",
            PerformanceImpactEn = "Low.",
            Recommendation = "Le Browser Monitor peut être désactivé.",
            RecommendationEn = "Browser Monitor can be disabled.",
            Tags = "ccleaner,cleanup,optimization,registry,privacy",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "Process Lasso",
            Aliases = "ProcessLasso,Bitsum Process Lasso",
            Publisher = "Bitsum LLC",
            ExecutableNames = "ProcessLasso.exe,ProcessLassoLauncher.exe,ProcessGovernor.exe",
            Category = KnowledgeCategory.Utility,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Optimiseur de processus Windows.",
            ShortDescriptionEn = "Windows process optimizer.",
            FullDescription = "Process Lasso optimise les processus Windows en gérant les priorités CPU, l'affinité et la consommation de ressources pour améliorer la réactivité.",
            FullDescriptionEn = "Process Lasso optimizes Windows processes by managing CPU priorities, affinity and resource consumption to improve responsiveness.",
            DisableImpact = "L'optimisation automatique des processus ne sera pas active.",
            DisableImpactEn = "Automatic process optimization won't be active.",
            PerformanceImpact = "Très faible.",
            PerformanceImpactEn = "Very low.",
            Recommendation = "Gardez actif si vous l'avez configuré pour des optimisations spécifiques.",
            RecommendationEn = "Keep enabled if you configured it for specific optimizations.",
            Tags = "process,lasso,optimization,cpu,priority",
            LastUpdated = DateTime.Now
        });

        // ============================================================
        // MONITORING & HARDWARE INFO
        // ============================================================

        Save(new KnowledgeEntry
        {
            Name = "HWiNFO",
            Aliases = "HWiNFO64,HWiNFO32",
            Publisher = "REALiX",
            ExecutableNames = "HWiNFO64.exe,HWiNFO32.exe",
            Category = KnowledgeCategory.Hardware,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Outil d'information et de monitoring matériel.",
            ShortDescriptionEn = "Hardware information and monitoring tool.",
            FullDescription = "HWiNFO fournit des informations détaillées sur le matériel et surveille les capteurs (température, tension, vitesse des ventilateurs).",
            FullDescriptionEn = "HWiNFO provides detailed information about hardware and monitors sensors (temperature, voltage, fan speed).",
            DisableImpact = "Le monitoring en arrière-plan ne sera pas actif.",
            DisableImpactEn = "Background monitoring won't be active.",
            PerformanceImpact = "Faible.",
            PerformanceImpactEn = "Low.",
            Recommendation = "Peut être désactivé du démarrage si utilisé ponctuellement.",
            RecommendationEn = "Can be disabled from startup if used occasionally.",
            Tags = "hwinfo,hardware,monitoring,sensors,temperature",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "MSI Afterburner",
            Aliases = "Afterburner,MSIAfterburner",
            Publisher = "MICRO-STAR INTERNATIONAL CO., LTD.",
            ExecutableNames = "MSIAfterburner.exe,RTSS.exe,RTSSHooksLoader64.exe",
            Category = KnowledgeCategory.Hardware,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Utilitaire d'overclocking GPU.",
            ShortDescriptionEn = "GPU overclocking utility.",
            FullDescription = "MSI Afterburner permet l'overclocking de la carte graphique, le monitoring et l'affichage d'OSD en jeu via RivaTuner.",
            FullDescriptionEn = "MSI Afterburner allows graphics card overclocking, monitoring and OSD display in games via RivaTuner.",
            DisableImpact = "L'overclocking personnalisé et l'OSD ne seront pas actifs.",
            DisableImpactEn = "Custom overclocking and OSD won't be active.",
            PerformanceImpact = "Faible.",
            PerformanceImpactEn = "Low.",
            Recommendation = "Gardez actif si vous utilisez l'overclocking ou l'OSD.",
            RecommendationEn = "Keep enabled if you use overclocking or OSD.",
            Tags = "msi,afterburner,overclock,gpu,monitoring,rtss",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "NZXT CAM",
            Aliases = "CAM,NZXT CAM Software",
            Publisher = "NZXT, Inc.",
            ExecutableNames = "NZXT CAM.exe,CAM.exe",
            Category = KnowledgeCategory.Hardware,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Logiciel de monitoring et contrôle NZXT.",
            ShortDescriptionEn = "NZXT monitoring and control software.",
            FullDescription = "NZXT CAM contrôle les produits NZXT (boîtiers, refroidissement, éclairage) et offre un monitoring système détaillé.",
            FullDescriptionEn = "NZXT CAM controls NZXT products (cases, cooling, lighting) and offers detailed system monitoring.",
            DisableImpact = "Le contrôle des produits NZXT et l'éclairage ne seront pas gérés.",
            DisableImpactEn = "NZXT product control and lighting won't be managed.",
            PerformanceImpact = "Modéré.",
            PerformanceImpactEn = "Moderate.",
            Recommendation = "Gardez actif si vous avez des produits NZXT.",
            RecommendationEn = "Keep enabled if you have NZXT products.",
            Tags = "nzxt,cam,monitoring,rgb,cooling,case",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "Fan Control",
            Aliases = "FanControl",
            Publisher = "Rem0o",
            ExecutableNames = "FanControl.exe",
            Category = KnowledgeCategory.Hardware,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Contrôleur de ventilateurs open-source.",
            ShortDescriptionEn = "Open-source fan controller.",
            FullDescription = "Fan Control est un logiciel open-source pour créer des courbes de ventilation personnalisées basées sur les capteurs de température.",
            FullDescriptionEn = "Fan Control is open-source software to create custom fan curves based on temperature sensors.",
            DisableImpact = "Les courbes de ventilation personnalisées ne seront pas actives.",
            DisableImpactEn = "Custom fan curves won't be active.",
            PerformanceImpact = "Très faible.",
            PerformanceImpactEn = "Very low.",
            Recommendation = "Gardez actif si vous avez configuré des courbes personnalisées.",
            RecommendationEn = "Keep enabled if you have configured custom curves.",
            Tags = "fancontrol,fan,cooling,temperature,opensource",
            LastUpdated = DateTime.Now
        });

        // ============================================================
        // OFFICE & PRODUCTIVITY
        // ============================================================

        Save(new KnowledgeEntry
        {
            Name = "LibreOffice",
            Aliases = "LibreOffice Quickstarter,soffice",
            Publisher = "The Document Foundation",
            ExecutableNames = "soffice.exe,soffice.bin,swriter.exe,scalc.exe",
            Category = KnowledgeCategory.Productivity,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Suite bureautique open-source.",
            ShortDescriptionEn = "Open-source office suite.",
            FullDescription = "LibreOffice est une suite bureautique gratuite incluant Writer, Calc, Impress et d'autres applications. Compatible avec Microsoft Office.",
            FullDescriptionEn = "LibreOffice is a free office suite including Writer, Calc, Impress and other applications. Compatible with Microsoft Office.",
            DisableImpact = "Le démarrage rapide ne sera pas actif (temps d'ouverture plus long).",
            DisableImpactEn = "Quickstarter won't be active (longer opening time).",
            PerformanceImpact = "Faible pour le quickstarter.",
            PerformanceImpactEn = "Low for quickstarter.",
            Recommendation = "Le quickstarter peut être désactivé pour économiser de la mémoire.",
            RecommendationEn = "Quickstarter can be disabled to save memory.",
            Tags = "libreoffice,office,writer,calc,opensource,productivity",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "Notion",
            Aliases = "Notion App",
            Publisher = "Notion Labs, Inc.",
            ExecutableNames = "Notion.exe",
            Category = KnowledgeCategory.Productivity,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Espace de travail tout-en-un.",
            ShortDescriptionEn = "All-in-one workspace.",
            FullDescription = "Notion combine notes, bases de données, wikis et gestion de projet dans une seule application. Populaire pour la productivité personnelle et d'équipe.",
            FullDescriptionEn = "Notion combines notes, databases, wikis and project management in a single application. Popular for personal and team productivity.",
            DisableImpact = "Notion ne sera pas accessible immédiatement.",
            DisableImpactEn = "Notion won't be immediately accessible.",
            PerformanceImpact = "Modéré (application Electron).",
            PerformanceImpactEn = "Moderate (Electron application).",
            Recommendation = "Peut être désactivé du démarrage automatique.",
            RecommendationEn = "Can be disabled from automatic startup.",
            Tags = "notion,notes,productivity,wiki,database",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "Obsidian",
            Aliases = "Obsidian MD",
            Publisher = "Dynalist Inc.",
            ExecutableNames = "Obsidian.exe",
            Category = KnowledgeCategory.Productivity,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Application de prise de notes en Markdown.",
            ShortDescriptionEn = "Markdown note-taking application.",
            FullDescription = "Obsidian est une base de connaissances personnelle avec support Markdown, liens bidirectionnels et graphe de connaissances.",
            FullDescriptionEn = "Obsidian is a personal knowledge base with Markdown support, bi-directional links and knowledge graph.",
            DisableImpact = "Obsidian ne sera pas accessible immédiatement.",
            DisableImpactEn = "Obsidian won't be immediately accessible.",
            PerformanceImpact = "Modéré.",
            PerformanceImpactEn = "Moderate.",
            Recommendation = "Peut être désactivé du démarrage automatique.",
            RecommendationEn = "Can be disabled from automatic startup.",
            Tags = "obsidian,notes,markdown,knowledge,productivity",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "Todoist",
            Aliases = "Todoist Desktop",
            Publisher = "Doist Inc.",
            ExecutableNames = "Todoist.exe",
            Category = KnowledgeCategory.Productivity,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Application de gestion de tâches.",
            ShortDescriptionEn = "Task management application.",
            FullDescription = "Todoist est une application de gestion de tâches et de listes de choses à faire avec synchronisation multi-appareils et rappels.",
            FullDescriptionEn = "Todoist is a task and to-do list management application with multi-device synchronization and reminders.",
            DisableImpact = "Les rappels de tâches ne seront pas affichés au démarrage.",
            DisableImpactEn = "Task reminders won't be displayed at startup.",
            PerformanceImpact = "Faible.",
            PerformanceImpactEn = "Low.",
            Recommendation = "Gardez actif si vous dépendez des rappels.",
            RecommendationEn = "Keep enabled if you depend on reminders.",
            Tags = "todoist,tasks,todo,productivity,reminders",
            LastUpdated = DateTime.Now
        });

        // ============================================================
        // PRINTERS & SCANNERS
        // ============================================================

        Save(new KnowledgeEntry
        {
            Name = "HP Smart",
            Aliases = "HP Smart App,HP Printer Software",
            Publisher = "HP Inc.",
            ExecutableNames = "HPSmart.exe,HPPSdr.exe",
            Category = KnowledgeCategory.Hardware,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Application de gestion d'imprimantes HP.",
            ShortDescriptionEn = "HP printer management app.",
            FullDescription = "HP Smart permet de configurer, imprimer, numériser et gérer les imprimantes HP. Inclut des fonctionnalités de diagnostic.",
            FullDescriptionEn = "HP Smart allows configuring, printing, scanning and managing HP printers. Includes diagnostic features.",
            DisableImpact = "L'impression reste possible, mais certaines fonctionnalités avancées ne seront pas disponibles.",
            DisableImpactEn = "Printing remains possible, but some advanced features won't be available.",
            PerformanceImpact = "Faible.",
            PerformanceImpactEn = "Low.",
            Recommendation = "Peut être désactivé si vous n'utilisez pas les fonctionnalités avancées.",
            RecommendationEn = "Can be disabled if you don't use advanced features.",
            Tags = "hp,printer,smart,print,scan,hardware",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "Epson Software Updater",
            Aliases = "Epson Scan,Epson Event Manager",
            Publisher = "Seiko Epson Corporation",
            ExecutableNames = "ESWU.exe,EEventManager.exe,escndv.exe",
            Category = KnowledgeCategory.Hardware,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Logiciels pour imprimantes Epson.",
            ShortDescriptionEn = "Epson printer software.",
            FullDescription = "Suite de logiciels Epson incluant le gestionnaire de mises à jour, le gestionnaire d'événements et les outils de numérisation.",
            FullDescriptionEn = "Epson software suite including updater, event manager and scanning tools.",
            DisableImpact = "Les mises à jour automatiques et certaines fonctionnalités de numérisation peuvent ne pas fonctionner.",
            DisableImpactEn = "Automatic updates and some scanning features might not work.",
            PerformanceImpact = "Faible.",
            PerformanceImpactEn = "Low.",
            Recommendation = "Le software updater peut être désactivé.",
            RecommendationEn = "Software updater can be disabled.",
            Tags = "epson,printer,scan,update,hardware",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "Canon My Image Garden",
            Aliases = "Canon IJ Scan Utility,Canon Quick Menu",
            Publisher = "Canon Inc.",
            ExecutableNames = "CanoMig.exe,CanoScan*.exe,CNQMMAIN.exe",
            Category = KnowledgeCategory.Hardware,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Suite logicielle Canon pour imprimantes.",
            ShortDescriptionEn = "Canon software suite for printers.",
            FullDescription = "Logiciels Canon pour l'impression, la numérisation et la gestion de photos. Inclut des outils de retouche basiques.",
            FullDescriptionEn = "Canon software for printing, scanning and photo management. Includes basic editing tools.",
            DisableImpact = "Certaines fonctionnalités de numérisation rapide ne seront pas disponibles.",
            DisableImpactEn = "Some quick scanning features won't be available.",
            PerformanceImpact = "Faible.",
            PerformanceImpactEn = "Low.",
            Recommendation = "Quick Menu peut être désactivé si non utilisé.",
            RecommendationEn = "Quick Menu can be disabled if not used.",
            Tags = "canon,printer,scan,photo,hardware",
            LastUpdated = DateTime.Now
        });

        // ============================================================
        // CLOUD GAMING
        // ============================================================

        Save(new KnowledgeEntry
        {
            Name = "GeForce NOW",
            Aliases = "NVIDIA GeForce NOW,GFN",
            Publisher = "NVIDIA Corporation",
            ExecutableNames = "GeForceNOW.exe",
            Category = KnowledgeCategory.Gaming,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Service de cloud gaming NVIDIA.",
            ShortDescriptionEn = "NVIDIA cloud gaming service.",
            FullDescription = "GeForce NOW permet de jouer à vos jeux PC en streaming depuis les serveurs NVIDIA. Supporte Steam, Epic et d'autres plateformes.",
            FullDescriptionEn = "GeForce NOW allows playing your PC games via streaming from NVIDIA servers. Supports Steam, Epic and other platforms.",
            DisableImpact = "GeForce NOW ne démarrera pas automatiquement.",
            DisableImpactEn = "GeForce NOW won't start automatically.",
            PerformanceImpact = "Faible au repos.",
            PerformanceImpactEn = "Low when idle.",
            Recommendation = "Peut être désactivé du démarrage automatique.",
            RecommendationEn = "Can be disabled from automatic startup.",
            Tags = "nvidia,geforcenow,cloud,gaming,streaming",
            LastUpdated = DateTime.Now
        });

        Save(new KnowledgeEntry
        {
            Name = "Xbox Game Pass",
            Aliases = "Xbox App,XboxGamingOverlay,GameBar",
            Publisher = "Microsoft Corporation",
            ExecutableNames = "XboxApp.exe,GameBar.exe,XboxGamingOverlay.exe,GamingServices.exe",
            Category = KnowledgeCategory.Gaming,
            SafetyLevel = SafetyLevel.Safe,
            ShortDescription = "Application Xbox et Game Pass pour PC.",
            ShortDescriptionEn = "Xbox App and Game Pass for PC.",
            FullDescription = "L'application Xbox permet d'accéder au Xbox Game Pass, de jouer avec des amis et d'utiliser la Game Bar pour les captures et le monitoring.",
            FullDescriptionEn = "The Xbox app allows accessing Xbox Game Pass, playing with friends and using Game Bar for captures and monitoring.",
            DisableImpact = "La Game Bar et certaines fonctionnalités Xbox ne seront pas disponibles.",
            DisableImpactEn = "Game Bar and some Xbox features won't be available.",
            PerformanceImpact = "Faible.",
            PerformanceImpactEn = "Low.",
            Recommendation = "Gardez actif si vous utilisez Xbox Game Pass ou la Game Bar.",
            RecommendationEn = "Keep enabled if you use Xbox Game Pass or Game Bar.",
            Tags = "xbox,gamepass,microsoft,gaming,gamebar",
            LastUpdated = DateTime.Now
        });
    }

    private void Save(KnowledgeEntry entry)
    {
        _service.SaveEntry(entry);
    }
}
