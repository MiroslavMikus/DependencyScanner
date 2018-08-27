using DependencyScanner.Core.Tools;
using Serilog;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace DependencyScanner.Standalone
{
    public class ChocoUpdater
    {
        public const string PackageId = "dependency-scanner";

        public async Task<bool> IsNewVersionAvailable()
        {
            var search = await SearchInChoco();

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

        private async Task<string> SearchInChoco()
        {
            Log.Debug("Checking latest version");

            var info = new ProcessStartInfo
            {
                FileName = "choco",
                Arguments = "outdated",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                CreateNoWindow = true
            };

            return await new AsyncProcess(info).StartAsync();
        }

        public void Update()
        {
            Log.Information("Updating with choco");

            var runDependencyScanner = "Start-Process (Join-Path ([System.Environment]::GetFolderPath('CommonPrograms')) 'DependencyScanner.Standalone.lnk')";
            var changelogUrl = "Start-Process https://github.com/MiroslavMikus/DependencyScanner/blob/master/DependencyScanner.Standalone/res/Changeset.md";
            var info = new ProcessStartInfo
            {
                FileName = "powershell",
                Arguments = $"choco upgrade {PackageId} -y;{runDependencyScanner};{changelogUrl}",
                UseShellExecute = true,
                CreateNoWindow = true,
                Verb = "runas"
            };

            new Process { StartInfo = info }.Start();
        }
    }
}
