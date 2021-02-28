using Microsoft.Extensions.DependencyInjection;
using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace ConsoleCommander.Extensions
{
    public static class ServiceCollectionAddCommandersExtensions
    {
        public static IServiceCollection AddCommanders(this IServiceCollection serviceCollection, IDefaultCommanderProvider provider, Assembly assembly)
        {
            return serviceCollection.AddCommanders(provider, new[] { assembly });
        }

        public static IServiceCollection AddCommanders(this IServiceCollection serviceCollection, IDefaultCommanderProvider provider, Assembly[] assemblies)
        {
            foreach (var a in assemblies)
            {
                var types = a.GetTypes()
                    .Where(t => t.IsCommander())
                ;

                serviceCollection.AddCommanders(provider, types.ToArray());
            }

            return serviceCollection;
        }

        public static IServiceCollection AddCommanders(this IServiceCollection serviceCollection, IDefaultCommanderProvider provider, Type[] commanderTypes)
        {
            serviceCollection.AddSingleton(provider);

            foreach (var type in commanderTypes.Where(t => !t.IsAbstract & t.IsCommander()))
            {
                Debug.WriteLine($"Register commander '{type.Name}'.");
                serviceCollection.AddTransient(type, type);
            }

            return serviceCollection;
        }

        public static bool IsCommander(this Type type)
        {
            if (type.BaseType == typeof(CommanderBase))
            {
                return true;
            }

            if (type.BaseType == typeof(object) ||
                type.BaseType == null)
            {
                return false;
            }

            return IsCommander(type.BaseType);
        }

        //public static IServiceCollection SetDefaultCommander(this IServiceCollection serviceCollection, Type defaultCommanderType)
        //{
        //    if (!defaultCommanderType.IsCommander())
        //    {
        //        throw new ApplicationException($"Type '{defaultCommanderType.Name}' is no Commander.");
        //    }

        //    serviceCollection.

        //    return serviceCollection.AddTransient(typeof(IDefaultCommander), defaultCommanderType);
        //}
    }
}
