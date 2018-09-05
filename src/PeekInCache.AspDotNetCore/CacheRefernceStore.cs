using System.Collections.Generic;

namespace PeekInCache.AspDotNetCore
{
    public sealed class CacheRefernceStore
    {
        public static Dictionary<string, object> CacheStore = new Dictionary<string, object>();


        public static void Register(string name, object cache)
        {
            if(!Exists(name))
                CacheStore.Add(name, cache);
        }

        public static object GetCache(string name)
        {
            return CacheStore[name];
        }

        public static bool Exists(string name)
        {
            return CacheStore.ContainsKey(name);
        }

    }
}
