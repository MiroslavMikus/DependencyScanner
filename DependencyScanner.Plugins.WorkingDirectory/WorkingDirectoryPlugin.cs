using DependencyScanner.Api.Interfaces;
using DependencyScanner.Core.Gui.ViewModel;
using DependencyScanner.Plugins.Wd;
using DependencyScanner.Plugins.Wd.Model;
using DependencyScanner.Plugins.Wd.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace DependencyScanner.Plugins.Wd
{
    public class WorkingDirectoryPlugin : PluginBase<WorkingDirectorySettings>
    {
        private readonly WorkingDirectoryViewModel _viewModel;

        public override string Title => "Working directories";

        public override string Description => "Organize and browse your working directories";

        public override UserControl ContentView { get; protected set; }

        public override int Order => 0;

        public WorkingDirectoryPlugin(WorkingDirectoryViewModel viewModel, WorkingDirectorySettings settings)
            : base(settings)
        {
            _viewModel = viewModel;

            ContentView = new WorkingDirectoryView()
            {
                DataContext = _viewModel
            };

            SettingsView = new WorkingDirectorySettingsView
            {
                DataContext = settings
            };
        }
    }
}
