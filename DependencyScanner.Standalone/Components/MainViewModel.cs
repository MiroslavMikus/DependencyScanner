using DependencyScanner.Standalone.Services;
using DependencyScanner.ViewModel;
using GalaSoft.MvvmLight;

namespace DependencyScanner.Standalone.Components
{
    public class MainViewModel : ViewModelBase
    {
        public AppSettings MainSettings { get; } = AppSettings.Instance;
        public NuspecUpdaterViewModel NuspecUpdaterViewModel { get; }
        public NugetScanViewModel NugetScanViewModel { get; }

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

        public MainViewModel(NuspecUpdaterViewModel nuspecUpdaterViewModel,
                             NugetScanViewModel nugetScanViewModel,
                             EventSink eventSink,
                             string logPath)
        {
            NuspecUpdaterViewModel = nuspecUpdaterViewModel;
            NugetScanViewModel = nugetScanViewModel;
            LogPath = logPath;

            eventSink.NotifyEvent += (s, e) =>
            {
                Notification = e;
            };
        }
    }
}
