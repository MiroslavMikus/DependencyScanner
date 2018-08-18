﻿using DependencyScanner.Core;
using DependencyScanner.Core.Model;
using DependencyScanner.ViewModel.Events;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        private bool _filterForConsolidates;

        public bool FilterForSelected
        {
            get => _filterForConsolidates;
            set
            {
                Set(ref _filterForConsolidates, value);
                FilterScanResult?.Refresh();
            }
        }

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
            bool filterName = true;
            bool filterConsolidates = true;
            if (value is SolutionSelectionViewModel input)
            {
                if (!string.IsNullOrEmpty(SolutionFilter))
                {
                    filterName = input.Result.Info.Name.IndexOf(SolutionFilter, StringComparison.OrdinalIgnoreCase) >= 0;
                }

                if (FilterForSelected)
                {
                    filterConsolidates = input.IsSelected;
                }
            }
            return filterName && filterConsolidates;
        }
    }
}
