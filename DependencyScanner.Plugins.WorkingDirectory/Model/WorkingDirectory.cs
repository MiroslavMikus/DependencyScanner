using DependencyScanner.Api.Interfaces;
using DependencyScanner.Api.Model;
using DependencyScanner.Api.Services;
using DependencyScanner.Plugins.Wd.Model;
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

namespace DependencyScanner.Plugins.Wd.Model
{
    public class WorkingDirectory : ObservableObject, IWorkingDirectory, IEquatable<WorkingDirectory>
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
    }
}
