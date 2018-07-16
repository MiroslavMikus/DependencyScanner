using System.Collections.Generic;
using System.IO;

namespace DependencyScanner.Core.Model
{
    public class SolutionResult
    {
        public FileInfo Info { get; }
        public ICollection<ProjectResult> Projects { get; } = new List<ProjectResult>();

        public SolutionResult(FileInfo info)
        {
            Info = info;
        }
    }
}