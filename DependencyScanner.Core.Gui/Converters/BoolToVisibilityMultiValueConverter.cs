using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace DependencyScanner.Core.Gui.Converters
{
    public class BoolToVisibilityMultiValueConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            bool result = true;

            foreach (var item in values)
            {
                if (item is bool input)
                {
                    result &= input;
                }
                else
                {
                    return Visibility.Collapsed;
                }
            }

            if (result)
            {
                return Visibility.Visible;
            }
            return Visibility.Collapsed;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
