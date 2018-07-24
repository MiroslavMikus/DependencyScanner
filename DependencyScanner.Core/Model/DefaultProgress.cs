using DependencyScanner.Core.Interfaces;
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
        public CancellationToken Token { get; set; }

        public void Report(ProgressMessage value)
        {
            ReportAction(value);
        }

        public Action<ProgressMessage> ReportAction { get; set; }
    }
}
