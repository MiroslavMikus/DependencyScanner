public static class Paths
{
    public static FilePath SolutionPath => "DependencyScanner.sln";
    public static FilePath ChocoNuspecFile => "DependencyScanner.Standalone/DependencyScanner.Standalone.nuspec";
    public static FilePath ChocoHashFile => "DependencyScanner.Standalone/choco/DependencyScanner.Standalone.exe.hash.txt";
}

public static FilePath Combine(DirectoryPath directory, FilePath file)
{
    return directory.CombineWithFilePath(file);
}

public void CheckComfiguration(string required, string current)
{
    if (!required.Equals(current))
    {
        throw new Exception($"Required configuration '{required}' doesn't match current configuration '{current}'!");
    }
}

public void EnsureDoesNotExist(string path)
{
    if(FileExists(path)) 
        DeleteFile(path);
}

public void EnsureDoesNotExist(FilePath path)
{
    if(FileExists(path)) 
        DeleteFile(path);
}