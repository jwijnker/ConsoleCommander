using ConsoleCommander;
using Microsoft.Extensions.DependencyInjection;
using Sample_Console.Commanders;
using System;

namespace Core_Console
{
    public class MainCommander : CommanderBase<IServiceProvider>
    {
        public MainCommander(IServiceProvider serviceProvider)
            : base(serviceProvider)
        {
            registerCommand("0", "Simple 'Hello World' sample", hello);

            registerCommand("1", "Dependency Injection (using .Net Core)", () => useCommander(serviceProvider.GetService<DiCommander>()));
            registerCommand("2", "Dependency Injection (using Unity)", () => useCommander(serviceProvider.GetService<UnityCommander>()));
            registerCommand("3", "Dependency Injection (using CastleWindsor)", () => useCommander(serviceProvider.GetService<CastleWindsorCommander>()));
            registerCommand("4", "Filesystem commander", () => useCommander(serviceProvider.GetService<FilesystemCommander>()));
        }

        private void hello()
        {
            var name = this.requestValue("Name", "World");

            this.WriteLine($"Hello {name}");
        }
    }
}
