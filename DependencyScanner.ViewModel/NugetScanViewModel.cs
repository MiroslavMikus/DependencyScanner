using DependencyScanner.Core.Model;
using DependencyScanner.Core.NugetReference;
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

        public NugetScanViewModel(NugetScanFacade nugetScan, IMessenger messenger, Serilog.ILogger logger)
        {
            _nugetScan = nugetScan;
            _messenger = messenger;
            _logger = logger;

            _messenger.Register<IEnumerable<SolutionResult>>(this, a =>
            {
                PrimaryCollectoion = new ObservableCollection<SolutionResult>(a);
            });
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
