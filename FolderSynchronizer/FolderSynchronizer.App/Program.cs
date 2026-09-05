namespace FolderSynchronizer.App
{
    internal class Program
    {
        static int Main(string[] args)
        {
            try
            {
                var options = CommandLineArgumentsParser.Parse(args);
                var synchronizationService = new FolderSynchronizationService();
                synchronizationService.Synchronize(options.SourceFolder, options.ReplicaFolder);

                return 0;
            }
            catch (ArgumentException ex)
            {
                Console.Error.WriteLine($"Invalid arguments: {ex.Message}");
                return 1;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Synchronization failed: {ex.Message}");
                return 1;
            }
        }
    }
}
