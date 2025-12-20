#define MyAppName "BootSentry"
#define MyAppVersion "1.1.0"
#define MyAppPublisher "Julien Bombled"
#define MyAppURL "https://github.com/VBlackJack/BootSentry"
#define MyAppExeName "BootSentry.UI.exe"

[Setup]
; NOTE: The value of AppId uniquely identifies this application.
AppId={{A3B4C5D6-E7F8-9012-3456-7890ABCDEF12}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
AppPublisher={#MyAppPublisher}
AppPublisherURL={#MyAppURL}
AppSupportURL={#MyAppURL}
AppUpdatesURL={#MyAppURL}
DefaultDirName={autopf}\{#MyAppName}
DisableProgramGroupPage=yes
; Remove the following line to run in administrative install mode (install for all users.)
PrivilegesRequired=admin
OutputDir=..\releases
OutputBaseFilename=BootSentry_Setup_v{#MyAppVersion}
SetupIconFile=..\src\BootSentry.UI\Resources\app.ico
Compression=lzma
SolidCompression=yes
WizardStyle=modern
ArchitecturesInstallIn64BitMode=x64

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"
Name: "french"; MessagesFile: "compiler:Languages\French.isl"

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked

[Files]
; Main Executable (Single File Publish)
Source: "..\publish\{#MyAppExeName}"; DestDir: "{app}"; Flags: ignoreversion
; Native dependencies and other files in the publish folder
Source: "..\publish\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs createallsubdirs

[Icons]
Name: "{autoprograms}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"
Name: "{autodesktop}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; Tasks: desktopicon

[Run]
Filename: "{app}\{#MyAppExeName}"; Description: "{cm:LaunchProgram,{#StringChange(MyAppName, '&', '&&')}}"; Flags: nowait postinstall skipifsilent runascurrentuser

[Dirs]
; Ensure Log and Backup folders exist in CommonAppData
Name: "{commonappdata}\BootSentry\Logs"; Permissions: users-modify
Name: "{commonappdata}\BootSentry\Backups"; Permissions: users-modify

[Code]
// Optional: Add custom logic here if needed
