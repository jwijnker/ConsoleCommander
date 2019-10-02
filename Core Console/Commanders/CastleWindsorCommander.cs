using Castle.MicroKernel.Registration;
using Castle.Windsor;
using System;
using WT.ConsoleCommander;

namespace Sample_Console.Commanders
{
    public class CastleWindsorCommander : CommanderBase<IWindsorContainer>
    {
        #region Constructor(s)

        public CastleWindsorCommander()
            : this(new WindsorContainer())
        {
            this.OnClose += CastleWindsorCommander_OnClose;
        }

        public CastleWindsorCommander(IWindsorContainer container)
            : base(container)
        {
            // Skip installers, register directly
            DataProvider.Register(
                 //Castle.MicroKernel
                 Classes.FromAssemblyNamed(this.GetType().Assembly.FullName)
                           .Where(type => type.Name != null)
                           .WithService.AllInterfaces(),
                 Classes.FromAssemblyNamed("Castle.Core")
                           .Where(type => type.Name != null)
                           .WithService.AllInterfaces(),
                 Classes.FromAssemblyNamed("Castle.Windsor")
                           .Where(type => type.Name != null)
                           .WithService.AllInterfaces()

            );

            registerCommand("1", "List registrations", list);
        }

        #endregion

        #region Events and Handlers

        private void CastleWindsorCommander_OnClose(object sender, EventArgs e)
        {
            // clean up, application exits
            DataProvider.Dispose();
        }

        #endregion

        protected void list()
        {
            var items = DataProvider.Kernel.GetAssignableHandlers(typeof(object));

            this.WriteList(items, i => $"{i.ComponentModel.Name} {i.ComponentModel.Implementation}");
        }
    }
}
