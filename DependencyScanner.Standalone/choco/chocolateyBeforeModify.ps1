$tools = Split-Path $MyInvocation.MyCommand.Definition

$shortcut = Join-Path ([System.Environment]::GetFolderPath('CommonPrograms')) 'DependencyScanner.Standalone.lnk'
$desktopShortcut = Join-Path ([System.Environment]::GetFolderPath('Desktop')) 'DependencyScanner.Standalone.lnk'

Remove-Item $shortcut
Remove-Item $desktopShortcut

$toos > C:\temp2\test.txt
