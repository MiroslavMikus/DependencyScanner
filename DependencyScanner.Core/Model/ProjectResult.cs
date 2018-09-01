using NuGet;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Versioning;
using static DependencyScanner.Core.FileScan.ProjectReader;

namespace DependencyScanner.Core.Model
{
    [DebuggerDisplay("{ProjectInfo.Name}")]
    public class ProjectResult
    {
        public FileInfo ProjectInfo { get; }
        public FileInfo PackageInfo { get; }
        public FileInfo NuspecInfo { get; internal set; }
        public FrameworkName FrameworkId { get; }
        public string FrameworkName { get => FrameworkId.Identifier; }
        public string FrameworkVersion { get => FrameworkId.Version.ToString(); }
        public bool HasNuspec { get => NuspecInfo != null; }

        public ICollection<ProjectReference> References { get; } = new List<ProjectReference>();

        public ProjectResult(FileInfo projectInfo)
        {
            ProjectInfo = projectInfo;

            // package reference was not found
            var docu = GetDocument(ProjectInfo.FullName);

            FrameworkId = GetFrameworkName(docu);

            References = ReadPackageReferences(docu, FrameworkId).ToList();
        }

        public ProjectResult(FileInfo projectInfo, FileInfo packageInfo)
        {
            ProjectInfo = projectInfo;

            PackageInfo = packageInfo;

            // there are package references
            var file = new PackageReferenceFile(packageInfo.FullName);

            References.AddRange(file.GetPackageReferences().Select(a => new ProjectReference(a)));

            FrameworkId = GetFrameworkName(ProjectInfo.FullName);
        }

        public override bool Equals(object obj)
        {
            if (obj is ProjectResult result)
            {
                return ProjectInfo.FullName == result.ProjectInfo.FullName;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return ProjectInfo.FullName.GetHashCode();
        }
    }
}
