using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.Text.Json.Serialization;
using Xunit;
using Xunit.Abstractions;

namespace ConsoleCommander.Tests
{
    public class TestBase : IClassFixture<DefaultTestFixture>
    {
        #region Properties

        private ITestOutputHelper OutputHelper;
        protected DefaultTestFixture TestFixture { get; set; }
        protected ILogger Logger { get; set; }
        protected JsonSerializerOptions jsonSerializerOptions;

        #endregion

        #region Constructor(s)

        protected TestBase(DefaultTestFixture testFixture, ITestOutputHelper testOutputHelper)
        {
            TestFixture = testFixture;

            TestFixture.Services.AddLogging(o =>
            {
                o.ClearProviders()
                .AddProvider(new XunitLoggerFactory(testOutputHelper))
                .SetMinimumLevel(LogLevel.Trace);
            });

            Logger = testFixture.ServiceProvider.GetRequiredService<ILogger<TestBase>>(); // For logging in the test
            OutputHelper = testOutputHelper;

            jsonSerializerOptions = new JsonSerializerOptions();
            jsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        }

        #endregion

        protected void Log(string message)
        {
            OutputHelper.WriteLine(message);
        }

        protected ILogger<T> GetLogger<T>()
        {
            // call factory.AddConsole or other provider extension method
            return TestFixture.ServiceProvider.GetRequiredService<ILogger<T>>();
        }
    }
}
