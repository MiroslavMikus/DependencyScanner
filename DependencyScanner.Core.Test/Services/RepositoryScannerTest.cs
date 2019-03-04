using DependencyScanner.Api.Interfaces;
using DependencyScanner.Core.Model;
using DependencyScanner.Core.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DependencyScanner.Core.Test.Services
{
    [TestClass]
    [Ignore]
    public class RepositoryScannerTest : TestBase
    {
        [TestMethod]
        public async Task InteractiveTest()
        {
            Func<string, IGitInfo> ctor = a => new GitInfo(a, Git);

            var scanner = new RepositoryScanner(ctor, _logger);

            var result = await scanner.ScanForGitRepositories(@"F:\Projects\_GitHub", null, false, CancellationToken.None);
        }
    }
}
