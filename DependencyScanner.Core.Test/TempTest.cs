using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DependencyScanner.Core.Test
{
    [Ignore]
    [TestClass]
    public class TempTest
    {
        [TestMethod]
        public void ScanSolution()
        {
            var scan = new FileScanner();

            var result = scan.ScanSolution(@"F:\Projects\_GitHub\Exercise.DynamicProxy");
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

            var solution = await scan.ScanSolution(@"F:\s\Serva.Application.OperationControlCenter");

            var comparer = new ProjectComparer();

            var result = comparer.FindConsolidateReferences(solution).ToList();
        }
    }
}
