using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace DependencyScanner.ViewModel
{
    public class ConsolidateProjectsViewModel : ViewModelBase
    {
        public RelayCommand ScanCommand { get; private set; }
    }
}
