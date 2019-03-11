using DependencyScanner.Api.Interfaces;
using DependencyScanner.Core.Interfaces;
using DependencyScanner.Standalone.Services;
using DependencyScanner.Standalone.Setting;
using DependencyScanner.Standalone.ViewModel;
using DependencyScanner.ViewModel;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Threading;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace DependencyScanner.Standalone.Components
{
    public class MainViewModel : ViewModelBase
    {
        public string LogPath { get; }
        private readonly IDialogCoordinator _dialogCoordinator;
        public AppSettings MainSettings { get; } = AppSettings.Instance;
        public ObservableProgress Progress { get; }

        public MainSettings Settings { get; set; }

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
            string logPath,
            IDialogCoordinator dialogCoordinator,
            ChocoUpdater chocoUpdater)
        {
            _dialogCoordinator = dialogCoordinator;
#if DEBUG
            CheckUpdates(chocoUpdater);
#endif
            Plugins = plugins.OrderBy(a => a.Order);

            Settings = settings;

            SettingsList = new List<SettingsViewModel>()
            {
                new SettingsViewModel("View settings", new MainSettingsView
                {
                    DataContext = settings
                })
            }
            .Concat(ReadSettings(Plugins));

            Progress = progress;

            LogPath = logPath;

            eventSink.NotifyEvent += (s, e) =>
            {
                Notification = e;
            };

            foreach (var plugin in Plugins)
            {
                plugin.OnStarted();
            }
        }

        private IEnumerable<SettingsViewModel> ReadSettings(IEnumerable<IPlugin> plugins)
        {
            return plugins.OfType<IPlugin<ISettings>>().Select(a => new SettingsViewModel(a.Title, a.SettingsView));
        }

        private void CheckUpdates(ChocoUpdater updater)
        {
            Task.Run(async () =>
            {
                if (await updater.IsNewVersionAvailable())
                {
                    var mySettings = new MetroDialogSettings()
                    {
                        DefaultButtonFocus = MessageDialogResult.Affirmative,
                        AffirmativeButtonText = "Update",
                        NegativeButtonText = "Do not update",
                        FirstAuxiliaryButtonText = "Cancel"
                    };

                    var result = await _dialogCoordinator.ShowMessageAsync(this, "Newer version was detected!", "Do you want to update dependency-scanner?",
                        MessageDialogStyle.AffirmativeAndNegativeAndSingleAuxiliary, mySettings);

                    if (result == MessageDialogResult.Affirmative)
                    {
                        updater.Update();

                        await DispatcherHelper.RunAsync(() =>
                        {
                            Application.Current.Shutdown();
                        }).Task;
                    }
                }
            });
        }
    }
}
