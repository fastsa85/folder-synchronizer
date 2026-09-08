using Reqnroll;
using System.Diagnostics;

namespace FolderSynchronizer.E2ETests.Steps
{
    [Binding]
    public class ConsoleOutputsteps
    {
        private readonly ScenarioState _scenarioState;
        public ConsoleOutputsteps(ScenarioState scenarioState)
        {
            _scenarioState = scenarioState ?? throw new ArgumentNullException(nameof(scenarioState));
        }

        [Then("the console output contains the following messages:")]
        public async Task ThenTheConsoleOutputContainsTheFollowingMessages(DataTable dataTable)
        {
            foreach (var row in dataTable.Rows)
            {
                var message = row["Message"];
                var item = row["Item"];

                Assert.That(await WaitForConsoleOutputAsync(message, item), Is.True, $"Console output not found for message '{message}' and item '{item}'");
            }
        }

        private async Task<bool> WaitForConsoleOutputAsync(string expectedMessage, string expectedItem)
        {
            var stopwatch = Stopwatch.StartNew();

            while (stopwatch.Elapsed < TimeSpan.FromSeconds(_scenarioState.SyncInterval * 2))
            {
                var lines = _scenarioState.StandardOutput.Split(Environment.NewLine);

                if (lines.Any(line => line.Contains(expectedMessage, StringComparison.Ordinal) && line.Contains(expectedItem, StringComparison.Ordinal)))
                {
                    return true;
                }

                await Task.Delay(300);
            }

            return false;
        }
    }
}
