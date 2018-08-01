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

        private string _remoteUrl;
        public string RemoteUrl { get => _remoteUrl; private set => Set(ref _remoteUrl, value); }

        private IEnumerable<string> _branchList;
        public IEnumerable<string> BranchList { get => _branchList; private set => Set(ref _branchList, value); }

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
        public bool IsClean { get => Status?.Contains("working tree clean") == true; }
        public bool IsBehind { get => Status?.Contains("Your branch is behind") == true; }

        public GitInfo(string root)
        {
            Root = new FileInfo(root);

            PullCommand = new RelayCommand(() =>
            {
                Task.Run(() =>
                {
                    Pull();
                    Status = GitEngine.GitProcess(Root.DirectoryName, GitCommand.Status);
                });
            });
        }

        internal async Task Init()
        {
            await Task.Run(() =>
            {
                if (FileScanner.ExecuteGitFetchWithScan)
                {
                    var result = GitEngine.GitProcess(Root.DirectoryName, GitCommand.UpdateRemote);
                }

                var branches = GitEngine.GitProcess(Root.DirectoryName, GitCommand.BranchList);

                BranchList = GitParser.GetBranchList(branches);

                _currentBranch = GitParser.GetCurrentBranch(branches);

                RaisePropertyChanged(nameof(CurrentBranch));

                Status = GitEngine.GitProcess(Root.DirectoryName, GitCommand.Status);

                RemoteUrl = GitEngine.GitExecute(Root.DirectoryName, GitCommand.RemoteBranch, a => GitParser.GetRemoteUrl(a));
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