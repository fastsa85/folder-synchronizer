using Reqnroll;
using System;
using System.Collections.Generic;
using System.Text;

namespace FolderSynchronizer.E2ETests.Hooks
{
    [Binding]
    internal class AfterScenarioHooks
    {
        [AfterScenario]
        public void CleanupTempDirectories(ScenarioState scenarioState)
        {
            if (!string.IsNullOrEmpty(scenarioState.SourceFolder) && Directory.Exists(scenarioState.SourceFolder))
            {
                Directory.Delete(scenarioState.SourceFolder, recursive: true);
            }

            if (!string.IsNullOrEmpty(scenarioState.ReplicaFolder) &&  Directory.Exists(scenarioState.ReplicaFolder))
            {
                Directory.Delete(scenarioState.ReplicaFolder, recursive: true);
            }
        }
    }
}
