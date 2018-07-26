using DependencyScanner.Core.GitClient;
using DependencyScanner.Core.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DependencyScanner.Core.Test.GitTests
{
    [TestClass]
    [Ignore]
    public class GitExamples
    {
        [TestMethod]
        public void MyTestMethod()
        {
            var Status = GitEngine.GitProcess(Environment.CurrentDirectory, GitCommand.RemoteBranch);
        }

        [TestMethod]
        public void SuperTest()
        {
            var wd = @"F:\Projects\_GitHub\DependencyScanner\.git";

            var info = new GitInfo(wd);

            //info.Checkout("mastesr");
        }
    }
}
