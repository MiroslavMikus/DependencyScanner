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

namespace DependencyScanner.ViewModel
{
    public class BrowseViewModel : ViewModelBase
    {
        private readonly IScanner _scanner;
        private readonly IMessenger _messenger;

        public RelayCommand PickWorkingDirectoryCommand { get; private set; }
        public RelayCommand ScanCommand { get; private set; }

        private ObservableCollection<SolutionResult> _scanResult;
        public ObservableCollection<SolutionResult> ScanResult { get => _scanResult; set => Set(ref _scanResult, value); }

        private string _solutionFilter;

        public string SolutionFilter
        {
            get { return _solutionFilter; }
            set
            {
                Set(ref _solutionFilter, value);
                FilterScanResult.Refresh();
            }
        }

        private ICollectionView _filterScanResult;
        public ICollectionView FilterScanResult { get => _filterScanResult; private set => Set(ref _filterScanResult, value); }

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
                var scanResult = _scanner.ScanSolutions(_workingDirectory.FullName);

                ScanResult = new ObservableCollection<SolutionResult>(scanResult);

                FilterScanResult = CollectionViewSource.GetDefaultView(ScanResult);

                FilterScanResult.Filter = FilterJob;

                _messenger.Send<IEnumerable<SolutionResult>>(ScanResult);
            });
        }

        private bool FilterJob(object value)
        {
            if (value is SolutionResult input && !string.IsNullOrEmpty(SolutionFilter))
            {
                return input.Info.Name.Contains(SolutionFilter);
            }
            return true;
        }
    }
}
