using Microsoft.Extensions.DependencyInjection;
using Sugcon.Foundation.Caching.Helpers;
using Sugcon.Foundation.Caching.Models;
using Sugcon.Foundation.Caching.Providers;
using ServiceStack.Redis;
using Sitecore.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace Sugcon.Foundation.Caching
{
    public class ServicesConfigurator : IServicesConfigurator
    {
        public void Configure(IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IRedisClientsManager>(c => new RedisManagerPool(ConfigurationHelper.GetSetting(Constants.REDIS_CONNECTION)));
            serviceCollection.AddScoped<IRedisConnectionProvider, RedisCachingProvider>();
            serviceCollection.AddScoped<ICachingProvider, DistributedCachingProvider>();
        }
    }
}