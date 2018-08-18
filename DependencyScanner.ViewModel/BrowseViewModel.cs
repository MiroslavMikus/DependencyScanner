using DependencyScanner.Core.Interfaces;
using DependencyScanner.Core.Model;
using DependencyScanner.ViewModel.Events;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.IO;
using System.Windows.Forms;
using System.Threading.Tasks;
using NuGet;
using System.ComponentModel;
using System.Windows.Data;
using System.Globalization;
using System.Threading;
using System.Collections.Specialized;
using System.Linq;
using GalaSoft.MvvmLight.Threading;

namespace DependencyScanner.ViewModel
{
    public class BrowseViewModel : FilterViewModelBase<SolutionResult, ProjectResult>
    {
        private readonly IScanner _scanner;
        private readonly IMessenger _messenger;
        private readonly Serilog.ILogger _logger;
        private CancellationTokenSource _cancellationTokenSource;

        private bool _isScanning;
        public bool IsScanning { get => _isScanning; set => Set(ref _isScanning, value); }

        private double _progress;
        public double Progress { get => _progress; set => Set(ref _progress, value); }

        private string _progressMessage = "";
        public string ProgressMessage { get => _progressMessage; set => Set(ref _progressMessage, value); }

        public RelayCommand PickWorkingDirectoryCommand { get; private set; }
        public RelayCommand ScanCommand { get; private set; }
        public RelayCommand CancelCommand { get; private set; }
        public RelayCommand<string> RemoveWorkingDirectoryCommand { get; private set; }

        private ObservableCollection<string> _workingDirectories;
        public ObservableCollection<string> WorkingDirectories { get => _workingDirectories; set => Set(ref _workingDirectories, value); }

        private string _workingDirectory = null;
        public string WorkingDirectory
        {
            get => _workingDirectory;
            set
            {
                if (Set(ref _workingDirectory, value))
                {
                    Properties.Settings.Default.WorkingDirectory = value;

                    Properties.Settings.Default.Save();

                    if (AppSettings.Instance.ScanAfterDirectoryChange &&
                        !string.IsNullOrEmpty(value))
                    {
                        ScanCommand.Execute(null);
                    }
                }
            }
        }

        public BrowseViewModel(IScanner scanner, IMessenger messenger, Serilog.ILogger logger)
        {
            _scanner = scanner;
            _messenger = messenger;
            _logger = logger;

            PickWorkingDirectoryCommand = new RelayCommand(() =>
            {
                DispatcherHelper.CheckBeginInvokeOnUI(() =>
                {
                    using (var dialog = new FolderBrowserDialog())
                    {
                        DialogResult result = dialog.ShowDialog();

                        if (result == DialogResult.OK && !string.IsNullOrEmpty(dialog.SelectedPath))
                        {
                            WorkingDirectory = dialog.SelectedPath;

                            //ScanResult?.Clear();

                            _messenger.Send<ClearResultEvent>(new ClearResultEvent());

                            if (!WorkingDirectories.Contains(WorkingDirectory))
                            {
                                WorkingDirectories.Add(WorkingDirectory);
                            }
                        }
                    }
                });
            });

            ScanCommand = new RelayCommand(async () =>
            {
                try
                {
                    ProgressMessage = "Init scan";

                    Progress = 0D;

                    IsScanning = true;

                    _cancellationTokenSource = new CancellationTokenSource();

                    await Scan(_cancellationTokenSource.Token);
                }
                catch (OperationCanceledException)
                {
                }
                finally
                {
                    IsScanning = false;

                    _cancellationTokenSource = null;
                }
            });

            CancelCommand = new RelayCommand(() =>
            {
                _cancellationTokenSource?.Cancel();
            });

            RemoveWorkingDirectoryCommand = new RelayCommand<string>(a =>
            {
                if (string.IsNullOrEmpty(a))
                {
                    return;
                }

                if (a == WorkingDirectory)
                {
                    WorkingDirectory = null;
                }

                WorkingDirectories.Remove(a);
            });


            if (Properties.Settings.Default.WorkingDirectories != null)
            {
                WorkingDirectories = new ObservableCollection<string>(Properties.Settings.Default.WorkingDirectories.OfType<string>());
            }
            else
            {
                WorkingDirectories = new ObservableCollection<string>();
            }

            WorkingDirectories.CollectionChanged += (s, e) =>
            {
                var collection = new StringCollection();

                foreach (var item in WorkingDirectories)
                {
                    collection.Add(item);
                }

                Properties.Settings.Default.WorkingDirectories = collection;

                Properties.Settings.Default.Save();
            };

            if (!string.IsNullOrEmpty(Properties.Settings.Default.WorkingDirectory))
            {
                WorkingDirectory = Properties.Settings.Default.WorkingDirectory;
            }
        }

        private Task Scan(CancellationToken Token)
        {
            return Task.Run(async () =>
            {
                var progress = new DefaultProgress()
                {
                    Token = Token
                };

                progress.ReportAction = a =>
                {
                    Progress = a.Value;
                    ProgressMessage = a.Message;
                };

                var scanResult = await _scanner.ScanSolutions(WorkingDirectory, progress);

                PrimaryCollectoion = new ObservableCollection<SolutionResult>(scanResult);

                DispatcherHelper.CheckBeginInvokeOnUI(() =>
                {
                    _messenger.Send<IEnumerable<SolutionResult>>(PrimaryCollectoion);
                });
            });
        }

        protected override bool PrimaryFilterJob(object value)
        {
            if (!string.IsNullOrEmpty(PrimaryFilter) && value is SolutionResult input)
            {
                return input.Info.Name.IndexOf(PrimaryFilter, StringComparison.OrdinalIgnoreCase) >= 0;
            }
            return true;
        }

        protected override bool SecondaryFilterJob(object value)
        {
            if (!string.IsNullOrEmpty(SecondaryFilter) && value is ProjectResult input)
            {
                return input.ProjectInfo.Name.IndexOf(SecondaryFilter, StringComparison.OrdinalIgnoreCase) >= 0;
            }
            return true;
        }

        protected override IEnumerable<ProjectResult> GetSecondaryCollection(SolutionResult primary)
        {
            return primary.Projects;
        }
    }
}
