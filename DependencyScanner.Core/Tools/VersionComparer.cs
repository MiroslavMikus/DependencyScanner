using NuGet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DependencyScanner.Core.Tools
{
    internal static class VersionComparer
    {
        internal static bool AllAreSame(IEnumerable<SemanticVersion> versions)
        {
            if (versions.Count() == 1) return true;

            foreach (var versionSource in versions)
            {
                foreach (var versionTarget in versions)
                {
                    if (versionSource != versionTarget)
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    }
}
