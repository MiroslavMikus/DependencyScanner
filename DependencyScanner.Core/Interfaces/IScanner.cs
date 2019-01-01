using DependencyScanner.Core.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DependencyScanner.Core.Interfaces
{
    public interface IScanner : IService
    {
        Task<IEnumerable<SolutionResult>> ScanSolutions(string rootDirectory, ICancelableProgress<ProgressMessage> progress, bool executeGitFetch);

        Task<SolutionResult> ScanSolution(string rootDirectory, ICancelableProgress<ProgressMessage> progress, bool executeGitFetch);
    }
}
