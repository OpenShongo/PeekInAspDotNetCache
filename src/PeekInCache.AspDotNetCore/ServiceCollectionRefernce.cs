using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace PeekInCache.AspDotNetCore
{
    public sealed class ServiceCollectionRefernce
    {
        private static IServiceProvider _serviceProvider;
        private static IServiceCollection _serviceCollection;

        public static void RegisterServiceCollection(IServiceCollection serviceCollection)
        {
            _serviceCollection = serviceCollection;
        }


        public static void RegisterServiceProvider(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public static object GetAspDotNetDefaultMemoryCache()
        {
            if (_serviceCollection == null || _serviceProvider == null)
                return null;

            var items = from item in _serviceCollection
                where item.ServiceType.Name.Equals("IMemoryCache")
                select _serviceProvider.GetService(item.ServiceType);
            return items.FirstOrDefault();
        }

    }
}