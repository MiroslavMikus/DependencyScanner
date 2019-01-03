using DependencyScanner.Standalone.Components.Browse;
using DependencyScanner.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace DependencyScanner.Standalone.ViewModel
{
    public class BrowsePlugin : PluginBase<BrowseSettings>
    {
        public override string Title => "Browse";

        public override string Description => "Scan and browse your solutions.";

        public BrowsePlugin(BrowseViewModel browseViewModel, BrowseSettings settings)
            : base(settings)
        {
            Order = 1;

            ContentView = new BrowseView
            {
                DataContext = browseViewModel
            };

            SettingsView = new BrowseSettingsView
            {
                DataContext = Settings
            };
        }
    }
}
