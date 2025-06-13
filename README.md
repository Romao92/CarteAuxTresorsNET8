# 🗺️ Carte Aux Trésors - Simulation .NET 8
test
Ce projet est une simulation d'une carte aux trésors en console, écrite en .NET 8.  
Il lit des fichiers d'entrée décrivant une carte, des montagnes, des trésors et des aventuriers, puis exécute une simulation tour par tour et génère un fichier de sortie.

---

## 🚀 Fonctionnalités

- Lecture d’un fichier d’entrée texte décrivant la carte, les trésors, les montagnes et les aventuriers.
- Simulation **tour par tour**, avec gestion des mouvements, orientation et ramassage de trésors.
- Blocage correct des mouvements interdits (montagnes, cases occupées, bords de carte).
- Journalisation des actions dans la console **et dans un fichier log** (`logs/`).
- Génération d’un **fichier de sortie** avec état final de la carte.
- **Visualisation tour par tour** de la carte.
- Suite de **tests unitaires xUnit** complète.

---

## 🧪 Structure du projet

CarbonITCarteAuxTresorsNET8/ --> Projet console
CarbonITCarteAuxTresorsNET8.Tests/ --> Projet de tests xUnit
InputFile/ --> Fichiers d’entrée (.txt)
OutputFile/ --> Fichiers de sortie générés
logs/ --> Fichier log généré par NLog

---

## 📝 Exemple d'entrée (`InputFile/input.txt`)

C - 3 - 4
M - 1 - 0
M - 2 - 1
T - 0 - 3 - 2
T - 2 - 3 - 1
A - Lara - 1 - 1 - S - AADADAGGA

## 📝 Exemple en sortie (`ResultOutput_input-date`)
C - 3 - 4
M - 1 - 0
M - 2 - 1
T - 2 - 3 - 0
A - Lara - 0 - 3 - O - 2

---

### ✅ Prérequis

- [.NET SDK 8](https://dotnet.microsoft.com/en-us/download/dotnet/8.0) installé
- Un terminal (Terminal, iTerm2, VS Code ou JetBrains Rider)
- macOS, Linux ou Windows

---

### 💻 Exécution

Dans le terminal, place-toi à la racine du projet :

```bash
cd CarteAuxTresors
dotnet run
```

Cela va =>
Lire tous les fichiers .txt dans le dossier InputFile
Simuler les mouvements des aventuriers
Écrire les résultats dans OutputFile
Générer un log détaillé dans le dossier logs

📝 Résultat généré
Un fichier OutputFile/input-output.txt sera produit avec :
- Les éléments finaux de la carte
- La position et l’orientation finale des aventuriers
- La carte affichée visuellement avec les trésors restants

📊 Logs
Des fichiers de logs seront créés dans le dossier logs/ avec la date d'exécution :
logs/log-date-heure.txt

✅ Tests unitaires
Le projet contient des tests avec xUnit.

Pour les exécuter :

```bash
cd CarteAuxTresors
dotnet test
```
