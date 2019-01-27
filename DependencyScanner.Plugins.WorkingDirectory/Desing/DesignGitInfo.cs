using System;
using DependencyScanner.Api.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.IO;

namespace DependencyScanner.Plugins.Wd.Desing
{
    internal class DesignGitInfo : IGitInfo
    {
        public IEnumerable<string> BranchList => new[] { "branch1", "branch2" };

        public IGitConfig Config { get => new DesignGitConfig(); set => throw new NotImplementedException(); }
        public string CurrentBranch { get => "master"; set => throw new NotImplementedException(); }

        public bool IsBehind => false;

        public bool IsClean => true;

        public string RemoteUrl => "Remote Url";

        public FileInfo Root => new FileInfo("test");

        public string Status => "ok";

        public event PropertyChangedEventHandler PropertyChanged;

        public string Checkout(string branch)
        {
            throw new NotImplementedException();
        }

        public Task Init(bool executeGitFetch)
        {
            throw new NotImplementedException();
        }

        public void Pull()
        {
            throw new NotImplementedException();
        }
    }
}
