using DependencyScanner.Core.Interfaces;
using DependencyScanner.Standalone.Services;
using DependencyScanner.Standalone.Setting;
using DependencyScanner.ViewModel;
using GalaSoft.MvvmLight;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace DependencyScanner.Standalone.Components
{
    public class MainViewModel : ViewModelBase
    {
        private readonly ISettingsManager _settingsManager;

        #region window settings

        private double _windowLeft = Properties.Settings.Default.Window_Left;

        public double Window_Left
        {
            get => _windowLeft;
            set
            {
                if (Set(ref _windowLeft, value))
                {
                    Properties.Settings.Default.Window_Left = value;
                    Properties.Settings.Default.Save();
                }
            }
        }

        private double _windowTop = Properties.Settings.Default.Window_Top;

        public double Window_Top
        {
            get => _windowTop;
            set
            {
                if (Set(ref _windowTop, value))
                {
                    Properties.Settings.Default.Window_Top = value;
                    Properties.Settings.Default.Save();
                }
            }
        }

        private WindowState _windowState = Properties.Settings.Default.Window_State;

        public WindowState Window_State
        {
            get => _windowState;
            set
            {
                if (Set(ref _windowState, value))
                {
                    Properties.Settings.Default.Window_State = value;
                    Properties.Settings.Default.Save();
                }
            }
        }

        private double _windowsWidth = Properties.Settings.Default.Windows_Width;

        public double WindowWidth
        {
            get => _windowsWidth;
            set
            {
                if (Set(ref _windowsWidth, value))
                {
                    Properties.Settings.Default.Windows_Width = value;
                    Properties.Settings.Default.Save();
                }
            }
        }

        private double _windowsHeight = Properties.Settings.Default.Window_Height;

        public double WindowHeight
        {
            get => _windowsHeight;
            set
            {
                if (Set(ref _windowsHeight, value))
                {
                    Properties.Settings.Default.Window_Height = value;
                    Properties.Settings.Default.Save();
                }
            }
        }

        #endregion window settings

        public AppSettings MainSettings { get; } = AppSettings.Instance;
        public ObservableProgress Progress { get; }

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
                             ObservableProgress progress,
                             EventSink eventSink,
                             ISettingsManager settingsManager,
                             string logPath)
        {
            _settingsManager = settingsManager;

            var settings = _settingsManager.Load<MainViewSettings>("MainViewSettings");

            settings.Window_Left = 150;

            _settingsManager.Save(settings);

            Plugins = plugins.OrderBy(a => a.Order);

            Progress = progress;

            LogPath = logPath;

            eventSink.NotifyEvent += (s, e) =>
            {
                Notification = e;
            };
        }
    }
}
