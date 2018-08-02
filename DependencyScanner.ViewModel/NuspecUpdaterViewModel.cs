using DependencyScanner.Core.Model;
using DependencyScanner.Core.Nuspec;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace DependencyScanner.ViewModel
{
    public class NuspecUpdaterViewModel : SolutionBaseViewModel<SolutionNuspecCheckResult>
    {
        private readonly IMessenger _messenger;
        private readonly NuspecComparer _comparer;

        public RelayCommand SearchForIssuesCommand { get; }
        public RelayCommand<ProjectNuspecResult> UpdateNuspecCommand { get; }

        private bool _filterForConsolidates;
        public bool FilterForConsolidates
        {
            get => _filterForConsolidates;
            set
            {
                Set(ref _filterForConsolidates, value);
                FilterScanResult?.Refresh();
            }
        }

        public NuspecUpdaterViewModel(IMessenger messenger, NuspecComparer comparer)
        {
            _messenger = messenger;
            _comparer = comparer;

            SearchForIssuesCommand = new RelayCommand(() =>
            {
                foreach (var sln in ScanResult)
                {
                    sln.ProjectResult = _comparer.ConsolidateSolution(sln.Result);
                }
            });

            UpdateNuspecCommand = new RelayCommand<ProjectNuspecResult>(a =>
            {
                NuspecUpdater.UpdateNuspec(a);
            });

            _messenger.Register<IEnumerable<SolutionResult>>(this, a =>
            {
                var filterSolutions = a.Where(b => b.Projects.Any(c => c.HasNuspec && c.References.Count() > 0))
                                       .Select(v => new SolutionNuspecCheckResult
                                       {
                                           Result = v
                                       });

                ScanResult = new ObservableCollection<SolutionNuspecCheckResult>(filterSolutions);

                FilterScanResult = CollectionViewSource.GetDefaultView(ScanResult);

                FilterScanResult.Filter = FilterJob;
            });
        }

        protected override bool FilterJob(object value)
        {
            bool filterName = true;
            bool filterConsolidates = true;

            if (value is SolutionNuspecCheckResult input)
            {
                if (!string.IsNullOrEmpty(SolutionFilter))
                {
                    filterName = input.Result.Info.Name.IndexOf(SolutionFilter, StringComparison.OrdinalIgnoreCase) >= 0;
                }

                if (FilterForConsolidates)
                {
                    filterConsolidates = input.HasConsolidates;
                }
            }
            return filterName && filterConsolidates;
        }
    }
}
