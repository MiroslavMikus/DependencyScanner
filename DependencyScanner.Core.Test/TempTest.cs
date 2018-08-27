using DependencyScanner.Core.Model;
using DependencyScanner.Core.NugetReference;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NuGet;
using System.IO;
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
        public void NugetScan_CopyNuspec_Test()
        {
            var scan = new NugetReferenceScan(@"C:\ProgramData\DependencyScanner");

            scan.CopyNuspec(@"C:\s\Serva.Application.OperationControlCenter\packages\Autofac.4.8.1", @"C:\ProgramData\DependencyScanner\temp");
        }

        [TestMethod]
        public void NugetScan_CopyNuspecs_Test()
        {
            var scan = new NugetReferenceScan(@"C:\ProgramData\DependencyScanner");

            scan.CopyNuspecs(@"C:\s\Serva.Application.OperationControlCenter\packages\", @"C:\ProgramData\DependencyScanner\temp");
        }

        [TestMethod]
        public void NugetScan_GetVersion()
        {
            var input = File.ReadAllText(@"TestData\Markdig.Wpf.Editor.nuspec");

            var scan = new NugetReferenceScan(@"C:\ProgramData\DependencyScanner");

            var actual = scan.ReadNuspecVerion(input);

            Assert.AreEqual("0.1.2.3", actual);
        }
    }
}
