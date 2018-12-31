using DependencyScanner.Core.Interfaces;

namespace DependencyScanner.Standalone.Components.Browse
{
    public class SomeTest : ISettings
    {
        public string Id => "test";

        public string MyProperty { get; set; }
    }
}
