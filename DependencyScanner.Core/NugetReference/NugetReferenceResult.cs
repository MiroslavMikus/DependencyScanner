//                                                       id                                                version                                                            dependencies

namespace DependencyScanner.Core.NugetReference
{
    public class NugetReferenceResult
    {
        public NugetReferenceResult(string source, string target)
        {
            this.source = source;
            this.target = target;
        }

        public string source { get; set; }
        public string target { get; set; }
        public string color { get; set; }
    }
}
