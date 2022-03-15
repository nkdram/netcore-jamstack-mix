namespace Sugcon.Foundation.Caching.Rendering.Providers
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Caching.Distributed;
    using System.Text.Json;
    using Newtonsoft.Json;

    public class DistributedCachingProvider : ICachingProvider
    {
        private readonly IDistributedCache _distributedCache;

        public DistributedCachingProvider(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }

        public async Task<T> GetFromCacheAsync<T>(string key) where T : class
        {
            var cachedValue = await _distributedCache.GetStringAsync(key);
            return cachedValue != null ? JsonConvert.DeserializeObject<T>(cachedValue) : null;
        }

        public T GetFromCache<T>(string key) where T : class
        {
            var cachedValue = _distributedCache.GetString(key);
            return cachedValue != null ? JsonConvert.DeserializeObject<T>(cachedValue) : null;
        }

        public async Task SetCache<T>(string key, T value, DistributedCacheEntryOptions options = null) where T : class
        {
            var serializedValue = JsonConvert.SerializeObject(value);
            options ??= new DistributedCacheEntryOptions()
            { AbsoluteExpiration = DateTimeOffset.Parse(DateTime.Now.AddDays(1).ToString()) };
            await _distributedCache.SetStringAsync(key, serializedValue, options);
        }

        public async Task ClearCache(string key)
        {
            await _distributedCache.RemoveAsync(key);
        }
    }
}
