using DependencyScanner.Api.Interfaces;
using DependencyScanner.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace DependencyScanner.Standalone.Components.Nuspec_Updater
{
    public class NuspecUpdaterPlugin : IPlugin
    {
        public string Title => "Nuspec updater";

        public string Description => "Add or remove references from your a specific nuspec file.";

        public UserControl ContentView { get; }

        public int Order => 4;

        public NuspecUpdaterPlugin(NuspecUpdaterViewModel viewModel)
        {
            ContentView = new NuspecUpdaterView
            {
                DataContext = viewModel
            };
        }
        public void OnStarted() { }

    }
}
