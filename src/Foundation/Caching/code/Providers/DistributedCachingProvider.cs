using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sugcon.Foundation.Caching.Providers
{
    public class DistributedCachingProvider : ICachingProvider
    {
        private readonly IRedisConnectionProvider _redisConnectionProvider;
        public DistributedCachingProvider(IRedisConnectionProvider redisConnection)
        {
            _redisConnectionProvider = redisConnection;
        }
        public bool ContainsKey(string key)
        {
            using (var client = _redisConnectionProvider.GetRedisClient())
            {
                return client.ContainsKey(key);
            }
        }

        public bool RemoveKey(string key)
        {
            using (var client = _redisConnectionProvider.GetRedisClient())
            {
                return client.Remove(key);
            }
        }
    }
}
