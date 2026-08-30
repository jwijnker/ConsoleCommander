using ConsoleCommander.Extensions;
using Xunit;

namespace ConsoleCommander.Tests.Extensions
{
    public class CommanderContextExtensionsTest : TestBase
    {
        public CommanderContextExtensionsTest(DefaultTestFixture testFixture, ITestOutputHelper testOutputHelper)
            : base(testFixture, testOutputHelper)
        {
        }

        [Fact]
        public void NoContextByDefault_Test()
        {
            // Arrange
            var commander = new TestCommander();

            // Act
            var hasContext = commander.HasContext();

            // Assert
            Assert.False(hasContext);
        }

        [Fact]
        public void Commander_UseContext_Test()
        {
            // Arrange
            var commander = new TestCommander();

            // Act
            commander.UseContext();
            var hasContext = commander.HasContext();

            // Assert
            Assert.True(hasContext);
        }
    }
}
