using DependencyScanner.Core;
using DependencyScanner.Core.Model;
using DependencyScanner.ViewModel.Events;
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
    public class ConsolidateProjectsViewModel : SolutionBaseViewModel<SolutionResult>
    {
        public RelayCommand<SolutionResult> ScanCommand { get; private set; }
        private readonly IMessenger _messenger;
        private readonly ProjectComparer _projectComparer;

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

                FilterScanResult = CollectionViewSource.GetDefaultView(ScanResult);

                FilterScanResult.Filter = FilterJob;
            });

            _messenger.Register<ClearResultEvent>(this, a =>
            {
                ScanResult?.Clear();
            });
        }

        protected override bool FilterJob(object value)
        {
            if (value is SolutionResult input && !string.IsNullOrEmpty(SolutionFilter))
            {
                return input.Info.Name.IndexOf(SolutionFilter, StringComparison.OrdinalIgnoreCase) >= 0;
            }
            return true;
        }
    }
}
