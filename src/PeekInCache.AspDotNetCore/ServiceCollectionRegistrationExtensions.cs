using System;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace PeekInCache.AspDotNetCore
{
    public static class ServiceCollectionRegistrationExtensions
    {
        public static IServiceCollection PeekInAClass(
            this IServiceCollection service, Type classType)
        {
            var classFields = classType.GetRuntimeFields()
                .Where(x => x.GetCustomAttribute<PeekInCacheAttribute>() != null).ToArray();

            foreach (var field in classFields)
            {
                var attribute = (PeekInCacheAttribute)field.GetCustomAttribute(typeof(PeekInCacheAttribute));
                var classCache = field.GetValue(null);
                CacheRefernceStore.Register(attribute.CacheName, classCache);
            }
            return service;
        }

        public static IServiceCollection PeekInAllClasses(
            this IServiceCollection service)
        {
            var currentAssembly = Assembly.GetEntryAssembly();
            var classes = currentAssembly.GetTypes()
                .Where(x => x.IsClass &&
                            x.GetRuntimeFields().Any(y => y.GetCustomAttribute<PeekInCacheAttribute>() != null));

            ServiceCollectionRefernce.RegisterServiceCollection(service);

            foreach (var @class in classes)
            {
                var classFields = @class.GetRuntimeFields()
                    .Where(x => x.GetCustomAttribute<PeekInCacheAttribute>() != null)
                    .ToArray();

                foreach (var field in classFields)
                {
                    if (field.IsStatic)
                    {
                        RegisterStaticCache(field);
                        continue;
                    }

                    if (IsAspNetCoreDefaultMemoryCache(field))
                        RegisterAspNetCoreDefaultMemoryCache(field);
                }
            }
            return service;
        }

        private static void RegisterAspNetCoreDefaultMemoryCache(FieldInfo field)
        {
            var attribute = (PeekInCacheAttribute)field.GetCustomAttribute(typeof(PeekInCacheAttribute));
            var classCache = new PlaceholderMemoryCache(attribute.CacheName);
            CacheRefernceStore.Register(attribute.CacheName, classCache);
        }

        private static bool IsAspNetCoreDefaultMemoryCache(FieldInfo field)
        {
            return field.FieldType.Name.Equals("IMemoryCache");
        }

        private static void RegisterStaticCache(FieldInfo field)
        {
            var attribute = (PeekInCacheAttribute) field.GetCustomAttribute(typeof(PeekInCacheAttribute));
            var classCache = field.GetValue(null);
            CacheRefernceStore.Register(attribute.CacheName, classCache);
        }
    }
}
