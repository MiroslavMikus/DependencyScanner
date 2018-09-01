using DependencyScanner.Core.Tools;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DependencyScanner.Core.Test.Tools
{
    [TestClass]
    public class StringExtensions
    {
        [TestMethod]
        [DataRow("someFIrSt", "first", true)]
        [DataRow("someFsIrSt", "first", false)]
        [DataRow("someFIrStAha", "fIrst", true)]
        [DataRow("someFIrSt", "First", true)]
        public void StringContains(string first, string second, bool result)
        {
            Assert.AreEqual(first.ContainsCaseInvariant(second), result);
        }
    }
}
