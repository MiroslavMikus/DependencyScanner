namespace DependencyScanner.Core.GitClient
{
    public static class GitCommand
    {
        public const string Pull = "pull";
        public const string BranchList = "branch";
        public const string Status = "status";
        public const string SwitchBranch = "checkout";
        public const string RemoteBranch = "remote -v";
    }
}
