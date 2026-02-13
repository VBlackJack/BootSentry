# BootSentry v1.2

**Reprenez le contrôle du démarrage de Windows.**

BootSentry est un utilitaire de sécurité et d'optimisation conçu pour analyser, nettoyer et sécuriser le démarrage de votre ordinateur. Il détecte les programmes ralentisseurs, les extensions de navigateur indésirables et les mécanismes de persistance utilisés par les malwares.

---

## Pourquoi utiliser BootSentry ?

* **Accélérez votre PC :** Identifiez et désactivez les logiciels inutiles qui se lancent automatiquement.
* **Nettoyez vos navigateurs :** Visualisez et supprimez les extensions suspectes sur Chrome, Firefox, Edge, Opera et Brave.
* **Détectez les menaces :** Analyse antivirus intégrée (via Windows Defender/AMSI) pour repérer les fichiers dangereux.
* **Interface Claire :** Grâce au **Filtre Intelligent**, masquez les centaines de processus système Microsoft pour vous concentrer sur l'essentiel.

---

## Nouveautés de la version 1.2

* **Script de build unifié :** `build.ps1` orchestre clean, tests, publish et installeur en une seule commande.
* **Déduplication registre :** Les entrées en double (active + désactivée) sont correctement fusionnées.
* **Correction export CSV :** La neutralisation des formules CSV fonctionne avec tous les caractères spéciaux.

### Rappel version 1.1

* **Nouvelle Interface :** Navigation simplifiée par onglets (Applications, Navigateurs, Système).
* **Mode "Filtre Microsoft" :** Masque automatiquement les services et applications Microsoft sûrs (OneDrive, Edge, Système) pour une lecture plus facile.
* **Moteur "Undo" (Annuler) :** Une erreur ? Cliquez simplement sur "Annuler" pour restaurer une entrée supprimée ou modifiée.
* **Sécurité Chirurgicale :** Nettoyage avancé des clés critiques (AppInit, Winlogon) sans risquer de casser Windows.
* **Visite Guidée :** Un tutoriel interactif vous guide lors du premier lancement.

---

## Installation

1.  Téléchargez la dernière version (`BootSentry_Setup_v1.2.0.exe`) depuis la section **Releases**.
2.  Lancez l'installateur.
    * *Note : Windows SmartScreen peut afficher un avertissement car l'application est récente. Cliquez sur "Informations complémentaires" > "Exécuter quand même".*
3.  L'application se lancera automatiquement après l'installation.

---

## Guide Rapide

### 1. Vue "Mes Applications"
C'est l'écran principal. Il liste les logiciels (Spotify, Steam, Discord...) qui se lancent au démarrage.
* **Désactiver :** Clic droit > "Désactiver" pour empêcher le lancement sans supprimer l'entrée.
* **Supprimer :** (Mode Expert) Retire définitivement l'entrée.

### 2. Vue "Navigateurs"
Affiche toutes les extensions installées sur tous vos navigateurs.
* Idéal pour repérer une extension publicitaire ou espionne installée à votre insu.

### 3. Mode Expert
Cliquez sur le bouton "Expert" en haut à droite pour afficher les onglets techniques :
* **Services & Pilotes :** Pour les utilisateurs avancés.
* **Sécurité Avancée :** (Winlogon, Image Hijack, AppInit).
    * *Bouton "Réparer" :* Remet les valeurs Windows par défaut en cas d'infection.

### 4. En cas d'erreur
Pas de panique ! Utilisez le menu **Actions > Annuler (Ctrl+Z)** pour revenir en arrière immédiatement, ou ouvrez l'**Historique** pour voir toutes vos modifications passées.

---

## Sécurité et Confidentialité

* **Open Source :** Le code est transparent et auditable.
* **Local :** Aucune donnée n'est envoyée sur le cloud. Tout se passe sur votre machine.
* **Protection Système :** BootSentry empêche la suppression des fichiers vitaux de Windows pour éviter les écrans noirs.

---

## Raccourcis Clavier

| Raccourci | Action |
|-----------|--------|
| F5 | Actualiser |
| Ctrl+F | Rechercher |
| Ctrl+E | Mode Expert |
| Suppr | Désactiver |
| Ctrl+Z | Annuler |
| Ctrl+H | Historique |

---

## Contribution

Le projet est open source ! Contributions bienvenues sur [GitHub](https://github.com/VBlackJack/BootSentry).

---

*Développé par Julien Bombled - Licence Apache 2.0*
