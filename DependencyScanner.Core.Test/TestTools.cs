using DependencyScanner.Core.GitClient;
using DependencyScanner.Core.Interfaces;
using DependencyScanner.Core.Model;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DependencyScanner.Core.Test
{
    public static class TestTools
    {
        public static ILogger Logger => (new LoggerConfiguration().WriteTo.Console().CreateLogger());

        public static ICancelableProgress<ProgressMessage> Progress => new DefaultProgress(Logger) { Token = default(CancellationToken) };

        public static GitEngine Git => new GitEngine(Logger);

        public static FileScanner Scanner => new FileScanner(Git);
    }
}
