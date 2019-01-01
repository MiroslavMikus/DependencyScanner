using DependencyScanner.Core.Model;
using DependencyScanner.Core.NugetReference;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DependencyScanner.Core.Test
{
    [Ignore]
    [TestClass]
    public class TempTest : TestBase
    {
        [TestMethod]
        public void ScanSolution()
        {
            var result = Scanner.ScanSolution(@"F:\Projects\_GitHub\Exercise.DynamicProxy", Progress, false);
        }

        [TestMethod]
        public async Task CheckConsolidateSolution()
        {
            var solution = await Scanner.ScanSolution(@"F:\Projects\_GitHub\Exercise.DynamicProxy", Progress, false);

            var comparer = new ProjectComparer();

            var result = comparer.FindConsolidateReferences(solution).ToList();
        }

        [TestMethod]
        public void ScanNugerRef_Test()
        {
            var scan = new NugetReferenceScan();

            var project = new ProjectResult(new FileInfo(@"C:\s\Serva.Base.Plugin\Serva.Base.Plugin\Serva.Base.Plugin\Serva.Base.Plugin.csproj"),
                                            new FileInfo(@"C:\s\Serva.Base.Plugin\Serva.Base.Plugin\Serva.Base.Plugin\packages.config"));

            var actual = scan.ScanNugetReferences(project);
        }

        [TestMethod]
        public void TestGenerateReport()
        {
            var scan = new NugetReferenceScan();

            var project = new ProjectResult(new FileInfo(@"F:\Projects\_GitHub\DependencyScanner\DependencyScanner.Standalone\DependencyScanner.Standalone.csproj"),
                                            new FileInfo(@"F:\Projects\_GitHub\DependencyScanner\DependencyScanner.Standalone\packages.config"));

            var actual = scan.ScanNugetReferences(project);

            var generator = new ReportGenerator();

            var result = generator.GenerateReport(actual, "DependencyScanner", "1.2.3.4");

            Debug.WriteLine(result);

            var docu = XDocument.Parse(result);

            var node = docu.Nodes().OfType<XProcessingInstruction>().ToList();

            var key = new StorageKey(node);
        }
    }
}
