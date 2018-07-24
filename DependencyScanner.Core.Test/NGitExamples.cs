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
    public class NGitExamples
    {
        [TestMethod]
        public void NGit_GetBranch()
        {
            var git = Git.Open(@"F:\Projects\_GitHub\DependencyScanner");

            ListBranchCommand command = git.BranchList();

            var result = command.Call();
        }
    }
}
