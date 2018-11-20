using System;
using System.Globalization;
using System.Windows.Data;

namespace DependencyScanner.Standalone.Converters
{
    public abstract class AbstractBoolConverter<T> : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool input && input)
            {
                return Positive;
            }
            return Negative;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }

        public abstract T Positive { get; set; }

        public abstract T Negative { get; set; }
    }
}
