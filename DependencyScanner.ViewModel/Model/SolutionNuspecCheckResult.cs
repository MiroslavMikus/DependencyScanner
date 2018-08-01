using DependencyScanner.Core.Model;
using GalaSoft.MvvmLight;
using System.Collections.Generic;
using System.Linq;

namespace DependencyScanner.ViewModel
{
    public class SolutionNuspecCheckResult : ViewModelBase
    {
        private SolutionResult _result;
        public SolutionResult Result { get => _result; set => Set(ref _result, value); }

        private IEnumerable<ProjectNuspecResult> _projectResult;
        public IEnumerable<ProjectNuspecResult> ProjectResult
        {
            get => _projectResult;
            set
            {
                Set(ref _projectResult, value);
                RaisePropertyChanged(nameof(HasConsolidates));
            }
        }

        public bool HasConsolidates
        {
            get
            {
                if (ProjectResult == null) return false;

                return ProjectResult.Count() > 0;
            }
        }
    }
}
