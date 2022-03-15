using Microsoft.Extensions.DependencyInjection;
using Sugcon.Foundation.Caching.Extension;
using Sugcon.Foundation.Caching.Providers;
using Sitecore.Data.Items;
using Sitecore.DependencyInjection;
using Sitecore.Publishing.Pipelines.PublishItem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sugcon.Foundation.Caching.Events
{
    public class PublishEventHandlerRedisPurge
    {
        protected void OnItemProcessed(object sender, EventArgs args)
        {
            ItemProcessedEventArgs itemProcessedEventArgs = args as ItemProcessedEventArgs;
            PublishItemContext context = itemProcessedEventArgs != null ? itemProcessedEventArgs.Context : null;
            if (context?.VersionToPublish != null)
            {
                Item currentItem = context?.VersionToPublish;
                // Do the cache clear only if the item is type of a page
                if (currentItem.IsPageType()) 
                {
                    string redisKey = currentItem.GenerateCacheKeyFromItem();
                    if (string.IsNullOrEmpty(redisKey)) return;
                    using (var innerScope = ServiceLocator.ServiceProvider
                                  .GetRequiredService<IServiceScopeFactory>().CreateScope())
                    {
                        var redisCache = innerScope.ServiceProvider.GetService<ICachingProvider>();
                        redisCache.RemoveKey(redisKey);
                    }
                }
            }
        }
    }
}