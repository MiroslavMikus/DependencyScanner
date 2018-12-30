using DependencyScanner.Core.Interfaces;
using DependencyScanner.Standalone.Services;
using DependencyScanner.ViewModel;
using GalaSoft.MvvmLight;
using System.Collections.Generic;
using System.Linq;

namespace DependencyScanner.Standalone.Components
{
    public class MainViewModel : ViewModelBase
    {
        public AppSettings MainSettings { get; } = AppSettings.Instance;

        public string LogPath { get; }

        private string _notification;

        private IEnumerable<IPlugin> _plugins;

        public IEnumerable<IPlugin> Plugins
        {
            get { return _plugins; }
            set
            {
                Set(ref _plugins, value);
            }
        }

        public string Notification
        {
            get { return _notification; }
            set
            {
                Set(ref _notification, value);
                OpenNotificationBar = true;
            }
        }

        private bool _openNotificationBar;

        public bool OpenNotificationBar
        {
            get { return _openNotificationBar; }
            set { Set(ref _openNotificationBar, value); }
        }

        public MainViewModel(IEnumerable<IPlugin> plugins,
                             EventSink eventSink,
                             string logPath)
        {
            Plugins = plugins.OrderBy(a => a.Order);

            LogPath = logPath;

            eventSink.NotifyEvent += (s, e) =>
            {
                Notification = e;
            };
        }
    }
}
