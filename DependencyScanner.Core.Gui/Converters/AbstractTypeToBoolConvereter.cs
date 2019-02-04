using System;
using System.Globalization;
using System.Windows.Data;

namespace DependencyScanner.Core.Gui.Converters
{
    public class AbstractTypeToBoolConvereter<T> : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is T;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
