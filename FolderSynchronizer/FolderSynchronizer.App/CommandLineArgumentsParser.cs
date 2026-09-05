namespace FolderSynchronizer.App;

internal static class CommandLineArgumentsParser
{
    const int COMMAND_LINE_ARGUMENTS_COUNT = 4;

    internal static FolderSynchronizerOptions Parse(string[] args)
    {
        if (args.Length != COMMAND_LINE_ARGUMENTS_COUNT)
        {
            throw new ArgumentException($"Exactly {COMMAND_LINE_ARGUMENTS_COUNT} command line arguments are required");
        }

        if (args.Any(x => string.IsNullOrEmpty(x)))
        {
            throw new ArgumentException("Command line arguments can not be empty.");
        }
        
        int syncInterval;

        if (!int.TryParse(args[2], out syncInterval))
        {
            throw new ArgumentException("Sync interval must be a valid integer.");
        }

        if (syncInterval <= 0)
        {
            throw new ArgumentException("Sync interval must be positive");
        }

        if (args[0].Equals(args[1], StringComparison.OrdinalIgnoreCase))
        {
            throw new ArgumentException("Source and replica folders must be different.");
        }

        return new FolderSynchronizerOptions(
            SourceFolder: args[0],
            ReplicaFolder: args[1],
            SyncInterval: TimeSpan.FromSeconds(syncInterval),
            LogFilePath: args[3]
        );
    }
}
