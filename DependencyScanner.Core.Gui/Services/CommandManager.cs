using DependencyScanner.Api.Interfaces;
using GalaSoft.MvvmLight.Command;
using Serilog;
using System;
using System.Diagnostics;
using System.Windows;

namespace DependencyScanner.Core.Gui.Services
{
    public class CommandManager : IService
    {
        private readonly ILogger _logger;

        private readonly CommandManagerSettings _settings;

        public CommandManager(ILogger logger, CommandManagerSettings settings)
        {
            _logger = logger;
            _settings = settings;
        }

        private RelayCommand<string> _runCommand;

        public RelayCommand<string> RunCommand
        {
            get => _runCommand ?? (_runCommand = new RelayCommand<string>(a =>
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
            }));
        }

        private RelayCommand<string> _openCmdCommand;

        public RelayCommand<string> OpenCmdCommand
        {
            get => _openCmdCommand ?? (_openCmdCommand = new RelayCommand<string>(a =>
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
                    FileName = IfNone(_settings.ConsoleTool, "cmd.exe"),
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
            }));
        }

        private RelayCommand<string> _openLinkCommand;

        public RelayCommand<string> OpenLinkCommand
        {
            get => _openLinkCommand ?? (_openLinkCommand = new RelayCommand<string>(a =>
            {
                try
                {
                    if (!string.IsNullOrEmpty(_settings.WebBrowser))
                    {
                        Log.Information("Starting process {@site}", a);

                        Process.Start(_settings.WebBrowser, a);
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
            }));
        }

        private RelayCommand<string> _openTextFileCommand;

        public RelayCommand<string> OpenTextFileCommand
        {
            get => _openTextFileCommand ?? (_openTextFileCommand = new RelayCommand<string>(a =>
            {
                var startInfo = new ProcessStartInfo
                {
                    Arguments = a,
                    WindowStyle = ProcessWindowStyle.Normal,
                    FileName = IfNone(_settings.TextEditor, "notepad")
                };

                try
                {
                    Process.Start(startInfo);
                }
                catch (Exception ex)
                {
                    Log.Error(ex, "Open text file command: Error while executing {a}", a);
                }
            }));
        }

        private RelayCommand<string> _copyToClipCommand;

        public RelayCommand<string> CopyToClipCommand
        {
            get => _copyToClipCommand ?? (_copyToClipCommand = new RelayCommand<string>(a =>
            {
                if (!string.IsNullOrEmpty(a))
                {
                    Clipboard.SetText(a);
                }
            }));
        }

        private static string IfNone(string input, string defaultInput)
        {
            return string.IsNullOrEmpty(input) ? defaultInput = "notepad" : input;
        }
    }
}
