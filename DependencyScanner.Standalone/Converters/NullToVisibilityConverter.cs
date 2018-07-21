using DependencyScanner.ViewModel.Converters;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace DependencyScanner.Standalone.Converters
{
    public class NullToVisibilityConverter : AbstractNullConverter<Visibility>
    {
        public override Visibility Positive { get; set; }
        public override Visibility Negative { get; set; }
    }
}
