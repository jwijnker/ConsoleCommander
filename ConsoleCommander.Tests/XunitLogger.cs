using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using Xunit;

namespace ConsoleCommander.Tests
{
    public class XunitLogger<T> : ILogger<T>, IDisposable
    {
        private readonly ITestOutputHelper _output;
        private readonly string CategoryName;

        public XunitLogger(string categoryName, ITestOutputHelper output)
        {
            _output = output;
            CategoryName = categoryName;
        }
        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception, string> formatter)
        {
            _output.WriteLine($"[{DateTimeOffset.UtcNow.LocalDateTime.ToString("HH:mm:ss.fff")}] [{logLevel.ToString()}] [{string.Join('.', CategoryName.Split('.').TakeLast(3))}]");
            _output.WriteLine($"  {state!.ToString()}");
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        #region Disposable

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            // Cleanup
        }

        public IDisposable? BeginScope<TState>(TState state) where TState : notnull
        {
            return this;
        }

        #endregion
    }
}
