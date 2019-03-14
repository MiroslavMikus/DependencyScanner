using DependencyScanner.Api.Interfaces;
using DependencyScanner.Api.Model;
using DependencyScanner.Plugins.Wd.Components.Repository;
using DependencyScanner.Plugins.Wd.Components.Settings;
using Serilog;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DependencyScanner.Plugins.Wd.Services
{
    public class WorkingDirectorySettingsManager : IService
    {
        public WorkingDirectorySettings Settings { get; }

        private readonly Func<string, IGitInfo> _gitCtor;
        private readonly Func<IWorkingDirectory> _wdCtor;
        private readonly ILogger _logger;

        public WorkingDirectorySettingsManager(WorkingDirectorySettings settings,
            Func<string, IGitInfo> gitCtor,
            Func<IWorkingDirectory> wdCtor,
            ILogger logger)
        {
            Settings = settings;
            _gitCtor = gitCtor;
            _wdCtor = wdCtor;
            _logger = logger;
        }

        public IEnumerable<IWorkingDirectory> RestoreWorkingDirectories()
        {
            foreach (var wdSettings in Settings.WorkingDirectoryStructure)
            {
                // reassembly repos
                var repos = wdSettings.Repositories.Select(a =>
                {
                    if (!Directory.Exists(a))
                    {
                        _logger.Error("Git directory doens't exist {directory}", a);
                        return null;
                    }
                    var git = _gitCtor(a);

                    git.Init(Settings.ExecuteGitFetchWhileScanning);

                    return new RepositoryViewModel(git);
                }).Where(a => a != null);

                // create working directory
                var wd = _wdCtor();

                wd.Path = wdSettings.Path;

                wd.Name = wdSettings.Name;

                wd.Repositories = new ObservableCollection<IRepository>(repos);

                yield return wd;
            }
        }

        public void SyncSettings(IEnumerable<IWorkingDirectory> workingDirectories)
        {
            string[] GetRepos(IWorkingDirectory wd) => wd.Repositories.Select(a => a.GitInfo.Root.FullName).ToArray();

            Settings.WorkingDirectoryStructure = workingDirectories.Select(a => new StorableWorkingDirectory
            {
                Path = a.Path,
                Name = a.Name,
                Repositories = GetRepos(a)
            }).ToList();
        }
    }
}
