using Serilog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DependencyScanner.Standalone
{
    public class ChocoUpdater
    {
        public const string PackageId = "dependency-scanner";

        public bool IsNewVersionAvailable()
        {
            var search = SearchInChoco();

            var result = IsOutdated(search);

            Log.Information($"New version was {(result ? "" : "not ")}found");

            return result;
        }

        private bool IsOutdated(string input)
        {
            var lines = input.Split(new string[] { Environment.NewLine }, StringSplitOptions.None).Where(a => !string.IsNullOrEmpty(a));

            foreach (var line in lines)
            {
                if (line.IndexOf(PackageId, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    return true;
                }
            }
            return false;
        }

        private string SearchInChoco()
        {
            Log.Debug("Checking latest version");

            var proc = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "choco",
                    Arguments = "outdated",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true
                }
            };

            try
            {
                proc.Start();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Cant start choco outdated");

                return string.Empty;
            }

            StringBuilder sb = new StringBuilder();

            while (!proc.StandardOutput.EndOfStream)
            {
                sb.AppendLine(proc.StandardOutput.ReadLine());
            }

            return sb.ToString();
        }

        public void Update()
        {
            Log.Information("Updating with choco");

            var runDependencyScanner = "Start-Process (Join-Path ([System.Environment]::GetFolderPath('CommonPrograms')) 'DependencyScanner.Standalone.lnk')";

            var proc = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "powershell",
                    Arguments = $"choco upgrade {PackageId} -y;{runDependencyScanner}",
                    UseShellExecute = true,
                    CreateNoWindow = true,
                    Verb = "runas"
                }
            };

            try
            {
                proc.Start();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Cant start choco upgrade");
            }
        }
    }
}
