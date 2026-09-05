using Reqnroll;
using Reqnroll.BoDi;

namespace FolderSynchronizer.E2ETests.Hooks
{
    [Binding]
    public static class BeforeTestRunHooks
    {
        [BeforeTestRun]
        public static void Configure(IObjectContainer container)
        {
            var settingsPath = Path.Combine(AppContext.BaseDirectory, "appsettings.json");
            if (!File.Exists(settingsPath))
            {
                throw new InvalidOperationException($"Test settings file not found: {settingsPath}");
            }

            var json = File.ReadAllText(settingsPath);
            var settings = System.Text.Json.JsonSerializer.Deserialize<TestSettings>(json) ?? throw new InvalidOperationException("Test settings could not be loaded.");
            container.RegisterInstanceAs(settings);
        }
    }
}
