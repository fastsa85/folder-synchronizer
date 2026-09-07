using Reqnroll;
using System;
using System.Collections.Generic;
using System.Text;

namespace FolderSynchronizer.E2ETests.Steps
{
    [Binding]
    public class LogFileSteps
    {
        private readonly ScenarioState _scenarioState;

        public LogFileSteps(ScenarioState scenarioState)
        {
            _scenarioState = scenarioState ?? throw new ArgumentNullException(nameof(scenarioState));
        }

        [Then("the log file is generated and is not empty")]
        public void ThenTheLogFileExistsAndIsNotEmpty()
        {
            Assert.That(File.Exists(_scenarioState.LogFilePath), Is.True, $"Log file was not found: {_scenarioState.LogFilePath}");

            var fileInfo = new FileInfo(_scenarioState.LogFilePath);

            Assert.That(fileInfo.Length, Is.GreaterThan(0), $"Log file is empty: {_scenarioState.LogFilePath}");
        }
    }
}
