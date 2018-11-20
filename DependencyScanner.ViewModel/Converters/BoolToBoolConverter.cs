using DependencyScanner.ViewModel.Converters;

namespace DependencyScanner.ViewModel.Converters
{
    public class BoolToBoolConverter : AbstractBoolConverter<bool>
    {
        public override bool Positive { get; set; } = true;
        public override bool Negative { get; set; } = false;
    }
}
