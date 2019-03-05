using DependencyScanner.Api.Interfaces;
using DependencyScanner.Api.Services;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;

namespace DependencyScanner.Core.Model
{
    [DebuggerDisplay("{Info.Name}")]
    public class SolutionResult : ObservableObject, IEquatable<SolutionResult>
    {
        private readonly FileScanner _fileScanner;

        private FileInfo _info;
        public FileInfo Info { get => _info; private set => Set(ref _info, value); }

        private ICollection<ProjectResult> _projects = new List<ProjectResult>();
        public ICollection<ProjectResult> Projects { get => _projects; protected set => Set(ref _projects, value); }

        private IGitInfo _gitInformation;
        public IGitInfo GitInformation { get => _gitInformation; set => Set(ref _gitInformation, value); }

        public RelayCommand RefreshCommand { get; }

        public SolutionResult(FileInfo info, FileScanner fileScanner)
        {
            Info = info;

            _fileScanner = fileScanner;

            RefreshCommand = new RelayCommand(async () =>
            {
                var progress = new DefaultProgress()
                {
                    Token = default(CancellationToken)
                };

                var result = await _fileScanner.ScanSolution(Info.DirectoryName, progress, true); // todo refactor here!

                Projects = result.Projects;

                await GitInformation.Init(false);
            });
        }

        public IEnumerable<ProjectReference> GetSolutionReferences()
        {
            return Projects.SelectMany(a => a.References);
        }

        public override string ToString()
        {
            return Info.Name;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as SolutionResult);
        }

        public bool Equals(SolutionResult other)
        {
            return other != null &&
                   EqualityComparer<string>.Default.Equals(Info.FullName, other.Info.FullName);
        }

        public override int GetHashCode()
        {
            return 1340155117 + EqualityComparer<string>.Default.GetHashCode(Info.FullName);
        }
    }
}
