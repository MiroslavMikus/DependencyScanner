using DependencyScanner.Core.Interfaces;
using DependencyScanner.Standalone.Services;
using DependencyScanner.Standalone.Setting;
using DependencyScanner.ViewModel;
using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace DependencyScanner.Standalone.Components
{
    public class MainViewModel : ViewModelBase, IDisposable
    {
        private readonly ISettingsManager _settingsManager;

        private MainViewSettings _settings;

        public MainViewSettings Settings
        {
            get { return _settings ?? (_settings = _settingsManager.Load<MainViewSettings>("MainViewSettings")); ; }
            set { Set(ref _settings, value); }
        }

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

            Plugins = plugins.OrderBy(a => a.Order);

            Progress = progress;

            LogPath = logPath;

            eventSink.NotifyEvent += (s, e) =>
            {
                Notification = e;
            };
        }

        public void Dispose()
        {
            _settingsManager.Save(_settings);
        }
    }
}
