using DependencyScanner.Api.Interfaces;
using DependencyScanner.Api.Services;
using DependencyScanner.Core.GitClient;
using DependencyScanner.Core.Interfaces;
using DependencyScanner.Core.Model;
using DependencyScanner.Core.Tools;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DependencyScanner.Core
{
    public class FileScanner : IScanner, ISolutionScanner
    {
        private readonly GitEngine _gitEngine;

        private const string PackagePattern = "packages.config";
        private const string SolutionPattern = "*.sln";
        private const string ProjectPattern = "*.csproj";
        private const string NuspecPattern = "*.nuspec";
        private const string GitPattern = ".git";

        public FileScanner(GitEngine gitEngine)
        {
            _gitEngine = gitEngine;
        }

        private static string[] GetPackages(string rootDirectory) => Directory.GetFiles(rootDirectory, PackagePattern, SearchOption.TopDirectoryOnly);

        private static string[] GetSolutions(string rootDirectory) => Directory.GetFiles(rootDirectory, SolutionPattern, SearchOption.AllDirectories);

        private static string[] GetProjects(string rootDirectory) => Directory.GetFiles(rootDirectory, ProjectPattern, SearchOption.AllDirectories);

        private static string[] GetNuspec(string rootDirectory) => Directory.GetFiles(rootDirectory, NuspecPattern, SearchOption.TopDirectoryOnly);

        private static string[] GetGitFolder(string dir) => Directory.GetDirectories(dir, GitPattern, SearchOption.TopDirectoryOnly);

        public async Task<SolutionResult> ScanSolution(string rootDirectory, ICancelableProgress<ProgressMessage> progress, bool executeGitFetch)
        {
            var solutions = GetSolutions(rootDirectory);

            if (!solutions.Any())
            {
                var ex = new ArgumentException("No solutions were found");

                Log.Error(ex, "Exception in ScanSolution");

                throw ex;
            }

            return await ExecuteSolutionScan(solutions.First(), progress, executeGitFetch);
        }

        private async Task<SolutionResult> ExecuteSolutionScan(string solutionPath, ICancelableProgress<ProgressMessage> progress, bool executeGitFetch)
        {
            var result = new SolutionResult(new FileInfo(solutionPath), this);

            foreach (var projectPath in GetProjects(result.Info.DirectoryName))
            {
                if (progress.Token.IsCancellationRequested)
                {
                    Log.Information("Cancelling scan");

                    throw new OperationCanceledException("Operation was canceled by user");
                }

                var projectInfo = new FileInfo(projectPath);

                var packagePaths = GetPackages(projectInfo.DirectoryName);

                ProjectResult projectResult;

                if (packagePaths.Count() != 0)
                // Projects contains package.config file
                {
                    var packageInfo = new FileInfo(packagePaths.First());

                    projectResult = new ProjectResult(projectInfo, packageInfo);
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

            var gitPath = DirectoryTools.SearchDirectory(solutionPath, GetGitFolder);

            if (!string.IsNullOrEmpty(gitPath))
            {
                result.GitInformation = new GitInfo(gitPath, _gitEngine);

                await result.GitInformation.Init(executeGitFetch);
            }

            return result;
        }

        public async Task<IEnumerable<SolutionResult>> ScanSolutions(string rootDirectory, ICancelableProgress<ProgressMessage> progress, bool executeGitFetch)
        {
            progress.Report(new ProgressMessage { Value = 0D, Message = "Searching for solutions" });

            var solutions = GetSolutions(rootDirectory);

            double Progress(int current) => Math.Round(current / (solutions.Count() / 100D), 2);

            List<Task<SolutionResult>> SolutionsTask = new List<Task<SolutionResult>>();

            for (int i = 0; i < solutions.Length; i++)
            {
                string solution = solutions[i];

                progress.Report(new ProgressMessage { Value = Progress(i + 1), Message = $"Scanning {i + 1}/{solutions.Count()}" });

                if (progress.Token.IsCancellationRequested)
                {
                    Log.Information("Cancelling scan");

                    throw new OperationCanceledException("Operation was canceled by user");
                }

                SolutionsTask.Add(ExecuteSolutionScan(solution, progress, executeGitFetch));
            }

            progress.Report(new ProgressMessage { Value = 0, Message = "Finishing scan" });

            await Task.WhenAll(SolutionsTask);

            return SolutionsTask.Select(a => a.Result);
        }

        public async Task<SolutionResult> ScanSolution(string rootDirectory)
        {
            var solutions = GetSolutions(rootDirectory);

            if (!solutions.Any())
            {
                return null;
            }

            var solutionPath = solutions.First();

            var result = new SolutionResult(new FileInfo(solutionPath), this);

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

            var gitPath = DirectoryTools.SearchDirectory(solutionPath, GetGitFolder);

            if (!string.IsNullOrEmpty(gitPath))
            {
                result.GitInformation = new GitInfo(gitPath, _gitEngine);

                await result.GitInformation.Init(false);
            }

            return result;
        }
    }
}
