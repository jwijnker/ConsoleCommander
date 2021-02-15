using ConsoleCommander;
using System;

namespace Core_Console
{
    public class MainCommander : CommanderBase<IServiceProvider>
    {
        #region Constructor(s)

        public MainCommander(IServiceProvider serviceProvider)
            : base(serviceProvider)
        {
            registerCommand(0, "Try the samples", () => this.useCommander<SamplesCommander>(serviceProvider));
        }

        #endregion
    }
}
