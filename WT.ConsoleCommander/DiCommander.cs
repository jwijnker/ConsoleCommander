using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Text;

namespace WT.ConsoleCommander
{
    public class DiCommander : CommanderBase<IServiceProvider>
    {
        #region Properties

        #endregion

        #region Constructor(s)

        public DiCommander(IServiceProvider serviceProvider)
            : base(serviceProvider)
        {
            registerCommand("1", "Add registations on ApplicationLifetime", sampleAction);
        }

        #endregion

        private void sampleAction()
        {
            var hal = DataProvider.GetService<IHostApplicationLifetime>();

            // Add registration
            hal.ApplicationStopping.Register(() => {
                this.Warning("Application is stopping...");
            });

            // Add registration
            hal.ApplicationStopped.Register(() => {
                this.Info("Application is stopped.");
            });
        }
    }
}
