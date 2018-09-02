using DependencyScanner.Core.Model;
using System;
using System.Collections.Generic;

namespace DependencyScanner.Core.NugetReference
{
    public class NugetScanFacade
    {
        public ReportGenerator ReportGenerator { get; }
        public NugetReferenceScan Scan { get; }
        public ReportStorage Storage { get; }
        public string ProductVersion { get; }

        public NugetScanFacade(ReportGenerator reportGenerator, NugetReferenceScan scan, ReportStorage storage, string productVersion)
        {
            ReportGenerator = reportGenerator ?? throw new ArgumentNullException(nameof(reportGenerator));
            Scan = scan ?? throw new ArgumentNullException(nameof(scan));
            Storage = storage ?? throw new ArgumentNullException(nameof(storage));
        }

        public KeyValuePair<DateTime, string> ExecuteScan(ProjectResult project)
        {
            var scanResult = Scan.ScanNugetReferences(project);

            var report = ReportGenerator.GenerateReport(scanResult, project.ProjectInfo.FullName, ProductVersion);

            return Storage.Store(report);
        }
    }
}
