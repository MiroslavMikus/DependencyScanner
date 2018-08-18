using DependencyScanner.Core.Nuspec;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using static DependencyScanner.Core.FileScan.ProjectReader;
using static DependencyScanner.Core.Nuspec.NuspecUpdater;

namespace DependencyScanner.Core.Test
{
    [TestClass]
    public class NuspecUpdateTest
    {
        [TestMethod]
        public void AddTags_Test()
        {
            var docu = GetDocument(@"TestData\Markdig.Wpf.Editor.nuspec");

            var addResult = AddDependency(docu, "SomeNewPackage");

            var removeResult = RemoveDependency(docu, "SomeNewPackage");
        }

        [TestMethod]
        public void ReadAllDependencies_Test()
        {
            var docu = GetDocument(@"TestData\Markdig.Wpf.Editor.nuspec");

            var result = GetDependencies(docu);
        }

        [TestMethod]
        public void CheckMissingPackages_Test()
        {
            var comparer = new NuspecComparer();

            var package = new string[] { "package1", "package2", "package3", "package4" };
            var nuspec = new string[] { "package1", "package2", "package3" };

            var result = comparer.CheckMissingPackages(nuspec, package);

            Assert.AreEqual("package4", result.First());
            Assert.AreEqual(1, result.Count());
        }

        [TestMethod]
        public void CheckMissingPackages2_Test()
        {
            var comparer = new NuspecComparer();

            var package = new string[] { "package1", "package2", "package4" };
            var nuspec = new string[] { "package1", "package2", "package3", "package4" };

            var result = comparer.CheckMissingPackages(nuspec, package);

            Assert.AreEqual(0, result.Count());
        }

        [TestMethod]
        public void UselessMissingPackages_Test()
        {
            var comparer = new NuspecComparer();

            var nuspec = new string[] { "package1", "package2", "package3", "package4" };
            var package = new string[] { "package1", "package2", "package3" };

            var result = comparer.CheckUselessPackages(nuspec, package);

            Assert.AreEqual("package4", result.First());
            Assert.AreEqual(1, result.Count());
        }

        [TestMethod]
        public void UselessMissingPackages2_Test()
        {
            var comparer = new NuspecComparer();

            var nuspec = new string[] { "package1", "package2", "package4" };
            var package = new string[] { "package1", "package2", "package3", "package4" };

            var result = comparer.CheckUselessPackages(nuspec, package);

            Assert.AreEqual(0, result.Count());
        }
    }
}
