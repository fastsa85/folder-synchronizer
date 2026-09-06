using Reqnroll;
using System.Diagnostics;

namespace FolderSynchronizer.E2ETests.Steps
{
    [Binding]
    public class SynchronizationSteps
    {
        private readonly ScenarioState _scenarioState;
        private readonly TestSettings _testSettings;

        public SynchronizationSteps(
            ScenarioState scenarioState,
            TestSettings testSettings)
        {
            _scenarioState = scenarioState
                ?? throw new ArgumentNullException(nameof(scenarioState));

            _testSettings = testSettings
                ?? throw new ArgumentNullException(nameof(testSettings));
        }

        [When("I run the folder synchronizer")]
        public void WhenIRunTheFolderSynchronizer()
        {
            StartSynchronizer(3);
        }

        [When("I run the folder synchronizer with sync interval {int} seconds")]
        public void WhenIRunTheFolderSynchronizerWithSyncInterval(int syncInterval)
        {
            StartSynchronizer(syncInterval);
        }

        [When("I wait {int} seconds")]
        public async Task WhenIWaitSeconds(int seconds)
        {
            await Task.Delay(seconds * 1000);
        }

        private void StartSynchronizer(int syncInterval)
        {
            var logFilePath = Path.Combine(
                Path.GetTempPath(),
                $"FolderSynchronizer-E2E-{Guid.NewGuid():N}.log");

            var arguments =
                $"\"{_testSettings.FolderSynchronizerPath}\" " +
                $"\"{_scenarioState.SourceFolder}\" " +
                $"\"{_scenarioState.ReplicaFolder}\" " +
                $"{syncInterval} " +
                $"\"{logFilePath}\"";

            var processStartInfo = new ProcessStartInfo
            {
                FileName = "dotnet",
                Arguments = arguments,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            var process = Process.Start(processStartInfo)
                ?? throw new InvalidOperationException(
                    "Failed to start FolderSynchronizer.");

            _scenarioState.SynchronizerProcess = process;
            _scenarioState.LogFilePath = logFilePath;
        }
    }
}
