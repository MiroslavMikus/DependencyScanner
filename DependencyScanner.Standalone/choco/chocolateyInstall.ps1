$tools = Split-Path $MyInvocation.MyCommand.Definition

$shortcut = Join-Path ([System.Environment]::GetFolderPath('CommonPrograms')) 'DependencyScanner.Standalone.lnk'
$desktopShortcut = Join-Path ([System.Environment]::GetFolderPath('Desktop')) 'DependencyScanner.Standalone.lnk'

$exe = Join-Path (Split-Path $tools) 'DependencyScanner.Standalone.exe'


Install-ChocolateyShortcut `
    -ShortcutFilePath $shortcut `
    -TargetPath $exe

Install-ChocolateyShortcut `
    -ShortcutFilePath $desktopShortcut `
    -TargetPath $exe
