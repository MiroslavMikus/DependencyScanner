using GalaSoft.MvvmLight.Command;
using Serilog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DependencyScanner.ViewModel.Services
{
    public static class CommandManager
    {
        public static RelayCommand<string> RunCommand { get; } = new RelayCommand<string>(a =>
        {
            if (string.IsNullOrEmpty(a))
            {
                Log.Warning($"Can't execute {nameof(RunCommand)}, the parameter is null or empty");

                return;
            }

            try
            {
                Log.Information("Starting process {@command}", a);

                Process.Start(a);
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"Error while executing {nameof(RunCommand)}");
            }
        });

        public static RelayCommand ClearNuspec { get; } = new RelayCommand(() =>
        {
            string path = AppSettings.Instance.PathToNuspec;

            if (string.IsNullOrEmpty(path))
            {
                Log.Warning($"Can't execute {nameof(ClearNuspec)}, the parameter is null or empty");

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
                Log.Information("Starting process {@process}", new { startInfo.Arguments, startInfo.FileName });

                Process.Start(startInfo);
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"Error while executing {nameof(ClearNuspec)}");
            }
        });

        public static RelayCommand<string> OpenCmdCommand { get; } = new RelayCommand<string>(a =>
        {
            if (string.IsNullOrEmpty(a))
            {
                Log.Warning($"Can't execute {nameof(OpenCmdCommand)}, the parameter is null or empty");

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
                Log.Information("Starting process {@process}", new { startInfo.Arguments, startInfo.FileName });

                Process.Start(startInfo);
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"Error while executing {nameof(OpenCmdCommand)}");
            }
        });

        public static RelayCommand<string> OpenLinkCommand { get; } = new RelayCommand<string>(a =>
        {
            string browser = AppSettings.Instance.PreferencedWebBrowser;

            try
            {
                if (!string.IsNullOrEmpty(browser))
                {
                    Log.Information("Starting process {@site}", a);

                    Process.Start(browser, a);
                }
                else
                {
                    Log.Information("Starting process {@site}", a);

                    Process.Start(a);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"Error while executing {nameof(OpenLinkCommand)}");
            }
        });

        public static RelayCommand<string> OpenTextFileCommand { get; } = new RelayCommand<string>(a =>
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
                Log.Error(ex, $"Error while executing {nameof(OpenTextFileCommand)}: {a}");
            }
        });

        public static RelayCommand<string> CopyToClipCommand { get; } = new RelayCommand<string>(a =>
        {
            if (!string.IsNullOrEmpty(a))
            {
                Clipboard.SetText(a);
            }
        });

        private static string GetTerminalTool()
        {
            string terminalTool = AppSettings.Instance.PreferedConsoleTool;

            if (string.IsNullOrEmpty(terminalTool))
            {
                terminalTool = "cmd.exe";
            }

            return terminalTool;
        }

        private static string GetPreferedTextEditor()
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
