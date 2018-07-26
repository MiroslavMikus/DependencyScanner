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
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Data;

namespace DependencyScanner.ViewModel
{
    public class ConsolidateSolutionsViewModel : SolutionBaseViewModel<SolutionSelectionViewModel>
    {
        private readonly IMessenger _messenger;
        private readonly SolutionComparer _solutionComparer;

        public RelayCommand ScanCommand { get; }
        public RelayCommand SelectAllCommand { get; }
        public RelayCommand DeSelectAllCommand { get; }

        private ObservableCollection<ConsolidateSolution> _resultReferences;
        public ObservableCollection<ConsolidateSolution> ResultReferences { get => _resultReferences; set => Set(ref _resultReferences, value); }

        public ConsolidateSolutionsViewModel(IMessenger messenger, SolutionComparer solutionComparer)
        {
            _messenger = messenger;
            _solutionComparer = solutionComparer;

            ScanCommand = new RelayCommand(() =>
            {
                var solutionsToCompare = ScanResult.Where(a => a.IsSelected).Select(b => b.Result);

                var result = _solutionComparer.FindConsolidateReferences(solutionsToCompare);

                ResultReferences = new ObservableCollection<ConsolidateSolution>(result);
            });

            SelectAllCommand = new RelayCommand(() =>
            {
                foreach (var item in ScanResult)
                {
                    item.IsSelected = true;
                }
                RaisePropertyChanged(nameof(ScanResult));
            });

            DeSelectAllCommand = new RelayCommand(() =>
            {
                foreach (var item in ScanResult)
                {
                    item.IsSelected = false;
                }
                RaisePropertyChanged(nameof(ScanResult));
            });

            _messenger.Register<IEnumerable<SolutionResult>>(this, a =>
            {
                var viewModels = a.Select(b => new SolutionSelectionViewModel { Result = b, IsSelected = false });

                ScanResult = new ObservableCollection<SolutionSelectionViewModel>(viewModels);

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
            if (value is SolutionSelectionViewModel input && !string.IsNullOrEmpty(SolutionFilter))
            {
                return input.Result.Info.Name.IndexOf(SolutionFilter, StringComparison.OrdinalIgnoreCase) >= 0;
            }
            return true;
        }
    }
}
