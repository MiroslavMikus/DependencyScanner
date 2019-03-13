using DependencyScanner.Plugins.Wd.Model;
using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DependencyScanner.Plugins.Wd.Services
{
    public class DashboardExec : ViewModelBase
    {
        private readonly DashboardElement _dashboardElement;
        public ObservableCollection<string> DashboardLine { get; set; } = new ObservableCollection<string>();

        public DashboardExec(DashboardElement dashboardElement)
        {
            _dashboardElement = dashboardElement;

            Task.Run(async () =>
            {
                foreach (var item in dashboardElement.Lines)
                {
                }
            });
        }
    }
}
