using DependencyScanner.Core.Model;
using System.Collections.Generic;

namespace DependencyScanner.Core.Interfaces
{
    public interface IScanner
    {
        IEnumerable<SolutionResult> ScanMultipleDirectories(IEnumerable<string> directores, ICancelableProgress<ProgressMessage> progress);
        IEnumerable<SolutionResult> ScanSolutions(string rootDirectory, ICancelableProgress<ProgressMessage> progress);
        SolutionResult ScanSolution(string rootDirectory);
    }
}
