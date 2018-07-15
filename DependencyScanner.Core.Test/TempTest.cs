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
            var scan = new Scanner();

            var result = scan.Scan(@"F:\Projects\_GitHub\Exercise.DynamicProxy");


        }
    }
}
