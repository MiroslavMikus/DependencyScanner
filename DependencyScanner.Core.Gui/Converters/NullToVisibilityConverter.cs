using System.Windows;

namespace DependencyScanner.Core.Gui.Converters
{
    public class NullToVisibilityConverter : AbstractNullConverter<Visibility>
    {
        public override Visibility Positive { get; set; }
        public override Visibility Negative { get; set; }
    }
}
