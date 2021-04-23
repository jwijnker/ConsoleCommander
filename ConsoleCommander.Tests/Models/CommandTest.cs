using ConsoleCommander.Models;
using System;
using Xunit;
using Xunit.Abstractions;

namespace ConsoleCommander.Tests.Models
{
    public class CommandTest : TestBase
    {
        public CommandTest(ITestOutputHelper testOutputHelper)
            : base(testOutputHelper)
        {

        }

        [Fact]
        public void Initialize_Test()
        {
            // Arrange
            var id = "ID";
            var description = "DESCRIPTION";

            // Act
            var command = new StringCommand(id, description, () => { });

            // Assert
            Assert.Equal(id, command.Id);
            Assert.Equal(description, command.Description);
        }

        [Fact]
        public void Initialize_InvalidId_Test()
        {
            // Arrange
            var id = string.Empty;
            var description = "DESCRIPTION";

            // Act
            var e = Assert.Throws<ArgumentNullException>(() => new StringCommand(id, description, () => { } ));

            // Assert
            Assert.Equal("id", e.ParamName);
        }

        [Fact]
        public void Initialize_InvalidDescription_Test()
        {
            // Arrange
            var id = "ID";
            var description = string.Empty;

            // Act
            var e = Assert.Throws<ArgumentNullException>(() => new StringCommand(id, description, () => { }));

            // Assert
            Assert.Equal("description", e.ParamName);
        }

        [Fact]
        public void Initialize_InvalidAction_Test()
        {
            // Arrange
            var id = "ID";
            var description = "DESCRIPTION";

            // Act
            var e = Assert.Throws<ArgumentNullException>(() => new StringCommand(id, description, null));

            // Assert
            Assert.Equal("action", e.ParamName);
        }

        [Fact]
        public void Initialize_StringCommand_Test()
        {
            // Arrange
            var commandId = "One";
            var description = "TestCommand One";

            // Act
            var command = new StringCommand(commandId, description, () => { });

            // Assert
            Assert.NotNull(command);
        }

        [Fact]
        public void Initialize_NumericCommand_Test()
        {
            // Arrange
            var commandId = 1;
            var description = "TestCommand One";

            // Act
            var command = new NumericCommand(commandId, description, () => { });

            // Assert
            Assert.NotNull(command);
        }

        [Fact]
        public void Initialize_SystemCommand_Test()
        {
            // Arrange
            var commandId = "Fork";
            var description = "TestCommand One";

            // Act
            var command = new SystemCommand(commandId, description, () => { });

            // Assert
            Assert.NotNull(command);
        }
    }
}
