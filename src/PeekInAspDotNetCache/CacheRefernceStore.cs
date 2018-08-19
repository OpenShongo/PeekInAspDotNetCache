using System.Collections.Generic;

namespace PeekInAspDotNetCache
{
    public class CacheRefernceStore
    {
        public static Dictionary<string, object> CacheStore = new Dictionary<string, object>();


        public void Register(string name, object cache)
        {
            if(!Exists(name))
                CacheStore.Add(name, cache);
        }

        public object GetCache(string name)
        {
            return CacheStore[name];
        }

        public bool Exists(string name)
        {
            return CacheStore.ContainsKey(name);
        }

    }
}
