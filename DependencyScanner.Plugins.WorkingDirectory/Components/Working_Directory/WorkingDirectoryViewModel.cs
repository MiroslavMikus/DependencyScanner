using DependencyScanner.Api.Events;
using DependencyScanner.Api.Interfaces;
using DependencyScanner.Api.Model;
using DependencyScanner.Api.Services;
using DependencyScanner.Core.Gui.Services;
using DependencyScanner.Core.Tools;
using DependencyScanner.Core.Tools.DependencyScanner.Core.Tools;
using DependencyScanner.Plugins.Wd.Components.Repository;
using DependencyScanner.Plugins.Wd.Desing;
using DependencyScanner.Plugins.Wd.Services;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Threading;
using MahApps.Metro.Controls.Dialogs;
using Serilog;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DependencyScanner.Plugins.Wd.Components.Working_Directory
{
    public class WorkingDirectoryViewModel : ViewModelBase
    {
        // commands
        public CommandManager Commands { get; set; }

        public RelayCommand PickWorkingDirectoryCommand { get; private set; }

        public RelayCommand CancelCommand { get; private set; }
        public RelayCommand SyncAllCommand { get; private set; }
        public RelayCommand PullAllCommand { get; private set; }
        public RelayCommand<IWorkingDirectory> CloneCommand { get; private set; }
        public RelayCommand<IWorkingDirectory> RemoveWorkingDirectoryCommand { get; private set; }
        public RelayCommand<IWorkingDirectory> RenameWorkingDirectoryCommand { get; private set; }

        // private fields

        private readonly WorkingDirectorySettingsManager _settingsManager;
        private readonly IMessenger _messenger;
        private readonly ILogger _logger;
        private readonly IRepositoryScanner _scanner;
        private readonly IFolderPicker _folderPicker;
        private readonly Func<IWorkingDirectory> _wdCtor;
        private readonly IDialogCoordinator _dialogCoordinator;

        // mvvm props
        public ObservableCollection<IWorkingDirectory> Directories { get; set; }

        private CancellationTokenSource _cancellationTokenSource;
        public CancellationTokenSource CancellationTokenSource { get => _cancellationTokenSource; set => Set(ref _cancellationTokenSource, value); }

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

            testwd.Repositories.Add(new RepositoryViewModel(new DesignGitInfo()));
            testwd.Repositories.Add(new RepositoryViewModel(new DesignGitInfo()));

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
            IDialogCoordinator dialogCoordinator,
            CommandManager commandManager
            )
        {
            _settingsManager = settingsManager;
            _messenger = messenger;
            _logger = logger;
            _scanner = scanner;
            _folderPicker = folderPicker;
            _wdCtor = wdCtor;
            _dialogCoordinator = dialogCoordinator;

            Commands = commandManager;

            Directories = new ObservableCollection<IWorkingDirectory>(_settingsManager.RestoreWorkingDirectories());

            Task.Run(async () =>
            {
                await ExecuteForEachWorkingDirectorParallel((dir, tok) =>
                {
                    return dir.ExecuteForEachRepositoryParallel(async a =>
                    {
                        var repo = a as RepositoryViewModel;
                        try
                        {
                            repo.StartProgress();
                            repo.IsMarquee = true;
                            await a.GitInfo.Init(_settingsManager.Settings.ExecuteGitFetchWhileScanning);
                        }
                        finally
                        {
                            repo.StopProgress();
                        }
                    }, new SemaphoreSlim(5, 5), tok);
                });
            });

            InitCommands();
        }

        private async Task ExecuteForEachWorkingDirectory(Func<IWorkingDirectory, CancellationToken, Task> action)
        {
            CancellationTokenSource = new CancellationTokenSource();

            try
            {
                foreach (var directory in Directories)
                {
                    try
                    {
                        await action(directory, CancellationTokenSource.Token);
                    }
                    catch (OperationCanceledException)
                    {
                        break;
                    }
                    if (CancellationTokenSource.Token.IsCancellationRequested) break;
                }
            }
            finally
            {
                CancellationTokenSource = null;
            }
        }

        private async Task ExecuteForEachWorkingDirectorParallel(Func<IWorkingDirectory, CancellationToken, Task> action)
        {
            CancellationTokenSource = new CancellationTokenSource();

            try
            {
                var taskList = new List<Task>();
                foreach (var directory in Directories)
                {
                    try
                    {
                        taskList.Add(action(directory, CancellationTokenSource.Token));
                    }
                    catch (OperationCanceledException)
                    {
                        break;
                    }
                    if (CancellationTokenSource.Token.IsCancellationRequested) break;
                }
                await Task.WhenAll(taskList);
            }
            finally
            {
                CancellationTokenSource = null;
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
                    CancellationTokenSource = new CancellationTokenSource();

                    var controller = await _dialogCoordinator.ShowProgressAsync(this, $"Scanning working directory {folder}", "Initialization.");

                    controller.SetIndeterminate();

                    var progress = new DefaultProgress()
                    {
                        Token = CancellationTokenSource.Token
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
                        //var wd = await Scan(progress, folder, CancellationToken.None, _settingsManager.Settings.ExecuteGitFetchWhileScanning);
                        var wd = await Scan(folder);

                        wd.Name = name;

                        DispatcherHelper.CheckBeginInvokeOnUI(() =>
                        {
                            Directories.Add(wd);
                        });

                        _settingsManager.SyncSettings(Directories);

                        controller.SetMessage("Publishing results...");

                        _messenger.Send<AddWorkindDirectory>(new AddWorkindDirectory(wd));

                        wd.ExecuteForEachRepository(a => a.GitInfo.Init(_settingsManager.Settings.ExecuteGitFetchWhileScanning), CancellationTokenSource.Token);
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

            CloneCommand = new RelayCommand<IWorkingDirectory>(async wd =>
            {
                var gitUrl = await _dialogCoordinator.ShowInputAsync(this, $"Clone new repository to {wd.Name}", "Enter git remote url:");

                if (string.IsNullOrEmpty(gitUrl)) return;

                var url = TryCast(gitUrl);

                if (url == null)
                {
                    await _dialogCoordinator.ShowMessageAsync(this, "Wrong format", $"The specified url:'{gitUrl}' has wrong format");
                    return;
                }

                var progress = await _dialogCoordinator.ShowProgressAsync(this, $"Cloning to {wd.Name}", $"Executing clone from {url} to {wd.Path}");

                progress.SetIndeterminate();

                var command = $"git.exe clone {url}";

                _logger.Information("Clonning {command}", command);

                var startInfo = new ProcessStartInfo
                {
                    WorkingDirectory = wd.Path,
                    WindowStyle = ProcessWindowStyle.Hidden,
                    Arguments = $"clone {url}",
                    FileName = "git.exe",
                    UseShellExecute = false
                };

                var process = new AsyncProcess(startInfo);

                await process.StartAsync();

                await progress.CloseAsync();

                await wd.Sync(CancellationToken.None);

                _settingsManager.SyncSettings(Directories);
            });

            SyncAllCommand = new RelayCommand(async () =>
            {
                await ExecuteForEachWorkingDirectory(async (directory, token) =>
                {
                    await directory.Sync(token);
                });
            });

            CancelCommand = new RelayCommand(() =>
            {
                CancellationTokenSource?.Cancel();
            });

            PullAllCommand = new RelayCommand(async () =>
            {
                await ExecuteForEachWorkingDirectory(async (dir, tok) =>
                {
                    await ((WorkingDirectory)dir).PullAllRepos(tok);
                });
            });
        }

        private Uri TryCast(string url)
        {
            var result = Uri.TryCreate(url, UriKind.Absolute, out Uri uriResult) &&
                (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);

            return result ? uriResult : null;
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

        private Task<IWorkingDirectory> Scan(string folder)
        {
            return Task.Run(() =>
            {
                var repos = _scanner.ScanForGitRepositories(folder);

                var newWorkinDir = _wdCtor();

                newWorkinDir.Repositories = new ObservableCollection<IRepository>(repos.Select(a => new RepositoryViewModel(a)));

                newWorkinDir.Path = folder;

                return newWorkinDir;
            });
        }
    }
}
