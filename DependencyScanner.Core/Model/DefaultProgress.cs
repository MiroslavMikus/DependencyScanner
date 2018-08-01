using DependencyScanner.Core.Interfaces;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DependencyScanner.Core.Model
{
    public class DefaultProgress : ICancelableProgress<ProgressMessage>
    {
        public Action<ProgressMessage> ReportAction { get; set; }
        public CancellationToken Token { get; set; }
        public ILogger Logger { get; }

        public DefaultProgress(ILogger logger)
        {
            Logger = logger;
        }

        public void Report(ProgressMessage value)
        {
            ReportAction(value);
        }
    }
}
