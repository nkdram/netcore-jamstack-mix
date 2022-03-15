#nullable enable
using GraphQL;
using System;
using System.IO;
using System.Reflection;

namespace Sugcon.Foundation.GrapheNetor.Rendering.Infrastructure
{
    public class GraphQLRequestBuilder
    {

        public GraphQLRequest BuildQuery(string query, string operationName, dynamic? variables)
        {
            return new GraphQLRequest
            {
                Query = query,
                OperationName = operationName,
                Variables = variables
            };
        }

        public GraphQLRequest BuildQuery(GraphQLFiles queryFile, dynamic? variables)
        {
            return BuildQuery(GetOperationResource(queryFile), queryFile.ToString(), variables);
        }


        protected string GetOperationResource(GraphQLFiles queryFile)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = $"{assembly.GetName().Name}.GraphQLQueries.{queryFile}.graphql";
            if (assembly.GetManifestResourceInfo(resourceName) == null)
            {
                throw new Exception($"Unknown GraphQL resource: {resourceName} -- is the file embedded?");
            }
            using var stream = assembly.GetManifestResourceStream(resourceName);
            using var reader = new StreamReader(stream ?? throw new InvalidOperationException($"An error occurred with GraphQL resource {resourceName}"));
            return reader.ReadToEnd();
        }
    }

    [Flags]
    public enum GraphQLFiles
    {
        None = 0,
        LayoutRequest = 1,
        SitePages = 2
    }
}
