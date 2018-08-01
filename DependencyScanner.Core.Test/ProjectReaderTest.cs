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
            var docu = GetDocument(@"TestData\NewDependencies.csproj");

            var result = ReadPackageReferences(docu);

            Assert.AreEqual(2, result.Count());
        }

        [TestMethod]
        public void ReadFrameworkVersion_Test()
        {
            var docu = GetDocument(@"TestData\NewDependencies.csproj");

            var result = ReadFrameworkVersion(docu);

            Assert.AreEqual("v4.6.1", result);
        }
    }
}

