using NGit.Api;
using System.Collections.Generic;

namespace DependencyScanner.Core.Model
{
    public class GitStatus
    {
        public ICollection<string> Added { get; }
        public ICollection<string> Changed { get; }
        public ICollection<string> Modified { get; }
        public ICollection<string> Removed { get; }

        public GitStatus(Status status)
        {
            Added       = status.GetAdded();
            Changed     = status.GetChanged();
            Modified    = status.GetModified();
            Removed     = status.GetRemoved();
        }
    }
}