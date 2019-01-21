using DependencyScanner.Api.Interfaces;
using System.Collections.Generic;

namespace DependencyScanner.Plugins.Wd.Model
{
    public class WorkingDirectorySettings : ISettings
    {
        public string Id => "WorkingDirectorySettings";
        public Dictionary<string, string[]> WorkingDirectoryStructure { get; set; } = new Dictionary<string, string[]>();
        public bool ExecuteGitFetchWhileScanning { get; set; } = true;
    }
}
