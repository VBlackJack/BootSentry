# Roadmap BootSentry

## v1.0.0 (Current)
- [x] 14 providers de sources de démarrage
- [x] Vérification des signatures Authenticode
- [x] Système de backup/rollback transactionnel
- [x] Interface WPF moderne avec mode Expert
- [x] Localisation FR/EN
- [x] 108 tests unitaires
- [x] CI/CD avec GitHub Actions

---

## v1.1.0 - Sécurité Renforcée

### Haute Priorité

#### 1. Protection des Drivers Critiques (DriverProvider)
**Problème**: Désactiver un driver Boot (Type 0) peut rendre le système non-bootable (BSOD INACCESSIBLE_BOOT_DEVICE).

**Solution**:
- [ ] Étendre la liste `criticalDrivers` avec tous les drivers système critiques
- [ ] Ajouter un attribut `IsCritical` sur `StartupEntry`
- [ ] Bloquer complètement la désactivation des drivers Boot (Type 0)
- [ ] Double confirmation "DANGER" pour les drivers système
- [ ] Afficher un avertissement explicite sur les conséquences

```csharp
// Drivers qui ne doivent JAMAIS être désactivés
private static readonly HashSet<string> CriticalBootDrivers = new(StringComparer.OrdinalIgnoreCase)
{
    "disk", "volmgr", "volsnap", "partmgr", "volume",
    "fvevol", "iorate", "rdyboost", "wd", "intelpep",
    "acpi", "pci", "isapnp", "msisadrv", "vdrvroot",
    "storahci", "stornvme", "storufs", "spaceport",
    "ntfs", "refs", "fastfat", "exfat", "cdfs", "udfs"
};
```

#### 2. Restauration des ACLs du Registre
**Problème**: La restauration d'une clé supprimée peut perdre les ACLs d'origine.

**Solution**:
- [ ] Sauvegarder les ACLs (DACL) dans le manifest de backup
- [ ] Restaurer les ACLs avec la clé lors du rollback
- [ ] Gérer les cas spéciaux (clés protégées, TrustedInstaller)

```csharp
// Dans BackupManifest.cs
public class RegistryBackupData
{
    public string KeyPath { get; set; }
    public string ValueName { get; set; }
    public object Value { get; set; }
    public RegistryValueKind ValueKind { get; set; }
    public string SecurityDescriptor { get; set; } // SDDL format
}
```

#### 3. Tests CommandLineParser Exhaustifs
**Problème**: Le parsing de lignes de commande Windows est notoirement complexe.

**Cas à tester**:
- [ ] `"C:\Program Files\App\foo.exe" /bar "baz qux"`
- [ ] `C:\Program Files\App\foo.exe /bar` (sans quotes)
- [ ] `"C:\Path With ""Quotes""\app.exe" --arg`
- [ ] `cmd /c "start "" notepad.exe"`
- [ ] `rundll32.exe shell32.dll,Control_RunDLL`
- [ ] `%SystemRoot%\system32\cmd.exe /k dir`
- [ ] Chemins UNC: `\\server\share\app.exe`
- [ ] Chemins relatifs: `.\app.exe`, `..\bin\app.exe`

---

## v1.2.0 - Améliorations UX

### Moyenne Priorité

#### 4. Dry-Run Visuel
- [ ] Afficher un aperçu des modifications avant exécution
- [ ] Checkbox "Afficher les détails" dans la confirmation
- [ ] Résumé des clés/fichiers qui seront modifiés

#### 5. Export/Import de Configuration
- [ ] Exporter la liste des entrées désactivées
- [ ] Importer une configuration sur une autre machine
- [ ] Format JSON avec validation de schéma

#### 6. Notifications Toast
- [ ] Remplacer certains MessageBox par des toasts non-bloquants
- [ ] Feedback visuel pour les actions réussies
- [ ] Intégration avec le centre de notifications Windows

---

## v2.0.0 - Fonctionnalités Avancées

### Basse Priorité

#### 7. Internationalisation .resx
**Contexte**: La solution actuelle (Dictionary C#) est performante mais limitée.

**Migration**:
- [ ] Convertir `Strings.cs` en fichiers `.resx`
- [ ] `Resources.resx` (défaut anglais)
- [ ] `Resources.fr.resx` (français)
- [ ] Support des outils de traduction externes (Crowdin, POEditor)

#### 8. Intégration VirusTotal (Opt-in)
- [ ] Option pour vérifier les fichiers sur VirusTotal
- [ ] Affichage du score de détection
- [ ] Cache local des résultats
- [ ] Respect de la vie privée (opt-in explicite)

#### 9. Mode Comparaison
- [ ] Comparer l'état actuel avec un snapshot précédent
- [ ] Détecter les nouvelles entrées depuis le dernier scan
- [ ] Alerte sur les changements suspects

#### 10. Planificateur de Scan
- [ ] Scan automatique au démarrage Windows
- [ ] Notification des changements détectés
- [ ] Tâche planifiée optionnelle

#### 11. Plugin System
- [ ] Architecture pour providers tiers
- [ ] API publique documentée
- [ ] Sandbox pour les plugins

---

## Notes Techniques

### Sécurité
- Ne jamais exécuter les binaires cibles
- Toujours créer un backup avant modification
- Valider les chemins pour éviter les path traversal
- Utiliser les API Windows natives pour les signatures

### Performance
- Scan initial < 2s (sans hash)
- UI non-bloquante (async/await)
- Virtualisation DataGrid pour grandes listes
- Lazy loading des métadonnées fichiers

### Compatibilité
- Windows 10 version 1903+ (build 18362)
- Windows 11 toutes versions
- .NET 8.0 LTS
- x64 uniquement (pour l'instant)
