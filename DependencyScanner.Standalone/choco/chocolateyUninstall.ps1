$shortcut = Join-Path ([System.Environment]::GetFolderPath('CommonPrograms')) 'DependencyScanner.Standalone.lnk'
$desktopShortcut = Join-Path ([System.Environment]::GetFolderPath('Desktop')) 'DependencyScanner.Standalone.lnk'
$programData = Join-Path $env:ProgramData "DependencyScanner"

Remove-Item -Force -Path $shortcut | Out-Null
Remove-Item -Force -Path $desktopShortcut | Out-Null
Remove-Item -Force -Path $programData | Out-Null
