using DependencyScanner.ViewModel.Converters;
using System.Windows;

namespace DependencyScanner.Standalone.Converters
{
    public class BoolToVisibilityConverter : AbstractBoolConverter<Visibility>
    {
        public override Visibility Positive { get; set; } = Visibility.Visible;
        public override Visibility Negative { get; set; } = Visibility.Hidden;
    }
}
