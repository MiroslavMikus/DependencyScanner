using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DependencyScanner.Core.GitClient
{
    public class GitParser
    {
        public static IEnumerable<string> GetBranchList(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return Enumerable.Empty<string>();
            }

            var arr = SplitString(input);

            return arr.Select(a => a.Substring(2));
        }

        public static string GetCurrentBranch(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return string.Empty;
            }

            var arr = SplitString(input);

            return arr.First(a => a.StartsWith("*")).Substring(2);
        }

        public static string GetRemoteUrl(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return string.Empty;
            }

            var startPosition = input.IndexOf("http");

            var endPosition = input.IndexOf("(");

            if(startPosition == -1 || endPosition == -1)
            {
                return string.Empty;
            }

            return input.Substring(startPosition, endPosition - startPosition).Trim();
        }

        public static IEnumerable<string> SplitString(string input) => input.Split(new string[] { Environment.NewLine }, StringSplitOptions.None).Where(a => !string.IsNullOrEmpty(a));
    }
}
