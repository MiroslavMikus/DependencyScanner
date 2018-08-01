using DependencyScanner.Core.FileScan;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static DependencyScanner.Core.FileScan.ProjectReader;

namespace DependencyScanner.Core.Test
{
    [TestClass]
    public class ProjectReaderTest
    {
        [TestMethod]
        public void GetReferences_Test()
        {
            var docu = GetDocument(@"F:\Projects\_Exercises_local\NewDependencies\NewDependencies\NewDependencies.csproj");

            var result = ReadPackageReferences(docu);

            Assert.AreEqual(2, result.Count());
        }

        [TestMethod]
        public void ReadFrameworkVersion_Test()
        {
            var docu = GetDocument(@"F:\Projects\_Exercises_local\NewDependencies\NewDependencies\NewDependencies.csproj");

            var result = ReadFrameworkVersion(docu);

            Assert.AreEqual("v4.6.1", result);
        }
    }

    [TestClass]
    public class NuspecUpdateTest
    {
        [TestMethod]
        public void AddTags()
        {
            var docu = GetDocument(@"F:\s\Serva.Base\Serva.Base\Serva.Base.nuspec");

            var addResult = NuspecUpdater.AddDependency(docu, "SomeNewPackage");

            var removeResult = NuspecUpdater.RemoveDependency(docu, "SomeNewPackage");
        }
    }

}

