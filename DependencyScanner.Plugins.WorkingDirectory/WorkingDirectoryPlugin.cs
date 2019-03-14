using DependencyScanner.Api.Events;
using DependencyScanner.Api.Interfaces;
using DependencyScanner.Core.Gui.ViewModel;
using DependencyScanner.Plugins.Wd;
using DependencyScanner.Plugins.Wd.Components.Settings;
using DependencyScanner.Plugins.Wd.Components.Working_Directory;
using DependencyScanner.Plugins.Wd.Services;
using GalaSoft.MvvmLight.Messaging;
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
        private readonly IMessenger _messenger;

        public override string Title => "Working directories";

        public override string Description => "Organize and browse your working directories";

        public override UserControl ContentView { get; protected set; }

        public override int Order => 0;

        public WorkingDirectoryPlugin(WorkingDirectoryViewModel viewModel, WorkingDirectorySettings settings, IMessenger messenger)
            : base(settings)
        {
            _viewModel = viewModel;
            _messenger = messenger;

            ContentView = new WorkingDirectoryView()
            {
                DataContext = _viewModel
            };

            SettingsView = new WorkingDirectorySettingsView
            {
                DataContext = settings
            };
        }

        public override void OnStarted()
        {
            // publish init directoreies
            foreach (var dir in _viewModel.Directories)
            {
                _messenger.Send<AddWorkindDirectory>(new AddWorkindDirectory(dir));
            }
        }
    }
}
