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
        public BrowseViewModel BrowseVM { get; set; }
        public ConsolidateSolutionsViewModel ConsolidateSolutionsVM{ get; set; }
    }

    public class BrowseViewModel : ViewModelBase
    {
        public RelayCommand PickWorkingDirectoryCommand { get; private set; }

        public FileInfo WorkingDirectory { get; set; }
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
