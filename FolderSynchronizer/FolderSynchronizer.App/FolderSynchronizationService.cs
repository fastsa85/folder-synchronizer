namespace FolderSynchronizer.App
{
    internal class FolderSynchronizationService
    {
        internal void Synchronize(string sourceFolder, string replicaFolder)
        {
            foreach (var sourceFile in Directory.GetFiles(sourceFolder))
            {
                var fileName = Path.GetFileName(sourceFile);
                var replicaFile = Path.Combine(replicaFolder, fileName);

                File.Copy(sourceFile, replicaFile, overwrite: true);
            }
        }
    }
}
