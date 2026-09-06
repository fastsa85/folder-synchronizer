using Microsoft.Extensions.Hosting;

namespace FolderSynchronizer.App
{
    internal sealed class FolderSynchronizationBackgroundService : BackgroundService
    {
        private readonly FolderSynchronizerOptions _options;
        private readonly IFolderSynchronizationService _synchronizationService;

        public FolderSynchronizationBackgroundService(FolderSynchronizerOptions options, IFolderSynchronizationService synchronizationService)
        {
            _options = options;
            _synchronizationService = synchronizationService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _synchronizationService.Synchronize(_options.SourceFolder, _options.ReplicaFolder);

            using var timer = new PeriodicTimer(_options.SyncInterval);

            while (await timer.WaitForNextTickAsync(stoppingToken))
            {
                _synchronizationService.Synchronize(_options.SourceFolder, _options.ReplicaFolder);
            }
        }
    }
}
