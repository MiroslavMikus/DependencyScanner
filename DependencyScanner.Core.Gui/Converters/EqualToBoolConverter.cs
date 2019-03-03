namespace DependencyScanner.Core.Gui.Converters
{
    public class EqualToBoolConverter : AbstractEqualConverter<bool>
    {
        public override bool Positive { get; set; } = true;
        public override bool Negative { get; set; } = false;
    }
}
