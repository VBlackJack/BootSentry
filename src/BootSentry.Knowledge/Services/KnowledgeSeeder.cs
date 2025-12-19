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
            FullDescription = "Windows Security Health Service surveille l'état de la sécurité de votre système. Il gère l'icône de la barre des tâches qui vous alerte des problèmes de sécurité, des mises à jour antivirus et de l'état du pare-feu. Ce service est intégré à Windows 10/11 et communique avec le Centre de sécurité Windows.",
            DisableImpact = "Vous ne recevrez plus d'alertes de sécurité. L'icône de Windows Security disparaîtra de la barre des tâches. Le système pourrait devenir vulnérable sans notifications appropriées.",
            PerformanceImpact = "Très faible (~5 Mo RAM). Impact négligeable sur les performances.",
            Recommendation = "Ne jamais désactiver sauf si vous utilisez un autre antivirus qui le remplace.",
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
            FullDescription = "Shell Experience Host est responsable de l'affichage des éléments visuels de l'interface Windows moderne : les tuiles du menu Démarrer, la barre des tâches, le centre de notifications et les effets de transparence (Fluent Design). Il fait partie intégrante de l'expérience utilisateur Windows 10/11.",
            DisableImpact = "L'interface Windows deviendra instable. Le menu Démarrer, la barre des tâches et les notifications pourraient ne plus fonctionner correctement.",
            PerformanceImpact = "Modéré (~50-100 Mo RAM selon les effets visuels activés).",
            Recommendation = "Ne jamais désactiver. Composant essentiel de Windows.",
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
            FullDescription = "Windows Search indexe vos fichiers, emails (Outlook) et autres contenus pour permettre une recherche rapide. Le service maintient une base de données de tous vos fichiers et leur contenu. L'indexation se fait généralement pendant les périodes d'inactivité.",
            DisableImpact = "La recherche Windows sera très lente ou ne fonctionnera pas. La recherche dans le menu Démarrer, l'Explorateur de fichiers et Outlook sera affectée.",
            PerformanceImpact = "Peut consommer des ressources significatives pendant l'indexation (~100-500 Mo RAM, utilisation CPU lors de l'indexation). Une fois l'index créé, l'impact est minimal.",
            Recommendation = "Gardez activé si vous utilisez beaucoup la recherche. Peut être désactivé sur les PC avec SSD rapide si vous n'utilisez jamais la recherche Windows.",
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
            FullDescription = "Windows Update télécharge et installe les mises à jour de sécurité, les correctifs et les nouvelles fonctionnalités pour Windows. Il vérifie régulièrement les nouvelles mises à jour disponibles et peut les installer automatiquement selon vos paramètres.",
            DisableImpact = "Votre système ne recevra plus de mises à jour de sécurité, vous exposant aux vulnérabilités connues. Les failles de sécurité ne seront pas corrigées.",
            PerformanceImpact = "Variable. Peut utiliser beaucoup de bande passante et CPU lors des téléchargements/installations. Minimal au repos.",
            Recommendation = "Ne jamais désactiver. Les mises à jour de sécurité sont essentielles.",
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
            FullDescription = "Cortana est l'assistant personnel de Microsoft qui répond aux commandes vocales et textuelles. Elle peut définir des rappels, effectuer des recherches web, ouvrir des applications et répondre à des questions. Dans Windows 11, Cortana est séparée de la recherche Windows.",
            DisableImpact = "Perte de l'assistant vocal. La recherche Windows continuera de fonctionner normalement.",
            PerformanceImpact = "Modéré (~50-100 Mo RAM). Écoute en permanence si 'Hey Cortana' est activé.",
            Recommendation = "Peut être désactivé en toute sécurité si vous n'utilisez pas l'assistant vocal.",
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
            FullDescription = "Les widgets Windows 11 affichent des informations en un coup d'œil : météo, actualités, calendrier, tâches, photos, etc. Accessible via le bouton Widgets dans la barre des tâches ou Win+W.",
            DisableImpact = "Pas d'accès au panneau de widgets. Aucun impact sur les autres fonctionnalités.",
            PerformanceImpact = "Modéré (~50-100 Mo RAM). Consomme des ressources même en arrière-plan.",
            Recommendation = "Peut être désactivé si vous n'utilisez pas les widgets. Économie de RAM.",
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
            FullDescription = "Phone Link (anciennement Votre Téléphone) permet de voir les notifications, envoyer des SMS, passer des appels et accéder aux photos de votre smartphone depuis Windows. Support complet pour Android, limité pour iPhone.",
            DisableImpact = "Pas de synchronisation avec le téléphone. Les notifications téléphoniques n'apparaîtront pas sur PC.",
            PerformanceImpact = "Modéré (~50-100 Mo RAM).",
            Recommendation = "Peut être désactivé si vous n'utilisez pas la synchronisation téléphone-PC.",
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
            FullDescription = "Le Microsoft Store permet d'installer des applications, jeux et mises à jour. Il met également à jour automatiquement les applications UWP installées en arrière-plan.",
            DisableImpact = "Les applications du Store ne se mettront plus à jour automatiquement.",
            PerformanceImpact = "Faible au repos. Peut utiliser de la bande passante lors des mises à jour.",
            Recommendation = "Le service de mise à jour peut être configuré dans les paramètres du Store.",
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
            FullDescription = "La Xbox Game Bar offre un overlay en jeu pour l'enregistrement, les captures d'écran, le chat Xbox, le monitoring des performances et les widgets. Accessible avec Win+G.",
            DisableImpact = "Pas d'overlay de jeu. L'enregistrement rapide (Win+Alt+R) ne fonctionnera plus.",
            PerformanceImpact = "Faible au repos (~20-40 Mo RAM). L'enregistrement peut impacter les FPS.",
            Recommendation = "Peut être désactivé si vous utilisez OBS ou ShadowPlay pour l'enregistrement.",
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
            FullDescription = "Copilot est l'assistant IA de Microsoft basé sur GPT-4. Il peut répondre à des questions, générer du texte, créer des images et aider avec les tâches Windows. Accessible via la barre des tâches ou Win+C.",
            DisableImpact = "Pas d'accès à l'assistant IA Copilot.",
            PerformanceImpact = "Faible au repos. Les requêtes utilisent le cloud Microsoft.",
            Recommendation = "Peut être désactivé si vous n'utilisez pas l'assistant IA.",
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
            FullDescription = "Microsoft To Do permet de créer des listes de tâches, définir des rappels et synchroniser avec Outlook Tasks. Intégration avec l'écosystème Microsoft 365.",
            DisableImpact = "Pas de notifications de rappels au démarrage. Les tâches restent accessibles en ligne.",
            PerformanceImpact = "Faible (~30-50 Mo RAM).",
            Recommendation = "Peut être désactivé si vous n'utilisez pas les rappels.",
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
            FullDescription = "Microsoft Outlook gère vos emails, calendrier, contacts et tâches. Le nouveau Outlook pour Windows est une version modernisée avec interface web. Outlook classique fait partie de Microsoft 365.",
            DisableImpact = "Pas de notifications d'emails au démarrage. Outlook peut être lancé manuellement.",
            PerformanceImpact = "Modéré à élevé (~100-300 Mo RAM).",
            Recommendation = "Peut être désactivé si vous préférez lancer Outlook manuellement.",
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
            FullDescription = "OneNote est un bloc-notes numérique pour capturer des notes, dessins, captures d'écran et enregistrements audio. Synchronise via OneDrive avec tous vos appareils.",
            DisableImpact = "Pas de raccourci 'Envoyer à OneNote' au démarrage. OneNote peut être lancé manuellement.",
            PerformanceImpact = "Modéré (~80-150 Mo RAM).",
            Recommendation = "Peut être désactivé si vous n'utilisez pas le raccourci de capture.",
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
            FullDescription = "PowerPoint permet de créer des présentations professionnelles avec diapositives, animations et transitions. Fait partie de Microsoft 365.",
            DisableImpact = "Aucun impact. PowerPoint ne devrait pas être dans le démarrage automatique.",
            PerformanceImpact = "Aucun au repos.",
            Recommendation = "Ne devrait pas être dans le démarrage. Peut être désactivé.",
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
            FullDescription = "Microsoft Word est le traitement de texte standard de l'industrie pour créer des documents professionnels. Fait partie de Microsoft 365.",
            DisableImpact = "Aucun impact. Word ne devrait pas être dans le démarrage automatique.",
            PerformanceImpact = "Aucun au repos.",
            Recommendation = "Ne devrait pas être dans le démarrage. Peut être désactivé.",
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
            FullDescription = "Microsoft Excel est le tableur de référence pour l'analyse de données, les graphiques et les formules. Fait partie de Microsoft 365.",
            DisableImpact = "Aucun impact. Excel ne devrait pas être dans le démarrage automatique.",
            PerformanceImpact = "Aucun au repos.",
            Recommendation = "Ne devrait pas être dans le démarrage. Peut être désactivé.",
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
            FullDescription = "Microsoft Access permet de créer des bases de données relationnelles avec formulaires et rapports. Fait partie de certaines versions de Microsoft 365.",
            DisableImpact = "Aucun impact. Access ne devrait pas être dans le démarrage automatique.",
            PerformanceImpact = "Aucun au repos.",
            Recommendation = "Ne devrait pas être dans le démarrage. Peut être désactivé.",
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
            FullDescription = "Power Automate Desktop permet de créer des flux de travail automatisés sans code : automatisation de clics, remplissage de formulaires, manipulation de fichiers, etc.",
            DisableImpact = "Les flux automatisés planifiés ne s'exécuteront pas.",
            PerformanceImpact = "Faible au repos (~30-50 Mo RAM).",
            Recommendation = "Gardez activé si vous utilisez des flux automatisés. Peut être désactivé sinon.",
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
            FullDescription = "OneDrive synchronise automatiquement vos fichiers avec le cloud Microsoft. Il s'intègre à l'Explorateur de fichiers et permet d'accéder à vos documents depuis n'importe quel appareil. Inclut la fonctionnalité 'Fichiers à la demande' qui ne télécharge les fichiers que lorsque nécessaire.",
            DisableImpact = "Vos fichiers ne seront plus synchronisés avec le cloud. Les fichiers 'à la demande' ne seront plus accessibles.",
            PerformanceImpact = "Faible à modéré (~30-80 Mo RAM). Peut utiliser de la bande passante lors de la synchronisation.",
            Recommendation = "Peut être désactivé si vous n'utilisez pas OneDrive ou préférez un autre service cloud.",
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
            FullDescription = "Windows Defender (maintenant Windows Security) est l'antivirus intégré de Microsoft. Il offre une protection en temps réel contre les virus, malwares, ransomwares et autres menaces. Il inclut également une protection cloud et comportementale.",
            DisableImpact = "Votre PC sera sans protection antivirus. Fortement déconseillé sauf si vous installez un autre antivirus.",
            PerformanceImpact = "Modéré (~100-200 Mo RAM). Peut utiliser beaucoup de CPU lors des analyses.",
            Recommendation = "Ne jamais désactiver sauf si vous utilisez un antivirus tiers.",
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
            FullDescription = "Le pare-feu Windows filtre le trafic réseau entrant et sortant selon des règles définies. Il protège votre PC contre les accès non autorisés depuis le réseau et Internet.",
            DisableImpact = "Votre PC sera exposé aux attaques réseau. Aucun filtrage du trafic entrant/sortant.",
            PerformanceImpact = "Très faible. Impact négligeable sur les performances.",
            Recommendation = "Ne jamais désactiver sauf si vous utilisez un pare-feu tiers.",
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
            FullDescription = "GeForce Experience optimise automatiquement les paramètres de vos jeux, met à jour les pilotes graphiques, permet l'enregistrement vidéo (ShadowPlay) et le streaming. Inclut également des filtres de jeu et la fonctionnalité NVIDIA Highlights.",
            DisableImpact = "Pas de mise à jour automatique des pilotes. Perte de ShadowPlay et des optimisations automatiques de jeux.",
            PerformanceImpact = "Modéré (~100-200 Mo RAM avec tous les services). ShadowPlay peut impacter les FPS de 2-5%.",
            Recommendation = "Peut être désactivé si vous mettez à jour manuellement les pilotes et n'utilisez pas ShadowPlay.",
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
            FullDescription = "NVIDIA App est le nouveau logiciel unifié qui remplace progressivement GeForce Experience. Combine les paramètres du panneau de configuration NVIDIA avec les fonctionnalités de GFE : mise à jour des pilotes, optimisation des jeux, et enregistrement.",
            DisableImpact = "Pas d'accès aux paramètres avancés NVIDIA. Pas de mise à jour automatique.",
            PerformanceImpact = "Modéré (~80-150 Mo RAM).",
            Recommendation = "Peut être désactivé si vous n'utilisez pas les fonctionnalités avancées.",
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
            FullDescription = "NVIDIA Container héberge divers services NVIDIA : télémétrie, GameStream, Shield, et autres fonctionnalités. Plusieurs instances peuvent tourner simultanément pour différents services.",
            DisableImpact = "Certaines fonctionnalités NVIDIA pourraient ne plus fonctionner (ShadowPlay, GameStream, etc.).",
            PerformanceImpact = "Faible à modéré (~20-50 Mo RAM par instance).",
            Recommendation = "Peut être partiellement désactivé via les services NVIDIA. Gardez le service de base.",
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
            FullDescription = "NVIDIA Broadcast utilise l'IA des GPU RTX pour la suppression de bruit du micro, le remplacement d'arrière-plan sans fond vert, le suivi automatique de la caméra et l'élimination de l'écho.",
            DisableImpact = "Pas d'effets IA pour le streaming. Le micro et la webcam fonctionnent normalement.",
            PerformanceImpact = "Modéré (~200-400 Mo VRAM, ~50 Mo RAM). Utilise ~5-10% du GPU RTX.",
            Recommendation = "Peut être désactivé si vous ne faites pas de streaming ou appels vidéo.",
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
            FullDescription = "Le service de télémétrie NVIDIA collecte des données anonymes sur votre utilisation du GPU et des pilotes. Ces données sont envoyées à NVIDIA pour améliorer leurs produits.",
            DisableImpact = "Aucun impact sur le fonctionnement. Moins de données envoyées à NVIDIA.",
            PerformanceImpact = "Très faible (~10-20 Mo RAM).",
            Recommendation = "Peut être désactivé pour la confidentialité. Aucun impact sur les performances ou fonctionnalités.",
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
            FullDescription = "FrameView affiche les FPS, frametime, consommation électrique du GPU et autres métriques de performance en temps réel. Permet aussi de capturer des benchmarks pour analyse.",
            DisableImpact = "Pas de monitoring de performance au démarrage.",
            PerformanceImpact = "Faible (~30-50 Mo RAM).",
            Recommendation = "Peut être désactivé. Lancez-le manuellement pour les benchmarks.",
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
            FullDescription = "Radeon Software (Adrenalin Edition) permet de configurer votre carte graphique AMD, mettre à jour les pilotes, optimiser les jeux et enregistrer des vidéos avec ReLive. Inclut des fonctionnalités comme Radeon Anti-Lag, Radeon Boost, FSR et le streaming.",
            DisableImpact = "Pas d'accès aux paramètres avancés de la carte graphique. Pas de mise à jour automatique.",
            PerformanceImpact = "Modéré (~80-150 Mo RAM).",
            Recommendation = "Peut être désactivé si vous n'utilisez pas les fonctionnalités avancées.",
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
            FullDescription = "Les pilotes de chipset AMD optimisent la communication entre le processeur Ryzen et les composants de la carte mère (USB, SATA, NVMe). Essentiels pour les performances optimales sur les systèmes AMD.",
            DisableImpact = "Les services du chipset restent actifs même si l'interface est désactivée.",
            PerformanceImpact = "Très faible (~10-20 Mo RAM).",
            Recommendation = "Gardez les pilotes installés. Les services au démarrage peuvent être désactivés sans impact.",
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
            FullDescription = "AMD Ryzen Master permet d'overclocker les processeurs Ryzen, ajuster les voltages, surveiller les températures et configurer les profils de performance (Precision Boost Overdrive, Curve Optimizer).",
            DisableImpact = "Les profils d'overclocking ne seront pas appliqués automatiquement. Le monitoring ne sera pas disponible.",
            PerformanceImpact = "Faible (~30-50 Mo RAM).",
            Recommendation = "Peut être désactivé si vous n'utilisez pas l'overclocking. Lancez-le manuellement si besoin.",
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
            FullDescription = "AMD Link permet de streamer vos jeux PC vers smartphone, tablette ou TV. Offre également un second écran avec stats de performance et le contrôle de Radeon Software à distance.",
            DisableImpact = "Pas de streaming vers mobile. Les jeux PC ne sont pas affectés.",
            PerformanceImpact = "Faible au repos (~20-30 Mo RAM).",
            Recommendation = "Peut être désactivé si vous n'utilisez pas le streaming mobile.",
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
            FullDescription = "Permet de configurer les paramètres d'affichage, la résolution, les profils de couleur et les raccourcis clavier pour les graphiques Intel intégrés.",
            DisableImpact = "Perte des raccourcis clavier Intel (rotation écran, etc.). Les paramètres de base restent accessibles via Windows.",
            PerformanceImpact = "Faible (~20-40 Mo RAM).",
            Recommendation = "Peut être désactivé si vous n'utilisez pas les raccourcis ou paramètres avancés.",
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
            FullDescription = "Intel Management Engine est un sous-système matériel intégré aux processeurs Intel. Il gère des fonctions comme Intel AMT (gestion à distance), la sécurité et la gestion d'alimentation. Fonctionne indépendamment du système d'exploitation.",
            DisableImpact = "Certaines fonctionnalités système peuvent ne plus fonctionner. La désactivation complète n'est pas recommandée et peut causer des instabilités.",
            PerformanceImpact = "Très faible (~10-20 Mo RAM).",
            Recommendation = "Ne pas désactiver sauf si vous savez ce que vous faites. Composant système important.",
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
            FullDescription = "Intel RST gère les configurations RAID, le caching SSD (Intel Optane) et optimise les performances de stockage. Fournit également des notifications sur l'état des disques.",
            DisableImpact = "Perte des notifications d'état de disque. Les configurations RAID pourraient être affectées. Le caching Optane pourrait ne plus fonctionner.",
            PerformanceImpact = "Faible (~20-40 Mo RAM).",
            Recommendation = "Gardez activé si vous utilisez RAID ou Intel Optane. Peut être désactivé sur les systèmes simples.",
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
            FullDescription = "Intel Optane Memory accélère les disques durs traditionnels en utilisant un SSD Optane comme cache. Si vous avez un module Optane installé, ce service est essentiel.",
            DisableImpact = "CRITIQUE si vous utilisez Optane : votre disque dur sera considérablement ralenti. Pas d'impact si vous n'avez pas d'Optane.",
            PerformanceImpact = "Très faible (~10-20 Mo RAM).",
            Recommendation = "NE PAS désactiver si vous avez Intel Optane. Vérifiez dans le BIOS si Optane est actif.",
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
            FullDescription = "Gère la fonctionnalité Bluetooth des cartes Wi-Fi Intel intégrées. Fournit des fonctionnalités avancées au-delà du Bluetooth Windows standard.",
            DisableImpact = "Le Bluetooth fonctionnera toujours via Windows mais certaines fonctionnalités Intel pourraient manquer.",
            PerformanceImpact = "Très faible (~5-15 Mo RAM).",
            Recommendation = "Peut être désactivé si vous n'utilisez pas le Bluetooth ou préférez le gestionnaire Windows.",
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
            FullDescription = "Intel PROSet offre des options de configuration Wi-Fi avancées : profils de connexion, paramètres de sécurité avancés, et diagnostics réseau.",
            DisableImpact = "Le Wi-Fi fonctionne normalement via Windows. Perte des fonctionnalités avancées Intel.",
            PerformanceImpact = "Faible (~15-30 Mo RAM).",
            Recommendation = "Peut être désactivé. Windows gère très bien le Wi-Fi sans cet outil.",
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
            FullDescription = "Intel XTU permet d'overclocker les processeurs Intel débloqués (série K), de surveiller les températures et voltages, et d'optimiser les performances.",
            DisableImpact = "Les profils d'overclocking ne seront pas appliqués au démarrage. Le monitoring ne sera pas disponible.",
            PerformanceImpact = "Faible (~30-50 Mo RAM).",
            Recommendation = "Gardez activé si vous avez des profils d'overclocking. Sinon, peut être désactivé.",
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
            FullDescription = "Permet de configurer les paramètres audio avancés : égaliseur, effets sonores, configuration des haut-parleurs, détection des prises jack.",
            DisableImpact = "Perte de l'interface de configuration avancée. L'audio fonctionnera toujours via les paramètres Windows standards.",
            PerformanceImpact = "Faible (~15-30 Mo RAM).",
            Recommendation = "Peut être désactivé si vous n'utilisez que les paramètres audio Windows.",
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
            FullDescription = "Gère les gestes multi-touch, le défilement, le zoom et autres fonctionnalités avancées des touchpads Synaptics présents sur de nombreux laptops.",
            DisableImpact = "Le touchpad fonctionnera mais avec des fonctionnalités réduites. Les gestes avancés pourraient ne plus fonctionner.",
            PerformanceImpact = "Faible (~10-20 Mo RAM).",
            Recommendation = "Gardez activé sur les laptops. Non nécessaire sur les PC de bureau.",
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
            FullDescription = "Norton offre une protection antivirus, un pare-feu, un VPN, un gestionnaire de mots de passe et une protection de l'identité. C'est une suite de sécurité complète payante.",
            DisableImpact = "Perte de la protection antivirus en temps réel. Windows Defender prendra le relais si Norton est complètement désactivé.",
            PerformanceImpact = "Modéré à élevé (~150-300 Mo RAM). Peut ralentir les analyses de fichiers.",
            Recommendation = "Gardez activé si vous avez payé pour Norton. Sinon, désinstallez complètement et utilisez Windows Defender.",
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
            FullDescription = "McAfee offre une protection antivirus, un pare-feu et diverses fonctionnalités de sécurité. Souvent préinstallé sur les PC de marque.",
            DisableImpact = "Perte de la protection en temps réel.",
            PerformanceImpact = "Modéré à élevé (~200-400 Mo RAM). Connu pour impacter les performances.",
            Recommendation = "Si vous ne l'avez pas acheté volontairement, envisagez de le désinstaller et d'utiliser Windows Defender.",
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
            FullDescription = "Malwarebytes détecte et supprime les malwares, adwares et PUP que les antivirus traditionnels peuvent manquer. La version gratuite est un scanner à la demande, la version premium offre une protection en temps réel.",
            DisableImpact = "Version gratuite : aucun impact (pas de protection temps réel). Version premium : perte de la protection temps réel.",
            PerformanceImpact = "Faible à modéré (~50-100 Mo RAM en version premium).",
            Recommendation = "Gardez activé si vous avez la version premium. La version gratuite peut être lancée manuellement.",
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
            FullDescription = "Steam est la plus grande plateforme de distribution de jeux PC. Elle gère vos achats, téléchargements, mises à jour de jeux, le cloud save, les succès et les fonctionnalités communautaires. Le démarrage automatique permet de recevoir les notifications et de mettre à jour les jeux en arrière-plan.",
            DisableImpact = "Pas de mise à jour automatique des jeux. Pas de notifications d'amis. Les jeux fonctionneront toujours si Steam est lancé manuellement.",
            PerformanceImpact = "Modéré (~100-200 Mo RAM avec le navigateur intégré).",
            Recommendation = "Peut être désactivé si vous préférez lancer Steam manuellement quand vous voulez jouer.",
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
            FullDescription = "Le lanceur Epic Games permet d'accéder aux jeux achetés sur l'Epic Games Store, dont les jeux gratuits hebdomadaires. Nécessaire pour Fortnite et les jeux Unreal Engine.",
            DisableImpact = "Pas de récupération automatique des jeux gratuits. Les jeux fonctionneront si le lanceur est démarré manuellement.",
            PerformanceImpact = "Modéré (~80-150 Mo RAM).",
            Recommendation = "Peut être désactivé. Pensez juste à le lancer régulièrement pour récupérer les jeux gratuits.",
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
            FullDescription = "GOG Galaxy est le client de la plateforme GOG.com, spécialisée dans les jeux sans DRM. Il peut également intégrer vos bibliothèques Steam, Epic, etc. en une seule interface.",
            DisableImpact = "Aucun impact sur les jeux GOG (sans DRM). Perte de l'intégration des bibliothèques.",
            PerformanceImpact = "Faible à modéré (~50-100 Mo RAM).",
            Recommendation = "Peut être désactivé. Les jeux GOG fonctionnent sans le client.",
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
            FullDescription = "L'EA App (anciennement Origin) est nécessaire pour jouer aux jeux EA (FIFA, Battlefield, The Sims, etc.). Gère les téléchargements, mises à jour et la connexion EA.",
            DisableImpact = "Les jeux EA ne pourront pas se lancer sans le client. Démarrez-le manuellement avant de jouer.",
            PerformanceImpact = "Modéré (~70-120 Mo RAM).",
            Recommendation = "Peut être désactivé si vous ne jouez pas souvent aux jeux EA.",
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
            FullDescription = "Ubisoft Connect (anciennement Uplay) est requis pour les jeux Ubisoft (Assassin's Creed, Far Cry, etc.). Gère les succès, récompenses et connexion en ligne.",
            DisableImpact = "Les jeux Ubisoft ne pourront pas se lancer. Même les jeux Steam Ubisoft nécessitent ce client.",
            PerformanceImpact = "Modéré (~60-100 Mo RAM).",
            Recommendation = "Peut être désactivé si vous ne jouez pas régulièrement aux jeux Ubisoft.",
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
            FullDescription = "L'app Xbox permet d'accéder au Xbox Game Pass PC, aux jeux Xbox Play Anywhere, au chat Xbox et aux fonctionnalités sociales Xbox. La Game Bar (Win+G) fait également partie de cet écosystème.",
            DisableImpact = "Perte de la Game Bar et des fonctionnalités Xbox. Les jeux Game Pass pourraient ne plus fonctionner.",
            PerformanceImpact = "Faible à modéré (~40-80 Mo RAM).",
            Recommendation = "Gardez activé si vous utilisez le Xbox Game Pass ou la Game Bar.",
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
            FullDescription = "Battle.net est le client de Blizzard pour ses jeux (World of Warcraft, Diablo, Overwatch, Hearthstone, StarCraft, etc.). Gère les téléchargements, mises à jour, le chat avec les amis et les fonctionnalités sociales Blizzard.",
            DisableImpact = "Pas de notifications d'amis. Les jeux Blizzard fonctionneront si Battle.net est lancé manuellement.",
            PerformanceImpact = "Modéré (~80-150 Mo RAM).",
            Recommendation = "Peut être désactivé si vous ne jouez pas souvent aux jeux Blizzard.",
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
            FullDescription = "Discord permet la communication vocale et textuelle, principalement utilisé par les gamers. Offre des serveurs communautaires, le partage d'écran, le streaming et l'intégration avec de nombreux jeux.",
            DisableImpact = "Pas de connexion automatique. Vous devrez lancer Discord manuellement pour rejoindre vos serveurs.",
            PerformanceImpact = "Modéré (~80-150 Mo RAM, plus si plusieurs serveurs sont ouverts).",
            Recommendation = "Peut être désactivé si vous n'avez pas besoin d'être connecté en permanence.",
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
            FullDescription = "Le service Click-to-Run gère les mises à jour de Microsoft Office/365 et la synchronisation des paramètres. Il permet également le démarrage rapide des applications Office.",
            DisableImpact = "Mises à jour Office manuelles uniquement. Démarrage des applications Office légèrement plus lent.",
            PerformanceImpact = "Faible (~30-60 Mo RAM).",
            Recommendation = "Peut être désactivé si vous préférez des mises à jour manuelles.",
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
            FullDescription = "Creative Cloud gère les installations, mises à jour et licences des applications Adobe (Photoshop, Illustrator, Premiere, etc.). Synchronise également les fichiers et polices Creative Cloud.",
            DisableImpact = "Pas de mise à jour automatique. Les applications Adobe fonctionnent toujours mais vous devrez gérer les mises à jour manuellement.",
            PerformanceImpact = "Modéré à élevé (~100-250 Mo RAM avec tous les processus).",
            Recommendation = "Peut être désactivé si vous n'avez pas besoin des mises à jour automatiques. Attention : certaines fonctionnalités cloud pourraient ne plus fonctionner.",
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
            FullDescription = "Vérifie et notifie les mises à jour disponibles pour Java Runtime Environment. Java est utilisé par certaines applications et sites web.",
            DisableImpact = "Pas de notification des mises à jour Java. Vous devrez vérifier manuellement.",
            PerformanceImpact = "Très faible (~5-10 Mo RAM).",
            Recommendation = "Peut être désactivé. Vérifiez manuellement les mises à jour Java de temps en temps pour la sécurité.",
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
            FullDescription = "Skype permet les appels vidéo/audio, la messagerie instantanée et le partage d'écran. Peut aussi appeler des numéros de téléphone (payant).",
            DisableImpact = "Pas de connexion automatique. Vous ne recevrez pas les appels/messages si Skype n'est pas lancé.",
            PerformanceImpact = "Modéré (~80-150 Mo RAM).",
            Recommendation = "Peut être désactivé si vous utilisez Teams, Zoom ou d'autres alternatives.",
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
            FullDescription = "Teams combine chat, visioconférence, stockage de fichiers et intégration Office 365. Utilisé massivement en entreprise pour le travail collaboratif.",
            DisableImpact = "Pas de notifications de messages/réunions si Teams n'est pas lancé.",
            PerformanceImpact = "Élevé (~200-400 Mo RAM). Connu pour sa consommation mémoire.",
            Recommendation = "Peut être désactivé si non utilisé pour le travail. Attention aux réunions manquées.",
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
            FullDescription = "Zoom permet d'organiser et rejoindre des réunions vidéo, webinaires et conférences. Très utilisé pour le télétravail et l'enseignement à distance.",
            DisableImpact = "Vous devrez lancer Zoom manuellement avant les réunions.",
            PerformanceImpact = "Faible au repos (~20 Mo RAM). Élevé pendant les appels.",
            Recommendation = "Peut être désactivé. Zoom se lance généralement via des liens de réunion.",
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
            FullDescription = "Slack est une plateforme de communication en équipe avec des canaux, messages directs, intégrations d'apps et partage de fichiers.",
            DisableImpact = "Pas de notifications si Slack n'est pas lancé. Messages manqués.",
            PerformanceImpact = "Modéré à élevé (~150-300 Mo RAM).",
            Recommendation = "Gardez activé si utilisé pour le travail. Peut être désactivé sinon.",
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
            FullDescription = "Telegram est une app de messagerie axée sur la vitesse et la sécurité. Offre des chats secrets chiffrés, des groupes jusqu'à 200 000 membres et des canaux.",
            DisableImpact = "Pas de notifications de messages si Telegram n'est pas lancé.",
            PerformanceImpact = "Faible (~30-60 Mo RAM).",
            Recommendation = "Peut être désactivé si vous préférez utiliser la version web ou mobile.",
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
            FullDescription = "Signal est une application de messagerie ultra-sécurisée recommandée par des experts en sécurité. Tous les messages, appels et fichiers sont chiffrés de bout en bout. Open source et sans publicité.",
            DisableImpact = "Pas de notifications de messages Signal si l'application n'est pas lancée.",
            PerformanceImpact = "Faible (~40-80 Mo RAM).",
            Recommendation = "Peut être désactivé si vous préférez lancer Signal manuellement.",
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
            FullDescription = "WhatsApp Desktop permet d'envoyer des messages et passer des appels depuis votre PC, synchronisé avec votre téléphone. Chiffrement de bout en bout pour les messages.",
            DisableImpact = "Pas de notifications WhatsApp sur le PC.",
            PerformanceImpact = "Modéré (~80-150 Mo RAM).",
            Recommendation = "Peut être désactivé si vous préférez utiliser WhatsApp Web ou uniquement sur mobile.",
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
            FullDescription = "Google Drive synchronise vos fichiers avec le cloud Google. Intègre Google Docs, Sheets et Slides. Peut streamer les fichiers sans les télécharger complètement.",
            DisableImpact = "Fichiers non synchronisés. Les fichiers locaux restent accessibles mais pas mis à jour avec le cloud.",
            PerformanceImpact = "Faible à modéré (~40-80 Mo RAM).",
            Recommendation = "Peut être désactivé si vous n'utilisez pas activement Google Drive.",
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
            FullDescription = "Dropbox synchronise vos fichiers entre vos appareils et le cloud. Offre le partage de fichiers, la collaboration et l'historique des versions.",
            DisableImpact = "Fichiers non synchronisés automatiquement.",
            PerformanceImpact = "Modéré (~70-120 Mo RAM).",
            Recommendation = "Peut être désactivé si vous préférez synchroniser manuellement.",
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
            FullDescription = "pCloud est un service de stockage cloud basé en Suisse, offrant un chiffrement côté client optionnel (pCloud Crypto). Propose un stockage à vie avec un seul paiement et synchronise vos fichiers entre appareils.",
            DisableImpact = "Vos fichiers ne seront plus synchronisés automatiquement avec le cloud pCloud.",
            PerformanceImpact = "Faible à modéré (~40-80 Mo RAM).",
            Recommendation = "Gardez activé si vous utilisez pCloud pour la synchronisation. Peut être désactivé sinon.",
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
            FullDescription = "Proton Drive est un service de stockage cloud chiffré de bout en bout, créé par les développeurs de ProtonMail. Toutes les données sont chiffrées avant de quitter votre appareil.",
            DisableImpact = "Les fichiers ne seront plus synchronisés avec Proton Drive.",
            PerformanceImpact = "Faible (~30-60 Mo RAM).",
            Recommendation = "Gardez activé si vous utilisez Proton Drive pour la synchronisation sécurisée.",
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
            FullDescription = "Proton Mail Bridge permet d'utiliser votre compte ProtonMail avec des clients email classiques (Outlook, Thunderbird, etc.) en créant un serveur local IMAP/SMTP qui déchiffre vos emails.",
            DisableImpact = "Votre client email ne pourra plus accéder à ProtonMail. Les emails ne seront plus synchronisés.",
            PerformanceImpact = "Faible (~40-70 Mo RAM).",
            Recommendation = "Gardez activé si vous utilisez ProtonMail avec un client email de bureau.",
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
            FullDescription = "iCloud pour Windows synchronise vos photos, documents, favoris et mots de passe avec vos appareils Apple.",
            DisableImpact = "Pas de synchronisation avec les appareils Apple. Photos et fichiers iCloud non accessibles localement.",
            PerformanceImpact = "Modéré (~50-100 Mo RAM).",
            Recommendation = "Gardez activé si vous utilisez des appareils Apple. Peut être désactivé sinon.",
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
            FullDescription = "Spotify permet d'écouter de la musique en streaming, créer des playlists et découvrir de nouveaux artistes. Le démarrage automatique permet une lecture rapide.",
            DisableImpact = "Spotify ne démarrera pas automatiquement. Vous devrez le lancer manuellement.",
            PerformanceImpact = "Modéré (~100-150 Mo RAM).",
            Recommendation = "Peut être désactivé si vous ne voulez pas que Spotify démarre automatiquement.",
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
            FullDescription = "iTunes gère votre bibliothèque musicale, synchronise les appareils iOS et permet l'achat de musique/films. iTunesHelper détecte les connexions d'iPhone/iPad.",
            DisableImpact = "La détection automatique des iPhones/iPads ne fonctionnera pas. iTunes devra être lancé manuellement.",
            PerformanceImpact = "Faible pour iTunesHelper (~10 Mo RAM). iTunes complet : ~100-200 Mo RAM.",
            Recommendation = "Peut être désactivé si vous ne connectez pas régulièrement d'appareils Apple.",
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
            FullDescription = "Chrome peut être configuré pour démarrer automatiquement et restaurer vos onglets. Google Update vérifie les mises à jour du navigateur.",
            DisableImpact = "Chrome ne s'ouvrira pas automatiquement. Les mises à jour pourraient être retardées.",
            PerformanceImpact = "Très variable selon les extensions et onglets (~100-500+ Mo RAM).",
            Recommendation = "Peut être désactivé sauf si vous voulez restaurer automatiquement vos onglets.",
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
            FullDescription = "Edge est le navigateur par défaut de Windows, basé sur Chromium. Intègre des fonctionnalités comme Collections, le lecteur PDF et l'intégration Microsoft.",
            DisableImpact = "Edge ne démarrera pas automatiquement.",
            PerformanceImpact = "Variable (~80-400+ Mo RAM).",
            Recommendation = "Peut être désactivé si vous utilisez un autre navigateur.",
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
            FullDescription = "Brave est un navigateur basé sur Chromium qui bloque automatiquement les publicités et trackers. Offre des récompenses en crypto (BAT) pour les publicités opt-in et une protection renforcée de la vie privée.",
            DisableImpact = "Brave ne démarrera pas automatiquement.",
            PerformanceImpact = "Variable (~80-400+ Mo RAM).",
            Recommendation = "Peut être désactivé si vous ne voulez pas le démarrage automatique.",
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
            FullDescription = "Firefox est un navigateur axé sur la vie privée avec un bloqueur de trackers intégré. Le service de maintenance gère les mises à jour en arrière-plan.",
            DisableImpact = "Firefox ne démarrera pas automatiquement. Les mises à jour nécessiteront une élévation admin.",
            PerformanceImpact = "Variable (~80-400+ Mo RAM).",
            Recommendation = "Peut être désactivé. Gardez le service de maintenance si vous voulez des mises à jour silencieuses.",
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
            FullDescription = "Opera est un navigateur avec des fonctionnalités uniques : VPN gratuit intégré, sidebar pour les réseaux sociaux, bloqueur de pubs, et Workspaces pour organiser les onglets. Opera GX est la version gaming.",
            DisableImpact = "Opera ne démarrera pas automatiquement.",
            PerformanceImpact = "Variable (~80-400+ Mo RAM).",
            Recommendation = "Peut être désactivé si vous ne voulez pas le démarrage automatique.",
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
            FullDescription = "CCleaner nettoie les fichiers temporaires, le cache des navigateurs et les entrées de registre inutiles. La surveillance active optimise le système en arrière-plan.",
            DisableImpact = "Pas de nettoyage automatique. Vous devrez lancer CCleaner manuellement.",
            PerformanceImpact = "Faible (~15-30 Mo RAM).",
            Recommendation = "La surveillance active peut être désactivée. Lancez CCleaner manuellement de temps en temps.",
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
            FullDescription = "WinRAR permet de créer et extraire des archives RAR, ZIP et autres formats. Le démarrage automatique n'est généralement pas nécessaire.",
            DisableImpact = "Aucun impact. WinRAR fonctionne via le menu contextuel ou en ouvrant les archives.",
            PerformanceImpact = "Très faible au démarrage.",
            Recommendation = "Peut être désactivé. Le démarrage automatique est inutile pour WinRAR.",
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
            FullDescription = "7-Zip est une alternative gratuite à WinRAR, supportant de nombreux formats d'archives. Ne nécessite pas de démarrage automatique.",
            DisableImpact = "Aucun impact. 7-Zip fonctionne via le menu contextuel.",
            PerformanceImpact = "Aucun si non démarré.",
            Recommendation = "Devrait être désactivé s'il est dans le démarrage automatique.",
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
            FullDescription = "G HUB permet de configurer les souris, claviers, casques et volants gaming Logitech. Gère les profils de jeu, l'éclairage RGB LIGHTSYNC, les macros et les paramètres DPI.",
            DisableImpact = "Les périphériques fonctionneront avec les paramètres par défaut. Les profils personnalisés et l'éclairage RGB ne seront pas appliqués.",
            PerformanceImpact = "Modéré (~80-150 Mo RAM).",
            Recommendation = "Gardez activé si vous utilisez des périphériques Logitech avec des profils personnalisés.",
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
            FullDescription = "iCUE gère l'éclairage RGB, les profils de ventilateurs, les macros clavier/souris et les paramètres audio pour tous les périphériques Corsair (claviers, souris, casques, RAM RGB, refroidissement).",
            DisableImpact = "L'éclairage RGB reviendra aux effets par défaut. Les profils de ventilateurs et macros ne fonctionneront pas.",
            PerformanceImpact = "Modéré à élevé (~100-200 Mo RAM).",
            Recommendation = "Gardez activé si vous utilisez l'éclairage RGB ou des profils de ventilateurs personnalisés.",
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
            FullDescription = "SteelSeries GG configure les souris, claviers et casques SteelSeries. Inclut Sonar pour l'audio gaming et Moments pour l'enregistrement de clips.",
            DisableImpact = "Les périphériques utiliseront les paramètres par défaut. Pas de personnalisation RGB ou audio Sonar.",
            PerformanceImpact = "Modéré (~70-120 Mo RAM).",
            Recommendation = "Gardez activé si vous utilisez des périphériques SteelSeries avec des profils personnalisés.",
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
            FullDescription = "Razer Synapse 3 configure tous les périphériques Razer (souris, claviers, casques, tapis de souris RGB). Gère l'éclairage Chroma RGB, les macros, les profils de jeu et la synchronisation cloud des paramètres.",
            DisableImpact = "Les périphériques Razer fonctionneront avec les paramètres par défaut. Pas d'éclairage Chroma personnalisé ni de macros.",
            PerformanceImpact = "Modéré à élevé (~100-200 Mo RAM avec tous les modules).",
            Recommendation = "Gardez activé si vous utilisez des périphériques Razer. Peut être désactivé sinon.",
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
            FullDescription = "Waves Audio fournit le traitement audio avancé (DSP) sur de nombreux PC de marque (Dell, HP, Lenovo). Offre des améliorations sonores, l'égalisation et les effets MaxxAudio.",
            DisableImpact = "L'audio fonctionnera mais sans les améliorations Waves. Le son pourrait sembler plus plat.",
            PerformanceImpact = "Faible (~20-40 Mo RAM).",
            Recommendation = "Gardez activé si vous appréciez les améliorations audio. Peut être désactivé si vous préférez un son neutre.",
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
            FullDescription = "AMD Noise Suppression utilise l'IA pour filtrer le bruit de fond de votre microphone en temps réel. Similaire à NVIDIA RTX Voice mais pour les GPU AMD. Élimine les bruits de clavier, ventilateurs, etc.",
            DisableImpact = "Pas de réduction de bruit IA sur votre microphone.",
            PerformanceImpact = "Faible à modéré (~1-3% GPU selon le modèle).",
            Recommendation = "Gardez activé si vous faites des appels vocaux ou du streaming. Peut être désactivé sinon.",
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
            FullDescription = "Visual Studio Code est un éditeur de code gratuit très populaire chez les développeurs. Supporte de nombreux langages via extensions, le débogage intégré et l'intégration Git.",
            DisableImpact = "VS Code ne démarrera pas automatiquement. Aucun impact sur le fonctionnement.",
            PerformanceImpact = "Variable selon les extensions (~150-400 Mo RAM).",
            Recommendation = "Peut être désactivé. Le démarrage automatique est rarement nécessaire.",
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
            FullDescription = "Docker Desktop permet d'exécuter des conteneurs Linux et Windows sur votre PC. Utilisé massivement pour le développement, les tests et le déploiement d'applications.",
            DisableImpact = "Les conteneurs Docker ne démarreront pas automatiquement. Vous devrez lancer Docker manuellement.",
            PerformanceImpact = "Élevé (~500 Mo - 2 Go RAM selon les conteneurs actifs). Utilise WSL2 ou Hyper-V.",
            Recommendation = "Peut être désactivé si vous n'utilisez pas Docker quotidiennement. Lance-le manuellement quand nécessaire.",
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
            FullDescription = "Git est le système de gestion de version le plus utilisé au monde. Il ne démarre normalement pas automatiquement, c'est un outil en ligne de commande.",
            DisableImpact = "Aucun impact. Git est un outil à la demande.",
            PerformanceImpact = "Aucun au repos.",
            Recommendation = "Si Git apparaît dans le démarrage, il peut être désactivé en toute sécurité.",
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
            FullDescription = "Android Studio est l'IDE officiel de Google pour développer des applications Android. Inclut un émulateur Android et les outils SDK.",
            DisableImpact = "Aucun impact. Android Studio devrait être lancé manuellement.",
            PerformanceImpact = "Très élevé quand actif (~1-4 Go RAM avec émulateur).",
            Recommendation = "Si des services Android apparaissent au démarrage (ADB, émulateur), ils peuvent être désactivés.",
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
            FullDescription = "Toolbox gère l'installation et les mises à jour des IDE JetBrains (IntelliJ, PyCharm, WebStorm, etc.). Permet de lancer rapidement les projets récents.",
            DisableImpact = "Pas de mises à jour automatiques des IDE. Les IDE fonctionnent toujours.",
            PerformanceImpact = "Faible (~30-50 Mo RAM).",
            Recommendation = "Peut être désactivé si vous préférez lancer les IDE manuellement.",
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
            FullDescription = "Everything indexe instantanément tous les fichiers de vos disques NTFS. La recherche est quasi instantanée, bien plus rapide que Windows Search. L'indexation est légère car elle utilise la MFT du système de fichiers.",
            DisableImpact = "La recherche ne sera pas disponible immédiatement. L'index devra être reconstruit au lancement.",
            PerformanceImpact = "Très faible (~15-30 Mo RAM). Indexation quasi instantanée.",
            Recommendation = "Gardez activé pour une recherche instantanée. Excellente alternative à Windows Search.",
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
            FullDescription = "HWiNFO fournit des informations détaillées sur le matériel et surveille les capteurs (températures, voltages, vitesses de ventilateurs). Très utilisé pour le monitoring en temps réel.",
            DisableImpact = "Pas de monitoring des températures au démarrage. Les widgets de monitoring ne fonctionneront pas.",
            PerformanceImpact = "Faible (~20-40 Mo RAM).",
            Recommendation = "Peut être désactivé si vous ne surveillez pas les températures en permanence.",
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
            FullDescription = "Core Temp affiche la température de chaque cœur du processeur. Peut afficher la température dans la barre des tâches et alerter en cas de surchauffe.",
            DisableImpact = "Pas de surveillance de température CPU au démarrage.",
            PerformanceImpact = "Très faible (~5-10 Mo RAM).",
            Recommendation = "Peut être désactivé. Utile pour surveiller la température pendant les jeux ou le travail intensif.",
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
            FullDescription = "CrystalDiskInfo lit les données S.M.A.R.T. de vos disques durs et SSD pour évaluer leur santé. Peut alerter en cas de problème détecté sur un disque.",
            DisableImpact = "Pas de surveillance de la santé des disques au démarrage.",
            PerformanceImpact = "Très faible (~10-20 Mo RAM).",
            Recommendation = "Gardez activé pour être alerté des problèmes de disque. Peut aider à prévenir les pertes de données.",
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
            FullDescription = "Intel DSA détecte les composants Intel de votre système et propose les dernières mises à jour de pilotes. Inclut un service en arrière-plan qui vérifie régulièrement les mises à jour.",
            DisableImpact = "Pas de notification des mises à jour Intel. Les pilotes peuvent être mis à jour manuellement sur le site Intel.",
            PerformanceImpact = "Faible (~20-40 Mo RAM).",
            Recommendation = "Peut être désactivé. Les mises à jour importantes arrivent généralement via Windows Update.",
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
            FullDescription = "Samsung Magician permet de surveiller la santé des SSD Samsung, mettre à jour le firmware, activer les modes de performance et gérer le surapprovisionnement.",
            DisableImpact = "Pas de surveillance automatique du SSD. Les mises à jour de firmware devront être faites manuellement.",
            PerformanceImpact = "Faible (~30-50 Mo RAM).",
            Recommendation = "Peut être désactivé. Lancez-le occasionnellement pour vérifier les mises à jour de firmware.",
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
            FullDescription = "Audacity est un éditeur audio gratuit permettant d'enregistrer, éditer et mixer des pistes audio. Ne nécessite pas de démarrage automatique.",
            DisableImpact = "Aucun impact. Audacity est une application à lancer manuellement.",
            PerformanceImpact = "Aucun au repos.",
            Recommendation = "Si Audacity est dans le démarrage, il peut être désactivé en toute sécurité.",
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
            FullDescription = "OBS Studio permet le streaming en direct vers Twitch, YouTube, etc., ainsi que l'enregistrement local de vidéos. Très populaire chez les streamers et créateurs de contenu.",
            DisableImpact = "OBS ne démarrera pas automatiquement. Aucun impact sur le streaming.",
            PerformanceImpact = "Élevé pendant l'utilisation (encodage). Aucun impact au repos.",
            Recommendation = "Ne devrait pas démarrer automatiquement. Peut être désactivé en toute sécurité.",
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
            FullDescription = "VLC peut lire pratiquement tous les formats audio et vidéo sans codecs supplémentaires. Inclut des fonctionnalités de streaming et de conversion.",
            DisableImpact = "Aucun impact. VLC est un lecteur à lancer manuellement.",
            PerformanceImpact = "Aucun au repos.",
            Recommendation = "Ne devrait pas être dans le démarrage automatique. Peut être désactivé.",
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
            FullDescription = "qBittorrent est un client torrent gratuit et sans publicité. Permet de télécharger et partager des fichiers via le protocole BitTorrent.",
            DisableImpact = "Les téléchargements torrents ne reprendront pas automatiquement.",
            PerformanceImpact = "Variable selon l'activité (~30-100 Mo RAM, bande passante selon téléchargements).",
            Recommendation = "Peut être désactivé si vous ne téléchargez pas régulièrement des torrents.",
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
            FullDescription = "IDM accélère les téléchargements en les divisant en segments. S'intègre aux navigateurs pour capturer les téléchargements et les vidéos.",
            DisableImpact = "L'intégration avec les navigateurs ne fonctionnera pas. Les téléchargements devront être ajoutés manuellement.",
            PerformanceImpact = "Faible (~20-30 Mo RAM).",
            Recommendation = "Gardez activé si vous téléchargez fréquemment des fichiers volumineux.",
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
            FullDescription = "AutoHotkey permet de créer des raccourcis clavier personnalisés, des macros et d'automatiser des tâches répétitives. Les scripts peuvent démarrer automatiquement.",
            DisableImpact = "Vos scripts AutoHotkey ne s'exécuteront pas au démarrage. Les raccourcis personnalisés ne fonctionneront pas.",
            PerformanceImpact = "Très faible (~5-15 Mo RAM par script).",
            Recommendation = "Gardez activé si vous utilisez des scripts AHK. Vérifiez quels scripts sont lancés.",
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
            FullDescription = "PowerToys est une collection d'outils pour utilisateurs avancés : FancyZones (gestion des fenêtres), PowerRename, Color Picker, File Explorer add-ons, Keyboard Manager, PowerToys Run (lanceur), et plus encore.",
            DisableImpact = "Toutes les fonctionnalités PowerToys seront indisponibles : FancyZones, PowerToys Run (Alt+Space), raccourcis personnalisés, etc.",
            PerformanceImpact = "Faible à modéré (~50-100 Mo RAM selon les modules activés).",
            Recommendation = "Gardez activé si vous utilisez FancyZones ou PowerToys Run. Excellent outil de productivité.",
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
            FullDescription = "Textify permet de sélectionner et copier du texte depuis des boîtes de dialogue, messages d'erreur et autres éléments Windows normalement non sélectionnables. Très utile pour copier des messages d'erreur.",
            DisableImpact = "Vous ne pourrez plus extraire le texte des fenêtres non-sélectionnables avec le raccourci Textify.",
            PerformanceImpact = "Très faible (~5-10 Mo RAM).",
            Recommendation = "Peut être désactivé si vous ne l'utilisez pas régulièrement.",
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
            FullDescription = "YoloMouse remplace le curseur de souris dans les jeux par un curseur plus visible et personnalisable. Utile quand le curseur du jeu est trop petit ou se confond avec le décor.",
            DisableImpact = "Le curseur personnalisé ne sera pas disponible dans les jeux.",
            PerformanceImpact = "Très faible (~10-20 Mo RAM).",
            Recommendation = "Peut être désactivé si vous n'avez pas de problème à voir votre curseur dans les jeux.",
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
            FullDescription = "RoboForm stocke vos mots de passe de manière sécurisée et les remplit automatiquement sur les sites web. Offre aussi le remplissage de formulaires et la génération de mots de passe forts.",
            DisableImpact = "Pas de remplissage automatique des mots de passe. Vous devrez lancer RoboForm manuellement.",
            PerformanceImpact = "Faible (~30-50 Mo RAM).",
            Recommendation = "Gardez activé si vous utilisez RoboForm comme gestionnaire de mots de passe principal.",
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
            FullDescription = "VMware Workstation permet d'exécuter des machines virtuelles (Windows, Linux, etc.) sur votre PC. L'icône de la barre des tâches donne un accès rapide aux VMs et aux paramètres réseau virtuels.",
            DisableImpact = "Les VMs partagées ne démarreront pas automatiquement. Pas d'icône dans la barre des tâches.",
            PerformanceImpact = "Les services réseau utilisent ~20-40 Mo RAM même sans VM active.",
            Recommendation = "Peut être désactivé si vous n'utilisez pas de VMs partagées. Les VMs fonctionneront toujours.",
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
            FullDescription = "Vérifie et notifie les mises à jour disponibles pour les pilotes et logiciels de vos imprimantes/scanners Brother.",
            DisableImpact = "Pas de notification des mises à jour Brother. Vous devrez vérifier manuellement sur le site Brother.",
            PerformanceImpact = "Très faible (~10-20 Mo RAM).",
            Recommendation = "Peut être désactivé. Vérifiez occasionnellement les mises à jour sur le site Brother.",
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
            FullDescription = "Brother iPrint&Scan permet de numériser des documents, d'imprimer des photos et de gérer vos imprimantes Brother. ControlCenter offre un accès rapide aux fonctions de numérisation.",
            DisableImpact = "Pas de raccourci rapide pour la numérisation. L'imprimante fonctionne toujours normalement.",
            PerformanceImpact = "Faible (~20-40 Mo RAM).",
            Recommendation = "Peut être désactivé si vous ne numérisez pas régulièrement.",
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
            FullDescription = "HP Smart permet de configurer, surveiller et gérer vos imprimantes HP. Offre la numérisation, l'impression mobile, la commande de cartouches et le suivi de l'état de l'imprimante.",
            DisableImpact = "Pas de notifications d'encre faible. L'imprimante fonctionne toujours normalement.",
            PerformanceImpact = "Modéré (~50-100 Mo RAM).",
            Recommendation = "Peut être désactivé. Lancez HP Smart manuellement quand nécessaire.",
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
            FullDescription = "HP Scan fournit une interface pour numériser des documents et photos avec les scanners HP. Offre des préréglages de numérisation et l'OCR.",
            DisableImpact = "Pas d'icône de raccourci. La numérisation est toujours possible via HP Smart ou Windows.",
            PerformanceImpact = "Faible (~20-30 Mo RAM).",
            Recommendation = "Peut être désactivé si vous n'utilisez pas le scanner fréquemment.",
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
            FullDescription = "Canon My Printer permet de configurer les paramètres d'imprimante par défaut, vérifier les niveaux d'encre et diagnostiquer les problèmes d'impression Canon.",
            DisableImpact = "Pas d'accès rapide aux réglages. L'imprimante fonctionne normalement.",
            PerformanceImpact = "Faible (~15-30 Mo RAM).",
            Recommendation = "Peut être désactivé. Lancez-le manuellement si besoin.",
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
            FullDescription = "Canon IJ Scan Utility offre une interface de numérisation complète pour les multifonctions Canon. Permet de numériser en PDF, JPEG, avec OCR et détection automatique du type de document.",
            DisableImpact = "Pas de raccourci de numérisation. La numérisation reste possible via Windows.",
            PerformanceImpact = "Faible (~20-40 Mo RAM).",
            Recommendation = "Peut être désactivé si vous n'utilisez pas souvent le scanner.",
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
            FullDescription = "Epson Scan 2 offre des options de numérisation avancées : correction des couleurs, suppression de la poussière, numérisation de films/négatifs, et préréglages personnalisables.",
            DisableImpact = "Pas de raccourci de numérisation. Utilisez Windows Scan ou lancez Epson Scan manuellement.",
            PerformanceImpact = "Faible (~20-40 Mo RAM).",
            Recommendation = "Peut être désactivé si vous ne numérisez pas régulièrement.",
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
            FullDescription = "Epson Status Monitor surveille l'état de l'imprimante : niveaux d'encre, erreurs, et progression des travaux d'impression. Affiche des alertes quand l'encre est faible.",
            DisableImpact = "Pas d'alertes d'encre faible ou d'erreurs. L'impression fonctionne normalement.",
            PerformanceImpact = "Très faible (~10-20 Mo RAM).",
            Recommendation = "Peut être désactivé. Vérifiez manuellement les niveaux d'encre.",
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
            FullDescription = "Epson Software Updater vérifie et télécharge les mises à jour de pilotes et logiciels pour vos produits Epson.",
            DisableImpact = "Pas de notification des mises à jour Epson.",
            PerformanceImpact = "Très faible (~10-15 Mo RAM).",
            Recommendation = "Peut être désactivé. Vérifiez les mises à jour sur le site Epson occasionnellement.",
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
            FullDescription = "Samsung Printer Experience (maintenant repris par HP) gère les imprimantes Samsung : numérisation, paramètres, niveaux de toner et diagnostics.",
            DisableImpact = "Pas de notifications de toner faible. L'imprimante fonctionne normalement.",
            PerformanceImpact = "Faible (~20-40 Mo RAM).",
            Recommendation = "Peut être désactivé. Samsung a été racheté par HP, utilisez HP Smart pour les nouvelles imprimantes.",
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
            FullDescription = "Le logiciel Lexmark permet de surveiller l'état de l'imprimante, les niveaux de toner/encre et de configurer les paramètres d'impression.",
            DisableImpact = "Pas de surveillance d'état. L'impression fonctionne normalement.",
            PerformanceImpact = "Faible (~15-30 Mo RAM).",
            Recommendation = "Peut être désactivé si vous n'avez pas besoin des alertes.",
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
            FullDescription = "Xerox Print Experience offre une interface moderne pour imprimer, numériser et gérer vos imprimantes Xerox. Inclut des fonctionnalités de workflow et de conversion.",
            DisableImpact = "Pas de raccourcis Xerox. L'impression via Windows fonctionne normalement.",
            PerformanceImpact = "Modéré (~40-70 Mo RAM).",
            Recommendation = "Peut être désactivé si vous n'utilisez pas les fonctionnalités avancées.",
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
            FullDescription = "PaperPort est un logiciel de gestion documentaire qui permet de numériser, organiser et convertir des documents. Offre l'OCR et la création de PDF recherchables. Souvent fourni avec les scanners.",
            DisableImpact = "Pas de lancement automatique de PaperPort. La numérisation est toujours possible.",
            PerformanceImpact = "Modéré (~50-100 Mo RAM).",
            Recommendation = "Peut être désactivé. Lancez PaperPort manuellement quand vous numérisez.",
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
            FullDescription = "ABBYY FineReader est l'un des meilleurs logiciels OCR. Convertit les documents numérisés et PDF en fichiers éditables avec une grande précision, même pour les documents complexes.",
            DisableImpact = "Pas de raccourcis OCR. Lancez FineReader manuellement.",
            PerformanceImpact = "Faible au repos (~20-40 Mo RAM).",
            Recommendation = "Peut être désactivé si vous n'utilisez pas l'OCR fréquemment.",
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
            FullDescription = "NAPS2 (Not Another PDF Scanner) est un outil gratuit pour numériser vers PDF, TIFF ou images. Simple mais puissant avec OCR intégré et support de tous les scanners TWAIN/WIA.",
            DisableImpact = "Aucun impact. NAPS2 est un outil à lancer manuellement.",
            PerformanceImpact = "Aucun au repos.",
            Recommendation = "Ne devrait pas être dans le démarrage automatique.",
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
            FullDescription = "VueScan est un logiciel de numérisation qui supporte plus de 7000 scanners, y compris les anciens modèles sans pilotes Windows récents. Excellent pour la numérisation de films et photos.",
            DisableImpact = "Aucun impact. VueScan est lancé manuellement.",
            PerformanceImpact = "Aucun au repos.",
            Recommendation = "Ne devrait pas être dans le démarrage automatique.",
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
            FullDescription = "Le pilote Wacom est essentiel pour les tablettes graphiques Wacom (Intuos, Cintiq, etc.). Gère la pression du stylet, les boutons personnalisés, les raccourcis et les paramètres par application.",
            DisableImpact = "La tablette graphique pourrait ne pas fonctionner correctement. Perte de la sensibilité à la pression et des boutons personnalisés.",
            PerformanceImpact = "Faible (~30-50 Mo RAM).",
            Recommendation = "Gardez activé si vous utilisez une tablette Wacom. Essentiel pour les artistes et designers.",
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
            FullDescription = "Le pilote XP-Pen configure les tablettes graphiques XP-Pen : sensibilité à la pression, touches express, paramètres de zone de travail et raccourcis par application.",
            DisableImpact = "La tablette pourrait ne pas fonctionner correctement. Perte de la pression et des boutons.",
            PerformanceImpact = "Faible (~20-40 Mo RAM).",
            Recommendation = "Gardez activé si vous utilisez une tablette XP-Pen.",
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
            FullDescription = "Le pilote Huion gère les tablettes graphiques Huion : sensibilité à la pression (8192+ niveaux), touches programmables et paramètres d'affichage pour les pen displays.",
            DisableImpact = "La tablette ne fonctionnera pas correctement sans le pilote actif.",
            PerformanceImpact = "Faible (~20-40 Mo RAM).",
            Recommendation = "Gardez activé si vous utilisez une tablette Huion.",
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
            FullDescription = "Le pilote Gaomon configure les tablettes graphiques Gaomon avec les paramètres de pression, les touches express et la zone de travail.",
            DisableImpact = "La tablette ne fonctionnera pas correctement.",
            PerformanceImpact = "Faible (~15-30 Mo RAM).",
            Recommendation = "Gardez activé si vous utilisez une tablette Gaomon.",
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
            FullDescription = "Logitech Capture permet de configurer les webcams Logitech (Brio, C920, StreamCam, etc.), de capturer des vidéos, d'ajouter des effets et de diffuser en direct.",
            DisableImpact = "Pas de configuration avancée de la webcam. La webcam fonctionne toujours avec les paramètres par défaut.",
            PerformanceImpact = "Modéré (~60-100 Mo RAM).",
            Recommendation = "Peut être désactivé si vous n'utilisez pas les fonctionnalités avancées de capture.",
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
            FullDescription = "Elgato Camera Hub configure les paramètres avancés des webcams Elgato Facecam : exposition, balance des blancs, HDR, zoom et cadrage.",
            DisableImpact = "Les paramètres personnalisés de la webcam ne seront pas appliqués.",
            PerformanceImpact = "Faible (~30-50 Mo RAM).",
            Recommendation = "Gardez activé si vous utilisez une Elgato Facecam avec des paramètres personnalisés.",
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
            FullDescription = "Stream Deck configure les boutons LCD programmables du Stream Deck. Permet de créer des raccourcis, contrôler OBS, gérer les scènes de stream et automatiser des actions.",
            DisableImpact = "Le Stream Deck n'affichera pas les boutons personnalisés et les actions ne fonctionneront pas.",
            PerformanceImpact = "Modéré (~50-80 Mo RAM).",
            Recommendation = "Gardez activé si vous utilisez un Stream Deck. Essentiel pour le streaming.",
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
            FullDescription = "Wave Link est un mixeur audio virtuel pour les microphones Elgato Wave. Permet de mixer plusieurs sources audio, d'appliquer des effets et de gérer le monitoring pour le streaming.",
            DisableImpact = "Le mixage audio Wave Link ne sera pas disponible. Le micro fonctionnera comme un périphérique USB standard.",
            PerformanceImpact = "Modéré (~50-80 Mo RAM).",
            Recommendation = "Gardez activé si vous utilisez un micro Elgato Wave pour le streaming.",
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
            FullDescription = "Blue Sherpa configure les microphones Blue (maintenant Logitech) : gain, pattern de capture, monitoring. Blue VO!CE ajoute des effets vocaux et la suppression de bruit.",
            DisableImpact = "Pas d'accès aux paramètres avancés du micro. Les paramètres de base restent via Windows.",
            PerformanceImpact = "Faible (~30-50 Mo RAM).",
            Recommendation = "Peut être désactivé si vous n'utilisez pas Blue VO!CE ou les paramètres avancés.",
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
            FullDescription = "RØDE Central configure les microphones RØDE (NT-USB, PodMic USB, etc.) et le RØDECaster. Permet les mises à jour firmware, la configuration audio et l'enregistrement.",
            DisableImpact = "Pas d'accès aux paramètres RØDE. Le micro fonctionne avec les paramètres par défaut.",
            PerformanceImpact = "Faible (~30-50 Mo RAM).",
            Recommendation = "Peut être désactivé si vous n'avez pas besoin des fonctionnalités avancées.",
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
            FullDescription = "L'app GoXLR contrôle le mixeur audio GoXLR/GoXLR Mini : routage audio, effets vocaux, sampler, et intégration streaming. Essentiel pour utiliser le matériel.",
            DisableImpact = "Le GoXLR ne fonctionnera pas correctement sans l'application.",
            PerformanceImpact = "Modéré (~60-100 Mo RAM).",
            Recommendation = "Gardez activé si vous possédez un GoXLR. Essentiel pour son fonctionnement.",
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
            FullDescription = "VoiceMeeter est un mixeur audio virtuel qui permet de router, mixer et traiter plusieurs sources audio. Très populaire pour le streaming et le podcasting. Versions Banana et Potato pour plus de canaux.",
            DisableImpact = "Tout le routage audio VoiceMeeter sera désactivé. Les applications utilisant les entrées/sorties virtuelles n'auront plus d'audio.",
            PerformanceImpact = "Faible (~20-40 Mo RAM).",
            Recommendation = "Gardez activé si votre configuration audio dépend de VoiceMeeter.",
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
            FullDescription = "NordVPN chiffre votre connexion internet et masque votre adresse IP. Offre des serveurs dans 60+ pays, le double VPN, et la protection contre les menaces (CyberSec).",
            DisableImpact = "Le VPN ne se connectera pas automatiquement au démarrage. Vous devrez le lancer manuellement.",
            PerformanceImpact = "Faible (~40-60 Mo RAM). Peut légèrement réduire la vitesse internet.",
            Recommendation = "Gardez activé si vous voulez une connexion VPN permanente. Peut être désactivé sinon.",
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
            FullDescription = "ExpressVPN offre des connexions rapides avec serveurs dans 94 pays. Connu pour sa fiabilité, son support du streaming et sa politique no-logs.",
            DisableImpact = "Le VPN ne se connectera pas automatiquement.",
            PerformanceImpact = "Faible (~40-60 Mo RAM).",
            Recommendation = "Gardez activé pour une protection VPN permanente.",
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
            FullDescription = "Surfshark permet des connexions simultanées illimitées. Inclut CleanWeb (bloqueur de pubs), MultiHop et mode camouflage.",
            DisableImpact = "Le VPN ne démarrera pas automatiquement.",
            PerformanceImpact = "Faible (~30-50 Mo RAM).",
            Recommendation = "Peut être désactivé si vous préférez lancer le VPN manuellement.",
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
            FullDescription = "CyberGhost offre des profils préconfigurés pour le streaming, le torrent et la navigation. Interface simple avec 7000+ serveurs.",
            DisableImpact = "Le VPN ne se connectera pas au démarrage.",
            PerformanceImpact = "Faible (~40-60 Mo RAM).",
            Recommendation = "Peut être désactivé si vous n'avez pas besoin d'un VPN permanent.",
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
            FullDescription = "ProtonVPN est créé par les développeurs de ProtonMail. Offre un niveau gratuit, Secure Core (double VPN via pays sûrs) et une politique stricte no-logs.",
            DisableImpact = "Le VPN ne se connectera pas automatiquement.",
            PerformanceImpact = "Faible (~30-50 Mo RAM).",
            Recommendation = "Gardez activé pour une protection permanente.",
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
            FullDescription = "PIA est un VPN qui a prouvé sa politique no-logs devant les tribunaux. Offre le port forwarding, un bloqueur de pubs (MACE) et WireGuard.",
            DisableImpact = "Le VPN ne démarrera pas automatiquement.",
            PerformanceImpact = "Faible (~30-50 Mo RAM).",
            Recommendation = "Peut être désactivé si vous lancez le VPN manuellement.",
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
            FullDescription = "Windscribe offre 10 Go/mois gratuits. Inclut R.O.B.E.R.T. (bloqueur de pubs/malware), split tunneling et le mode pont pour contourner les censures.",
            DisableImpact = "Le VPN ne se connectera pas au démarrage.",
            PerformanceImpact = "Faible (~30-50 Mo RAM).",
            Recommendation = "Peut être désactivé si vous n'utilisez pas le VPN en permanence.",
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
            FullDescription = "Mullvad est réputé pour sa confidentialité maximale : pas d'email requis, paiement en cash/crypto possible, numéro de compte anonyme. Recommandé par les experts en sécurité.",
            DisableImpact = "Le VPN ne démarrera pas automatiquement.",
            PerformanceImpact = "Faible (~20-40 Mo RAM).",
            Recommendation = "Gardez activé pour une protection VPN permanente.",
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
            FullDescription = "1Password stocke vos mots de passe, cartes de crédit et notes sécurisées. Offre Watchtower (surveillance des fuites), le partage familial et l'intégration navigateur.",
            DisableImpact = "Pas de remplissage automatique au démarrage. Vous devrez lancer 1Password manuellement.",
            PerformanceImpact = "Faible (~50-80 Mo RAM).",
            Recommendation = "Gardez activé pour un remplissage automatique des mots de passe.",
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
            FullDescription = "Bitwarden est un gestionnaire de mots de passe gratuit et open source. Offre le stockage illimité, la synchronisation multi-appareils et peut être auto-hébergé.",
            DisableImpact = "Pas de raccourci rapide. L'extension navigateur fonctionne indépendamment.",
            PerformanceImpact = "Faible (~40-60 Mo RAM).",
            Recommendation = "Peut être désactivé si vous utilisez principalement l'extension navigateur.",
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
            FullDescription = "LastPass stocke vos mots de passe dans le cloud avec chiffrement local. Offre le remplissage automatique, le générateur de mots de passe et le partage sécurisé.",
            DisableImpact = "Pas de remplissage automatique hors navigateur.",
            PerformanceImpact = "Faible (~40-60 Mo RAM).",
            Recommendation = "L'extension navigateur fonctionne sans l'application desktop.",
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
            FullDescription = "Dashlane offre la gestion des mots de passe, la surveillance du dark web, et un VPN intégré dans les plans premium.",
            DisableImpact = "Pas de remplissage automatique hors navigateur.",
            PerformanceImpact = "Faible (~50-70 Mo RAM).",
            Recommendation = "Peut être désactivé si vous utilisez l'extension navigateur.",
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
            FullDescription = "KeePass stocke vos mots de passe localement dans un fichier chiffré. Pas de cloud, contrôle total. KeePassXC est une version cross-platform moderne.",
            DisableImpact = "La base de données ne sera pas ouverte automatiquement.",
            PerformanceImpact = "Très faible (~20-40 Mo RAM).",
            Recommendation = "Peut être désactivé si vous ouvrez KeePass manuellement.",
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
            FullDescription = "Acronis True Image offre la sauvegarde complète du système, le clonage de disque, la sauvegarde cloud et la protection anti-ransomware. Permet de restaurer un système complet.",
            DisableImpact = "Les sauvegardes planifiées ne s'exécuteront pas automatiquement.",
            PerformanceImpact = "Modéré (~80-150 Mo RAM avec les services).",
            Recommendation = "Gardez activé si vous avez des sauvegardes planifiées.",
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
            FullDescription = "EaseUS Todo Backup permet de sauvegarder fichiers, partitions ou disques entiers. Offre le clonage de disque et la création de médias de récupération.",
            DisableImpact = "Les sauvegardes planifiées ne s'exécuteront pas.",
            PerformanceImpact = "Faible (~30-60 Mo RAM).",
            Recommendation = "Gardez activé pour les sauvegardes automatiques.",
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
            FullDescription = "Macrium Reflect crée des images de disque pour sauvegarde et restauration. Connu pour sa fiabilité et sa version gratuite fonctionnelle. Excellent pour cloner vers SSD.",
            DisableImpact = "Les sauvegardes planifiées ne fonctionneront pas.",
            PerformanceImpact = "Faible (~30-50 Mo RAM).",
            Recommendation = "Gardez activé si vous utilisez les sauvegardes planifiées.",
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
            FullDescription = "Backblaze sauvegarde automatiquement tous vos fichiers vers le cloud pour un prix fixe. Stockage illimité, restauration par envoi de disque dur possible.",
            DisableImpact = "Vos fichiers ne seront plus sauvegardés automatiquement vers le cloud.",
            PerformanceImpact = "Faible à modéré (~30-60 Mo RAM). Utilise la bande passante en arrière-plan.",
            Recommendation = "Gardez activé pour une sauvegarde continue. C'est son but.",
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
            FullDescription = "Carbonite sauvegarde automatiquement vos fichiers vers le cloud. Offre la restauration de fichiers, la protection contre les ransomwares et le support technique.",
            DisableImpact = "Les sauvegardes cloud s'arrêteront.",
            PerformanceImpact = "Faible (~30-50 Mo RAM).",
            Recommendation = "Gardez activé pour la sauvegarde continue.",
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
            FullDescription = "Veeam Agent offre une sauvegarde gratuite de niveau entreprise pour les particuliers. Sauvegarde vers disque local, NAS ou cloud avec restauration bare-metal.",
            DisableImpact = "Les sauvegardes planifiées ne s'exécuteront pas.",
            PerformanceImpact = "Faible (~30-50 Mo RAM).",
            Recommendation = "Gardez activé pour les sauvegardes automatiques.",
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
            FullDescription = "TeamViewer permet de contrôler des PC à distance, transférer des fichiers, faire des réunions en ligne et fournir du support technique. Gratuit pour usage personnel.",
            DisableImpact = "Les connexions entrantes ne seront pas possibles. Le PC ne sera pas accessible à distance.",
            PerformanceImpact = "Faible au repos (~30-50 Mo RAM).",
            Recommendation = "Désactivez si vous n'avez pas besoin d'accès distant entrant. Peut être lancé manuellement.",
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
            FullDescription = "AnyDesk est un logiciel de bureau à distance léger et rapide. Connu pour sa faible latence et sa fluidité, même sur des connexions lentes.",
            DisableImpact = "Pas d'accès distant entrant sans lancer AnyDesk.",
            PerformanceImpact = "Très faible (~15-30 Mo RAM).",
            Recommendation = "Peut être désactivé si vous n'avez pas besoin d'accès permanent.",
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
            FullDescription = "Parsec est optimisé pour le streaming de jeux avec une latence ultra-faible. Permet de jouer à vos jeux PC depuis n'importe où ou d'héberger des sessions multijoueur locales à distance.",
            DisableImpact = "Le PC ne sera pas accessible pour le streaming de jeux.",
            PerformanceImpact = "Faible au repos (~20-40 Mo RAM). Élevé pendant le streaming.",
            Recommendation = "Gardez activé si vous utilisez Parsec pour le cloud gaming.",
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
            FullDescription = "Chrome Remote Desktop permet d'accéder à votre PC depuis n'importe quel navigateur Chrome ou l'application mobile. Gratuit et simple à configurer.",
            DisableImpact = "Le PC ne sera plus accessible via Chrome Remote Desktop.",
            PerformanceImpact = "Très faible (~10-20 Mo RAM).",
            Recommendation = "Peut être désactivé si vous n'utilisez pas l'accès distant Chrome.",
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
            FullDescription = "RustDesk est une solution de bureau à distance open source. Peut être auto-hébergé pour un contrôle total. Alternative gratuite aux solutions propriétaires.",
            DisableImpact = "Le PC ne sera pas accessible à distance.",
            PerformanceImpact = "Très faible (~15-25 Mo RAM).",
            Recommendation = "Peut être désactivé si vous n'avez pas besoin d'accès distant.",
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
            FullDescription = "L'application Oculus (Meta Quest) gère vos casques VR Meta, la bibliothèque de jeux, le Link/Air Link pour jouer aux jeux PC VR et les paramètres du Guardian.",
            DisableImpact = "Les services VR ne démarreront pas automatiquement. Vous devrez lancer l'app avant d'utiliser le casque.",
            PerformanceImpact = "Modéré à élevé (~150-300 Mo RAM avec les services).",
            Recommendation = "Peut être désactivé si vous n'utilisez pas la VR quotidiennement.",
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
            FullDescription = "SteamVR est la plateforme VR universelle de Valve. Compatible avec la plupart des casques (Valve Index, HTC Vive, Oculus, WMR). Gère le tracking, les contrôleurs et la bibliothèque VR.",
            DisableImpact = "SteamVR ne démarrera pas automatiquement. Lancé par Steam quand nécessaire.",
            PerformanceImpact = "Élevé en utilisation (~200-400 Mo RAM).",
            Recommendation = "Peut être désactivé du démarrage. Steam le lance automatiquement.",
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
            FullDescription = "Vive Console gère les casques HTC Vive : configuration du tracking, des contrôleurs, mise à jour du firmware. Viveport est la boutique de jeux VR HTC.",
            DisableImpact = "Le casque Vive devra être configuré manuellement avant utilisation.",
            PerformanceImpact = "Modéré (~100-200 Mo RAM).",
            Recommendation = "Peut être désactivé si vous n'utilisez pas fréquemment la VR.",
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
            FullDescription = "Windows Mixed Reality gère les casques VR WMR (HP Reverb, Samsung Odyssey, etc.). Intégré à Windows 10/11 avec tracking inside-out.",
            DisableImpact = "Le portail MR ne démarrera pas automatiquement.",
            PerformanceImpact = "Modéré (~100-150 Mo RAM).",
            Recommendation = "Peut être désactivé si vous n'utilisez pas la VR fréquemment.",
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
            FullDescription = "Virtual Desktop permet de streamer votre bureau Windows et vos jeux VR vers un casque standalone (Quest) en Wi-Fi. Alternative à Oculus Link/Air Link.",
            DisableImpact = "Le streaming vers le casque Quest ne sera pas disponible immédiatement.",
            PerformanceImpact = "Faible au repos (~30-50 Mo RAM).",
            Recommendation = "Peut être désactivé si vous n'utilisez pas le streaming VR.",
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
            FullDescription = "Le Rockstar Games Launcher est requis pour jouer aux jeux Rockstar (GTA V, Red Dead Redemption 2, etc.). Gère les téléchargements, mises à jour et le Social Club.",
            DisableImpact = "Les jeux Rockstar ne pourront pas se lancer sans le lanceur. Démarrez-le manuellement.",
            PerformanceImpact = "Modéré (~60-100 Mo RAM).",
            Recommendation = "Peut être désactivé si vous ne jouez pas souvent aux jeux Rockstar.",
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
            FullDescription = "Le Riot Client gère vos jeux Riot (LoL, Valorant, Legends of Runeterra, etc.). Vanguard est l'anti-cheat de Valorant qui fonctionne au niveau kernel.",
            DisableImpact = "Les jeux Riot ne pourront pas se lancer. Vanguard doit tourner pour jouer à Valorant.",
            PerformanceImpact = "Vanguard : ~50 Mo RAM, toujours actif. Riot Client : ~80-120 Mo RAM.",
            Recommendation = "Vanguard peut être désactivé si vous ne jouez pas à Valorant, mais il redémarrera le PC.",
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
            FullDescription = "Amazon Games gère les jeux Amazon (New World, Lost Ark) et les jeux gratuits Prime Gaming. Télécharge et met à jour les jeux.",
            DisableImpact = "Les jeux Amazon devront être lancés manuellement.",
            PerformanceImpact = "Faible (~40-70 Mo RAM).",
            Recommendation = "Peut être désactivé si vous ne récupérez pas souvent les jeux Prime.",
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
            FullDescription = "Playnite agrège tous vos jeux de Steam, Epic, GOG, Origin, etc. en une seule bibliothèque. Offre un mode Big Picture et de nombreuses personnalisations.",
            DisableImpact = "Playnite ne s'ouvrira pas au démarrage. Aucun impact sur les launchers individuels.",
            PerformanceImpact = "Faible (~40-80 Mo RAM).",
            Recommendation = "Peut être désactivé si vous préférez lancer Playnite manuellement.",
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
            FullDescription = "Le Bethesda Launcher gérait les jeux Bethesda (Fallout, Elder Scrolls, DOOM). Note : Bethesda a migré vers Steam en 2022, ce lanceur est obsolète.",
            DisableImpact = "Aucun impact car les jeux ont été migrés vers Steam.",
            PerformanceImpact = "Faible (~30-50 Mo RAM).",
            Recommendation = "Peut être désinstallé car obsolète. Les jeux sont maintenant sur Steam.",
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
            FullDescription = "itch.io est une plateforme pour les jeux indépendants avec un modèle 'payez ce que vous voulez'. L'app desktop facilite le téléchargement et la mise à jour des jeux.",
            DisableImpact = "L'app itch.io ne démarrera pas automatiquement.",
            PerformanceImpact = "Faible (~40-60 Mo RAM).",
            Recommendation = "Peut être désactivé si vous n'achetez pas souvent sur itch.io.",
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
            FullDescription = "Notion combine notes, documents, bases de données, kanban et wikis. Très flexible pour la gestion de projets personnels ou en équipe.",
            DisableImpact = "Notion ne s'ouvrira pas automatiquement. Accès toujours possible via navigateur.",
            PerformanceImpact = "Modéré (~100-200 Mo RAM basé sur Electron).",
            Recommendation = "Peut être désactivé si vous préférez la version web.",
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
            FullDescription = "Obsidian stocke vos notes en fichiers Markdown locaux avec des liens bidirectionnels pour créer un 'second cerveau'. Hautement personnalisable avec des plugins.",
            DisableImpact = "Obsidian ne démarrera pas automatiquement.",
            PerformanceImpact = "Modéré (~80-150 Mo RAM).",
            Recommendation = "Peut être désactivé si vous n'avez pas besoin d'y accéder au démarrage.",
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
            FullDescription = "Evernote synchronise vos notes entre appareils avec OCR sur les images, Web Clipper et recherche puissante. Un des pionniers de la prise de notes numériques.",
            DisableImpact = "Pas de capture rapide au démarrage. Les notes restent accessibles via l'app ou le web.",
            PerformanceImpact = "Modéré (~100-200 Mo RAM).",
            Recommendation = "Peut être désactivé si vous n'utilisez pas le raccourci de capture.",
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
            FullDescription = "Joplin est une alternative open source à Evernote. Supporte Markdown, le chiffrement bout en bout et la synchronisation via Dropbox, OneDrive ou Nextcloud.",
            DisableImpact = "Joplin ne démarrera pas automatiquement.",
            PerformanceImpact = "Modéré (~80-150 Mo RAM).",
            Recommendation = "Peut être désactivé si vous n'avez pas besoin d'accès immédiat.",
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
            FullDescription = "Adobe Acrobat Reader est le lecteur PDF standard. Permet de lire, annoter, signer et remplir des formulaires PDF. AdobeARM gère les mises à jour.",
            DisableImpact = "Pas de mise à jour automatique. Les PDF s'ouvrent toujours normalement.",
            PerformanceImpact = "Faible (~30-50 Mo RAM pour le service de mise à jour).",
            Recommendation = "Le service de mise à jour peut être désactivé. Mettez à jour manuellement.",
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
            FullDescription = "Acrobat Pro permet de créer, modifier, convertir et signer des PDF. Inclut l'OCR, la fusion de documents et les formulaires avancés.",
            DisableImpact = "Les services de synchronisation et mise à jour ne démarreront pas.",
            PerformanceImpact = "Modéré (~50-100 Mo RAM avec les services).",
            Recommendation = "Les services de démarrage peuvent être désactivés sans impact sur l'édition PDF.",
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
            FullDescription = "Foxit PDF Reader est une alternative légère à Adobe Reader. Offre lecture, annotations, signatures et formulaires. PhantomPDF ajoute l'édition complète.",
            DisableImpact = "Aucun impact. Foxit ne devrait pas être dans le démarrage automatique.",
            PerformanceImpact = "Aucun au repos.",
            Recommendation = "Si dans le démarrage, peut être désactivé en toute sécurité.",
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
            FullDescription = "PDF-XChange Editor offre de nombreuses fonctionnalités gratuites : édition de texte, annotations, OCR, comparaison de documents. Alternative puissante à Acrobat.",
            DisableImpact = "Aucun impact. L'application devrait être lancée manuellement.",
            PerformanceImpact = "Aucun au repos.",
            Recommendation = "Ne devrait pas être dans le démarrage automatique.",
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
            FullDescription = "Nitro Pro est une alternative à Acrobat Pro pour créer, éditer et convertir des PDF. Interface familière style Office et intégration cloud.",
            DisableImpact = "Les services Nitro ne démarreront pas automatiquement.",
            PerformanceImpact = "Faible (~20-40 Mo RAM).",
            Recommendation = "Les services de démarrage peuvent être désactivés.",
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
            FullDescription = "SumatraPDF est un lecteur PDF minimaliste et rapide. Supporte aussi ePub, Mobi, XPS et autres formats. Portable, pas d'installation requise.",
            DisableImpact = "Aucun impact. SumatraPDF ne devrait pas être dans le démarrage.",
            PerformanceImpact = "Aucun au repos.",
            Recommendation = "Ne devrait pas être dans le démarrage automatique.",
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
            FullDescription = "ShareX est un outil de capture d'écran open source très complet : capture de région, fenêtre, scroll, GIF, vidéo. Upload automatique vers de nombreux services. Éditeur d'image intégré.",
            DisableImpact = "Les raccourcis de capture ne fonctionneront pas au démarrage.",
            PerformanceImpact = "Faible (~30-50 Mo RAM).",
            Recommendation = "Gardez activé si vous utilisez régulièrement les captures d'écran.",
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
            FullDescription = "Greenshot est un outil de capture d'écran simple mais efficace. Capture de région, fenêtre, plein écran avec éditeur intégré et export direct vers applications.",
            DisableImpact = "Les raccourcis de capture ne seront pas disponibles.",
            PerformanceImpact = "Très faible (~15-25 Mo RAM).",
            Recommendation = "Gardez activé si vous utilisez les raccourcis de capture.",
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
            FullDescription = "Lightshot permet de capturer une zone de l'écran rapidement, d'annoter et de partager via un lien. Simple et efficace pour le partage rapide.",
            DisableImpact = "Le raccourci Print Screen modifié ne fonctionnera plus.",
            PerformanceImpact = "Très faible (~10-20 Mo RAM).",
            Recommendation = "Gardez activé si vous utilisez Print Screen pour Lightshot.",
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
            FullDescription = "Snagit est un outil de capture premium avec éditeur puissant, capture vidéo, scrolling capture, templates et intégrations professionnelles.",
            DisableImpact = "Les raccourcis de capture Snagit ne seront pas disponibles.",
            PerformanceImpact = "Modéré (~50-100 Mo RAM).",
            Recommendation = "Gardez activé si vous utilisez Snagit régulièrement.",
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
            FullDescription = "Flameshot est un outil de capture open source avec annotations en temps réel. Interface simple avec upload vers Imgur et autres services.",
            DisableImpact = "Les raccourcis de capture ne fonctionneront pas.",
            PerformanceImpact = "Très faible (~15-25 Mo RAM).",
            Recommendation = "Gardez activé si vous utilisez les raccourcis de capture.",
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
            FullDescription = "Thunderbird est un client email complet supportant IMAP, POP3, Exchange. Inclut calendrier, contacts, chat et de nombreuses extensions. Alternative gratuite à Outlook.",
            DisableImpact = "Pas de notifications email au démarrage. Lancez Thunderbird manuellement.",
            PerformanceImpact = "Modéré (~100-200 Mo RAM).",
            Recommendation = "Peut être désactivé si vous préférez lancer Thunderbird manuellement.",
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
            FullDescription = "Mailbird est un client email élégant avec boîte de réception unifiée, intégrations apps (WhatsApp, Slack, Todoist) et recherche rapide.",
            DisableImpact = "Pas de notifications email au démarrage.",
            PerformanceImpact = "Modéré (~80-150 Mo RAM).",
            Recommendation = "Peut être désactivé si vous ne voulez pas de notifications email.",
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
            FullDescription = "eM Client offre email, calendrier, contacts et tâches dans une interface unifiée. Supporte Gmail, Outlook.com, Exchange et autres services.",
            DisableImpact = "Pas de notifications au démarrage.",
            PerformanceImpact = "Modéré (~100-180 Mo RAM).",
            Recommendation = "Peut être désactivé si vous préférez le lancer manuellement.",
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
            FullDescription = "OpenRGB unifie le contrôle de l'éclairage RGB de tous vos périphériques (cartes mères, RAM, GPU, périphériques) sans bloatware constructeur. Remplace Aura, iCUE, etc.",
            DisableImpact = "L'éclairage RGB reviendra aux effets par défaut ou restera éteint.",
            PerformanceImpact = "Très faible (~20-40 Mo RAM).",
            Recommendation = "Gardez activé pour appliquer vos profils RGB au démarrage.",
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
            FullDescription = "SignalRGB offre le contrôle RGB unifié avec des effets visuels avancés, des intégrations de jeux et du matériel de streaming. Alternative plus visuelle à OpenRGB.",
            DisableImpact = "Les effets RGB personnalisés ne s'appliqueront pas au démarrage.",
            PerformanceImpact = "Modéré (~80-150 Mo RAM).",
            Recommendation = "Gardez activé pour les effets RGB automatiques.",
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
            FullDescription = "FanControl permet de créer des courbes de ventilateurs personnalisées basées sur les températures CPU, GPU ou autres capteurs. Interface moderne et flexible.",
            DisableImpact = "Les ventilateurs reviendront aux courbes par défaut du BIOS.",
            PerformanceImpact = "Très faible (~15-30 Mo RAM).",
            Recommendation = "Gardez activé pour vos courbes de ventilateurs personnalisées.",
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
            FullDescription = "Argus Monitor surveille les températures CPU, GPU, disques et contrôle les ventilateurs avec des courbes personnalisées. Affiche les données dans la barre des tâches.",
            DisableImpact = "Pas de monitoring au démarrage. Courbes de ventilateurs par défaut.",
            PerformanceImpact = "Faible (~20-40 Mo RAM).",
            Recommendation = "Gardez activé pour le contrôle des ventilateurs.",
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
            FullDescription = "SpeedFan est un outil historique pour surveiller les températures et contrôler les ventilateurs. Moins maintenu mais toujours fonctionnel sur du matériel plus ancien.",
            DisableImpact = "Pas de contrôle automatique des ventilateurs.",
            PerformanceImpact = "Très faible (~10-20 Mo RAM).",
            Recommendation = "Peut être remplacé par FanControl ou Argus Monitor sur le matériel récent.",
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
            FullDescription = "GlassWire surveille votre trafic réseau avec des graphiques visuels, détecte les connexions suspectes et peut bloquer les applications. Alerte sur les nouvelles connexions réseau.",
            DisableImpact = "Pas de surveillance réseau au démarrage. Le pare-feu Windows reste actif.",
            PerformanceImpact = "Faible à modéré (~40-80 Mo RAM).",
            Recommendation = "Gardez activé pour la surveillance continue du réseau.",
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
            FullDescription = "NetLimiter permet de limiter la bande passante utilisée par chaque application, de définir des priorités et de surveiller le trafic réseau en détail.",
            DisableImpact = "Les limites de bande passante ne seront pas appliquées au démarrage.",
            PerformanceImpact = "Faible (~30-50 Mo RAM).",
            Recommendation = "Gardez activé si vous limitez la bande passante de certaines apps.",
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
            FullDescription = "Wireshark capture et analyse le trafic réseau en détail. Outil essentiel pour le diagnostic réseau et la sécurité. Utilisé par les professionnels IT.",
            DisableImpact = "Aucun impact. Wireshark ne devrait pas être dans le démarrage.",
            PerformanceImpact = "Aucun au repos.",
            Recommendation = "Ne devrait pas être dans le démarrage automatique.",
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
            FullDescription = "f.lux ajuste automatiquement la température des couleurs de votre écran selon l'heure du jour. Réduit la lumière bleue le soir pour améliorer le sommeil.",
            DisableImpact = "Pas de filtrage automatique de la lumière bleue.",
            PerformanceImpact = "Très faible (~10-20 Mo RAM).",
            Recommendation = "Gardez activé pour la protection des yeux le soir. Windows 10/11 a une fonction similaire 'Éclairage nocturne'.",
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
            FullDescription = "DisplayFusion améliore la gestion multi-écrans : barre des tâches sur chaque écran, fonds d'écran par moniteur, fenêtres accrochables, raccourcis clavier et profils.",
            DisableImpact = "Les fonctionnalités multi-écrans avancées ne seront pas disponibles.",
            PerformanceImpact = "Faible (~30-60 Mo RAM).",
            Recommendation = "Gardez activé si vous utilisez plusieurs moniteurs.",
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
            FullDescription = "Rainmeter affiche des widgets personnalisables sur le bureau : horloge, météo, monitoring système, lecteur audio, etc. Des milliers de skins disponibles.",
            DisableImpact = "Les widgets Rainmeter ne s'afficheront pas sur le bureau.",
            PerformanceImpact = "Variable selon les skins (~20-100+ Mo RAM).",
            Recommendation = "Gardez activé pour vos widgets de bureau personnalisés.",
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
            FullDescription = "Wallpaper Engine permet d'utiliser des fonds d'écran animés, vidéo ou interactifs. Steam Workshop avec des milliers de créations. Peut utiliser des pages web comme fond.",
            DisableImpact = "Les fonds d'écran animés ne s'afficheront pas.",
            PerformanceImpact = "Variable (~50-200 Mo RAM, utilisation GPU selon le fond).",
            Recommendation = "Peut être désactivé si vous n'utilisez pas de fonds animés ou pour économiser des ressources.",
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
            FullDescription = "Lively est une alternative gratuite à Wallpaper Engine. Supporte les vidéos, GIFs, pages web et shaders comme fonds d'écran animés.",
            DisableImpact = "Les fonds d'écran animés ne s'afficheront pas.",
            PerformanceImpact = "Variable (~30-150 Mo RAM).",
            Recommendation = "Peut être désactivé si vous n'utilisez pas de fonds animés.",
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
            FullDescription = "HP Support Assistant vérifie les mises à jour de pilotes HP et l'état de la garantie. Souvent considéré comme bloatware car il consomme des ressources et affiche des publicités HP.",
            DisableImpact = "Pas de notification des mises à jour HP. Les pilotes peuvent être mis à jour via Windows Update ou manuellement.",
            PerformanceImpact = "Modéré (~50-100 Mo RAM avec les services).",
            Recommendation = "Peut être désactivé ou désinstallé. Windows Update fournit les pilotes essentiels.",
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
            FullDescription = "Dell SupportAssist surveille l'état de votre PC Dell, vérifie les mises à jour et peut collecter des données de diagnostic.",
            DisableImpact = "Pas de diagnostics automatiques ni de mises à jour Dell. Les pilotes peuvent être téléchargés manuellement.",
            PerformanceImpact = "Modéré (~40-80 Mo RAM).",
            Recommendation = "Peut être désactivé. Utilisez Windows Update ou le site Dell pour les pilotes.",
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
            FullDescription = "Dell Command | Update scanne votre système Dell et propose les derniers pilotes et BIOS. C'est l'outil recommandé par Dell pour maintenir les pilotes à jour.",
            DisableImpact = "Pas de vérification automatique des mises à jour Dell. Lancez-le manuellement occasionnellement.",
            PerformanceImpact = "Faible au repos (~20-30 Mo RAM).",
            Recommendation = "Peut être désactivé du démarrage. Lancez-le manuellement pour vérifier les mises à jour.",
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
            FullDescription = "Dell Peripheral Manager permet de configurer les moniteurs Dell (luminosité, contraste, profils), les claviers et souris Dell. Offre des raccourcis de productivité pour les moniteurs multi-écrans.",
            DisableImpact = "Les paramètres par défaut seront utilisés pour les moniteurs Dell. Pas de profils personnalisés.",
            PerformanceImpact = "Faible (~20-40 Mo RAM).",
            Recommendation = "Gardez activé si vous utilisez des moniteurs Dell avec des profils personnalisés.",
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
            FullDescription = "Lenovo Vantage permet de gérer les paramètres spécifiques Lenovo, les mises à jour de pilotes, les paramètres de batterie et les fonctionnalités ThinkPad.",
            DisableImpact = "Perte des fonctionnalités Lenovo spécifiques comme les modes de performance personnalisés.",
            PerformanceImpact = "Modéré (~50-100 Mo RAM).",
            Recommendation = "Gardez si vous utilisez les fonctionnalités Lenovo. Peut être désactivé sinon.",
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
            FullDescription = "Armoury Crate gère l'éclairage RGB Aura Sync, les profils de ventilateurs, les modes de performance et les mises à jour ASUS sur les PC gaming ROG/TUF.",
            DisableImpact = "Perte du contrôle RGB et des profils de performance personnalisés.",
            PerformanceImpact = "Modéré à élevé (~80-150 Mo RAM).",
            Recommendation = "Gardez si vous utilisez les fonctionnalités RGB ou gaming. Peut être désactivé sur les PC non-gaming.",
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
            FullDescription = "Le service Spouleur d'impression gère toutes les imprimantes locales et réseau, met en file d'attente les travaux d'impression et communique avec les pilotes d'imprimante. Essentiel si vous utilisez une imprimante.",
            DisableImpact = "Aucune impression possible. Les imprimantes ne seront pas détectées.",
            PerformanceImpact = "Faible (~5-15 Mo RAM). Impact minimal sauf lors d'impressions actives.",
            Recommendation = "Gardez activé si vous avez une imprimante. Peut être désactivé sur les PC sans imprimante.",
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
            FullDescription = "BITS transfère des fichiers en arrière-plan en utilisant la bande passante inutilisée. Il est utilisé par Windows Update, Microsoft Store et d'autres services Microsoft pour télécharger les mises à jour sans impacter votre connexion.",
            DisableImpact = "Windows Update ne fonctionnera plus. Le Microsoft Store ne pourra pas télécharger d'applications.",
            PerformanceImpact = "Variable selon les téléchargements. Conçu pour être non-intrusif.",
            Recommendation = "Ne jamais désactiver. Essentiel pour les mises à jour Windows.",
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
            FullDescription = "Windows Audio gère le son pour les programmes Windows. AudioEndpointBuilder détecte les périphériques audio et permet la commutation dynamique.",
            DisableImpact = "Aucun son sur le système. Ni haut-parleurs, ni casque ne fonctionneront.",
            PerformanceImpact = "Très faible (~5-10 Mo RAM).",
            Recommendation = "Ne jamais désactiver sauf sur des serveurs sans audio.",
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
            FullDescription = "Le service Thèmes fournit la gestion des thèmes de l'expérience utilisateur. Il permet les effets visuels, les couleurs d'accentuation et les thèmes personnalisés.",
            DisableImpact = "L'interface revient au thème Windows classique. Perte des effets visuels et transparences.",
            PerformanceImpact = "Faible (~10-20 Mo RAM).",
            Recommendation = "Gardez activé pour une interface moderne. Peut être désactivé pour économiser des ressources.",
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
            FullDescription = "Le service Journal des événements Windows gère les événements et journaux d'événements. Il permet le diagnostic des problèmes, la surveillance de sécurité et l'audit du système.",
            DisableImpact = "Impossible de diagnostiquer les problèmes. Journaux de sécurité non enregistrés. Certaines applications peuvent échouer.",
            PerformanceImpact = "Faible (~10-20 Mo RAM). Écriture disque périodique.",
            Recommendation = "Ne jamais désactiver. Essentiel pour le dépannage et la sécurité.",
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
            FullDescription = "SysMain (anciennement Superfetch) analyse vos habitudes d'utilisation et précharge les applications fréquemment utilisées en RAM pour un démarrage plus rapide.",
            DisableImpact = "Les applications mettront plus de temps à démarrer au premier lancement.",
            PerformanceImpact = "Modéré (~50-200 Mo RAM utilisée pour le cache). Peut causer de l'activité disque sur les HDD.",
            Recommendation = "Gardez activé sur les PC avec SSD. Peut être désactivé sur les PC avec HDD si cela cause des ralentissements.",
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
            FullDescription = "Le service Temps Windows maintient la synchronisation de la date et l'heure sur tous les clients et serveurs du réseau. Utilise NTP pour la synchronisation.",
            DisableImpact = "L'heure du système peut dériver. Problèmes de certificats SSL et authentification possibles.",
            PerformanceImpact = "Négligeable. Synchronisation périodique uniquement.",
            Recommendation = "Gardez activé pour une heure précise. Essentiel pour les environnements d'entreprise.",
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
            FullDescription = "Le service Client DNS résout et met en cache les noms de domaine DNS. Il accélère l'accès aux sites web en évitant de refaire les requêtes DNS.",
            DisableImpact = "Chaque requête DNS sera effectuée sans cache. Navigation web plus lente.",
            PerformanceImpact = "Faible (~5-10 Mo RAM pour le cache).",
            Recommendation = "Ne jamais désactiver. Essentiel pour le réseau.",
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
            FullDescription = "Le service Client DHCP enregistre et met à jour les adresses IP et les enregistrements DNS. Sans lui, vous devez configurer manuellement l'adresse IP.",
            DisableImpact = "Pas de connexion réseau automatique. Configuration IP manuelle requise.",
            PerformanceImpact = "Négligeable.",
            Recommendation = "Ne jamais désactiver sauf si vous utilisez des IP statiques.",
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
            FullDescription = "Les services de chiffrement fournissent la gestion des clés, la signature de code et la vérification des certificats. Utilisé par Windows Update et les applications sécurisées.",
            DisableImpact = "Windows Update échouera. Problèmes avec les sites HTTPS et les applications signées.",
            PerformanceImpact = "Faible (~10-20 Mo RAM).",
            Recommendation = "Ne jamais désactiver. Essentiel pour la sécurité.",
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
            FullDescription = "Le service Cache de polices Windows optimise les performances des applications en mettant en cache les données de polices courantes.",
            DisableImpact = "Les applications peuvent démarrer plus lentement. Rendu des polices moins performant.",
            PerformanceImpact = "Faible (~10-30 Mo RAM).",
            Recommendation = "Gardez activé pour de meilleures performances d'affichage.",
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
            FullDescription = "Le Planificateur de tâches permet de programmer des tâches automatiques. Windows et de nombreuses applications l'utilisent pour la maintenance, les mises à jour et les sauvegardes.",
            DisableImpact = "Aucune tâche planifiée ne s'exécutera. Maintenance Windows compromise. Beaucoup d'applications ne fonctionneront pas correctement.",
            PerformanceImpact = "Faible (~10-20 Mo RAM).",
            Recommendation = "Ne jamais désactiver. Composant essentiel de Windows.",
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
            FullDescription = "WMI fournit une interface commune pour accéder aux informations de gestion du système. Utilisé par les outils de monitoring, les scripts et les applications de gestion.",
            DisableImpact = "De nombreuses applications de gestion échoueront. Monitoring système impossible. Scripts PowerShell affectés.",
            PerformanceImpact = "Modéré (~20-50 Mo RAM).",
            Recommendation = "Ne jamais désactiver. Essentiel pour la gestion du système.",
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
            FullDescription = "RPC est le service de base qui permet la communication entre les processus Windows. Pratiquement tous les services Windows en dépendent.",
            DisableImpact = "Windows ne fonctionnera pas. Le système sera instable ou ne démarrera pas.",
            PerformanceImpact = "Faible (partie intégrante de Windows).",
            Recommendation = "Ne jamais désactiver. Composant critique de Windows.",
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
            FullDescription = "Windows Installer gère l'installation, la modification et la suppression des applications utilisant le format MSI. De nombreux programmes professionnels utilisent ce format.",
            DisableImpact = "Impossible d'installer ou désinstaller les programmes MSI.",
            PerformanceImpact = "Faible. S'exécute uniquement lors des installations.",
            Recommendation = "Gardez activé. Peut être en démarrage manuel.",
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
            FullDescription = "Le service Windows Update permet la détection, le téléchargement et l'installation des mises à jour pour Windows et les autres produits Microsoft.",
            DisableImpact = "Aucune mise à jour de sécurité. Le système devient vulnérable aux failles de sécurité.",
            PerformanceImpact = "Variable. Peut utiliser CPU et réseau lors des mises à jour.",
            Recommendation = "Ne jamais désactiver. Les mises à jour de sécurité sont essentielles.",
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
            FullDescription = "Ce service est responsable du chargement et déchargement des profils utilisateur. Il gère les dossiers Documents, Bureau et les paramètres utilisateur.",
            DisableImpact = "Impossible de se connecter à Windows. Les profils utilisateur ne se chargeront pas.",
            PerformanceImpact = "Faible.",
            Recommendation = "Ne jamais désactiver. Essentiel pour l'ouverture de session.",
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
            FullDescription = "Le Pare-feu Windows Defender filtre le trafic réseau entrant et sortant selon des règles de sécurité. Protection de base contre les attaques réseau.",
            DisableImpact = "Le PC sera vulnérable aux attaques réseau. Aucun filtrage du trafic.",
            PerformanceImpact = "Très faible.",
            Recommendation = "Ne jamais désactiver sauf si un autre pare-feu est installé.",
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
            FullDescription = "Windows Defender fournit une protection en temps réel contre les virus, malwares, spywares et autres menaces. Intégré à Windows 10/11.",
            DisableImpact = "Le PC sera vulnérable aux malwares. Protection en temps réel désactivée.",
            PerformanceImpact = "Modéré (~100-200 Mo RAM). Peut utiliser du CPU lors des analyses.",
            Recommendation = "Ne jamais désactiver sauf si un autre antivirus est installé.",
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
            FullDescription = "Ce service collecte les informations sur le réseau et notifie les applications des changements. Il détermine si vous êtes sur un réseau public ou privé.",
            DisableImpact = "Windows ne détectera pas le type de réseau. Les paramètres de pare-feu peuvent être incorrects.",
            PerformanceImpact = "Négligeable.",
            Recommendation = "Gardez activé pour une configuration réseau correcte.",
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
            FullDescription = "Le service Serveur prend en charge le partage de fichiers, d'imprimantes et de canaux nommés sur le réseau. Essentiel pour le partage réseau.",
            DisableImpact = "Impossible de partager des fichiers ou imprimantes. Les autres PC ne pourront pas accéder à vos partages.",
            PerformanceImpact = "Faible sauf lors de transferts actifs.",
            Recommendation = "Gardez activé si vous partagez des fichiers. Peut être désactivé sur les PC isolés.",
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
            FullDescription = "Le service Station de travail crée et maintient les connexions aux serveurs distants via le protocole SMB. Permet d'accéder aux partages réseau.",
            DisableImpact = "Impossible d'accéder aux dossiers partagés sur le réseau.",
            PerformanceImpact = "Faible.",
            Recommendation = "Gardez activé pour accéder aux ressources réseau.",
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
            FullDescription = "Plug and Play permet à Windows de reconnaître et configurer automatiquement les périphériques sans intervention manuelle. Essentiel pour l'USB.",
            DisableImpact = "Les nouveaux périphériques ne seront pas détectés. L'USB pourrait ne pas fonctionner.",
            PerformanceImpact = "Très faible.",
            Recommendation = "Ne jamais désactiver. Composant essentiel de Windows.",
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
            FullDescription = "Google Update vérifie et installe les mises à jour pour Chrome, Google Drive, Google Earth et autres produits Google. S'exécute périodiquement en arrière-plan.",
            DisableImpact = "Les produits Google ne se mettront plus à jour automatiquement. Vulnérabilités de sécurité possibles.",
            PerformanceImpact = "Très faible. S'exécute brièvement à intervalles.",
            Recommendation = "Gardez activé pour les mises à jour de sécurité Chrome.",
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
            FullDescription = "Cette tâche vérifie et installe les mises à jour pour le navigateur Microsoft Edge. Importante pour la sécurité du navigateur.",
            DisableImpact = "Edge ne se mettra plus à jour automatiquement.",
            PerformanceImpact = "Très faible.",
            Recommendation = "Gardez activé si vous utilisez Edge.",
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
            FullDescription = "Cette tâche maintient OneDrive à jour avec les dernières fonctionnalités et correctifs de sécurité.",
            DisableImpact = "OneDrive ne se mettra plus à jour automatiquement.",
            PerformanceImpact = "Très faible.",
            Recommendation = "Gardez activé si vous utilisez OneDrive.",
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
            FullDescription = "Cette tâche télécharge et installe les mises à jour pour Microsoft Office (Word, Excel, PowerPoint, Outlook...). Importante pour la sécurité et les nouvelles fonctionnalités.",
            DisableImpact = "Office ne recevra plus de mises à jour automatiques. Vulnérabilités possibles.",
            PerformanceImpact = "Modéré lors des mises à jour. Faible au repos.",
            Recommendation = "Gardez activé pour la sécurité d'Office.",
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
            FullDescription = "Cette tâche effectue des analyses antivirus pendant les périodes d'inactivité du système. Partie intégrante de la protection Windows Defender.",
            DisableImpact = "Pas d'analyses automatiques. Dépendance à la protection en temps réel uniquement.",
            PerformanceImpact = "Peut utiliser des ressources significatives pendant l'analyse. Conçu pour s'exécuter en période d'inactivité.",
            Recommendation = "Gardez activé pour une protection complète.",
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
            FullDescription = "Cette tâche vérifie régulièrement si de nouvelles mises à jour Windows sont disponibles. Prépare les téléchargements et installations.",
            DisableImpact = "Windows ne recherchera plus automatiquement les mises à jour.",
            PerformanceImpact = "Faible. Utilise le réseau brièvement.",
            Recommendation = "Gardez activé pour recevoir les mises à jour de sécurité.",
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
            FullDescription = "Cette tâche collecte des informations sur la compatibilité du système pour les mises à jour Windows. Fait partie du programme d'amélioration de l'expérience Windows.",
            DisableImpact = "Microsoft ne recevra plus de données de compatibilité. Aucun impact fonctionnel.",
            PerformanceImpact = "Peut utiliser du CPU périodiquement. Parfois gourmand sur les anciens systèmes.",
            Recommendation = "Peut être désactivé pour la confidentialité ou les performances.",
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
            FullDescription = "Cette tâche exécute le nettoyage de disque silencieux pour supprimer les fichiers temporaires, le cache et autres fichiers inutiles.",
            DisableImpact = "Les fichiers temporaires s'accumuleront. Nettoyage manuel nécessaire.",
            PerformanceImpact = "Faible. Libère de l'espace disque.",
            Recommendation = "Gardez activé pour maintenir l'espace disque.",
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
            FullDescription = "Cette tâche analyse l'impact des applications de démarrage et fournit des recommandations dans le Gestionnaire des tâches.",
            DisableImpact = "Pas de mesure d'impact au démarrage. Fonctionnalité informative uniquement.",
            PerformanceImpact = "Négligeable.",
            Recommendation = "Gardez activé pour les informations de démarrage.",
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
            FullDescription = "Exécute CCleaner automatiquement pour nettoyer les fichiers temporaires, le cache des navigateurs et autres fichiers inutiles.",
            DisableImpact = "Pas de nettoyage automatique. CCleaner devra être lancé manuellement.",
            PerformanceImpact = "Modéré pendant le nettoyage.",
            Recommendation = "Optionnel. Utile pour un nettoyage régulier automatique.",
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
            FullDescription = "Cette tâche vérifie et installe les mises à jour pour Adobe Acrobat et Reader. Important car les PDF peuvent contenir des vulnérabilités.",
            DisableImpact = "Acrobat/Reader ne se mettra plus à jour automatiquement.",
            PerformanceImpact = "Très faible.",
            Recommendation = "Gardez activé pour les correctifs de sécurité PDF.",
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
            FullDescription = "Collecte des données d'utilisation pour NVIDIA. Aide NVIDIA à améliorer ses pilotes et logiciels.",
            DisableImpact = "NVIDIA ne recevra plus de données d'utilisation. Aucun impact fonctionnel.",
            PerformanceImpact = "Très faible.",
            Recommendation = "Peut être désactivé pour la confidentialité.",
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
            FullDescription = "Cette tâche s'assure que l'Explorateur Windows (le bureau et la barre des tâches) démarre correctement avec les droits utilisateur normaux.",
            DisableImpact = "Peut causer des problèmes avec le bureau et l'Explorateur.",
            PerformanceImpact = "Aucun (s'exécute une fois au démarrage).",
            Recommendation = "Ne jamais désactiver.",
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
            FullDescription = "CTF Loader (ctfmon.exe) active le processeur de texte alternatif et la barre de langue Microsoft Office. Gère les méthodes d'entrée et la reconnaissance d'écriture manuscrite.",
            DisableImpact = "La barre de langue et certaines fonctionnalités de saisie de texte peuvent ne pas fonctionner.",
            PerformanceImpact = "Très faible (~5-10 Mo RAM).",
            Recommendation = "Gardez activé si vous utilisez plusieurs langues ou la saisie spéciale.",
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
            FullDescription = "VMware Tools améliore les performances et la gestion des machines virtuelles. Permet le copier-coller entre hôte et VM, le redimensionnement de l'écran et la synchronisation de l'heure.",
            DisableImpact = "Perte du copier-coller hôte/VM, performances graphiques réduites dans la VM.",
            PerformanceImpact = "Faible (~20-40 Mo RAM).",
            Recommendation = "Gardez activé si vous êtes dans une VM VMware.",
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
            FullDescription = "Les Guest Additions VirtualBox permettent les dossiers partagés, le copier-coller, le redimensionnement de l'écran et l'intégration du pointeur souris.",
            DisableImpact = "Perte des fonctionnalités d'intégration VirtualBox.",
            PerformanceImpact = "Faible (~15-30 Mo RAM).",
            Recommendation = "Gardez activé si vous êtes dans une VM VirtualBox.",
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
            FullDescription = "Fournit un accès rapide aux paramètres des graphiques Intel intégrés depuis la barre des tâches. Permet de changer la résolution et les paramètres d'affichage.",
            DisableImpact = "Pas d'icône Intel dans la barre des tâches. Les paramètres restent accessibles via le Panneau de configuration Intel.",
            PerformanceImpact = "Très faible.",
            Recommendation = "Peut être désactivé si vous n'utilisez pas les raccourcis Intel.",
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
            FullDescription = "Swift Pair affiche des notifications lorsqu'un nouveau périphérique Bluetooth est détecté à proximité, permettant un appairage en un clic.",
            DisableImpact = "Pas de notifications d'appairage rapide. Appairage manuel toujours possible.",
            PerformanceImpact = "Négligeable.",
            Recommendation = "Gardez activé si vous utilisez régulièrement le Bluetooth.",
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
            FullDescription = "Windows Terminal est l'application de terminal moderne de Microsoft qui combine PowerShell, Command Prompt, WSL et d'autres shells dans une interface à onglets.",
            DisableImpact = "Le terminal ne s'ouvrira pas au démarrage (comportement normal).",
            PerformanceImpact = "Aucun au démarrage (lance à la demande).",
            Recommendation = "N'a généralement pas besoin de démarrer avec Windows.",
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
            FullDescription = "Windows Sandbox crée un environnement de bureau léger et temporaire pour exécuter des applications en isolation. Tout est supprimé à la fermeture.",
            DisableImpact = "N'a pas besoin de démarrer avec Windows.",
            PerformanceImpact = "Aucun au démarrage.",
            Recommendation = "Ne devrait pas être dans les programmes de démarrage.",
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
            FullDescription = "PowerShell est un shell de ligne de commande et un langage de script puissant. Sa présence au démarrage peut être normale (scripts) ou suspecte (malware).",
            DisableImpact = "Les scripts PowerShell au démarrage ne s'exécuteront pas.",
            PerformanceImpact = "Variable selon le script.",
            Recommendation = "Vérifiez le script exécuté. Les malwares utilisent souvent PowerShell.",
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
            FullDescription = "L'invite de commandes Windows au démarrage exécute généralement un script batch. Peut être légitime ou suspect selon le script.",
            DisableImpact = "Les scripts batch au démarrage ne s'exécuteront pas.",
            PerformanceImpact = "Variable selon le script.",
            Recommendation = "Examinez la commande complète. Peut indiquer un malware.",
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
            FullDescription = "Windows Script Host exécute des scripts VBS et JS. Souvent utilisé par les malwares pour leurs capacités de scripting.",
            DisableImpact = "Les scripts VBS/JS au démarrage ne s'exécuteront pas.",
            PerformanceImpact = "Variable selon le script.",
            Recommendation = "Inspectez le script. Vecteur de malware courant.",
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
            FullDescription = "MSHTA exécute les fichiers HTA (HTML Application). Très rarement légitime au démarrage. Fréquemment utilisé par les malwares.",
            DisableImpact = "Les applications HTA au démarrage ne s'exécuteront pas.",
            PerformanceImpact = "Variable.",
            Recommendation = "SUSPECT : Vérifiez immédiatement. Vecteur de malware très courant.",
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
            FullDescription = "Rundll32 exécute des fonctions exportées depuis des fichiers DLL. Utilisé légitimement par Windows et les applications, mais aussi par les malwares.",
            DisableImpact = "Variable selon la DLL appelée.",
            PerformanceImpact = "Variable.",
            Recommendation = "Examinez la DLL et la fonction appelées. Peut être légitime ou malveillant.",
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
            FullDescription = "Regsvr32 enregistre et désenregistre les DLL et contrôles OLE dans le registre. Peut être utilisé pour exécuter du code malveillant.",
            DisableImpact = "L'enregistrement de DLL au démarrage ne se fera pas.",
            PerformanceImpact = "Faible.",
            Recommendation = "Rare au démarrage. Vérifiez la DLL concernée.",
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
            FullDescription = "Overwolf est une plateforme qui permet d'exécuter des applications et overlays pendant les jeux. Héberge des apps comme CurseForge pour les mods.",
            DisableImpact = "Les overlays et apps Overwolf ne seront pas disponibles en jeu.",
            PerformanceImpact = "Modéré. Consomme des ressources pour les overlays.",
            Recommendation = "Peut être désactivé si non utilisé. Relancez manuellement si besoin.",
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
            FullDescription = "CurseForge est un gestionnaire de mods populaire pour Minecraft, World of Warcraft, et de nombreux autres jeux. Fait partie de l'écosystème Overwolf.",
            DisableImpact = "Les mods ne seront pas mis à jour automatiquement.",
            PerformanceImpact = "Faible au démarrage.",
            Recommendation = "Peut être désactivé. Lancez manuellement pour gérer les mods.",
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
            FullDescription = "itch.io est une plateforme de distribution de jeux indépendants. L'application desktop permet de télécharger et gérer les jeux achetés.",
            DisableImpact = "Pas de mise à jour automatique des jeux itch.io.",
            PerformanceImpact = "Faible.",
            Recommendation = "Peut être désactivé. Lancez manuellement quand nécessaire.",
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
            FullDescription = "L'application Humble permet de télécharger et gérer les jeux achetés sur Humble Bundle, ainsi que les jeux du Humble Choice.",
            DisableImpact = "Pas de mise à jour automatique des jeux Humble.",
            PerformanceImpact = "Faible.",
            Recommendation = "Peut être désactivé si vous n'utilisez pas souvent Humble.",
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
            FullDescription = "L'application desktop Twitch permet de regarder des streams, gérer les abonnements et recevoir des notifications.",
            DisableImpact = "Pas de notifications de streams en direct.",
            PerformanceImpact = "Modéré si actif en arrière-plan.",
            Recommendation = "Peut être désactivé. Utilisez le site web comme alternative.",
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
            FullDescription = "Guilded est une plateforme de communication pour les communautés gaming, offrant chat, forums, calendriers et streaming intégrés.",
            DisableImpact = "Pas de notifications ni messages instantanés.",
            PerformanceImpact = "Modéré. Similaire à Discord.",
            Recommendation = "Peut être désactivé si utilisé occasionnellement.",
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
            FullDescription = "Prism Launcher est un lanceur Minecraft open source permettant de gérer plusieurs instances avec différentes versions et mods.",
            DisableImpact = "Minecraft ne sera pas lancé automatiquement.",
            PerformanceImpact = "Aucun si pas utilisé.",
            Recommendation = "N'a pas besoin de démarrer avec Windows.",
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
            FullDescription = "Perplexity est un assistant de recherche alimenté par l'IA qui fournit des réponses avec des sources citées. L'application desktop offre un accès rapide.",
            DisableImpact = "Pas d'accès rapide via raccourci global.",
            PerformanceImpact = "Faible à modéré.",
            Recommendation = "Peut être désactivé si utilisé via le navigateur.",
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
            FullDescription = "Claude est un assistant IA développé par Anthropic. L'application desktop offre un accès direct sans navigateur.",
            DisableImpact = "Claude ne sera pas accessible via raccourci global.",
            PerformanceImpact = "Faible.",
            Recommendation = "Peut être désactivé. Lancez manuellement si besoin.",
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
            FullDescription = "ChatGPT est l'assistant IA conversationnel d'OpenAI. L'application desktop permet un accès rapide avec raccourcis globaux.",
            DisableImpact = "Pas de raccourci global pour accéder à ChatGPT.",
            PerformanceImpact = "Faible.",
            Recommendation = "Peut être désactivé. Utilisez le site web comme alternative.",
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
            FullDescription = "Pieces est un outil IA pour développeurs qui aide à sauvegarder, enrichir et réutiliser des snippets de code avec contexte.",
            DisableImpact = "Les fonctionnalités d'IA et snippets ne seront pas disponibles.",
            PerformanceImpact = "Modéré. Exécute un modèle IA local.",
            Recommendation = "Désactivez si non utilisé régulièrement.",
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
            FullDescription = "Codeium est un outil d'autocomplétion de code alimenté par l'IA, gratuit pour les développeurs individuels.",
            DisableImpact = "L'autocomplétion IA dans les IDE ne fonctionnera pas.",
            PerformanceImpact = "Faible.",
            Recommendation = "Peut être désactivé si non utilisé.",
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
            FullDescription = "Vivaldi est un navigateur hautement personnalisable basé sur Chromium, créé par les anciens fondateurs d'Opera. Offre de nombreuses fonctionnalités intégrées.",
            DisableImpact = "Vivaldi ne s'ouvrira pas au démarrage.",
            PerformanceImpact = "Modéré si lancé au démarrage.",
            Recommendation = "Peut être désactivé si non utilisé comme navigateur principal.",
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
            FullDescription = "Arc est un navigateur repensé avec une interface utilisateur innovante, des espaces de travail et des fonctionnalités IA intégrées.",
            DisableImpact = "Arc ne s'ouvrira pas au démarrage.",
            PerformanceImpact = "Modéré.",
            Recommendation = "Peut être désactivé. Lancez manuellement quand nécessaire.",
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
            FullDescription = "Waterfox est un navigateur basé sur Firefox avec un focus sur la vie privée et la suppression de la télémétrie.",
            DisableImpact = "Waterfox ne s'ouvrira pas au démarrage.",
            PerformanceImpact = "Modéré.",
            Recommendation = "Peut être désactivé.",
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
            FullDescription = "Tor Browser est un navigateur basé sur Firefox qui route le trafic via le réseau Tor pour l'anonymat. Ne devrait généralement pas démarrer automatiquement.",
            DisableImpact = "N'affecte pas la navigation normale.",
            PerformanceImpact = "Aucun si désactivé.",
            Recommendation = "Ne devrait pas être au démarrage. Vérifiez si c'est intentionnel.",
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
            FullDescription = "Zen est un navigateur basé sur Firefox avec une interface à onglets verticaux et un design moderne et minimaliste.",
            DisableImpact = "Zen ne s'ouvrira pas au démarrage.",
            PerformanceImpact = "Modéré.",
            Recommendation = "Peut être désactivé.",
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
            FullDescription = "Cursor est un éditeur de code basé sur VS Code avec des fonctionnalités d'IA avancées pour l'autocomplétion et la génération de code.",
            DisableImpact = "Cursor ne s'ouvrira pas au démarrage.",
            PerformanceImpact = "Faible au démarrage.",
            Recommendation = "N'a pas besoin de démarrer avec Windows.",
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
            FullDescription = "JetBrains Toolbox gère l'installation et les mises à jour de tous les IDE JetBrains (IntelliJ, PyCharm, WebStorm, etc.).",
            DisableImpact = "Pas de mises à jour automatiques des IDE JetBrains.",
            PerformanceImpact = "Faible.",
            Recommendation = "Peut être désactivé. Les mises à jour se feront manuellement.",
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
            FullDescription = "Sublime Text est un éditeur de texte sophistiqué pour le code et le texte, connu pour sa rapidité et son interface élégante.",
            DisableImpact = "Sublime Text ne s'ouvrira pas au démarrage.",
            PerformanceImpact = "Très faible.",
            Recommendation = "N'a pas besoin de démarrer avec Windows.",
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
            FullDescription = "Notepad++ est un éditeur de texte et de code source gratuit et open source. Léger et rapide avec support de nombreux langages.",
            DisableImpact = "Notepad++ ne s'ouvrira pas au démarrage.",
            PerformanceImpact = "Très faible.",
            Recommendation = "N'a pas besoin de démarrer avec Windows.",
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
            FullDescription = "GitHub Desktop est une application graphique pour gérer les dépôts Git et GitHub sans ligne de commande.",
            DisableImpact = "GitHub Desktop ne s'ouvrira pas au démarrage.",
            PerformanceImpact = "Faible.",
            Recommendation = "N'a pas besoin de démarrer avec Windows. Lancez manuellement.",
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
            FullDescription = "GitKraken est un client Git graphique puissant avec visualisation des branches, intégrations et fonctionnalités de collaboration.",
            DisableImpact = "GitKraken ne s'ouvrira pas au démarrage.",
            PerformanceImpact = "Modéré.",
            Recommendation = "N'a pas besoin de démarrer avec Windows.",
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
            FullDescription = "Sourcetree est un client Git et Mercurial gratuit avec interface graphique pour visualiser et gérer les dépôts.",
            DisableImpact = "Sourcetree ne s'ouvrira pas au démarrage.",
            PerformanceImpact = "Faible.",
            Recommendation = "N'a pas besoin de démarrer avec Windows.",
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
            FullDescription = "Postman est une plateforme collaborative pour le développement d'API permettant de créer, tester et documenter les APIs.",
            DisableImpact = "Postman ne s'ouvrira pas au démarrage.",
            PerformanceImpact = "Modéré.",
            Recommendation = "N'a pas besoin de démarrer avec Windows.",
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
            FullDescription = "Insomnia est un client API open source pour REST, GraphQL et gRPC avec une interface moderne.",
            DisableImpact = "Insomnia ne s'ouvrira pas au démarrage.",
            PerformanceImpact = "Faible.",
            Recommendation = "N'a pas besoin de démarrer avec Windows.",
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
            FullDescription = "WSL permet d'exécuter un environnement Linux directement sur Windows sans machine virtuelle traditionnelle.",
            DisableImpact = "Les distributions Linux WSL ne démarreront pas automatiquement.",
            PerformanceImpact = "Variable selon l'utilisation.",
            Recommendation = "Généralement n'a pas besoin de démarrer automatiquement.",
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
            FullDescription = "Grammarly est un assistant d'écriture qui vérifie la grammaire, l'orthographe et le style dans toutes les applications.",
            DisableImpact = "Pas de corrections Grammarly en temps réel.",
            PerformanceImpact = "Faible à modéré.",
            Recommendation = "Peut être désactivé si utilisé principalement via extension navigateur.",
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
            FullDescription = "Todoist est un gestionnaire de tâches multiplateforme avec projets, étiquettes, filtres et rappels.",
            DisableImpact = "Pas de rappels ni accès rapide au démarrage.",
            PerformanceImpact = "Faible.",
            Recommendation = "Peut être désactivé. Utilisez l'app web ou mobile.",
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
            FullDescription = "ClickUp est une plateforme tout-en-un pour la gestion de projets, tâches, documents, objectifs et temps.",
            DisableImpact = "Pas de notifications desktop ClickUp.",
            PerformanceImpact = "Modéré.",
            Recommendation = "Peut être désactivé. L'app web est fonctionnellement équivalente.",
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
            FullDescription = "Linear est un outil de suivi des issues moderne et rapide, populaire auprès des équipes de développement produit.",
            DisableImpact = "Pas de notifications Linear au démarrage.",
            PerformanceImpact = "Faible.",
            Recommendation = "Peut être désactivé. Lancez manuellement ou utilisez le web.",
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
            FullDescription = "Figma est un outil de design d'interface utilisateur collaboratif basé sur le cloud avec édition en temps réel.",
            DisableImpact = "Figma ne s'ouvrira pas au démarrage.",
            PerformanceImpact = "Modéré.",
            Recommendation = "N'a pas besoin de démarrer avec Windows. L'app web est identique.",
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
            FullDescription = "Loom permet d'enregistrer rapidement des vidéos de son écran avec webcam pour partager des messages asynchrones.",
            DisableImpact = "Pas d'accès rapide pour l'enregistrement Loom.",
            PerformanceImpact = "Faible au repos.",
            Recommendation = "Peut être désactivé si utilisé occasionnellement.",
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
            FullDescription = "Raycast est un lanceur de productivité extensible qui remplace Spotlight/PowerToys Run avec des extensions communautaires.",
            DisableImpact = "Raycast ne sera pas disponible via raccourci.",
            PerformanceImpact = "Faible.",
            Recommendation = "Doit rester actif si utilisé comme lanceur principal.",
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
            FullDescription = "Flow Launcher est un lanceur d'applications rapide et extensible pour Windows, alternative open source à Alfred/Raycast.",
            DisableImpact = "Flow Launcher ne sera pas disponible via raccourci.",
            PerformanceImpact = "Très faible.",
            Recommendation = "Doit rester actif si utilisé comme lanceur principal.",
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
            FullDescription = "Plex est un client pour le système de media center Plex, permettant de streamer films, séries et musique depuis un serveur Plex.",
            DisableImpact = "Plex ne s'ouvrira pas au démarrage.",
            PerformanceImpact = "Faible au repos.",
            Recommendation = "Peut être désactivé sur les postes non-HTPC.",
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
            FullDescription = "Plex Media Server organise et streame votre bibliothèque multimédia vers tous les appareils. Doit tourner pour que Plex fonctionne.",
            DisableImpact = "Votre bibliothèque Plex ne sera pas accessible.",
            PerformanceImpact = "Modéré à élevé lors du transcodage.",
            Recommendation = "Gardez actif si vous utilisez Plex comme serveur.",
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
            FullDescription = "foobar2000 est un lecteur audio gratuit pour Windows, réputé pour sa légèreté, sa qualité sonore et sa personnalisation.",
            DisableImpact = "foobar2000 ne s'ouvrira pas au démarrage.",
            PerformanceImpact = "Très faible.",
            Recommendation = "N'a pas besoin de démarrer avec Windows.",
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
            FullDescription = "AIMP est un lecteur audio gratuit avec une interface élégante, support de nombreux formats et fonctionnalités avancées.",
            DisableImpact = "AIMP ne s'ouvrira pas au démarrage.",
            PerformanceImpact = "Très faible.",
            Recommendation = "N'a pas besoin de démarrer avec Windows.",
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
            FullDescription = "MusicBee est un gestionnaire de musique puissant avec bibliothèque, podcasts, radio et synchronisation d'appareils.",
            DisableImpact = "MusicBee ne s'ouvrira pas au démarrage.",
            PerformanceImpact = "Faible.",
            Recommendation = "N'a pas besoin de démarrer avec Windows.",
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
            FullDescription = "PotPlayer est un lecteur multimédia coréen gratuit supportant de nombreux formats et codecs avec une interface personnalisable.",
            DisableImpact = "PotPlayer ne s'ouvrira pas au démarrage.",
            PerformanceImpact = "Très faible.",
            Recommendation = "N'a pas besoin de démarrer avec Windows.",
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
            FullDescription = "Kodi est un centre multimédia open source pour organiser et lire films, séries, musique, photos et plus.",
            DisableImpact = "Kodi ne s'ouvrira pas au démarrage.",
            PerformanceImpact = "Faible au repos.",
            Recommendation = "Utile au démarrage uniquement sur les HTPC dédiés.",
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
            FullDescription = "Kindle for PC permet de lire les ebooks achetés sur Amazon et synchronise la progression avec les autres appareils.",
            DisableImpact = "Kindle ne s'ouvrira pas au démarrage.",
            PerformanceImpact = "Faible.",
            Recommendation = "N'a pas besoin de démarrer avec Windows.",
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
            FullDescription = "calibre est un gestionnaire d'ebooks gratuit et open source pour organiser, convertir et transférer des ebooks.",
            DisableImpact = "calibre ne s'ouvrira pas au démarrage.",
            PerformanceImpact = "Faible.",
            Recommendation = "N'a pas besoin de démarrer avec Windows.",
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
            FullDescription = "DaVinci Resolve est une suite professionnelle de montage vidéo, étalonnage, effets visuels et post-production audio.",
            DisableImpact = "DaVinci Resolve ne s'ouvrira pas au démarrage.",
            PerformanceImpact = "Très faible au repos.",
            Recommendation = "N'a pas besoin de démarrer avec Windows.",
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
            FullDescription = "HandBrake est un transcodeur vidéo open source multiplateforme pour convertir des vidéos en formats modernes.",
            DisableImpact = "HandBrake ne s'ouvrira pas au démarrage.",
            PerformanceImpact = "Aucun au repos.",
            Recommendation = "N'a pas besoin de démarrer avec Windows.",
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
            FullDescription = "GIMP est un éditeur d'images libre et gratuit, alternative open source à Photoshop.",
            DisableImpact = "GIMP ne s'ouvrira pas au démarrage.",
            PerformanceImpact = "Aucun au repos.",
            Recommendation = "N'a pas besoin de démarrer avec Windows.",
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
            FullDescription = "Inkscape est un éditeur de graphiques vectoriels libre, alternative open source à Illustrator.",
            DisableImpact = "Inkscape ne s'ouvrira pas au démarrage.",
            PerformanceImpact = "Aucun au repos.",
            Recommendation = "N'a pas besoin de démarrer avec Windows.",
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
            FullDescription = "Krita est une application de peinture numérique gratuite et open source pour illustrateurs, artistes concept et peintres.",
            DisableImpact = "Krita ne s'ouvrira pas au démarrage.",
            PerformanceImpact = "Aucun au repos.",
            Recommendation = "N'a pas besoin de démarrer avec Windows.",
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
            FullDescription = "Blender est une suite de création 3D open source pour modélisation, animation, rendu, compositing et montage vidéo.",
            DisableImpact = "Blender ne s'ouvrira pas au démarrage.",
            PerformanceImpact = "Aucun au repos.",
            Recommendation = "N'a pas besoin de démarrer avec Windows.",
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
            FullDescription = "Unity Hub gère les installations de l'éditeur Unity, les projets et les licences pour le développement de jeux et applications.",
            DisableImpact = "Unity Hub ne s'ouvrira pas au démarrage.",
            PerformanceImpact = "Faible.",
            Recommendation = "N'a pas besoin de démarrer avec Windows.",
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
            FullDescription = "MSI Afterburner est un utilitaire d'overclocking GPU populaire avec monitoring en temps réel, profils et statistiques en jeu.",
            DisableImpact = "Pas d'overclocking GPU ni de monitoring au démarrage.",
            PerformanceImpact = "Faible.",
            Recommendation = "Gardez actif si vous utilisez l'overclocking ou le monitoring.",
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
            FullDescription = "NZXT CAM surveille les performances du PC, contrôle l'éclairage RGB et les ventilateurs des produits NZXT.",
            DisableImpact = "Pas de contrôle RGB/ventilateurs NZXT, pas de monitoring.",
            PerformanceImpact = "Faible à modéré.",
            Recommendation = "Gardez actif si vous avez du matériel NZXT ou voulez le monitoring.",
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
            FullDescription = "Voicemod est un modificateur de voix en temps réel pour Discord, Twitch et autres applications de communication.",
            DisableImpact = "Les effets de voix ne seront pas disponibles.",
            PerformanceImpact = "Modéré.",
            Recommendation = "Peut être désactivé si non utilisé régulièrement.",
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
            FullDescription = "Krisp utilise l'IA pour supprimer le bruit de fond et les échos lors des appels vidéo et audio.",
            DisableImpact = "Pas de suppression de bruit automatique.",
            PerformanceImpact = "Modéré (traitement IA).",
            Recommendation = "Utile si vous faites beaucoup d'appels. Sinon désactivable.",
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
            FullDescription = "EarTrumpet remplace l'icône de volume Windows avec un contrôle avancé du volume par application.",
            DisableImpact = "Pas de contrôle de volume par app via EarTrumpet.",
            PerformanceImpact = "Très faible.",
            Recommendation = "Gardez actif si vous l'utilisez pour le contrôle du volume.",
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
            FullDescription = "Streamlabs est une version enrichie d'OBS avec alertes, widgets et outils de streaming intégrés.",
            DisableImpact = "Streamlabs ne s'ouvrira pas au démarrage.",
            PerformanceImpact = "Faible au repos.",
            Recommendation = "N'a pas besoin de démarrer avec Windows.",
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
            FullDescription = "XSplit est une suite de diffusion en direct incluant le streaming, la caméra virtuelle et l'enregistrement.",
            DisableImpact = "XSplit ne s'ouvrira pas au démarrage.",
            PerformanceImpact = "Faible au repos.",
            Recommendation = "N'a pas besoin de démarrer avec Windows.",
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
            FullDescription = "GPU-Z affiche les informations détaillées sur la carte graphique, les capteurs et les fréquences.",
            DisableImpact = "GPU-Z ne s'ouvrira pas au démarrage.",
            PerformanceImpact = "Très faible.",
            Recommendation = "N'a pas besoin de démarrer avec Windows.",
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
            FullDescription = "CPU-Z affiche les informations détaillées sur le processeur, la carte mère, la mémoire et le cache.",
            DisableImpact = "CPU-Z ne s'ouvrira pas au démarrage.",
            PerformanceImpact = "Très faible.",
            Recommendation = "N'a pas besoin de démarrer avec Windows.",
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
            FullDescription = "AIDA64 est un outil complet de diagnostic système, benchmarking et monitoring avec des informations détaillées sur tout le matériel.",
            DisableImpact = "Pas de monitoring AIDA64 au démarrage.",
            PerformanceImpact = "Faible.",
            Recommendation = "Peut être désactivé si le monitoring n'est pas nécessaire.",
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
            FullDescription = "Element est un client de messagerie basé sur le protocole Matrix, offrant une communication décentralisée et chiffrée.",
            DisableImpact = "Pas de notifications Element au démarrage.",
            PerformanceImpact = "Faible à modéré.",
            Recommendation = "Peut être désactivé si non utilisé régulièrement.",
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
            FullDescription = "Mumble est une application de chat vocal open source à faible latence, populaire pour les jeux et les équipes.",
            DisableImpact = "Mumble ne se connectera pas automatiquement.",
            PerformanceImpact = "Faible.",
            Recommendation = "Peut être désactivé. Lancez manuellement avant les sessions.",
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
            FullDescription = "TeamSpeak est une application VoIP pour la communication d'équipe, populaire dans le gaming et le milieu professionnel.",
            DisableImpact = "TeamSpeak ne se connectera pas automatiquement.",
            PerformanceImpact = "Faible.",
            Recommendation = "Peut être désactivé si non utilisé quotidiennement.",
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
            FullDescription = "Webex est une plateforme de visioconférence et collaboration d'entreprise de Cisco.",
            DisableImpact = "Pas de connexion automatique aux réunions.",
            PerformanceImpact = "Faible au repos.",
            Recommendation = "Peut être désactivé si utilisé occasionnellement.",
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
            FullDescription = "BlueJeans est une plateforme de visioconférence d'entreprise appartenant à Verizon.",
            DisableImpact = "BlueJeans ne démarrera pas automatiquement.",
            PerformanceImpact = "Faible au repos.",
            Recommendation = "Peut être désactivé. Lancez manuellement pour les réunions.",
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
            FullDescription = "GoTo Meeting est un service de visioconférence et webinaires pour les entreprises.",
            DisableImpact = "GoTo Meeting ne démarrera pas automatiquement.",
            PerformanceImpact = "Faible au repos.",
            Recommendation = "Peut être désactivé. Lancez via lien de réunion.",
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
            FullDescription = "LM Studio permet de télécharger et exécuter des modèles de langage (LLMs) localement sur votre machine. Supporte de nombreux modèles open source.",
            DisableImpact = "LM Studio ne s'ouvrira pas au démarrage.",
            PerformanceImpact = "Élevé lors de l'utilisation (GPU/CPU intensif).",
            Recommendation = "N'a pas besoin de démarrer avec Windows.",
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
            FullDescription = "Sandboxie Plus permet d'exécuter des programmes dans un environnement isolé pour protéger votre système. Idéal pour tester des logiciels suspects.",
            DisableImpact = "Les programmes ne seront pas automatiquement sandboxés.",
            PerformanceImpact = "Faible.",
            Recommendation = "Gardez actif si vous utilisez régulièrement le sandboxing.",
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
            FullDescription = "Synology Image Assistant aide à gérer et configurer les NAS Synology depuis votre PC.",
            DisableImpact = "L'assistant Synology ne sera pas disponible immédiatement.",
            PerformanceImpact = "Très faible.",
            Recommendation = "Peut être désactivé si utilisé occasionnellement.",
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
            FullDescription = "YoloMouse permet de remplacer le curseur par défaut dans les jeux par un curseur plus visible et personnalisé. Utile pour les jeux avec curseurs difficiles à voir.",
            DisableImpact = "Les curseurs personnalisés ne seront pas appliqués automatiquement.",
            PerformanceImpact = "Très faible.",
            Recommendation = "Peut être désactivé. Lancez manuellement avant de jouer.",
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
            FullDescription = "RoboForm est un gestionnaire de mots de passe qui stocke et remplit automatiquement vos identifiants de connexion.",
            DisableImpact = "Le remplissage automatique des mots de passe ne sera pas disponible.",
            PerformanceImpact = "Faible.",
            Recommendation = "Gardez actif si vous l'utilisez comme gestionnaire de mots de passe principal.",
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
            FullDescription = "Waves MaxxAudio améliore la qualité audio sur les PC Dell et autres marques. Fournit des effets audio et égalisation.",
            DisableImpact = "Les améliorations audio Waves ne seront pas appliquées.",
            PerformanceImpact = "Faible.",
            Recommendation = "Gardez actif si vous utilisez les effets audio Waves.",
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
            FullDescription = "Ce service vérifie et notifie les mises à jour disponibles pour les pilotes et logiciels d'imprimantes Brother.",
            DisableImpact = "Pas de notifications de mises à jour Brother.",
            PerformanceImpact = "Très faible.",
            Recommendation = "Peut être désactivé. Vérifiez manuellement les mises à jour.",
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
            FullDescription = "Opera Browser Assistant gère les mises à jour automatiques d'Opera et affiche des notifications promotionnelles.",
            DisableImpact = "Pas de mises à jour automatiques d'Opera ni de notifications.",
            PerformanceImpact = "Très faible.",
            Recommendation = "Peut être désactivé. Les mises à jour se feront via le navigateur.",
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
            FullDescription = "Opera est un navigateur web avec VPN intégré, bloqueur de publicités et sidebar de messagerie.",
            DisableImpact = "Opera ne s'ouvrira pas au démarrage.",
            PerformanceImpact = "Modéré si lancé au démarrage.",
            Recommendation = "Peut être désactivé si vous lancez Opera manuellement.",
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
            FullDescription = "Ce service gère les mises à jour automatiques du navigateur Brave.",
            DisableImpact = "Brave ne se mettra pas à jour automatiquement.",
            PerformanceImpact = "Très faible.",
            Recommendation = "Gardez actif pour les mises à jour de sécurité automatiques.",
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
            FullDescription = "Services nécessaires au fonctionnement de VMware Workstation pour les machines virtuelles, réseau et périphériques USB.",
            DisableImpact = "Les machines virtuelles VMware ne fonctionneront pas correctement.",
            PerformanceImpact = "Modéré.",
            Recommendation = "Gardez actif si vous utilisez VMware Workstation.",
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
            FullDescription = "Services nécessaires au fonctionnement de Corsair iCUE pour contrôler l'éclairage RGB et les fonctionnalités des périphériques Corsair.",
            DisableImpact = "L'éclairage RGB Corsair et les fonctionnalités iCUE ne fonctionneront pas.",
            PerformanceImpact = "Faible.",
            Recommendation = "Gardez actif si vous avez des périphériques Corsair.",
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
            FullDescription = "Services nécessaires au fonctionnement de Razer Synapse et Chroma RGB pour les périphériques Razer.",
            DisableImpact = "L'éclairage Chroma et les fonctionnalités Synapse ne fonctionneront pas.",
            PerformanceImpact = "Modéré.",
            Recommendation = "Gardez actif si vous avez des périphériques Razer.",
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
            FullDescription = "Service nécessaire pour utiliser les casques VR Oculus Rift et Meta Quest avec Link sur PC.",
            DisableImpact = "Les casques Oculus/Meta Quest ne fonctionneront pas avec le PC.",
            PerformanceImpact = "Modéré.",
            Recommendation = "Désactivez si vous n'utilisez pas de casque VR Oculus.",
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
            FullDescription = "Service de gestion pour les disques externes Western Digital. Peut inclure des fonctionnalités de sauvegarde et sécurité.",
            DisableImpact = "Certaines fonctionnalités WD peuvent ne pas fonctionner.",
            PerformanceImpact = "Très faible.",
            Recommendation = "Peut être désactivé si vous n'utilisez pas les fonctionnalités WD avancées.",
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
            FullDescription = "DTS Audio Processing Object améliore la qualité audio avec des effets surround et égalisation sur les PC compatibles.",
            DisableImpact = "Les effets audio DTS ne seront pas appliqués.",
            PerformanceImpact = "Très faible.",
            Recommendation = "Gardez actif si vous utilisez les améliorations audio DTS.",
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
            FullDescription = "Service pour contrôler l'éclairage RGB dynamique sur les périphériques Logitech compatibles LampArray.",
            DisableImpact = "L'éclairage dynamique Logitech ne fonctionnera pas.",
            PerformanceImpact = "Très faible.",
            Recommendation = "Gardez actif si vous utilisez l'éclairage RGB Logitech.",
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
            FullDescription = "PLAUD est l'application compagnon pour les enregistreurs vocaux PLAUD. Permet de transcrire et gérer les enregistrements audio.",
            DisableImpact = "L'application PLAUD ne s'ouvrira pas au démarrage.",
            PerformanceImpact = "Faible.",
            Recommendation = "Peut être désactivé si vous n'utilisez pas régulièrement l'enregistreur.",
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
            FullDescription = "Ce service AMD gère les événements externes comme les changements d'affichage, les hotkeys et la communication avec les pilotes graphiques.",
            DisableImpact = "Certaines fonctionnalités AMD (hotkeys, détection d'affichage) peuvent ne pas fonctionner.",
            PerformanceImpact = "Très faible.",
            Recommendation = "Gardez actif si vous avez une carte graphique AMD.",
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
            FullDescription = "Ce service vérifie les mises à jour disponibles pour les logiciels et pilotes ASUS sur votre système.",
            DisableImpact = "Pas de notifications de mises à jour ASUS.",
            PerformanceImpact = "Très faible.",
            Recommendation = "Peut être désactivé. Vérifiez manuellement via MyASUS.",
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
            FullDescription = "ASUS GameSDK optimise les performances système pour les jeux sur les appareils ASUS. Fait partie de l'écosystème Armoury Crate.",
            DisableImpact = "Les optimisations gaming automatiques ASUS ne s'appliqueront pas.",
            PerformanceImpact = "Faible.",
            Recommendation = "Gardez actif si vous utilisez Armoury Crate pour le gaming.",
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
            FullDescription = "Service de filtrage pour PDF-XChange Editor Pro, permettant l'intégration système et les fonctionnalités avancées d'édition PDF.",
            DisableImpact = "Certaines fonctionnalités d'intégration PDF-XChange peuvent ne pas fonctionner.",
            PerformanceImpact = "Très faible.",
            Recommendation = "Gardez actif si vous utilisez PDF-XChange régulièrement.",
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
            FullDescription = "Service Intel qui collecte des données d'utilisation système pour l'amélioration des produits. Peut être désactivé sans impact.",
            DisableImpact = "Pas de collecte de données d'utilisation Intel.",
            PerformanceImpact = "Très faible.",
            Recommendation = "Peut être désactivé pour la vie privée.",
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
            FullDescription = "Service Intel pour la gestion de l'énergie et l'optimisation des performances sur les systèmes Intel.",
            DisableImpact = "Les optimisations d'énergie Intel peuvent ne pas fonctionner.",
            PerformanceImpact = "Très faible.",
            Recommendation = "Peut être désactivé sur les PC de bureau.",
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
            FullDescription = "Google Updater maintient à jour les applications Google installées (Chrome, Drive, Earth, etc.).",
            DisableImpact = "Les applications Google ne se mettront pas à jour automatiquement.",
            PerformanceImpact = "Très faible.",
            Recommendation = "Gardez actif pour les mises à jour de sécurité automatiques.",
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
            FullDescription = "Service qui gère l'interface TWAIN pour les scanners et appareils d'imagerie.",
            DisableImpact = "Les scanners TWAIN peuvent ne pas fonctionner correctement.",
            PerformanceImpact = "Très faible.",
            Recommendation = "Gardez actif si vous utilisez un scanner.",
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
            FullDescription = "Service de commande pour Radeon Software, gérant les fonctionnalités avancées des cartes graphiques AMD.",
            DisableImpact = "Certaines fonctionnalités Radeon Software peuvent ne pas fonctionner.",
            PerformanceImpact = "Faible.",
            Recommendation = "Gardez actif si vous utilisez Radeon Software.",
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
            FullDescription = "AMD ReLive permet d'enregistrer et de streamer vos sessions de jeu avec les cartes graphiques AMD.",
            DisableImpact = "L'enregistrement et le streaming ReLive ne seront pas disponibles.",
            PerformanceImpact = "Faible au repos, modéré lors de l'enregistrement.",
            Recommendation = "Peut être désactivé si vous n'utilisez pas ReLive.",
            Tags = "amd,relive,recording,streaming,gaming",
            LastUpdated = DateTime.Now
        });
    }

    private void Save(KnowledgeEntry entry)
    {
        _service.SaveEntry(entry);
    }
}
