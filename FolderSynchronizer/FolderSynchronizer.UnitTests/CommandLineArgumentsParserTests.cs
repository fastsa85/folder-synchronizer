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

        [Test]
        public void Parse_WhenNotAllNumberOfArguments_ThrowsArgumentException()
        {
            var args = new[]
            {
                @"C:\Source",
                @"C:\Replica",
                "12"
            };

            Assert.That(() => CommandLineArgumentsParser.Parse(args), Throws.ArgumentException);
        }

        [Test]
        public void Parse_WhenExtraArguments_ThrowsArgumentException()
        {
            var args = new[]
            {
                @"C:\Source",
                @"C:\Replica",
                "10",
                @"C:\sync.log",
                "unexpected"
            };

            Assert.That(() => CommandLineArgumentsParser.Parse(args), Throws.ArgumentException);
        }

        [Test]
        public void Parse_WhenNonNumericSyncInterval_ThrowsArgumentException()
        {
            var args = new[]
            {
                @"C:\Source",
                 @"C:\Replica",
                "abc",              // !
                 @"C:\sync.log"
            };

            Assert.That(() => CommandLineArgumentsParser.Parse(args), Throws.ArgumentException);
        }

        [Test]
        public void Parse_WhenDecimalSyncInterval_ThrowsArgumentException()
        {
            var args = new[]
            {
                @"C:\Source",
                @"C:\Replica",
                "12.5",         // !
                @"C:\sync.log"
            };

            Assert.That(() => CommandLineArgumentsParser.Parse(args), Throws.ArgumentException);
        }

        [TestCase("0")]
        [TestCase("-10")]
        public void Parse_WhenNonPositiveSyncInterval_ThrowsArgumentException(string syncInterval)
        {
            var args = new[]
            {
                @"C:\Source",
                @"C:\Replica",
                syncInterval,   // !
                @"C:\sync.log"
            };

            Assert.That(() => CommandLineArgumentsParser.Parse(args), Throws.ArgumentException);
        }

        [TestCase("", @"C:\Replica", "10", @"C:\sync.log")]
        [TestCase(@"C:\Source", "", "10", @"C:\sync.log")]
        [TestCase(@"C:\Source", @"C:\Replica", "", @"C:\sync.log")]
        [TestCase(@"C:\Source", @"C:\Replica", "10", "")]
        public void Parse_WhenArgumentIsEmpty_ThrowsArgumentException(string sourceFolder, string replicaFolder, string syncInterval, string logFilePath)
        {
            var args = new[]
            {
                sourceFolder,
                replicaFolder,
                syncInterval,
                logFilePath
            };

            Assert.That(() => CommandLineArgumentsParser.Parse(args), Throws.ArgumentException);
        }

        [Test]
        public void Parse_WhenSourceAndReplicaFoldersAreTheSame_ThrowsArgumentException()
        {
            var args = new[]
            {
                @"C:\Data",     // !
                 @"C:\Data",    // !
                "10",
                @"C:\sync.log"
            };

            Assert.That(() => CommandLineArgumentsParser.Parse(args), Throws.ArgumentException);
        }
    }
}
