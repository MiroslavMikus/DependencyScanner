using DependencyScanner.Api.Model;
using DependencyScanner.Api.Services;
using System.Threading.Tasks;

namespace DependencyScanner.Api.Interfaces
{
    public interface ISolutionScanner : IService
    {
        Task<IWorkingDirectory> ScanWorkingDirectory(string rootDirectory, ICancelableProgress<ProgressMessage> progress, bool executeGitFetch);

        Task<Solution> ScanSolution(string rootDirectory, ICancelableProgress<ProgressMessage> progress, bool executeGitFetch);
    }
}
