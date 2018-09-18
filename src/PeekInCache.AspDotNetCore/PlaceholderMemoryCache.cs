using System;
using System.Collections.Generic;
using System.Text;

namespace PeekInCache.AspDotNetCore
{
    public class PlaceholderMemoryCache
    {
        public string Name { get; }

        public PlaceholderMemoryCache(string name)
        {
            Name = name;
        }
    }
}
