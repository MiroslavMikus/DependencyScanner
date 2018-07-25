using DependencyScanner.Core.Model;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using NuGet;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DependencyScanner.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        public RelayCommand<string> RunCommand { get; }
        public RelayCommand<string> OpenCmdCommand { get; }

        public AppSettings MainSettings { get; } = AppSettings.Instance;
        public BrowseViewModel BrowseVM { get; }
        public ConsolidateSolutionsViewModel ConsolidateSolutionsVM { get; }
        public ConsolidateProjectsViewModel ConsolidateProjectsViewModel { get; }

        public MainViewModel()
        {
            BrowseVM = new BrowseViewModel();
        }

        public MainViewModel(BrowseViewModel browseViewModel,
                             ConsolidateSolutionsViewModel consolidateSolutionsViewModel,
                             ConsolidateProjectsViewModel consolidateProjectsViewModel)
        {
            BrowseVM = browseViewModel;
            ConsolidateSolutionsVM = consolidateSolutionsViewModel;
            ConsolidateProjectsViewModel = consolidateProjectsViewModel;

            RunCommand = new RelayCommand<string>(a =>
            {
                try
                {
                    Process.Start(a);
                }
                catch
                {
                }
            });

            OpenCmdCommand = new RelayCommand<string>(a =>
            {
                var startInfo = new ProcessStartInfo
                {
                    WorkingDirectory = a,
                    WindowStyle = ProcessWindowStyle.Normal,
                    FileName = "cmd.exe",
                };


                try
                {
                    Process.Start(startInfo);
                }
                catch
                {
                }
            });
        }
    }

    public class AppSettings : ViewModelBase
    {
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
    }
}
