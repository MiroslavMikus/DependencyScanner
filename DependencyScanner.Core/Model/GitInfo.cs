using NGit.Api;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DependencyScanner.Core.Model
{
    public class GitInfo
    {
        public FileInfo Root { get; }
        public string CurrentBranch { get; private set; }
        public IEnumerable<string> BranchList { get; private set; }

        public GitInfo(string root)
        {
            Root = new FileInfo(root);

            var git = GetGit();

            BranchList = GetBranches(git);

            var repo = git.GetRepository();

            CurrentBranch = repo.GetBranch();
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

        private IEnumerable<string> GetBranches(Git git)
        {
            ListBranchCommand command = git.BranchList();

            var result = command.Call();

            return result.Select(a => a.GetName()).Where(a => !a.Contains("remote"));
        }
    }
}