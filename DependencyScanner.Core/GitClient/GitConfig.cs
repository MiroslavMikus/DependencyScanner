using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DependencyScanner.Core.GitClient
{
    public class GitConfig
    {
        public string Content { get; private set; }
        public string RootPath { get; }
        private string ConfigPath => Path.Combine(RootPath, "config");

        public GitConfig(string path)
        {
            RootPath = path;
            RefreshContent();
        }

        public void RefreshContent() => Content = File.ReadAllText(ConfigPath);

        public string GetRemoteUrl()
        {
            return GetRemoteUrlFromConfig(Content);
        }

        internal static string GetRemoteUrlFromConfig(string config)
        {
            var lines = config.Split(new[] { '\r', '\n' }).Where(a => !string.IsNullOrEmpty(a)).ToList();

            var element = lines.FirstOrDefault(b => b.Contains("[remote \"origin\"]"));

            if (element == null) return string.Empty;

            var urlLine = lines.IndexOf(element) + 1;

            var urlText = lines[urlLine];

            var url = "url = ";

            var startAt = urlText.IndexOf(url) + url.Length;

            return urlText.Substring(startAt).Trim(' ');
        }

        public IEnumerable<string> GetBranchList() => GetBranchList(RootPath);

        internal static IEnumerable<string> GetBranchList(string rootPath)
        {
            var refsPath = Path.Combine(rootPath, "refs", "heads");

            var refs = Directory.GetFiles(refsPath, "*", SearchOption.AllDirectories);

            return refs.Select(a => a.Remove(0, refsPath.Count() + 1).Replace('\\', '/'));
        }

        public string GetCurrentBranch()
        {
            var path = Path.Combine(RootPath, "HEAD");

            return GetCurrentBranchFromConfig(File.ReadAllText(path));
        }

        internal static string GetCurrentBranchFromConfig(string config)
        {
            var result = config.Split(new[] { '\r', '\n' }).First();

            var lastIndex = result.LastIndexOf('/');

            return result.Substring(lastIndex + 1, result.Length - lastIndex - 1);
        }
    }
}
