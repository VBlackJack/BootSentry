Cahier des Charges – Projet BootSentry

Nom du projet : BootSentry
Auteur : Julien Bombled
Licence : Apache License 2.0
Date : 13 décembre 2025
Version : 2.1 (complété)

0. Objet du document

Ce cahier des charges définit le périmètre fonctionnel et technique de BootSentry, ainsi que les exigences non-fonctionnelles, la sécurité, les critères d’acceptation, la stratégie de tests, la distribution, et la roadmap.

Public cible :

Utilisateurs Windows (grand public et avancés)

Administrateurs systèmes / support IT

Contributeurs Open Source

1. Contexte, vision et objectifs

1.1 Contexte

Windows expose plusieurs mécanismes de lancement automatique (registre, dossiers de démarrage, tâches planifiées, services, drivers, etc.). Les outils existants sont soit :

trop simplistes (Gestionnaire des tâches), soit

trop denses/arides (Sysinternals Autoruns).

1.2 Vision

BootSentry se positionne comme un outil moderne, sûr et pédagogique pour :

inventorier toutes les sources de démarrage pertinentes,

guider l’utilisateur (mode grand public),

permettre des actions avancées (mode expert),

garantir un rollback fiable avant toute action destructive.

1.3 Objectifs (priorisés)

Inventaire fiable des entrées de démarrage (multi-sources)

Action sûre (désactivation réversible + suppression encadrée)

Rollback transactionnel (restauration un clic)

Compréhension (explications, heuristiques, détails)

Performance (scan rapide, UI non bloquante)

Portabilité (single-file publish + MSI)

1.4 Principes

Ne jamais exécuter les binaires cibles.

Par défaut : ne pas casser Windows (garde-fous).

Transparence : chaque action doit expliquer ce qu’elle fait réellement.

Réversibilité : priorité au disable plutôt qu’au delete.

Respect OS : utiliser les APIs Windows quand elles existent (Task Scheduler, SCM, WinVerifyTrust, etc.).

1.5 Hors périmètre (v2.0)

Nettoyage malware certifié (BootSentry n’est pas un antivirus).

Modification profonde du boot (BCD, kernel, drivers critiques) hors “Expert + avertissement”.

Télémetrie obligatoire (opt-in seulement).

2. Personas et cas d’usage

2.1 Personas

P1 – Grand Public : veut désactiver “des trucs” qui ralentissent son PC, sans risque.

P2 – Power User : veut comprendre, trier, vérifier signatures, analyser.

P3 – Admin IT : veut auditer des postes, exporter des rapports, standardiser.

2.2 Parcours clés

Audit rapide → désactiver 3 entrées → redémarrer → rollback si souci.

Recherche d’un élément suspect → vérifier signature/hachage → isoler/désactiver.

Export inventaire pour ticket support.

Mode expert → analyser tâches planifiées et services non-Microsoft.

3. Périmètre fonctionnel

3.1 Sources analysées (Scanning)

3.1.1 Registre – Entrées “Run” (MVP)

HKCU\Software\Microsoft\Windows\CurrentVersion\Run

HKCU\Software\Microsoft\Windows\CurrentVersion\RunOnce

HKLM\Software\Microsoft\Windows\CurrentVersion\Run

HKLM\Software\Microsoft\Windows\CurrentVersion\RunOnce

Compatibilité 32/64-bit :

HKLM\Software\Wow6432Node\Microsoft\Windows\CurrentVersion\Run

HKLM\Software\Wow6432Node\Microsoft\Windows\CurrentVersion\RunOnce

3.1.2 Registre – Policies / variantes (recommandé v1.1+)

HKLM\Software\Microsoft\Windows\CurrentVersion\Policies\Explorer\Run

HKCU\Software\Microsoft\Windows\CurrentVersion\Policies\Explorer\Run

3.1.3 Registre – Zones sensibles (Expert)

Winlogon (Shell, Userinit…)

Session Manager (BootExecute)

(Option) AppInit_DLLs, KnownDLLs, providers d’auth, LSA (phase ultérieure)

3.1.4 Dossiers de démarrage

Démarrage utilisateur (roaming)

Démarrage commun (ProgramData)

Gestion .lnk : résolution de :

cible réelle

arguments

répertoire de travail

3.1.5 Tâches planifiées

Déclencheurs :

At startup

At logon

At workstation unlock (option)

On idle (option)

On event (option)

Tâches :

par machine et par utilisateur

hidden vs visible

3.1.6 Services Windows (SCM)

Services non-Microsoft (filtre)

StartupType :

Automatic

Automatic (Delayed)

Manual

Disabled

Exclure/Protéger : services critiques Microsoft (liste blanche)

