using ConsoleCommander.Extensions;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Xunit.Abstractions;

namespace ConsoleCommander.Tests.Extensions
{
    public class CommanderContextExtensionsTest : TestBase
    {
        public CommanderContextExtensionsTest(ITestOutputHelper testOutputHelper) 
            : base(testOutputHelper)
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
