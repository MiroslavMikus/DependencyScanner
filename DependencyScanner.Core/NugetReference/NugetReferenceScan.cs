using DependencyScanner.Core.Model;
using DependencyScanner.Core.Tools;
using NuGet;
using Serilog;
using System.Collections.Generic;
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

        public IEnumerable<NugetReferenceResult> ScanNugetReferences(IEnumerable<ProjectReference> References, string solutionFolder, string rootLabel)
        {
            // get all dependencies with no structure
            var allDependencies = ReadDependencies(solutionFolder);

            // execute scan

            // generate report to programdata folder

            return null;
        }

        public IEnumerable<NugetReferenceResult> ResolveNugetDependency(T_Result input, string rootLabel)
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