3.1.7 Drivers (Expert)

Drivers boot/system/auto

Par défaut : lecture + avertissements (pas de delete en public)

3.1.8 Sources supplémentaires (Phase 2+)

Image File Execution Options (IFEO) – PRIORITAIRE

HKLM\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Image File Execution Options\

Détection des clés "Debugger" (vecteur malware courant)

Alerte si debugger pointe vers un exécutable non-Microsoft

Shell Extensions (Expert)

HKLM\SOFTWARE\Classes\*\ShellEx\ContextMenuHandlers

HKLM\SOFTWARE\Classes\Directory\ShellEx\ContextMenuHandlers

HKLM\SOFTWARE\Classes\Folder\ShellEx\ContextMenuHandlers

Browser Helper Objects – BHO (Legacy, Expert)

HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\Browser Helper Objects

HKLM\SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Explorer\Browser Helper Objects

Print Monitors (Expert)

HKLM\SYSTEM\CurrentControlSet\Control\Print\Monitors

Winsock Providers – LSP (Legacy, Expert)

Enumération via WSCEnumProtocols API

3.2 Modèle de donnée – "StartupEntry"

Chaque entrée affichée DOIT être normalisée via un modèle commun.

3.2.1 Champs obligatoires

Id (stable, déterministe)

Type : RegistryRun | RegistryRunOnce | StartupFolder | ScheduledTask | Service | Driver | (Future)

Scope : User | Machine

DisplayName

SourcePath (ex: chemin clé registre + nom valeur / chemin dossier / nom tâche / nom service)

CommandLineRaw

CommandLineNormalized (parsing guillemets, env vars)

TargetPath (si résolvable)

Arguments (si résolvables)

WorkingDirectory (si applicable)

Publisher (si détectable)

SignatureStatus : SignedTrusted | SignedUntrusted | Unsigned | Unknown

Sha256 (optionnel, calculable)

FileExists (bool)

LastModified (si disponible)

Status : Enabled | Disabled | Unknown

RiskLabel : Safe | Unknown | Suspicious | Disabled (fonctionnel) | Critical (garde-fou)

Notes (explication courte)

3.2.2 Champs optionnels

FileVersion, ProductName, CompanyName

InstallLocation (heuristique)

RegistryView : 32/64

TaskXmlHash (tâches)

ServiceImagePath, ServiceStartType, ServiceAccount

3.3 Moteur d’analyse – Normalisation et enrichissement

3.3.1 Résolution CommandLine

Parser fiable (guillemets, espaces, variables env)

Normaliser :

expansion %ENV% (optionnel affichage double: brut + résolu)

chemin canonicalisé (full path si possible)

3.3.2 Récupération métadonnées fichier

Icône (si accessible)

Version info (FileVersionInfo)

Taille

Date modification

3.3.3 Signature

Authenticode via WinVerifyTrust

Distinguer :

signé et chaîne de confiance OK

signé mais non trusté

non signé

inconnu (fichier inaccessible)

3.3.4 Hash

SHA-256

Calcul :

à la demande (bouton)

ou en tâche de fond configurable

4. Actions utilisateur

4.1 Règles générales

Toute action destructive nécessite :

confirmation

backup préalable

journalisation

L’UI doit afficher clairement “ce qui va être modifié” (dry-run).

4.2 Désactiver / Réactiver (Disable / Enable)

4.2.1 Registre

Stratégie A (recommandée) : déplacer vers une clé BootSentry :

HKCU\Software\BootSentry\Disabled\...

HKLM\Software\BootSentry\Disabled\...

Conserver : valeur originale, type, timestamp action

Réactiver = restaurer vers la clé source.

4.2.2 Startup folder

Déplacer le fichier / raccourci vers : %ProgramData%\BootSentry\Quarantine\StartupFolder\...

Conserver chemin original pour restore.

4.2.3 Scheduled task

Utiliser API Task Scheduler :

Disable = Enabled = false

Enable = Enabled = true

4.2.4 Services

Par défaut (Public) : disable uniquement si identifié “tiers non critique”

Disable = StartType → Disabled

Enable = restaurer l’état précédent

4.2.5 Drivers

Expert seulement

Disable = selon type (avec avertissement majeur)

Par défaut : aucune action en public.

4.3 Supprimer (Delete)

4.3.1 Registre

Backup granularisé

Suppression de la valeur

4.3.2 Startup folder

Backup (copie)

Suppression du fichier

4.3.3 Scheduled task

Export XML

Delete via API

4.3.4 Services/Drivers

Public : interdit

Expert : optionnel, uniquement si restauration possible et avertissement explicite

4.4 Actions non destructives

Ouvrir emplacement fichier

Ouvrir source :

