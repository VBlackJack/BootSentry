<#
 Copyright 2025 Julien Bombled

 Licensed under the Apache License, Version 2.0 (the "License");
 you may not use this file except in compliance with the License.
 You may obtain a copy of the License at

     http://www.apache.org/licenses/LICENSE-2.0

 Unless required by applicable law or agreed to in writing, software
 distributed under the License is distributed on an "AS IS" BASIS,
 WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 See the License for the specific language governing permissions and
 limitations under the License.
#>

<#
.SYNOPSIS
    Build pipeline for BootSentry.

.DESCRIPTION
    Orchestrates clean, restore, version injection, build, test, publish,
    and InnoSetup installer compilation into a single script.

.PARAMETER Version
    Semver version string (e.g. 1.2.0). Required.

.PARAMETER Configuration
    Build configuration. Defaults to Release.

.PARAMETER Runtime
    Runtime identifier. Defaults to win-x64.

.PARAMETER SkipTests
    Skip running unit tests.

.PARAMETER SkipInstaller
    Skip InnoSetup installer compilation.

.EXAMPLE
    .\build.ps1 -Version 1.2.0
    .\build.ps1 -Version 1.2.0 -SkipTests
    .\build.ps1 -Version 1.2.0 -SkipInstaller
#>

param(
    [Parameter(Mandatory = $true)]
    [string]$Version,

    [string]$Configuration = "Release",

    [string]$Runtime = "win-x64",

    [switch]$SkipTests,

    [switch]$SkipInstaller
)

Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"

# ---------------------------------------------------------------------------
# Constants
# ---------------------------------------------------------------------------

$RootDir = $PSScriptRoot
$SolutionFile = Join-Path $RootDir "BootSentry.sln"
$PublishDir = Join-Path $RootDir "publish"
$BuildsDir = Join-Path $RootDir "builds"
$UIProject = Join-Path $RootDir "src\BootSentry.UI\BootSentry.UI.csproj"
$InstallerScript = Join-Path $RootDir "installer\BootSentry.iss"

$LibraryProjects = @(
    Join-Path $RootDir "src\BootSentry.Core\BootSentry.Core.csproj"
    Join-Path $RootDir "src\BootSentry.Actions\BootSentry.Actions.csproj"
    Join-Path $RootDir "src\BootSentry.Backup\BootSentry.Backup.csproj"
    Join-Path $RootDir "src\BootSentry.Providers\BootSentry.Providers.csproj"
    Join-Path $RootDir "src\BootSentry.Security\BootSentry.Security.csproj"
)

$InnoSetupCandidates = @(
    "C:\Program Files (x86)\Inno Setup 6\ISCC.exe"
    "C:\Program Files\Inno Setup 6\ISCC.exe"
)

# ---------------------------------------------------------------------------
# Helpers
# ---------------------------------------------------------------------------

function Write-Step {
    param([string]$Number, [string]$Title)
    Write-Host ""
    Write-Host "[$Number] $Title" -ForegroundColor Cyan
    Write-Host ("-" * 60) -ForegroundColor DarkGray
}

function Write-Success {
    param([string]$Message)
    Write-Host "  [OK] $Message" -ForegroundColor Green
}

function Write-Info {
    param([string]$Message)
    Write-Host "  $Message" -ForegroundColor Gray
}

function Write-Fail {
    param([string]$Message)
    Write-Host "  [FAIL] $Message" -ForegroundColor Red
}

function Stop-WithError {
    param([string]$Message)
    Write-Host ""
    Write-Host "ERROR: $Message" -ForegroundColor Red
    exit 1
}

function Get-FileSizeFormatted {
    param([string]$Path)
    $size = (Get-Item $Path).Length
    if ($size -ge 1MB) {
        return "{0:N2} MB" -f ($size / 1MB)
    }
    return "{0:N2} KB" -f ($size / 1KB)
}

# ---------------------------------------------------------------------------
# Step 1: Validation
# ---------------------------------------------------------------------------

Write-Host ""
Write-Host "========================================" -ForegroundColor Yellow
Write-Host "  BootSentry Build Pipeline" -ForegroundColor Yellow
Write-Host "========================================" -ForegroundColor Yellow

Write-Step "1" "Validation"

# Validate semver format
if ($Version -notmatch '^\d+\.\d+\.\d+$') {
    Stop-WithError "Invalid version format '$Version'. Expected semver (e.g. 1.2.0)."
}

