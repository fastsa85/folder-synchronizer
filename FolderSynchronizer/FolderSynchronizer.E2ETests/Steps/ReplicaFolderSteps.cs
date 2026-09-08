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

        [Given("the replica folder contains the following files:")]
        public void GivenTheReplicaFolderContainsTheFollowingFiles(DataTable dataTable)
        {
            foreach (var row in dataTable.Rows)
            {
                var fileName = row["file"];

                var assetPath = Path.Combine(AppContext.BaseDirectory, _scenarioState.TestAssetsFolder, fileName);

                if (!File.Exists(assetPath))
                {
                    throw new FileNotFoundException($"Test asset not found: {assetPath}");
                }

                var destinationPath = Path.Combine(_scenarioState.ReplicaFolder, fileName);

                File.Copy(assetPath, destinationPath);
            }
        }

        [Then("the replica folder contains the following files:")]
        public async Task ThenTheSourceFolderContainsTheFollowingFiles(DataTable dataTable)
        {
            foreach (var row in dataTable.Rows)
            {
                var relativeFilePath = row["file"];

                var filePath = Path.Combine(_scenarioState.ReplicaFolder, relativeFilePath);

                Assert.That(await WaitForFileAsync(filePath, WaitTimeOut), Is.True, $"Expected file was not found in replica folder: {relativeFilePath}");
            }
        }

        [Then("the replica folder does not contain the following files:")]
        public async Task ThenTheReplicaFolderDoesNotContainTheFollowingFiles(DataTable dataTable)
        {
            foreach (var row in dataTable.Rows)
            {
                var relativeFilePath = row["file"];

                var filePath = Path.Combine(_scenarioState.ReplicaFolder, relativeFilePath);

                Assert.That(await WaitForFileDisappearAsync(filePath, WaitTimeOut), Is.True, $"Unexpected file was found in replica folder: {relativeFilePath}");
            }
        }

        [Then("the replica folder contains the following folders:")]
        public async Task ThenTheReplicaFolderContainsTheFollowingFolders(DataTable dataTable)
        {
            foreach (var row in dataTable.Rows)
            {
                var relativeFolderPath = row["folder"];

                var folderPath = Path.Combine(_scenarioState.ReplicaFolder, relativeFolderPath);

                Assert.That(await WaitForDirectoryAsync(folderPath, WaitTimeOut), Is.True, $"Expected folder was not found in replica: {relativeFolderPath}");
            }
        }

        [Then("the replica folder does not contain the following folders:")]
        public async Task ThenTheReplicaFolderDoesNotContainTheFollowingFolders(DataTable dataTable)
        {
            foreach (var row in dataTable.Rows)
            {
                var relativeFolderPath = row["folder"];

                var folderPath = Path.Combine(_scenarioState.ReplicaFolder, relativeFolderPath);

                Assert.That(await WaitForDirectoryDisapearAsync(folderPath, WaitTimeOut), Is.True, $"Unxpected folder was found in replica: {relativeFolderPath}");
            }
        }

        [Then("the folder {string} in the replica contains the following files:")]
        public async Task ThenTheFolderInTheReplicaContainsTheFollowingFiles(string relativeFolderPath, DataTable dataTable)
        {
            var replicaFolder = Path.Combine(_scenarioState.ReplicaFolder, relativeFolderPath);

            Assert.That(Directory.Exists(replicaFolder), Is.True, $"Expected replica folder was not found: {relativeFolderPath}");

            foreach (var row in dataTable.Rows)
            {
                var fileName = row["file"];

                var filePath = Path.Combine(replicaFolder, fileName);

                Assert.That(await WaitForFileAsync(filePath, WaitTimeOut), Is.True, $"Expected file was not found in replica folder '{relativeFolderPath}': {fileName}");
            }
        }

        [Then("the content of the file {string} in the replica should be:")]
        public async Task ThenTheContentOfTheFileInTheReplicaShouldBe(string fileName, string expectedContent)
        {
            var filePath = Path.Combine(_scenarioState.ReplicaFolder, fileName);

            Assert.That(await WaitForFileContentAsync(filePath, expectedContent, WaitTimeOut),
                Is.True,
                $"File '{fileName}' in replica does not contain the expected content.");
        }

        private async Task<bool> WaitForFileAsync(string filePath, TimeSpan timeout)
        {
            var stopwatch = Stopwatch.StartNew();

            while (stopwatch.Elapsed < timeout)
            {
                if (File.Exists(filePath))
                {
                    return true;
                }

                await Task.Delay(100);
            }

            return false;
        }

        private async Task<bool> WaitForFileDisappearAsync(string filePath, TimeSpan timeout)
        {
            var stopwatch = Stopwatch.StartNew();

            while (stopwatch.Elapsed < timeout)
            {
                if (!File.Exists(filePath))
                {
                    return true;
                }

                await Task.Delay(100);
            }

            return false;
        }

        private async Task<bool> WaitForDirectoryAsync(string directory, TimeSpan timeout)
        {
            var stopwatch = Stopwatch.StartNew();

            while (stopwatch.Elapsed < timeout)
            {
                if (Directory.Exists(directory))
                {
                    return true;
                }

                await Task.Delay(100);
            }

            return false;
        }

        private async Task<bool> WaitForDirectoryDisapearAsync(string directory, TimeSpan timeout)
        {
            var stopwatch = Stopwatch.StartNew();

            while (stopwatch.Elapsed < timeout)
            {
                if (!Directory.Exists(directory))
                {
                    return true;
                }

                await Task.Delay(100);
            }

            return false;
        }

        private async Task<bool> WaitForFileContentAsync(string filePath, string expectedContent, TimeSpan timeout)
        {
            var stopwatch = Stopwatch.StartNew();

            while (stopwatch.Elapsed < timeout)
            {
                if (File.Exists(filePath))
                {
                    var actualContent = await File.ReadAllTextAsync(filePath);

                    if (actualContent == expectedContent)
                    {
                        return true;
                    }
                }

                await Task.Delay(100);
            }

            return false;
        }
    }
}
