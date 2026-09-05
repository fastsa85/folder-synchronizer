using FolderSynchronizer.App;

namespace FolderSynchronizer.IntegrationTests
{
    [TestFixture]
    public class FolderSynchronizerServiceTests
    {
        [Test]
        public void Synchronize_WhenSourceContainsFile_CopiesFileToReplica()
        {
            var fileName = "test.txt";
            var fileContent = "Test content 123 !@#";

            // Arrange
            var sourceFolder = Directory.CreateTempSubdirectory();
            var replicaFolder = Directory.CreateTempSubdirectory();

            try
            {
                var sourceFile = Path.Combine(sourceFolder.FullName, fileName);
                File.WriteAllText(sourceFile, fileContent);

                var folderSynchronizerService = new FolderSynchronizationService();

                // Act
                folderSynchronizerService.Synchronize(sourceFolder.FullName, replicaFolder.FullName);

                // Assert
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
    }
}
