using DependencyScanner.Api.Interfaces;
using DependencyScanner.Api.Services;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Threading;
using Serilog;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Windows.Input;

namespace DependencyScanner.Api.Model
{
    public class WorkingDirectory : ObservableObject, ISyncable
    {
        private string _path;
        private readonly ILogger _logger;
        private readonly IRepositoryScanner _scanner;
        private readonly IMessenger _messenger;
        private CancellationTokenSource _cancellationTokenSource;

        public string Path { get => _path; set => Set(ref _path, value); }

        public ICollection<Repository> Repositories { get; set; } = new ObservableCollection<Repository>();
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
                    Repositories = new ObservableCollection<Repository>(repos.Select(a => new Repository(a)));

                    //_messenger.Send<WorkingDirectory>(this);
                });
            });

            CancelCommand = new RelayCommand(() =>
            {
                _cancellationTokenSource?.Cancel();
            });
        }
    }

    public class Repository : ISyncable
    {
        private CancellationTokenSource _cancellationTokenSource;

        public IGitInfo GitInfo { get; set; }
        public ICommand PullCommand { get; }
        public ICommand CancelCommand { get; }

        public Repository(IGitInfo gitInfo)
        {
            GitInfo = gitInfo;

            PullCommand = new RelayCommand(() =>
            {
                GitInfo.Pull();
            });

            CancelCommand = new RelayCommand(() =>
            {
                _cancellationTokenSource?.Cancel();
            });
        }
    }

    public class Solution
    {
        public ICollection<Project> Projects { get; set; }
    }

    public class Project
    {
        public ICollection<ProjectReference> ProjectReferences { get; set; }
    }

    public class ProjectReference
    {
    }
}