Regedit (positionné si possible)

Task Scheduler (ouvrir console)

Services.msc

Copier : commande, chemin, arguments, hash

Recherche web : requête sur DisplayName + Publisher (sans envoyer la commande complète par défaut)

5. Modes d’utilisation

5.1 Mode Grand Public (défaut)

Filtrage par défaut :

masquer entrées Microsoft (signed trusted)

masquer éléments critiques (services/drivers)

Actions :

disable/enable sûr

delete limité (registre/startup folder)

UI :

explications simples

avertissements courts

5.2 Mode Expert

Afficher toutes les entrées + sources sensibles

Afficher raw commandline

Autoriser actions avancées (selon garde-fous)

Fournir outils d'analyse :

hash/signature

accès direct à la source

5.3 Modèle de privilèges et élévation UAC

5.3.1 Principes

L'application démarre SANS privilèges administrateur par défaut

Élévation à la demande selon l'action requise

Feedback clair à l'utilisateur sur pourquoi l'élévation est nécessaire

5.3.2 Matrice des privilèges

| Opération | Privilège requis | Comportement |
|-----------|------------------|--------------|
| Lecture HKCU | Standard | Direct |
| Lecture HKLM | Standard | Direct |
| Lecture Startup User | Standard | Direct |
| Lecture Startup Common | Standard | Direct |
| Lecture Services/Tasks | Standard | Direct (lecture seule) |
| Écriture HKCU | Standard | Direct |
| Écriture HKLM | Admin | Élévation requise |
| Écriture Startup Common | Admin | Élévation requise |
| Modification Services | Admin | Élévation requise |
| Modification Tasks (machine) | Admin | Élévation requise |
| Modification Drivers | Admin | Élévation requise |
| Accès backups ProgramData | Admin (écriture) | Élévation pour backup |

5.3.3 Stratégie d'élévation

Option A (recommandée) : Élévation à la demande

L'UI démarre en mode standard

Lors d'une action nécessitant admin :
  - Afficher dialogue explicatif ("Cette action nécessite des droits administrateur car...")
  - Proposer "Élever maintenant" ou "Annuler"
  - Si accepté : relancer le processus avec runas ou utiliser COM Elevation Moniker
  - Conserver l'état de l'UI après élévation

Avantages : UX fluide, principe du moindre privilège

Option B : Élévation au démarrage (alternative)

Manifest avec requestedExecutionLevel = requireAdministrator

Avantages : Simplicité

Inconvénients : UAC systématique, moins user-friendly

5.3.4 Gestion du refus d'élévation

Si l'utilisateur refuse l'élévation :

Annuler l'action en cours

Afficher message explicatif : "L'action n'a pas pu être effectuée. Droits administrateur requis."

Proposer alternatives si disponibles (ex: désactiver entrée HKCU à la place de HKLM)

Logger l'événement (niveau Info)

5.3.5 Implémentation technique

```csharp
// Vérification des privilèges
public static bool IsRunningAsAdmin()
{
    using var identity = WindowsIdentity.GetCurrent();
    var principal = new WindowsPrincipal(identity);
    return principal.IsInRole(WindowsBuiltInRole.Administrator);
}

// Élévation via nouveau processus
public static void RestartAsAdmin(string arguments = "")
{
    var startInfo = new ProcessStartInfo
    {
        FileName = Process.GetCurrentProcess().MainModule.FileName,
        Arguments = arguments,
        Verb = "runas",
        UseShellExecute = true
    };
    Process.Start(startInfo);
    Application.Current.Shutdown();
}
```

6. Gestion des erreurs et états dégradés

6.1 Principes

Aucune erreur silencieuse : toute erreur doit être loggée et visible

Dégradation gracieuse : une erreur sur une source ne bloque pas les autres

Récupération possible : proposer des actions correctives quand c'est possible

6.2 Matrice erreurs / comportements

6.2.1 Erreurs de scan

| Erreur | Comportement | Message utilisateur |
|--------|--------------|---------------------|
| Clé registre inaccessible (permissions) | Ignorer + logger Warning | Badge "Scan partiel" |
| Fichier cible inexistant | Marquer FileExists = false | Indicateur visuel "Fichier manquant" |
| Fichier cible verrouillé | Marquer comme "Accès limité" | Tooltip explicatif |
| Timeout lecture réseau (UNC path) | Timeout 5s, marquer Unknown | "Chemin réseau inaccessible" |
| Chemin trop long (> 260 chars) | Utiliser API Long Path (\\?\) | - |
| Raccourci .lnk corrompu | Marquer comme "Raccourci invalide" | Indicateur + tooltip |

6.2.2 Erreurs d'action

