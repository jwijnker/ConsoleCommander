using ConsoleCommander.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Xunit.Abstractions;

namespace ConsoleCommander.Tests.Models
{
    public class CommandExtensionsTest : TestBase
    {
        public CommandExtensionsTest(ITestOutputHelper testOutputHelper) 
            : base(testOutputHelper)
        {
        }

        [Fact]
        public void Command_IsNummeric_Test()
        {
            // Arrange
            var command = new NumericCommand(0, "Test Command 0", () => { });

            // Act
            var isNumeric = command.IsNumericCommand();

            // Assert
            Assert.True(isNumeric);
        }

        [Fact]
        public void Command_IsNotNummeric_Test()
        {
            // Arrange
            var command = new StringCommand("A", "Test Command A", () => { });

            // Act
            var isNumeric = command.IsNumericCommand();

            // Assert
            Assert.False(isNumeric);
        }

        [Fact]
        public void CommandId_IsNummeric_Test()
        {
            // Arrange
            var commandId = "2";

            // Act
            var isNumeric = CommandExtensions.IsNumericCommand(commandId);

            // Assert
            Assert.True(isNumeric);
        }

        [Fact]
        public void CommandId_IsNotNummeric_Test()
        {
            // Arrange
            var commandId = "Two";

            // Act
            var isNumeric = CommandExtensions.IsNumericCommand(commandId);

            // Assert
            Assert.False(isNumeric);
        }
    }
}
