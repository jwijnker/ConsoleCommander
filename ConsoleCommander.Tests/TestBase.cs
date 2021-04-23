using Divergic.Logging.Xunit;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace ConsoleCommander.Tests
{
    public class TestBase
    {
        private ITestOutputHelper _testOutputHelper;

        public TestBase(ITestOutputHelper testOutputHelper)
        {
            this._testOutputHelper = testOutputHelper;
        }

        protected void Log(string message)
        {
            this._testOutputHelper.WriteLine(message);
        }

        protected ILogger<T> GetLogger<T>()
        {
            // call factory.AddConsole or other provider extension method
            var factory = LogFactory.Create(_testOutputHelper);

            return factory.CreateLogger<T>();
        }
    }
}
