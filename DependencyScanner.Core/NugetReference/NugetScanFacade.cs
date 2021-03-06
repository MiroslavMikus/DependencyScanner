﻿using DependencyScanner.Core.Model;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;

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
            ProductVersion = productVersion;
        }

        public StorageKey ExecuteScan(ProjectResult project)
        {
            var scanResult = Scan.ScanNugetReferences(project);

            if (scanResult == null)
            {
                // error was already logged
                return null;
            }

            if (scanResult.Any())
            {
                var report = ReportGenerator.GenerateReport(scanResult, project.ProjectInfo.FullName, ProductVersion);

                return Storage.Store(report);
            }
            else
            {
                Log.Error("No dependencies were found. Project: {project}", project.ProjectInfo.FullName);

                return null;
            }
        }
    }
}
