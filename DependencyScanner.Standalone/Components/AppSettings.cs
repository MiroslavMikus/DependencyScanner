using DependencyScanner.Core;
using GalaSoft.MvvmLight;

namespace DependencyScanner.ViewModel
{
    public class AppSettings : ViewModelBase
    {
        private static AppSettings _instance;
        public static AppSettings Instance { get => _instance ?? (_instance = new AppSettings()); }

        private bool _showCmdButton = Standalone.Properties.Settings.Default.ShowCmdButton;

        public bool ShowCmdButton
        {
            get { return _showCmdButton; }
            set
            {
                if (Set(ref _showCmdButton, value))
                {
                    Standalone.Properties.Settings.Default.ShowCmdButton = value;
                    Standalone.Properties.Settings.Default.Save();
                }
            }
        }

        private bool _showOpenFileButton = Standalone.Properties.Settings.Default.ShowOpenFileButton;

        public bool ShowOpenFileButton
        {
            get { return _showOpenFileButton; }
            set
            {
                if (Set(ref _showOpenFileButton, value))
                {
                    Standalone.Properties.Settings.Default.ShowOpenFileButton = value;
                    Standalone.Properties.Settings.Default.Save();
                }
            }
        }

        private bool _showFolderButton = Standalone.Properties.Settings.Default.ShowFolderButton;

        public bool ShowFolderButton
        {
            get { return _showFolderButton; }
            set
            {
                if (Set(ref _showFolderButton, value))
                {
                    Standalone.Properties.Settings.Default.ShowFolderButton = value;
                    Standalone.Properties.Settings.Default.Save();
                }
            }
        }

        private bool _showOpenButton = Standalone.Properties.Settings.Default.ShowOpenButton;

        public bool ShowOpenButton
        {
            get { return _showOpenButton; }
            set
            {
                if (Set(ref _showOpenButton, value))
                {
                    Standalone.Properties.Settings.Default.ShowOpenButton = value;
                    Standalone.Properties.Settings.Default.Save();
                }
            }
        }

        private string _preferedConsoleTool = Standalone.Properties.Settings.Default.PreferedConsoleTool;

        public string PreferedConsoleTool
        {
            get { return _preferedConsoleTool; }
            set
            {
                if (Set(ref _preferedConsoleTool, value))
                {
                    Standalone.Properties.Settings.Default.PreferedConsoleTool = value;
                    Standalone.Properties.Settings.Default.Save();
                }
            }
        }

        private string _preferencedWebBrowser = Standalone.Properties.Settings.Default.PreferencedWebBrowser;

        public string PreferencedWebBrowser
        {
            get { return _preferencedWebBrowser; }
            set
            {
                if (Set(ref _preferencedWebBrowser, value))
                {
                    Standalone.Properties.Settings.Default.PreferencedWebBrowser = value;
                    Standalone.Properties.Settings.Default.Save();
                }
            }
        }

        private bool _showOpenUrlButton = Standalone.Properties.Settings.Default.ShowOpenUrlButton;

        public bool ShowOpenUrlButton
        {
            get { return _showOpenUrlButton; }
            set
            {
                if (Set(ref _showOpenUrlButton, value))
                {
                    Standalone.Properties.Settings.Default.ShowOpenUrlButton = value;
                    Standalone.Properties.Settings.Default.Save();
                }
            }
        }

        private string _pathToNuspec = Standalone.Properties.Settings.Default.PathToNuspec;

        public string PathToNuspec
        {
            get { return _pathToNuspec; }
            set
            {
                if (Set(ref _pathToNuspec, value))
                {
                    Standalone.Properties.Settings.Default.PathToNuspec = value;
                    Standalone.Properties.Settings.Default.Save();
                }
            }
        }

        private string _preferedTextEditor = Standalone.Properties.Settings.Default.PreferedTextEditor;

        public string PreferedTextEditor
        {
            get { return _preferedTextEditor; }
            set
            {
                if (Set(ref _preferedTextEditor, value))
                {
                    Standalone.Properties.Settings.Default.PreferedTextEditor = value;
                    Standalone.Properties.Settings.Default.Save();
                }
            }
        }

        private bool _autoOpenNugetScan = Standalone.Properties.Settings.Default.AutoOpenNugetScan;

        public bool AutoOpenNugetScan
        {
            get { return _autoOpenNugetScan; }
            set
            {
                if (Set(ref _autoOpenNugetScan, value))
                {
                    Standalone.Properties.Settings.Default.AutoOpenNugetScan = value;
                    Standalone.Properties.Settings.Default.Save();
                }
            }
        }
    }
}
