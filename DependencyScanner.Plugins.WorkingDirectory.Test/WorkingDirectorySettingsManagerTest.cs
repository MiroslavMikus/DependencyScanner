using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using DependencyScanner.Api.Interfaces;
using DependencyScanner.Api.Model;
using DependencyScanner.Plugins.Wd.Model;
using DependencyScanner.Plugins.Wd.Services;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Serilog;

namespace DependencyScanner.Plugins.WorkingDirectory.Test
{
    [TestClass]
    [Ignore]
    public class WorkingDirectorySettingsManagerTest
    {
        [TestMethod]
        public void Restore()
        {
            var givenManager = SetupManager(GivenSettings());

            var result = givenManager.RestoreWorkingDirectories();

            result.Should().HaveCount(2);

            result.SelectMany(a => a.Repositories).Should().HaveCount(4);
        }

        [TestMethod]
        public void Sync()
        {
            var settings = GivenSettings();

            var givenManager = SetupManager(settings);

            var givenWd = WdCtor();

            givenWd.Path = @"C:\new\wd";

            givenWd.Repositories.Add(new Repository(GitCtor(@"C:\new\wd\rpo1")));
            givenWd.Repositories.Add(new Repository(GitCtor(@"C:\new\wd\rpo2")));
            givenWd.Repositories.Add(new Repository(GitCtor(@"C:\new\wd\rpo3")));

            var givenDirectories = givenManager.RestoreWorkingDirectories().Concat(new List<IWorkingDirectory>
            {
                givenWd
            });

            givenManager.SyncSettings(givenDirectories);

            settings.WorkingDirectoryStructure.Should().HaveCount(3);
            settings.WorkingDirectoryStructure.SelectMany(a => a.Repositories).Should().HaveCount(7);

            settings.WorkingDirectoryStructure.Should().Contain(a => a.Path == @"C:\new\wd");
            settings.WorkingDirectoryStructure.SelectMany(a => a.Repositories)
                .Should().Contain(new[] { @"C:\new\wd\rpo1", @"C:\new\wd\rpo2", @"C:\new\wd\rpo3" });
        }

        private IGitInfo GitCtor(string a)
        {
            var mock = new Mock<IGitInfo>();
            mock.SetupAllProperties();
            mock.SetupGet(b => b.Root).Returns(new FileInfo(a));
            return mock.Object;
        }

        private IWorkingDirectory WdCtor()
        {
            var mock = new Mock<IWorkingDirectory>();
            mock.SetupAllProperties();
            mock.Object.Repositories = new List<IRepository>();
            return mock.Object;
        }

        private WorkingDirectorySettingsManager SetupManager(WorkingDirectorySettings settings)
        {
            var logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console()
                .CreateLogger();

            return new WorkingDirectorySettingsManager(settings, GitCtor, WdCtor, logger);
        }

        private WorkingDirectorySettings GivenSettings() => new WorkingDirectorySettings()
        {
            WorkingDirectoryStructure = new List<StorableWorkingDirectory>
                {
                    new StorableWorkingDirectory {Name =  @"C:\S",  Repositories = new [] { @"C:\S\repo1", @"C:\S\repo2" } },
                    new StorableWorkingDirectory {Name = @"C:\code",Repositories = new [] { @"C:\S\GitHub", @"C:\S\GitLab" } }
                }
        };
    }
}
