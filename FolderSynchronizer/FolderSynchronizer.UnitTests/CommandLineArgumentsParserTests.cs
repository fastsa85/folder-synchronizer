using FolderSynchronizer.App;

namespace FolderSynchronizer.UnitTests
{
    [TestFixture]
    public class Tests
    {
        [Test]
        public void Parse_WithValidArguments_ReturnsOptions()
        {
            var args = new[] {
                @"C:\Source",
                @"C:\Replica",
                "10",
                @"C:\sync.log"
            };

            var result = CommandLineArgumentsParser.Parse(args);

            Assert.That(result.SourceFolder, Is.EqualTo(@"C:\Source"));
            Assert.That(result.ReplicaFolder, Is.EqualTo(@"C:\Replica"));
            Assert.That(result.SyncInterval, Is.EqualTo(TimeSpan.FromSeconds(10)));
            Assert.That(result.LogFilePath, Is.EqualTo(@"C:\sync.log"));
        }
    }
}
