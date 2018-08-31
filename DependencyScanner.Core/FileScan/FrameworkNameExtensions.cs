using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Versioning;

namespace DependencyScanner.Core.FileScan
{
    public static class FrameworkNameExtensions
    {
        /// <summary>
        /// Compares framewokr name indentifier case invariant. This method ignores version.
        /// To compare version as well use <see cref="FrameworkName.Equals(FrameworkName)"/> method.
        /// </summary>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <returns></returns>
        public static bool IsSame(this FrameworkName first, FrameworkName second)
        {
            return string.Equals(first.Identifier, second.Identifier, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Return false if first is smaller than second.
        /// Return null if first equals second.
        /// Return true if first is greather than second
        /// </summary>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <returns></returns>
        public static bool? Compare(this FrameworkName first, FrameworkName second)
        {
            if (first.Version == second.Version)
            {
                return null;
            }

            return first.Version > second.Version;
        }

        public static FrameworkName FindFramework(IEnumerable<FrameworkName> input, string identifier)
        {
            return input.FirstOrDefault(a => a.Identifier == identifier);
        }
    }
}
