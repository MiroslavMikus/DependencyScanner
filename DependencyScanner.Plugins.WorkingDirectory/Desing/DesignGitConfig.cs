using System;
using DependencyScanner.Api.Interfaces;
using System.Collections.Generic;

namespace DependencyScanner.Plugins.Wd.Desing
{
    internal class DesignGitConfig : IGitConfig
    {
        public string Content => "NoContent";

        public string RootPath => "NoRoot";

        public IEnumerable<string> GetBranchList()
        {
            throw new NotImplementedException();
        }

        public string GetCurrentBranch()
        {
            throw new NotImplementedException();
        }

        public string GetRemoteUrl()
        {
            throw new NotImplementedException();
        }

        public void RefreshContent()
        {
            throw new NotImplementedException();
        }
    }
}
