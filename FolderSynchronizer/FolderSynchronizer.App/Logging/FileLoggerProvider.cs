using Microsoft.Extensions.Logging;

namespace FolderSynchronizer.App.Logging
{
    public class FileLoggerProvider : ILoggerProvider
    {
        private readonly StreamWriter _writer;

        public FileLoggerProvider(string logFilePath)
        {
            var directory = Path.GetDirectoryName(logFilePath);

            if (!string.IsNullOrEmpty(directory))
            {
                Directory.CreateDirectory(directory);
            }

            _writer = new StreamWriter(logFilePath, append: true)
            {
                AutoFlush = true
            };
        }

        public ILogger CreateLogger(string categoryName)
        {
            return new FileLogger(_writer, categoryName);
        }

        public void Dispose()
        {
            _writer.Dispose();
        }
    }
}
