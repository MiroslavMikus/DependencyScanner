using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;

namespace DependencyScanner.Core.Model
{
    [DebuggerDisplay("{Info.Name}")]
    public class SolutionResult : ObservableObject
    {
        private FileInfo _info;
        public FileInfo Info { get => _info; private set => Set(ref _info, value); }

        private ICollection<ProjectResult> _projects = new List<ProjectResult>();
        public ICollection<ProjectResult> Projects { get => _projects; protected set => Set(ref _projects, value); }

        private GitInfo _gitInformation;
        private readonly FileScanner _fileScanner;

        public GitInfo GitInformation { get => _gitInformation; internal set => Set(ref _gitInformation, value); }

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

                var result = await _fileScanner.ScanSolution(Info.DirectoryName, progress);

                Projects = result.Projects;

                GitInformation = result.GitInformation;
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
    }
}
