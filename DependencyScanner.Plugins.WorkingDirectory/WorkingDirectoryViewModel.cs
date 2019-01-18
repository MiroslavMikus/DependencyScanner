using DependencyScanner.Api.Interfaces;
using DependencyScanner.Api.Model;
using DependencyScanner.Plugins.Wd.Services;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Serilog;
using System;
using System.Collections.ObjectModel;

namespace Dependency.Scanner.Plugins.Wd
{
    public class WorkingDirectoryViewModel : ObservableObject
    {
        public RelayCommand PickWorkingDirectoryCommand { get; private set; }
        public RelayCommand ScanCommand { get; private set; }
        public RelayCommand CancelCommand { get; private set; }
        public RelayCommand<string> RemoveWorkingDirectoryCommand { get; private set; }

        private readonly WorkingDirectorySettingsManager _settingsManager;
        private readonly IMessenger _messenger;
        private readonly ILogger _logger;
        private readonly IRepositoryScanner _scanner;

        public ObservableCollection<IWorkingDirectory> Directories { get; set; }

        public WorkingDirectoryViewModel(WorkingDirectorySettingsManager settingsManager, IMessenger messenger, ILogger logger, IRepositoryScanner scanner)
        {
            _settingsManager = settingsManager;
            _messenger = messenger;
            _logger = logger;
            _scanner = scanner;

            Directories = new ObservableCollection<IWorkingDirectory>(_settingsManager.RestoreWorkingDirectories());

            InitCommands();
        }

        private void InitCommands()
        {
            PickWorkingDirectoryCommand = new RelayCommand(() =>
            {
            });
            ScanCommand = new RelayCommand(() =>
            {
            });
            CancelCommand = new RelayCommand(() =>
            {
            });
            RemoveWorkingDirectoryCommand = new RelayCommand<string>(a =>
            {
            });
        }
    }
}
