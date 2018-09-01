using DependencyScanner.Core.Model;
using DependencyScanner.Core.Tools;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Versioning;
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

                try
                {
                    return references.Elements(msbuild + "PackageReference")
                            .Select(a => new ProjectReference(a.Attribute("Include").Value, a.Value));
                }
                catch (Exception ex)
                {
                    Log.Error(ex, "Cant find PackageReference");

                    return Enumerable.Empty<ProjectReference>();
                }
            }
            catch (NullReferenceException)
            {
                return Enumerable.Empty<ProjectReference>();
            }
        }

        internal static FrameworkName GetFrameworkName(string projectPath)
        {
            return GetFrameworkName(GetDocument(projectPath));
        }

        internal static FrameworkName GetFrameworkName(XDocument project)
        {
            if (TryReadFromPropertyGroup(project, "TargetFramework", out string framework))
            // framework will be core or standard
            {
                return CheckFramework(framework, SupportedFrameworks.Core) ?? CheckFramework(framework, SupportedFrameworks.Standard);
            }

            if (TryReadFromPropertyGroup(project, "TargetPlatformMinVersion", msbuild, out string uwpFramework))
            // framework will be UWP
            {
                var version = GetVersion(uwpFramework);

                if (version != string.Empty)
                {
                    return new FrameworkName(SupportedFrameworks.UWP, new Version(version));
                }
            }

            if (TryReadFromPropertyGroup(project, "TargetFrameworkVersion", msbuild, out string fullFramework))
            {
                var version = GetVersion(fullFramework);

                if (version != string.Empty)
                {
                    return new FrameworkName(SupportedFrameworks.DotNet, new Version(version));
                }
            }

            return new FrameworkName(SupportedFrameworks.Unknown, new Version("0.0")); ;
        }

        private static FrameworkName CheckFramework(string framework, string frameworkName)
        {
            if (framework.ContainsCaseInvariant(frameworkName))
            {
                var version = GetVersion(framework);

                if (version != string.Empty)
                {
                    return new FrameworkName(frameworkName, new Version(version));
                }
            }
            return null;
        }

        private static bool TryReadFromPropertyGroup(XDocument document, string tag, out string value)
        {
            value = document
                ?.Element("Project")
                ?.Elements("PropertyGroup")
                ?.Elements(tag)
                ?.FirstOrDefault()
                ?.Value
                ?? string.Empty;

            return value != string.Empty;
        }

        private static bool TryReadFromPropertyGroup(XDocument document, string tag, XNamespace @namespace, out string value)
        {
            value = document
                ?.Element(@namespace + "Project")
                ?.Elements(@namespace + "PropertyGroup")
                ?.Elements(@namespace + tag)
                ?.FirstOrDefault()
                ?.Value
                ?? string.Empty;

            return value != string.Empty;
        }

        /// <summary>
        /// Example input: 'netstandard2.0', netcoreapp2.1
        /// </summary>
        /// <param name="frameworkWithVersion"></param>
        /// <returns></returns>
        internal static string GetVersion(string frameworkWithVersion)
        {
            if (frameworkWithVersion.Any(a => char.IsDigit(a)))
            {
                char firstDigit = frameworkWithVersion.FirstOrDefault(a => Char.IsDigit(a));
                int versionIndex = frameworkWithVersion.IndexOf(firstDigit);

                return frameworkWithVersion.Substring(versionIndex);
            }
            return string.Empty;
        }
    }
}
