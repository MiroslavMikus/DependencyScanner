using System.Threading;

namespace DependencyScanner.Core.Interfaces
{
    /// <summary>
    /// Cancelable progress with message
    /// </summary>
    public interface ICancelableProgress
    {
        CancellationToken Token { get; }
        void Report(double value, string message);
    }
}
