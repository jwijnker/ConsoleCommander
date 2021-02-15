using Microsoft.Extensions.DependencyInjection;
using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace ConsoleCommander.Extensions
{
    public static class ServiceCollectionAddCommandersExtensions
    {
        public static IServiceCollection AddCommanders(this IServiceCollection serviceCollection, Assembly assembly)
        {
            return AddCommanders(serviceCollection, new[] { assembly });
        }

        public static IServiceCollection AddCommanders(this IServiceCollection serviceCollection, Assembly[] assemblies)
        {
            foreach (var a in assemblies)
            {
                var types = a.GetTypes()
                    .Where(t => t.IsCommander())
                ;

                serviceCollection.AddCommanders(types.ToArray());
            }

            return serviceCollection;
        }

        public static IServiceCollection AddCommanders(this IServiceCollection serviceCollection, Type[] commanderTypes)
        {
            foreach (var type in commanderTypes.Where(t => t.IsCommander()))
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

            if (type.BaseType == typeof(object))
            {
                return false;
            }

            return IsCommander(type.BaseType);
        }
    }
}
