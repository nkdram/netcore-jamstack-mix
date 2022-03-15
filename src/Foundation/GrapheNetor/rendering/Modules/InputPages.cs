using Sugcon.Foundation.GrapheNetor.Rendering.Param;
using Sugcon.Foundation.GrapheNetor.Rendering.Response;
using Sugcon.Foundation.GrapheNetor.Rendering.Services;
using Statiq.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Sugcon.Foundation.GrapheNetor.Rendering.Modules
{
    /// <summary>
    /// Module ::: InputPages fetches all pages to be processed
    /// </summary>
    public class InputPages : Module
    {
        private readonly IGraphQLService<SitePagesResponse> _service;
        private readonly ContextParam _contextParam;

        public InputPages(IGraphQLService<SitePagesResponse> service, ContextParam contextParam)
        {
            _service = service ?? throw new ArgumentNullException("GraphQLService", "GraphQLService must not be null");
            _contextParam = contextParam ?? throw new ArgumentNullException("ContextParam", "ContextParam must not be null");
        }

        protected override async Task<IEnumerable<IDocument>> ExecuteContextAsync(IExecutionContext context)
        {
            var responseData = await _service.Fetch(_contextParam);
            //create a custom list with object type mapping
            List<KeyValuePair<string, object>> list = new List<KeyValuePair<string, object>>
            {
                new KeyValuePair<string, object>(Constants.Generator_Module_PagesResponseKey, responseData)
            };
            var document = context.CreateDocument(list);
            return document.Yield();
        }
    }
}
