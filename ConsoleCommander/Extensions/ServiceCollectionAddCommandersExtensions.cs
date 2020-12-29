using ConsoleCommander;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Reflection;

namespace ConsoleCommander.Extensions
//namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionAddCommandersExtensions
    {
        //public static IServiceCollection AddCommanders(this IServiceCollection serviceCollection)
        //{
        //    return AddCommanders(serviceCollection, typeof(DependencyInjection).Assembly);
        //}

        public static IServiceCollection AddCommanders(this IServiceCollection serviceCollection, Assembly assembly)
        {
            return AddCommanders(serviceCollection, new[] { assembly });
        }

        public static IServiceCollection AddCommanders(this IServiceCollection serviceCollection, Assembly[] assemblies)
        {
            foreach (var a in assemblies)
            {
                var types = a.GetTypes()
                    .Where(e => e.DeclaringType != null)
                    .Where(x => x.IsClass && (x.BaseType.IsAssignableFrom(typeof(CommanderBase)) | x.BaseType.IsAssignableFrom(typeof(CommanderBase<object>))))
                   .OrderBy(e => e.FullName)
                   .Select(e => e.DeclaringType)
                    .Distinct()
               ;

                foreach (var type in types)
                {
                    serviceCollection.AddTransient(type, type);
                }
            }

            return serviceCollection;
        }
    }
}
