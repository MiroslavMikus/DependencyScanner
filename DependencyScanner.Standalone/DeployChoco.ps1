# add up assemby version
# add up nuspec version -> same as assembly version
# build with release
# update choco\VERIFICATION.txt file
# pack
# push

## Update verification hash
$currentPath = (Get-Item -Path ".\").FullName

Write-Host "The current path is $currentPath"

$exePath = Join-Path $currentPath 'bin\Release\DependencyScanner.Standalone.exe'

Write-Host "`nThe path to exe is $exePath"

$hash = (Get-FileHash $exePath).Hash 

Write-Host "`nCalculating new hash $hash"

$verificationPath = Join-Path $currentPath 'choco\VERIFICATION.txt'

$verificationContent = get-content $verificationPath

$newcontent = $verificationContent | select -SkipLast 1
$newcontent += $hash

$newcontent | Out-File $verificationPath

Write-Host "`nVerificaiton file was updated: $verificationPath"

###

## setup gui shrim
New-Item "bin\Release\DependencyScanner.Standalone.exe.gui" -type file -force | Out-Null

Write-Host "`nGui shrim was added"

## Pack and push
$fileVersion = [System.Diagnostics.FileVersionInfo]::GetVersionInfo($exePath).FileVersion
$nupkgName = "dependency-scanner.$fileVersion.nupkg"

Write-Host "`nDetected file version $fileVersion"

Write-Host "`nChoco pack"
choco pack .\DependencyScanner.Standalone.nuspec


$pushResult = Read-Host "Do you want push $nupkgName to https://push.chocolatey.org/ (y/n)"

if($puahResult -contains "y"){

    Write-Host "`nPushing  $nupkgName"
    #choco push $nupkgName --source https://push.chocolatey.org/
}

Read-Host "`nDone press enter to exit"

# Start-Process (Join-Path ([System.Environment]::GetFolderPath('CommonPrograms')) 'DependencyScanner.Standalone.lnk')

# choco upgrade dependency-scanner -y