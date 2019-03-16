using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DependencyScanner.Api.Model
{
    public interface IWorkingDirectory : ISyncable
    {
        string Name { get; set; }
        ICollection<IRepository> Repositories { get; set; }
        string Path { get; set; }
        Task ExecuteForEachRepository(Func<IRepository, Task> repositoryAction, CancellationToken token);
    }
}
