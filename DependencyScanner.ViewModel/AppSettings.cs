using DependencyScanner.Core;
using GalaSoft.MvvmLight;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;

namespace DependencyScanner.ViewModel
{
    public class AppSettings : ViewModelBase
    {
        static AppSettings()
        {
            FileScanner.ExecuteGitFetchWitScan = Properties.Settings.Default.ExecuteGitFetchWitScan;
        }

        private static AppSettings _instance;
        public static AppSettings Instance { get => _instance ?? (_instance = new AppSettings()); }

        private bool _executeScanOnInit = Properties.Settings.Default.ExecuteScanOnInit;
        public bool ExecuteScanOnInit
        {
            get { return _executeScanOnInit; }
            set
            {
                if (Set(ref _executeScanOnInit, value))
                {
                    Properties.Settings.Default.ExecuteScanOnInit = value;
                    Properties.Settings.Default.Save();
                }
            }
        }

        private bool _showCmdButton = Properties.Settings.Default.ShowCmdButton;
        public bool ShowCmdButton
        {
            get { return _showCmdButton; }
            set
            {
                if (Set(ref _showCmdButton, value))
                {
                    Properties.Settings.Default.ShowCmdButton = value;
                    Properties.Settings.Default.Save();
                }
            }
        }

        private bool _showFolderButton = Properties.Settings.Default.ShowFolderButton;
        public bool ShowFolderButton
        {
            get { return _showFolderButton; }
            set
            {
                if (Set(ref _showFolderButton, value))
                {
                    Properties.Settings.Default.ShowFolderButton = value;
                    Properties.Settings.Default.Save();
                }
            }
        }

        private bool _showOpenButton = Properties.Settings.Default.ShowOpenButton;
        public bool ShowOpenButton
        {
            get { return _showOpenButton; }
            set
            {
                if (Set(ref _showOpenButton, value))
                {
                    Properties.Settings.Default.ShowOpenButton = value;
                    Properties.Settings.Default.Save();
                }
            }
        }

        private string _preferedConsoleTool = Properties.Settings.Default.PreferedConsoleTool;
        public string PreferedConsoleTool
        {
            get { return _preferedConsoleTool; }
            set
            {
                if (Set(ref _preferedConsoleTool, value))
                {
                    Properties.Settings.Default.PreferedConsoleTool = value;
                    Properties.Settings.Default.Save();
                }
            }
        }

        private string _preferencedWebBrowser = Properties.Settings.Default.PreferencedWebBrowser;
        public string PreferencedWebBrowser
        {
            get { return _preferencedWebBrowser; }
            set
            {
                if (Set(ref _preferencedWebBrowser, value))
                {
                    Properties.Settings.Default.PreferencedWebBrowser = value;
                    Properties.Settings.Default.Save();
                }
            }
        }

        private bool _autoScanAfterPickingDirectory = Properties.Settings.Default.AutoScanAfterPickingDirectory;
        public bool AutoScanAfterPickingDirectory
        {
            get { return _autoScanAfterPickingDirectory; }
            set
            {
                if (Set(ref _autoScanAfterPickingDirectory, value))
                {
                    Properties.Settings.Default.AutoScanAfterPickingDirectory = value;
                    Properties.Settings.Default.Save();
                }
            }
        }

        private bool _showOpenUrlButton = Properties.Settings.Default.ShowOpenUrlButton;
        public bool ShowOpenUrlButton
        {
            get { return _showOpenUrlButton; }
            set
            {
                if (Set(ref _showOpenUrlButton, value))
                {
                    Properties.Settings.Default.ShowOpenUrlButton = value;
                    Properties.Settings.Default.Save();
                }
            }
        }

        private string _pathToNuspec = Properties.Settings.Default.PathToNuspec;
        public string PathToNuspec
        {
            get { return _pathToNuspec; }
            set
            {
                if (Set(ref _pathToNuspec, value))
                {
                    Properties.Settings.Default.PathToNuspec = value;
                    Properties.Settings.Default.Save();
                }
            }
        }

        private bool _executeGitFetchWitScan = Properties.Settings.Default.ExecuteGitFetchWitScan;
        public bool ExecuteGitFetchWitScan
        {
            get { return _executeGitFetchWitScan; }
            set
            {
                if (Set(ref _executeGitFetchWitScan, value))
                {
                    FileScanner.ExecuteGitFetchWitScan = value;
                    Properties.Settings.Default.ExecuteGitFetchWitScan = value;
                    Properties.Settings.Default.Save();
                }
            }
        }


        private bool _scanAfterDirectoryChange = Properties.Settings.Default.ScanAfterDirectoryChange;
        public bool ScanAfterDirectoryChange
        {
            get { return _scanAfterDirectoryChange; }
            set
            {
                if (Set(ref _scanAfterDirectoryChange, value))
                {
                    Properties.Settings.Default.ScanAfterDirectoryChange = value;
                    Properties.Settings.Default.Save();
                }
            }
        }
    }
}
