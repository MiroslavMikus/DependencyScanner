using DependencyScanner.Core.GitClient;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DependencyScanner.Core.Test.GitTests
{
    [TestClass]
    public class GitConfigTest
    {
        [TestMethod]
        [DataRow("HEAD1", "master")]
        [DataRow("HEAD2", "alsoMaster")]
        public void TestReadCurrentBranch(string fileInput, string expectedResult)
        {
            var givenContent = File.ReadLines(System.IO.Path.Combine(@"TestData\Git", fileInput));

            var result = GitConfig.GetCurrentBranchFromHead(givenContent.First());

            result.Should().Be(expectedResult);
        }
    }
}
