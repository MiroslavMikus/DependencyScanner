using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DependencyScanner.Core.Tools
{
    public static class StringExtensions
    {
        public static bool ContainsCaseInvariant(this string first, string second)
        {
            return CultureInfo.CurrentCulture.CompareInfo.IndexOf(first, second, CompareOptions.IgnoreCase) >= 0;
        }
    }
}
