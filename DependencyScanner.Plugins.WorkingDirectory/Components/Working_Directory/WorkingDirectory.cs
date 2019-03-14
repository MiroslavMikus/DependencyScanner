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
        private CancellationTokenSource _cancellationTokenSource;

        public string Path { get => _path; set => Set(ref _path, value); }

        private ICollection<IRepository> _repositories = new ObservableCollection<IRepository>();
        public ICollection<IRepository> Repositories { get => _repositories; set => Set(ref _repositories, value); }
        public ICommand PullCommand { get; }
        public ICommand CancelCommand { get; }

        private string _name;

        public string Name { get => _name; set => Set(ref _name, value); }

        public WorkingDirectory(ILogger logger, IRepositoryScanner scanner, IMessenger messenger, WorkingDirectorySettings settings)
        {
            _logger = logger;
            _scanner = scanner;
            _messenger = messenger;
            _settings = settings;

            PullCommand = new RelayCommand(async () =>
            {
                await Sync(CancellationToken.None);
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
                    Repositories = new ObservableCollection<IRepository>(repos.Select(a => new RepositoryViewModel(a)));

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
            await ExecuteForEachRepository(a => a.Sync(token), token);
        }

        internal async Task ExecuteForEachRepository(Func<IRepository, Task> repositoryAction, CancellationToken token)
        {
            StartProgress();

            try
            {
                var repos = Repositories.ToArray();

                for (int i = 0; i < repos.Count(); i++)
                {
                    ProgressValue = CalculateProgress(i, Repositories.Count);

                    await repositoryAction(repos[i]);

                    if (token.IsCancellationRequested) break;
                }
            }
            finally
            {
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
