using System;
using System.Threading;

namespace DependencyScanner.Api.Interfaces
{
    /// <summary>
    /// Cancelable progress with message
    /// </summary>
    public interface ICancelableProgress<T> : IProgress<T>
    {
        CancellationToken Token { get; }
    }
}
