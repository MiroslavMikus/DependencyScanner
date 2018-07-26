using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DependencyScanner.Core.Test
{
    [TestClass]
    public class ReadRemoteUrlTest
    {
        const string Folder = @"F:\Projects\_GitHub\Exercise.DynamicProxy\.git";
        string[] GetConfig(string directory) => Directory.GetFiles(directory, "config", SearchOption.AllDirectories);

        [TestMethod]
        public void ReadUrl_Test()
        {
            var result = ReadUrl(Folder);
        }

        public string ReadUrl(string directory)
        {
            var configs = GetConfig(directory);

            if (configs.Count() > 0)
            {
                var firstConfig = configs.First();

                var configContent = File.ReadAllText(firstConfig).Split(new[] { "\r\n" }, StringSplitOptions.None);

                var remoteLine = configContent.FirstOrDefault(a => a.Contains("url"));

                if (remoteLine != null)
                {
                    return remoteLine.Substring(remoteLine.LastIndexOf(" ") + 1);
                }
            }
            return string.Empty;
        }
    }
}
