using DependencyScanner.Core.Model;
using DependencyScanner.Core.Tools;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DependencyScanner.Core.NugetReference
{
    public class NugetReferenceScan
    {
        private const string PackagesDirName = "packages";

        private string[] GetPackagesFolder(string dir) => Directory.GetDirectories(dir, PackagesDirName, SearchOption.TopDirectoryOnly);

        public IEnumerable<NugetReferenceResult> ScanNugetReferences(IEnumerable<ProjectReference> input, string solutionDirectory)
        {
            var packagesDirectory = DirectoryTools.SearchDirectory(solutionDirectory, GetPackagesFolder);

            if (string.IsNullOrEmpty(packagesDirectory))
            {
                Log.Error("Cant fing packages folder in {solutionDirectory}. Aborting nuget reference scan", solutionDirectory);

                return Enumerable.Empty<NugetReferenceResult>();
            }

            var packagesDirectories = Directory.GetDirectories(packagesDirectory);

            var ScanResult = new Dictionary<string, IEnumerable<NugetReferenceResult>>();

            foreach (var reference in input)
            {
                if (ScanResult.ContainsKey(reference.Id))
                {
                    // already done
                    continue;
                }

                ScanResult.Add(reference.Id, ResolveNugetDependency(reference.Id, a => packagesDirectories.FirstOrDefault(b => b.Contains(a))));
            }

            return ScanResult.SelectMany(a => a.Value);
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
