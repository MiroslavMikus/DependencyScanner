using DependencyScanner.Api.Services;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DependencyScanner.Api.Interfaces
{
    public interface IRepositoryScanner
    {
        Task<IEnumerable<IGitInfo>> ScanForGitRepositories(string rootDirectory, IProgress<ProgressMessage> progress, bool executeGitFetch, CancellationToken token);
        IEnumerable<IGitInfo> ScanForGitRepositories(string rootDirectory);
    }
}
