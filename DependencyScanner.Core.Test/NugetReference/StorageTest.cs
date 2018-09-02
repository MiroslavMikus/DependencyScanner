using DependencyScanner.Core.NugetReference;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
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
    }
}
