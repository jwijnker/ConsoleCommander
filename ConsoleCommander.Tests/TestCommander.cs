namespace ConsoleCommander.Tests
{
    internal class TestCommander : CommanderBase
    {
        public TestCommander()
        {
            registerCommand(1, "Test Command 1", () => { });
            registerCommand("Two", "Test Command Two", () => { });
        }
    }
}
