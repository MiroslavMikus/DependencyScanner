using DependencyScanner.Api.Events;
using DependencyScanner.Api.Interfaces;
using DependencyScanner.Api.Model;
using DependencyScanner.Api.Services;
using DependencyScanner.Plugins.Wd.Desing;
using DependencyScanner.Plugins.Wd.Model;
using DependencyScanner.Plugins.Wd.Services;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Threading;
using MahApps.Metro.Controls.Dialogs;
using Serilog;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DependencyScanner.Plugins.Wd
{
    public class WorkingDirectoryViewModel : ViewModelBase
    {
        // commands
        public RelayCommand PickWorkingDirectoryCommand { get; private set; }

        public RelayCommand CancelCommand { get; private set; }
        public RelayCommand<IWorkingDirectory> RemoveWorkingDirectoryCommand { get; private set; }
        public RelayCommand<IWorkingDirectory> RenameWorkingDirectoryCommand { get; private set; }

        // private fields
        private CancellationTokenSource _cancellationTokenSource;

        private readonly WorkingDirectorySettingsManager _settingsManager;
        private readonly IMessenger _messenger;
        private readonly ILogger _logger;
        private readonly IRepositoryScanner _scanner;
        private readonly IFolderPicker _folderPicker;
        private readonly Func<IWorkingDirectory> _wdCtor;
        private readonly IDialogCoordinator _dialogCoordinator;

        // mvvm props
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
            IDialogCoordinator dialogCoordinator
            )
        {
            _settingsManager = settingsManager;
            _messenger = messenger;
            _logger = logger;
            _scanner = scanner;
            _folderPicker = folderPicker;
            _wdCtor = wdCtor;
            _dialogCoordinator = dialogCoordinator;

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

                if (string.IsNullOrEmpty(folder))
                {
                    await _dialogCoordinator.ShowMessageAsync(this, "Folder can't be empty", "Please enter valid folder path!");
                    return;
                }

                var name = await PickWdName((Directory.CreateDirectory(folder)).Name);

                if (string.IsNullOrWhiteSpace(name))
                {
                    return;
                }

                // folder is not null or empty && directories list doesnt contain same path!
                if (!Directories.Any(a => a.Path == folder))
                {
                    _cancellationTokenSource = new CancellationTokenSource();

                    var controller = await _dialogCoordinator.ShowProgressAsync(this, $"Scanning working directory {folder}", "Initialization.");

                    controller.SetIndeterminate();

                    var progress = new DefaultProgress()
                    {
                        Token = _cancellationTokenSource.Token
                    };

                    progress.ReportAction = a =>
                    {
                        if (a.Value == 0)
                        {
                            controller.SetIndeterminate();
                        }
                        else
                        {
                            controller.SetProgress(a.Value / 100);
                        }
                        controller.SetMessage(a.Message);
                    };

                    try
                    {
                        var wd = await Scan(progress, folder);

                        wd.Name = name;

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
                        await controller.CloseAsync();
                    }
                }
            });

            RemoveWorkingDirectoryCommand = new RelayCommand<IWorkingDirectory>(a =>
            {
                Directories.Remove(a);

                _settingsManager.SyncSettings(Directories);

                _messenger.Send<RemoveWorkingDirectory>(new RemoveWorkingDirectory(a));
            });

            RenameWorkingDirectoryCommand = new RelayCommand<IWorkingDirectory>(async a =>
            {
                var name = await PickWdName(a.Name);

                if (string.IsNullOrWhiteSpace(name))
                {
                    return;
                }
                else
                {
                    a.Name = name;
                }
            });
        }

        private async Task<string> PickWdName(string defaultName)
        {
            var mySettings = new MetroDialogSettings()
            {
                DefaultText = defaultName
            };

            var name = await _dialogCoordinator.ShowInputAsync(this, "Working directory name", "Enter working directory name:", mySettings);

            if (string.IsNullOrWhiteSpace(name))
            {
                await _dialogCoordinator.ShowMessageAsync(this, "Name can't be empty", "Consider to enter a valid name :)");
                return string.Empty;
            }
            else
            {
                return name;
            }
        }

        private Task<IWorkingDirectory> Scan(IProgress<ProgressMessage> progress, string folder)
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
