using DependencyScanner.Core.GitClient;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;
namespace DependencyScanner.Core.Test.GitTests
{
    [TestClass]
    public class GitParserTest
    {
        private string _exampleBranchListInput = "  fix/SyncWithUi\r\n* master\r\n";

        [TestMethod]
        public void Parser()
        {
            var processResult = GitEngine.GitProcess(Environment.CurrentDirectory, GitCommand.BranchList);
            var result = GitParser.SplitString(processResult);

            AreEqual("  fix/SyncWithUi", result.First());
            AreEqual("* master", result.Last());
        }

        [TestMethod]
        public void GetBranchList_Test()
        {
            var result = GitParser.GetBranchList(_exampleBranchListInput);

            AreEqual("fix/SyncWithUi", result.First());
            AreEqual("master", result.Last());
        }

        [TestMethod]
        public void GetMaster_Test()
        {
            var result = GitParser.GetCurrentBranch(_exampleBranchListInput);

            AreEqual("master", result);
        }
    }
}
