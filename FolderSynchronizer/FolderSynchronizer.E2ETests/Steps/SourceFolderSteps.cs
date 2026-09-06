using Reqnroll;

namespace FolderSynchronizer.E2ETests.Steps
{
    [Binding]
    public class SourceFolderSteps
    {
        private const string TestAssetsFolder = "TestAssets";
        private readonly ScenarioState _scenarioState;

        public SourceFolderSteps(ScenarioState scenarioState)
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

        [Given("the source folder contains the following folders:")]
        public void GivenTheSourceFolderContainsTheFollowingFolders(DataTable dataTable)
        {
            foreach (var row in dataTable.Rows)
            {
                var relativeFolderPath = row["folder"];

                var folderPath = Path.Combine(_scenarioState.SourceFolder, relativeFolderPath);

                Directory.CreateDirectory(folderPath);
            }
        }

        [Given("the folder {string} in the source contains the following files:")]
        public void GivenTheFolderInTheSourceContainsTheFollowingFiles(string relativeFolderPath, DataTable dataTable)
        {
            var destinationFolder = Path.Combine(_scenarioState.SourceFolder, relativeFolderPath);

            foreach (var row in dataTable.Rows)
            {
                var fileName = row["file"];

                var assetPath = Path.Combine(AppContext.BaseDirectory, TestAssetsFolder, fileName);

                if (!File.Exists(assetPath))
                {
                    throw new FileNotFoundException( $"Test asset not found: {assetPath}");
                }

                var destinationPath = Path.Combine(destinationFolder, fileName);

                File.Copy(assetPath, destinationPath);
            }
        }

        [When("I rename the following files in the source folder:")]
        public void WhenIRenameTheFollowingFilesInTheSourceFolder(DataTable dataTable)
        {
            foreach (var row in dataTable.Rows)
            {
                var originalFileName = row["original"];
                var newFileName = row["new"];

                var originalFilePath = Path.Combine(_scenarioState.SourceFolder, originalFileName);

                var newFilePath = Path.Combine(_scenarioState.SourceFolder, newFileName);

                File.Move(originalFilePath, newFilePath);
            }
        }
    }
}
