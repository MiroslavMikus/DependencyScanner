using DependencyScanner.Api.Model;

namespace DependencyScanner.Standalone.Events
{
    public class AddWorkindDirectory
    {
        public IWorkingDirectory Directory { get; }

        public AddWorkindDirectory(IWorkingDirectory directory)
        {
            Directory = directory;
        }
    }
}
