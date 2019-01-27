using System.Collections.Generic;

namespace DependencyScanner.Api.Model
{
    public interface IWorkingDirectory : ISyncable
    {
        string Name { get; set; }
        ICollection<IRepository> Repositories { get; set; }
        string Path { get; set; }
    }
}
