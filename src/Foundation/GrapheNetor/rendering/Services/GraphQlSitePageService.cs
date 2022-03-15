using GraphQL;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Sugcon.Foundation.GrapheNetor.Rendering.Infrastructure;
using Sugcon.Foundation.GrapheNetor.Rendering.Param;
using Sugcon.Foundation.GrapheNetor.Rendering.Response;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Sugcon.Foundation.Caching.Rendering.Providers;
using Sugcon.Foundation.GrapheNetor.Rendering.Extension;

namespace Sugcon.Foundation.GrapheNetor.Rendering.Services
{
    public class GraphQlSitePageService : IGraphQLService<SitePagesResponse>
    {
        private readonly ICachingProvider _cachingProvider;
        private readonly IConfiguration _configuration;
        private readonly IGraphQLProvider _graphQLProvider;

        public GraphQlSitePageService(ICachingProvider cachingProvider, IConfiguration configuration, IGraphQLProvider graphQLProvider)
        {
            _cachingProvider = cachingProvider;
            _configuration = configuration;
            _graphQLProvider = graphQLProvider;
        }

        public async Task<SitePagesResponse> Fetch(ContextParam contextParam)
        {
            GraphQLResponse<SitePagesResponse> response = await _graphQLProvider.SendQueryAsync<SitePagesResponse>(false, GraphQLFiles.SitePages, new
            {
                language = contextParam.Language,
                routePath = contextParam.RoutePath,
                site = contextParam.Site
            });
            return response?.Data;
        }
    }
}
