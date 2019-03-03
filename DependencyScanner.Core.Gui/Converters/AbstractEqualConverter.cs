using System;
using System.Globalization;
using System.Windows.Data;

namespace DependencyScanner.Core.Gui.Converters
{
    public abstract class AbstractEqualConverter<T> : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value?.Equals(parameter) ?? false)
            {
                return Positive;
            }
            return Negative;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public abstract T Positive { get; set; }

        public abstract T Negative { get; set; }
    }
}
