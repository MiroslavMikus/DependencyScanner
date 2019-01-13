using DependencyScanner.Api.Interfaces;
using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Dependency.Scanner.Plugins.Browse
{
    public class WorkingDirectoryPlugin : IPlugin
    {
        public string Title => "Working directories";

        public string Description => "Organize and browse your working directories";

        public UserControl ContentView => new WorkingDirectoryView();

        public int Order => 0;

        public WorkingDirectoryPlugin()
        {
        }
    }

    public class WorkingDirectorySettings : ObservableObject, ISettings
    {
        public string Id => "WorkingDirectorySettings";
    }
}
