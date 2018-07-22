using DependencyScanner.Core;
using DependencyScanner.Core.Model;
using DependencyScanner.ViewModel.Events;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace DependencyScanner.ViewModel
{
    public class ConsolidateProjectsViewModel : ViewModelBase
    {
        public RelayCommand<SolutionResult> ScanCommand { get; private set; }
        private readonly IMessenger _messenger;
        private readonly ProjectComparer _projectComparer;

        private ObservableCollection<SolutionResult> _scanResult;
        public ObservableCollection<SolutionResult> ScanResult { get => _scanResult; set => Set(ref _scanResult, value); }

        private ObservableCollection<ConsolidateProject> _resultReferences;
        public ObservableCollection<ConsolidateProject> ResultReferences { get => _resultReferences; set => Set(ref _resultReferences, value); }

        public ConsolidateProjectsViewModel(IMessenger messenger, ProjectComparer projectComparer)
        {
            _messenger = messenger;
            _projectComparer = projectComparer;

            ScanCommand = new RelayCommand<SolutionResult>(a =>
            {
                if (a == null) return;

                var result = _projectComparer.FindConsolidateReferences(a);

                ResultReferences = new ObservableCollection<ConsolidateProject>(result.ToList());
            });

            _messenger.Register<IEnumerable<SolutionResult>>(this, a =>
            {
                ScanResult = new ObservableCollection<SolutionResult>(a);
            });

            _messenger.Register<ClearResultEvent>(this, a =>
            {
                ScanResult.Clear();
            });
        }
    }
}
