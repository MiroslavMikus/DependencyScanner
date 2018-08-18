﻿using DependencyScanner.Core;
using GalaSoft.MvvmLight;

namespace DependencyScanner.ViewModel
{
    public class AppSettings : ViewModelBase
    {
        static AppSettings()
        {
            FileScanner.ExecuteGitFetchWithScan = Properties.Settings.Default.ExecuteGitFetchWithScan;
        }

        private static AppSettings _instance;
        public static AppSettings Instance { get => _instance ?? (_instance = new AppSettings()); }

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

        private bool _showOpenFileButton = Properties.Settings.Default.ShowOpenFileButton;

        public bool ShowOpenFileButton
        {
            get { return _showOpenFileButton; }
            set
            {
                if (Set(ref _showOpenFileButton, value))
                {
                    Properties.Settings.Default.ShowOpenFileButton = value;
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

        private bool _executeGitFetchWitScan = Properties.Settings.Default.ExecuteGitFetchWithScan;

        public bool ExecuteGitFetchWitScan
        {
            get { return _executeGitFetchWitScan; }
            set
            {
                if (Set(ref _executeGitFetchWitScan, value))
                {
                    FileScanner.ExecuteGitFetchWithScan = value;
                    Properties.Settings.Default.ExecuteGitFetchWithScan = value;
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

        private string _preferedTextEditor = Properties.Settings.Default.PreferedTextEditor;

        public string PreferedTextEditor
        {
            get { return _preferedTextEditor; }
            set
            {
                if (Set(ref _preferedTextEditor, value))
                {
                    Properties.Settings.Default.PreferedTextEditor = value;
                    Properties.Settings.Default.Save();
                }
            }
        }
    }
}
