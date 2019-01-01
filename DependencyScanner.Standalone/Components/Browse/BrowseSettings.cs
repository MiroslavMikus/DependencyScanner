using DependencyScanner.Core.Interfaces;
using DependencyScanner.Standalone.Setting;
using System.Collections.Generic;

namespace DependencyScanner.Standalone.Components.Browse
{
    public class BrowseSettings : ObservableSettingsBase
    {
        private bool _scanAfterDirectoryChange = true;

        public bool ScanAfterDirectoryChange
        {
            get { return _scanAfterDirectoryChange; }
            set
            {
                Set(ref _scanAfterDirectoryChange, value);
            }
        }

        private string _workingDirectory;

        public string WorkingDirectory
        {
            get { return _workingDirectory; }
            set
            {
                Set(ref _workingDirectory, value);
            }
        }

        private List<string> _workingDirectories;

        public List<string> WorkingDirectories
        {
            get { return _workingDirectories; }
            set
            {
                Set(ref _workingDirectories, value);
            }
        }

        private bool _executeGitFetchWithScan = true;

        public bool ExecuteGitFetchWithScan
        {
            get { return _executeGitFetchWithScan; }
            set
            {
                Set(ref _executeGitFetchWithScan, value);
            }
        }
    }
}
