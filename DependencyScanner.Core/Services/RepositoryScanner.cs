using DependencyScanner.Api.Interfaces;
using DependencyScanner.Api.Services;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DependencyScanner.Core.Services
{
    public class RepositoryScanner : IRepositoryScanner, IService
    {
        private const string GitPattern = ".git";
        private readonly Func<string, IGitInfo> _gitCtor;
        private readonly ILogger _logger;

        public RepositoryScanner(Func<string, IGitInfo> gitCtor, ILogger logger)
        {
            _gitCtor = gitCtor;
            _logger = logger;
        }

        private static string[] GetGitFolder(string dir) => Directory.GetDirectories(dir, GitPattern, SearchOption.AllDirectories);

        public async Task<IEnumerable<IGitInfo>> ScanForGitRepositories(string rootDirectory, IProgress<ProgressMessage> progress, bool executeGitFetch, CancellationToken token)
        {
            progress.Report(new ProgressMessage { Value = 0D, Message = "Searching for '.git' folders." });

            var gitFolders = GetGitFolder(rootDirectory);

            double Progress(int current) => Math.Round(current / (gitFolders.Count() / 100D), 2);

            _logger.Debug("found {folders}", gitFolders);

            var infos = gitFolders.Select(a => _gitCtor(a)).ToArray();

            List<Task> InitTasks = new List<Task>();

            for (int i = 0; i < gitFolders.Length; i++)
            {
                progress.Report(new ProgressMessage { Value = Progress(i + 1), Message = $"Scanning {i + 1}/{gitFolders.Count()}" });

                InitTasks.Add(infos[i].Init(executeGitFetch));
            }

            progress.Report(new ProgressMessage { Value = 0, Message = "Finishing scan" });

            var tcs = new TaskCompletionSource<bool>();

            token.Register(() =>
            {
                tcs.TrySetCanceled();
            });

            await Task.WhenAny(Task.WhenAll(InitTasks), tcs.Task);

            return infos;
        }
    }
}
