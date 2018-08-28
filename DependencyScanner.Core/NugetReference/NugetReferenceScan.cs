using DependencyScanner.Core.Model;
using DependencyScanner.Core.Tools;
using NuGet;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

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

        internal Dictionary<string, Dictionary<string, IEnumerable<PackageDependencySet>>> ReadDependencies(string solutionFolder)
        {
            //                          id                version              dependencies
            var result = new Dictionary<string, Dictionary<string, IEnumerable<PackageDependencySet>>>();

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

                var version = package.Version.ToString();

                var dependencies = package.DependencySets;

                if (result.ContainsKey(id))
                {
                    result[id].Add(version, dependencies);
                }
                else
                {
                    var temp = new Dictionary<string, IEnumerable<PackageDependencySet>>()
                    {
                        {version, dependencies }
                    };

                    result.Add(id, temp);
                }
            }

            return result;
        }

        public IEnumerable<NugetReferenceResult> ScanNugetReferences(IEnumerable<SolutionResult> input)
        {
            // copy all packages nuspecs to temp folder

            // execute scan

            // generate report

            // delete temp folder

            //var packagesDirectories = Directory.GetDirectories(packagesDirectory);

            //var ScanResult = new Dictionary<string, IEnumerable<NugetReferenceResult>>();

            //foreach (var reference in input)
            //{
            //    if (ScanResult.ContainsKey(reference.Id))
            //    {
            //        // already done
            //        continue;
            //    }

            //    ScanResult.Add(reference.Id, ResolveNugetDependency(reference.Id, a => packagesDirectories.FirstOrDefault(b => b.Contains(a))));
            //}

            //return ScanResult.SelectMany(a => a.Value);

            return null;
        }

        public IEnumerable<NugetReferenceResult> ResolveNugetDependency(string packageDir, Func<string, string> getPackageFolder)
        {
            return null;
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
        public string source { get; set; }
        public string target { get; set; }
    }
}
