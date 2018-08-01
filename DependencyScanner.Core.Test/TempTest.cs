using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DependencyScanner.Core.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Serilog;

namespace DependencyScanner.Core.Test
{
    [Ignore]
    [TestClass]
    public class TempTest
    {
        ILogger logger = (new LoggerConfiguration().WriteTo.Console().CreateLogger());

        [TestMethod]
        public void ScanSolution()
        {
            var scan = new FileScanner();

            var progress = new DefaultProgress(logger)
            {
                Token = default(CancellationToken)
            };

            var result = scan.ScanSolution(@"F:\Projects\_GitHub\Exercise.DynamicProxy", progress);
        }

        [TestMethod]
        public void ScanMultipleSolutions()
        {
            var scan = new FileScanner();

            //var result = scan.ScanSolutions(@"F:\Projects\_GitHub").ToList();
        }

        [TestMethod]
        public async Task CheckConsolidateSolution()
        {
            var scan = new FileScanner();

            var progress = new DefaultProgress(logger)
            {
                Token = default(CancellationToken)
            };

            var solution = await scan.ScanSolution(@"F:\Projects\_GitHub\Exercise.DynamicProxy", progress);

            var comparer = new ProjectComparer();

            var result = comparer.FindConsolidateReferences(solution).ToList();
        }
    }
}
