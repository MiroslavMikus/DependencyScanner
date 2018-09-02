using DependencyScanner.Core.Model;
using DependencyScanner.Core.NugetReference;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace DependencyScanner.Core.Test.NugetReference
{
    [TestClass]
    public class StorageTest
    {
        [TestMethod]
        public void StorageReadData()
        {
            var storage = new ReportStorage(@"TestData\StorageData");

            IsTrue(storage.Contains("DependencyScanner1"));
            IsTrue(storage.Contains("DependencyScanner 2"));
            IsTrue(storage.Contains("DependencyScanner3"));
        }

        [TestMethod]
        public void StorageReadData2()
        {
            var storage = new ReportStorage(@"TestData\StorageData");

            IsTrue(storage.Contains("DependencyScanner1", out Dictionary<DateTime, string> result));

            IsTrue(result[DateTime.Parse("02/05/2018 15:41:59")] == @"TestData\StorageData\file1.1.html");
            IsTrue(result[DateTime.Parse("02/06/2018 15:41:59")] == @"TestData\StorageData\file1.html");
        }

        [TestMethod]
        [Ignore]
        public async Task TestEndToEnd()
        {
            var targetPath = @"TestData\StorageTest";

            if (Directory.Exists(targetPath))
                Directory.Delete(targetPath, true);

            Directory.CreateDirectory(targetPath);

            var facade = new NugetScanFacade(
                new ReportGenerator(),
                new NugetReferenceScan(),
                new ReportStorage(targetPath),
                "3.2.1");

            var project = new ProjectResult(new FileInfo(@"C:\s\Serva.Base.Plugin\Serva.Base.Plugin\Serva.Base.Plugin\Serva.Base.Plugin.csproj"),
                                new FileInfo(@"C:\s\Serva.Base.Plugin\Serva.Base.Plugin\Serva.Base.Plugin\packages.config"));

            var project2 = new ProjectResult(new FileInfo(@"F:\Projects\_GitHub\DependencyScanner\DependencyScanner.Standalone\DependencyScanner.Standalone.csproj"),
                                new FileInfo(@"F:\Projects\_GitHub\DependencyScanner\DependencyScanner.Standalone\packages.config"));

            for (int i = 0; i < 5; i++)
            {
                facade.ExecuteScan(project);
                facade.ExecuteScan(project2);
                await Task.Delay(1100);
            }
        }
    }
}