| Erreur | Comportement | Message utilisateur |
|--------|--------------|---------------------|
| Backup échoue (disque plein) | ANNULER l'action | "Espace disque insuffisant pour le backup. Action annulée." |
| Backup échoue (permissions) | ANNULER l'action | "Impossible de créer le backup. Vérifiez les permissions." |
| Écriture registre refusée | Annuler + logger | "Accès refusé. Élévation requise ou clé protégée." |
| Fichier verrouillé par process | Annuler + identifier le process | "Fichier verrouillé par [ProcessName]. Fermez l'application et réessayez." |
| Service en cours d'exécution | Avertir avant disable | "Ce service est actuellement en cours d'exécution. Voulez-vous continuer ?" |
| Entrée modifiée entre scan et action | Rafraîchir + redemander confirmation | "L'entrée a été modifiée. Veuillez vérifier et réessayer." |
| Antivirus bloque l'accès | Logger + avertir | "Accès bloqué (possible protection antivirus). Vérifiez les exclusions." |

6.2.3 Erreurs de restauration

| Erreur | Comportement | Message utilisateur |
|--------|--------------|---------------------|
| Backup corrompu/manquant | Signaler + proposer suppression backup | "Le backup est corrompu ou incomplet. Restauration impossible." |
| Destination déjà occupée | Proposer écrasement ou annulation | "Une entrée existe déjà à cet emplacement. Écraser ?" |
| Permissions insuffisantes | Demander élévation | "Droits administrateur requis pour la restauration." |

6.3 États dégradés de l'UI

6.3.1 Scan partiel

Si certaines sources échouent :

Afficher les résultats disponibles

Bandeau d'avertissement : "⚠️ Scan incomplet - Certaines sources n'ont pas pu être analysées"

Bouton "Voir détails" → liste des sources en échec avec raisons

Option "Réessayer" pour les sources en échec

6.3.2 Mode hors-ligne

Si pas de connexion réseau :

Recherche web : bouton grisé + tooltip "Connexion requise"

Vérification signature (CRL) : utiliser cache local, marquer "Non vérifié (hors-ligne)"

Mise à jour : ignorer silencieusement

6.4 Logging des erreurs

Toutes les erreurs sont loggées avec :

Timestamp

Niveau (Error, Warning, Info)

Code erreur interne (ex: ERR_REG_ACCESS_DENIED)

Message détaillé

Stack trace (niveau Debug)

Contexte (entrée concernée, action tentée)

7. Rollback, historique et transactions

7.1 Concepts

Toute action = transaction

Transaction contient :

snapshot avant

action

résultat

possibilité de restore

7.2 Stockage

%ProgramData%\BootSentry\Backups\

Format :

manifest.json

payload spécifique par type

7.3 Manifest (exemple conceptuel)

transactionId

timestamp

user (SID / username)

actionType (Disable/Enable/Delete/Restore)

entryId

entrySnapshotBefore

entrySnapshotAfter

payloadPaths

status

error (si échec)

7.4 Restauration

Restore "1 clic" par entrée

Restore "batch" par transaction groupée

Gestion restauration partielle si composants manquants

7.5 Rétention

Politique configurable :

max N transactions

ou max X jours

8. UI/UX

8.1 Vues

Dashboard :

total entrées actives

désactivées

inconnues/suspectes

(option) score santé du démarrage

Liste principale :

DataGrid : Nom, Type, Scope, Publisher, Signature, Status, Risk

filtres + recherche

multi-sélection

Panneau détails :

source exacte

target path

arguments

signature + hash

boutons actions

Historique/Rollback :

liste transactions

restore

export

Settings :

mode public/expert

politique hashing

rétention backups

options d'affichage

8.2 Accessibilité

8.2.1 Principes WCAG 2.1

Navigation clavier complète (Tab, Entrée, Échap, flèches)

Contrast ratio minimum 4.5:1 (texte normal), 3:1 (grand texte)

Ne pas dépendre uniquement des couleurs (icônes + texte)

Focus visible sur tous les éléments interactifs

8.2.2 Support lecteurs d'écran

AutomationProperties sur tous les contrôles :
  - AutomationProperties.Name pour les boutons sans texte
  - AutomationProperties.HelpText pour contexte supplémentaire
  - AutomationProperties.LiveSetting pour les mises à jour dynamiques

Test obligatoire avec Windows Narrator

8.2.3 Raccourcis clavier

| Raccourci | Action |
|-----------|--------|
| F5 | Rafraîchir le scan |
| Ctrl+F | Rechercher |
| Ctrl+E | Basculer mode Expert |
| Delete | Désactiver sélection |
| Ctrl+Delete | Supprimer sélection |
| Ctrl+Z | Annuler dernière action (si possible) |
| Ctrl+Shift+Z | Restaurer |
| F1 | Aide contextuelle |
| Échap | Fermer panneau/dialogue |

