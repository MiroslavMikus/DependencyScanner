using DependencyScanner.Core.Model;
using System.Collections.Generic;

namespace DependencyScanner.Core.Interfaces
{
    public interface IScanner
    {
        IEnumerable<SolutionResult> ScanMultipleDirectories(IEnumerable<string> directores);
        IEnumerable<SolutionResult> ScanSolutions(string rootDirectory);
        SolutionResult ScanSolution(string rootDirectory);
    }
}
