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
        private readonly TaskCompletionSource<string> _tcs = new TaskCompletionSource<string>();
        private StringBuilder _output = new StringBuilder();

        public AsyncProcess(ProcessStartInfo info)
        {
            _info = info;
        }

        public bool OutputHandler { get; private set; }

        public async Task<string> StartAsync()
        {
            var proc = new Process
            {
                StartInfo = _info,
                EnableRaisingEvents = true
            };

            proc.StartInfo.RedirectStandardOutput = true;

            proc.Exited += Proc_Exited;

            proc.OutputDataReceived += Proc_OutputDataReceived;

            try
            {
                proc.Start();
                proc.BeginOutputReadLine();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Execute process {FileName} : {Arguments}", _info.FileName, _info.Arguments);

                return await Task.FromResult(string.Empty);
            }

            return await _tcs.Task;
        }

        private void Proc_Exited(object sender, EventArgs e)
        {
            _tcs.SetResult(_output.ToString());
        }

        private void Proc_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            _output.AppendLine(e.Data);
        }
    }
}