8.2.4 Support High Contrast

Tester avec les thèmes Windows High Contrast

Utiliser SystemColors pour les couleurs de base

Éviter les images avec texte intégré

8.2.5 Taille et scaling

Police minimum : 12pt

Support DPI awareness (Per-Monitor V2)

UI responsive jusqu'à 150% scaling minimum

8.3 Design system

WPF moderne (Fluent-like)

Dark/Light (suivre le thème Windows par défaut, option override)

États clairs : Enabled/Disabled/Unknown

Animations subtiles (respecter prefers-reduced-motion si disponible)

8.4 Première utilisation (Onboarding)

8.4.1 Premier lancement

Dialogue de bienvenue :
  - Présentation rapide (3 slides max)
  - Choix du mode : "Je suis débutant" / "Je suis expérimenté"
  - Option "Ne plus afficher"

Proposition de scan initial

8.4.2 Aide contextuelle

Tooltips informatifs sur chaque colonne/bouton

Icône "?" ouvrant l'aide pour la section courante

Liens vers documentation en ligne (si disponible)

9. Exigences non-fonctionnelles (NFR)

9.1 Compatibilité

Windows 10 (1903+) / Windows 11 x64

.NET 8 (inclus dans single-file publish)

Pas de dépendance externe requise (self-contained)

9.2 Performance

9.2.1 Objectifs mesurables

| Opération | Objectif | Conditions |
|-----------|----------|------------|
| Démarrage application | < 1.5s | Cold start, SSD |
| Scan registre seul | < 300ms | ~100 entrées |
| Scan complet (sans hash) | < 2s | ~500 entrées, SSD |
| Scan complet (avec hash) | < 10s | ~500 entrées, background |
| Affichage liste | < 100ms | 500 entrées, virtualisation |
| Action disable/enable | < 500ms | Incluant backup |

9.2.2 Hardware de référence

CPU : Intel i5 8ème gen ou équivalent

RAM : 8 Go

Stockage : SSD SATA

9.2.3 Stratégies

UI non bloquante (async/await systématique)

Virtualisation du DataGrid (VirtualizingStackPanel)

Lazy loading des métadonnées fichiers

Hash en background avec CancellationToken

9.3 Sécurité

Ne jamais exécuter les binaires cibles

Backups systématiques avant toute modification

Gardes-fous sur services/drivers

Validation des chemins (éviter path traversal)

Pas d'évaluation dynamique de code (eval, Reflection.Emit pour input utilisateur)

9.4 Confidentialité et RGPD

9.4.1 Données collectées localement

