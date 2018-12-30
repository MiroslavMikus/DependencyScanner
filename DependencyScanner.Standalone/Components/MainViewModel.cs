using DependencyScanner.Standalone.Services;
using DependencyScanner.ViewModel;
using GalaSoft.MvvmLight;

namespace DependencyScanner.Standalone.Components
{
    public class MainViewModel : ViewModelBase
    {
        public AppSettings MainSettings { get; } = AppSettings.Instance;

        public string LogPath { get; }

        private string _notification;

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

        public MainViewModel(NugetScanViewModel nugetScanViewModel,
                             EventSink eventSink,
                             string logPath)
        {
            LogPath = logPath;

            eventSink.NotifyEvent += (s, e) =>
            {
                Notification = e;
            };
        }
    }
}
