using Microsoft.VisualStudio.TestTools.UnitTesting;
using NGit;
using NGit.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DependencyScanner.Core.Test
{
    [TestClass]
    public class NgitTest
    {
        [TestMethod]
        public void Ngit_Test()
        {
            var repo = Git.Open(@"F:\Projects\_GitHub\DependencyScanner").GetRepository();
            //ListBranchCommand
            var test = repo.GetBranch();
            var test1 = repo.GetConfig();
            var test2 = repo.GetRepositoryState();
            var test3 = repo.GetFullBranch();
        }
    }
}
