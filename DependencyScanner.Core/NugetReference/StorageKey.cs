using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Xml.Linq;

namespace DependencyScanner.Core.NugetReference
{
    [DebuggerDisplay("{Date}-{Project}")]
    public class StorageKey : IEquatable<StorageKey>
    {
        private const string ProjectInformation = "xml-sourceProject";
        private const string DateInformation = "xml-date";

        public string Project { get; }
        public string Path { get; internal set; }
        public DateTime Date { get; }

        public StorageKey(IEnumerable<XProcessingInstruction> instuctions)
        {
            Project = instuctions.First(b => b.Target == ProjectInformation).Data;
            Date = DateTime.Parse(instuctions.First(b => b.Target == DateInformation).Data);
        }

        public StorageKey(string project, DateTime date)
        {
            Project = project ?? throw new ArgumentNullException(nameof(project));
            Date = date;
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
}
