﻿using DependencyScanner.Api.Events;
using DependencyScanner.Api.Interfaces;
using DependencyScanner.Core.Gui.ViewModel;
using DependencyScanner.Core.Interfaces;
using DependencyScanner.Core.Model;
using DependencyScanner.Standalone.Components.Browse;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Threading;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace DependencyScanner.ViewModel
{
    public class BrowseViewModel : FilterViewModelBase<SolutionResult, ProjectResult>
    {
        private readonly ISolutionScanner _scanner;
        private readonly IMessenger _messenger;
        private readonly Serilog.ILogger _logger;
        private readonly BrowseSettings _settings;

        public RelayCommand PickWorkingDirectoryCommand { get; private set; }
        public RelayCommand ScanCommand { get; private set; }
        public RelayCommand CancelCommand { get; private set; }
        public RelayCommand<string> RemoveWorkingDirectoryCommand { get; private set; }

        public BrowseViewModel(ISolutionScanner scanner,
                               IMessenger messenger,
                               Serilog.ILogger logger,
                               BrowseSettings settings)
        {
            _scanner = scanner;
            _messenger = messenger;
            _logger = logger;
            _settings = settings;

            PrimaryCollection = new ObservableCollection<SolutionResult>();

            _messenger.Register<AddWorkindDirectory>(this, async a =>
            {
                foreach (var project in a.Directory.Repositories)
                {
                    await Scan(project.GitInfo.Root.DirectoryName, project.GitInfo);
                }
            });
        }

        private Task Scan(string directory, IGitInfo gitInfo)
        {
            return Task.Run(async () =>
            {
                var scanResult = await _scanner.ScanSolution(directory);

                if (scanResult == null) return;

                scanResult.GitInformation = gitInfo;

                DispatcherHelper.CheckBeginInvokeOnUI(() =>
                {
                    if (PrimaryCollection.Contains(scanResult))
                    {
                        PrimaryCollection.Remove(scanResult);
                    }
                    PrimaryCollection.Add(scanResult);

                    _messenger.Send<IEnumerable<SolutionResult>>(PrimaryCollection);
                });
            });
        }

        protected override bool PrimaryFilterJob(object value)
        {
            if (!string.IsNullOrEmpty(PrimaryFilter) && value is SolutionResult input)
            {
                return input.Info.Name.IndexOf(PrimaryFilter, StringComparison.OrdinalIgnoreCase) >= 0;
            }
            return true;
        }

        protected override bool SecondaryFilterJob(object value)
        {
            if (!string.IsNullOrEmpty(SecondaryFilter) && value is ProjectResult input)
            {
                return input.ProjectInfo.Name.IndexOf(SecondaryFilter, StringComparison.OrdinalIgnoreCase) >= 0;
            }
            return true;
        }

        protected override IEnumerable<ProjectResult> GetSecondaryCollection(SolutionResult primary)
        {
            return primary.Projects;
        }
    }
}
