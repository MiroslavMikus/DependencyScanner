using DependencyScanner.Core.Interfaces;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DependencyScanner.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        public BrowseViewModel BrowseVM { get; }
        public ConsolidateSolutionsViewModel ConsolidateSolutionsVM { get; }

        public MainViewModel(BrowseViewModel browseViewModel, ConsolidateSolutionsViewModel consolidateSolutionsViewModel)
        {
            BrowseVM = browseViewModel;
            ConsolidateSolutionsVM = consolidateSolutionsViewModel;
        }
    }

    public class BrowseViewModel : ViewModelBase
    {
        private readonly IScanner _scanner;

        public RelayCommand PickWorkingDirectoryCommand { get; private set; }
        public RelayCommand ScanCommand { get; private set; }

        private FileInfo _workingDirectory;

        public FileInfo WorkingDirectory { get => _workingDirectory; set => Set(ref _workingDirectory, value); }

        public BrowseViewModel(IScanner scanner)
        {
            _scanner = scanner;

            PickWorkingDirectoryCommand = new RelayCommand(() =>
            {
                using (var dialog = new FolderBrowserDialog())
                {
                    DialogResult result = dialog.ShowDialog();

                    if (result == DialogResult.OK && !string.IsNullOrEmpty(dialog.SelectedPath))
                    {
                        WorkingDirectory = new FileInfo(dialog.SelectedPath);
                    }
                }
            });

            ScanCommand = new RelayCommand(() =>
            {

            });
        }
    }

    public class ConsolidateSolutionsViewModel : ViewModelBase
    {
        public RelayCommand ScanCommand { get; private set; }
    }

    public class ConsolidateProjects : ViewModelBase
    {
        public RelayCommand ScanCommand { get; private set; }
    }
}
