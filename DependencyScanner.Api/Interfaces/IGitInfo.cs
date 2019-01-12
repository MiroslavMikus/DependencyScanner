using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Threading.Tasks;

namespace DependencyScanner.Api.Interfaces
{
    public interface IGitInfo : INotifyPropertyChanged
    {
        IEnumerable<string> BranchList { get; }
        IGitConfig Config { get; set; }
        string CurrentBranch { get; set; }
        bool IsBehind { get; }
        bool IsClean { get; }
        string RemoteUrl { get; }
        FileInfo Root { get; }
        string Status { get; }

        string Checkout(string branch);

        Task Init(bool executeGitFetch);

        void Pull();
    }
}
