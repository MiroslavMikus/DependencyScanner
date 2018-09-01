using DependencyScanner.Core.FileScan;
using DependencyScanner.Core.Model;
using DependencyScanner.Core.Tools;
using Newtonsoft.Json;
using NuGet;
using Serilog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;

//                                                       id                                                version                                                            dependencies
using T_Result = System.Collections.Generic.Dictionary<string, System.Collections.Generic.Dictionary<NuGet.SemanticVersion, System.Collections.Generic.IEnumerable<NuGet.PackageDependencySet>>>;

namespace DependencyScanner.Core.NugetReference
{
    public class NugetReferenceScan
    {
        private const string PackagesDirName = "packages";
        private readonly string _programDataPath;

        public NugetReferenceScan(string programDataPath)
        {
            _programDataPath = programDataPath;
        }

        internal string[] GetPackagesFolder(string dir) => Directory.GetDirectories(dir, PackagesDirName, SearchOption.TopDirectoryOnly);

        internal T_Result ReadDependencies(string solutionFolder)
        {
            var result = new T_Result();

            var packagesDirectory = DirectoryTools.SearchDirectory(solutionFolder, GetPackagesFolder);

            if (string.IsNullOrEmpty(packagesDirectory))
            {
                Log.Error("Cant fing packages folder in {solutionDirectory}. Aborting nuget reference scan", solutionFolder);
                return null;
            }

            foreach (var folder in Directory.GetDirectories(packagesDirectory))
            {
                var nuspec = Directory.GetFiles(folder, "*.nupkg", SearchOption.TopDirectoryOnly).FirstOrDefault();

                if (nuspec == null)
                {
                    Log.Error("Cant find nuspec in {source}", folder);
                    continue;
                }

                var package = new ZipPackage(nuspec);

                var id = package.Id;

                var version = package.Version;

                var dependencies = package.DependencySets;

                if (result.ContainsKey(id))
                {
                    result[id].Add(version, dependencies);
                }
                else
                {
                    var temp = new Dictionary<SemanticVersion, IEnumerable<PackageDependencySet>>()
                    {
                        {version, dependencies }
                    };

                    result.Add(id, temp);
                }
            }

            return result;
        }

        public IEnumerable<NugetReferenceResult> ScanNugetReferences(ProjectResult project)
        {
            var packagesPath = DirectoryTools.SearchDirectory(project.ProjectInfo.DirectoryName, a => Directory.GetDirectories(a, "packages", SearchOption.TopDirectoryOnly));

            if (packagesPath == string.Empty)
            {
                Log.Error("Cant find packages directory. Project: {Project}", project.ProjectInfo.DirectoryName);

                return Enumerable.Empty<NugetReferenceResult>();
            }

            // get all dependencies with no structure
            var allDependencies = ReadDependencies(packagesPath);

            // execute scan
            var result = new List<NugetReferenceResult>();

            foreach (var dep in project.References)
            {
                // add current dependency
                ResolveNugetDependency(dep, allDependencies, dep.ToString(), a => result.Add(a));

                result.Add(new NugetReferenceResult(project.ProjectInfo.Name, dep.ToString()) { color = "red" });
            }

            // generate report to programdata folder

            var test = JsonConvert.SerializeObject(result);

            Debug.WriteLine(test);

            return result;
        }

        public void ResolveNugetDependency(ProjectReference reference, T_Result input, string parent, Action<NugetReferenceResult> report)
        {
            if (input.ContainsKey(reference.Id))
            {
                SemanticVersion compatibleVersion = default(SemanticVersion);

                if (reference.Version == null)
                {
                    compatibleVersion = input[reference.Id].First().Key;
                }
                else
                {
                    compatibleVersion = input[reference.Id]
                        .Where(a => a.Key.Version.Major == reference.Version.Version.Major)
                        .Select(a => a.Key)
                        .OrderByDescending(a => a.Version)
                        .FirstOrDefault();
                }

                IEnumerable<PackageDependencySet> currentDependencies = input[reference.Id][compatibleVersion];

                PackageDependencySet compatibleFrameworkDependencies = default(PackageDependencySet);

                if (currentDependencies.Count() == 1 && currentDependencies.First().TargetFramework == null)
                // there are not specified target frameworks -> just use all available
                {
                    compatibleFrameworkDependencies = currentDependencies.First();
                }
                else
                {
                    var targetFrameworks = currentDependencies.Select(a => a.TargetFramework);

                    // Search for same framework with same version or less
                    var compatibleFramework = targetFrameworks.FindCompatibleFramework(reference.Framework);

                    // we will work with the found framework dependencies
                    compatibleFrameworkDependencies = currentDependencies.FirstOrDefault(a => a.TargetFramework == compatibleFramework);
                }

                if (compatibleFrameworkDependencies == null)
                {
                    return;
                }

                var castDependencies = compatibleFrameworkDependencies
                    .Dependencies
                    .Select(a =>
                    {
                        var version = a.VersionSpec?.MinVersion?.ToString();

                        if (version == null)
                        {
                            return new ProjectReference(a.Id, compatibleFrameworkDependencies.TargetFramework ?? reference.Framework);
                        }

                        return new ProjectReference(a.Id, a.VersionSpec?.MinVersion?.ToString(), compatibleFrameworkDependencies.TargetFramework ?? reference.Framework);
                    });

                foreach (var dep in castDependencies)
                {
                    report(new NugetReferenceResult(parent, dep.ToString()));

                    ResolveNugetDependency(dep, input, dep.ToString(), report);
                }

                return;
            }

            return;
        }

        public async Task<string> GetNuspec(string packageDirectory)
        {
            var nugpkgFilePath = Directory.GetFiles(packageDirectory, "*.nupkg", SearchOption.TopDirectoryOnly).FirstOrDefault();

            if (string.IsNullOrEmpty(nugpkgFilePath))
            {
                Log.Error("Cant find nuspec file in {packageDirectory}", packageDirectory);

                return string.Empty;
            }

            using (ZipArchive archive = ZipFile.OpenRead(nugpkgFilePath))
            {
                var nuspecZipFile = archive.Entries.Single(a => a.Name.Contains(".nuspec"));

                using (var nuspecStream = nuspecZipFile.Open())
                using (var streamReader = new StreamReader(nuspecStream))
                {
                    return await streamReader.ReadToEndAsync();
                }
            }
        }
    }

    public class NugetReferenceResult
    {
        public NugetReferenceResult(string source, string target)
        {
            this.source = source;
            this.target = target;
        }

        public string source { get; set; }
        public string target { get; set; }
        public string color { get; set; }
    }
}
