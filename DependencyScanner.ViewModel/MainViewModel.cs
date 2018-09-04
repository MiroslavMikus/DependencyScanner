using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Serilog;
using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace DependencyScanner.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        public AppSettings MainSettings { get; } = AppSettings.Instance;
        public BrowseViewModel BrowseVM { get; }
        public ConsolidateSolutionsViewModel ConsolidateSolutionsVM { get; }
        public ConsolidateProjectsViewModel ConsolidateProjectsViewModel { get; }
        public NuspecUpdaterViewModel NuspecUpdaterViewModel { get; }
        public NugetScanViewModel NugetScanViewModel { get; }

        public MainViewModel(BrowseViewModel browseViewModel,
                             ConsolidateSolutionsViewModel consolidateSolutionsViewModel,
                             ConsolidateProjectsViewModel consolidateProjectsViewModel,
                             NuspecUpdaterViewModel nuspecUpdaterViewModel,
                             NugetScanViewModel nugetScanViewModel)
        {
            BrowseVM = browseViewModel;
            ConsolidateSolutionsVM = consolidateSolutionsViewModel;
            ConsolidateProjectsViewModel = consolidateProjectsViewModel;
            NuspecUpdaterViewModel = nuspecUpdaterViewModel;
            NugetScanViewModel = nugetScanViewModel;
        }
    }
}
