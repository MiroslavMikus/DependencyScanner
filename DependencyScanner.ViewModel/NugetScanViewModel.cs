using DependencyScanner.Core.Model;
using DependencyScanner.Core.NugetReference;
using DependencyScanner.ViewModel.Model;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DependencyScanner.ViewModel
{
    public class NugetScanViewModel : FilterViewModelBase<SolutionResult, ProjectResult>
    {
        private readonly Serilog.ILogger _logger;
        private readonly IMessenger _messenger;
        private readonly NugetScanFacade _nugetScan;

        public RelayCommand<ProjectResult> GenerateReportCommand { get; }

        private ObservableCollection<Report> _reports;
        public ObservableCollection<Report> Reports { get => _reports; set => Set(ref _reports, value); }

        public NugetScanViewModel(NugetScanFacade nugetScan, IMessenger messenger, Serilog.ILogger logger)
        {
            _nugetScan = nugetScan;
            _messenger = messenger;
            _logger = logger;

            GenerateReportCommand = new RelayCommand<ProjectResult>(a =>
            {
                var result = _nugetScan.ExecuteScan(a);

                if (result.Key != null)
                {
                    UpdateReports();
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

            if (_nugetScan.Storage.Contains(path, out Dictionary<DateTime, string> result))
            {
                Reports = new ObservableCollection<Report>(result.Select(a => new Report(a)));
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
