using DependencyScanner.Api.Interfaces;
using System.Collections.Generic;

namespace DependencyScanner.Plugins.Wd.Model
{
    public class WorkingDirectorySettings : ISettings
    {
        public string Id => "WorkingDirectorySettings";
        public Dictionary<string, string[]> WorkingDirectoryStructure;
        public bool ExecuteGitFetchWhileScanning = true;
    }
}
