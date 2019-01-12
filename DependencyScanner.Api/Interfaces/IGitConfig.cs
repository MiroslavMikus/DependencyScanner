using System.Collections.Generic;

namespace DependencyScanner.Api.Interfaces
{
    public interface IGitConfig
    {
        string Content { get; }
        string RootPath { get; }

        IEnumerable<string> GetBranchList();

        string GetCurrentBranch();

        string GetRemoteUrl();

        void RefreshContent();
    }
}
