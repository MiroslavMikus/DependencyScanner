using GalaSoft.MvvmLight.Command;
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

namespace DependencyScanner.Standalone.Templates
{
    /// <summary>
    /// Interaction logic for SelectableSolutionTemplate.xaml
    /// </summary>
    public partial class SelectableSolutionTemplate : UserControl
    {
        //public RelayCommand SelectCurrentSolution
        //{
        //    get
        //    {
        //        return new RelayCommand(() =>
        //        {
        //            CheckSolution.IsChecked ^= CheckSolution.IsChecked;
        //        });
        //    }
        //}

        public SelectableSolutionTemplate()
        {
            InitializeComponent();
        }
    }
}
