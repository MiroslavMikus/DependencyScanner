using DependencyScanner.Api.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DependencyScanner.Api.Interfaces
{
    public interface IRepositoryScanner
    {
        Task<IEnumerable<IGitInfo>> ScanForGitRepositories(string rootDirectory, ICancelableProgress<ProgressMessage> progress);
    }
}
