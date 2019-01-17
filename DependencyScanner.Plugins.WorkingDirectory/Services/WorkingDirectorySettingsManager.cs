using DependencyScanner.Api.Interfaces;
using DependencyScanner.Api.Model;
using DependencyScanner.Plugins.Wd.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace DependencyScanner.Plugins.Wd.Services
{
    public class WorkingDirectorySettingsManager
    {
        private readonly WorkingDirectorySettings _settings;
        private readonly Func<string, IGitInfo> _gitCtor;
        private readonly Func<IWorkingDirectory> _wdCtor;

        public WorkingDirectorySettingsManager(WorkingDirectorySettings settings, Func<string, IGitInfo> gitCtor, Func<IWorkingDirectory> wdCtor)
        {
            _settings = settings;
            _gitCtor = gitCtor;
            _wdCtor = wdCtor;
        }

        public IEnumerable<IWorkingDirectory> RestoreWorkingDirectories()
        {
            foreach (var wdSettings in _settings.WorkingDirectoryStructure)
            {
                // reassembly repos
                var repos = wdSettings.Value.Select(a =>
                {
                    var git = _gitCtor(a);

                    git.Init(_settings.ExecuteGitFetchWhileScanning);

                    return new Repository(git);
                });

                // create working directory
                var wd = _wdCtor();

                wd.Path = wdSettings.Key;

                wd.Repositories = new ObservableCollection<IRepository>(repos);

                yield return wd;
            }
        }

        public void SyncSettings(IEnumerable<IWorkingDirectory> workingDirectories)
        {
            string[] GetRepos(IWorkingDirectory wd) => wd.Repositories.Select(a => a.GitInfo.Root.FullName).ToArray();

            _settings.WorkingDirectoryStructure = workingDirectories.ToDictionary(a => a.Path, b => GetRepos(b));
        }
    }
}
