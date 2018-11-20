using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;

namespace DependencyScanner.Standalone.Components
{
    public abstract class FilterViewModelBase<TPrimary, TSecondary> : ViewModelBase
    {
        private ObservableCollection<TPrimary> _primaryCollection;

        public ObservableCollection<TPrimary> PrimaryCollectoion
        {
            get
            {
                return _primaryCollection;
            }
            set
            {
                if (Set(ref _primaryCollection, value))
                {
                    PrimaryFilterResult = CollectionViewSource.GetDefaultView(value);

                    PrimaryFilterResult.Filter = PrimaryFilterJob;
                }
            }
        }

        private ObservableCollection<TSecondary> _secondaryCollection;

        public ObservableCollection<TSecondary> SecondaryCollection
        {
            get => _secondaryCollection;
            set => Set(ref _secondaryCollection, value);
        }

        private string _primaryFilter;

        public string PrimaryFilter
        {
            get { return _primaryFilter; }
            set
            {
                if (Set(ref _primaryFilter, value))
                {
                    PrimaryFilterResult?.Refresh();
                }
            }
        }

        private string _secondaryFilter;

        public string SecondaryFilter
        {
            get { return _secondaryFilter; }
            set
            {
                if (Set(ref _secondaryFilter, value))
                {
                    SecondaryFilterResult?.Refresh();
                }
            }
        }

        private ICollectionView _primaryFilterResult;

        public ICollectionView PrimaryFilterResult
        {
            get => _primaryFilterResult;
            protected set => Set(ref _primaryFilterResult, value);
        }

        private ICollectionView _secondaryFilterResult;

        public ICollectionView SecondaryFilterResult
        {
            get => _secondaryFilterResult;
            protected set => Set(ref _secondaryFilterResult, value);
        }

        public event EventHandler PrimarySelectionUpdated;

        private TPrimary _primarySelectedImtem;

        public TPrimary PrimarySelectedItem
        {
            get { return _primarySelectedImtem; }
            set
            {
                if (Set(ref _primarySelectedImtem, value) && value != null)
                {
                    SecondaryCollection = new ObservableCollection<TSecondary>(GetSecondaryCollection(value));

                    SecondaryFilterResult = CollectionViewSource.GetDefaultView(SecondaryCollection);

                    SecondaryFilterResult.Filter = SecondaryFilterJob;

                    SecondaryFilterResult?.Refresh();

                    PrimarySelectionUpdated?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        protected abstract bool PrimaryFilterJob(object value);

        protected abstract bool SecondaryFilterJob(object value);

        protected abstract IEnumerable<TSecondary> GetSecondaryCollection(TPrimary primary);
    }
}
