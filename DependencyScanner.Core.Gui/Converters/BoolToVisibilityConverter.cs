using System.Windows;

namespace DependencyScanner.Core.Gui.Converters
{
    public class BoolToVisibilityConverter : AbstractBoolConverter<Visibility>
    {
        public override Visibility Positive { get; set; } = Visibility.Visible;
        public override Visibility Negative { get; set; } = Visibility.Hidden;
    }
}
