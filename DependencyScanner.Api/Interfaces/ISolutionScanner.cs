using DependencyScanner.Api.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DependencyScanner.Api.Interfaces
{
    public interface ISolutionScanner : IService
    {
        Task<IWorkingDirectory> ScanWorkingDirectory(string rootDirectory, ICancelableProgress<ProgressMessage> progress, bool executeGitFetch);

        Task<Solution> ScanSolution(string rootDirectory, ICancelableProgress<ProgressMessage> progress, bool executeGitFetch);
    }

    public interface IRepositoryScanner
    {
        Task<IEnumerable<IGitInfo>> ScanForGitRepositories(string rootDirectory, ICancelableProgress<ProgressMessage> progress);
    }

    public class ProgressMessage
    {
        public double Value { get; set; }
        public string Message { get; set; }
    }
}
