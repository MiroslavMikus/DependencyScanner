using DependencyScanner.Api.Interfaces;
using DependencyScanner.Api.Model;
using DependencyScanner.Core.Gui.ViewModel;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DependencyScanner.Plugins.Wd.Model
{
    public class Repository : ObservableProgressBase, IRepository
    {
        private CancellationTokenSource _cancellationTokenSource;

        public IGitInfo GitInfo { get; set; }
        public ICommand PullCommand { get; }
        public ICommand CancelCommand { get; }
        public ICommand SetBranchCommand { get; }
        public ICommand SetRemoteBranchCommand { get; }

        public Repository(IGitInfo gitInfo)
        {
            GitInfo = gitInfo;

            PullCommand = new RelayCommand(async () =>
            {
                await Sync(CancellationToken.None);
            });

            CancelCommand = new RelayCommand(() =>
            {
                _cancellationTokenSource?.Cancel();
            });

            SetBranchCommand = new RelayCommand<string>(a =>
            {
                GitInfo.CurrentBranch = a;
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
