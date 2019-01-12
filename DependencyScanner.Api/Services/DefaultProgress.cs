using DependencyScanner.Api.Interfaces;
using System;
using System.Threading;

namespace DependencyScanner.Api.Services
{
    public class DefaultProgress : ICancelableProgress<ProgressMessage>
    {
        public Action<ProgressMessage> ReportAction { get; set; }
        public CancellationToken Token { get; set; }

        public void Report(ProgressMessage value)
        {
            ReportAction?.Invoke(value);
        }
    }
}
