using NuGet;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace DependencyScanner.Core.Model
{
    [DebuggerDisplay("{ProjectInfo.Name}")]
    public class ProjectResult
    {
        public FileInfo ProjectInfo { get; }
        public FileInfo PackageInfo { get; }
        public FileInfo NuspecInfo { get; set; }
        public bool HasNuspec { get => NuspecInfo != null; }

        public ICollection<ProjectReference> References { get; } = new List<ProjectReference>();

        public ProjectResult(FileInfo projectInfo)
        {
            ProjectInfo = projectInfo;
        }

        public ProjectResult(FileInfo projectInfo, FileInfo packageInfo)
        {
            ProjectInfo = projectInfo;
            PackageInfo = packageInfo;
        }
    }

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