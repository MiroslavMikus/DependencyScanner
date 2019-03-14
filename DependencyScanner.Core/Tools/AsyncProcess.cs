using Serilog;
using System;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace DependencyScanner.Core.Tools
{
    using Serilog;
    using System;
    using System.Diagnostics;
    using System.Text;
    using System.Threading.Tasks;

    namespace DependencyScanner.Core.Tools
    {
        public class AsyncProcess
        {
            private readonly ProcessStartInfo _info;
            private readonly TaskCompletionSource<ProcessResult> _tcs = new TaskCompletionSource<ProcessResult>();
            private StringBuilder _output = new StringBuilder();
            private StringBuilder _errors = new StringBuilder();

            public AsyncProcess(ProcessStartInfo info)
            {
                _info = info;
            }

            public bool OutputHandler { get; private set; }

            public async Task<ProcessResult> StartAsync()
            {
                var proc = new Process
                {
                    StartInfo = _info,
                    EnableRaisingEvents = true
                };

                proc.StartInfo.RedirectStandardOutput = true;

                proc.Exited += (s, e) => _tcs.SetResult(new ProcessResult(proc.ExitCode, _output.ToString(), _errors.ToString()));

                proc.OutputDataReceived += Proc_OutputDataReceived;
                proc.ErrorDataReceived += Proc_ErrorDataReceived;

                try
                {
                    proc.Start();
                    proc.BeginOutputReadLine();
                }
                catch (Exception ex)
                {
                    Log.Error(ex, "Execute process {FileName} : {Arguments}", _info.FileName, _info.Arguments);

                    return await Task.FromResult(new ProcessResult(ex));
                }

                return await _tcs.Task;
            }

            private void Proc_ErrorDataReceived(object sender, DataReceivedEventArgs e)
            {
                _errors.AppendLine(e.Data);
            }

            private void Proc_OutputDataReceived(object sender, DataReceivedEventArgs e)
            {
                _output.AppendLine(e.Data);
            }
        }
    }
}
