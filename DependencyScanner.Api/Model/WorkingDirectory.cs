using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DependencyScanner.Api.Model
{
    public class WorkingDirectory
    {
        public ICollection<Solution> Solutions { get; set; }
    }

    public class Solution
    {
        public ICollection<Project> Projects { get; set; }
    }

    public class Project
    {
        public ICollection<ProjectReference> ProjectReferences { get; set; }
    }

    public class ProjectReference
    {
    }
}
