using FolderSynchronizer.App;

namespace FolderSynchronizer.IntegrationTests
{
    [TestFixture]
    public class FolderSynchronizerServiceTests
    {
        private FolderSynchronizationService folderSynchronizerService;

        [SetUp]
        public void Setup()
        {
            folderSynchronizerService = new FolderSynchronizationService();
        }

        [Test]
        public void Synchronize_WhenSourceContainsFile_CopiesFileToReplica()
        {
            var fileName = "test.txt";
            var fileContent = "Test content 123 !@#";

            // Arrange : create source and replica folders
            var sourceFolder = Directory.CreateTempSubdirectory();
            var replicaFolder = Directory.CreateTempSubdirectory();

            try
            {
                var sourceFile = Path.Combine(sourceFolder.FullName, fileName);
                File.WriteAllText(sourceFile, fileContent);

                // Act: run the synchronization
                folderSynchronizerService.Synchronize(sourceFolder.FullName, replicaFolder.FullName);

                // Assert: check the file has been copied
                var replicaFile = Path.Combine(replicaFolder.FullName, fileName);

                Assert.That(File.Exists(replicaFile), Is.True);
                Assert.That(File.ReadAllText(replicaFile), Is.EqualTo(fileContent));
            }
            finally
            {
                sourceFolder.Delete(recursive: true);
                replicaFolder.Delete(recursive: true);
            }
        }

        [Test]
        public void Synchronize_WhenNestedDirectory_CopiesDirectoryAndFilesToReplica()
        {
            // Arrange: create source and replica folders
            var sourceFolder = Directory.CreateTempSubdirectory();
            var replicaFolder = Directory.CreateTempSubdirectory();

            try
            {
                var nestedFolderName = "test-subfolder";
                var nestedFolder = Directory.CreateDirectory(Path.Combine(sourceFolder.FullName, nestedFolderName));

                var nestedFileName = "test-nested.txt";
                var nestedFileContent = "Nested file test content 123 !@#";
                var sourceFile = Path.Combine(nestedFolder.FullName, nestedFileName);
                File.WriteAllText(sourceFile, nestedFileContent);

                // Act: run the synchronization
                folderSynchronizerService.Synchronize(sourceFolder.FullName, replicaFolder.FullName);

                // Assert: check the sub-folder and file are copied
                var replicaNestedFolder = Path.Combine(replicaFolder.FullName, nestedFolderName);

                var replicaFile = Path.Combine(replicaNestedFolder, nestedFileName);

                Assert.That(Directory.Exists(replicaNestedFolder), Is.True);
                Assert.That(File.Exists(replicaFile), Is.True);
                Assert.That(File.ReadAllText(replicaFile), Is.EqualTo(nestedFileContent));
            }
            finally
            {
                sourceFolder.Delete(recursive: true);
                replicaFolder.Delete(recursive: true);
            }
        }

        [Test]
        public void Synchronize_WhenNestedEmptyDirectory_CopiesDirectoryToReplica()
        {
            // Arrange: create source and replica folders
            var sourceFolder = Directory.CreateTempSubdirectory();
            var replicaFolder = Directory.CreateTempSubdirectory();

            try
            {
                var nestedFolderName = "test-subfolder";
                var nestedFolder = Directory.CreateDirectory(Path.Combine(sourceFolder.FullName, nestedFolderName));

                // Act: run the synchronization
                folderSynchronizerService.Synchronize(sourceFolder.FullName, replicaFolder.FullName);

                // Assert: check the sub-folder was copied
                var replicaNestedFolder = Path.Combine(replicaFolder.FullName, nestedFolderName);

                Assert.That(Directory.Exists(replicaNestedFolder), Is.True);
                Assert.That(Directory.GetFileSystemEntries(nestedFolder.FullName), Is.Empty);
            }
            finally
            {
                sourceFolder.Delete(recursive: true);
                replicaFolder.Delete(recursive: true);
            }
        }

        [Test]
        public void Synchronize_WhenSourceDirectoryIsEmpty_DoesNotCopyAnyFilesOrDirectories()
        {
            // Arrange: create source and replica folders
            var sourceFolder = Directory.CreateTempSubdirectory();
            var replicaFolder = Directory.CreateTempSubdirectory();

            try
            {
                // Act: run the synchronization
                folderSynchronizerService.Synchronize(sourceFolder.FullName, replicaFolder.FullName);

                // Assert: check the directory was copied
                Assert.That(Directory.GetFileSystemEntries(replicaFolder.FullName), Is.Empty);
            }
            finally
            {
                sourceFolder.Delete(recursive: true);
                replicaFolder.Delete(recursive: true);
            }
        }

        [Test]
        public void Synchronize_WhenReplicaContainsObsoleteFile_RemovesFileFromReplica()
        {
            var sourceFolder = Directory.CreateTempSubdirectory();
            var replicaFolder = Directory.CreateTempSubdirectory();

            try
            {
                // Arrange: create current file in source and replica (this file should be kept in replica after synchronisation)
                var currentFileName = "current.txt";
                var currentFileContent = "Current file content 123 !@#";
                var sourceFile = Path.Combine(sourceFolder.FullName, currentFileName);
                File.WriteAllText(sourceFile, currentFileContent);

                var currentReplicaFile = Path.Combine(replicaFolder.FullName, currentFileName);
                File.WriteAllText(currentReplicaFile, currentFileContent);

                // Arrange: create an obsolete file in source and replica (this file should be removed from replica after synchronisation)
                var obsoleteFileName = "obsolete.txt";
                var obsoleteFileContent = "Obsolete file content 123 !@#";
                var obsoleteReplicaFile = Path.Combine(replicaFolder.FullName, obsoleteFileName);
                File.WriteAllText(obsoleteReplicaFile, obsoleteFileContent);

                // Act: run the synchronization
                folderSynchronizerService.Synchronize(sourceFolder.FullName, replicaFolder.FullName);

                // Assert: check the current file still exists in replica folder but the obsolete file has been deleted
                Assert.That(File.Exists(currentReplicaFile), Is.True);
                Assert.That(File.Exists(obsoleteReplicaFile), Is.False);
            }
            finally
            {
                sourceFolder.Delete(recursive: true);
                replicaFolder.Delete(recursive: true);
            }
        }
    }
}
