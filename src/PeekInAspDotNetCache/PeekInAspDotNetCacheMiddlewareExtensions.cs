using System;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace PeekInAspDotNetCache
{
    public static class PeekInAspDotNetCacheMiddlewareExtensions
    {
        public static IApplicationBuilder UsePeekInCache(
            this IApplicationBuilder app)
        {
            return app.UseMiddleware<PeekInAspDotNetCacheMiddleware>();
        }

        public static IServiceCollection ForClassPeekAtCache(
            this IServiceCollection service, Type classType)
        {
            var cacheWatcher = new CacheRefernceStore();

            var classFields = classType.GetRuntimeFields().Where(x => x.GetCustomAttribute<PeekInCacheAttribute>() != null).ToArray();

            foreach (var field in classFields)
            {
                var attribute = (PeekInCacheAttribute)field.GetCustomAttribute(typeof(PeekInCacheAttribute));
                var classCache = field.GetValue(null);
                cacheWatcher.Register(attribute.CacheName, classCache);
            }

            return service;
        }
    }
}
