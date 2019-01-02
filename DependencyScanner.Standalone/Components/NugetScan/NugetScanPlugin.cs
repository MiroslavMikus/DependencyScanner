using DependencyScanner.Core.Interfaces;
using DependencyScanner.Standalone.Components.Browse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace DependencyScanner.Standalone.Components.NugetScan
{
    public class NugetScanPlugin : PluginBase<NugetScanSettings>
    {
        public override string CollectionKey => "NugetScanSettings";

        public override string Title => "Nuget scan";

        public override string Description => "Scan an vizualize nuget packages dependencies";

        public NugetScanPlugin(Func<NugetScanSettings, NugetScanViewModel> viewModelCtor)
        {
            Order = 5;

            ContentView = new NugetScanView
            {
                DataContext = viewModelCtor(Settings)
            };

            SettingsView = new NugetScanSettingsView
            {
                DataContext = Settings
            };
        }
    }
}
