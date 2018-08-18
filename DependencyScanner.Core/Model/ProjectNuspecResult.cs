using System.Collections.Generic;

namespace DependencyScanner.Core.Model
{
    public class ProjectNuspecResult
    {
        public ProjectResult Project { get; set; }
        public IEnumerable<string> MissingPackages { get; set; }
        public IEnumerable<string> UselessPackages { get; set; }
    }
}
