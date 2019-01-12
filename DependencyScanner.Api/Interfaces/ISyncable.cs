using System.Windows.Input;

namespace DependencyScanner.Api.Model
{
    public interface ISyncable
    {
        ICommand PullCommand { get; }
        ICommand CancelCommand { get; }
    }
}
