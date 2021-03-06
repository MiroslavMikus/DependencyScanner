﻿using DependencyScanner.Core.Gui.Services;
using DependencyScanner.ViewModel;
using GalaSoft.MvvmLight.Command;
using Serilog;
using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace DependencyScanner.Standalone.Services
{
    public static class CommandManager
    {
        public static CommandManagerSettings Settings { get; internal set; }

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
                Log.Error(ex, "Run ommand: Error while executing process {process}", a);
            }
        });

        public static RelayCommand<string> OpenCmdCommand { get; } = new RelayCommand<string>(a =>
        {
            if (string.IsNullOrEmpty(a))
            {
                Log.Warning($"Can't execute {nameof(OpenCmdCommand)}, the parameter is null or empty.");

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
                Log.Error(ex, "Open terminal command: Error while executing {paht}", a);
            }
        });

        public static RelayCommand<string> OpenLinkCommand { get; } = new RelayCommand<string>(a =>
        {
            string browser = Settings.WebBrowser;

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
                Log.Error(ex, "Open link command: Error while executing {Process}", a);
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
                Log.Error(ex, "Open text file command: Error while executing {a}", a);
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
            string terminalTool = Settings.ConsoleTool;

            if (string.IsNullOrEmpty(terminalTool))
            {
                terminalTool = "cmd.exe";
            }

            return terminalTool;
        }

        private static string GetPreferedTextEditor()
        {
            string textEditor = Settings.TextEditor;

            if (string.IsNullOrEmpty(textEditor))
            {
                textEditor = "notepad";
            }

            return textEditor;
        }
    }
}
