#tool "nuget:?package=Microsoft.TestPlatform&version=15.7.0"
#tool "nuget:?package=OpenCover&version=4.7.922"
#tool "nuget:?package=ReportGenerator&version=4.0.9"
#tool "nuget:?package=GitVersion.CommandLine&version=4.0.0"
#tool "nuget:?package=NuGet.CommandLine&version=4.9.2"
#tool "nuget:?package=chocolatey&version=0.10.11"

#addin "nuget:?package=Cake.FileHelpers&version=3.1.0"

#load "build/paths.cake"

var packageVersion = "0.1.0.4";
///////////////////////////////////////////////////////////////////////////////
// ARGUMENTS
///////////////////////////////////////////////////////////////////////////////
var target = Argument("target", "Compile");
var configuration = Argument("configuration", "Release");
var packageOutputPath = Argument<DirectoryPath>("PackageOutputPath", "packages");

///////////////////////////////////////////////////////////////////////////////
// Setup / Teardown
///////////////////////////////////////////////////////////////////////////////

// Executed BEFORE the first task.
Setup(context =>
{
    EnsureDirectoryExists(packageOutputPath);
});

///////////////////////////////////////////////////////////////////////////////
// TASKS
///////////////////////////////////////////////////////////////////////////////

Task("Test-Resolve-Tool")
    .Does(()=> {
        FilePath nugetPath = Context.Tools.Resolve("choco.exe");
        Information(nugetPath?.ToString()?? "not found");
    });

Task("Clean")
    .Does(()=>{
        foreach (var dir in GetDirectories("**/Debug").Concat(GetDirectories("**/Release")))
        {
            Information($"Cleaning: {dir}");
            CleanDirectory(dir);
        }
        CleanDirectory("TestResults");
    });

Task("Restore-NuGet-Packages")
    .IsDependentOn("Clean")
    .Does(() =>
    {
        NuGetRestore(Paths.SolutionPath);
    });

Task("Build")
    .IsDependentOn("Restore-NuGet-Packages")
    .Does(()=>{
        MSBuild(Paths.SolutionPath.ToString(), settings => settings.SetConfiguration(configuration));
    });

Task("Test")
    .IsDependentOn("Build")
    .Does(()=> {
            VSTest($"**/bin/{configuration}/*.Test.dll", new VSTestSettings
            {
                EnableCodeCoverage = true,
                InIsolation = true,
                Logger = "trx",
                Parallel = true,
                TestAdapterPath = @"DependencyScanner.Core.Test\bin\Debug\"
            });
    });
Task("Version")
    .Does(()=> {
        Information($"Using version {packageVersion}");

        // update assembly version
        GitVersion(new GitVersionSettings
        {
            OutputType = GitVersionOutput.BuildServer,
            UpdateAssemblyInfo = true
        });
        // could be commited
        // GitCommit("./", "CakeBuild", "cake@build.com",$"Update assembly version to {packageVersion}")
    });

Task("Remove-Packages")
    .Does(()=>{
        Information($"Cleaning {packageOutputPath}");
        CleanDirectory(packageOutputPath);
    });

Task("Pack-Chocolatey")
    .IsDependentOn("Test")
    .Does(()=> {
        CheckComfiguration("Release", configuration);

        // calculate new hash
        var hash = CalculateFileHash(@"DependencyScanner.Standalone\bin\Release\DependencyScanner.Standalone.exe", HashAlgorithm.SHA256);
        Information("New hash: {0}", hash.ToHex());

        EnsureDoesNotExist(Paths.ChocoHashFile);

        FileWriteText(Paths.ChocoHashFile, hash.ToHex());

        // create shrim
        using (System.IO.File.Create(@"DependencyScanner.Standalone\bin\Release\DependencyScanner.Standalone.exe.gui")){};

        // choco pack
        ChocolateyPack(
            Paths.ChocoNuspecFile,
            new ChocolateyPackSettings
            {
                Version = packageVersion
            });

        // move choco nuget to packages/choco/...        
        var chocoFileName=$"dependency-scanner.{packageVersion}.nupkg";

        EnsureDirectoryExists(packageOutputPath);

        var chocoPackageFileName = Combine(packageOutputPath, chocoFileName);

        EnsureDoesNotExist(chocoPackageFileName);

        MoveFile(chocoFileName, chocoPackageFileName);
    });

//////////////////////////////////////////////////////////////////////
// TASK TARGETS
//////////////////////////////////////////////////////////////////////
Task("Default")
    .IsDependentOn("Test");

RunTarget(target); 