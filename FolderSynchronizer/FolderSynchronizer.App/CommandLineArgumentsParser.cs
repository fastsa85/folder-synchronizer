namespace FolderSynchronizer.App;

internal static class CommandLineArgumentsParser
{
    internal static FolderSynchronizerOptions Parse(string[] args)
    {
        return new FolderSynchronizerOptions(
            SourceFolder: args[0],
            ReplicaFolder: args[1],
            SyncInterval: TimeSpan.FromSeconds(int.Parse(args[2])),
            LogFilePath: args[3]
        );
    }
}
