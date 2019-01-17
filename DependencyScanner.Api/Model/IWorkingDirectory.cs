using System.Collections.Generic;

namespace DependencyScanner.Api.Model
{
    public interface IWorkingDirectory : ISyncable
    {
        ICollection<IRepository> Repositories { get; set; }
        string Path { get; set; }
    }
}
