using System;

namespace PeekInCache.AspDotNetCore
{
    public class PeekInCacheAttribute : Attribute
    {
        public string CacheName { get; }

        public PeekInCacheAttribute(string cacheName)
        {
            CacheName = cacheName;
        }
    }
}
