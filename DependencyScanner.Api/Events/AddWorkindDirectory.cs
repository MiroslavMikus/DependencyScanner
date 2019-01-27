using DependencyScanner.Api.Model;

namespace DependencyScanner.Api.Events
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
