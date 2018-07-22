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

        public ICollection<PackageReference> References { get; } = new List<PackageReference>();

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
}