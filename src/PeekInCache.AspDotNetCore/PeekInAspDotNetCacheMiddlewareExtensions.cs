using System;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace PeekInCache.AspDotNetCore
{
    public static class PeekInAspDotNetCacheMiddlewareExtensions
    {
        public static IApplicationBuilder UsePeekInCache(
            this IApplicationBuilder app)
        {
            return app.UseMiddleware<PeekInAspDotNetCacheMiddleware>();
        }

        public static IServiceCollection PeekInAClass(
            this IServiceCollection service, Type classType)
        {
            var classFields = classType.GetRuntimeFields().Where(x => x.GetCustomAttribute<PeekInCacheAttribute>() != null).ToArray();

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


            foreach (var @class in classes)
            {
                var classFields = @class.GetRuntimeFields().Where(x => x.GetCustomAttribute<PeekInCacheAttribute>() != null).ToArray();

                foreach (var field in classFields)
                {
                    var attribute = (PeekInCacheAttribute)field.GetCustomAttribute(typeof(PeekInCacheAttribute));
                    var classCache = field.GetValue(null);
                    CacheRefernceStore.Register(attribute.CacheName, classCache);
                }
            }
            return service;
        }
    }
}
