using DependencyScanner.Api.Interfaces;
using DependencyScanner.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace DependencyScanner.Standalone.Components.Consolidate_Solution
{
    public class ConsolidateSolutionsPlugin : IPlugin
    {
        public string Title => "Consolidate solutions";

        public string Description => "Consolidate solutions between your solutions";

        public UserControl ContentView { get; }

        public int Order => 3;

        public ConsolidateSolutionsPlugin(ConsolidateSolutionsViewModel viewModel)
        {
            ContentView = new ConsolidateSolutionView
            {
                DataContext = viewModel
            };
        }
    }
}
