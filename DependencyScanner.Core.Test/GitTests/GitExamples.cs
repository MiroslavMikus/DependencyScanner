using DependencyScanner.Core.GitClient;
using DependencyScanner.Core.Model;
using DependencyScanner.Core.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace DependencyScanner.Core.Test.GitTests
{
    [TestClass]
    [Ignore]
    public class GitExamples : TestBase
    {
        [TestMethod]
        public void MyTestMethod()
        {
            var Status = TestBase.Git.GitProcess(Environment.CurrentDirectory, GitCommand.RemoteBranch);
        }

        [TestMethod]
        public void SuperTest()
        {
            var wd = @"F:\Projects\_GitHub\DependencyScanner\.git";

            var info = new GitInfo(wd, TestBase.Git, new HasInternetConnection(), _logger);

            //info.Checkout("mastesr");
        }
    }
}
