using System.Globalization;
using System.Resources;

namespace BootSentry.UI.Resources;

/// <summary>
/// Provides localized strings for the application.
/// </summary>
public static class Strings
{
    private static readonly Dictionary<string, Dictionary<string, string>> Resources = new()
    {
        ["fr"] = new Dictionary<string, string>
        {
            // Application
            ["AppTitle"] = "BootSentry",
            ["AppSubtitle"] = "Gestionnaire de démarrage Windows",

            // Menu
            ["MenuFile"] = "Fichier",
            ["MenuExport"] = "Exporter...",
            ["MenuExportJson"] = "Exporter en JSON...",
            ["MenuExportCsv"] = "Exporter en CSV...",
            ["MenuSettings"] = "Paramètres",
            ["MenuExit"] = "Quitter",
            ["MenuView"] = "Affichage",
            ["MenuRefresh"] = "Actualiser",
            ["MenuExpertMode"] = "Mode Expert",
            ["MenuHelp"] = "Aide",
            ["MenuAbout"] = "À propos",
            ["MenuCheckUpdates"] = "Vérifier les mises à jour",
            ["MenuDocumentation"] = "Documentation",

            // Dashboard
            ["DashboardTitle"] = "Tableau de bord",
            ["TotalEntries"] = "Entrées totales",
            ["EnabledEntries"] = "Actives",
            ["DisabledEntries"] = "Désactivées",
            ["SuspiciousEntries"] = "Suspectes",
            ["UnknownEntries"] = "Inconnues",

            // Entry List
            ["ColumnName"] = "Nom",
            ["ColumnType"] = "Type",
            ["ColumnScope"] = "Portée",
            ["ColumnPublisher"] = "Éditeur",
            ["ColumnSignature"] = "Signature",
            ["ColumnStatus"] = "Statut",
            ["ColumnRisk"] = "Risque",
            ["ColumnPath"] = "Chemin",

            // Actions
            ["ActionEnable"] = "Activer",
            ["ActionDisable"] = "Désactiver",
            ["ActionDelete"] = "Supprimer",
            ["ActionRestore"] = "Restaurer",
            ["ActionOpenLocation"] = "Ouvrir l'emplacement",
            ["ActionCopyPath"] = "Copier le chemin",
            ["ActionProperties"] = "Propriétés",
            ["ActionWebSearch"] = "Rechercher sur le web",

            // Status
            ["StatusReady"] = "Prêt",
            ["StatusScanning"] = "Analyse en cours...",
            ["StatusScanComplete"] = "Scan terminé - {0} entrées trouvées",
            ["StatusDisabling"] = "Désactivation de {0}...",
            ["StatusEnabling"] = "Activation de {0}...",
            ["StatusDeleting"] = "Suppression de {0}...",
            ["StatusExporting"] = "Exportation...",
            ["StatusExportComplete"] = "Export terminé: {0}",

            // Entry Types
            ["TypeRegistryRun"] = "Registre (Run)",
            ["TypeRegistryRunOnce"] = "Registre (RunOnce)",
            ["TypeStartupFolder"] = "Dossier de démarrage",
            ["TypeScheduledTask"] = "Tâche planifiée",
            ["TypeService"] = "Service",
            ["TypeDriver"] = "Pilote",
            ["TypeIFEO"] = "IFEO",
            ["TypeWinlogon"] = "Winlogon",
            ["TypeRegistryPolicies"] = "Stratégies",

            // Risk Levels
            ["RiskSafe"] = "Sûr",
            ["RiskUnknown"] = "Inconnu",
            ["RiskSuspicious"] = "Suspect",
            ["RiskCritical"] = "Critique",

            // Signature Status
            ["SignatureTrusted"] = "Signé (confiance)",
            ["SignatureUntrusted"] = "Signé (non approuvé)",
            ["SignatureUnsigned"] = "Non signé",
            ["SignatureUnknown"] = "Inconnu",

            // Entry Status
            ["EntryEnabled"] = "Activé",
            ["EntryDisabled"] = "Désactivé",
            ["EntryUnknown"] = "Inconnu",

            // Entry Scope
            ["ScopeUser"] = "Utilisateur",
            ["ScopeMachine"] = "Machine",

            // Dialogs
            ["ConfirmDeleteTitle"] = "Confirmer la suppression",
            ["ConfirmDeleteMessage"] = "Êtes-vous sûr de vouloir supprimer '{0}'?\n\nUn backup sera créé et permettra de restaurer cette entrée.",
            ["ErrorTitle"] = "Erreur",
            ["WarningTitle"] = "Attention",
            ["InfoTitle"] = "Information",
            ["ProtectedEntryMessage"] = "Cette entrée est protégée: {0}",
            ["AdminRequiredTitle"] = "Élévation requise",
            ["AdminRequiredMessage"] = "Cette action nécessite des droits administrateur.\n\nVoulez-vous relancer l'application en tant qu'administrateur?",

            // Settings
            ["SettingsTitle"] = "Paramètres",
            ["SettingsGeneral"] = "Général",
            ["SettingsAppearance"] = "Apparence",
            ["SettingsBackups"] = "Sauvegardes",
            ["SettingsAdvanced"] = "Avancé",
            ["SettingsLanguage"] = "Langue",
            ["SettingsTheme"] = "Thème",
            ["SettingsThemeSystem"] = "Système",
            ["SettingsThemeLight"] = "Clair",
            ["SettingsThemeDark"] = "Sombre",
            ["SettingsCheckUpdates"] = "Vérifier les mises à jour au démarrage",
            ["SettingsBackupRetention"] = "Conserver les backups pendant",
            ["SettingsDays"] = "jours",
            ["SettingsPurgeData"] = "Purger toutes les données",
            ["SettingsPurgeConfirm"] = "Cela supprimera tous les logs, backups et préférences. Continuer?",

            // History
            ["HistoryTitle"] = "Historique",
            ["HistoryDate"] = "Date",
            ["HistoryAction"] = "Action",
            ["HistoryEntry"] = "Entrée",
            ["HistoryStatus"] = "Statut",
            ["HistoryEmpty"] = "Aucune action enregistrée",
            ["HistoryRestoreEntry"] = "Restaurer cette entrée",

            // Search
            ["SearchPlaceholder"] = "Rechercher...",
            ["SearchResults"] = "{0} résultats",

            // Update
            ["UpdateAvailable"] = "Nouvelle version disponible",
            ["UpdateMessage"] = "La version {0} est disponible. Voulez-vous la télécharger?",
            ["UpdateDownload"] = "Télécharger",
            ["UpdateLater"] = "Plus tard",
            ["UpdateIgnore"] = "Ignorer cette version",
            ["UpdateUpToDate"] = "Vous utilisez la dernière version",

            // Onboarding
            ["OnboardingWelcome"] = "Bienvenue dans BootSentry",
            ["OnboardingDescription"] = "BootSentry vous aide à gérer les programmes qui démarrent avec Windows.",
            ["OnboardingModeQuestion"] = "Quel mode souhaitez-vous utiliser?",
            ["OnboardingModePublic"] = "Mode Standard",
            ["OnboardingModePublicDesc"] = "Recommandé pour la plupart des utilisateurs. Cache les entrées système Microsoft et les éléments critiques.",
            ["OnboardingModeExpert"] = "Mode Expert",
            ["OnboardingModeExpertDesc"] = "Affiche toutes les entrées y compris les éléments système. Réservé aux utilisateurs avancés.",
            ["OnboardingNext"] = "Suivant",
            ["OnboardingPrevious"] = "Précédent",
            ["OnboardingFinish"] = "Commencer",
            ["OnboardingDontShowAgain"] = "Ne plus afficher",

            // Detail Panel
            ["DetailTitle"] = "Détails",
            ["DetailSource"] = "Source",
            ["DetailTarget"] = "Cible",
            ["DetailCommandLine"] = "Ligne de commande",
            ["DetailArguments"] = "Arguments",
            ["DetailWorkingDir"] = "Répertoire de travail",
            ["DetailPublisher"] = "Éditeur",
            ["DetailSignature"] = "Signature",
            ["DetailFileVersion"] = "Version du fichier",
            ["DetailProductName"] = "Nom du produit",
            ["DetailFileSize"] = "Taille",
            ["DetailLastModified"] = "Dernière modification",
            ["DetailHash"] = "Hash SHA-256",
            ["DetailCalculateHash"] = "Calculer",
            ["DetailRiskAnalysis"] = "Analyse de risque",

            // Misc
            ["Yes"] = "Oui",
            ["No"] = "Non",
            ["OK"] = "OK",
            ["Cancel"] = "Annuler",
            ["Close"] = "Fermer",
            ["Apply"] = "Appliquer",
            ["Save"] = "Enregistrer",
            ["Loading"] = "Chargement...",
            ["NoSelection"] = "Aucune sélection",
            ["FileNotFound"] = "Fichier introuvable",
            ["FolderNotFound"] = "Dossier introuvable",
            ["PathCopied"] = "Chemin copié dans le presse-papiers",
            ["ExpertModeEnabled"] = "Mode Expert activé",
            ["ExpertModeDisabled"] = "Mode Expert désactivé",
            ["RunningAsAdmin"] = "Exécution en tant qu'administrateur",
            ["NotRunningAsAdmin"] = "Mode standard (certaines actions nécessitent une élévation)",
        },

        ["en"] = new Dictionary<string, string>
        {
            // Application
            ["AppTitle"] = "BootSentry",
            ["AppSubtitle"] = "Windows Startup Manager",

            // Menu
            ["MenuFile"] = "File",
            ["MenuExport"] = "Export...",
            ["MenuExportJson"] = "Export to JSON...",
            ["MenuExportCsv"] = "Export to CSV...",
            ["MenuSettings"] = "Settings",
            ["MenuExit"] = "Exit",
            ["MenuView"] = "View",
            ["MenuRefresh"] = "Refresh",
            ["MenuExpertMode"] = "Expert Mode",
            ["MenuHelp"] = "Help",
            ["MenuAbout"] = "About",
            ["MenuCheckUpdates"] = "Check for Updates",
            ["MenuDocumentation"] = "Documentation",

            // Dashboard
            ["DashboardTitle"] = "Dashboard",
            ["TotalEntries"] = "Total Entries",
            ["EnabledEntries"] = "Enabled",
            ["DisabledEntries"] = "Disabled",
            ["SuspiciousEntries"] = "Suspicious",
            ["UnknownEntries"] = "Unknown",

            // Entry List
            ["ColumnName"] = "Name",
            ["ColumnType"] = "Type",
            ["ColumnScope"] = "Scope",
            ["ColumnPublisher"] = "Publisher",
            ["ColumnSignature"] = "Signature",
            ["ColumnStatus"] = "Status",
            ["ColumnRisk"] = "Risk",
            ["ColumnPath"] = "Path",

            // Actions
            ["ActionEnable"] = "Enable",
            ["ActionDisable"] = "Disable",
            ["ActionDelete"] = "Delete",
            ["ActionRestore"] = "Restore",
            ["ActionOpenLocation"] = "Open Location",
            ["ActionCopyPath"] = "Copy Path",
            ["ActionProperties"] = "Properties",
            ["ActionWebSearch"] = "Search on Web",

            // Status
            ["StatusReady"] = "Ready",
            ["StatusScanning"] = "Scanning...",
            ["StatusScanComplete"] = "Scan complete - {0} entries found",
            ["StatusDisabling"] = "Disabling {0}...",
            ["StatusEnabling"] = "Enabling {0}...",
            ["StatusDeleting"] = "Deleting {0}...",
            ["StatusExporting"] = "Exporting...",
            ["StatusExportComplete"] = "Export complete: {0}",

            // Entry Types
            ["TypeRegistryRun"] = "Registry (Run)",
            ["TypeRegistryRunOnce"] = "Registry (RunOnce)",
            ["TypeStartupFolder"] = "Startup Folder",
            ["TypeScheduledTask"] = "Scheduled Task",
            ["TypeService"] = "Service",
            ["TypeDriver"] = "Driver",
            ["TypeIFEO"] = "IFEO",
            ["TypeWinlogon"] = "Winlogon",
            ["TypeRegistryPolicies"] = "Policies",

            // Risk Levels
            ["RiskSafe"] = "Safe",
            ["RiskUnknown"] = "Unknown",
            ["RiskSuspicious"] = "Suspicious",
            ["RiskCritical"] = "Critical",

            // Signature Status
            ["SignatureTrusted"] = "Signed (Trusted)",
            ["SignatureUntrusted"] = "Signed (Untrusted)",
            ["SignatureUnsigned"] = "Unsigned",
            ["SignatureUnknown"] = "Unknown",

            // Entry Status
            ["EntryEnabled"] = "Enabled",
            ["EntryDisabled"] = "Disabled",
            ["EntryUnknown"] = "Unknown",

            // Entry Scope
            ["ScopeUser"] = "User",
            ["ScopeMachine"] = "Machine",

            // Dialogs
            ["ConfirmDeleteTitle"] = "Confirm Deletion",
            ["ConfirmDeleteMessage"] = "Are you sure you want to delete '{0}'?\n\nA backup will be created to allow restoration.",
            ["ErrorTitle"] = "Error",
            ["WarningTitle"] = "Warning",
            ["InfoTitle"] = "Information",
            ["ProtectedEntryMessage"] = "This entry is protected: {0}",
            ["AdminRequiredTitle"] = "Elevation Required",
            ["AdminRequiredMessage"] = "This action requires administrator privileges.\n\nDo you want to restart the application as administrator?",

            // Settings
            ["SettingsTitle"] = "Settings",
            ["SettingsGeneral"] = "General",
            ["SettingsAppearance"] = "Appearance",
            ["SettingsBackups"] = "Backups",
            ["SettingsAdvanced"] = "Advanced",
            ["SettingsLanguage"] = "Language",
            ["SettingsTheme"] = "Theme",
            ["SettingsThemeSystem"] = "System",
            ["SettingsThemeLight"] = "Light",
            ["SettingsThemeDark"] = "Dark",
            ["SettingsCheckUpdates"] = "Check for updates on startup",
            ["SettingsBackupRetention"] = "Keep backups for",
            ["SettingsDays"] = "days",
            ["SettingsPurgeData"] = "Purge all data",
            ["SettingsPurgeConfirm"] = "This will delete all logs, backups and preferences. Continue?",

            // History
            ["HistoryTitle"] = "History",
            ["HistoryDate"] = "Date",
            ["HistoryAction"] = "Action",
            ["HistoryEntry"] = "Entry",
            ["HistoryStatus"] = "Status",
            ["HistoryEmpty"] = "No actions recorded",
            ["HistoryRestoreEntry"] = "Restore this entry",

            // Search
            ["SearchPlaceholder"] = "Search...",
            ["SearchResults"] = "{0} results",

            // Update
            ["UpdateAvailable"] = "Update Available",
            ["UpdateMessage"] = "Version {0} is available. Would you like to download it?",
            ["UpdateDownload"] = "Download",
            ["UpdateLater"] = "Later",
            ["UpdateIgnore"] = "Ignore this version",
            ["UpdateUpToDate"] = "You are using the latest version",

            // Onboarding
            ["OnboardingWelcome"] = "Welcome to BootSentry",
            ["OnboardingDescription"] = "BootSentry helps you manage programs that start with Windows.",
            ["OnboardingModeQuestion"] = "Which mode would you like to use?",
            ["OnboardingModePublic"] = "Standard Mode",
            ["OnboardingModePublicDesc"] = "Recommended for most users. Hides Microsoft system entries and critical items.",
            ["OnboardingModeExpert"] = "Expert Mode",
            ["OnboardingModeExpertDesc"] = "Shows all entries including system items. For advanced users only.",
            ["OnboardingNext"] = "Next",
            ["OnboardingPrevious"] = "Previous",
            ["OnboardingFinish"] = "Get Started",
            ["OnboardingDontShowAgain"] = "Don't show again",

            // Detail Panel
            ["DetailTitle"] = "Details",
            ["DetailSource"] = "Source",
            ["DetailTarget"] = "Target",
            ["DetailCommandLine"] = "Command Line",
            ["DetailArguments"] = "Arguments",
            ["DetailWorkingDir"] = "Working Directory",
            ["DetailPublisher"] = "Publisher",
            ["DetailSignature"] = "Signature",
            ["DetailFileVersion"] = "File Version",
            ["DetailProductName"] = "Product Name",
            ["DetailFileSize"] = "Size",
            ["DetailLastModified"] = "Last Modified",
            ["DetailHash"] = "SHA-256 Hash",
            ["DetailCalculateHash"] = "Calculate",
            ["DetailRiskAnalysis"] = "Risk Analysis",

            // Misc
            ["Yes"] = "Yes",
            ["No"] = "No",
            ["OK"] = "OK",
            ["Cancel"] = "Cancel",
            ["Close"] = "Close",
            ["Apply"] = "Apply",
            ["Save"] = "Save",
            ["Loading"] = "Loading...",
            ["NoSelection"] = "No selection",
            ["FileNotFound"] = "File not found",
            ["FolderNotFound"] = "Folder not found",
            ["PathCopied"] = "Path copied to clipboard",
            ["ExpertModeEnabled"] = "Expert Mode enabled",
            ["ExpertModeDisabled"] = "Expert Mode disabled",
            ["RunningAsAdmin"] = "Running as Administrator",
            ["NotRunningAsAdmin"] = "Standard mode (some actions require elevation)",
        }
    };

