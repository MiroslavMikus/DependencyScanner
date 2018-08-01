using Serilog;
using System;
using System.Threading;

namespace DependencyScanner.Core.Interfaces
{
    /// <summary>
    /// Cancelable progress with message
    /// </summary>
    public interface ICancelableProgress<T> : IProgress<T>
    {
        CancellationToken Token { get; }
        ILogger Logger { get; }
    }
}
