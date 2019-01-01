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

        public override string CollectionKey { get; } = "BrowseSettings";

        private Lazy<UserControl> _settingsView;

        public UserControl SettingsView
        {
            get => _settingsView.Value;
        }

        public BrowsePlugin(BrowseViewModel browseViewModel)
        {
            Order = 1;

            ContentView = new BrowseView
            {
                DataContext = browseViewModel
            };

            _settingsView = new Lazy<UserControl>(() => new BrowseSettingsView
            {
                DataContext = Settings
            });
        }
    }
}
