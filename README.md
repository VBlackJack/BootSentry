# BootSentry

[![CI](https://github.com/VBlackJack/BootSentry/actions/workflows/ci.yml/badge.svg)](https://github.com/VBlackJack/BootSentry/actions/workflows/ci.yml)
[![.NET](https://img.shields.io/badge/.NET-8.0-512BD4)](https://dotnet.microsoft.com/)
[![License](https://img.shields.io/badge/License-Apache%202.0-blue.svg)](LICENSE)
[![Platform](https://img.shields.io/badge/Platform-Windows-0078D4)](https://www.microsoft.com/windows)

**BootSentry** is a modern Windows startup manager that allows you to safely manage programs that start automatically with Windows.

![BootSentry Screenshot](docs/screenshot.png)

## Features

- **Complete Startup Analysis** - Scans 14+ startup locations including Registry, Startup folders, Scheduled Tasks, Services, Drivers, and more
- **Safe Operations** - Automatic backup before any modification with one-click rollback
- **Digital Signature Verification** - Authenticode signature verification using Windows native APIs
- **Risk Assessment** - Automatic risk level evaluation based on multiple factors
- **Expert Mode** - Toggle between standard and expert views
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

## Installation

### Portable (Recommended)

1. Download `BootSentry.exe` from [Releases](https://github.com/VBlackJack/BootSentry/releases)
2. Place it in a folder of your choice
3. Run it

No installation required. All data is stored in `%ProgramData%\BootSentry\`.

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
│   ├── BootSentry.Core/        # Models, enums, interfaces
│   ├── BootSentry.Providers/   # 14 startup source providers
│   ├── BootSentry.Actions/     # Action strategies (enable/disable/delete)
│   ├── BootSentry.Backup/      # Transaction manager and backup storage
│   ├── BootSentry.Security/    # Signature verification and hash calculation
│   └── BootSentry.UI/          # WPF application (MVVM)
└── tests/
    ├── BootSentry.Core.Tests/
    ├── BootSentry.Providers.Tests/
    └── BootSentry.Actions.Tests/
```

## Requirements

- Windows 10/11 (x64)
- .NET 8.0 Runtime (included in single-file build)

## Security

- **No code execution** - BootSentry never executes target binaries
- **Signature verification** - Uses Windows WinVerifyTrust API
- **Automatic backups** - All changes are backed up before execution
- **UAC integration** - Elevation requested only when necessary

## Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

1. Fork the repository
2. Create your feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

## License

This project is licensed under the Apache License 2.0 - see the [LICENSE](LICENSE) file for details.

## Author

**Julien Bombled** - [GitHub](https://github.com/VBlackJack)

## Acknowledgments

- Built with [.NET 8](https://dotnet.microsoft.com/) and [WPF](https://docs.microsoft.com/dotnet/desktop/wpf/)
- MVVM toolkit by [CommunityToolkit.Mvvm](https://github.com/CommunityToolkit/dotnet)
- Icons and UI design inspired by modern Windows applications
