using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sugcon.Foundation.Caching.Providers
{
    public class RedisCachingProvider : IRedisConnectionProvider
    {
        private readonly IRedisClientsManager _redisConnection;
        public RedisCachingProvider(IRedisClientsManager redisManager)
        {
            _redisConnection = redisManager;
        }

        public IRedisClient GetRedisClient()
        {
            return _redisConnection.GetClient();
        }
    }
}
