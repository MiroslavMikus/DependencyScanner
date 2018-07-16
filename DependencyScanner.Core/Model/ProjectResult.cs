using NuGet;
using System.Collections.Generic;
using System.IO;

namespace DependencyScanner.Core.Model
{
    public class ProjectResult
    {
        public FileInfo ProjectInfo { get; }
        public FileInfo PackageInfo{ get; }
        public ICollection<PackageReference> References { get; } = new List<PackageReference>();

        public ProjectResult(FileInfo projectInfo, FileInfo packageInfo)
        {
            ProjectInfo = projectInfo;
            PackageInfo = packageInfo;
        }
    }
}