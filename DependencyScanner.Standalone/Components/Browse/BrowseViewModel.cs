using DependencyScanner.Api.Services;
using DependencyScanner.Core.Interfaces;
using DependencyScanner.Core.Model;
using DependencyScanner.Standalone.Components;
using DependencyScanner.Standalone.Components.Browse;
using DependencyScanner.Standalone.Events;
using DependencyScanner.Standalone.Services;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Threading;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DependencyScanner.ViewModel
{
    public class BrowseViewModel : FilterViewModelBase<SolutionResult, ProjectResult>
    {
        private readonly IScanner _scanner;
        private readonly IMessenger _messenger;
        private readonly Serilog.ILogger _logger;
        private readonly BrowseSettings _settings;

        public ObservableProgress _globalProgress { get; }

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
                    _settings.WorkingDirectory = value;

                    Standalone.Properties.Settings.Default.Save();

                    if (_settings.ScanAfterDirectoryChange &&
                        !string.IsNullOrEmpty(value))
                    {
                        ScanCommand.Execute(null);
                    }
                }
            }
        }

        public BrowseViewModel(IScanner scanner,
                               IMessenger messenger,
                               Serilog.ILogger logger,
                               ObservableProgress progress,
                               BrowseSettings settings)
        {
            _scanner = scanner;
            _messenger = messenger;
            _logger = logger;
            _globalProgress = progress;
            _settings = settings;

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
                    _globalProgress.ProgressMessage = "Init scan";

                    _globalProgress.Progress = 0D;

                    IsScanning = _globalProgress.IsOpen = true;

                    _cancellationTokenSource = new CancellationTokenSource();

                    await Scan(_cancellationTokenSource.Token);
                }
                catch (OperationCanceledException)
                {
                }
                finally
                {
                    IsScanning = _globalProgress.IsOpen = false;

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

            if (_settings.WorkingDirectories != null)
            {
                WorkingDirectories = new ObservableCollection<string>(_settings.WorkingDirectories.OfType<string>());
            }
            else
            {
                WorkingDirectories = new ObservableCollection<string>();
            }

            WorkingDirectories.CollectionChanged += (s, e) =>
            {
                _settings.WorkingDirectories = WorkingDirectories.ToList();
            };

            if (!string.IsNullOrEmpty(_settings.WorkingDirectory))
            {
                WorkingDirectory = _settings.WorkingDirectory;
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

                _globalProgress.RegisterProgress(progress);

                var scanResult = await _scanner.ScanSolutions(WorkingDirectory, _globalProgress, _settings.ExecuteGitFetchWithScan);

                PrimaryCollection = new ObservableCollection<SolutionResult>(scanResult);

                DispatcherHelper.CheckBeginInvokeOnUI(() =>
                {
                    _messenger.Send<IEnumerable<SolutionResult>>(PrimaryCollection);
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
