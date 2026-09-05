namespace FolderSynchronizer.E2ETests
{
    public class ScenarioState
    {
        public string SourceFolder { get; set; } = null!;
        public string ReplicaFolder { get; set; } = null!;

        public string LogFilePath { get; set; } = null!;

        public int ExitCode { get; set; }

        public string StandardOutput { get; set; } = string.Empty;

        public string StandardError { get; set; } = string.Empty;
    }
}
