using DependencyScanner.Core.Model;
using DependencyScanner.Core.NugetReference;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DependencyScanner.Core.Test
{
    //[Ignore]
    [TestClass]
    public class TempTest : TestBase
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

            var actual = scan.ReadDependencies(@"F:\Projects\_GitHub\DependencyScanner\packages");
        }

        [TestMethod]
        public void ScanNugerRef_Test()
        {
            var scan = new NugetReferenceScan(@"C:\ProgramData\DependencyScanner");

            var project = new ProjectResult(new FileInfo(@"F:\Projects\_GitHub\DependencyScanner\DependencyScanner.Standalone\DependencyScanner.Standalone.csproj"),
                                            new FileInfo(@"F:\Projects\_GitHub\DependencyScanner\DependencyScanner.Standalone\packages.config"));

            var actual = scan.ScanNugetReferences(project);
        }
    }
}
