using DependencyScanner.Core.Interfaces;
using DependencyScanner.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace DependencyScanner.Standalone.Components.Browse
{
    public class BrowsePlugin : IPlugin
    {
        public string Title => "Browse";

        public string Description => "Scan and browse your solutions.";

        public UserControl ContentView { get; }

        public int Order => 1;

        public BrowsePlugin(BrowseViewModel browseViewModel)
        {
            ContentView = new BrowseView
            {
                DataContext = browseViewModel
            };
        }
    }
}
