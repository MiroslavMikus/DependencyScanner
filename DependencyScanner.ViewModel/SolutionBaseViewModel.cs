using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DependencyScanner.ViewModel
{
    public abstract class SolutionBaseViewModel<T> : ViewModelBase
    {
        private ObservableCollection<T> _scanResult;
        public ObservableCollection<T> ScanResult { get => _scanResult; set => Set(ref _scanResult, value); }
        private string _solutionFilter;

        public string SolutionFilter
        {
            get { return _solutionFilter; }
            set
            {
                Set(ref _solutionFilter, value);
                FilterScanResult?.Refresh();
            }
        }

        private ICollectionView _filterScanResult;
        public ICollectionView FilterScanResult { get => _filterScanResult; protected set => Set(ref _filterScanResult, value); }

        protected abstract bool FilterJob(object value);
    }
}
