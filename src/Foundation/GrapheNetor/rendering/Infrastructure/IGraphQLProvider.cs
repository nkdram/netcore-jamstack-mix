#nullable enable
using GraphQL;
using System.Threading.Tasks;
namespace Sugcon.Foundation.GrapheNetor.Rendering.Infrastructure
{
    public interface IGraphQLProvider
    {
        Task<GraphQLResponse<TResponse>> SendQueryAsync<TResponse>(bool? isEditMode, GraphQLFiles queryFile, dynamic? variables) where TResponse : class;
    }
}
