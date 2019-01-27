using DependencyScanner.Api.Model;

namespace DependencyScanner.Api.Events
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
