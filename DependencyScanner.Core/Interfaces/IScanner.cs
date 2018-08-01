using DependencyScanner.Core.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DependencyScanner.Core.Interfaces
{
    public interface IScanner
    {
        Task<IEnumerable<SolutionResult>> ScanSolutions(string rootDirectory, ICancelableProgress<ProgressMessage> progress);
        Task<SolutionResult> ScanSolution(string rootDirectory);
    }
}
