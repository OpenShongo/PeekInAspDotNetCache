using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace PeekInCache.AspDotNetCore
{
    public class PeekInAspDotNetCacheMiddleware
    {
        private readonly RequestDelegate _next;

        public PeekInAspDotNetCacheMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var content = RequestPeekInCacheContent(context.Request);
            if (content == null)
                await _next(context);
            else
                await RespondWithContent(context.Response, content);
        }

        private static string RequestPeekInCacheContent(HttpRequest request)
        {
            if (request.Method != "GET")
                return null;

            if (!request.Path.StartsWithSegments(new PathString("/PeekInCache")))
                return null;

            var cacheName = request.Path.Value.Split('/').Last();


            if (!CacheRefernceStore.Exists(cacheName))
                return null;

            var content = JsonConvert.SerializeObject(CacheRefernceStore.GetCache(cacheName));
            return content;
        }


        private static async Task RespondWithContent(HttpResponse response, string content)
        {
            response.StatusCode = 200;
            response.ContentType = "application/json;charset=utf-8";

            await response.WriteAsync(content, new UTF8Encoding(false));
        }

    }
}