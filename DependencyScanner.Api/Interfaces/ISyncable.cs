using System.Threading;
using System.Threading.Tasks;

namespace DependencyScanner.Api.Model
{
    public interface ISyncable
    {
        Task Sync(CancellationToken token);
    }
}
