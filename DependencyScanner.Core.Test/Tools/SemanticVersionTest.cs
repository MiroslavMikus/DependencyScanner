﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using NuGet;
using System;
using System.Collections.Generic;
using static DependencyScanner.Core.Tools.VersionComparer;

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
            var list = new List<SemanticVersion>
            {
                new SemanticVersion(new Version(1, 1, 1, 1)),
                new SemanticVersion(new Version(1, 1, 1, 2)),
                new SemanticVersion(new Version(1, 1, 1, 2)),
                new SemanticVersion(new Version(1, 1, 1, 2))
            };

            var areAllSame = AllAreSame(list);

            Assert.IsFalse(areAllSame);
        }

        [TestMethod]
        public void Compare4()
        {
            var list = new List<SemanticVersion>
            {
                new SemanticVersion(new Version(1, 1, 1, 2)),
                new SemanticVersion(new Version(1, 1, 1, 2)),
                new SemanticVersion(new Version(1, 1, 1, 2)),
                new SemanticVersion(new Version(1, 1, 1, 2))
            };

            var areAllSame = AllAreSame(list);

            Assert.IsTrue(areAllSame);
        }
    }
}
