using GraphQL.Client.Abstractions;
using GraphQL.Client.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Net.Http;
using GraphQL.Client.Serializer.Newtonsoft;
using Newtonsoft.Json;

namespace Sugcon.Foundation.GrapheNetor.Rendering.Infrastructure
{
    public class GraphQLClientFactory
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;
        public GraphQLClientFactory(IConfiguration configuration, HttpClient httpClient)
        {
            _configuration = configuration;
            _httpClient = httpClient;
        }

        private IGraphQLClient LiveClient { get; set; }

        private IGraphQLClient EditClient { get; set; }


        public IGraphQLClient CreateLiveClient()
        {
            return LiveClient ??= CreateGraphQLClient("Foundation:GrapheNetor:GraphQL:UrlLive");
        }

        public IGraphQLClient CreateEditClient()
        {
            return EditClient ??= CreateGraphQLClient("Foundation:GrapheNetor:GraphQL:UrlEdit");
        }

        private IGraphQLClient CreateGraphQLClient(string configurationKeyUrlLiveOrEditMode)
        {
            GraphQLHttpClientOptions graphQLHttpClientOptions = new GraphQLHttpClientOptions()
            {
                EndPoint = new Uri(
                    $"{_configuration.GetValue<string>(configurationKeyUrlLiveOrEditMode)}?sc_apikey={_configuration.GetValue<string>("Sitecore:ApiKey")}"),
            };
            var options = new JsonSerializerSettings();
            options.StringEscapeHandling = StringEscapeHandling.Default;
            options.MetadataPropertyHandling = MetadataPropertyHandling.Ignore;
            options.MissingMemberHandling = MissingMemberHandling.Ignore;
            options.ContractResolver = new Newtonsoft.Json.Serialization.DefaultContractResolver()
            {
                NamingStrategy = new Newtonsoft.Json.Serialization.CamelCaseNamingStrategy()
            };
            return new GraphQLHttpClient(graphQLHttpClientOptions, new NewtonsoftJsonSerializer(options), _httpClient);
        }


    }
}