Write-Success "Version format: $Version"

# Validate dotnet CLI
if (-not (Get-Command "dotnet" -ErrorAction SilentlyContinue)) {
    Stop-WithError "'dotnet' CLI not found. Install the .NET SDK first."
}

$dotnetVersion = dotnet --version
Write-Success "dotnet CLI found: v$dotnetVersion"

# Validate solution file
if (-not (Test-Path $SolutionFile)) {
    Stop-WithError "Solution file not found: $SolutionFile"
}

Write-Success "Solution file found"

# Validate InnoSetup (unless skipped)
$IsccPath = $null
if (-not $SkipInstaller) {
    # Check environment variable first
    if ($env:INNO_SETUP_PATH -and (Test-Path $env:INNO_SETUP_PATH)) {
        $IsccPath = $env:INNO_SETUP_PATH
    }
    else {
        foreach ($candidate in $InnoSetupCandidates) {
            if (Test-Path $candidate) {
                $IsccPath = $candidate
                break
            }
        }
    }

    if (-not $IsccPath) {
        Stop-WithError "InnoSetup ISCC.exe not found. Install Inno Setup 6 or use -SkipInstaller."
    }

    Write-Success "InnoSetup found: $IsccPath"
}
else {
    Write-Info "InnoSetup: skipped"
}

Write-Info "Configuration: $Configuration"
Write-Info "Runtime: $Runtime"

# ---------------------------------------------------------------------------
# Step 2: Clean
# ---------------------------------------------------------------------------

Write-Step "2" "Clean"

if (Test-Path $PublishDir) {
    Remove-Item -Path $PublishDir -Recurse -Force
    Write-Success "Removed publish/"
}
else {
    Write-Info "publish/ already clean"
}

$versionOutputDir = Join-Path $BuildsDir "v$Version"
if (Test-Path $versionOutputDir) {
    Remove-Item -Path $versionOutputDir -Recurse -Force
    Write-Success "Removed builds/v$Version/"
}

# ---------------------------------------------------------------------------
# Step 3: Restore
# ---------------------------------------------------------------------------

Write-Step "3" "Restore"

dotnet restore $SolutionFile --verbosity quiet
if ($LASTEXITCODE -ne 0) { Stop-WithError "dotnet restore failed." }

Write-Success "NuGet packages restored"

# ---------------------------------------------------------------------------
# Step 4: Version injection
# ---------------------------------------------------------------------------

Write-Step "4" "Version injection"

$assemblyVersion = "$Version.0"

# Update UI project (Version, AssemblyVersion, FileVersion)
$uiContent = Get-Content $UIProject -Raw

$uiContent = $uiContent -replace '<Version>[^<]+</Version>', "<Version>$Version</Version>"
$uiContent = $uiContent -replace '<AssemblyVersion>[^<]+</AssemblyVersion>', "<AssemblyVersion>$assemblyVersion</AssemblyVersion>"
$uiContent = $uiContent -replace '<FileVersion>[^<]+</FileVersion>', "<FileVersion>$assemblyVersion</FileVersion>"

Set-Content -Path $UIProject -Value $uiContent -NoNewline
Write-Success "UI project: $Version (Assembly: $assemblyVersion)"

# Update library projects (Version only)
foreach ($proj in $LibraryProjects) {
    if (Test-Path $proj) {
        $content = Get-Content $proj -Raw
        if ($content -match '<Version>[^<]+</Version>') {
            $content = $content -replace '<Version>[^<]+</Version>', "<Version>$Version</Version>"
            Set-Content -Path $proj -Value $content -NoNewline
            Write-Success "$(Split-Path $proj -Leaf): $Version"
        }
    }
}

# Update InnoSetup script
if (Test-Path $InstallerScript) {
    $issContent = Get-Content $InstallerScript -Raw
    $issContent = $issContent -replace '#define MyAppVersion "[^"]+"', "#define MyAppVersion ""$Version"""
    Set-Content -Path $InstallerScript -Value $issContent -NoNewline
    Write-Success "BootSentry.iss: $Version"
}

# ---------------------------------------------------------------------------
# Step 5: Build
# ---------------------------------------------------------------------------

Write-Step "5" "Build"

