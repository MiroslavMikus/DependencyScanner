using DependencyScanner.Core.Model;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace DependencyScanner.ViewModel
{
    public class ConsolidateSolutionsViewModel : ViewModelBase
    {
        private readonly IMessenger _messenger;

        public RelayCommand ScanCommand { get; private set; }

        private ObservableCollection<SolutionResult> _scanResult;
        public ObservableCollection<SolutionResult> ScanResult { get => _scanResult; set => Set(ref _scanResult, value); }

        public ConsolidateSolutionsViewModel(IMessenger messenger)
        {
            _messenger = messenger;

            _messenger.Register<IEnumerable<SolutionResult>>(this, a => ScanResult = new ObservableCollection<SolutionResult>(a));
        }
    }
}
