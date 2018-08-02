using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DependencyScanner.Core.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Serilog;
using static DependencyScanner.Core.Test.TestTools;

namespace DependencyScanner.Core.Test
{
    [Ignore]
    [TestClass]
    public class TempTest
    {
        [TestMethod]
        public void ScanSolution()
        {
            var result = Scanner.ScanSolution(@"F:\Projects\_GitHub\Exercise.DynamicProxy", Progress);
        }

        [TestMethod]
        public async Task CheckConsolidateSolution()
        {
            var solution = await Scanner.ScanSolution(@"F:\Projects\_GitHub\Exercise.DynamicProxy", Progress);

            var comparer = new ProjectComparer();

            var result = comparer.FindConsolidateReferences(solution).ToList();
        }
    }
}
