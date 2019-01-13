using DependencyScanner.Api.Interfaces;
using DependencyScanner.Core.FileScan;
using DependencyScanner.Core.Interfaces;
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
using System.Runtime.Versioning;

//                                                       id                                                version                                                            dependencies
using T_Result = System.Collections.Generic.Dictionary<string, System.Collections.Generic.Dictionary<NuGet.SemanticVersion, System.Collections.Generic.IEnumerable<NuGet.PackageDependencySet>>>;

namespace DependencyScanner.Core.NugetReference
{
    public class NugetReferenceScan : IService
    {
        private const string PackagesDirName = "packages";

        internal string[] GetPackagesFolder(string dir) => Directory.GetDirectories(dir, PackagesDirName, SearchOption.TopDirectoryOnly);

        internal T_Result ReadDependencies(string solutionFolder)
        {
            var result = new T_Result();

            var packagesDirectory = DirectoryTools.SearchDirectory(solutionFolder, GetPackagesFolder);

            if (string.IsNullOrEmpty(packagesDirectory))
            {
                Log.Error("Can't fing packages folder in {solutionDirectory}. Aborting nuget reference scan.", solutionFolder);
                return null;
            }

            foreach (var folder in Directory.GetDirectories(packagesDirectory))
            {
                var nuspec = Directory.GetFiles(folder, "*.nupkg", SearchOption.TopDirectoryOnly).FirstOrDefault();

                if (nuspec == null)
                {
                    Log.Warning("Can't find nuspec in {source}", folder);
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
            // get all dependencies with no structure
            var allDependencies = ReadDependencies(project.ProjectInfo.DirectoryName);

            if (allDependencies == null)
            {
                return null;
            }

            // execute scan
            var result = new List<NugetReferenceResult>();

            foreach (var dep in project.References)
            {
                // add current dependency
                ResolveProjectDependency(dep, allDependencies, a => result.Add(a));

                result.Add(new NugetReferenceResult(Path.GetFileNameWithoutExtension(project.ProjectInfo.Name), dep.ToString()) { color = "red" });
            }

            // generate report to programdata folder

            var test = JsonConvert.SerializeObject(result);

            Debug.WriteLine(JsonConvert.SerializeObject(result));

            return result;
        }

        internal void ResolveProjectDependency(ProjectReference reference, T_Result input, Action<NugetReferenceResult> report)
        {
            if (!input.ContainsKey(reference.Id)) return;

            SemanticVersion compatibleVersion = default(SemanticVersion);

            if (reference.Version == null)
            // Version is not specified -> just take the first one
            {
                compatibleVersion = input[reference.Id].First().Key;
            }
            else
            {
                compatibleVersion = reference.Version;
            }

            if (!input[reference.Id].ContainsKey(compatibleVersion))
            {
                Log.Error("Target package {package} - {version} is not installed. The nuget map will be incomplete. Try to build your solution and try again.", reference.Id, compatibleVersion);

                return;
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
                compatibleFrameworkDependencies = FindCompatibleFramework(currentDependencies, reference.Framework);
            }

            if (compatibleFrameworkDependencies == null)
            {
                return;
            }

            IEnumerable<NugetDefinition> castDependencies = CastDependencies(input, compatibleFrameworkDependencies);

            ReportDependencies(castDependencies, input, report, reference.ToString());
        }

        internal void ResolveNugetDependency(NugetDefinition nugetDependency, T_Result input, Action<NugetReferenceResult> report)
        {
            if (!input.ContainsKey(nugetDependency.Id)) return;

            SemanticVersion compatibleVersion = default(SemanticVersion);

            if (input[nugetDependency.Id].ContainsKey(nugetDependency.CurrentVersion))
            // Version was specified and packages folder contains the desired version
            {
                compatibleVersion = nugetDependency.CurrentVersion;
            }
            else
            // Search for compatible framework version. Since target and installed version can differ from each other.
            {
                compatibleVersion = input[nugetDependency.Id]
                    .Where(a => a.Key.Version.Major == nugetDependency.CurrentVersion.Version.Major)
                    .Select(a => a.Key)
                    .OrderByDescending(a => a.Version)
                    .FirstOrDefault();
            }

            IEnumerable<PackageDependencySet> currentDependencies = input[nugetDependency.Id][compatibleVersion];

            PackageDependencySet compatibleFrameworkDependencies = default(PackageDependencySet);

            if (currentDependencies.Count() == 1 && currentDependencies.First().TargetFramework == null
                || nugetDependency.Framework == null)
            // there are not specified target frameworks -> just use all available
            {
                compatibleFrameworkDependencies = currentDependencies.First();
            }
            else
            {
                compatibleFrameworkDependencies = FindCompatibleFramework(currentDependencies, nugetDependency.Framework);
            }

            if (compatibleFrameworkDependencies == null)
            {
                return;
            }

            IEnumerable<NugetDefinition> castDependencies = CastDependencies(input, compatibleFrameworkDependencies);

            ReportDependencies(castDependencies, input, report, nugetDependency.ToString());
        }

        private PackageDependencySet FindCompatibleFramework(IEnumerable<PackageDependencySet> currentDependencies, FrameworkName desiredFramework)
        {
            var targetFrameworks = currentDependencies.Select(a => a.TargetFramework);

            // Search for same framework with same version or less
            var compatibleFramework = targetFrameworks.FindCompatibleFramework(desiredFramework);

            // we will work with the found framework dependencies
            return currentDependencies.FirstOrDefault(a => a.TargetFramework == compatibleFramework);
        }

        private void ReportDependencies(IEnumerable<NugetDefinition> input, T_Result data, Action<NugetReferenceResult> report, string parent)
        {
            foreach (var dep in input)
            {
                // Report
                report(new NugetReferenceResult(parent, dep.ToString()));

                if (dep.Dependencies != null)
                // start recrusion here
                {
                    ResolveNugetDependency(dep, data, report);
                }
            }
        }

        private static IEnumerable<NugetDefinition> CastDependencies(T_Result input, PackageDependencySet compatibleFrameworkDependencies)
        {
            return compatibleFrameworkDependencies
                .Dependencies
                .Select(a =>
                {
                    if (!input.ContainsKey(a.Id)) return null;

                    var version = input[a.Id].Select(b => b.Key).First(b => a.VersionSpec.Satisfies(b));

                    PackageDependencySet dep = default(PackageDependencySet);

                    if (input[a.Id][version].Count() == 1 && compatibleFrameworkDependencies.TargetFramework == null)
                    {
                        dep = input[a.Id][version].First();
                    }
                    else
                    {
                        dep = input[a.Id][version].FirstOrDefault(b => b.TargetFramework == compatibleFrameworkDependencies.TargetFramework);
                    }

                    return new NugetDefinition()
                    {
                        Id = a.Id,
                        CurrentVersion = version,
                        Dependencies = dep
                    };
                }).Where(a => a != null);
        }
    }
}
