using DependencyScanner.Api.Interfaces;
using DependencyScanner.Standalone.Components.Browse;
using DependencyScanner.ViewModel;
using System.Windows.Controls;

namespace DependencyScanner.Standalone.ViewModel
{
    public class BrowsePlugin : IPlugin
    {
        public string Title => "Browse";

        public string Description => "Scan and browse your solutions.";
        public int Order => 1;

        public UserControl ContentView { get; }

        public BrowsePlugin(BrowseViewModel browseViewModel)
        {
            ContentView = new BrowseView
            {
                DataContext = browseViewModel
            };
        }

        public void OnStarted()
        {
        }
    }
}
