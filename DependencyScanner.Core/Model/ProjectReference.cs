using System.Runtime.Versioning;
using NuGet;

namespace DependencyScanner.Core.Model
{
    public class ProjectReference
    {
        public string Id { get; }
        public SemanticVersion Version { get; }
        public FrameworkName Framework { get; }

        public ProjectReference(PackageReference reference)
        {
            Id = reference.Id;
            Version = reference.Version;
            Framework = reference.TargetFramework;
        }

        public ProjectReference(string id, string version, FrameworkName frameworkId)
            : this(id, new SemanticVersion(version), frameworkId)
        {
        }

        public ProjectReference(string id, SemanticVersion version, FrameworkName frameworkId)
        {
            Id = id;
            Version = version;
            Framework = frameworkId;
        }

        public override string ToString()
        {
            return $"{Id}.{Version.ToString()}";
        }
    }
}
