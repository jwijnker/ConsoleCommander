using ConsoleCommander.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Xunit.Abstractions;

namespace ConsoleCommander.Tests.Models
{
    public class CommandComparerTest : TestBase
    {
        public CommandComparerTest(ITestOutputHelper testOutputHelper) 
            : base(testOutputHelper)
        {
        }

        [Fact]
        public void Compare_EqualCommands_Test()
        {
            // Arrange
            var command1 = new NumericCommand(1, "One", () => { });
            var command2 = new NumericCommand(1, "One", () => { });

            // Act
            var comparer = new CommandComparer();

            // Assert
            var result = comparer.Equals(command1, command2);
            Assert.True(result);
        }

        [Fact]
        public void Compare_NonEqualCommands_DifferInId_Test()
        {
            // Arrange
            var command1 = new NumericCommand(1, "One", () => { });
            var command2 = new NumericCommand(0, "One", () => { });

            // Act
            var comparer = new CommandComparer();

            // Assert
            var result = comparer.Equals(command1, command2);
            Assert.False(result);
        }

        [Fact]
        public void Compare_EqualCommands_DifferInDescription_Test()
        {
            // Arrange
            var command1 = new NumericCommand(2, "One", () => { });
            var command2 = new NumericCommand(2, "_", () => { });

            // Act
            var comparer = new CommandComparer();

            // Assert
            var result = comparer.Equals(command1, command2);
            Assert.True(result);
        }

        [Fact]
        public void GetHashcode_Test()
        {
            // Arrange
            var command = new StringCommand("a", "Test Command A", () => { });

            // Act
            var hashCode = new CommandComparer().GetHashCode(command);

            // Assert
            Assert.Equal(command.Id.GetHashCode(), hashCode);
        }
    }
}
