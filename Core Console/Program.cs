using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Sample_Console.Commanders;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using WT.ConsoleCommander;
using WT.ConsoleCommander.Commanders;

namespace Core_Console
{
    class Program : CommanderBase, IHostedService
    {
        public static void Main(string[] args)
        {
            var builder = new HostBuilder()

                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config.SetBasePath(Directory.GetCurrentDirectory());
                    config.AddJsonFile("appsettings.json", true);
                    if (args != null) config.AddCommandLine(args);
                })
                .ConfigureServices((hostingContext, services) =>
                {
                    // Set the services used.
                    services.AddHostedService(s => new Program(s));

                    services.AddScoped<DiCommander>();
                    services.AddScoped<UnityCommander>();
                    services.AddScoped<CastleWindsorCommander>();
                })
                .ConfigureLogging((hostingContext, logging) =>
                {
                    logging.AddConfiguration(hostingContext.Configuration);
                    logging.AddConsole();
                });

                builder.RunConsoleAsync();
        }
        
        #region Helpers

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            try
            {
                this.Run();
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(e.Message);

                Console.ResetColor();
                Console.ReadLine();
            }
            finally
            {
                await this.StopAsync(cancellationToken);
            }
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("Closing application");
            await Task.Delay(200);
        }

        #endregion

        public Program(IServiceProvider serviceProvider)
        {
            registerCommand("1", "Dependency Injection (using .Net Core)", () => useCommander(serviceProvider.GetService<DiCommander>()));
            registerCommand("2", "Dependency Injection (using Unity)", () => useCommander(serviceProvider.GetService<UnityCommander>()));
            registerCommand("3", "Dependency Injection (using CastleWindsor)", () => useCommander(serviceProvider.GetService<CastleWindsorCommander>()));
        }
       
    }
}
