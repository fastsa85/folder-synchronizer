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
        private const int SYNC_INTERVAL_MS = 100;
        private FolderSynchronizationBackgroundService _backgroundService;
        private Mock<IFolderSynchronizationService> _synchronizationService;
        private CancellationTokenSource _cancellationTokenSource;

        [SetUp]
        public void SetUp()
        {
            _synchronizationService = new Mock<IFolderSynchronizationService>();

            _cancellationTokenSource = new CancellationTokenSource();

            var options = new FolderSynchronizerOptions(
                "source",
                "replica",
                TimeSpan.FromMilliseconds(SYNC_INTERVAL_MS),
                "log.txt");

            _backgroundService = new FolderSynchronizationBackgroundService(options, _synchronizationService.Object);
        }

        [TearDown]
        public async Task TearDown()
        {
            await _backgroundService?.StopAsync(CancellationToken.None);
            _backgroundService?.Dispose();
            _cancellationTokenSource?.Dispose();
        }

        [Test]
        public async Task ExecuteAsync_ShouldSynchronizeOnStart()
        { 
            await _backgroundService.StartAsync(_cancellationTokenSource.Token);
             _synchronizationService.Verify(x => x.Synchronize("source", "replica"), Times.Once);
        }

        [Test]
        public async Task ExecuteAsync_ShouldSynchronizePeriodically()
        {
           
            await _backgroundService.StartAsync(_cancellationTokenSource.Token);
            await Task.Delay(SYNC_INTERVAL_MS * 2 + 50); // 50 ms is a small additional delay to ensure the second synchronization happens

            _synchronizationService.Verify(x => x.Synchronize("source", "replica"), Times.AtLeast(2));
        }

        [Test]
        public async Task StopAsync_ShouldStopTheService()
        {            
            await _backgroundService.StartAsync(_cancellationTokenSource.Token);
            _cancellationTokenSource.Cancel();

            Assert.DoesNotThrowAsync(async () => await _backgroundService.StopAsync(CancellationToken.None));
        }
    }
}
