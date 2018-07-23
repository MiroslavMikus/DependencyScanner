using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
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
    public partial class MainWindow : MetroWindow
    {
        public GridLength SolutionWidth
        {
            get => Properties.Settings.Default.Browse_Solution_Width;
            set
            {
                Properties.Settings.Default.Browse_Solution_Width = value;
                Properties.Settings.Default.Save();
            }
        }

        public MainWindow()
        {
            InitializeComponent();
        }
    }
}
