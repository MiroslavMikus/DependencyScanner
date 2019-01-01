using DependencyScanner.Standalone.Components.Browse;
using DependencyScanner.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DependencyScanner.Standalone.ViewModel
{
    public class BrowsePlugin : PluginBase<BrowseSettings>
    {
        public override string Title => "Browse";

        public override string Description => "Scan and browse your solutions.";

        public override string CollectionKey { get; } = "BrowseSettings";

        public BrowsePlugin(BrowseViewModel browseViewModel)
        {
            Order = 1;

            ContentView = new BrowseView
            {
                DataContext = browseViewModel
            };
        }
    }
}
