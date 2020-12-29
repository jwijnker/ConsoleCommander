using ConsoleCommander;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sample_Console.Commanders
{
    public class DiCommander : CommanderBase<IServiceProvider>
    {
        #region Properties

        #endregion

        #region Constructor(s)

        public DiCommander(IServiceProvider serviceProvider)
            : base(serviceProvider)
        {
            registerCommand("2", "Inform about ApplicaitonState.", informAboutAplicationState);
            registerCommand("1", "Add registations on ApplicationLifetime", registerCommanders);
        }

        #endregion

        private void registerCommanders()
        {
            var assemblies = new[] { this.GetType().Assembly };

            foreach (var a in assemblies)
            {
                var types = a.GetTypes()
                    .Where(e => e.DeclaringType != null)
                    .Where(x => x.IsClass && (x.BaseType.IsAssignableFrom(typeof(CommanderBase)) | x.BaseType.IsAssignableFrom(typeof(CommanderBase<object>))))
                    .OrderBy(e => e.FullName)
                    .Select(e => e.DeclaringType)
                    .Distinct()
                ;

                this.WriteAsTable(types, new Dictionary<string, Func<Type, object>>() {
                    { "Name", t => t.FullName },
                    { "DeclaringType", t => t.DeclaringType?.FullName },
                    { "BaseType", t => t.BaseType.Name },
                    { "Interfaces", t => string.Join(",", t.GetInterfaces().Select(i => i.Name)) },
                    { "Assg.From Base", t => t.IsAssignableFrom(typeof(CommanderBase)) },
                    { "Assg.From GenBase", t => t.IsAssignableFrom(typeof(CommanderBase<>)) }
                });
            }
        }


        private void informAboutAplicationState()
        {
            var hal = DataProvider.GetService<IHostApplicationLifetime>();

            // Add registration
            hal.ApplicationStopping.Register(() =>
            {
                this.Warning("Application is stopping...");
            });

            // Add registration
            hal.ApplicationStopped.Register(() =>
            {
                this.Info("Application is stopped.");
            });
        }
    }
}
