using DependencyScanner.Standalone.Converters;

namespace DependencyScanner.Standalone.Converters
{
    public class NullToBoolConverter : AbstractNullConverter<bool>
    {
        public override bool Positive { get; set; } = true;
        public override bool Negative { get; set; } = false;
    }
}
