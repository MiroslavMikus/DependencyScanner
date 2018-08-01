using DependencyScanner.Core.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NuGet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DependencyScanner.Core.Test
{
    [TestClass]
    public class FindConsolidateNugetsTest
    {
        private FileInfo FakeInfo() => new System.IO.FileInfo(@"C:\");

        [TestMethod]
        public void FindConsolidateNugets_1()
        {
            var reference1 = new PackageReference("Nuget", new SemanticVersion(new Version(1, 1, 1, 2)), null, new System.Runtime.Versioning.FrameworkName("dot.Net", new Version(1, 2)), false);
            var project1 = new ProjectResult(FakeInfo(), FakeInfo());
            project1.References.Add(new ProjectReference(reference1));
            var solution1 = new SolutionResult(FakeInfo());
            solution1.Projects.Add(project1);

            var reference2 = new PackageReference("Nuget", new SemanticVersion(new Version(1, 1, 1, 2)), null, new System.Runtime.Versioning.FrameworkName("dot.Net", new Version(2, 2)), false);
            var project2 = new ProjectResult(FakeInfo(), FakeInfo());
            project2.References.Add(new ProjectReference(reference2));
            var solution2 = new SolutionResult(FakeInfo());
            solution2.Projects.Add(project2);

            var comparer = new SolutionComparer();

            var result = comparer.FindConsolidateReferences(solution1, solution2).ToList();

            Assert.AreEqual(result.Count(), 0);
        }

        [TestMethod]
        public void FindConsolidateNugets_2()
        {
            var reference1 = new PackageReference("Nuget", new SemanticVersion(new Version(1, 1, 2, 2)), null, new System.Runtime.Versioning.FrameworkName("dot.Net", new Version(1, 2)), false);
            var project1 = new ProjectResult(FakeInfo(), FakeInfo());
            project1.References.Add(new ProjectReference(reference1));
            var solution1 = new SolutionResult(FakeInfo());
            solution1.Projects.Add(project1);

            var reference2 = new PackageReference("Nuget", new SemanticVersion(new Version(1, 1, 1, 2)), null, new System.Runtime.Versioning.FrameworkName("dot.Net", new Version(2, 2)), false);
            var project2 = new ProjectResult(FakeInfo(), FakeInfo());
            project2.References.Add(new ProjectReference(reference2));
            var solution2 = new SolutionResult(FakeInfo());
            solution2.Projects.Add(project2);

            var comparer = new SolutionComparer();

            var result = comparer.FindConsolidateReferences(solution1, solution2).ToList();

            Assert.AreEqual(result.Count(), 1);
            Assert.AreEqual(result.First().References.Count, 2);
        }


        [TestMethod]
        public void FindConsolidateNugets_3()
        {
            var reference1 = new PackageReference("Nuget", new SemanticVersion(new Version(1, 1, 1, 2)), null, new System.Runtime.Versioning.FrameworkName("dot.Net", new Version(1, 2)), false);
            var reference11 = new PackageReference("NotNuget", new SemanticVersion(new Version(1, 1, 1, 2)), null, new System.Runtime.Versioning.FrameworkName("dot.Net", new Version(1, 2)), false);
            var project1 = new ProjectResult(FakeInfo(), FakeInfo());
            project1.References.Add(new ProjectReference(reference1));
            project1.References.Add(new ProjectReference(reference11));
            var solution1 = new SolutionResult(FakeInfo());
            solution1.Projects.Add(project1);

            var reference2 = new PackageReference("Nuget", new SemanticVersion(new Version(1, 1, 1, 2)), null, new System.Runtime.Versioning.FrameworkName("dot.Net", new Version(2, 2)), false);
            var reference22 = new PackageReference("NotNuget", new SemanticVersion(new Version(1, 3, 1, 2)), null, new System.Runtime.Versioning.FrameworkName("dot.Net", new Version(1, 2)), false);
            var project2 = new ProjectResult(FakeInfo(), FakeInfo());
            project2.References.Add(new ProjectReference(reference2));
            project2.References.Add(new ProjectReference(reference22));
            var solution2 = new SolutionResult(FakeInfo());
            solution2.Projects.Add(project2);

            var comparer = new SolutionComparer();

            var result = comparer.FindConsolidateReferences(solution1, solution2).ToList();

            Assert.AreEqual(result.Count(), 1);
            Assert.AreEqual(result.First().References.Count, 2);
            Assert.AreEqual(result.First().Id, "NotNuget");
        }

        [TestMethod]
        public void FindConsolidateNugets_4()
        {
            var reference1 = new PackageReference("Nuget", new SemanticVersion(new Version(1, 1, 3, 2)), null, new System.Runtime.Versioning.FrameworkName("dot.Net", new Version(1, 2)), false);
            var reference11 = new PackageReference("NotNuget", new SemanticVersion(new Version(1, 1, 1, 2)), null, new System.Runtime.Versioning.FrameworkName("dot.Net", new Version(1, 2)), false);
            var project1 = new ProjectResult(FakeInfo(), FakeInfo());
            project1.References.Add(new ProjectReference(reference1));
            project1.References.Add(new ProjectReference(reference11));
            var solution1 = new SolutionResult(FakeInfo());
            solution1.Projects.Add(project1);

            var reference2 = new PackageReference("Nuget", new SemanticVersion(new Version(4, 1, 1, 2)), null, new System.Runtime.Versioning.FrameworkName("dot.Net", new Version(2, 2)), false);
            var project2 = new ProjectResult(FakeInfo(), FakeInfo());
            project2.References.Add(new ProjectReference(reference2));
            var solution2 = new SolutionResult(FakeInfo());
            solution2.Projects.Add(project2);

            var comparer = new SolutionComparer();

            var result = comparer.FindConsolidateReferences(solution1, solution2).ToList();

            Assert.AreEqual(result.Count(), 1);
            Assert.AreEqual(result.First().References.Count, 2);
        }
    }
}
