using DependencyScanner.Api.Interfaces;
using DependencyScanner.Plugins.Wd.Services;
using GalaSoft.MvvmLight;
using System.Collections.Generic;

namespace DependencyScanner.Plugins.Wd.Model
{
    public class WorkingDirectorySettings : ObservableObject, ISettings
    {
        public string Id => "WorkingDirectorySettings";
        public List<StorableWorkingDirectory> WorkingDirectoryStructure { get; set; } = new List<StorableWorkingDirectory>();
        public bool ExecuteGitFetchWhileScanning { get; set; } = true;
        //public List<DashboardElement> Dashboards { get; set; }
    }

    public class DashboardElement
    {
        public int Order { get; set; }
        public string Title { get; set; }
        public List<DashboardLine> Lines { get; set; }
    }

    public class DashboardLine
    {
        public string Process { get; set; }
        public string StringFormat { get; set; }
    }

    public class ButtonData
    {
        public int Order { get; set; }
        public string ToolTip { get; set; }
        public string Description { get; set; }
        public string Icon { get; set; }
        public string Process { get; set; }
        public bool RefreshDashboard { get; set; }
    }
}
