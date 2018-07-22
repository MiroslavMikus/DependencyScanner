using NuGet;
using System.Collections.Generic;
using System.Diagnostics;

namespace DependencyScanner.Core.Model
{
    [DebuggerDisplay("{Id}")]
    public class ConsolidateProject
    {
        public string Id { get; set; }
        public Dictionary<ProjectResult, SemanticVersion> References { get; set; }
    }
}