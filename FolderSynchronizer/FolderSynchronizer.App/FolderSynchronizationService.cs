using Microsoft.Extensions.Logging;

namespace FolderSynchronizer.App
{
    public class FolderSynchronizationService : IFolderSynchronizationService
    {
        private readonly ILogger<FolderSynchronizationService> _logger;

        public FolderSynchronizationService(ILogger<FolderSynchronizationService> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public void Synchronize(string sourceFolder, string replicaFolder)
        {
            SynchronizeDirectoryRecursively(sourceFolder, replicaFolder);
        }

        private void SynchronizeDirectoryRecursively(string sourceFolder, string replicaFolder)
        {
            EnsureReplcaFolderExists(replicaFolder);
            SyncronizeFiles(sourceFolder, replicaFolder);
            RemoveObsoleteFilesFromReplica(sourceFolder, replicaFolder);
            RemoveObsoleteFoldersFromReplica(sourceFolder, replicaFolder);

            foreach (var sourceDirectory in Directory.GetDirectories(sourceFolder))
            {
                var directoryName = Path.GetFileName(sourceDirectory);
                var replicaDirectory = Path.Combine(replicaFolder, directoryName);

                SynchronizeDirectoryRecursively(sourceDirectory, replicaDirectory);
            }
        }

        private void RemoveObsoleteFoldersFromReplica(string sourceFolder, string replicaFolder)
        {
            foreach (var replicaDirectory in Directory.GetDirectories(replicaFolder))
            {
                var directoryName = Path.GetFileName(replicaDirectory);
                var sourceDirectory = Path.Combine(sourceFolder, directoryName);

                if (!Directory.Exists(sourceDirectory))
                {
                    Directory.Delete(replicaDirectory, recursive: true);
                    _logger.LogInformation("Removed obsolete directory in replica folder: {replicaDirectory}", replicaDirectory);
                }
            }
        }

        private void RemoveObsoleteFilesFromReplica(string sourceFolder, string replicaFolder)
        {
            foreach (var replicaFile in Directory.GetFiles(replicaFolder))
            {
                var fileName = Path.GetFileName(replicaFile);
                var sourceFile = Path.Combine(sourceFolder, fileName);

                if (!File.Exists(sourceFile))
                {
                    File.Delete(replicaFile);
                    _logger.LogInformation("Removed obsolete file in replica folder: {replicaFile}", replicaFile);
                }
            }
        }

        private void SyncronizeFiles(string sourceFolder, string replicaFolder)
        {
            foreach (var file in Directory.GetFiles(sourceFolder))
            {
                var sourceFileRelativePath = Path.GetRelativePath(sourceFolder, file);
                var replicaFile = Path.Combine(replicaFolder, sourceFileRelativePath);
                var replicaFileDirectory = Path.GetDirectoryName(replicaFile);

                if (!Directory.Exists(replicaFileDirectory))
                {
                    Directory.CreateDirectory(replicaFileDirectory);
                }

                if (IsCopyFileRequired(file, replicaFile))
                {
                    File.Copy(file, replicaFile, overwrite: true);

                    File.SetLastWriteTimeUtc(
                        replicaFile,
                        File.GetLastWriteTimeUtc(file));

                    _logger.LogInformation(
                        "Copied file from source: {sourceFile} to replica: {replicaFile}",
                        file,
                        replicaFile);
                }
            }
        }

        private void EnsureReplcaFolderExists(string replicaFolder)
        {
            if (!Directory.Exists(replicaFolder))
            {
                Directory.CreateDirectory(replicaFolder);
                _logger.LogInformation("Created directory in replica folder: {replicaFolder}", replicaFolder);
            }
        }

        private bool IsCopyFileRequired(string sourceFile, string replicaFile)
        {
            if (!File.Exists(replicaFile))
            {
                return true;
            }

            var sourceFileInfo = new FileInfo(sourceFile);
            var replicaFileInfo = new FileInfo(replicaFile);

            return sourceFileInfo.LastWriteTimeUtc != replicaFileInfo.LastWriteTimeUtc || sourceFileInfo.Length != replicaFileInfo.Length;
        }
    }
}
