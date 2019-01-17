using DependencyScanner.Api.Interfaces;
using System.Collections.Generic;

namespace DependencyScanner.Plugins.Browse.Model
{
    public class WorkingDirectorySettings : ISettings
    {
        public string Id => "WorkingDirectorySettings";
        public Dictionary<string, string[]> WorkingDirectoryStructure;
        public bool ExecuteGitFetchWhileScanning = true;
    }
}
