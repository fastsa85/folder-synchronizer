namespace FolderSynchronizer.App;

internal static class CommandLineArgumentsParser
{
    const int COMMAND_LINE_ARGUMENTS_COUNT = 4;

    internal static FolderSynchronizerOptions Parse(string[] args)
    {
        if (args.Length != COMMAND_LINE_ARGUMENTS_COUNT)
        {
            throw new ArgumentException($"Exactly {COMMAND_LINE_ARGUMENTS_COUNT} command line arguments are required.");
        }

        return new FolderSynchronizerOptions(
            SourceFolder: args[0],
            ReplicaFolder: args[1],
            SyncInterval: TimeSpan.FromSeconds(int.Parse(args[2])),
            LogFilePath: args[3]
        );
    }
}
