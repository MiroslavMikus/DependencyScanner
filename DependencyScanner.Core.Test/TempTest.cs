using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DependencyScanner.Core.Test
{
    [TestClass]
    public class TempTest
    {
        [TestMethod]
        public void TempTesting()
        {
            var scan = new FileScanner();

            var result = scan.ScanSolution(@"F:\Projects\_GitHub\Exercise.DynamicProxy");


        }
    }
}
