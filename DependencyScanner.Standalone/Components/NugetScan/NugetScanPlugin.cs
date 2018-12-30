using DependencyScanner.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace DependencyScanner.Standalone.Components.NugetScan
{
    public class NugetScanPlugin : IPlugin
    {
        public string Title => "Nuget scan";

        public string Description => "Scan an vizualize nuget packages dependencies";

        public UserControl ContentView { get; }

        public int Order => 5;

        public NugetScanPlugin(NugetScanViewModel viewModel)
        {
            ContentView = new NugetScanView
            {
                DataContext = viewModel
            };
        }
    }
}
