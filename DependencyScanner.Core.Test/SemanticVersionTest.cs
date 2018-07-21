using Microsoft.VisualStudio.TestTools.UnitTesting;
using NuGet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DependencyScanner.Core.Test
{
    [TestClass]
    public class SemanticVersionTest
    {
        [TestMethod]
        public void Compare()
        {
            var version = new SemanticVersion(new Version(1, 1, 1, 1));
            var version2 = new SemanticVersion(new Version(1, 1, 1, 1));

            Assert.AreEqual(version, version2);
        }

        [TestMethod]
        public void Compare2()
        {
            var version = new SemanticVersion(new Version(1, 1, 1, 1));
            var version2 = new SemanticVersion(new Version(1, 1, 1, 2));

            Assert.AreNotEqual(version, version2);
        }

        [TestMethod]
        public void Compare3()
        {
            var comparer = new SolutionComparer();

            var list = new List<SemanticVersion>
            {
                new SemanticVersion(new Version(1, 1, 1, 1)),
                new SemanticVersion(new Version(1, 1, 1, 2)),
                new SemanticVersion(new Version(1, 1, 1, 2)),
                new SemanticVersion(new Version(1, 1, 1, 2))
            };

            var areAllSame = comparer.AllAreSame(list);

            Assert.IsFalse(areAllSame);
        }

        [TestMethod]
        public void Compare4()
        {
            var comparer = new SolutionComparer();

            var list = new List<SemanticVersion>
            {
                new SemanticVersion(new Version(1, 1, 1, 2)),
                new SemanticVersion(new Version(1, 1, 1, 2)),
                new SemanticVersion(new Version(1, 1, 1, 2)),
                new SemanticVersion(new Version(1, 1, 1, 2))
            };

            var areAllSame = comparer.AllAreSame(list);

            Assert.IsTrue(areAllSame);
        }
    }
}
