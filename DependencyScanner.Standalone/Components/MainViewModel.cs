using DependencyScanner.Core.Interfaces;
using DependencyScanner.Standalone.Services;
using DependencyScanner.Standalone.Setting;
using DependencyScanner.Standalone.ViewModel;
using DependencyScanner.ViewModel;
using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace DependencyScanner.Standalone.Components
{
    public class MainViewModel : ViewModelBase
    {
        public AppSettings MainSettings { get; } = AppSettings.Instance;
        public ObservableProgress Progress { get; }

        public MainSettings Settings { get; set; }

        public string LogPath { get; }

        private string _notification;

        private IEnumerable<SettingsViewModel> _settingsList;

        public IEnumerable<SettingsViewModel> SettingsList
        {
            get { return _settingsList; }
            set { Set(ref _settingsList, value); }
        }

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
                             MainSettings settings,
                             string logPath)
        {
            Plugins = plugins.OrderBy(a => a.Order);

            Settings = settings;

            SettingsList = new List<SettingsViewModel>()
            {
                new SettingsViewModel("View settings", new MainSettingsView
                {
                    DataContext = settings
                })
            }.Concat(ReadSettings(Plugins));

            Progress = progress;

            LogPath = logPath;

            eventSink.NotifyEvent += (s, e) =>
            {
                Notification = e;
            };
        }

        private IEnumerable<SettingsViewModel> ReadSettings(IEnumerable<IPlugin> plugins)
        {
            return plugins.OfType<IPlugin<ISettings>>().Select(a => new SettingsViewModel(a.Title, a.SettingsView));
        }
    }
}
