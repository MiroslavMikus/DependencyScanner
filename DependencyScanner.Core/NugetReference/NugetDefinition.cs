using NuGet;
using System.Runtime.Versioning;

namespace DependencyScanner.Core.NugetReference
{
    public class NugetDefinition
    {
        public string Id { get; set; }
        public SemanticVersion CurrentVersion { get; set; }
        public PackageDependencySet Dependencies { get; set; }
        public FrameworkName Framework { get => Dependencies?.TargetFramework; }

        public override string ToString()
        {
            return $"{Id}.{CurrentVersion.ToString()}";
        }
    }
}
