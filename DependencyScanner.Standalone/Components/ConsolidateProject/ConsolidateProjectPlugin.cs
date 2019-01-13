using DependencyScanner.Api.Interfaces;
using DependencyScanner.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace DependencyScanner.Standalone.Components.Consolidate_Project
{
    public class ConsolidateProjectPlugin : IPlugin
    {
        public string Title => "Consolidate projects";

        public string Description => "Consolidate nugets within your projects.";

        public UserControl ContentView { get; }

        public int Order => 2;

        public ConsolidateProjectPlugin(ConsolidateProjectsViewModel viewModel)
        {
            ContentView = new ConsolidateProjectView
            {
                DataContext = viewModel
            };
        }
    }
}
