using NuGet;

namespace DependencyScanner.Core.Model
{
    public class ProjectReference
    {
        public string Id { get; }
        public SemanticVersion Version { get; }

        public ProjectReference(PackageReference reference)
        {
            Id = reference.Id;
            Version = reference.Version;
        }

        public ProjectReference(string id, string version)
        {
            Id = id;
            Version = new SemanticVersion(version);
        }
    }
}