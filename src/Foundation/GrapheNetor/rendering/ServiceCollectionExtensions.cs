using Microsoft.Extensions.DependencyInjection;
using Sugcon.Foundation.GrapheNetor.Rendering.Infrastructure;
using Sugcon.Foundation.GrapheNetor.Rendering.Response;
using Sugcon.Foundation.GrapheNetor.Rendering.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sugcon.Foundation.GrapheNetor.Rendering
{
    public static class ServiceCollectionExtensions
    {
        public static void AddFoundationGrapheNetor(this IServiceCollection serviceCollection)
        {
            //Register builder / factory / provider
            serviceCollection.AddSingleton<GraphQLRequestBuilder>();
            serviceCollection.AddHttpClient<GraphQLClientFactory>();
            serviceCollection.AddSingleton<IGraphQLProvider, GraphQLProvider>();

            //Register custom services
            serviceCollection.AddSingleton<IGraphQLService<SitePagesResponse>, GraphQlSitePageService>();
            serviceCollection.AddSingleton<IGraphQLService<LayoutResponse>, GraphQlLayoutRequestService>();
            serviceCollection.AddScoped<ViewRenderer, ViewRenderer>();
        }
    }
}
