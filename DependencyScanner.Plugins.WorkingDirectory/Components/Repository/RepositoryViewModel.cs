using DependencyScanner.Api.Interfaces;
using DependencyScanner.Api.Model;
using DependencyScanner.Core.Gui.ViewModel;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DependencyScanner.Plugins.Wd.Components.Repository
{
    public class RepositoryViewModel : ObservableProgressBase, IRepository
    {
        private CancellationTokenSource _cancellationTokenSource;
        public CancellationTokenSource CancellationTokenSource { get => _cancellationTokenSource; set => Set(ref _cancellationTokenSource, value); }

        public Core.Gui.Services.CommandManager Commands { get; set; }

        public IGitInfo GitInfo { get; set; }
        public ICommand PullCommand { get; set; }
        public ICommand CancelCommand { get; set; }
        public ICommand SetBranchCommand { get; set; }
        public ICommand SetRemoteBranchCommand { get; set; }
        public ICommand RefreshGitInfoCommand { get; set; }

        public RepositoryViewModel(Core.Gui.Services.CommandManager commandManager, IGitInfo gitInfo)
        {
            GitInfo = gitInfo;
            Commands = commandManager;

            PullCommand = new RelayCommand(async () =>
            {
                StartProgress();

                IsMarquee = true;

                await Sync(CancellationToken.None);

                StopProgress();
            });

            CancelCommand = new RelayCommand(() =>
            {
                _cancellationTokenSource?.Cancel();
            });

            SetBranchCommand = new RelayCommand<string>(a =>
            {
                GitInfo.CurrentBranch = a;
            });

            RefreshGitInfoCommand = new RelayCommand(() =>
            {
                GitInfo.Init(false);
            });
        }

        public async Task Sync(CancellationToken token)
        {
            StartProgress();

            IsMarquee = true;

            await Task.Run(() =>
            {
                GitInfo.Pull();
            });

            StopProgress();
        }
    }
}
