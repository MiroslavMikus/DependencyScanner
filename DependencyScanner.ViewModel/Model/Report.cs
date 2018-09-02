using DependencyScanner.Core.Model;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DependencyScanner.ViewModel.Model
{
    public class Report
    {
        public DateTime Date { get; }
        public string Path { get; }

        public Report(KeyValuePair<DateTime, string> keyValue)
        {
            Date = keyValue.Key;
            Path = keyValue.Value;
        }

        public Report()
        {
        }
    }
}
