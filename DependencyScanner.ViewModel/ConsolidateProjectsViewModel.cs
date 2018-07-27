using DependencyScanner.Core;
using DependencyScanner.Core.Model;
using DependencyScanner.ViewModel.Events;
using DependencyScanner.ViewModel.Tools;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Data;

namespace DependencyScanner.ViewModel
{
    public class ConsolidateProjectsViewModel : SolutionBaseViewModel<SolutionProjectCompareResult>
    {
        private readonly IMessenger _messenger;
        private readonly ProjectComparer _projectComparer;

        public RelayCommand<SolutionProjectCompareResult> ScanCommand { get; private set; }
        public RelayCommand CompareAllCommand { get; }

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

        public ConsolidateProjectsViewModel(IMessenger messenger, ProjectComparer projectComparer)
        {
            _messenger = messenger;
            _projectComparer = projectComparer;

            ScanCommand = new RelayCommand<SolutionProjectCompareResult>(a =>
            {
                if (a == null) return;

                var result = _projectComparer.FindConsolidateReferences(a.Result);

                a.ProjectResult = result;

                FilterScanResult?.Refresh();
            });

            CompareAllCommand = new RelayCommand(() =>
            {
                if (ScanResult?.Count() > 0)
                {
                    foreach (var item in ScanResult)
                    {
                        var result = _projectComparer.FindConsolidateReferences(item.Result);

                        item.ProjectResult = result;
                    }
                    FilterScanResult?.Refresh();
                }
            });

            _messenger.Register<IEnumerable<SolutionResult>>(this, a =>
            {
                ScanResult = new ObservableCollection<SolutionProjectCompareResult>(a.Select(b => new SolutionProjectCompareResult
                {
                    Result = b
                }));

                FilterScanResult = CollectionViewSource.GetDefaultView(ScanResult);

                FilterScanResult.Filter = FilterJob;
            });

            _messenger.Register<ClearResultEvent>(this, a =>
            {
                //ScanResult?.Clear();
            });
        }

        protected override bool FilterJob(object value)
        {
            bool filterName = true;
            bool filterConsolidates = true;
            if (value is SolutionProjectCompareResult input)
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