dotnet build $SolutionFile -c $Configuration --no-restore --verbosity quiet
if ($LASTEXITCODE -ne 0) { Stop-WithError "dotnet build failed." }

Write-Success "Build succeeded"

# ---------------------------------------------------------------------------
# Step 6: Tests
# ---------------------------------------------------------------------------

Write-Step "6" "Tests"

if ($SkipTests) {
    Write-Info "Tests: skipped"
}
else {
    dotnet test $SolutionFile -c $Configuration --no-build --verbosity quiet
    if ($LASTEXITCODE -ne 0) { Stop-WithError "Tests failed." }

    Write-Success "All tests passed"
}

# ---------------------------------------------------------------------------
# Step 7: Publish
# ---------------------------------------------------------------------------

Write-Step "7" "Publish"

dotnet publish $UIProject `
    -c $Configuration `
    -r $Runtime `
    --self-contained true `
    -p:PublishSingleFile=true `
    -p:IncludeNativeLibrariesForSelfExtract=true `
    -o $PublishDir `
    --verbosity quiet

if ($LASTEXITCODE -ne 0) { Stop-WithError "dotnet publish failed." }

$publishedExe = Join-Path $PublishDir "BootSentry.exe"
if (-not (Test-Path $publishedExe)) {
    Stop-WithError "Published executable not found at $publishedExe"
}

Write-Success "Published to publish/"

# ---------------------------------------------------------------------------
# Step 8: Installer
# ---------------------------------------------------------------------------

Write-Step "8" "Installer"

$installerOutput = $null
if ($SkipInstaller) {
    Write-Info "Installer: skipped"
}
else {
    & $IsccPath $InstallerScript /Q
    if ($LASTEXITCODE -ne 0) { Stop-WithError "InnoSetup compilation failed." }

    $installerOutput = Join-Path $RootDir "releases\BootSentry_Setup_v$Version.exe"
    if (-not (Test-Path $installerOutput)) {
        Stop-WithError "Installer not found at $installerOutput"
    }

    Write-Success "Installer compiled"
}

# ---------------------------------------------------------------------------
# Step 9: Organize output
# ---------------------------------------------------------------------------

Write-Step "9" "Organize output"

New-Item -ItemType Directory -Path $versionOutputDir -Force | Out-Null

# Copy portable executable
$portableName = "BootSentry_v${Version}_${Runtime}.exe"
$portableDest = Join-Path $versionOutputDir $portableName
Copy-Item -Path $publishedExe -Destination $portableDest
Write-Success "Portable: $portableName"

# Copy installer
if ($installerOutput -and (Test-Path $installerOutput)) {
    $installerName = "BootSentry_Setup_v$Version.exe"
    $installerDest = Join-Path $versionOutputDir $installerName
    Copy-Item -Path $installerOutput -Destination $installerDest
    Write-Success "Installer: $installerName"
}

# ---------------------------------------------------------------------------
# Step 10: Summary
# ---------------------------------------------------------------------------

Write-Step "10" "Summary"

Write-Host ""
Write-Host "  Version:       $Version" -ForegroundColor White
Write-Host "  Configuration: $Configuration" -ForegroundColor White
Write-Host "  Runtime:       $Runtime" -ForegroundColor White
Write-Host "  Output:        $versionOutputDir" -ForegroundColor White
Write-Host ""

# Portable
$portableSize = Get-FileSizeFormatted $portableDest
$portableHash = (Get-FileHash -Path $portableDest -Algorithm SHA256).Hash
Write-Host "  Portable executable:" -ForegroundColor White
Write-Host "    Path:   $portableDest" -ForegroundColor Gray
Write-Host "    Size:   $portableSize" -ForegroundColor Gray
Write-Host "    SHA256: $portableHash" -ForegroundColor Gray

# Installer
if ($installerOutput -and (Test-Path $installerDest)) {
    $installerSize = Get-FileSizeFormatted $installerDest
    $installerHash = (Get-FileHash -Path $installerDest -Algorithm SHA256).Hash
    Write-Host ""
    Write-Host "  Installer:" -ForegroundColor White
    Write-Host "    Path:   $installerDest" -ForegroundColor Gray
    Write-Host "    Size:   $installerSize" -ForegroundColor Gray
    Write-Host "    SHA256: $installerHash" -ForegroundColor Gray
}

Write-Host ""
Write-Host "Build completed successfully!" -ForegroundColor Green
Write-Host ""
