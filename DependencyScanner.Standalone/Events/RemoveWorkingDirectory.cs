using DependencyScanner.Api.Model;

namespace DependencyScanner.Standalone.Events
{
    public class RemoveWorkingDirectory
    {
        public IWorkingDirectory Directory { get; }

        public RemoveWorkingDirectory(IWorkingDirectory directory)
        {
            Directory = directory;
        }
    }
}
