using DependencyScanner.Standalone.ViewModel;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Threading;
using MahApps.Metro;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace DependencyScanner.Standalone
{
    public partial class MainWindow : MetroWindow, INotifyPropertyChanged
    {
        public double WindowHeight
        {
            get => Properties.Settings.Default.Window_Height;
            set
            {
                Properties.Settings.Default.Window_Height = value;
                Properties.Settings.Default.Save();
            }
        }

        public double WindowWidth
        {
            get => Properties.Settings.Default.Windows_Width;
            set
            {
                Properties.Settings.Default.Windows_Width = value;
                Properties.Settings.Default.Save();
            }
        }

        public WindowState Window_State
        {
            get => Properties.Settings.Default.Window_State;
            set
            {
                Properties.Settings.Default.Window_State = value;
                Properties.Settings.Default.Save();
            }
        }

        public double Window_Top
        {
            get => Properties.Settings.Default.Window_Top;
            set
            {
                Properties.Settings.Default.Window_Top = value;
                Properties.Settings.Default.Save();
            }
        }

        public double Window_Left
        {
            get => Properties.Settings.Default.Window_Left;
            set
            {
                Properties.Settings.Default.Window_Left = value;
                Properties.Settings.Default.Save();
            }
        }

        public RelayCommand<string> SwitchTab { get; }

        public List<AppThemeMenuData> AppThemes { get; set; }
        public List<AccentColorMenuData> AccentColors { get; set; }

        public MainWindow()
        {
            SwitchTab = new RelayCommand<string>(a =>
            {
                if (int.TryParse(a, out int result))
                {
                    MainTabControl.SelectedIndex = result;
                }
            });

            // create accent color menu items for the demo
            AccentColors = ThemeManager.Accents
                .Select(a => new AccentColorMenuData() { Name = a.Name, ColorBrush = a.Resources["AccentColorBrush"] as Brush })
                .ToList();

            // create metro theme color menu items for the demo
            AppThemes = ThemeManager.AppThemes
                .Select(a => new AppThemeMenuData() { Name = a.Name, BorderColorBrush = a.Resources["BlackColorBrush"] as Brush, ColorBrush = a.Resources["WhiteColorBrush"] as Brush })
                .ToList();

            InitializeComponent();

#if !DEBUG
            Task.Factory.StartNew(async () =>
            {
                var updater = new ChocoUpdater();

                if (await updater.IsNewVersionAvailable())
                {
                    var mySettings = new MetroDialogSettings()
                    {
                        DefaultButtonFocus = MessageDialogResult.Affirmative,
                        AffirmativeButtonText = "Update",
                        NegativeButtonText = "Do not update",
                        FirstAuxiliaryButtonText = "Cancel"
                    };

                    var result = await this.ShowMessageAsync("Newer version was detected!", "Do you want to update dependency-scanner?",
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
            }, default(CancellationToken), TaskCreationOptions.None, TaskScheduler.FromCurrentSynchronizationContext());
#endif
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName]string a_propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(a_propertyName));
        }

        private void Settings_Click(object sender, RoutedEventArgs e)
        {
            settingsFlyout.IsOpen = true;
        }
    }
}
