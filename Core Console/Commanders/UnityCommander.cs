using System.Collections.Generic;
using System.Linq;
using Unity;
using Unity.RegistrationByConvention;

namespace ConsoleCommander.Commanders
{
    public class UnityCommander : CommanderBase<IUnityContainer>
    {
        #region Constructor(s)

        public UnityCommander()
            : this(new UnityContainer())
        {
        }

        public UnityCommander(IUnityContainer unityContainer)
            : base(unityContainer)
        {
            DataProvider.RegisterTypes(
                AllClasses.FromLoadedAssemblies(true, true, true, true),
                WithMappings.FromMatchingInterface,
                WithName.TypeName,
                WithLifetime.ContainerControlled);

            registerCommand("1", "List registrations", list);
            registerCommand("s", "Search", search);
        }

        #endregion

        public void list()
        {
            displayItems(DataProvider.Registrations
                .Where(r => !r.MappedToType.Namespace.StartsWith("System"))
                .Where(r => !r.MappedToType.Namespace.StartsWith("Microsoft"))
                .Where(r => !r.MappedToType.Namespace.StartsWith("Unity"))
                
                );
        }

        public void search()
        {
            var q = this.requestValue("Search for", string.Empty);

            var registrations = DataProvider.Registrations
                .Where(e => e.RegisteredType.Name.ToLower().Contains(q.ToLower()))
                .OrderBy(e => e.RegisteredType.FullName);

            displayItems(registrations);
        }

        private void displayItems(IEnumerable<IContainerRegistration> registrations)
        {
            var l1 = registrations.Max(r => r.RegisteredType.Name.Length) + 1;
            var l2 = registrations.Max(r => r.RegisteredType.Namespace.Length) + 1;
            var l3 = registrations.Max(r => r.MappedToType.Name.Length) + 1;
            var l4 = registrations.Max(r => r.MappedToType.Namespace.Length) + 1;

            this.WriteList(registrations, r => $"{r.RegisteredType.Name.PadRight(l1)} ({r.RegisteredType.Namespace.PadRight(l2)}) => {r.MappedToType.Name.PadRight(l3)} ({r.MappedToType.Namespace.PadRight(l4)}) as '{r.Name}'.");
        }
    }
}
