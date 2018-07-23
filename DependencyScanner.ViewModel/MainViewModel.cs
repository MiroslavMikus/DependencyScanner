using DependencyScanner.Core.Model;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using NuGet;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DependencyScanner.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        public RelayCommand<string> RunCommand { get; }
        public RelayCommand<string> OpenCmdCommand { get; }

        public BrowseViewModel BrowseVM { get; }
        public ConsolidateSolutionsViewModel ConsolidateSolutionsVM { get; }
        public ConsolidateProjectsViewModel ConsolidateProjectsViewModel { get; }

        public MainViewModel()
        {
            BrowseVM = new BrowseViewModel();
        }

        public MainViewModel(BrowseViewModel browseViewModel,
                             ConsolidateSolutionsViewModel consolidateSolutionsViewModel,
                             ConsolidateProjectsViewModel consolidateProjectsViewModel)
        {
            BrowseVM = browseViewModel;
            ConsolidateSolutionsVM = consolidateSolutionsViewModel;
            ConsolidateProjectsViewModel = consolidateProjectsViewModel;

            RunCommand = new RelayCommand<string>(a =>
            {
                try
                {
                    Process.Start(a);
                }
                catch
                {
                }
            });

            OpenCmdCommand = new RelayCommand<string>(a =>
            {
                var startInfo = new ProcessStartInfo
                {
                    WorkingDirectory = a,
                    WindowStyle = ProcessWindowStyle.Normal,
                    FileName = "cmd.exe",
                };


                try
                {
                    Process.Start(startInfo);
                }
                catch
                {
                }
            });
        }
    }
}
