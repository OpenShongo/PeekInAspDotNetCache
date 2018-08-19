using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PeekInAspDotNetCache
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
