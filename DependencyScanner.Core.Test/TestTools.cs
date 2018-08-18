using DependencyScanner.Core.GitClient;
using DependencyScanner.Core.Interfaces;
using DependencyScanner.Core.Model;
using Serilog;
using System.Threading;

namespace DependencyScanner.Core.Test
{
    public static class TestTools
    {
        public static ILogger Logger => (new LoggerConfiguration().WriteTo.Console().CreateLogger());

        public static ICancelableProgress<ProgressMessage> Progress => new DefaultProgress { Token = default(CancellationToken) };

        public static GitEngine Git => new GitEngine();

        public static FileScanner Scanner => new FileScanner(Git);
    }
}
