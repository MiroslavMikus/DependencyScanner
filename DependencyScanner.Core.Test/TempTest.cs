using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DependencyScanner.Core.Test
{
    //[Ignore]
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
        public void CheckConsolidateSolution()
        {
            var scan = new FileScanner();

            var solution = scan.ScanSolution(@"F:\s\Serva.Application.OperationControlCenter");

            var comparer = new ProjectComparer();

            var result = comparer.FindConsolidateReferences(solution).ToList();
        }
    }
}
