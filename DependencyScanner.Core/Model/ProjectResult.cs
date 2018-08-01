using NuGet;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using static DependencyScanner.Core.FileScan.ProjectReader;

namespace DependencyScanner.Core.Model
{
    [DebuggerDisplay("{ProjectInfo.Name}")]
    public class ProjectResult
    {
        public FileInfo ProjectInfo { get; }
        public FileInfo PackageInfo { get; }
        public FileInfo NuspecInfo { get; internal set; }
        public string FrameworkVersion { get; }
        public bool HasNuspec { get => NuspecInfo != null; }

        public ICollection<ProjectReference> References { get; } = new List<ProjectReference>();

        public ProjectResult(FileInfo projectInfo)
        {
            ProjectInfo = projectInfo;

            // package reference was not found
            var docu = GetDocument(ProjectInfo.FullName);

            References = ReadPackageReferences(docu).ToList();

            FrameworkVersion = ReadFrameworkVersion(docu);
        }

        public ProjectResult(FileInfo projectInfo, FileInfo packageInfo)
        {
            ProjectInfo = projectInfo;

            PackageInfo = packageInfo;

            // there are package references
            var file = new PackageReferenceFile(packageInfo.FullName);

            References.AddRange(file.GetPackageReferences().Select(a => new ProjectReference(a)));

            FrameworkVersion = ReadFrameworkVersion(ProjectInfo.FullName);
        }
    }
}