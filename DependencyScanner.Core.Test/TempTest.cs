using DependencyScanner.Core.Model;
using DependencyScanner.Core.NugetReference;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NuGet;
using System;
using System.Linq;
using System.Runtime.Versioning;
using System.Threading.Tasks;
using static DependencyScanner.Core.Test.TestTools;

namespace DependencyScanner.Core.Test
{
    //[Ignore]
    [TestClass]
    public class TempTest : BaseTest
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

        [TestMethod]
        public void NugetScan_CopyNuspec_Test()
        {
            var scan = new NugetReferenceScan(@"C:\ProgramData\DependencyScanner");
        }

        [TestMethod]
        public void NugetScan_CopyNuspecs_Test()
        {
            var scan = new NugetReferenceScan(@"C:\ProgramData\DependencyScanner");
        }

        [TestMethod]
        public void NugetScan_ReadNuspec()
        {
            var scan = new NugetReferenceScan(@"C:\ProgramData\DependencyScanner");

            var actual = scan.ReadDependencies(@"C:\s\Serva.Application.OperationControlCenter\packages");
        }
    }
}
