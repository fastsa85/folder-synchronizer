using Microsoft.Extensions.Logging;

namespace FolderSynchronizer.App.Logging
{
    public class FileLogger : ILogger
    {
        private readonly StreamWriter _writer;
        private readonly string _categoryName;

        public FileLogger(StreamWriter writer, string categoryName)
        {
            _writer = writer ?? throw new ArgumentNullException(nameof(writer));
            _categoryName = categoryName ?? throw new ArgumentNullException(nameof(categoryName));
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            if (IsEnabled(logLevel))
            {
                var logMessage = formatter(state, exception);                

                if (exception != null)
                {
                    _writer.WriteLine(exception.ToString());
                }

                _writer.WriteLine($"{DateTime.Now} - {logLevel}: [{_categoryName}] {logMessage}");
            }
        }

        public bool IsEnabled(LogLevel logLevel) => logLevel >= LogLevel.Information;

        public IDisposable? BeginScope<TState>(TState state) where TState : notnull => null;
    }
}
