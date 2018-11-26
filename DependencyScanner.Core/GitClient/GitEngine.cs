using DependencyScanner.Core.Interfaces;
using Serilog;
using System;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace DependencyScanner.Core.GitClient
{
    public class GitEngine : IService
    {
        public TResult GitExecute<TResult>(string workingDirectory, string command, Func<string, TResult> parse)
        {
            var engineResult = GitProcess(workingDirectory, command);

            return parse(engineResult);
        }

        public string GitProcess(string workingDirectory, params string[] parameter)
        {
            Log.Verbose("GitProcess {WorkingDirectory}, {params}", workingDirectory, parameter);

            var parameters = parameter.Aggregate((a, b) => $"{a} {b}");

            var proc = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    WorkingDirectory = workingDirectory,
                    FileName = "git",
                    Arguments = parameters,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true
                }
            };

            proc.Start();

            StringBuilder sb = new StringBuilder();

            while (!proc.StandardOutput.EndOfStream)
            {
                sb.AppendLine(proc.StandardOutput.ReadLine());
            }

            return sb.ToString();
        }
    }
}
