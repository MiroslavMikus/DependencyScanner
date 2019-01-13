using DependencyScanner.Api.Interfaces;
using DependencyScanner.Core.Interfaces;
using System;
using System.Threading;

namespace DependencyScanner.Core.Model
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
