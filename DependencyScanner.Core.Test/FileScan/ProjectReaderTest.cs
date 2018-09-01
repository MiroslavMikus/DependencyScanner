using DependencyScanner.Core.FileScan;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Runtime.Versioning;
using static DependencyScanner.Core.FileScan.ProjectReader;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace DependencyScanner.Core.Test
{
    [TestClass]
    public class ProjectReaderTest
    {
        [TestMethod]
        public void GetReferences_Test()
        {
            var docu = GetDocument(@"TestData\NewDependencies.csproj");

            var framework = new FrameworkName(SupportedFrameworks.Standard, new Version("2.0"));

            var actual = ReadPackageReferences(docu, framework);

            AreEqual(2, actual.Count());

            IsTrue(actual.All(a => a.Framework == framework));
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

            AreEqual(expected, actual);
        }

        [TestMethod]
        public void ReadStandardFramework()
        {
            var docu = GetDocument(@"TestData\Standard.csproj");

            var actual = GetFrameworkName(docu);

            var expected = new FrameworkName(SupportedFrameworks.Standard, new Version("2.0"));

            AreEqual(expected, actual);
        }

        [TestMethod]
        public void ReadCoreFramework()
        {
            var docu = GetDocument(@"TestData\Core.csproj");

            var actual = GetFrameworkName(docu);

            var expected = new FrameworkName(SupportedFrameworks.Core, new Version("2.1"));

            AreEqual(expected, actual);
        }

        [TestMethod]
        [DataRow(@"TestData\UWP.csproj", "10.0.15063.0")]
        [DataRow(@"TestData\Xamarin.UWP.csproj", "10.0.14393.0")]
        public void ReadUWPFramework(string path, string expected)
        {
            var docu = GetDocument(path);

            var actual = GetFrameworkName(docu);

            var expectedResult = new FrameworkName(SupportedFrameworks.UWP, new Version(expected));

            AreEqual(expectedResult, actual);
        }

        [TestMethod]
        [DataRow(@"TestData\NewDependencies.csproj", "4.5")]
        [DataRow(@"TestData\Full.csproj", "4.6.1")]
        public void ReadFullFramework(string path, string expected)
        {
            var docu = GetDocument(path);

            var actual = GetFrameworkName(docu);

            var expectedResult = new FrameworkName(SupportedFrameworks.DotNet, new Version(expected));

            AreEqual(expectedResult, actual);
        }
    }
}
