using DependencyScanner.Api.Interfaces;
using DependencyScanner.Api.Services;
using DependencyScanner.Core.GitClient;
using DependencyScanner.Core.Interfaces;
using DependencyScanner.Core.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Serilog;
using System.Threading;

namespace DependencyScanner.Core.Test
{
    [TestClass]
    abstract public class TestBase
    {
        protected ILogger _logger;

        [TestInitialize]
        public void Init()
        {
            var logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console()
                .CreateLogger();
            _logger = logger;

            Log.Logger = logger;
        }

        public static ICancelableProgress<ProgressMessage> Progress => new DefaultProgress { Token = default(CancellationToken) };

        public static GitEngine Git => new GitEngine();

        public static FileScanner Scanner => new FileScanner(Git);
    }
}
