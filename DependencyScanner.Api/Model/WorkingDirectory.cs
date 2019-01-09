using DependencyScanner.Api.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DependencyScanner.Api.Model
{
    public class WorkingDirectory : IScannable
    {
        public ICollection<Repository> Solutions { get; set; }

        public void Sync(IScanner scanner)
        {
            throw new NotImplementedException();
        }
    }

    public class Repository : IScannable
    {
        public ICollection<Project> Projects { get; set; }

        public void Sync(IScanner scanner)
        {
            throw new NotImplementedException();
        }
    }

    public class Solution
    {
    }

    public class Project
    {
        public ICollection<ProjectReference> ProjectReferences { get; set; }
    }

    public class ProjectReference
    {
    }
}
