using DependencyScanner.Api.Interfaces;
using DependencyScanner.Api.Model;
using DependencyScanner.Api.Services;
using DependencyScanner.Plugins.Wd.Desing;
using DependencyScanner.Plugins.Wd.Model;
using DependencyScanner.Plugins.Wd.Services;
using DependencyScanner.Standalone.Events;
using DependencyScanner.Standalone.Services;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Threading;
using Serilog;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Dependency.Scanner.Plugins.Wd
{
    public class WorkingDirectoryViewModel : ViewModelBase
    {
        public RelayCommand PickWorkingDirectoryCommand { get; private set; }
        public RelayCommand CancelCommand { get; private set; }
        public RelayCommand<string> RemoveWorkingDirectoryCommand { get; private set; }

        public ObservableProgress _globalProgress { get; }
        private CancellationTokenSource _cancellationTokenSource;
        private readonly WorkingDirectorySettingsManager _settingsManager;
        private readonly IMessenger _messenger;
        private readonly ILogger _logger;
        private readonly IRepositoryScanner _scanner;
        private readonly IFolderPicker _folderPicker;
        private readonly Func<IWorkingDirectory> _wdCtor;

        public ObservableCollection<IWorkingDirectory> Directories { get; set; }

        public WorkingDirectoryViewModel()
        {
            if (!IsInDesignMode)
            {
                throw new ArgumentException("Empty constructor should be used only at design time");
            }

            var testwd = new DesignWrokingDirectory()
            {
                Name = "FirstRepo",
                Path = @"C:\DemoPaht"
            };

            testwd.Repositories.Add(new Repository(new DesignGitInfo()));
            testwd.Repositories.Add(new Repository(new DesignGitInfo()));

            var longPathWd = new DesignWrokingDirectory()
            {
                Name = "LongPathWd",
                Path = @"C:\DemoPaht\extra\long\paht\here\be\hapy\working\directory"
            };

            Directories = new ObservableCollection<IWorkingDirectory>
            {
                testwd, longPathWd
            };
        }

        public WorkingDirectoryViewModel(WorkingDirectorySettingsManager settingsManager,
            IMessenger messenger,
            ILogger logger,
            IRepositoryScanner scanner,
            IFolderPicker folderPicker,
            Func<IWorkingDirectory> wdCtor,
            ObservableProgress progress
            )
        {
            _settingsManager = settingsManager;
            _messenger = messenger;
            _logger = logger;
            _globalProgress = progress;
            _scanner = scanner;
            _folderPicker = folderPicker;
            _wdCtor = wdCtor;

            Directories = new ObservableCollection<IWorkingDirectory>(_settingsManager.RestoreWorkingDirectories());

            InitCommands();

            foreach (var dir in Directories)
            {
                _messenger.Send<AddWorkindDirectory>(new AddWorkindDirectory(dir));
            }
        }

        private void InitCommands()
        {
            PickWorkingDirectoryCommand = new RelayCommand(async () =>
            {
                var folder = _folderPicker.PickFolder();

                // folder is not null or empty && directories list doesnt contain same path!
                if (!string.IsNullOrEmpty(folder) && !Directories.Any(a => a.Path == folder))
                {
                    _cancellationTokenSource = new CancellationTokenSource();

                    var progress = new DefaultProgress()
                    {
                        Token = _cancellationTokenSource.Token
                    };

                    _globalProgress.RegisterProgress(progress);

                    _globalProgress.IsOpen = true;

                    _globalProgress.ProgressMessage = "Init scan";

                    _globalProgress.Progress = 0D;

                    try
                    {
                        var wd = await Scan(_globalProgress, folder);

                        DispatcherHelper.CheckBeginInvokeOnUI(() =>
                        {
                            Directories.Add(wd);
                        });

                        _settingsManager.SyncSettings(Directories);

                        _messenger.Send<AddWorkindDirectory>(new AddWorkindDirectory(wd));
                    }
                    catch (OperationCanceledException)
                    {
                    }
                    finally
                    {
                        _globalProgress.IsOpen = false;
                    }
                }
            });

            RemoveWorkingDirectoryCommand = new RelayCommand<string>(a =>
            {
                var wd = Directories.FirstOrDefault(b => b.Path == a);

                if (wd != null)
                {
                    Directories.Remove(wd);

                    _settingsManager.SyncSettings(Directories);

                    _messenger.Send<RemoveWorkingDirectory>(new RemoveWorkingDirectory(wd));
                }
            });
        }

        private Task<IWorkingDirectory> Scan(ICancelableProgress<ProgressMessage> progress, string folder)
        {
            return Task.Run(async () =>
            {
                var repos = await _scanner.ScanForGitRepositories(folder, progress);

                var newWorkinDir = _wdCtor();

                newWorkinDir.Repositories = new ObservableCollection<IRepository>(repos.Select(a => new Repository(a)));

                newWorkinDir.Path = folder;

                return newWorkinDir;
            });
        }
    }
}
