namespace Sugcon.Foundation.Caching.Rendering
{
    using Microsoft.Extensions.Caching.Distributed;
    using Microsoft.Extensions.Caching.StackExchangeRedis;
    using Microsoft.Extensions.DependencyInjection;
    using Providers;

    public static class ServiceCollectionExtensions
    {
        public static void AddCachingServices(this IServiceCollection services)
        {
            services.AddSingleton<IDistributedCache, RedisCache>();
            services.AddSingleton<ICachingProvider, DistributedCachingProvider>();
        }
    }
}
