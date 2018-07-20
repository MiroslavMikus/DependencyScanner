using DependencyScanner.Core.Interfaces;
using DependencyScanner.Core.Model;
using NuGet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DependencyScanner.Core
{
    public class FileScanner : IScanner
    {
        private const string PackagePattern = "packages.config";
        private const string SolutionPattern = "*.sln";
        private const string ProjectPattern = "*.csproj";

        string[] GetPackages(string rootDirectory) => Directory.GetFiles(rootDirectory, PackagePattern, SearchOption.AllDirectories);
        string[] GetSolutions(string rootDirectory) => Directory.GetFiles(rootDirectory, SolutionPattern, SearchOption.AllDirectories);
        string[] GetProjects(string rootDirectory) => Directory.GetFiles(rootDirectory, "*.csproj", SearchOption.TopDirectoryOnly);

        public SolutionResult ScanSolution(string rootDirectory)
        {
            var solutions = GetSolutions(rootDirectory);

            if (solutions.Count() != 1)
            {
                throw new ArgumentException("There should be exactly one solution. Currently found: " + solutions.Count());
            }

            return ExecuteSolutionScan(solutions[0]);
        }

        private SolutionResult ExecuteSolutionScan(string solution)
        {
            var result = new SolutionResult(new FileInfo(solution));

            foreach (var packagePath in GetPackages(result.Info.DirectoryName))
            {
                var packageInfo = new FileInfo(packagePath);

                var projectPath = GetProjects(packageInfo.DirectoryName);

                var projectInfo = new FileInfo(projectPath[0]);

                var projectResult = new ProjectResult(projectInfo, packageInfo);

                var file = new PackageReferenceFile(packageInfo.FullName);

                projectResult.References.AddRange(file.GetPackageReferences());

                result.Projects.Add(projectResult);
            }

            return result;
        }

        public IEnumerable<SolutionResult> ScanSolutions(string rootDirectory)
        {
            var solutions = GetSolutions(rootDirectory);

            foreach (var solution in solutions)
            {
                yield return ExecuteSolutionScan(solution);
            }
        }

        public IEnumerable<SolutionResult> ScanMultipleDirectories(IEnumerable<string> directores)
        {
            foreach (var directory in directores)
            {
                foreach (var result in ScanSolutions(directory))
                {
                    yield return result;
                }
            }
        }
    }
}
