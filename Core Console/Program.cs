using ConsoleCommander;
using ConsoleCommander.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;


namespace Core_Console
{
    class Program : IHostedService
    {
        private static IConfigurationRoot configuration;

        public static void Main(string[] args)
        {
            var builder = new HostBuilder()

                .ConfigureHostConfiguration(config => { 
                    // Set the host configuration.
                })
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config.SetBasePath(Directory.GetCurrentDirectory());
                    config.AddJsonFile("appsettings.json", true);
                    if (args != null) config.AddCommandLine(args);
                    
                    configuration = config.Build();
                })
                .ConfigureLogging((hostingContext, logging) =>
                {
                    logging.AddConfiguration(hostingContext.Configuration);
                    logging.AddConsole();
                })
                .ConfigureServices((hostingContext, services) =>
                {
                    // Set the service to be hosted.
                    services.AddHostedService(s => new Program(s));

                    // Set the defaultCommander to use using Provider and register commanders found in given assembly.
                    
                    // Use the defined commander in config
                    services.AddCommanders(new ConfiguredDefaultCommanderProvider(configuration, "defaultCommander"), typeof(Program).Assembly);

                    // You can also define it in code using 'DefinedDefaultCommanderProvider'.
                    //services.AddCommanders(new DefinedDefaultCommanderProvider(typeof(MainCommander)), typeof(Program).Assembly);

                })
                ;

            builder.RunConsoleAsync();
        }

        private IServiceProvider _serviceProvider;

        public Program(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        #region Helpers

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            try
            {
                // 
                var defaultCommanderType = _serviceProvider.GetService<IDefaultCommanderProvider>()
                    .GetCommanderType;
                
                // Run the defined commander
                (_serviceProvider.GetService(defaultCommanderType) as CommanderBase)
                    .Run();

                Console.ResetColor();
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
    }
}
