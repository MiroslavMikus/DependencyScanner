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
using static DependencyScanner.ViewModel.Tools.DispatcherTools;
using System.Threading.Tasks;
using NuGet;
using System.ComponentModel;
using System.Windows.Data;
using System.Globalization;
using System.Threading;

namespace DependencyScanner.ViewModel
{
    public class BrowseViewModel : SolutionBaseViewModel<SolutionResult>
    {
        private readonly IScanner _scanner;
        private CancellationTokenSource _cancellationTokenSource;
        private readonly IMessenger _messenger;

        public RelayCommand PickWorkingDirectoryCommand { get; private set; }
        public RelayCommand ScanCommand { get; private set; }

        private FileInfo _workingDirectory;
        public FileInfo WorkingDirectory { get => _workingDirectory; set => Set(ref _workingDirectory, value); }

        public BrowseViewModel()
        {
            if (!IsInDesignMode)
            {
                throw new Exception("Allowed only in design time");
            }

            FileInfo FakeInfo() => new FileInfo(@"F:\Projects\_GitHub\DependencyScanner\DependencyScanner.sln");

            var reference1 = new PackageReference("Nuget", new SemanticVersion(new Version(1, 1, 1, 2)), null, new System.Runtime.Versioning.FrameworkName("dot.Net", new Version(1, 2)), false);
            var project1 = new ProjectResult(FakeInfo(), FakeInfo());
            project1.References.Add(reference1);
            var solution1 = new SolutionResult(FakeInfo());
            solution1.Projects.Add(project1);

            ScanResult = new ObservableCollection<SolutionResult>()
            {
                solution1
            };
        }

        public BrowseViewModel(IScanner scanner, IMessenger messenger)
        {
            _scanner = scanner;
            _messenger = messenger;

            if (!string.IsNullOrEmpty(Properties.Settings.Default.WorkingDirectory))
            {
                WorkingDirectory = new FileInfo(Properties.Settings.Default.WorkingDirectory);

                //Task.Run(() => Scan());
            }

            PickWorkingDirectoryCommand = new RelayCommand(() =>
            {
                using (var dialog = new FolderBrowserDialog())
                {
                    DialogResult result = dialog.ShowDialog();

                    if (result == DialogResult.OK && !string.IsNullOrEmpty(dialog.SelectedPath))
                    {
                        WorkingDirectory = new FileInfo(dialog.SelectedPath);

                        ScanResult?.Clear();

                        _messenger.Send<ClearResultEvent>(new ClearResultEvent());

                        Properties.Settings.Default.WorkingDirectory = dialog.SelectedPath;

                        Properties.Settings.Default.Save();
                    }
                }
            });

            ScanCommand = new RelayCommand(() =>
            {
                Scan();
            });
        }

        private void Scan()
        {
            DispacherInvoke(() =>
            {
                _cancellationTokenSource = new CancellationTokenSource();

                var progress = new DefaultProgress
                {
                    Token = _cancellationTokenSource.Token
                };

                progress.ReportAction = a =>
                {

                };

                var scanResult = _scanner.ScanSolutions(_workingDirectory.FullName, progress);

                ScanResult = new ObservableCollection<SolutionResult>(scanResult);

                FilterScanResult = CollectionViewSource.GetDefaultView(ScanResult);

                FilterScanResult.Filter = FilterJob;

                _messenger.Send<IEnumerable<SolutionResult>>(ScanResult);
            });
        }

        protected override bool FilterJob(object value)
        {
            if (value is SolutionResult input && !string.IsNullOrEmpty(SolutionFilter))
            {
                return input.Info.Name.IndexOf(SolutionFilter, StringComparison.OrdinalIgnoreCase) >= 0;
            }
            return true;
        }
    }
}
