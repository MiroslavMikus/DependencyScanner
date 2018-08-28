using Microsoft.VisualStudio.TestTools.UnitTesting;
using Serilog;

namespace DependencyScanner.Core.Test
{
    [TestClass]
    abstract public class BaseTest
    {
        [TestInitialize]
        public void Init()
        {
            var logger = new LoggerConfiguration()
                .WriteTo.Console()
                .CreateLogger();

            Log.Logger = logger;
        }
    }
}
