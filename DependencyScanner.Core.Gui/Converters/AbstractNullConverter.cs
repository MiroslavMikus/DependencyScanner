using System;
using System.Globalization;
using System.Windows.Data;

namespace DependencyScanner.Core.Gui.Converters
{
    public abstract class AbstractNullConverter<T> : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is null)
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
