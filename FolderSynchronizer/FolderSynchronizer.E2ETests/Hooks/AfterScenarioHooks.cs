using Reqnroll;

namespace FolderSynchronizer.E2ETests.Hooks
{
    [Binding]
    internal class AfterScenarioHooks
    {
        private readonly ScenarioState _scenarioState;

        public AfterScenarioHooks(ScenarioState scenarioState)
        {
            _scenarioState = scenarioState ?? throw new ArgumentNullException(nameof(scenarioState));
        }

        [AfterScenario]
        public void Cleanup()
        {
            StopSynchronizerProcess();
            CleanupTempDirectories();
            CleanupLogFile();
        }

        private void CleanupTempDirectories()
        {
            if (!string.IsNullOrEmpty(_scenarioState.SourceFolder) && Directory.Exists(_scenarioState.SourceFolder))
            {
                Directory.Delete(_scenarioState.SourceFolder, recursive: true);
            }

            if (!string.IsNullOrEmpty(_scenarioState.ReplicaFolder) && Directory.Exists(_scenarioState.ReplicaFolder))
            {
                Directory.Delete(_scenarioState.ReplicaFolder, recursive: true);
            }
        }

        private void CleanupLogFile()
        {
            if (!string.IsNullOrEmpty(_scenarioState.LogFilePath) && File.Exists(_scenarioState.LogFilePath))
            {
                File.Delete(_scenarioState.LogFilePath);
            }
        }

        private void StopSynchronizerProcess()
        {
            var process = _scenarioState.SynchronizerProcess;

            if (process is null || process.HasExited)
            {
                return;
            }

            process.Kill();
            process.WaitForExit();
            process.Dispose();

            _scenarioState.SynchronizerProcess = null!;
        }
    }
}
