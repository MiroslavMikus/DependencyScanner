# add up assemby version
# add up nuspec version -> same as assembly version
# build with release
# update choco\VERIFICATION.txt file
# pack
# push

Get-FileHash "F:\Projects\_GitHub\DependencyScanner\DependencyScanner.Standalone\bin\Release\DependencyScanner.Standalone.exe"

choco pack .\DependencyScanner.Standalone.nuspec

choco push Dependency.Scanner.0.1.0.1.nupkg --source https://push.chocolatey.org/