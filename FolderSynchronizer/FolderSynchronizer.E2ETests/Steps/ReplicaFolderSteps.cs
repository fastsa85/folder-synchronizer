using Reqnroll;
using System.Diagnostics;

namespace FolderSynchronizer.E2ETests.Steps
{
    [Binding]
    public class ReplicaFolderSteps
    {
        private TimeSpan WaitTimeOut => TimeSpan.FromSeconds(3); // seconds
        private readonly ScenarioState _scenarioState;

        public ReplicaFolderSteps(ScenarioState scenarioState)
        {
            _scenarioState = scenarioState ?? throw new ArgumentNullException(nameof(scenarioState));
        }

        [Given("an empty replica folder")]
        public void GivenAnEmptyReplicaFolder()
        {
            var replicaFolder = Directory.CreateTempSubdirectory("FolderSynchronizer-E2E-Replica-");
            _scenarioState.ReplicaFolder = replicaFolder.FullName;
        }

        [Then("the replica folder contains the following files:")]
        public void ThenTheSourceFolderContainsTheFollowingFiles(DataTable dataTable)
        {
            foreach (var row in dataTable.Rows)
            {
                var relativeFilePath = row["file"];

                var filePath = Path.Combine(_scenarioState.ReplicaFolder, relativeFilePath);

                Assert.That(WaitForFile(filePath, WaitTimeOut), Is.True, $"Expected file was not found in replica folder: {relativeFilePath}");
            }
        }

        [Then("the replica folder does not contain the following files:")]
        public void ThenTheReplicaFolderDoesNotContainTheFollowingFiles(DataTable dataTable)
        {
            foreach (var row in dataTable.Rows)
            {
                var relativeFilePath = row["file"];

                var filePath = Path.Combine(_scenarioState.ReplicaFolder, relativeFilePath);

                Assert.That(WaitForFile(filePath, WaitTimeOut), Is.False, $"Unexpected file was found in replica folder: {relativeFilePath}");
            }
        }

        [Then("the replica folder contains the following folders:")]
        public void ThenTheReplicaFolderContainsTheFollowingFolders(DataTable dataTable)
        {
            foreach (var row in dataTable.Rows)
            {
                var relativeFolderPath = row["folder"];

                var folderPath = Path.Combine(_scenarioState.ReplicaFolder, relativeFolderPath);

                Assert.That(WaitForDirectory(folderPath, WaitTimeOut), Is.True, $"Expected folder was not found in replica: {relativeFolderPath}");
            }
        }

        [Then("the folder {string} in the replica contains the following files:")]
        public void ThenTheFolderInTheReplicaContainsTheFollowingFiles(string relativeFolderPath, DataTable dataTable)
        {
            var replicaFolder = Path.Combine(_scenarioState.ReplicaFolder, relativeFolderPath);

            Assert.That(Directory.Exists(replicaFolder), Is.True, $"Expected replica folder was not found: {relativeFolderPath}");

            foreach (var row in dataTable.Rows)
            {
                var fileName = row["file"];

                var filePath = Path.Combine(replicaFolder, fileName);

                Assert.That(WaitForFile(filePath, WaitTimeOut), Is.True, $"Expected file was not found in replica folder '{relativeFolderPath}': {fileName}");
            }
        }

        private bool WaitForFile(string filePath, TimeSpan timeout)
        {
            var stopwatch = Stopwatch.StartNew();

            while (stopwatch.Elapsed < timeout)
            {
                if (File.Exists(filePath))
                {
                    return true;
                }

                Thread.Sleep(100);
            }

            return false;
        }

        private bool WaitForDirectory(string directory, TimeSpan timeout)
        {
            var stopwatch = Stopwatch.StartNew();

            while (stopwatch.Elapsed < timeout)
            {
                if (Directory.Exists(directory))
                {
                    return true;
                }

                Thread.Sleep(100);
            }

            return false;
        }
    }
}
