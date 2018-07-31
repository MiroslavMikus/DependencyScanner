using DependencyScanner.Core.GitClient;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DependencyScanner.Core.Model
{
    public class GitInfo
    {
        public FileInfo Root { get; }
        public string CurrentBranch { get; private set; }
        public string Status { get; private set; }
        public string RemoteUrl { get; private set; }
        public IEnumerable<string> BranchList { get; private set; }
        public bool IsClean { get => Status.Contains("working tree clean"); }
        public bool IsBehind { get => Status.Contains("Your branch is behind"); }

        public GitInfo(string root)
        {
            Root = new FileInfo(root);

            if (FileScanner.ExecuteGitFetchWitScan)
            {
                GitEngine.GitProcess(Root.DirectoryName, GitCommand.UpdateRemote);
            }

            var branches = GitEngine.GitProcess(Root.DirectoryName, GitCommand.BranchList);

            BranchList = GitParser.GetBranchList(branches);

            CurrentBranch = GitParser.GetCurrentBranch(branches);

            Status = GitEngine.GitProcess(Root.DirectoryName, GitCommand.Status);

            RemoteUrl = GitEngine.GitExecute(Root.DirectoryName, GitCommand.RemoteBranch, a => GitParser.GetRemoteUrl(a));
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