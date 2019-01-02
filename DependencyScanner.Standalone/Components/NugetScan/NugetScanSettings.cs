using DependencyScanner.Standalone.Setting;

namespace DependencyScanner.Standalone.Components.NugetScan
{
    public class NugetScanSettings : ObservableSettingsBase
    {
        private bool _autoOpenNugetScan = true;

        public bool AutoOpenNugetScan
        {
            get { return _autoOpenNugetScan; }
            set => Set(ref _autoOpenNugetScan, value);
        }
    }
}
