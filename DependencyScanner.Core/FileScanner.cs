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
        string[] GetPackages(string rootDirectory) => Directory.GetFiles(rootDirectory, "packages.config", SearchOption.AllDirectories);
        string[] GetSolutions(string rootDirectory) => Directory.GetFiles(rootDirectory, " *.sln", SearchOption.TopDirectoryOnly);
        string[] GetGetProjects(string rootDirectory) => Directory.GetFiles(rootDirectory, "*.csproj", SearchOption.TopDirectoryOnly);

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

                var projectPath = Directory.GetFiles(packageInfo.DirectoryName, "*.csproj", SearchOption.TopDirectoryOnly);

                var projectInfo = new FileInfo(projectPath[0]);

                var projectResult = new ProjectResult(packageInfo, projectInfo);

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
    }
}
