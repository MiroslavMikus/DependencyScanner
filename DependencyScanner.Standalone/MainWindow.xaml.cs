using DependencyScanner.Standalone.ViewModel;
using GalaSoft.MvvmLight.Command;
using MahApps.Metro;
using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DependencyScanner.Standalone
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow, INotifyPropertyChanged
    {
        public GridLength SolutionWidth
        {
            get => Properties.Settings.Default.Browse_Solution_Width;
            set
            {
                Properties.Settings.Default.Browse_Solution_Width = value;
                Properties.Settings.Default.Save();
                OnPropertyChanged();
            }
        }
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
            this.AccentColors = ThemeManager.Accents
                                            .Select(a => new AccentColorMenuData() { Name = a.Name, ColorBrush = a.Resources["AccentColorBrush"] as Brush })
                                            .ToList();

            // create metro theme color menu items for the demo
            this.AppThemes = ThemeManager.AppThemes
                                           .Select(a => new AppThemeMenuData() { Name = a.Name, BorderColorBrush = a.Resources["BlackColorBrush"] as Brush, ColorBrush = a.Resources["WhiteColorBrush"] as Brush })
                                           .ToList();

            InitializeComponent();
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
