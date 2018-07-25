using LibGit2Sharp;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DependencyScanner.Core.Test
{
    [TestClass]
    public class GitLibExample
    {
        private const string Path = "F:\\Projects\\_GitHub\\DependencyScanner";

        [TestMethod]
        public void Git_BranchList()
        {
            using (var repo = new Repository(Path))
            {
                var branches = repo.Branches;
                var Tags = repo.Tags;
                var currentBranch = repo.Head;

                var status = repo.RetrieveStatus();

                //Branch newBranch = repo.CreateBranch("dev/branch");

                //repo.Checkout(branches["master"]);

                PullOptions pullOptions = new PullOptions()
                {
                    MergeOptions = new MergeOptions()
                    {
                        FastForwardStrategy = FastForwardStrategy.Default
                    }
                };

                //Commands.Pull(repo, new Signature()
            }
        }
    }
}
