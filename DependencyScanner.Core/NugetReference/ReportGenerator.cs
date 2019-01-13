using DependencyScanner.Api.Interfaces;
using DependencyScanner.Core.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace DependencyScanner.Core.NugetReference
{
    public class ReportGenerator : IService
    {
        public Lazy<string> LazyTemplate { get; }

        public ReportGenerator()
        {
            LazyTemplate = new Lazy<string>(() => LoadTemplate());
        }

        private string LoadTemplate()
        {
            return Properties.Resources.ReportTemplate;
        }

        public string GenerateReport(IEnumerable<NugetReferenceResult> nugetReferenceResults, string sourceProject, string appVersion)
        {
            var data = JsonConvert.SerializeObject(nugetReferenceResults);

            var template = LazyTemplate.Value;

            template = template.Replace("{sourceProject}", sourceProject);
            template = template.Replace("{appVersion}", appVersion);
            template = template.Replace("{date}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture));
            template = template.Replace("{data}", data);

            return template;
        }
    }
}
