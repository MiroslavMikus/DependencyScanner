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
        private List<StorageKey> _storage;

        private const string d3jsPath = "d3.v3.min.js";

        public ReportStorage(string storageDirectory)
        {
            if (!Directory.Exists(storageDirectory))
            {
                Directory.CreateDirectory(storageDirectory);
            }

            var d3jsFullPath = Path.Combine(storageDirectory, d3jsPath);

            if (!File.Exists(d3jsFullPath))
            {
                File.WriteAllText(d3jsFullPath, Properties.Resources.d3_v3_min);
            }

            _storageDirectory = storageDirectory;

            _storage = ReadStorage(_storageDirectory).ToList();
        }

        public bool Contains(string project, out IEnumerable<StorageKey> result)
        {
            result = _storage.Where(a => a.Project == project).Select(a => a);

            return result.Any();
        }

        public bool Contains(string project)
        {
            return _storage.Any(a => a.Project == project);
        }

        internal IEnumerable<StorageKey> ReadStorage(string storageDirectory)
        {
            var files = Directory.GetFiles(storageDirectory, "*.html", SearchOption.TopDirectoryOnly);

            return files.Select(a => CreateKeyValue(a));
            //.Distinct(new StorageKeyComparer());
        }

        public StorageKey Store(string report)
        {
            var fileName = string.Format(@"{0}.html", Guid.NewGuid());

            var path = Path.Combine(_storageDirectory, fileName);

            File.WriteAllText(path, report);

            var newReport = CreateKeyValue(path);

            _storage.Add(newReport);

            return newReport;
        }

        public bool Remove(StorageKey key)
        {
            //var test = _storage[key];
            return true;
        }

        private static StorageKey CreateKeyValue(string a)
        {
            var docu = XDocument.Load(a);

            var infos = docu.Nodes().OfType<XProcessingInstruction>();

            return new StorageKey(infos) { Path = a };
        }
    }
}
