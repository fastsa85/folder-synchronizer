namespace FolderSynchronizer.App
{
    internal record FolderSynchronizerOptions(
        string SourceFolder,
        string ReplicaFolder,
        TimeSpan SyncInterval,
        string LogFilePath
    );
}
