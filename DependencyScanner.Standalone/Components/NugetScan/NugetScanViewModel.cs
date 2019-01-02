using DependencyScanner.Core.Model;
using DependencyScanner.Core.NugetReference;
using DependencyScanner.Standalone.Components.NugetScan;
using DependencyScanner.Standalone.Services;
using DependencyScanner.ViewModel;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DependencyScanner.Standalone.Components
{
    public class NugetScanViewModel : FilterViewModelBase<SolutionResult, ProjectResult>
    {
        private readonly IMessenger _messenger;
        private readonly NugetScanFacade _nugetScan;

        public RelayCommand<ProjectResult> GenerateReportCommand { get; }

        public RelayCommand<StorageKey> DeleteReportCommand { get; }

        private ObservableCollection<StorageKey> _reports;
        public ObservableCollection<StorageKey> Reports { get => _reports; set => Set(ref _reports, value); }

        public NugetScanViewModel(NugetScanFacade nugetScan, NugetScanSettings settings, IMessenger messenger)
        {
            _nugetScan = nugetScan;
            _messenger = messenger;

            GenerateReportCommand = new RelayCommand<ProjectResult>(a =>
            {
                var result = _nugetScan.ExecuteScan(a);

                if (result != null)
                {
                    UpdateReports();

                    if (settings.AutoOpenNugetScan)
                    {
                        CommandManager.OpenLinkCommand.Execute(result.Path);
                    }
                }
            });

            DeleteReportCommand = new RelayCommand<StorageKey>(a =>
            {
                if (_nugetScan.Storage.Remove(a))
                {
                    Reports.Remove(a);
                }
            });

            _messenger.Register<IEnumerable<SolutionResult>>(this, a =>
            {
                PrimaryCollectoion = new ObservableCollection<SolutionResult>(a);

                PrimarySelectionUpdated += (s, e) =>
                {
                    Reports?.Clear();

                    SecondaryFilterResult.CurrentChanged += (ss, ee) =>
                    {
                        UpdateReports();
                    };
                };
            });
        }

        private void UpdateReports()
        {
            var selected = SecondaryFilterResult.CurrentItem as ProjectResult;

            var path = selected.ProjectInfo.FullName;

            if (_nugetScan.Storage.Contains(path, out IEnumerable<StorageKey> result))
            {
                Reports = new ObservableCollection<StorageKey>(result);
            }
            else
            {
                Reports?.Clear();
            }
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
