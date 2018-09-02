using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace DependencyScanner.Core.NugetReference
{
    public class ReportGenerator
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
            template = template.Replace("{date}", DateTime.Now.ToString());
            template = template.Replace("{data}", data);

            return template;
        }
    }
}
