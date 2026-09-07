using FolderSynchronizer.App.Logging;
using Microsoft.Extensions.Logging;

namespace FolderSynchronizer.IntegrationTests
{
    [TestFixture]
    public class FileLoggerProviderIntegrationTests
    {
        private string _logFilePath = null!;

        [SetUp]
        public void SetUp()
        {
            _logFilePath = Path.Combine(Path.GetTempPath(), $"FolderSynchronizer-Test-{Guid.NewGuid():N}.log");
        }

        [TearDown]
        public void TearDown()
        {
            if (File.Exists(_logFilePath))
            {
                File.Delete(_logFilePath);
            }
        }

        [Test]
        public void Log_ShouldWriteMessageToFile()
        {
            // Arrange
            using (var provider = new FileLoggerProvider(_logFilePath))
            {
                var logger = provider.CreateLogger("Test");

                // Act
                logger.LogInformation("Test log message");
            }

            // Assert
            var logContent = File.ReadAllText(_logFilePath);
            Assert.That(logContent, Does.Contain("Test log message"));
        }
    }
}