Logs techniques (chemins, noms d'entrées)

Backups (contenu des entrées désactivées)

Préférences utilisateur

9.4.2 Données transmises

Aucune par défaut

Opt-in pour :
  - Recherche web (requête sur moteur de recherche externe)
  - Vérification mise à jour (requête HTTPS version seulement)

9.4.3 Anonymisation des exports

Option "Anonymiser" lors de l'export :
  - Remplacer les chemins utilisateurs par [USER]
  - Remplacer les noms de machines par [MACHINE]
  - Conserver uniquement les informations techniques pertinentes

9.4.4 Droit à l'effacement

Bouton "Purger toutes les données" dans Settings :
  - Supprime logs
  - Supprime backups
  - Réinitialise préférences

9.5 Gestion des instances

9.5.1 Single Instance

Une seule instance de BootSentry peut s'exécuter à la fois

Implémentation via Mutex nommé global : "Global\BootSentryMutex"

Si une instance existe déjà : activer sa fenêtre et quitter

9.5.2 Implémentation

```csharp
private static Mutex _mutex;

public static bool AcquireSingleInstance()
{
    _mutex = new Mutex(true, @"Global\BootSentryMutex", out bool createdNew);
    if (!createdNew)
    {
        // Activer l'instance existante via window message
        NativeMethods.PostMessage(NativeMethods.HWND_BROADCAST,
            NativeMethods.WM_SHOWBOOTSENTRY, IntPtr.Zero, IntPtr.Zero);
        return false;
    }
    return true;
}
```

10. Architecture technique

10.1 Stack

C# (.NET 8)

WPF

MVVM strict (CommunityToolkit.Mvvm recommandé)

10.2 Modules

BootSentry.Core (models, enums, parsing, interfaces)

BootSentry.Providers (scan par source)

BootSentry.Actions (disable/delete/restore strategies)

BootSentry.Backup (transactions, stockage)

BootSentry.Security (signature/hash, WinVerifyTrust)

BootSentry.UI (WPF, ViewModels, Views)

BootSentry.Tests (xUnit, tests unitaires et intégration)

10.3 Patterns

Providers par source (IStartupProvider)

Strategies par type d'entrée (IActionStrategy)

Transaction engine central (ITransactionManager)

Dependency Injection (Microsoft.Extensions.DependencyInjection)

10.4 Dépendances NuGet recommandées

| Package | Usage |
|---------|-------|
| CommunityToolkit.Mvvm | MVVM, INotifyPropertyChanged, RelayCommand |
| Microsoft.Extensions.DependencyInjection | DI Container |
| Microsoft.Extensions.Logging | Logging abstraction |
| Serilog + Serilog.Sinks.File | Implémentation logging |
| System.Text.Json | Sérialisation JSON (natif .NET) |
| WPF-UI ou ModernWpf | UI moderne Fluent (optionnel) |

10.5 Structure des projets

```
BootSentry/
├── src/
│   ├── BootSentry.Core/
│   │   ├── Models/
│   │   │   └── StartupEntry.cs
│   │   ├── Enums/
│   │   │   ├── EntryType.cs
│   │   │   ├── EntryScope.cs
│   │   │   └── SignatureStatus.cs
│   │   ├── Interfaces/
│   │   │   ├── IStartupProvider.cs
│   │   │   ├── IActionStrategy.cs
│   │   │   └── ITransactionManager.cs
│   │   └── Parsing/
│   │       └── CommandLineParser.cs
│   ├── BootSentry.Providers/
│   │   ├── RegistryRunProvider.cs
│   │   ├── StartupFolderProvider.cs
│   │   ├── ScheduledTaskProvider.cs
│   │   ├── ServiceProvider.cs
│   │   └── DriverProvider.cs
│   ├── BootSentry.Actions/
│   │   ├── Strategies/
│   │   │   ├── RegistryActionStrategy.cs
│   │   │   ├── StartupFolderActionStrategy.cs
│   │   │   └── ...
│   │   └── ActionExecutor.cs
│   ├── BootSentry.Backup/
│   │   ├── TransactionManager.cs
│   │   ├── BackupStorage.cs
│   │   └── Models/
│   │       └── TransactionManifest.cs
│   ├── BootSentry.Security/
│   │   ├── SignatureVerifier.cs
│   │   ├── HashCalculator.cs
│   │   └── NativeMethods.cs
│   └── BootSentry.UI/
│       ├── App.xaml
│       ├── ViewModels/
│       │   ├── MainViewModel.cs
│       │   ├── EntryListViewModel.cs
│       │   └── SettingsViewModel.cs
│       ├── Views/
│       │   ├── MainWindow.xaml
│       │   ├── EntryListView.xaml
│       │   └── SettingsView.xaml
│       └── Converters/
│           └── ...
├── tests/
│   ├── BootSentry.Core.Tests/
│   ├── BootSentry.Providers.Tests/
│   └── BootSentry.Actions.Tests/
└── BootSentry.sln
```

11. Internationalisation (i18n)

11.1 Stratégie

Architecture i18n dès le départ (difficile à ajouter après)

Langue par défaut : Français

Langues cibles (v1.x) : Français, Anglais

11.2 Implémentation

Fichiers de ressources .resx par assembly UI :
  - Resources.resx (français, défaut)
  - Resources.en.resx (anglais)

Binding XAML via x:Static ou markup extension

Détection automatique de la culture système

Option de changement manuel dans Settings

11.3 Éléments à traduire

Tous les textes UI (labels, boutons, menus)

Messages d'erreur

Tooltips et aide contextuelle

Noms des colonnes

États et statuts

11.4 Éléments NON traduits

Noms techniques (clés registre, chemins)

Logs techniques (anglais uniquement pour support)

Noms de fichiers/dossiers système

12. Mise à jour automatique

12.1 Stratégie

Vérification manuelle ou automatique (opt-in)

Notification seulement (pas d'auto-update silencieux)

Téléchargement via navigateur (lien GitHub Releases)

12.2 Implémentation

12.2.1 Vérification

Appel HTTPS vers GitHub API : GET /repos/{owner}/BootSentry/releases/latest

Comparer version courante vs latest release tag

Fréquence : au démarrage (si opt-in) ou manuel

12.2.2 Notification

Si nouvelle version disponible :
  - Bandeau discret en haut de l'UI
  - "Nouvelle version X.Y disponible - Télécharger"
  - Lien vers page de release (changelog visible)

Option "Ignorer cette version"

Option "Me rappeler plus tard"

12.2.3 Respect vie privée

Aucune donnée utilisateur transmise

User-Agent générique : "BootSentry/X.Y UpdateCheck"

Pas de tracking d'installation

13. Système de réputation et heuristiques

13.1 Stratégie de scoring

Pas de base de données externe par défaut

Heuristiques locales basées sur :
  - Signature Authenticode
  - Publisher connu
  - Emplacement du fichier
  - Caractéristiques suspectes

13.2 Règles de scoring (RiskLabel)

13.2.1 Safe (vert)

Signé par Microsoft ou éditeur de confiance connu

Situé dans Program Files ou Windows

Publisher dans liste blanche intégrée

13.2.2 Unknown (gris)

Non signé mais emplacement standard

Signé mais éditeur non reconnu

Informations insuffisantes

13.2.3 Suspicious (orange)

Non signé ET emplacement inhabituel (Temp, AppData, Downloads)

Commandline obfusquée (encodage base64, caractères suspects)

Nom de fichier imitant un fichier système (svchost.exe dans mauvais dossier)

Fichier caché ou attributs suspects

13.2.4 Critical (rouge)

Entrée système modifiée (Winlogon, IFEO avec debugger)

Driver non signé

Service avec compte LocalSystem et binaire suspect

13.3 Liste blanche intégrée

Publishers de confiance (non exhaustif) :
  - Microsoft Corporation
  - Google LLC
  - Mozilla Corporation
  - Adobe Inc.
  - Intel Corporation
  - NVIDIA Corporation
  - AMD
  - Oracle Corporation
  - ...

Mise à jour de la liste via releases

13.4 Extension future (opt-in)

Option pour requête VirusTotal (nécessite clé API utilisateur)

Cache local des résultats (éviter requêtes répétées)

14. Logs, diagnostics et export

14.1 Logs

Emplacement : %ProgramData%\BootSentry\Logs\

Rotation : 1 fichier par jour, rétention 30 jours

Niveaux : Debug, Info, Warning, Error

Format : [Timestamp] [Level] [Source] Message

14.2 Export

Export inventaire (CSV/JSON) avec options :
  - Toutes les entrées ou sélection
  - Avec/sans métadonnées détaillées
  - Option anonymisation

Export diagnostics (zip) :
  - Logs récents
  - Manifests de backup
  - Configuration (sans données sensibles)
  - Infos système (version OS, RAM, CPU)

15. Cas limites et comportements spéciaux

15.1 Entrées problématiques

| Cas | Comportement |
|-----|--------------|
| CommandLine vide | Afficher avec indicateur "Commande vide" |
| CommandLine très longue (> 1000 chars) | Tronquer à l'affichage + tooltip complet |
| Chemin avec caractères Unicode | Supporter via API Unicode (W) |
| Chemin UNC (\\server\share) | Timeout 5s, marquer "Réseau" |
| Variables d'environnement non résolues | Afficher brut + résolu si possible |
| Raccourci .lnk vers raccourci | Résoudre récursivement (max 5 niveaux) |
| Entrée en double (même source) | Dédupliquer avec indicateur |

15.2 Services spéciaux

| Service | Comportement |
|---------|--------------|
| Service avec dépendances | Lister les dépendances avant disable |
| Service en état "Starting" | Attendre ou avertir |
| Service protégé (anti-tamper) | Marquer comme "Protégé" |

15.3 Tâches planifiées spéciales

| Cas | Comportement |
|-----|--------------|
| Tâche avec triggers multiples | Lister tous les triggers |
| Tâche avec conditions (réseau, idle) | Afficher les conditions |
| Tâche dans sous-dossier | Afficher le chemin complet |
| Tâche corrompue | Marquer comme "Invalide" |

15.4 Permissions spéciales

| Cas | Comportement |
|-----|--------------|
| Clé registre TrustedInstaller | Lecture seule, marquer "Protégé système" |
| Fichier avec ACL restrictive | Signaler l'accès limité |
| Dossier avec héritage désactivé | Gérer correctement les permissions |

16. Tests et qualité

16.1 Unit tests

Framework : xUnit

Couverture cible : 80% minimum sur Core, Providers, Actions

Tests prioritaires :
  - Parsing CommandLine (cas limites, guillemets, espaces)
  - Génération Id stable (déterminisme)
  - Strategies disable/enable (tous les types)
  - Heuristiques de scoring

16.2 Integration tests

Environnement : VM Windows 10/11 dédiée

Fixtures :
  - Script de création d'entrées de démarrage contrôlées
  - Entrées registre de test
  - Tâches planifiées de test
  - Services de test (mock)

Scénarios :
  - Scan complet + vérification résultats
  - Disable + Restore cycle
  - Delete + vérification backup
  - Élévation UAC (manuel)

16.3 Tests UI

Framework : optionnel (WinAppDriver ou manuel)

Scénarios minimaux :
  - Navigation clavier complète
  - Test avec Narrator
  - Test High Contrast mode
  - Test à 150% DPI

16.4 CI/CD (GitHub Actions)

```yaml
# .github/workflows/ci.yml
name: CI

on: [push, pull_request]

jobs:
  build:
    runs-on: windows-latest
    steps:
      - uses: actions/checkout@v4
      - uses: actions/setup-dotnet@v4
        with:
          dotnet-version: '8.0.x'
      - run: dotnet restore
      - run: dotnet build --configuration Release --no-restore
      - run: dotnet test --no-build --configuration Release

  package:
    needs: build
    runs-on: windows-latest
    if: startsWith(github.ref, 'refs/tags/')
    steps:
      - uses: actions/checkout@v4
      - run: dotnet publish -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true
      - uses: actions/upload-artifact@v4
        with:
          name: BootSentry-portable
          path: src/BootSentry.UI/bin/Release/net8.0-windows/win-x64/publish/
```

17. Distribution

17.1 Portable (prioritaire)

Single-file publish self-contained

Commande : dotnet publish -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true

Résultat : BootSentry.exe (~60-80 Mo)

17.2 MSI (Phase 2)

WiX Toolset v4

Fonctionnalités :
  - Installation dans Program Files
  - Raccourci menu démarrer
  - Désinstallation propre (suppression données optionnelle)
  - Mise à jour in-place

17.3 (Option futur) MSIX

Avantages : auto-update, sandbox partielle

Contraintes : nécessite certificat de signature

18. Roadmap

Phase 1 – MVP (4-6 semaines)

Registry Run/RunOnce + Wow6432Node

UI liste + détails + recherche

Disable/Enable (HKCU direct, HKLM avec élévation)

Logs basiques

Architecture i18n en place

Single instance (mutex)

Phase 2 – Rollback + delete (3-4 semaines)

Backup/Restore transactionnel complet

Startup folders (user + common)

Delete registre/dossiers avec backup

Signature Authenticode + hash à la demande

Gestion erreurs complète

Phase 3 – Sources avancées (4-5 semaines)

Scheduled tasks (lecture + disable)

Services non-Microsoft

Mode Expert complet

IFEO (lecture + alerte)

MSI installer

Phase 4 – Polish (2-3 semaines)

Onboarding première utilisation

Heuristiques de scoring avancées

Vérification mise à jour

Export CSV/JSON complet

Documentation utilisateur

Phase 5 (option future)

Sources sensibles supplémentaires (Shell extensions, BHO)

Intégration VirusTotal opt-in

Plugins/providers externes

Thèmes personnalisés

19. Critères d'acceptation globaux

19.1 Fonctionnels

Scan ne freeze pas l'UI (async obligatoire)

Toute action destructive crée un backup exploitable

Rollback fonctionne sur tous les types implémentés

Mode public protège contre opérations à risque

Pas d'exécution de binaires cibles

Logs et diagnostics exploitables

19.2 Techniques

Couverture de tests > 80% sur Core/Providers/Actions

Temps de démarrage < 1.5s (cold start)

Scan complet < 2s (hors hash)

Aucune fuite mémoire détectable sur 1h d'utilisation

19.3 UX

Navigation clavier complète

Compatible lecteur d'écran (Narrator)

Fonctionne en High Contrast mode

Support DPI 100% à 150%

Messages d'erreur compréhensibles par un non-technicien

20. Annexes

20.1 Références techniques

Microsoft Docs - Registry Run Keys : https://docs.microsoft.com/en-us/windows/win32/setupapi/run-and-runonce-registry-keys

Task Scheduler API : https://docs.microsoft.com/en-us/windows/win32/taskschd/task-scheduler-start-page

WinVerifyTrust : https://docs.microsoft.com/en-us/windows/win32/api/wintrust/nf-wintrust-winverifytrust

Service Control Manager : https://docs.microsoft.com/en-us/windows/win32/services/service-control-manager

20.2 Outils de référence

Sysinternals Autoruns : https://docs.microsoft.com/en-us/sysinternals/downloads/autoruns

Process Monitor : https://docs.microsoft.com/en-us/sysinternals/downloads/procmon

20.3 Glossaire

| Terme | Définition |
|-------|------------|
| HKCU | HKEY_CURRENT_USER - Registre utilisateur courant |
| HKLM | HKEY_LOCAL_MACHINE - Registre machine |
| Authenticode | Système de signature numérique Microsoft |
| SCM | Service Control Manager |
| IFEO | Image File Execution Options |
| UAC | User Account Control |
| WoW64 | Windows on Windows 64-bit (émulation 32-bit) |