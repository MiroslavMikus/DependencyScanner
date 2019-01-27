using System;
using System.Collections.Generic;
using DependencyScanner.Api.Model;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;

namespace DependencyScanner.Plugins.Wd.Desing
{
    internal class DesignWrokingDirectory : IWorkingDirectory
    {
        public ICollection<IRepository> Repositories { get; set; } = new List<IRepository>();
        public string Path { get; set; }

        public ICommand PullCommand => new RelayCommand(() => { Console.WriteLine("Calling PullCommand"); });

        public ICommand CancelCommand => new RelayCommand(() => { Console.WriteLine("Calling CancelCommand"); });

        public string Name { get; set; }
    }
}
