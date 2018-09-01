using DependencyScanner.Core.FileScan;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Runtime.Versioning;
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
        [DataRow("netstandard2.0", "2.0")]
        [DataRow("netcoreapp2.1", "2.1")]
        [DataRow("netcoreapp2.3", "2.3")]
        [DataRow("netcoreapp2.1.3.4", "2.1.3.4")]
        [DataRow("netcoreapp", "")]
        public void GetFrameworkVersion(string input, string expected)
        {
            var actual = GetVersion(input);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ReadStandardFramework()
        {
            var docu = GetDocument(@"TestData\Standard.csproj");

            var actual = GetFrameworkName(docu);

            var expected = new FrameworkName(SupportedFrameworks.Standard, new Version("2.0"));

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ReadCoreFramework()
        {
            var docu = GetDocument(@"TestData\Core.csproj");

            var actual = GetFrameworkName(docu);

            var expected = new FrameworkName(SupportedFrameworks.Core, new Version("2.1"));

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ReadUWPFramework()
        {
            var docu = GetDocument(@"TestData\UWP.csproj");

            var actual = GetFrameworkName(docu);

            var expected = new FrameworkName(SupportedFrameworks.UWP, new Version("10.0.15063.0"));

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [DataRow(@"TestData\NewDependencies.csproj", "4.5")]
        [DataRow(@"TestData\Full.csproj", "4.6.1")]
        public void ReadFullFramework(string path, string expected)
        {
            var docu = GetDocument(path);

            var actual = GetFrameworkName(docu);

            var expectedResult = new FrameworkName(SupportedFrameworks.DotNet, new Version(expected));

            Assert.AreEqual(expectedResult, actual);
        }
    }
}
