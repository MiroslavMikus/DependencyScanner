using NGit.Api;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DependencyScanner.Core.Model
{
    public class GitInfo
    {
        public FileInfo Root { get; }
        public GitStatus Status { get; private set; }
        public string CurrentBranch { get; private set; }
        public IEnumerable<string> BranchList { get; private set; }

        public GitInfo(string root)
        {
            Root = new FileInfo(root);

            var git = GetGit();

            BranchList = GetBranches(git);

            //UpdateStatus(git);

            var repo = git.GetRepository();

            CurrentBranch = repo.GetBranch();
        }

        public void UpdateStatus()
        {
            UpdateStatus(GetGit());
        }

        public void Checkout(string branch)
        {
            if (BranchList.Contains(branch))
            {
                var git = GetGit();

                CheckoutCommand command = git.Checkout();

                command.SetName(branch);

                command.Call();

                CurrentBranch = branch;
            }
        }

        private Git GetGit() => Git.Open(Root.DirectoryName);
        private void UpdateStatus(Git git)
        {
            var repo = git.GetRepository();

            Status status = git.Status().Call();

            Status = new GitStatus(status);
        }

        private IEnumerable<string> GetBranches(Git git)
        {
            ListBranchCommand command = git.BranchList();

            var result = command.Call();

            return result.Select(a => a.GetName()).Where(a => !a.Contains("remote"));
        }
    }
}