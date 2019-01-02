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
    public class MainViewModel : SettingsViewModel<MainSettings>, IDisposable
    {
        private const string SettingsCollectionName = "MainViewSettings";

        public AppSettings MainSettings { get; } = AppSettings.Instance;
        public ObservableProgress Progress { get; }

        public string LogPath { get; }

        private string _notification;

        private IEnumerable<SettingsViewHelper> _settingsList;

        public IEnumerable<SettingsViewHelper> SettingsList
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
                             ISettingsManager settingsManager,
                             string logPath)
            : base(settingsManager, SettingsCollectionName)
        {
            Plugins = plugins.OrderBy(a => a.Order);

            SettingsList = new List<SettingsViewHelper>()
            {
                new SettingsViewHelper("Preferences", new MainSettingsView
                {
                    DataContext = Settings
                })
            }.Concat(ReadSettings(Plugins));

            Progress = progress;

            LogPath = logPath;

            eventSink.NotifyEvent += (s, e) =>
            {
                Notification = e;
            };
        }

        private IEnumerable<SettingsViewHelper> ReadSettings(IEnumerable<IPlugin> plugins)
        {
            return plugins.OfType<IPlugin<ISettings>>().Select(a => new SettingsViewHelper(a.Title, a.SettingsView));
        }
    }
}
