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
        public RelayCommand ClearNuspec { get; }
        public RelayCommand<string> OpenCmdCommand { get; }
        public RelayCommand<string> OpenLinkCommand { get; }
        public RelayCommand<string> OpenTextFileCommand { get; }

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
                if (string.IsNullOrEmpty(a))
                {
                    return;
                    // todo log here
                }

                try
                {
                    Process.Start(a);
                }
                catch (Exception ex)
                {
                    // todo log here
                }
            });

            OpenCmdCommand = new RelayCommand<string>(a =>
            {
                if (string.IsNullOrEmpty(a))
                {
                    return;
                    // todo log here
                }

                var startInfo = new ProcessStartInfo
                {
                    WorkingDirectory = a,
                    WindowStyle = ProcessWindowStyle.Normal,
                    FileName = GetTerminalTool()
                };

                try
                {
                    Process.Start(startInfo);
                }
                catch
                {
                    // todo log here
                }
            });

            OpenLinkCommand = new RelayCommand<string>(a =>
            {
                string browser = AppSettings.Instance.PreferencedWebBrowser;
                try
                {
                    if (!string.IsNullOrEmpty(browser))
                    {
                        Process.Start(browser, a);
                    }
                    else
                    {
                        Process.Start(a);
                    }
                }
                catch (Exception ex)
                {
                    // todo log here
                }
            });

            ClearNuspec = new RelayCommand(() =>
            {
                string path = AppSettings.Instance.PathToNuspec;

                if (string.IsNullOrEmpty(path))
                {
                    return;
                    // todo log here
                }

                var startInfo = new ProcessStartInfo
                {
                    WindowStyle = ProcessWindowStyle.Normal,
                    FileName = path,
                    Arguments = "locals all -clear"
                };

                try
                {
                    Process.Start(startInfo);
                }
                catch
                {
                    // log here!
                }
            });

            OpenTextFileCommand = new RelayCommand<string>(a =>
            {
                //if (string.IsNullOrEmpty(a))
                //{
                //    return;
                //    // todo log here
                //}

                var startInfo = new ProcessStartInfo
                {
                    Arguments = a,
                    WindowStyle = ProcessWindowStyle.Normal,
                    FileName = GetPreferedTextEditor()
                };

                try
                {
                    Process.Start(startInfo);
                }
                catch
                {
                    // todo log here
                }
            });
        }

        private string GetTerminalTool()
        {
            string terminalTool = AppSettings.Instance.PreferedConsoleTool;

            if (string.IsNullOrEmpty(terminalTool))
            {
                terminalTool = "cmd.exe";
            }

            return terminalTool;
        }

        private string GetPreferedTextEditor()
        {
            string terminalTool = AppSettings.Instance.PreferedTextEditor;

            if (string.IsNullOrEmpty(terminalTool))
            {
                terminalTool = "notepad";
            }

            return terminalTool;
        }
    }
}
