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
        private readonly Func<BrowseSettings, BrowseViewModel> _browseViewModelCtor;

        public override UserControl SettingsView
        {
            get => _settingsView.Value;
        }

        public override UserControl ContentView => new BrowseView
        {
            DataContext = _browseViewModelCtor(Settings)
        };

        public BrowsePlugin(Func<BrowseSettings, BrowseViewModel> browseViewModelCtor)
        {
            Order = 1;

            _settingsView = new Lazy<UserControl>(() => new BrowseSettingsView
            {
                DataContext = Settings
            });

            _browseViewModelCtor = browseViewModelCtor;
        }
    }
}
