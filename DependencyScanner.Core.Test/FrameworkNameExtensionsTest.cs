using DependencyScanner.Core.FileScan;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace DependencyScanner.Core.Test
{
    [TestClass]
    public class FrameworkNameExtensionsTest
    {
        [TestMethod]
        public void Equal_Test()
        {
            var First = new FrameworkName(".NETFramework", new Version("3.5"));
            var Second = new FrameworkName(".NETFramework,Version = v3.5");

            AreEqual(First, Second);
        }

        [TestMethod]
        public void Equal2_Test()
        {
            var First = new FrameworkName(".NETFramework", new Version("3.5"));
            var Second = new FrameworkName(".NETFramework,Version = v3.2");

            AreNotEqual(First, Second);
        }

        [TestMethod]
        public void IsSame1_Test()
        {
            var First = new FrameworkName(".NETFramework", new Version("3.5"));
            var Second = new FrameworkName(".netFramework,Version = v3.5");

            IsTrue(First.IsSame(Second));
        }

        [TestMethod]
        public void IsSame2_Test()
        {
            var First = new FrameworkName(".NETFramework", new Version("3.5"));
            var Second = new FrameworkName(".netFrameworkk,Version = v3.5");

            IsFalse(First.IsSame(Second));
        }

        [TestMethod]
        public void IsSame3_Test()
        {
            var First = new FrameworkName(".NETFramework", new Version("3.5"));
            var Second = new FrameworkName(".netFramework,Version = v3.1");

            IsTrue(First.IsSame(Second));
        }

        [TestMethod]
        public void IsSame4_Test()
        {
            var First = new FrameworkName(".NETFramework", new Version("3.5"));
            var Second = new FrameworkName(".netFrameworka,Version = v3.2");

            IsFalse(First.IsSame(Second));
        }

        [TestMethod]
        public void Compare_Test()
        {
            var First = new FrameworkName(".NETFramework", new Version("3.5"));
            var Second = new FrameworkName(".netFramework,Version = v3.1");

            IsTrue(First.Compare(Second) == true);
        }

        [TestMethod]
        public void Compare2_Test()
        {
            var First = new FrameworkName(".NETFramework", new Version("3.0"));
            var Second = new FrameworkName(".netFramework,Version = v3.1");

            IsTrue(First.Compare(Second) == false);
        }

        [TestMethod]
        public void Compare3_Test()
        {
            var First = new FrameworkName(".NETFramework", new Version("3.0"));
            var Second = new FrameworkName(".netFramework,Version = v3.0");

            IsTrue(First.Compare(Second) == null);
        }
    }
}
