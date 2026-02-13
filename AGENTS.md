# Repository Guidelines

## Project Structure & Module Organization
BootSentry is a .NET 8 Windows solution (`BootSentry.sln`) split by responsibility:
- `src/BootSentry.Core`: shared models, enums, interfaces, core services.
- `src/BootSentry.Providers`: startup data collectors (registry, tasks, services, browser extensions, etc.).
- `src/BootSentry.Actions`: remediation/action strategies and execution.
- `src/BootSentry.Security`: hashing, signature verification, AMSI scanning.
- `src/BootSentry.Backup`: transaction/rollback storage.
- `src/BootSentry.Knowledge`: knowledge base seeding and lookup.
- `src/BootSentry.UI`: WPF app (ViewModels, Views, Controls, Resources).
- `tests/*`: test projects mirroring each major source module.
- `installer/BootSentry.iss`: Inno Setup installer definition.

## Build, Test, and Development Commands
- `dotnet restore BootSentry.sln`: restore NuGet packages.
- `dotnet build BootSentry.sln -c Debug`: build all projects.
- `dotnet run --project src/BootSentry.UI/BootSentry.UI.csproj`: launch the desktop app locally.
- `dotnet test BootSentry.sln -c Debug`: run all automated tests.
- `dotnet test tests/BootSentry.Core.Tests/BootSentry.Core.Tests.csproj --collect:"XPlat Code Coverage"`: run one suite with coverage output (Coverlet collector).
- `dotnet publish src/BootSentry.UI/BootSentry.UI.csproj -c Release -r win-x64`: create release binaries.
- `.\build.ps1 -Version X.Y.Z`: full build pipeline (clean, restore, build, test, publish, installer). Accepts `-SkipTests` and `-SkipInstaller` switches.

## Coding Style & Naming Conventions
Use C# conventions already present in the codebase: 4-space indentation, file-scoped namespaces, `nullable` enabled, and `ImplicitUsings` enabled. Use `PascalCase` for types/methods/properties, `I`-prefixed interfaces (`IRiskService`), and `_camelCase` for private fields. Keep module boundaries clear (providers discover, actions modify, core models remain reusable).

## Testing Guidelines
Tests use xUnit + FluentAssertions (`tests/*`). Add tests in the matching module test project and keep names explicit, e.g. `Analyze_WithUnsigned_IncreasesRiskScore`. Cover both normal and high-risk/security scenarios (registry, Winlogon, AppInit, extensions). No strict coverage gate is defined; maintain or improve coverage in touched areas.

## Commit & Pull Request Guidelines
Follow Conventional Commit prefixes used in history: `feat:`, `fix:`, `docs:`, `chore:`. Keep commits scoped and descriptive. PRs should include:
- concise functional summary and impacted modules,
- linked issue/ticket when available,
- test evidence (`dotnet test` results),
- screenshots/GIFs for UI changes,
- notes for installer or security-behavior changes.

## Security & Configuration Tips
This application modifies startup-related system state. Prefer reversible actions, preserve transaction/undo behavior, and avoid introducing destructive defaults. Do not commit local logs, machine-specific settings, or temporary analysis artifacts.
