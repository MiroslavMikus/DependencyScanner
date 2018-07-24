using DependencyScanner.ViewModel.Converters;
using System.Windows;

namespace DependencyScanner.Standalone.Converters
{
    public class NullToVisibilityConverter : AbstractNullConverter<Visibility>
    {
        public override Visibility Positive { get; set; }
        public override Visibility Negative { get; set; }
    }
}
