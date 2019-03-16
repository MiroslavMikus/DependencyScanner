using System;
using System.Collections.Generic;
using DependencyScanner.Api.Model;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using System.Threading;
using System.Threading.Tasks;

namespace DependencyScanner.Plugins.Wd.Desing
{
    internal class DesignWrokingDirectory : IWorkingDirectory
    {
        public CancellationTokenSource CancellationTokenSource { get => new CancellationTokenSource(); }

        public ICollection<IRepository> Repositories { get; set; } = new List<IRepository>();
        public string Path { get; set; }

        public ICommand PullCommand => new RelayCommand(() => { Console.WriteLine("Calling PullCommand"); });

        public ICommand CancelCommand => new RelayCommand(() => { Console.WriteLine("Calling CancelCommand"); });

        public string Name { get; set; }

        public Task ExecuteForEachRepository(Func<IRepository, Task> repositoryAction, CancellationToken token)
        {
            throw new NotImplementedException();
        }

        public Task ExecuteForEachRepositoryParallel(Func<IRepository, Task> repositoryAction, SemaphoreSlim sem, CancellationToken token)
        {
            throw new NotImplementedException();
        }

        public Task ExecuteForEachRepositoryParallel(Func<IRepository, Task> repositoryAction, CancellationToken token)
        {
            throw new NotImplementedException();
        }

        public Task Sync(CancellationToken token)
        {
            throw new NotImplementedException();
        }
    }
}
