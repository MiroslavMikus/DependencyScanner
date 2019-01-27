using DependencyScanner.Api.Interfaces;
using DependencyScanner.Plugins.Wd.Services;
using System.Collections.Generic;

namespace DependencyScanner.Plugins.Wd.Model
{
    public class WorkingDirectorySettings : ISettings
    {
        public string Id => "WorkingDirectorySettings";
        public List<StorableWorkingDirectory> WorkingDirectoryStructure { get; set; } = new List<StorableWorkingDirectory>();
        public bool ExecuteGitFetchWhileScanning { get; set; } = true;
    }
}
