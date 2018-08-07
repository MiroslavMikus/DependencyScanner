# add up assemby version
# add up nuspec version -> same as assembly version
# build with release
# update choco\VERIFICATION.txt file
# pack
# push

## Update verification hash
$currentPath = (Get-Item -Path ".\").FullName

$exePath = Join-Path $currentPath 'bin\Release\DependencyScanner.Standalone.exe'

$hash = (Get-FileHash $exePath).Hash 

$verificationPath = Join-Path $currentPath 'choco\VERIFICATION.txt'
$verificationContent = get-content $verificationPath

$newcontent = $verificationContent | select -SkipLast 1
$newcontent += $hash

$newcontent | Out-File $verificationPath
###


## Pack and push
$fileVersion = [System.Diagnostics.FileVersionInfo]::GetVersionInfo($exePath).FileVersion
$nupkgName = "Dependency.Scanner.$fileVersion.nupkg"

choco pack .\DependencyScanner.Standalone.nuspec

choco push $nupkgName --source https://push.chocolatey.org/
