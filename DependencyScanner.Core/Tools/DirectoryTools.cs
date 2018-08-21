using System;
using System.IO;
using System.Linq;

namespace DependencyScanner.Core.Tools
{
    public static class DirectoryTools
    {
        /// <summary>
        /// Search for parent directory -> 'bottom to up' principe
        /// </summary>
        /// <param name="directory"></param>
        /// <param name="directorySearch"></param>
        /// <returns></returns>
        public static string SearchDirectory(string directory, Func<string, string[]> directorySearch)
        {
            var current = Directory.GetParent(directory);

            while (current != null)
            {
                var folders = directorySearch(current.FullName);

                if (folders.Count() == 0)
                {
                    current = current.Parent;
                }
                else
                {
                    return folders.First();
                }
            }
            return string.Empty;
        }
    }
}
