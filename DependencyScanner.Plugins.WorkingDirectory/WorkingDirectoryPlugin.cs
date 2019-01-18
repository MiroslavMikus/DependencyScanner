using DependencyScanner.Api.Interfaces;
using DependencyScanner.Plugins.Wd.Model;
using DependencyScanner.Plugins.Wd.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Dependency.Scanner.Plugins.Wd
{
    public class WorkingDirectoryPlugin : IPlugin
    {
        private readonly WorkingDirectorySettings _settings;
        private readonly WorkingDirectorySettingsManager _settingsManager;
        private readonly WorkingDirectoryViewModel _viewModel;

        public string Title => "Working directories";

        public string Description => "Organize and browse your working directories";

        public UserControl ContentView => new WorkingDirectoryView();

        public int Order => 0;

        public WorkingDirectoryPlugin(WorkingDirectoryViewModel viewModel)
        {
            _viewModel = viewModel;
            ContentView.DataContext = _viewModel;
        }
    }
}
