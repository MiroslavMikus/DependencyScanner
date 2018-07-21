using DependencyScanner.Core;
using DependencyScanner.Core.Model;
using DependencyScanner.ViewModel.Events;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace DependencyScanner.ViewModel
{
    public class ConsolidateSolutionsViewModel : ViewModelBase
    {
        private readonly IMessenger _messenger;
        private readonly SolutionComparer _solutionComparer;

        public RelayCommand ScanCommand { get; }
        public RelayCommand SelectAllCommand { get; }
        public RelayCommand DeSelectAllCommand { get; }

        private ObservableCollection<SolutionSelectionViewModel> _scanResult;
        public ObservableCollection<SolutionSelectionViewModel> ScanResult { get => _scanResult; set => Set(ref _scanResult, value); }

        private ObservableCollection<ConsolidateReference> _resultReferences;
        public ObservableCollection<ConsolidateReference> ResultReferences { get => _resultReferences; set => Set(ref _resultReferences, value); }

        public ConsolidateSolutionsViewModel(IMessenger messenger, SolutionComparer solutionComparer)
        {
            _messenger = messenger;
            _solutionComparer = solutionComparer;

            ScanCommand = new RelayCommand(() =>
            {
                var solutionsToCompare = ScanResult.Where(a => a.IsSelected).Select(b => b.Result);

                var result = _solutionComparer.FindConsolidateReferences(solutionsToCompare);

                ResultReferences = new ObservableCollection<ConsolidateReference>(result);
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
            });

            _messenger.Register<ClearResultEvent>(this, a =>
            {
                ScanResult.Clear();
                //ResultReferences.Clear();
            });
        }
    }

    public class SolutionSelectionViewModel : ObservableObject
    {
        public SolutionResult Result { get; set; }

        private bool _isSelected;
        public bool IsSelected { get => _isSelected; set => Set(ref _isSelected, value); }
    }
}
