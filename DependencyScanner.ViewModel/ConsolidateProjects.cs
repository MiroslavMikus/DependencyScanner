using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace DependencyScanner.ViewModel
{
    public class ConsolidateProjects : ViewModelBase
    {
        public RelayCommand ScanCommand { get; private set; }
    }
}
