using DependencyScanner.Api.Interfaces;

namespace DependencyScanner.Api.Model
{
    public interface IRepository : ISyncable
    {
        IGitInfo GitInfo { get; set; }
    }
}
