using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DependencyScanner.Core.NugetReference
{
    public class ReportStorage
    {
        private readonly string _storageDirectory;
        private Dictionary<StorageKey, string> _storage;

        public ReportStorage(string storageDirectory)
        {
            if (!Directory.Exists(storageDirectory))
            {
                Directory.CreateDirectory(storageDirectory);
            }

            _storageDirectory = storageDirectory;

            _storage = ReadStorage(_storageDirectory);
        }

        public bool Contains(string key, out Dictionary<DateTime, string> result)
        {
            result = _storage.Where(a => a.Key.Project == key).ToDictionary(a => a.Key.Date, a => a.Value);

            return result.Any();
        }

        public bool Contains(string key)
        {
            return _storage.Where(a => a.Key.Project == key).Any();
        }

        internal Dictionary<StorageKey, string> ReadStorage(string storageDirectory)
        {
            var files = Directory.GetFiles(storageDirectory, "*.html", SearchOption.TopDirectoryOnly);

            return files.Select(a => CreateKeyValue(a))
                .Distinct(new StorageKeyComparer())
                .ToDictionary(a => a.Key, b => b.Value);
        }

        public KeyValuePair<DateTime, string> Store(string report)
        {
            var fileName = string.Format(@"{0}.html", Guid.NewGuid());

            var path = Path.Combine(_storageDirectory, fileName);

            File.WriteAllText(path, report);

            var newReport = CreateKeyValue(path);

            _storage.Add(newReport.Key, newReport.Value);

            return new KeyValuePair<DateTime, string>(newReport.Key.Date, newReport.Value);
        }

        private static KeyValuePair<StorageKey, string> CreateKeyValue(string a)
        {
            var docu = XDocument.Load(a);

            var infos = docu.Nodes().OfType<XProcessingInstruction>();

            var key = new StorageKey(infos);

            return new KeyValuePair<StorageKey, string>(key, a);
        }
    }
}
