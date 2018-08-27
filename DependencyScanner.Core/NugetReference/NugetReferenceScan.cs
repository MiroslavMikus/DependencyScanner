using DependencyScanner.Core.Model;
using DependencyScanner.Core.Tools;
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

        /// <summary>
        /// Create temp directory, unzip and copy all nuspecs
        /// </summary>
        /// <param name="solutionFolters"></param>
        /// <returns></returns>
        internal string PrepareScan(string[] solutionFolters)
        {
            string targetPath = Path.Combine(_programDataPath, Path.GetRandomFileName());

            Directory.CreateDirectory(targetPath);

            foreach (var solutionDirectory in solutionFolters)
            {
                var packagesDirectory = DirectoryTools.SearchDirectory(solutionDirectory, GetPackagesFolder);

                if (string.IsNullOrEmpty(packagesDirectory))
                {
                    Log.Error("Cant fing packages folder in {solutionDirectory}. Aborting nuget reference scan", solutionDirectory);

                    continue;
                }

                CopyNuspecs(solutionDirectory, targetPath);
            }
            return targetPath;
        }

        /// <summary>
        /// Unzip and copy nuspec
        /// </summary>
        /// <param name="source">Package folder of the current directory</param>
        /// <param name="target">Target 'temp' folder </param>
        internal void CopyNuspecs(string source, string target)
        {
            foreach (var folder in Directory.GetDirectories(source))
            {
                CopyNuspec(folder, target);
            }
        }

        internal void CopyNuspec(string source, string target)
        {
            var nuspec = Directory.GetFiles(source, "*.nupkg", SearchOption.TopDirectoryOnly).FirstOrDefault();

            if (nuspec == null)
            {
                Log.Error("Cant find nuspec in {source}", source);
                return;
            }

            using (ZipArchive zip = ZipFile.Open(nuspec, ZipArchiveMode.Read))
            {
                var entry = zip.Entries.Single(a => a.Name.Contains("nuspec"));

                string version;
                using (var stream = entry.Open())
                using (var reader = new StreamReader(stream))
                {
                    version = ReadNuspecVerion(reader.ReadToEnd());
                }

                if (!string.IsNullOrEmpty(version))
                {
                    var fileName = Path.GetFileNameWithoutExtension(entry.Name) + "." + version + Path.GetExtension(entry.Name);
                }

                entry.ExtractToFile(Path.Combine(target, entry.Name), true);
            }
        }

        internal string ReadNuspecVerion(string content)
        {
            var doc = new XDocument(content);

            var version = doc.Element("package").Element("metadata").Element("version").Value;

            return version;
        }

        internal void CleanUpScan()
        {
        }

        public IEnumerable<NugetReferenceResult> ScanNugetReferences(IEnumerable<SolutionResult> input)
        {
            // copy all packages nuspecs to temp folder
            PrepareScan(input.Select(a => a.Info.FullName).ToArray());

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
