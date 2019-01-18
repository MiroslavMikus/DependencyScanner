using DependencyScanner.Api.Model;

namespace DependencyScanner.Standalone.Events
{
    public class RemoveWorkinbDirectory
    {
        public IWorkingDirectory Directory { get; }

        public RemoveWorkinbDirectory(IWorkingDirectory directory)
        {
            Directory = directory;
        }
    }
}
