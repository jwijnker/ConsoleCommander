using ConsoleCommander;

namespace MinimalSetupDemo
{
    internal static class Program
    {
        static void Main(string[] args)
        {
            // In main, just instantiate a commander and run it.
            new MinimalSetupCommander()
                .Run();
        }
    }

    public class MinimalSetupCommander : CommanderBase
    {
        public MinimalSetupCommander()
        {
            registerCommand(0, "Demo minimal setup", demo);
        }

        private void demo()
        {
            this.WriteLine("Completed");
        }
    }
}
