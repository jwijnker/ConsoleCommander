using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using Xunit;

namespace ConsoleCommander.Tests
{
    [DebuggerStepThrough]
    public class XunitLoggerFactory : ILoggerProvider
    {
        private readonly ITestOutputHelper _output;
        public XunitLoggerFactory(ITestOutputHelper output)
        {
            _output = output;
        }

        public ILogger CreateLogger(string categoryName)
        {
            return new XunitLogger<object>(categoryName, _output);
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

        #endregion
    }
}
