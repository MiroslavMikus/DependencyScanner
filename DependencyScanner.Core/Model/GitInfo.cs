using DependencyScanner.Core.GitClient;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DependencyScanner.Core.Model
{
    public class GitInfo : ObservableObject
    {
        public FileInfo Root { get; }
        public string RemoteUrl { get; private set; }
        public IEnumerable<string> BranchList { get; private set; }
        public bool IsClean { get => Status.Contains("working tree clean"); }
        public bool IsBehind { get => Status.Contains("Your branch is behind"); }

        private string _currentBranch;
        public string CurrentBranch
        {
            get => _currentBranch;
            set
            {
                if (Set(ref _currentBranch, value))
                {
                    Task.Run(() => 
                    {
                        Checkout(value);
                        Status = GitEngine.GitProcess(Root.DirectoryName, GitCommand.Status);
                    });
                }
            }
        }

        private string _status;
        public string Status
        {
            get => _status;
            private set
            {
                Set(ref _status, value);

                RaisePropertyChanged(nameof(IsClean));
                RaisePropertyChanged(nameof(IsBehind));
            }
        }

        public RelayCommand PullCommand { get; }

        public GitInfo(string root)
        {
            Root = new FileInfo(root);

            if (FileScanner.ExecuteGitFetchWitScan)
            {
                GitEngine.GitProcess(Root.DirectoryName, GitCommand.UpdateRemote);
            }

            var branches = GitEngine.GitProcess(Root.DirectoryName, GitCommand.BranchList);

            BranchList = GitParser.GetBranchList(branches);

            _currentBranch = GitParser.GetCurrentBranch(branches);

            Status = GitEngine.GitProcess(Root.DirectoryName, GitCommand.Status);

            RemoteUrl = GitEngine.GitExecute(Root.DirectoryName, GitCommand.RemoteBranch, a => GitParser.GetRemoteUrl(a));

            PullCommand = new RelayCommand(() =>
            {
                Task.Run(() =>
                {
                    Pull();
                    Status = GitEngine.GitProcess(Root.DirectoryName, GitCommand.Status);
                });
            });
        }

        public string Checkout(string branch)
        {
            if (BranchList.Contains(branch))
            {
                return GitEngine.GitProcess(Root.DirectoryName, GitCommand.SwitchBranch, branch);
            }

            return $"Branch '{branch}' doesnt exist in scan results!";
        }

        public string Pull()
        {
            return GitEngine.GitProcess(Root.DirectoryName, GitCommand.Pull);
        }
    }
}