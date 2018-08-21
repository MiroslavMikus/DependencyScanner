using DependencyScanner.Core.Model;
using DependencyScanner.Core.NugetReference;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NuGet;
using System.Linq;
using System.Threading.Tasks;
using static DependencyScanner.Core.Test.TestTools;

namespace DependencyScanner.Core.Test
{
    //[Ignore]
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

        [TestMethod]
        public async Task NugerRefScan_Test()
        {
            //var scan = new NugetReferenceScan();

            //var result = await scan.GetNuspec(@"F:\Projects\_GitHub\DependencyScanner\packages\Autofac.4.8.1");
        }
    }
}
