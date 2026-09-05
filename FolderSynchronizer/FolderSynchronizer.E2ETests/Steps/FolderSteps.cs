using Reqnroll;

namespace FolderSynchronizer.E2ETests.Steps
{
    [Binding]
    public class FolderSteps
    {
        private const string TestAssetsFolder = "TestAssets";
        private readonly ScenarioState _scenarioState;

        public FolderSteps(ScenarioState scenarioState)
        {
            _scenarioState = scenarioState ?? throw new ArgumentNullException(nameof(scenarioState));
        }

        [Given("a source folder")]
        public void GivenASourceFolder()
        {
            var sourceFolder = Directory.CreateTempSubdirectory("FolderSynchronizer-E2E-Source-");
            _scenarioState.SourceFolder = sourceFolder.FullName;
        }

        [Given("the source folder contains the following files:")]
        public void GivenTheSourceFolderContainsTheFollowingFiles(DataTable dataTable)
        {
            foreach (var row in dataTable.Rows)
            {
                var assetFileName = row["file"];

                var assetPath = Path.Combine(AppContext.BaseDirectory, TestAssetsFolder, assetFileName);

                if (!File.Exists(assetPath))
                {
                    throw new FileNotFoundException($"Test asset not found: {assetPath}");
                }

                var destinationPath = Path.Combine(_scenarioState.SourceFolder, assetFileName);

                File.Copy(assetPath, destinationPath);
            }
        }

        [Given("an empty replica folder")]
        public void GivenAnEmptyReplicaFolder()
        {
            var replicaFolder = Directory.CreateTempSubdirectory("FolderSynchronizer-E2E-Replica-");
            _scenarioState.ReplicaFolder = replicaFolder.FullName;
        }

        [Then("the source folder contains the following files:")]
        public void ThenTheSourceFolderContainsTheFollowingFiles(DataTable dataTable)
        {
            foreach (var row in dataTable.Rows)
            {
                var relativeFilePath = row["file"];

                var filePath = Path.Combine(_scenarioState.SourceFolder, relativeFilePath);

                Assert.That(File.Exists(filePath), Is.True, $"Expected file was not found in source folder: {relativeFilePath}");
            }
        }
    }
}
