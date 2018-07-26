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
}
