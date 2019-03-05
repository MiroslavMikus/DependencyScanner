namespace DependencyScanner.Core.GitClient
{
    public static class GitCommand
    {
        public const string Pull = "pull";
        public const string BranchList = "branch -a";
        public const string Status = "status";
        public const string SwitchBranch = "checkout";
        public const string RemoteBranch = "remote -v";
        public const string Fetch = "fetch";
        public const string CommitCount = "rev-list --all --count";
        public const string UpdateRemoteReferences = "remote update origin --prune";
    }
}
