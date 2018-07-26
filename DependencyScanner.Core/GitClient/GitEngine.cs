using System;
using System.Diagnostics;
using System.Text;

namespace DependencyScanner.Core.GitClient
{
    public static class GitEngine
    {
        public static TResult GitExecute<TResult>(string workingDirectory, string command, Func<string, TResult> parse)
        {
            var engineResult = GitProcess(workingDirectory, command);

            return parse(engineResult);
        }

        public static string GitProcess(string workingDirectory, string parameter)
        {
            var proc = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    WorkingDirectory = workingDirectory,
                    FileName = "git",
                    Arguments = parameter,
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
