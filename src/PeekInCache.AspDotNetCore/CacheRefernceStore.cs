using System.Collections.Generic;

namespace PeekInCache.AspDotNetCore
{
    public sealed class CacheRefernceStore
    {
        private static readonly Dictionary<string, object> CacheStore = new Dictionary<string, object>();

        public static void Register(string name, object cache)
        {
            if(!Exists(name.ToUpper()))
                CacheStore.Add(name.ToUpper(), cache);
        }

        public static object GetCache(string name)
        {
            return CacheStore[name.ToUpper()];
        }

        public static bool Exists(string name)
        {
            return CacheStore.ContainsKey(name.ToUpper());
        }
    }
}
