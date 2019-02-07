using DependencyScanner.Api.Interfaces;
using DependencyScanner.Api.Model;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DependencyScanner.Plugins.Wd.Model
{
    public class Repository : ObservableObject, IRepository
    {
        private CancellationTokenSource _cancellationTokenSource;

        public IGitInfo GitInfo { get; set; }
        public ICommand PullCommand { get; }
        public ICommand CancelCommand { get; }

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
        }

        public Task Sync(CancellationToken token)
        {
            return Task.Run(() => GitInfo.Pull());
        }
    }
}
