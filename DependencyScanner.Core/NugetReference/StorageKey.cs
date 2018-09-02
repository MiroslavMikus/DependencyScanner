using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Xml.Linq;

namespace DependencyScanner.Core.NugetReference
{
    [DebuggerDisplay("{Date}-{Project}")]
    public struct StorageKey : IEquatable<StorageKey>
    {
        private const string ProjectInformation = "xml-sourceProject";
        private const string DateInformation = "xml-date";

        public string Project { get; }
        public DateTime Date { get; }

        public StorageKey(IEnumerable<XProcessingInstruction> instuctions)
        {
            Project = instuctions.First(b => b.Target == ProjectInformation).Data;
            Date = DateTime.Parse(instuctions.First(b => b.Target == DateInformation).Data);
        }

        public override bool Equals(object obj)
        {
            if (obj is StorageKey key)
            {
                return Equals(key);
            }
            return false;
        }

        public bool Equals(StorageKey other)
        {
            return Project == other.Project && Date == other.Date;
        }

        public override int GetHashCode()
        {
            return Project.GetHashCode() ^ 3;
        }
    }

    public class StorageKeyComparer : IEqualityComparer<KeyValuePair<StorageKey, string>>
    {
        public bool Equals(KeyValuePair<StorageKey, string> x, KeyValuePair<StorageKey, string> y)
        {
            return x.Key.Project == y.Key.Project && x.Key.Date == y.Key.Date;
        }

        public int GetHashCode(KeyValuePair<StorageKey, string> obj)
        {
            return obj.Key.Project.GetHashCode() ^ 3;
        }
    }
}
