using Microsoft.Extensions.Configuration;
using GraphQL;
using Sugcon.Foundation.GrapheNetor.Rendering.Response;
using Sugcon.Foundation.GrapheNetor.Rendering.Infrastructure;
using Sugcon.Foundation.GrapheNetor.Rendering.Param;
using System.Threading.Tasks;
using Sugcon.Foundation.Caching.Rendering.Providers;
using Sugcon.Foundation.GrapheNetor.Rendering.Extension;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Sugcon.Foundation.GrapheNetor.Rendering.Services
{
    public class GraphQlLayoutRequestService : IGraphQLService<LayoutResponse>
    {
        private readonly ICachingProvider _cachingProvider;
        private readonly IConfiguration _configuration;
        private readonly IGraphQLProvider _graphQLProvider;
        private readonly ILogger<GraphQlLayoutRequestService> _logger;

        public GraphQlLayoutRequestService(ICachingProvider cachingProvider, IConfiguration configuration, IGraphQLProvider graphQLProvider, ILogger<GraphQlLayoutRequestService> logger)
        {
            _cachingProvider = cachingProvider;
            _configuration = configuration;
            _graphQLProvider = graphQLProvider;
            _logger = logger;
        }

        public async Task<LayoutResponse> Fetch(ContextParam contextParam)
        {
            var cacheKeyForContextParam = contextParam.GenerateCacheKeyForContextParam();
            LayoutResponse response =
                _cachingProvider.GetFromCache<LayoutResponse>(cacheKeyForContextParam);
            _logger.Log(LogLevel.Information, JsonConvert.SerializeObject(response, Formatting.Indented));
            if (response != null && response?.layout != null && response?.layout?.item?.rendered != null)
                return response;

            response = (await _graphQLProvider.SendQueryAsync<LayoutResponse>(false, GraphQLFiles.LayoutRequest, new
            {
                language = contextParam.Language,
                routePath = contextParam.RoutePath,
                site = contextParam.Site
            })).Data;

            await _cachingProvider.SetCache(cacheKeyForContextParam, response);

            return response;
        }
    }
}
