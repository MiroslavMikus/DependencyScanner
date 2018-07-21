using DependencyScanner.Core.Interfaces;
using DependencyScanner.Core.Model;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Forms;

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

        private FileInfo _workingDirectory;
        public FileInfo WorkingDirectory { get => _workingDirectory; set => Set(ref _workingDirectory, value); }

        public BrowseViewModel(IScanner scanner, IMessenger messenger)
        {
            _scanner = scanner;
            _messenger = messenger;

            PickWorkingDirectoryCommand = new RelayCommand(() =>
            {
                using (var dialog = new FolderBrowserDialog())
                {
                    DialogResult result = dialog.ShowDialog();

                    if (result == DialogResult.OK && !string.IsNullOrEmpty(dialog.SelectedPath))
                    {
                        WorkingDirectory = new FileInfo(dialog.SelectedPath);
                    }
                    WorkingDirectory = new FileInfo(@"F:\Projects\_GitHub"); // todo remove
                }
            });

            ScanCommand = new RelayCommand(() =>
            {
                var scanResult = _scanner.ScanSolutions(_workingDirectory.FullName);

                ScanResult = new ObservableCollection<SolutionResult>(scanResult);

                _messenger.Send<IEnumerable<SolutionResult>>(ScanResult);
            });
        }
    }
}
