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

        public MainWindow()
        {
            InitializeComponent();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName]string a_propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(a_propertyName));
        }
    }
}
