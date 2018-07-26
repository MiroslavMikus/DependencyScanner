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
        public IEnumerable<string> BranchList { get; private set; }

        public GitInfo(string root)
        {
            Root = new FileInfo(root);

            var branches = GitEngine.GitProcess(root, GitCommand.BranchList);

            BranchList = GitParser.GetBranchList(branches);

            CurrentBranch = GitParser.GetCurrentBranch(branches);

            Status = GitEngine.GitProcess(root, GitCommand.Status);
        }
    }
}