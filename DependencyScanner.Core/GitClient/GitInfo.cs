﻿using DependencyScanner.Api.Interfaces;
using DependencyScanner.Core.GitClient;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DependencyScanner.Core.Model
{
    public class GitInfo : ObservableObject, IGitInfo, IEquatable<GitInfo>
    {
        #region Services

        private readonly GitEngine _gitEngine;
        private readonly IHasInternetConnection _hasInternetConnection;
        private readonly ILogger _logger;

        #endregion Services

        public FileInfo Root { get; }
        public IGitConfig Config { get; set; }

        private string _commitCount;
        public string CommitCount { get => _commitCount; private set => Set(ref _commitCount, value); }

        private string _remoteUrl;
        public string RemoteUrl { get => _remoteUrl; private set => Set(ref _remoteUrl, value); }

        private IEnumerable<string> _branchList;
        public IEnumerable<string> BranchList { get => _branchList; private set => Set(ref _branchList, value); }

        private IEnumerable<string> _remoteBranchList;
        public IEnumerable<string> RemoteBranchList { get => _remoteBranchList; private set => Set(ref _remoteBranchList, value); }

        private string _currentBranch;

        public string CurrentBranch
        {
            get => _currentBranch;
            set
            {
                if (string.IsNullOrEmpty(value)) return;
                if (Set(ref _currentBranch, value))
                {
                    Task.Run(async () =>
                    {
                        Checkout(value);
                        await Init(false);
                    });
                }
            }
        }

        private string _status;

        public string Status
        {
            get => _status;
            private set
            {
                if (Set(ref _status, value))
                {
                    RaisePropertyChanged(nameof(IsClean));
                    RaisePropertyChanged(nameof(IsBehind));
                }
            }
        }

        public RelayCommand PullCommand { get; }

        public bool IsClean { get => Status?.Contains("working tree clean") == true; }
        public bool IsBehind { get => Status?.Contains("Your branch is behind") == true; }

        public GitInfo(string root,
            GitEngine gitEngine,
            IHasInternetConnection hasInternetConnection,
            ILogger logger)
        {
            _gitEngine = gitEngine;
            _hasInternetConnection = hasInternetConnection;
            _logger = logger;

            Root = new FileInfo(root);

            Config = new GitConfig(Root.FullName);

            PullCommand = new RelayCommand(() =>
            {
                Task.Run(() =>
                {
                    Pull();
                    Status = _gitEngine.GitProcess(Root.DirectoryName, GitCommand.Status);
                });
            });
        }

        public async Task Init(bool executeGitFetch)
        {
            await Task.Run(() =>
            {
                _gitEngine.GitProcess(Root.DirectoryName, GitCommand.UpdateRemoteReferences);

                if (executeGitFetch)
                {
                    if (_hasInternetConnection.CheckInternetConnection())
                    {
                        var result = _gitEngine.GitProcess(Root.DirectoryName, GitCommand.Fetch);
                    }
                    else
                    {
                        _logger.Information("Skipping git fetch, since there is no internet connection. Repository: {repository}", Root.DirectoryName);
                    }
                }

                UpdateBranchList();

                _currentBranch = Config.GetCurrentBranch();

                RaisePropertyChanged(nameof(CurrentBranch));

                Status = _gitEngine.GitProcess(Root.DirectoryName, GitCommand.Status);

                RemoteUrl = Config.GetRemoteUrl();

                CommitCount = _gitEngine.GitProcess(Root.DirectoryName, GitCommand.CommitCount).Split('\r')[0];
            });
        }

        private void UpdateBranchList()
        {
            BranchList = Config.GetLocalBranches();

            RemoteBranchList = Config.GetRemoteBranches();
        }

        public string Checkout(string branch)
        {
            if (BranchList.Contains(branch))
            {
                return _gitEngine.GitProcess(Root.DirectoryName, GitCommand.SwitchBranch, branch);
            }
            else if (RemoteBranchList.Contains(branch))
            {
                return _gitEngine.GitProcess(Root.DirectoryName, $"checkout -b {branch} remotes/origin/{branch}");
            }

            return $"Branch '{branch}' doesnt exist in scan results!";
        }

        public void Pull()
        {
            _gitEngine.GitProcess(Root.DirectoryName, GitCommand.Pull);
            Status = _gitEngine.GitProcess(Root.DirectoryName, GitCommand.Status);
            UpdateBranchList();
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as GitInfo);
        }

        public bool Equals(GitInfo other)
        {
            return other != null &&
                   EqualityComparer<string>.Default.Equals(Root.DirectoryName, other.Root.DirectoryName);
        }

        public override int GetHashCode()
        {
            return -1490287827 + EqualityComparer<string>.Default.GetHashCode(Root.DirectoryName);
        }
    }
}
