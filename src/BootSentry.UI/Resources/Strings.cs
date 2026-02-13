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

            // Tabs (simplified navigation)
            ["TabApplications"] = "Mes Applications",
            ["TabBrowsers"] = "Navigateurs",
            ["TabSystem"] = "Système & Pilotes",
            ["TabAdvanced"] = "Sécurité Avancée",

            // Legacy tabs (kept for compatibility)
            ["TabStartup"] = "Démarrage",
            ["TabTasks"] = "Tâches",
            ["TabServices"] = "Services",
            ["TabExtensions"] = "Extensions",

            // Empty state
            ["NoResultsFound"] = "Aucun résultat trouvé",
            ["NoResultsHint"] = "Modifiez vos critères de recherche ou de filtrage",
            ["ResetFilters"] = "Réinitialiser les filtres",
            ["EmptyStateAllClean"] = "Tout est propre !",
            ["EmptyStateAllCleanHint"] = "Aucune entrée de démarrage ne correspond à vos filtres actuels.",
            ["EmptyStateMicrosoftHidden"] = "(Les services Microsoft sont masqués)",

            // Winlogon Repair action
            ["ActionRepair"] = "Réparer",
            ["TooltipRepairWinlogon"] = "Réinitialiser à la valeur par défaut de Windows",

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
            ["StatusExportError"] = "Erreur d'export: {0}",
            ["StatusScanCancelled"] = "Analyse annulée",
            ["StatusError"] = "Erreur: {0}",
            ["StatusSmartScanThreats"] = "{0} menace(s) détectée(s) lors du scan automatique",
            ["StatusCancelling"] = "Annulation en cours...",
            ["StatusCheckingUpdates"] = "Vérification des mises à jour...",
            ["StatusDiagnosticsCreating"] = "Création du fichier de diagnostics...",
            ["StatusDiagnosticsExported"] = "Diagnostics exportés: {0}",
            ["StatusRegeditOpened"] = "Regedit ouvert",
            ["StatusServicesOpened"] = "Services.msc ouvert",
            ["StatusTaskSchedulerOpened"] = "Planificateur de tâches ouvert",
            ["StatusHashCalculating"] = "Calcul du hash SHA-256...",
            ["StatusHashCalculated"] = "Hash calculé et copié: {0}...",
            ["StatusBatchEnabling"] = "Activation de {0} entrées...",
            ["StatusBatchEnabled"] = "{0} entrée(s) activée(s)",
            ["StatusBatchEnabledWithFailures"] = "{0} activée(s), {1} échec(s)",
            ["StatusBatchDisabling"] = "Désactivation de {0} entrées...",
            ["StatusBatchDisabled"] = "{0} entrée(s) désactivée(s)",
            ["StatusBatchDisabledWithFailures"] = "{0} désactivée(s), {1} échec(s)",
            ["SelectAllHint"] = "Sélectionner tout via Ctrl+A",

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
            ["ErrorTechnicalDetails"] = "Détails techniques",
            ["ErrorRetry"] = "Réessayer",
            ["ErrorCopy"] = "Copier l'erreur",
            ["ErrorOpenLocation"] = "Erreur lors de l'ouverture: {0}",
            ["ErrorExportWithDetails"] = "Erreur lors de l'export: {0}",
            ["ErrorRegistryAccess"] = "Impossible d'accéder au registre Windows.",
            ["ErrorRegistrySuggestion"] = "Lancez l'application en tant qu'administrateur pour accéder au registre système.",
            ["ErrorFileAccess"] = "Impossible d'accéder au fichier:\n{0}",
            ["ErrorFileSuggestion"] = "Le fichier est peut-être verrouillé par un autre programme ou vous n'avez pas les permissions nécessaires.",
            ["ErrorNetworkMessage"] = "Erreur de connexion réseau.",
            ["ErrorNetworkSuggestion"] = "Vérifiez votre connexion Internet et réessayez.",
            ["ErrorScanMessage"] = "Erreur lors du scan antivirus.",
            ["ErrorScanSuggestion"] = "Assurez-vous que Windows Defender ou un autre antivirus compatible AMSI est actif.",
            ["ErrorOpeningRegedit"] = "Erreur lors de l'ouverture de Regedit: {0}",
            ["ErrorHashFileMissing"] = "Impossible de calculer le hash: fichier introuvable.",
            ["ErrorHashCalculation"] = "Erreur lors du calcul du hash: {0}",
            ["ErrorHashCalculationWithDetails"] = "Erreur lors du calcul du hash:\n{0}",
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
            ["ThemePreviewAccent"] = "Accent",
            ["ThemePreviewSuccess"] = "Succès",
            ["ThemePreviewWarning"] = "Avertissement",
            ["ThemePreviewError"] = "Erreur",
            ["ThemePreviewNeutral"] = "Neutre",
            ["ThemePreviewPrimaryButton"] = "Primaire",
            ["ThemePreviewSecondaryButton"] = "Secondaire",
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
            ["SettingsPurgeDone"] = "Données purgées",
            ["SettingsLanguageRestartPrompt"] = "La langue sera appliquée après le redémarrage.\n\nRedémarrer l'application maintenant ?",
            ["SettingsLanguageChangeTitle"] = "Changement de langue",
            ["SettingsResetDone"] = "Paramètres réinitialisés",

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
            ["OnboardingStep1of3"] = "Étape 1 sur 3",
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
            ["ScanResultTooLarge"] = "Fichier trop volumineux pour le scan (max 250 Mo)",
            ["ScanResultNoAntivirusProvider"] = "Aucun antivirus disponible",
            ["ScanResultError"] = "Erreur",
            ["ScanInProgress"] = "Scan antivirus en cours...",
            ["ScanComplete"] = "Scan terminé",
            ["ScanNotAvailable"] = "Le scanner antivirus n'est pas disponible",
            ["MalwareDetectedTitle"] = "Menace détectée",
            ["MalwareDetectedMessage"] = "Le fichier a été identifié comme malveillant. Il est recommandé de le désactiver ou supprimer.",
            ["MalwareDetectedDetailedMessage"] = "MENACE DÉTECTÉE!\n\nLe fichier suivant a été identifié comme malveillant:\n{0}\n\nIl est fortement recommandé de désactiver ou supprimer cette entrée.",
            ["ScanNoAntivirusProviderTitle"] = "Antivirus non disponible",
            ["ScanNoAntivirusProviderMessage"] = "Aucun antivirus compatible n'est disponible pour le scan.\n\nVeuillez activer Windows Defender ou installer un antivirus compatible AMSI.",
            ["ErrorScanStatus"] = "Erreur lors du scan: {0}",
            ["ErrorScanMessageWithDetails"] = "Erreur lors du scan antivirus:\n{0}",
            ["ErrorScanFileMissing"] = "Impossible de scanner: fichier introuvable.",
            ["ScanGlobalTitle"] = "Scan global",
            ["ScanGlobalNoFiles"] = "Aucun fichier à scanner.",
            ["ScanGlobalConfirm"] = "Lancer le scan antivirus sur {0} fichiers ?\n\nCette opération peut prendre plusieurs minutes.",
            ["ScanProgressFormat"] = "Scan {0}/{1}: {2}",
            ["ScanGlobalInterruptedHeader"] = "Scan interrompu.\n\n",
            ["ScanGlobalCompletedHeader"] = "Scan terminé.\n\n",
            ["ScanGlobalScannedCount"] = "Fichiers scannés: {0}\n",
            ["ScanGlobalCleanCount"] = "Fichiers sains: {0}\n",
            ["ScanGlobalThreatsCount"] = "MENACES DÉTECTÉES: {0}\n",
            ["ScanGlobalErrorsCount"] = "Erreurs/Non scannés: {0}",
            ["ScanGlobalStatusThreats"] = "Scan terminé: {0} menace(s) détectée(s)!",
            ["ScanGlobalStatusClean"] = "Scan terminé: {0} fichiers sains",
            ["ScanGlobalThreatsTitle"] = "Menaces détectées!",
            ["ScanGlobalCancelling"] = "Annulation du scan...",

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
            ["StatusFilterHelp"] = "Filtrer les entrées par leur statut (Tous, Activé, Désactivé, Suspect)",
            ["TypeFilterLabel"] = "Filtre par type",
            ["TypeFilterHelp"] = "Filtrer les entrées par type (Tous, Registre, Dossier, Tâches, Services, Pilotes, Expert)",
            ["StatusMessageAutomationName"] = "Message de statut",
            ["FilterAll"] = "Tous",
            ["FilterTypeRegistry"] = "Registre",
            ["FilterTypeStartupFolder"] = "Dossier Démarrage",
            ["FilterTypeTasks"] = "Tâches",
            ["FilterTypeServices"] = "Services",
            ["FilterTypeDrivers"] = "Pilotes",
            ["FilterTypeExpert"] = "Expert",
            ["FilterStatusEnabled"] = "Actives",
            ["FilterStatusDisabled"] = "Désactivées",
            ["FilterStatusSuspicious"] = "Suspectes",
            ["HideMicrosoftEntries"] = "Masquer Microsoft",
            ["HideMicrosoftEntriesTooltip"] = "Masquer les services et pilotes signés par Microsoft",
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
            ["ConfirmDeleteMultiple"] = "Voulez-vous vraiment supprimer {0} entrées?\n\nUn backup sera créé pour chaque entrée.",
            ["ConfirmPurgeTitle"] = "Confirmer la purge",
            ["ConfirmPurgeMessage"] = "Cette action supprimera définitivement toutes les données (logs, backups, préférences).\n\nCette action est irréversible. Continuer?",
            ["ConfirmPurgeButton"] = "Purger tout",
            ["ConfirmResetTitle"] = "Confirmer la réinitialisation",
            ["ConfirmResetMessage"] = "Voulez-vous réinitialiser tous les paramètres aux valeurs par défaut?",
            ["ConfirmResetButton"] = "Réinitialiser",
            ["ConfirmEnableBatchTitle"] = "Confirmation",
            ["ConfirmEnableBatchMessage"] = "Voulez-vous activer {0} entrée(s)?",
            ["ConfirmDisableBatchTitle"] = "Confirmation",
            ["ConfirmDisableBatchMessage"] = "Voulez-vous désactiver {0} entrée(s)?",
            ["NoDisabledEntrySelected"] = "Aucune entrée désactivée sélectionnée.",
            ["NoEnabledEntrySelected"] = "Aucune entrée active non-protégée sélectionnée.",
            ["BatchResultTitle"] = "Résultat",
            ["BatchEnableResultMessage"] = "{0} entrée(s) activée(s) avec succès.\n{1} échec(s).",
            ["BatchDisableResultMessage"] = "{0} entrée(s) désactivée(s) avec succès.\n{1} échec(s).",
            ["UndoNoActionMessage"] = "Aucune action à annuler.",
            ["UndoNoActionTitle"] = "Historique vide",
            ["UndoConfirmTitle"] = "Confirmation d'annulation",
            ["UndoConfirmMessage"] = "Voulez-vous annuler l'action '{0}' sur '{1}'?",
            ["UndoInProgress"] = "Annulation de {0} sur {1}...",
            ["UndoSuccess"] = "Action annulée: {0}",
            ["UndoErrorTitle"] = "Erreur d'annulation",
            ["UndoErrorMessage"] = "Erreur lors de l'annulation: {0}",
            ["HashCalculatedTitle"] = "Hash calculé",
            ["HashCalculatedMessage"] = "SHA-256:\n{0}\n\nLe hash a été copié dans le presse-papiers.",
            ["DiagnosticsCreatedTitle"] = "Export terminé",
            ["DiagnosticsCreatedOpenPrompt"] = "Le fichier de diagnostics a été créé:\n{0}\n\nVoulez-vous ouvrir le dossier contenant le fichier?",
            ["Delete"] = "Supprimer",

            // Notifications/Feedback
            ["NotifDisabled"] = "'{0}' a été désactivé",
            ["NotifEnabled"] = "'{0}' a été activé",
            ["NotifDeleted"] = "'{0}' a été supprimé (backup créé)",
            ["NotifCopied"] = "Copié dans le presse-papiers",
            ["NotifExportSuccess"] = "Export réussi: {0}",
            ["NotifBrowserRestartRequired"] = "Redémarrez le navigateur pour appliquer les changements",

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
            ["UnknownError"] = "Erreur inconnue",
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

            // Tabs (simplified navigation)
            ["TabApplications"] = "My Applications",
            ["TabBrowsers"] = "Web Browsers",
            ["TabSystem"] = "System & Drivers",
            ["TabAdvanced"] = "Advanced Security",

            // Legacy tabs (kept for compatibility)
            ["TabStartup"] = "Startup",
            ["TabTasks"] = "Tasks",
            ["TabServices"] = "Services",
            ["TabExtensions"] = "Extensions",

            // Empty state
            ["NoResultsFound"] = "No results found",
            ["NoResultsHint"] = "Modify your search or filter criteria",
            ["ResetFilters"] = "Reset filters",
            ["EmptyStateAllClean"] = "All clean!",
            ["EmptyStateAllCleanHint"] = "No startup entries match your current filters.",
            ["EmptyStateMicrosoftHidden"] = "(Microsoft services are hidden)",

            // Winlogon Repair action
            ["ActionRepair"] = "Repair",
            ["TooltipRepairWinlogon"] = "Reset to Windows default value",

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
            ["StatusExportError"] = "Export error: {0}",
            ["StatusScanCancelled"] = "Scan cancelled",
            ["StatusError"] = "Error: {0}",
            ["StatusSmartScanThreats"] = "{0} threat(s) detected during automatic scan",
            ["StatusCancelling"] = "Cancelling...",
            ["StatusCheckingUpdates"] = "Checking for updates...",
            ["StatusDiagnosticsCreating"] = "Creating diagnostics file...",
            ["StatusDiagnosticsExported"] = "Diagnostics exported: {0}",
            ["StatusRegeditOpened"] = "Regedit opened",
            ["StatusServicesOpened"] = "Services.msc opened",
            ["StatusTaskSchedulerOpened"] = "Task Scheduler opened",
            ["StatusHashCalculating"] = "Calculating SHA-256 hash...",
            ["StatusHashCalculated"] = "Hash calculated and copied: {0}...",
            ["StatusBatchEnabling"] = "Enabling {0} entries...",
            ["StatusBatchEnabled"] = "{0} entries enabled",
            ["StatusBatchEnabledWithFailures"] = "{0} enabled, {1} failed",
            ["StatusBatchDisabling"] = "Disabling {0} entries...",
            ["StatusBatchDisabled"] = "{0} entries disabled",
            ["StatusBatchDisabledWithFailures"] = "{0} disabled, {1} failed",
            ["SelectAllHint"] = "Select all with Ctrl+A",

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
            ["ErrorTechnicalDetails"] = "Technical details",
            ["ErrorRetry"] = "Retry",
            ["ErrorCopy"] = "Copy error",
            ["ErrorOpenLocation"] = "Error opening location: {0}",
            ["ErrorExportWithDetails"] = "Error during export: {0}",
            ["ErrorRegistryAccess"] = "Unable to access Windows Registry.",
            ["ErrorRegistrySuggestion"] = "Run the application as administrator to access system registry keys.",
            ["ErrorFileAccess"] = "Unable to access file:\n{0}",
            ["ErrorFileSuggestion"] = "The file may be locked by another program or you may not have the required permissions.",
            ["ErrorNetworkMessage"] = "Network connection error.",
            ["ErrorNetworkSuggestion"] = "Check your internet connection and try again.",
            ["ErrorScanMessage"] = "Error during antivirus scan.",
            ["ErrorScanSuggestion"] = "Make sure Windows Defender or another AMSI-compatible antivirus is active.",
            ["ErrorOpeningRegedit"] = "Error opening Regedit: {0}",
            ["ErrorHashFileMissing"] = "Unable to calculate hash: file not found.",
            ["ErrorHashCalculation"] = "Error calculating hash: {0}",
            ["ErrorHashCalculationWithDetails"] = "Error calculating hash:\n{0}",
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
            ["ThemePreviewAccent"] = "Accent",
            ["ThemePreviewSuccess"] = "Success",
            ["ThemePreviewWarning"] = "Warning",
            ["ThemePreviewError"] = "Error",
            ["ThemePreviewNeutral"] = "Neutral",
            ["ThemePreviewPrimaryButton"] = "Primary",
            ["ThemePreviewSecondaryButton"] = "Secondary",
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
            ["SettingsPurgeDone"] = "Data purged",
            ["SettingsLanguageRestartPrompt"] = "The language will be applied after restart.\n\nRestart the application now?",
            ["SettingsLanguageChangeTitle"] = "Language Change",
            ["SettingsResetDone"] = "Settings reset to defaults",

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
            ["OnboardingStep1of3"] = "Step 1 of 3",
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
            ["ScanResultTooLarge"] = "File too large to scan (max 250 MB)",
            ["ScanResultNoAntivirusProvider"] = "No antivirus available",
            ["ScanResultError"] = "Error",
            ["ScanInProgress"] = "Antivirus scan in progress...",
            ["ScanComplete"] = "Scan complete",
            ["ScanNotAvailable"] = "Antivirus scanner is not available",
            ["MalwareDetectedTitle"] = "Threat Detected",
            ["MalwareDetectedMessage"] = "The file has been identified as malicious. It is recommended to disable or remove it.",
            ["MalwareDetectedDetailedMessage"] = "THREAT DETECTED!\n\nThe following file has been identified as malicious:\n{0}\n\nIt is strongly recommended to disable or remove this entry.",
            ["ScanNoAntivirusProviderTitle"] = "Antivirus unavailable",
            ["ScanNoAntivirusProviderMessage"] = "No compatible antivirus is available for scanning.\n\nPlease enable Windows Defender or install an AMSI-compatible antivirus.",
            ["ErrorScanStatus"] = "Scan error: {0}",
            ["ErrorScanMessageWithDetails"] = "Error during antivirus scan:\n{0}",
            ["ErrorScanFileMissing"] = "Unable to scan: file not found.",
            ["ScanGlobalTitle"] = "Global scan",
            ["ScanGlobalNoFiles"] = "No files to scan.",
            ["ScanGlobalConfirm"] = "Start antivirus scan on {0} files?\n\nThis operation may take several minutes.",
            ["ScanProgressFormat"] = "Scan {0}/{1}: {2}",
            ["ScanGlobalInterruptedHeader"] = "Scan interrupted.\n\n",
            ["ScanGlobalCompletedHeader"] = "Scan completed.\n\n",
            ["ScanGlobalScannedCount"] = "Scanned files: {0}\n",
            ["ScanGlobalCleanCount"] = "Clean files: {0}\n",
            ["ScanGlobalThreatsCount"] = "THREATS DETECTED: {0}\n",
            ["ScanGlobalErrorsCount"] = "Errors/Not scanned: {0}",
            ["ScanGlobalStatusThreats"] = "Scan complete: {0} threat(s) detected!",
            ["ScanGlobalStatusClean"] = "Scan complete: {0} clean files",
            ["ScanGlobalThreatsTitle"] = "Threats detected!",
            ["ScanGlobalCancelling"] = "Cancelling scan...",

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
            ["StatusFilterHelp"] = "Filter entries by their status (All, Enabled, Disabled, Suspicious)",
            ["TypeFilterLabel"] = "Filter by type",
            ["TypeFilterHelp"] = "Filter entries by type (All, Registry, Folder, Tasks, Services, Drivers, Expert)",
            ["StatusMessageAutomationName"] = "Status message",
            ["FilterAll"] = "All",
            ["FilterTypeRegistry"] = "Registry",
            ["FilterTypeStartupFolder"] = "Startup Folder",
            ["FilterTypeTasks"] = "Tasks",
            ["FilterTypeServices"] = "Services",
            ["FilterTypeDrivers"] = "Drivers",
            ["FilterTypeExpert"] = "Expert",
            ["FilterStatusEnabled"] = "Enabled",
            ["FilterStatusDisabled"] = "Disabled",
            ["FilterStatusSuspicious"] = "Suspicious",
            ["HideMicrosoftEntries"] = "Hide Microsoft",
            ["HideMicrosoftEntriesTooltip"] = "Hide services and drivers signed by Microsoft",
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
            ["ConfirmDeleteMultiple"] = "Are you sure you want to delete {0} entries?\n\nA backup will be created for each entry.",
            ["ConfirmPurgeTitle"] = "Confirm Purge",
            ["ConfirmPurgeMessage"] = "This will permanently delete all data (logs, backups, preferences).\n\nThis action is irreversible. Continue?",
            ["ConfirmPurgeButton"] = "Purge All",
            ["ConfirmResetTitle"] = "Confirm Reset",
            ["ConfirmResetMessage"] = "Do you want to reset all settings to default values?",
            ["ConfirmResetButton"] = "Reset",
            ["ConfirmEnableBatchTitle"] = "Confirmation",
            ["ConfirmEnableBatchMessage"] = "Do you want to enable {0} entries?",
            ["ConfirmDisableBatchTitle"] = "Confirmation",
            ["ConfirmDisableBatchMessage"] = "Do you want to disable {0} entries?",
            ["NoDisabledEntrySelected"] = "No disabled entries selected.",
            ["NoEnabledEntrySelected"] = "No enabled non-protected entries selected.",
            ["BatchResultTitle"] = "Result",
            ["BatchEnableResultMessage"] = "{0} entries enabled successfully.\n{1} failed.",
            ["BatchDisableResultMessage"] = "{0} entries disabled successfully.\n{1} failed.",
            ["UndoNoActionMessage"] = "No action to undo.",
            ["UndoNoActionTitle"] = "History empty",
            ["UndoConfirmTitle"] = "Undo confirmation",
            ["UndoConfirmMessage"] = "Do you want to undo '{0}' on '{1}'?",
            ["UndoInProgress"] = "Undoing {0} on {1}...",
            ["UndoSuccess"] = "Action undone: {0}",
            ["UndoErrorTitle"] = "Undo error",
            ["UndoErrorMessage"] = "Error while undoing: {0}",
            ["HashCalculatedTitle"] = "Hash calculated",
            ["HashCalculatedMessage"] = "SHA-256:\n{0}\n\nThe hash has been copied to clipboard.",
            ["DiagnosticsCreatedTitle"] = "Export completed",
            ["DiagnosticsCreatedOpenPrompt"] = "Diagnostics file created:\n{0}\n\nDo you want to open its folder?",
            ["Delete"] = "Delete",

            // Notifications/Feedback
            ["NotifDisabled"] = "'{0}' has been disabled",
            ["NotifEnabled"] = "'{0}' has been enabled",
            ["NotifDeleted"] = "'{0}' has been deleted (backup created)",
            ["NotifCopied"] = "Copied to clipboard",
            ["NotifExportSuccess"] = "Export successful: {0}",
            ["NotifBrowserRestartRequired"] = "Restart browser to apply changes",

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
            ["UnknownError"] = "Unknown error",
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
