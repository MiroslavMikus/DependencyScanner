using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DependencyScanner.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        public BrowseViewModel BrowseVM { get; } = new BrowseViewModel();
        public ConsolidateSolutionsViewModel ConsolidateSolutionsVM { get; } = new ConsolidateSolutionsViewModel();
    }

    public class BrowseViewModel : ViewModelBase
    {
        public RelayCommand PickWorkingDirectoryCommand { get; private set; }

        public FileInfo WorkingDirectory { get; set; }

        public BrowseViewModel()
        {
            PickWorkingDirectoryCommand = new RelayCommand(() =>
            {
                using (var dialog = new System.Windows.Forms.FolderBrowserDialog())
                {
                    System.Windows.Forms.DialogResult result = dialog.ShowDialog();
                }
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
