using DependencyScanner.Core.GitClient;
using DependencyScanner.Core.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using static DependencyScanner.Core.Test.TestTools;

namespace DependencyScanner.Core.Test.GitTests
{
    [TestClass]
    [Ignore]
    public class GitExamples
    {
        [TestMethod]
        public void MyTestMethod()
        {
            var Status = Git.GitProcess(Environment.CurrentDirectory, GitCommand.RemoteBranch);
        }

        [TestMethod]
        public void SuperTest()
        {
            var wd = @"F:\Projects\_GitHub\DependencyScanner\.git";

            var info = new GitInfo(wd, Git);

            //info.Checkout("mastesr");
        }
    }
}
