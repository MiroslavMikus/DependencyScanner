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
        private const string NuspecPattern = "*.nuspec";
        private const string GitPattern = ".git";


        string[] GetPackages(string rootDirectory) => Directory.GetFiles(rootDirectory, PackagePattern, SearchOption.TopDirectoryOnly);
        string[] GetSolutions(string rootDirectory) => Directory.GetFiles(rootDirectory, SolutionPattern, SearchOption.AllDirectories);
        string[] GetProjects(string rootDirectory) => Directory.GetFiles(rootDirectory, ProjectPattern, SearchOption.AllDirectories);
        string[] GetNuspec(string rootDirectory) => Directory.GetFiles(rootDirectory, NuspecPattern, SearchOption.TopDirectoryOnly);
        string[] GetGitFolder(DirectoryInfo dir) => Directory.GetDirectories(dir.FullName, GitPattern, SearchOption.TopDirectoryOnly);


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

            foreach (var projectPath in GetProjects(result.Info.DirectoryName))
            {
                var projectInfo = new FileInfo(projectPath);

                var packagePaths = GetPackages(projectInfo.DirectoryName);

                ProjectResult projectResult;

                if (packagePaths.Count() != 0)
                // Projects contains package.config file
                {
                    var packageInfo = new FileInfo(packagePaths.First());

                    projectResult = new ProjectResult(projectInfo, packageInfo);

                    var file = new PackageReferenceFile(packageInfo.FullName);

                    projectResult.References.AddRange(file.GetPackageReferences());
                }
                else
                {
                    projectResult = new ProjectResult(projectInfo);
                }

                var nuspecInfo = GetNuspec(projectInfo.DirectoryName).FirstOrDefault();

                if (!string.IsNullOrEmpty(nuspecInfo))
                {
                    projectResult.NuspecInfo = new FileInfo(nuspecInfo);
                }

                result.Projects.Add(projectResult);
            }

            var gitPath = SearchGit(solution);

            if (!string.IsNullOrEmpty(gitPath))
            {
                result.GitInformation = new GitInfo(gitPath);
            }

            return result;
        }

        public IEnumerable<SolutionResult> ScanSolutions(string rootDirectory, ICancelableProgress<ProgressMessage> progress)
        {
            progress.Report(new ProgressMessage { Value = 0D, Message = "Searching for solutions" });
            var solutions = GetSolutions(rootDirectory);

            double Progress(int current) => Math.Round(current / (solutions.Count() / 100D), 2);

            for (int i = 0; i < solutions.Length; i++)
            {
                string solution = solutions[i];

                progress.Report(new ProgressMessage { Value = Progress(i + 1), Message = $"Scanning {i + 1}/{solutions.Count()}" });

                if (progress.Token.IsCancellationRequested)
                {
                    throw new OperationCanceledException("Operation was canceled by user");
                }

                yield return ExecuteSolutionScan(solution);
            }
        }

        public IEnumerable<SolutionResult> ScanMultipleDirectories(IEnumerable<string> directores, ICancelableProgress<ProgressMessage> progress)
        {
            foreach (var directory in directores)
            {
                foreach (var result in ScanSolutions(directory, progress))
                {
                    yield return result;
                }
            }
        }

        private string SearchGit(string directory)
        {
            var current = Directory.GetParent(directory);

            while (true)
            {
                if (current == null)
                {
                    return string.Empty;
                }

                var folders = GetGitFolder(current);

                if (folders.Count() == 0)
                {
                    current = current.Parent;
                }
                else
                {
                    return folders.First();
                }
            }
        }
    }
}
