using DependencyScanner.Core.Model;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace DependencyScanner.Core.FileScan
{
    internal static class ProjectReader
    {
        internal static readonly XNamespace msbuild = "http://schemas.microsoft.com/developer/msbuild/2003";

        internal static XDocument GetDocument(string path)
        {
            try
            {
                return XDocument.Load(path);
            }
            catch (FileNotFoundException ex)
            {
                Log.Error(ex, "Trying to get {path}", path);
                throw;
            }
        }

        internal static IEnumerable<ProjectReference> ReadPackageReferences(XDocument document)
        {
            try
            {
                var references = document
                    .Element(msbuild + "Project")
                    .Elements(msbuild + "ItemGroup");

                if (references.Descendants("PackageReference").Any())
                {
                    return references.Elements(msbuild + "PackageReference")
                            .Select(a => new ProjectReference(a.Attribute("Include").Value, a.Value));
                }
                else
                {
                    return Enumerable.Empty<ProjectReference>();
                }
            }
            catch (NullReferenceException)
            {
                return Enumerable.Empty<ProjectReference>();
            }
        }

        internal static string ReadFrameworkVersion(XDocument document)
        {
            try
            {
                return document
                      .Element(msbuild + "Project")
                      .Elements(msbuild + "PropertyGroup")
                      .Elements(msbuild + "TargetFrameworkVersion")
                      .Select(a => a.Value)
                      .FirstOrDefault();
            }
            catch (NullReferenceException)
            {
                return String.Empty;
            }
        }

        internal static string ReadFrameworkVersion(string path)
        {
            var docu = GetDocument(path);

            return ReadFrameworkVersion(docu);
        }
    }
}
