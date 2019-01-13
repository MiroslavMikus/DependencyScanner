using DependencyScanner.Api.Interfaces;
using DependencyScanner.Standalone.Setting;
using GalaSoft.MvvmLight;

namespace DependencyScanner.Standalone.Components.NugetScan
{
    public class NugetScanSettings : ObservableObject, ISettings
    {
        public string Id => "NugetScanSettings";

        private bool _autoOpenNugetScan = true;

        public bool AutoOpenNugetScan
        {
            get { return _autoOpenNugetScan; }
            set => Set(ref _autoOpenNugetScan, value);
        }
    }
}
