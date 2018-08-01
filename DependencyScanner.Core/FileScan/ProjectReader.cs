using DependencyScanner.Core.Model;
using NuGet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DependencyScanner.Core.FileScan
{
    public static class ProjectReader
    {
        internal static readonly XNamespace msbuild = "http://schemas.microsoft.com/developer/msbuild/2003";
        internal static XDocument GetDocument(string path) => XDocument.Load(path);

        internal static IEnumerable<ProjectReference> ReadPackageReferences(XDocument document)
        {
            var references = document
                .Element(msbuild + "Project")
                .Elements(msbuild + "ItemGroup")
                .Elements(msbuild + "PackageReference");

            return references.Select(a => new ProjectReference(a.Attribute("Include").Value, a.Value));
        }
    }
}
