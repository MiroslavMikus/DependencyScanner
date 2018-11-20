using DependencyScanner.ViewModel.Converters;

namespace DependencyScanner.ViewModel.Converters
{
    public class NullToBoolConverter : AbstractNullConverter<bool>
    {
        public override bool Positive { get; set; } = true;
        public override bool Negative { get; set; } = false;
    }
}
