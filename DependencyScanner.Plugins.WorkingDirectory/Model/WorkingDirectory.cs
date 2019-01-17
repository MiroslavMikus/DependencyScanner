using DependencyScanner.Api.Interfaces;
using DependencyScanner.Api.Model;
using DependencyScanner.Api.Services;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Threading;
using Serilog;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DependencyScanner.Plugins.Browse.Model
{
    public class WorkingDirectory : ObservableObject, IWorkingDirectory
    {
        private string _path;
        private readonly ILogger _logger;
        private readonly IRepositoryScanner _scanner;
        private readonly IMessenger _messenger;
        private CancellationTokenSource _cancellationTokenSource;

        public string Path { get => _path; set => Set(ref _path, value); }

        public ICollection<IRepository> Repositories { get; set; } = new ObservableCollection<IRepository>();
        public ICommand PullCommand { get; }
        public ICommand CancelCommand { get; }

        public WorkingDirectory(ILogger logger, IRepositoryScanner scanner, IMessenger messenger)
        {
            _logger = logger;
            _scanner = scanner;
            _messenger = messenger;

            PullCommand = new RelayCommand(async () =>
            {
                _cancellationTokenSource = new CancellationTokenSource();

                var progress = new DefaultProgress
                {
                    Token = _cancellationTokenSource.Token
                };

                // todo implement progress

                var repos = await _scanner.ScanForGitRepositories(_path, progress);

                DispatcherHelper.CheckBeginInvokeOnUI(() =>
                {
                    Repositories = new ObservableCollection<IRepository>(repos.Select(a => new Repository(a)));

                    //_messenger.Send<WorkingDirectory>(this);
                });
            });

            CancelCommand = new RelayCommand(() =>
            {
                _cancellationTokenSource?.Cancel();
            });
        }
    }

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
