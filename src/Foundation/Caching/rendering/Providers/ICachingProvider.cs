namespace Sugcon.Foundation.Caching.Rendering.Providers
{
    using System.Threading.Tasks;
    using Microsoft.Extensions.Caching.Distributed;

    public interface ICachingProvider
    {
        Task<T> GetFromCacheAsync<T>(string key) where T : class;
        T GetFromCache<T>(string key) where T : class;
        Task SetCache<T>(string key, T value, DistributedCacheEntryOptions options = null) where T : class;
        Task ClearCache(string key);
    }
}
