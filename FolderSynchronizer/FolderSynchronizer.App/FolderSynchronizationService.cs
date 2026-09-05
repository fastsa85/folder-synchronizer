namespace FolderSynchronizer.App
{
    internal class FolderSynchronizationService
    {
        internal void Synchronize(string sourceFolder, string replicaFolder)
        {
            SynchronizeDirectoryRecursively(sourceFolder, replicaFolder);
        }

        private void SynchronizeDirectoryRecursively(string sourceFolder, string replicaFolder)
        {
            Directory.CreateDirectory(replicaFolder);

            foreach (var file in Directory.GetFiles(sourceFolder))
            {
                var sourceFileRelativePath = Path.GetRelativePath(sourceFolder, file);
                var replicaFile = Path.Combine(replicaFolder, sourceFileRelativePath);
                var replicaFileDirectory = Path.GetDirectoryName(replicaFile);

                if (!Directory.Exists(replicaFileDirectory))
                {
                    Directory.CreateDirectory(replicaFileDirectory);
                }

                File.Copy(file, replicaFile, overwrite: true);
            }

            foreach (var sourceDirectory in Directory.GetDirectories(sourceFolder))
            {
                var directoryName = Path.GetFileName(sourceDirectory);
                var replicaDirectory = Path.Combine(replicaFolder, directoryName);

                SynchronizeDirectoryRecursively(sourceDirectory, replicaDirectory);
            }
        }
    }
}
