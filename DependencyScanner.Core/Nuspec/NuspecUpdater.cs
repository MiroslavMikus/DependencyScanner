using DependencyScanner.Core.FileScan;
using DependencyScanner.Core.Model;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace DependencyScanner.Core.Nuspec
{
    public static class NuspecUpdater
    {
        public static bool UpdateNuspec(ProjectNuspecResult input)
        {
            var docu = ProjectReader.GetDocument(input.Project.NuspecInfo.FullName);

            docu = UpdateNuspec(docu, input.MissingPackages, input.UselessPackages);

            var allDependencies = GetDependencies(docu);

            docu.Save(input.Project.NuspecInfo.FullName);

            return input.MissingPackages.All(a => allDependencies.Contains(a)) &&
                   input.UselessPackages.All(a => !allDependencies.Contains(a));
        }

        internal static XDocument UpdateNuspec(XDocument document, IEnumerable<string> Missing, IEnumerable<string> Useless)
        {
            if (Missing.Count() > 0)
            {
                AddDependency(document, Missing.ToArray());
            }

            if (Useless.Count() > 0)
            {
                RemoveDependency(document, Useless.ToArray());
            }

            return document;
        }

        internal static XDocument AddDependency(XDocument document, params string[] ids)
        {
            var dep = document
                .Element("package")
                .Element("metadata");

            if (!dep.Descendants("dependencies").Any())
            {
                dep.Add(new XElement("dependencies"));
            }

            dep = dep.Element("dependencies");

            foreach (var id in ids)
            {
                dep.Add(new XElement("dependency", new XAttribute("id", id)));
            }

            return document;
        }

        internal static XDocument RemoveDependency(XDocument document, params string[] ids)
        {
            var dep = document
                .Element("package")
                .Element("metadata")
                .Element("dependencies")
                .Elements("dependency")
                .Where(a => ids.Any(b => b == a.Attribute("id").Value));

            dep.Remove();

            return document;
        }

        internal static IEnumerable<string> GetDependencies(XDocument document)
        {
            try
            {
                var docu = document
                    .Element("package")
                    .Element("metadata");

                if (docu.Descendants("dependencies").Any())
                {
                    return docu.Element("dependencies")
                        .Elements("dependency")
                        .Select(a => a.Attribute("id").Value);
                }
                else
                {
                    return Enumerable.Empty<string>();
                }
            }
            catch (Exception ex)
            // occurs if the nuspec is using wrong namespace -> for example choco nuspec
            {
                Log.Error(ex, "Can't read {document}", document);
                return Enumerable.Empty<string>();
            }
        }
    }
}
