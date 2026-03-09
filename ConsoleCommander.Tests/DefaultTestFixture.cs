using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace ConsoleCommander.Tests
{
    public class DefaultTestFixture
    {
        public IServiceCollection Services { get; }

        private IServiceProvider _serviceProvider;
        public IServiceProvider ServiceProvider
        {
            get
            {

                if (_serviceProvider is null)
                {
                    _serviceProvider = Services.BuildServiceProvider();
                }
                return _serviceProvider;
            }
        }

        public DefaultTestFixture()
        {
            var configurationBuilder = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: true)
                .AddEnvironmentVariables();

            var configuration = configurationBuilder.Build();

            var services = new ServiceCollection();
            services.AddSingleton<IConfiguration>(configuration);

            services.AddLogging();

            Services = services;
        }
    }
}
