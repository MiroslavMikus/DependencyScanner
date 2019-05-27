using DependencyScanner.Api.Events;
using DependencyScanner.Api.Interfaces;
using DependencyScanner.Api.Model;
using DependencyScanner.Api.Services;
using DependencyScanner.Core.Gui.ViewModel;
using DependencyScanner.Plugins.Wd.Components.Repository;
using DependencyScanner.Plugins.Wd.Components.Settings;
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

namespace DependencyScanner.Plugins.Wd.Components.Working_Directory
{
    public class WorkingDirectory : ObservableProgressBase, IWorkingDirectory, IEquatable<WorkingDirectory>
    {
        private string _path;
        private readonly ILogger _logger;
        private readonly IRepositoryScanner _scanner;
        private readonly IMessenger _messenger;
        private readonly WorkingDirectorySettings _settings;
        private readonly Func<IGitInfo, IRepository> _repoCtor;

        private CancellationTokenSource _cancellationTokenSource;
        public CancellationTokenSource CancellationTokenSource { get => _cancellationTokenSource; set => Set(ref _cancellationTokenSource, value); }

        public string Path { get => _path; set => Set(ref _path, value); }

        public int AtWorkCount { get => Repositories.Where(a => !a.GitInfo.IsClean).Count(); }
        public int BehindCount { get => Repositories.Where(a => a.GitInfo.IsBehind).Count(); }

        private ICollection<IRepository> _repositories = new ObservableCollection<IRepository>();

        public ICollection<IRepository> Repositories
        {
            get => _repositories;
            set
            {
                if (Set(ref _repositories, value))
                {
                    RaisePropertyChanged(nameof(BehindCount));
                    RaisePropertyChanged(nameof(AtWorkCount));
                }
            }
        }

        public ICommand PullCommand { get; }
        public ICommand CancelCommand { get; }

        private string _name;

        public string Name { get => _name; set => Set(ref _name, value); }

        public WorkingDirectory(ILogger logger,
                                IRepositoryScanner scanner,
                                IMessenger messenger,
                                WorkingDirectorySettings settings,
                                Func<IGitInfo, IRepository> repoCtor)
        {
            _logger = logger;
            _scanner = scanner;
            _messenger = messenger;
            _settings = settings;
            _repoCtor = repoCtor;

            PullCommand = new RelayCommand(async () =>
            {
                CancellationTokenSource = new CancellationTokenSource();

                try
                {
                    var repos = await _scanner.ScanForGitRepositories(_path, this, _settings.ExecuteGitFetchWhileScanning, CancellationTokenSource.Token);

                    if (repos.Count() != Repositories.Count())
                    {
                        // add
                        var missingRepos = repos.Except(Repositories.Select(a => a.GitInfo));

                        foreach (var repo in missingRepos.Select(a => _repoCtor(a)))
                        {
                            Repositories.Add(repo);
                        }

                        // remove
                        var gitInfosToRemove = Repositories.Select(a => a.GitInfo).Except(repos).ToList();

                        if (gitInfosToRemove.Any())
                        {
                            foreach (var repo in Repositories.Where(a => gitInfosToRemove.Any(b => b.Equals(a.GitInfo))))
                            {
                                Repositories.Remove(repo);
                            }
                        }
                    }

                    await PullAllRepos(CancellationTokenSource.Token);
                }
                finally
                {
                    CancellationTokenSource = null;
                }
            });

            CancelCommand = new RelayCommand(() =>
            {
                _cancellationTokenSource?.Cancel();
            });
        }

        public async Task Sync(CancellationToken token)
        {
            _cancellationTokenSource = new CancellationTokenSource();

            try
            {
                StartProgress();

                var repos = await _scanner.ScanForGitRepositories(_path, this, _settings.ExecuteGitFetchWhileScanning, token);

                DispatcherHelper.CheckBeginInvokeOnUI(() =>
                {
                    Repositories = new ObservableCollection<IRepository>(repos.Select(a => _repoCtor(a)));

                    _messenger.Send<AddWorkindDirectory>(new AddWorkindDirectory(this));
                });
            }
            catch (OperationCanceledException)
            {
            }
            finally
            {
                StopProgress();
            }
        }

        internal async Task PullAllRepos(CancellationToken token)
        {
            if (!IsRunning)
            {
                await ExecuteForEachRepositoryParallel(a => a.Sync(token), new SemaphoreSlim(5, 5), token);
            }
        }

        public async Task ExecuteForEachRepository(Func<IRepository, Task> repositoryAction, CancellationToken token)
        {
            StartProgress();

            CancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(token);

            try
            {
                var repos = Repositories.ToArray();

                for (int i = 0; i < repos.Count(); i++)
                {
                    ProgressValue = CalculateProgress(i, Repositories.Count);

                    await repositoryAction(repos[i]);

                    if (CancellationTokenSource.Token.IsCancellationRequested) break;
                }
            }
            finally
            {
                CancellationTokenSource = null;
                StopProgress();
            }
        }

        public async Task ExecuteForEachRepositoryParallel(Func<IRepository, Task> repositoryAction, SemaphoreSlim sem, CancellationToken token)
        {
            StartProgress();

            CancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(token);

            try
            {
                var repos = Repositories.ToArray();

                for (int i = 0; i < repos.Count(); i++)
                {
                    await sem.WaitAsync();

                    var repo = repos[i] as ObservableProgressBase;
                    repo.StartProgress();
                    repo.IsMarquee = true;

#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                    repositoryAction(repos[i]).ContinueWith(a =>
                    {
                        ProgressValue = CalculateProgress(i, Repositories.Count);
                        sem.Release();
                        repo.StopProgress();
                    });
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed

                    if (CancellationTokenSource.Token.IsCancellationRequested) break;
                }
            }
            finally
            {
                CancellationTokenSource = null;
                StopProgress();
            }
        }

        #region Equals

        public bool Equals(WorkingDirectory other)
        {
            return this.Path == other.Path;
        }

        public override bool Equals(object other)
        {
            if (other is WorkingDirectory wd)
            {
                return this.Equals(wd);
            }

            return false;
        }

        public override int GetHashCode()
        {
            return 467214278 + EqualityComparer<string>.Default.GetHashCode(Path);
        }

        #endregion
    }
}
