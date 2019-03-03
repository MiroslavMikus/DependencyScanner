using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace DependencyScanner.Core.Gui.Converters
{
    public class BoolAndMultiValueConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            return values.All(a => a is bool input && input);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
