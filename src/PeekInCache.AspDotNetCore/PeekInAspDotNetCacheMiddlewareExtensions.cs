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
            ServiceCollectionRefernce.RegisterServiceProvider(app.ApplicationServices);
            return app.UseMiddleware<PeekInAspDotNetCacheMiddleware>();
        }
    }
}
