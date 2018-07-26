using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DependencyScanner.Core.GitClient
{
    public class GitManager
    {

    }

    public class GitParser
    {
        public static IEnumerable<string> GetBranchList(string input)
        {
            var arr = SplitString(input);

            return arr.Select(a => a.Substring(2));
        }

        public static string GetCurrentBranch(string input)
        {
            var arr = SplitString(input);

            return arr.First(a => a.StartsWith("*")).Substring(2);
        }

        public static IEnumerable<string> SplitString(string input) => input.Split(new string[] { Environment.NewLine }, StringSplitOptions.None).Where(a => !string.IsNullOrEmpty(a));
    }

    public static class GitEngine
    {
        internal static string GitProcess(string workingDirectory, string parameter)
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

    public static class GitCommand
    {
        public const string BranchList = "branch";
        public const string Status = "status";
        public const string SwitchBranch = "checkout";
    }
}
