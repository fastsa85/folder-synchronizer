using FolderSynchronizer.App;
using Microsoft.Extensions.Logging;
using Moq;

namespace FolderSynchronizer.UnitTests
{
    /// <summary>
    /// The tests vefifies that the FolderSynchronizationService correctly logs, assuming that log messages are part of the contract and don't mutate too much.
    /// Asserts are made on the log messages to ensure that they contain the expected information.
    /// </summary>
    public class FolderSynchronizationServiceTests
    {
        private string _sourceFolder;
        private string _replicaFolder;

        private Mock<ILogger<FolderSynchronizationService>> _loggerMock;
        private FolderSynchronizationService _folderSynchronizatioService;

        [SetUp]
        public void SetUp()
        {
            _sourceFolder = Directory.CreateTempSubdirectory("FolderSynchronizer-Unit-Source-").FullName;
            _replicaFolder = Directory.CreateTempSubdirectory("FolderSynchronizer-Unit-Replica-").FullName;

            _loggerMock = new Mock<ILogger<FolderSynchronizationService>>();

            _folderSynchronizatioService = new FolderSynchronizationService(_loggerMock.Object);
        }

        [TearDown]
        public void TearDown()
        {
            if (Directory.Exists(_sourceFolder))
            {
                Directory.Delete(_sourceFolder, recursive: true);
            }

            if (Directory.Exists(_replicaFolder))
            {
                Directory.Delete(_replicaFolder, recursive: true);
            }
        }

        [Test]
        public void Synchronize_WhenFileIsCopied_ShouldLogCopyOperation()
        {
            // Arrange
            var sourceFileName = "test.txt";
            var sourceFile = Path.Combine(_sourceFolder, sourceFileName);
            File.WriteAllText(sourceFile, "test content");

            // Act
            _folderSynchronizatioService.Synchronize(_sourceFolder, _replicaFolder);

            // Assert
            _loggerMock.Verify(logger => logger.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((state, _) =>
                    state.ToString()!.Contains("Copied file from source") &&
                    state.ToString()!.Contains(_replicaFolder) &&
                    state.ToString()!.Contains(sourceFileName)),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
        }

        [Test]
        public void Synchronize_WhenObsoleteFileIsRemoved_ShouldLogRemoval()
        {
            // Arrange
            var obsoleteFileName = "obsolete.txt";
            var obsoleteFile = Path.Combine(_replicaFolder, obsoleteFileName);
            File.WriteAllText(obsoleteFile, "obsolete");

            // Act
            _folderSynchronizatioService.Synchronize(_sourceFolder, _replicaFolder);

            // Assert
            _loggerMock.Verify(logger => logger.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((state, _) =>
                    state.ToString()!.Contains("Removed obsolete file") &&
                    state.ToString()!.Contains(_replicaFolder) &&
                    state.ToString()!.Contains(obsoleteFileName)),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
        }

        [Test]
        public void Synchronize_WhenObsoleteDirectoryIsRemoved_ShouldLogRemoval()
        {
            // Arrange
            var obsoleteDirectoryName = "obsolete-directory";
            var obsoleteDirectory = Path.Combine(_replicaFolder, obsoleteDirectoryName);

            Directory.CreateDirectory(obsoleteDirectory);

            // Act
            _folderSynchronizatioService.Synchronize(_sourceFolder, _replicaFolder);

            // Assert
            _loggerMock.Verify(logger => logger.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((state, _) =>
                    state.ToString()!.Contains("Removed obsolete directory") &&
                    state.ToString()!.Contains(_replicaFolder) &&
                    state.ToString()!.Contains(obsoleteDirectoryName)),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
        }

        [Test]
        public void Synchronize_WhenMultipleOperationsOccur_ShouldLogEachOperation()
        {
            // Arrange
            var sourceFile = Path.Combine(_sourceFolder, "source.txt");
            File.WriteAllText(sourceFile, "source");

            var obsoleteFile = Path.Combine(_replicaFolder, "obsolete.txt");
            File.WriteAllText(obsoleteFile, "obsolete");

            // Act
            _folderSynchronizatioService.Synchronize(_sourceFolder, _replicaFolder);

            // Assert
            _loggerMock.Verify(logger => logger.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((state, _) =>
                    state.ToString()!.Contains("Copied file from source")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);

            _loggerMock.Verify(logger => logger.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((state, _) =>
                    state.ToString()!.Contains("Removed obsolete file")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);

        }
    }
}
