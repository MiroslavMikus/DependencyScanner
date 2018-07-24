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
    [Ignore]
    [TestClass]
    public class NGitExamples
    {
        [TestMethod]
        public void NGit_GetBranch()
        {
            var git = Git.Open(@"F:\Projects\_GitHub\DependencyScanner");

            ListBranchCommand command = git.BranchList();

            var result = command.Call();

            var localNames = result.Select(a => a.GetName()).Where(a => !a.Contains("remote")).ToList();
        }

        [TestMethod]
        public void NGit_CurrentBranch()
        {
            var git = Git.Open(@"F:\Projects\_GitHub\DependencyScanner");

            var repo = git.GetRepository();

            var branch = repo.GetBranch();
        }

        [TestMethod]
        public void NGit_CreateBranch()
        {
            var git = Git.Open(@"F:\Projects\_GitHub\DependencyScanner");

            CreateBranchCommand command = git.BranchCreate();

            command.SetName("dev/SomeTest");

            var result = command.Call();
        }

        [TestMethod]
        public void NGit_Remove()
        {
            var git = Git.Open(@"F:\Projects\_GitHub\DependencyScanner");

            DeleteBranchCommand command = git.BranchDelete();

            command.SetBranchNames("dev/SomeTest");

            var result = command.Call();
        }

        [TestMethod]
        public void NGit_CheckoutCommand()
        {
            var git = Git.Open(@"F:\Projects\_GitHub\DependencyScanner");

            CheckoutCommand command = git.Checkout();

            command.SetName("dev/SomeTest");

            var result = command.Call();
        }

        [TestMethod]
        public void NGit_State()
        {
            var git = Git.Open(@"F:\Projects\_GitHub\DependencyScanner");

            var repo = git.GetRepository();

            Status status = git.Status().Call();
        }
    }
}
