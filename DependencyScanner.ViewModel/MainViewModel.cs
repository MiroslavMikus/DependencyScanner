using DependencyScanner.Core.Model;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Threading;
using Serilog;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DependencyScanner.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private readonly ILogger _logger;

        public RelayCommand<string> RunCommand { get; }
        public RelayCommand ClearNuspec { get; }
        public RelayCommand<string> OpenCmdCommand { get; }
        public RelayCommand<string> OpenLinkCommand { get; }
        public RelayCommand<string> OpenTextFileCommand { get; }
        public RelayCommand<string> CopyToClipCommand { get; }

        public AppSettings MainSettings { get; } = AppSettings.Instance;
        public BrowseViewModel BrowseVM { get; }
        public ConsolidateSolutionsViewModel ConsolidateSolutionsVM { get; }
        public ConsolidateProjectsViewModel ConsolidateProjectsViewModel { get; }
        public NuspecUpdaterViewModel NuspecUpdaterViewModel { get; }

        public MainViewModel(BrowseViewModel browseViewModel,
                             ConsolidateSolutionsViewModel consolidateSolutionsViewModel,
                             ConsolidateProjectsViewModel consolidateProjectsViewModel,
                             NuspecUpdaterViewModel nuspecUpdaterViewModel,
                             ILogger logger)
        {
            BrowseVM = browseViewModel;
            ConsolidateSolutionsVM = consolidateSolutionsViewModel;
            ConsolidateProjectsViewModel = consolidateProjectsViewModel;
            NuspecUpdaterViewModel = nuspecUpdaterViewModel;

            _logger = logger;

            RunCommand = new RelayCommand<string>(a =>
            {
                if (string.IsNullOrEmpty(a))
                {
                    _logger.Warning($"Cant execute {nameof(RunCommand)}, the parameter is null or empty");

                    return;
                }

                try
                {
                    _logger.Information("Starting process {@command}", a);

                    Process.Start(a);
                }
                catch (Exception ex)
                {
                    _logger.Error(ex, $"Error while executing {nameof(RunCommand)}");
                }
            });

            OpenCmdCommand = new RelayCommand<string>(a =>
            {
                if (string.IsNullOrEmpty(a))
                {
                    _logger.Warning($"Cant execute {nameof(OpenCmdCommand)}, the parameter is null or empty");

                    return;
                }

                var startInfo = new ProcessStartInfo
                {
                    WorkingDirectory = a,
                    WindowStyle = ProcessWindowStyle.Normal,
                    FileName = GetTerminalTool(),
                    UseShellExecute = false
                };

                try
                {
                    _logger.Information("Starting process {@process}", new { startInfo.Arguments, startInfo.FileName });

                    Process.Start(startInfo);
                }
                catch (Exception ex)
                {
                    _logger.Error(ex, $"Error while executing {nameof(OpenCmdCommand)}");
                }
            });

            OpenLinkCommand = new RelayCommand<string>(a =>
            {
                string browser = AppSettings.Instance.PreferencedWebBrowser;

                try
                {
                    if (!string.IsNullOrEmpty(browser))
                    {
                        _logger.Information("Starting process {@site}", a);

                        Process.Start(browser, a);
                    }
                    else
                    {
                        _logger.Information("Starting process {@site}", a);

                        Process.Start(a);
                    }
                }
                catch (Exception ex)
                {
                    _logger.Error(ex, $"Error while executing {nameof(OpenLinkCommand)}");
                }
            });

            ClearNuspec = new RelayCommand(() =>
            {
                string path = AppSettings.Instance.PathToNuspec;

                if (string.IsNullOrEmpty(path))
                {
                    _logger.Warning($"Cant execute {nameof(ClearNuspec)}, the parameter is null or empty");

                    return;
                }

                var startInfo = new ProcessStartInfo
                {
                    WindowStyle = ProcessWindowStyle.Normal,
                    FileName = path,
                    Arguments = "locals all -clear"
                };

                try
                {
                    _logger.Information("Starting process {@process}", new { startInfo.Arguments, startInfo.FileName });

                    Process.Start(startInfo);
                }
                catch (Exception ex)
                {
                    _logger.Error(ex, $"Error while executing {nameof(ClearNuspec)}");
                }
            });

            OpenTextFileCommand = new RelayCommand<string>(a =>
            {
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
                catch (Exception ex)
                {
                    _logger.Error(ex, $"Error while executing {nameof(OpenTextFileCommand)}: {a}");
                }
            });

            CopyToClipCommand = new RelayCommand<string>(a =>
            {
                if (!string.IsNullOrEmpty(a))
                {
                    Clipboard.SetText(a);
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
