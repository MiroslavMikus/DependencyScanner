using NuGet;
using System.Collections.Generic;

namespace DependencyScanner.Core.Model
{
    public class ConsolidateReference
    {
        public string Id { get; set; }
        public Dictionary<SolutionResult, SemanticVersion> References { get; set; }
    }
}