    private static string _currentLanguage = "fr";

    /// <summary>
    /// Gets or sets the current language code.
    /// </summary>
    public static string CurrentLanguage
    {
        get => _currentLanguage;
        set
        {
            if (Resources.ContainsKey(value))
            {
                _currentLanguage = value;
                LanguageChanged?.Invoke(null, EventArgs.Empty);
            }
        }
    }

    /// <summary>
    /// Event raised when the language changes.
    /// </summary>
    public static event EventHandler? LanguageChanged;

    /// <summary>
    /// Gets a localized string by key.
    /// </summary>
    public static string Get(string key)
    {
        if (Resources.TryGetValue(_currentLanguage, out var lang) && lang.TryGetValue(key, out var value))
            return value;

        // Fallback to English
        if (Resources.TryGetValue("en", out var en) && en.TryGetValue(key, out var fallback))
            return fallback;

        return $"[{key}]";
    }

    /// <summary>
    /// Gets a formatted localized string.
    /// </summary>
    public static string Format(string key, params object[] args)
    {
        return string.Format(Get(key), args);
    }

    /// <summary>
    /// Initializes the language based on system culture.
    /// </summary>
    public static void Initialize()
    {
        var culture = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
        if (Resources.ContainsKey(culture))
            _currentLanguage = culture;
        else
            _currentLanguage = "en";
    }

    /// <summary>
    /// Gets all available languages.
    /// </summary>
    public static IReadOnlyDictionary<string, string> AvailableLanguages => new Dictionary<string, string>
    {
        ["fr"] = "Français",
        ["en"] = "English"
    };
}
