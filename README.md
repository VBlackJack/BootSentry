# BootSentry

[![CI](https://github.com/VBlackJack/BootSentry/actions/workflows/ci.yml/badge.svg)](https://github.com/VBlackJack/BootSentry/actions/workflows/ci.yml)
[![.NET](https://img.shields.io/badge/.NET-8.0-512BD4)](https://dotnet.microsoft.com/)
[![License](https://img.shields.io/badge/License-Apache%202.0-blue.svg)](LICENSE)
[![Platform](https://img.shields.io/badge/Platform-Windows-0078D4)](https://www.microsoft.com/windows)

**BootSentry** is a modern Windows startup manager that allows you to safely manage programs that start automatically with Windows.

## What's New in v1.1

- **Redesigned Interface** - User-centric tabs (Applications, Browsers, System, Advanced) instead of technical categories
- **Smart Microsoft Filter** - Hide system noise across ALL entry types (Services, Drivers, AND Apps like OneDrive/Edge)
- **Surgical Security** - Safe "Reset" for Winlogon entries and precise DLL removal for AppInit (prevents system breakage)
- **Full Undo Engine** - Complete rollback capability for all actions including complex registry list modifications
- **Browser Extensions** - Support for Chrome, Edge, Firefox, Brave, Opera, Opera GX, and Vivaldi
- **Onboarding Tour** - New welcome experience for first-time users
- **Improved Layout** - Optimized window dimensions and proportions for 1080p screens

## Features

- **Complete Startup Analysis** - Scans 14+ startup locations including Registry, Startup folders, Scheduled Tasks, Services, Drivers, and more
- **Knowledge Base** - 363+ software entries with descriptions, safety levels, and recommendations
- **AMSI Malware Scanning** - Integrated antivirus scanning using Windows AMSI (Antimalware Scan Interface)
- **Risk Assessment** - Automatic risk level evaluation based on signature, location, publisher, and behavior patterns
- **Safe Operations** - Automatic backup before any modification with one-click rollback
- **Digital Signature Verification** - Authenticode signature verification using Windows native APIs
- **Dark Theme** - Full dark mode support with system theme detection
- **Expert Mode** - Toggle between standard and expert views
- **Export** - JSON/CSV export with optional knowledge base matching and anonymization
- **Multi-language** - French and English support
- **Accessible** - Full keyboard navigation, screen reader support (WCAG 2.1)
- **High DPI** - Per-Monitor V2 DPI awareness

## Startup Sources

BootSentry analyzes the following startup locations:

| Source | Description |
|--------|-------------|
| Registry Run/RunOnce | HKLM & HKCU Run keys |
| Registry Policies | Group Policy startup scripts |
| Startup Folders | User and common startup folders |
| Scheduled Tasks | Windows Task Scheduler |
| Services | Windows services (Auto-start) |
| Drivers | Kernel drivers |
| Winlogon | Shell, Userinit, Notify |
| Session Manager | BootExecute, PendingFileRename |
| AppInit_DLLs | DLL injection mechanism |
| IFEO | Image File Execution Options (Debugger) |
| Shell Extensions | Explorer extensions |
| Browser Helper Objects | IE/Edge BHOs |
| Print Monitors | Spooler print monitors |
| Winsock LSP | Layered Service Providers |
| Browser Extensions | Chrome, Edge, Firefox, Brave, Opera, Vivaldi |

## Installation

### Portable (Recommended)

1. Download `BootSentry.exe` from [Releases](https://github.com/VBlackJack/BootSentry/releases)
2. Place it in a folder of your choice
3. Run it

No installation required. Data is stored in:
- Settings: `%LocalAppData%\BootSentry\settings.json`
- Logs, Backups, Knowledge DB: `%ProgramData%\BootSentry\`

### Build from Source

```bash
# Clone the repository
git clone https://github.com/VBlackJack/BootSentry.git
cd BootSentry

# Build
dotnet build -c Release

# Run tests
dotnet test

# Publish single-file executable
dotnet publish src/BootSentry.UI/BootSentry.UI.csproj -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true
```

## Usage

### Standard Mode
- View and manage non-system startup entries
- Safe for most users
- Critical system entries are hidden

### Expert Mode (Ctrl+E)
- View all startup entries including system components
- Access to advanced actions (Delete, Services, Drivers)
- For advanced users only

### Knowledge Base
When selecting an entry, BootSentry displays:
- **Safety Level** - Critical, Important, Safe, Recommended to Disable, Should Remove
- **Description** - What the software does
- **Disable Impact** - What happens if you disable it
- **Performance Impact** - RAM/CPU usage
- **Recommendation** - Suggested action

### Malware Scanning
- Right-click an entry and select "Scan with antivirus"
- Uses Windows AMSI to scan files with your installed antivirus
- Results: Clean, Malware Detected, Blocked, Error

### Keyboard Shortcuts

| Shortcut | Action |
|----------|--------|
| F5 | Refresh |
| Ctrl+F | Search |
| Ctrl+E | Toggle Expert Mode |
| Delete | Disable selected |
| Ctrl+Delete | Delete selected |
| Ctrl+Z | Undo last action |
| Ctrl+Shift+Z | Open restore history |
| Ctrl+S | Export to JSON |
| Ctrl+H | Show history |
| F1 | Help |
| Escape | Clear selection |

## Architecture

```
BootSentry/
├── src/
│   ├── BootSentry.Core/        # Models, enums, services (RiskAnalyzer, Export)
│   ├── BootSentry.Providers/   # 14 startup source providers
│   ├── BootSentry.Actions/     # Action strategies (enable/disable/delete)
│   ├── BootSentry.Backup/      # Transaction manager and backup storage
│   ├── BootSentry.Security/    # Signature verification, hash, AMSI scanner
│   ├── BootSentry.Knowledge/   # SQLite knowledge base (363+ entries)
│   └── BootSentry.UI/          # WPF application (MVVM)
└── tests/
    ├── BootSentry.Core.Tests/
    ├── BootSentry.Providers.Tests/
    ├── BootSentry.Actions.Tests/
    ├── BootSentry.Backup.Tests/
    └── BootSentry.Security.Tests/
```

## Requirements

- Windows 10 1809+ / Windows 11 (x64)
- .NET 8.0 Runtime (included in single-file build)

## Security

- **No code execution** - BootSentry never executes target binaries
- **Signature verification** - Uses Windows WinVerifyTrust API
- **AMSI integration** - Leverages your existing antivirus for malware scanning
- **Automatic backups** - All changes are backed up before execution
- **UAC integration** - Elevation requested only when necessary
- **Path traversal protection** - Validated backup transaction IDs

## Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

1. Fork the repository
2. Create your feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

### Adding Knowledge Base Entries

To add new software entries to the knowledge base, edit `src/BootSentry.Knowledge/Services/KnowledgeSeeder.cs`.

## License

This project is licensed under the Apache License 2.0 - see the [LICENSE](LICENSE) file for details.

## Author

**Julien Bombled** - [GitHub](https://github.com/VBlackJack)

## Acknowledgments

- Built with [.NET 8](https://dotnet.microsoft.com/) and [WPF](https://docs.microsoft.com/dotnet/desktop/wpf/)
- MVVM toolkit by [CommunityToolkit.Mvvm](https://github.com/CommunityToolkit/dotnet)
- Knowledge base powered by [SQLite](https://www.sqlite.org/) via [Microsoft.Data.Sqlite](https://docs.microsoft.com/dotnet/standard/data/sqlite/)
- Icons and UI design inspired by modern Windows applications
