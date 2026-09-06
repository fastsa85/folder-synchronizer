using FolderSynchronizer.App;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Timers;

namespace FolderSynchronizer.UnitTests
{
    [TestFixture]
    public class FolderSynchronizationBackgroundServiceTests
    {
        [Test]
        public async Task ExecuteAsync_ShouldSynchronizeOnStart()
        {
            // Arrange
            var synchronizationService = new Mock<IFolderSynchronizationService>();

            var options = new FolderSynchronizerOptions(
                "source",
                "replica",
                TimeSpan.FromSeconds(3),
                "log.txt");

            var backgroundService = new FolderSynchronizationBackgroundService(options, synchronizationService.Object);

            using var cancellationTokenSource = new CancellationTokenSource();

            try
            {
                // Act
                await backgroundService.StartAsync(cancellationTokenSource.Token);

                // Assert
                synchronizationService.Verify(x => x.Synchronize("source", "replica"), Times.Once);
            }
            finally
            {
                // Cleanup
                await backgroundService.StopAsync(CancellationToken.None);
            }
        }

        [Test]
        public async Task ExecuteAsync_ShouldSynchronizePeriodically()
        {
            // Arrange
            var synchronizationService = new Mock<IFolderSynchronizationService>();

            var options = new FolderSynchronizerOptions(
                "source",
                "replica",
                    TimeSpan.FromMilliseconds(100),
                "log.txt");

            var backgroundService = new FolderSynchronizationBackgroundService(options, synchronizationService.Object);

            using var cancellationTokenSource = new CancellationTokenSource();

            try
            {
                // Act
                await backgroundService.StartAsync(cancellationTokenSource.Token);

                await Task.Delay(250);

                // Assert
                synchronizationService.Verify(x => x.Synchronize("source", "replica"), Times.AtLeast(2));
            }
            finally
            {
                // Cleanup
                await backgroundService.StopAsync(CancellationToken.None);
                backgroundService.Dispose();
            }
        }

        [Test]
        public async Task StopAsync_ShouldStopTheService()
        {
            var synchronizationService = new Mock<IFolderSynchronizationService>();

            var options = new FolderSynchronizerOptions(
                "source",
                "replica",
                TimeSpan.FromMilliseconds(100),
                "log.txt");

            var backgroundService = new FolderSynchronizationBackgroundService(options, synchronizationService.Object);

            using var cancellationTokenSource = new CancellationTokenSource();

            try
            {
                await backgroundService.StartAsync(cancellationTokenSource.Token);

                cancellationTokenSource.Cancel();

                Assert.DoesNotThrowAsync(async () => await backgroundService.StopAsync(CancellationToken.None));
            }
            finally
            {
                await backgroundService.StopAsync(CancellationToken.None);
                backgroundService.Dispose();
            }
        }
    }
}
