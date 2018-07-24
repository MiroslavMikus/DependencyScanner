using NGit.Api;
using System.Collections.Generic;

namespace DependencyScanner.Core.Model
{
    public class GitStatus
    {
        public ICollection<string> Added { get; }
        public ICollection<string> Changed { get; }
        public ICollection<string> Conflicting { get; }
        public ICollection<string> Missing { get; }
        public ICollection<string> Modified { get; }
        public ICollection<string> Removed { get; }
        public ICollection<string> Untracked { get; }

        public GitStatus(Status status)
        {
            Added       = status.GetAdded();
            Changed     = status.GetChanged();
            Conflicting = status.GetConflicting();
            Missing     = status.GetMissing();
            Modified    = status.GetModified();
            Removed     = status.GetRemoved();
            Untracked   = status.GetUntracked();
        }
    }
}