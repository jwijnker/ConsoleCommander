using ConsoleCommander.Exceptions;
using ConsoleCommander.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Xunit.Abstractions;

namespace ConsoleCommander.Tests.Models
{
    public class CommandsContainerTest : TestBase
    {
        public CommandsContainerTest(ITestOutputHelper testOutputHelper) 
            : base(testOutputHelper)
        {
        }

        [Fact]
        public void CommandsContainer_Initialize_Test()
        {
            // Arrange

            // Act
            var container = new CommandsContainer();

            // Assert
            Assert.NotNull(container);
        }

        [Fact]
        public void CommandsContainer_AddStringCommand_Test()
        {
            // Arrange
            var container = new CommandsContainer();
            var command = new StringCommand("One", "TestCommand One", () => { });

            // Act
            container.Add(command);

            // Assert
            Assert.Contains(command, container.AllCommands);
            Assert.Contains(command, container.StringCommands);
            Assert.DoesNotContain(command as Command, container.NumericCommands);
            Assert.DoesNotContain(command as Command, container.SystemCommands);
        }

        [Fact]
        public void CommandsContainer_AddNumericCommand_Test()
        {
            // Arrange
            var container = new CommandsContainer();
            var command = new NumericCommand(1, "TestCommand One", () => { });

            // Act
            container.Add(command);

            // Assert
            Assert.Contains(command, container.AllCommands);
            Assert.DoesNotContain(command as Command, container.StringCommands);
            Assert.Contains(command, container.NumericCommands);
            Assert.DoesNotContain(command as Command, container.SystemCommands);
        }

        [Fact]
        public void CommandsContainer_AddSystemCommand_Test()
        {
            // Arrange
            var container = new CommandsContainer();
            var command = new SystemCommand("Join", "TestCommand One", () => { });

            // Act
            container.Add(command);

            // Assert
            Assert.Contains(command, container.AllCommands);
            Assert.DoesNotContain(command as Command, container.StringCommands);
            Assert.DoesNotContain(command as Command, container.NumericCommands);
            Assert.Contains(command as Command, container.SystemCommands);
        }

        [Fact]
        public void CommandsContainer_CanRegister_Test()
        {
            // Arrange
            var container = new CommandsContainer();
            var command = new NumericCommand(1, "Test Command 1", () => { });

            // Act
            var canRegister = container.CanRegister(command);

            // Assert
            Assert.True(canRegister);
        }

        [Fact]
        public void CommandsContainer_CanNotRegister_Test()
        {
            // Arrange
            var container = new CommandsContainer();
            var command = new StringCommand("One", "Test Command 1", () => { });

            // Act
            var canRegister1 = container.CanRegister(command);
            container.Add(command);

            var canRegister2 = container.CanRegister(command);

            // Assert
            Assert.True(canRegister1);
            Assert.False(canRegister2);
        }

        [Fact]
        public void CommandsContainer_GetCommand_Test()
        {
            // Arrange
            var container = new CommandsContainer();

            container.Add(new StringCommand("a", "TestCommand A", () => { }));
            container.Add(new NumericCommand(1, "TestCommand 1", () => { }));

            // Act
            var commandA = container.GetCommand("a");
            var command1 = container.GetCommand(1.ToString());

            // Assert
            Assert.NotNull(commandA);
            Assert.NotNull(command1);
        }

        [Fact]
        public void CommandsContainer_GetCommandThrowsException_Test()
        {
            // Arrange
            var container = new CommandsContainer();
            var commandId = "aNonRegisteredCommand";

            // Act
            var e = Assert.Throws<CommandNotRegisteredException>(() => container.GetCommand(commandId));

            // Assert
            Assert.Equal(commandId, e.CommandId);
        }
    }
}
