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
            ["AppTitleFull"] = "BootSentry - Gestionnaire de démarrage Windows",
            ["AppSubtitle"] = "Gestionnaire de démarrage Windows",

            // Menu
            ["MenuMain"] = "Menu principal",
            ["MenuFile"] = "_Fichier",
            ["MenuExport"] = "Exporter...",
            ["MenuExportJson"] = "_Exporter en JSON...",
            ["MenuExportCsv"] = "Exporter en _CSV...",
            ["MenuExportDiagnostics"] = "Exporter _diagnostics (ZIP)...",
            ["MenuSettings"] = "_Paramètres...",
            ["MenuExit"] = "_Quitter",
            ["MenuView"] = "_Affichage",
            ["MenuRefresh"] = "_Actualiser",
            ["MenuExpertMode"] = "Mode _Expert",
            ["MenuHistory"] = "_Historique",
            ["MenuActions"] = "_Actions",
            ["MenuHelp"] = "_Aide",
            ["MenuAbout"] = "À _propos de BootSentry",
            ["MenuCheckUpdates"] = "Vérifier les _mises à jour",
            ["MenuDocumentation"] = "_Documentation",

            // Dashboard
            ["DashboardTitle"] = "Tableau de bord",
            ["TotalEntries"] = "Total",
            ["EnabledEntries"] = "Actives",
            ["DisabledEntries"] = "Désactivées",
            ["SuspiciousEntries"] = "Suspectes",
            ["UnknownEntries"] = "Inconnues",
            ["Expert"] = "Expert",
            ["Entries"] = "entrées",

            // Tabs
            ["TabStartup"] = "Démarrage",
            ["TabTasks"] = "Tâches",
            ["TabServices"] = "Services",
            ["TabSystem"] = "Système",
            ["TabExtensions"] = "Extensions",

            // Empty state
            ["NoResultsFound"] = "Aucun résultat trouvé",
            ["NoResultsHint"] = "Modifiez vos critères de recherche ou de filtrage",
            ["ResetFilters"] = "Réinitialiser les filtres",

            // Entry List
            ["ColumnName"] = "Nom",
            ["ColumnDescription"] = "Description",
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
            ["ActionRestore"] = "_Restaurer",
            ["ActionUndo"] = "A_nnuler",
            ["ActionOpenLocation"] = "_Ouvrir l'emplacement",
            ["ActionCopyPath"] = "_Copier le chemin",
            ["ActionCalculateHash"] = "Calculer le _hash",
            ["ActionProperties"] = "Propriétés",
            ["ActionWebSearch"] = "Rechercher sur le _Web",
            ["ActionScanAntivirus"] = "_Scanner avec l'antivirus",
            ["ActionScanAll"] = "Scan _global antivirus",
            ["ActionOpenRegedit"] = "Ouvrir dans _Regedit",
            ["ActionOpenServices"] = "Ouvrir _Services.msc",
            ["ActionOpenTaskScheduler"] = "Ouvrir _Planificateur de tâches",
            ["ActionEnableSelection"] = "Activer la sélection",
            ["ActionDisableSelection"] = "Désactiver la sélection",

            // Status
            ["StatusReady"] = "Prêt",
            ["StatusScanning"] = "Analyse en cours...",
            ["ScanningProviders"] = "Analyse des sources de démarrage...",
            ["StatusScanComplete"] = "Scan terminé - {0} entrées trouvées",

            // Progress steps
            ["Step"] = "Étape",
            ["ProgressStep1"] = "Initialisation...",
            ["ProgressScanning"] = "{0}: {1} entrées trouvées",
            ["ProgressAnalyzing"] = "Analyse des risques...",
            ["ProgressFinalizing"] = "Finalisation...",
            ["ProgressComplete"] = "Scan terminé - {0} entrées trouvées",
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
            ["TypeBrowserExtension"] = "Extension navigateur",

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
            ["ErrorActionImpossible"] = "Action impossible",
            ["ErrorNotARegistryKey"] = "Cette entrée n'est pas une clé de registre.",
            ["ErrorNotAService"] = "Cette entrée n'est pas un service.",
            ["ErrorNotATask"] = "Cette entrée n'est pas une tâche planifiée.",
            ["ErrorOpeningServices"] = "Erreur lors de l'ouverture de Services.msc",
            ["ErrorOpeningTaskScheduler"] = "Erreur lors de l'ouverture du Planificateur de tâches",
            ["ProtectedEntryMessage"] = "Cette entrée est protégée: {0}",
            ["AdminRequiredTitle"] = "Élévation requise",
            ["AdminRequiredMessage"] = "Cette action nécessite des droits administrateur.\n\nVoulez-vous relancer l'application en tant qu'administrateur?",

            // Settings
            ["SettingsWindowTitle"] = "Paramètres - BootSentry",
            ["SettingsTitle"] = "Paramètres",
            ["SettingsGeneral"] = "Général",
            ["SettingsAppearance"] = "Apparence",
            ["SettingsBackups"] = "Sauvegardes",
            ["SettingsAdvanced"] = "Avancé",
            ["SettingsLanguage"] = "Langue",
            ["SettingsLanguageHint"] = "Redémarrage requis pour certains éléments",
            ["SettingsExpertDefault"] = "Démarrer en mode Expert par défaut",
            ["SettingsShowOnboarding"] = "Afficher l'assistant au premier démarrage",
            ["SettingsTheme"] = "Thème",
            ["ThemePreview"] = "Aperçu du thème",
            ["SettingsThemeSystem"] = "Système",
            ["SettingsThemeLight"] = "Clair",
            ["SettingsThemeDark"] = "Sombre",
            ["SettingsCheckUpdates"] = "Vérifier les mises à jour au démarrage",
            ["SettingsBackupRetention"] = "Conservation des backups",
            ["SettingsBackupHint"] = "0 = conserver indéfiniment",
            ["SettingsDays"] = " jours",
            ["SettingsAutoHash"] = "Calculer automatiquement les hashes SHA-256",
            ["SettingsAutoHashWarning"] = "Attention: peut ralentir le scan pour les gros fichiers",
            ["SettingsDangerZone"] = "Zone de danger",
            ["SettingsResetButton"] = "Réinitialiser les paramètres",
            ["SettingsPurgeData"] = "Purger toutes les données",
            ["SettingsPurgeHint"] = "Supprime les logs, backups et préférences",
            ["SettingsPurgeConfirm"] = "Cela supprimera tous les logs, backups et préférences. Continuer?",

            // History
            ["HistoryWindowTitle"] = "Historique - BootSentry",
            ["HistoryTitle"] = "Historique des actions",
            ["HistorySubtitle"] = "Consultez et restaurez les modifications précédentes",
            ["HistoryDate"] = "Date",
            ["HistoryAction"] = "Action",
            ["HistoryEntry"] = "Entrée",
            ["HistoryStatus"] = "Statut",
            ["HistoryEmpty"] = "Aucune action enregistrée",
            ["HistoryRestoreEntry"] = "Restaurer cette entrée",
            ["HistoryPurgeOld"] = "Purger les anciens",
            ["HistoryLoading"] = "Chargement de l'historique...",
            ["HistoryCount"] = "{0} transactions",
            ["HistoryLoadError"] = "Erreur lors du chargement de l'historique",
            ["HistoryRestoring"] = "Restauration en cours...",
            ["HistoryRestoreSuccess"] = "Restauration effectuée avec succès",
            ["HistoryRestoreError"] = "Erreur lors de la restauration: {0}",
            ["HistoryPurged"] = "{0} anciennes transactions supprimées",
            ["HistoryPurgeError"] = "Erreur lors de la purge",
            ["HistoryCanRestore"] = "Restaurable",
            ["HistoryCannotRestore"] = "Non restaurable",
            ["HistoryRestored"] = "Restauré",
            ["HistoryPurgeButton"] = "Purger les anciens",
            ["HistoryRestoreButton"] = "Restaurer",
            ["HistoryUser"] = "Utilisateur",
            ["HistoryType"] = "Type",

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

            // About
            ["AboutTitle"] = "À propos de BootSentry",
            ["AboutDescription"] = "BootSentry vous permet de gérer facilement les programmes qui démarrent automatiquement avec Windows, en toute sécurité grâce au système de backup intégré.",
            ["AboutAuthor"] = "Auteur:",
            ["AboutLicense"] = "Licence:",
            ["AboutLicenseBtn"] = "Licence",

            // Onboarding
            ["OnboardingWindowTitle"] = "Bienvenue - BootSentry",
            ["OnboardingWelcome"] = "Bienvenue !",
            ["OnboardingStep1of2"] = "Étape 1 sur 2",
            ["OnboardingStep2of2"] = "Étape 2 sur 2",
            ["OnboardingDescription"] = "BootSentry vous permet de gérer facilement les programmes qui démarrent automatiquement avec Windows.",
            ["OnboardingDesc2"] = "Vous pouvez désactiver, réactiver ou supprimer les entrées de démarrage en toute sécurité grâce au système de backup intégré.",
            ["OnboardingModeQuestion"] = "Choisissez votre mode",
            ["OnboardingModePublic"] = "Mode Standard (Recommandé)",
            ["OnboardingModePublicDesc"] = "Cache les entrées système Microsoft et les éléments critiques. Idéal pour la plupart des utilisateurs.",
            ["OnboardingModeExpert"] = "Mode Expert",
            ["OnboardingModeExpertDesc"] = "Affiche toutes les entrées y compris les services système et les pilotes. Pour utilisateurs avancés uniquement.",
            ["OnboardingNext"] = "Suivant",
            ["OnboardingPrevious"] = "Précédent",
            ["OnboardingFinish"] = "Commencer",
            ["OnboardingStart"] = "Commencer",
            ["OnboardingDontShowAgain"] = "Ne plus afficher",
            ["OnboardingStepN"] = "Étape {0} sur {1}",
            ["OnboardingShortcutsTitle"] = "Raccourcis clavier",
            ["OnboardingShortcutsDesc"] = "Voici les raccourcis essentiels pour utiliser BootSentry efficacement:",
            ["ShortcutRefresh"] = "Actualiser la liste",
            ["ShortcutSearch"] = "Rechercher une entrée",
            ["ShortcutDelete"] = "Supprimer l'entrée sélectionnée",
            ["ShortcutUndo"] = "Annuler la dernière action",
            ["ShortcutHelp"] = "Afficher l'aide",

            // Detail Panel
            ["DetailTitle"] = "Détails",
            ["SelectEntryHint"] = "Sélectionnez une entrée pour voir ses détails",
            ["DetailType"] = "Type:",
            ["DetailScope"] = "Portée:",
            ["DetailSource"] = "Source:",
            ["DetailTarget"] = "Cible:",
            ["DetailCommandLine"] = "Ligne de commande:",
            ["DetailArguments"] = "Arguments",
            ["DetailWorkingDir"] = "Répertoire de travail",
            ["DetailPublisher"] = "Éditeur:",
            ["DetailSignature"] = "Signature:",
            ["DetailStatus"] = "Statut:",
            ["DetailScanAV"] = "Scan AV:",
            ["DetailNotes"] = "Notes:",
            ["DetailFileInfo"] = "Informations fichier",
            ["DetailFileVersion"] = "Version:",
            ["DetailProductName"] = "Produit:",
            ["DetailFileSize"] = "Taille:",
            ["DetailLastModified"] = "Modifié:",
            ["DetailHash"] = "Hash SHA-256",
            ["DetailCalculateHash"] = "Calculer",
            ["DetailRiskAnalysis"] = "Analyse de risque",

            // Knowledge Base
            ["KnowledgeInfo"] = "Informations",
            ["KnowledgeSafetyLevel"] = "Niveau de sécurité:",
            ["KnowledgeFullDesc"] = "Description détaillée",
            ["KnowledgeDisableImpact"] = "Impact si désactivé",
            ["KnowledgePerfImpact"] = "Impact sur les performances",
            ["KnowledgeRecommendation"] = "Recommandation",
            ["NoKnowledgeInfo"] = "Aucune information disponible",
            ["NoKnowledgeInfoSub"] = "dans la base de connaissances",

            // Action Buttons
            ["BtnOpen"] = "Ouvrir",
            ["BtnCopy"] = "Copier",
            ["BtnWeb"] = "Web",
            ["BtnHash"] = "Hash",
            ["BtnScanAV"] = "Scan AV",
            ["CancelScan"] = "Annuler le scan",

            // Malware Scan
            ["ScanResult"] = "Résultat du scan",
            ["ScanResultClean"] = "Fichier sain",
            ["ScanResultMalware"] = "Menace détectée!",
            ["ScanResultBlocked"] = "Bloqué",
            ["ScanResultNotScanned"] = "Non scanné",
            ["ScanResultError"] = "Erreur",
            ["ScanInProgress"] = "Scan antivirus en cours...",
            ["ScanComplete"] = "Scan terminé",
            ["ScanNotAvailable"] = "Le scanner antivirus n'est pas disponible",
            ["MalwareDetectedTitle"] = "Menace détectée",
            ["MalwareDetectedMessage"] = "Le fichier a été identifié comme malveillant. Il est recommandé de le désactiver ou supprimer.",

            // Tooltips
            ["TooltipRefresh"] = "Actualiser la liste (F5)",
            ["TooltipDisable"] = "Désactiver l'entrée sélectionnée (Suppr)",
            ["TooltipDelete"] = "Supprimer définitivement (Ctrl+Suppr)",
            ["SplitterTooltip"] = "Glisser pour redimensionner les panneaux",

            // Accessibility HelpText
            ["ActionEnableHelp"] = "Active l'entrée de démarrage sélectionnée",
            ["ActionDisableHelp"] = "Désactive l'entrée de démarrage sélectionnée",
            ["ActionDeleteHelp"] = "Supprime définitivement l'entrée sélectionnée (un backup sera créé)",
            ["StatusFilterLabel"] = "Filtre par statut",
            ["StatusFilterHelp"] = "Filtrer les entrées par leur statut (Tous, Activé, Désactivé)",
            ["DataGridHelp"] = "Liste des entrées de démarrage. Utilisez les flèches pour naviguer.",
            ["HistoryEmptyHint"] = "Les actions seront enregistrées ici",
            ["SelectEntryHintLarge"] = "Cliquez sur une entrée dans la liste pour afficher ses détails",
            ["LongOperationHint"] = "Cette opération peut prendre quelques secondes...",

            // Malware Scan Status (localized)
            ["ScanResultUnknown"] = "Inconnu",
            ["ScanResultCleanShort"] = "Fichier sain",
            ["ScanResultMalwareShort"] = "Menace détectée!",
            ["ScanResultBlockedShort"] = "Bloqué",
            ["ScanResultNotScannedShort"] = "Non scanné",
            ["ScanResultTooLargeShort"] = "Fichier trop volumineux",
            ["ScanResultErrorShort"] = "Erreur",
            ["ScanResultNoAVShort"] = "Aucun antivirus",

            // Safety Levels (localized)
            ["SafetyUnknown"] = "Inconnu",
            ["SafetyCritical"] = "Critique - Ne pas désactiver",
            ["SafetyImportant"] = "Important",
            ["SafetySafe"] = "Peut être désactivé",
            ["SafetyRecommendedDisable"] = "Désactivation recommandée",
            ["SafetyShouldRemove"] = "À supprimer",

            // Admin Status
            ["AdminStatusAdmin"] = "Administrateur",
            ["AdminStatusStandard"] = "Standard",

            // Confirmations
            ["ConfirmDisableTitle"] = "Confirmer la désactivation",
            ["ConfirmDisableMessage"] = "Voulez-vous désactiver '{0}'?",
            ["ConfirmDeleteTitle"] = "Confirmer la suppression",
            ["ConfirmDeleteMessage"] = "Voulez-vous vraiment supprimer cette entrée?",
            ["ConfirmDeleteMultiple"] = "Voulez-vous vraiment supprimer {0} entrées?\n\nUn backup sera créé pour chaque entrée.",
            ["ConfirmPurgeTitle"] = "Confirmer la purge",
            ["ConfirmPurgeMessage"] = "Cette action supprimera définitivement toutes les données (logs, backups, préférences).\n\nCette action est irréversible. Continuer?",
            ["ConfirmPurgeButton"] = "Purger tout",
            ["ConfirmResetTitle"] = "Confirmer la réinitialisation",
            ["ConfirmResetMessage"] = "Voulez-vous réinitialiser tous les paramètres aux valeurs par défaut?",
            ["ConfirmResetButton"] = "Réinitialiser",
            ["Delete"] = "Supprimer",

            // Notifications/Feedback
            ["NotifDisabled"] = "'{0}' a été désactivé",
            ["NotifEnabled"] = "'{0}' a été activé",
            ["NotifDeleted"] = "'{0}' a été supprimé (backup créé)",
            ["NotifCopied"] = "Copié dans le presse-papiers",
            ["NotifExportSuccess"] = "Export réussi: {0}",

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
            ["AppTitleFull"] = "BootSentry - Windows Startup Manager",
            ["AppSubtitle"] = "Windows Startup Manager",

            // Menu
            ["MenuMain"] = "Main menu",
            ["MenuFile"] = "_File",
            ["MenuExport"] = "Export...",
            ["MenuExportJson"] = "_Export to JSON...",
            ["MenuExportCsv"] = "Export to _CSV...",
            ["MenuExportDiagnostics"] = "Export _diagnostics (ZIP)...",
            ["MenuSettings"] = "_Settings...",
            ["MenuExit"] = "E_xit",
            ["MenuView"] = "_View",
            ["MenuRefresh"] = "_Refresh",
            ["MenuExpertMode"] = "_Expert Mode",
            ["MenuHistory"] = "_History",
            ["MenuActions"] = "_Actions",
            ["MenuHelp"] = "_Help",
            ["MenuAbout"] = "_About BootSentry",
            ["MenuCheckUpdates"] = "Check for _Updates",
            ["MenuDocumentation"] = "_Documentation",

            // Dashboard
            ["DashboardTitle"] = "Dashboard",
            ["TotalEntries"] = "Total",
            ["EnabledEntries"] = "Enabled",
            ["DisabledEntries"] = "Disabled",
            ["SuspiciousEntries"] = "Suspicious",
            ["UnknownEntries"] = "Unknown",
            ["Expert"] = "Expert",
            ["Entries"] = "entries",

            // Tabs
            ["TabStartup"] = "Startup",
            ["TabTasks"] = "Tasks",
            ["TabServices"] = "Services",
            ["TabSystem"] = "System",
            ["TabExtensions"] = "Extensions",

            // Empty state
            ["NoResultsFound"] = "No results found",
            ["NoResultsHint"] = "Modify your search or filter criteria",
            ["ResetFilters"] = "Reset filters",

            // Entry List
            ["ColumnName"] = "Name",
            ["ColumnDescription"] = "Description",
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
            ["ActionRestore"] = "_Restore",
            ["ActionUndo"] = "_Undo",
            ["ActionOpenLocation"] = "_Open Location",
            ["ActionCopyPath"] = "_Copy Path",
            ["ActionCalculateHash"] = "Calculate _Hash",
            ["ActionProperties"] = "Properties",
            ["ActionWebSearch"] = "Search on _Web",
            ["ActionScanAntivirus"] = "_Scan with antivirus",
            ["ActionScanAll"] = "_Global antivirus scan",
            ["ActionOpenRegedit"] = "Open in _Regedit",
            ["ActionOpenServices"] = "Open _Services.msc",
            ["ActionOpenTaskScheduler"] = "Open _Task Scheduler",
            ["ActionEnableSelection"] = "Enable selection",
            ["ActionDisableSelection"] = "Disable selection",

            // Status
            ["StatusReady"] = "Ready",
            ["StatusScanning"] = "Scanning...",
            ["ScanningProviders"] = "Scanning startup sources...",
            ["StatusScanComplete"] = "Scan complete - {0} entries found",

            // Progress steps
            ["Step"] = "Step",
            ["ProgressStep1"] = "Initializing...",
            ["ProgressScanning"] = "{0}: {1} entries found",
            ["ProgressAnalyzing"] = "Analyzing risks...",
            ["ProgressFinalizing"] = "Finalizing...",
            ["ProgressComplete"] = "Scan complete - {0} entries found",
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
            ["TypeBrowserExtension"] = "Browser Extension",

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
            ["ErrorActionImpossible"] = "Action not possible",
            ["ErrorNotARegistryKey"] = "This entry is not a registry key.",
            ["ErrorNotAService"] = "This entry is not a service.",
            ["ErrorNotATask"] = "This entry is not a scheduled task.",
            ["ErrorOpeningServices"] = "Error opening Services.msc",
            ["ErrorOpeningTaskScheduler"] = "Error opening Task Scheduler",
            ["ProtectedEntryMessage"] = "This entry is protected: {0}",
            ["AdminRequiredTitle"] = "Elevation Required",
            ["AdminRequiredMessage"] = "This action requires administrator privileges.\n\nDo you want to restart the application as administrator?",

            // Settings
            ["SettingsWindowTitle"] = "Settings - BootSentry",
            ["SettingsTitle"] = "Settings",
            ["SettingsGeneral"] = "General",
            ["SettingsAppearance"] = "Appearance",
            ["SettingsBackups"] = "Backups",
            ["SettingsAdvanced"] = "Advanced",
            ["SettingsLanguage"] = "Language",
            ["SettingsLanguageHint"] = "Restart required for some elements",
            ["SettingsExpertDefault"] = "Start in Expert Mode by default",
            ["SettingsShowOnboarding"] = "Show onboarding wizard on first start",
            ["SettingsTheme"] = "Theme",
            ["ThemePreview"] = "Theme preview",
            ["SettingsThemeSystem"] = "System",
            ["SettingsThemeLight"] = "Light",
            ["SettingsThemeDark"] = "Dark",
            ["SettingsCheckUpdates"] = "Check for updates on startup",
            ["SettingsBackupRetention"] = "Backup retention",
            ["SettingsBackupHint"] = "0 = keep indefinitely",
            ["SettingsDays"] = " days",
            ["SettingsAutoHash"] = "Automatically calculate SHA-256 hashes",
            ["SettingsAutoHashWarning"] = "Warning: may slow down scanning for large files",
            ["SettingsDangerZone"] = "Danger zone",
            ["SettingsResetButton"] = "Reset settings",
            ["SettingsPurgeData"] = "Purge all data",
            ["SettingsPurgeHint"] = "Deletes logs, backups and preferences",
            ["SettingsPurgeConfirm"] = "This will delete all logs, backups and preferences. Continue?",

            // History
            ["HistoryWindowTitle"] = "History - BootSentry",
            ["HistoryTitle"] = "Action History",
            ["HistorySubtitle"] = "View and restore previous changes",
            ["HistoryDate"] = "Date",
            ["HistoryAction"] = "Action",
            ["HistoryEntry"] = "Entry",
            ["HistoryStatus"] = "Status",
            ["HistoryEmpty"] = "No actions recorded",
            ["HistoryRestoreEntry"] = "Restore this entry",
            ["HistoryPurgeOld"] = "Purge old",
            ["HistoryLoading"] = "Loading history...",
            ["HistoryCount"] = "{0} transactions",
            ["HistoryLoadError"] = "Error loading history",
            ["HistoryRestoring"] = "Restoring...",
            ["HistoryRestoreSuccess"] = "Restore completed successfully",
            ["HistoryRestoreError"] = "Error during restore: {0}",
            ["HistoryPurged"] = "{0} old transactions deleted",
            ["HistoryPurgeError"] = "Error during purge",
            ["HistoryCanRestore"] = "Can restore",
            ["HistoryCannotRestore"] = "Cannot restore",
            ["HistoryRestored"] = "Restored",
            ["HistoryPurgeButton"] = "Purge old",
            ["HistoryRestoreButton"] = "Restore",
            ["HistoryUser"] = "User",
            ["HistoryType"] = "Type",

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

            // About
            ["AboutTitle"] = "About BootSentry",
            ["AboutDescription"] = "BootSentry lets you easily manage programs that start automatically with Windows, safely thanks to the built-in backup system.",
            ["AboutAuthor"] = "Author:",
            ["AboutLicense"] = "License:",
            ["AboutLicenseBtn"] = "License",

            // Onboarding
            ["OnboardingWindowTitle"] = "Welcome - BootSentry",
            ["OnboardingWelcome"] = "Welcome!",
            ["OnboardingStep1of2"] = "Step 1 of 2",
            ["OnboardingStep2of2"] = "Step 2 of 2",
            ["OnboardingDescription"] = "BootSentry lets you easily manage programs that start automatically with Windows.",
            ["OnboardingDesc2"] = "You can disable, re-enable or delete startup entries safely thanks to the built-in backup system.",
            ["OnboardingModeQuestion"] = "Choose your mode",
            ["OnboardingModePublic"] = "Standard Mode (Recommended)",
            ["OnboardingModePublicDesc"] = "Hides Microsoft system entries and critical items. Ideal for most users.",
            ["OnboardingModeExpert"] = "Expert Mode",
            ["OnboardingModeExpertDesc"] = "Shows all entries including system services and drivers. For advanced users only.",
            ["OnboardingNext"] = "Next",
            ["OnboardingPrevious"] = "Previous",
            ["OnboardingFinish"] = "Get Started",
            ["OnboardingStart"] = "Get Started",
            ["OnboardingDontShowAgain"] = "Don't show again",
            ["OnboardingStepN"] = "Step {0} of {1}",
            ["OnboardingShortcutsTitle"] = "Keyboard Shortcuts",
            ["OnboardingShortcutsDesc"] = "Here are the essential shortcuts to use BootSentry efficiently:",
            ["ShortcutRefresh"] = "Refresh the list",
            ["ShortcutSearch"] = "Search for an entry",
            ["ShortcutDelete"] = "Delete selected entry",
            ["ShortcutUndo"] = "Undo last action",
            ["ShortcutHelp"] = "Show help",

            // Detail Panel
            ["DetailTitle"] = "Details",
            ["SelectEntryHint"] = "Select an entry to view details",
            ["DetailType"] = "Type:",
            ["DetailScope"] = "Scope:",
            ["DetailSource"] = "Source:",
            ["DetailTarget"] = "Target:",
            ["DetailCommandLine"] = "Command line:",
            ["DetailArguments"] = "Arguments",
            ["DetailWorkingDir"] = "Working Directory",
            ["DetailPublisher"] = "Publisher:",
            ["DetailSignature"] = "Signature:",
            ["DetailStatus"] = "Status:",
            ["DetailScanAV"] = "AV Scan:",
            ["DetailNotes"] = "Notes:",
            ["DetailFileInfo"] = "File information",
            ["DetailFileVersion"] = "Version:",
            ["DetailProductName"] = "Product:",
            ["DetailFileSize"] = "Size:",
            ["DetailLastModified"] = "Modified:",
            ["DetailHash"] = "SHA-256 Hash",
            ["DetailCalculateHash"] = "Calculate",
            ["DetailRiskAnalysis"] = "Risk Analysis",

            // Knowledge Base
            ["KnowledgeInfo"] = "Information",
            ["KnowledgeSafetyLevel"] = "Safety level:",
            ["KnowledgeFullDesc"] = "Full description",
            ["KnowledgeDisableImpact"] = "Impact if disabled",
            ["KnowledgePerfImpact"] = "Performance impact",
            ["KnowledgeRecommendation"] = "Recommendation",
            ["NoKnowledgeInfo"] = "No information available",
            ["NoKnowledgeInfoSub"] = "in the knowledge base",

            // Action Buttons
            ["BtnOpen"] = "Open",
            ["BtnCopy"] = "Copy",
            ["BtnWeb"] = "Web",
            ["BtnHash"] = "Hash",
            ["BtnScanAV"] = "AV Scan",
            ["CancelScan"] = "Cancel scan",

            // Malware Scan
            ["ScanResult"] = "Scan Result",
            ["ScanResultClean"] = "File is clean",
            ["ScanResultMalware"] = "Threat detected!",
            ["ScanResultBlocked"] = "Blocked",
            ["ScanResultNotScanned"] = "Not scanned",
            ["ScanResultError"] = "Error",
            ["ScanInProgress"] = "Antivirus scan in progress...",
            ["ScanComplete"] = "Scan complete",
            ["ScanNotAvailable"] = "Antivirus scanner is not available",
            ["MalwareDetectedTitle"] = "Threat Detected",
            ["MalwareDetectedMessage"] = "The file has been identified as malicious. It is recommended to disable or remove it.",

            // Tooltips
            ["TooltipRefresh"] = "Refresh list (F5)",
            ["TooltipDisable"] = "Disable selected entry (Del)",
            ["TooltipDelete"] = "Delete permanently (Ctrl+Del)",
            ["SplitterTooltip"] = "Drag to resize panels",

            // Accessibility HelpText
            ["ActionEnableHelp"] = "Enable the selected startup entry",
            ["ActionDisableHelp"] = "Disable the selected startup entry",
            ["ActionDeleteHelp"] = "Permanently delete the selected entry (a backup will be created)",
            ["StatusFilterLabel"] = "Filter by status",
            ["StatusFilterHelp"] = "Filter entries by their status (All, Enabled, Disabled)",
            ["DataGridHelp"] = "List of startup entries. Use arrow keys to navigate.",
            ["HistoryEmptyHint"] = "Actions will be recorded here",
            ["SelectEntryHintLarge"] = "Click on an entry in the list to view its details",
            ["LongOperationHint"] = "This operation may take a few seconds...",

            // Malware Scan Status (localized)
            ["ScanResultUnknown"] = "Unknown",
            ["ScanResultCleanShort"] = "File is clean",
            ["ScanResultMalwareShort"] = "Threat detected!",
            ["ScanResultBlockedShort"] = "Blocked",
            ["ScanResultNotScannedShort"] = "Not scanned",
            ["ScanResultTooLargeShort"] = "File too large",
            ["ScanResultErrorShort"] = "Error",
            ["ScanResultNoAVShort"] = "No antivirus",

            // Safety Levels (localized)
            ["SafetyUnknown"] = "Unknown",
            ["SafetyCritical"] = "Critical - Do not disable",
            ["SafetyImportant"] = "Important",
            ["SafetySafe"] = "Can be disabled",
            ["SafetyRecommendedDisable"] = "Recommended to disable",
            ["SafetyShouldRemove"] = "Should be removed",

            // Admin Status
            ["AdminStatusAdmin"] = "Administrator",
            ["AdminStatusStandard"] = "Standard",

            // Confirmations
            ["ConfirmDisableTitle"] = "Confirm Disable",
            ["ConfirmDisableMessage"] = "Do you want to disable '{0}'?",
            ["ConfirmDeleteTitle"] = "Confirm Deletion",
            ["ConfirmDeleteMessage"] = "Are you sure you want to delete this entry?",
            ["ConfirmDeleteMultiple"] = "Are you sure you want to delete {0} entries?\n\nA backup will be created for each entry.",
            ["ConfirmPurgeTitle"] = "Confirm Purge",
            ["ConfirmPurgeMessage"] = "This will permanently delete all data (logs, backups, preferences).\n\nThis action is irreversible. Continue?",
            ["ConfirmPurgeButton"] = "Purge All",
            ["ConfirmResetTitle"] = "Confirm Reset",
            ["ConfirmResetMessage"] = "Do you want to reset all settings to default values?",
            ["ConfirmResetButton"] = "Reset",
            ["Delete"] = "Delete",

            // Notifications/Feedback
            ["NotifDisabled"] = "'{0}' has been disabled",
            ["NotifEnabled"] = "'{0}' has been enabled",
            ["NotifDeleted"] = "'{0}' has been deleted (backup created)",
            ["NotifCopied"] = "Copied to clipboard",
            ["NotifExportSuccess"] = "Export successful: {0}",

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
