#nullable enable
using Newtonsoft.Json;
using Sugcon.Foundation.GrapheNetor.Rendering.Param;
using Sugcon.Foundation.GrapheNetor.Rendering.Response;
using Sugcon.Foundation.GrapheNetor.Rendering.Services;
using Sitecore.LayoutService.Client.Response;
using Statiq.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Sugcon.Foundation.GrapheNetor.Rendering.Response.SitePagesResponse;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Sugcon.Foundation.GrapheNetor.Rendering.Helpers;

namespace Sugcon.Foundation.GrapheNetor.Rendering.Modules
{
    public class ProcessPage : Module
    {
        private readonly IGraphQLService<LayoutResponse> _service;
        private readonly IConfiguration _configuration;

        public ProcessPage(IGraphQLService<LayoutResponse> service, IServiceProvider serviceprovider)
        {
            _service = service ?? throw new ArgumentNullException("GraphQlLayoutRequestService", "GraphQlLayoutRequestService must not be null");
            _configuration = (IConfiguration)serviceprovider.GetService(typeof(IConfiguration));
        }

        protected override async Task<IEnumerable<IDocument>> ExecuteContextAsync(IExecutionContext context)
        {
            SitePagesResponse sitePagesResponse = new SitePagesResponse();
            ///Step 1 : Get Site pages
            var ctx = context.Inputs[0].TryGetValue(Constants.Generator_Module_PagesResponseKey, out sitePagesResponse);
            //Step 1.1 :Input should contain key
            if (!ctx)
            {
                context.LogWarning("Input doesn't have Pages that is required to be process - please make sure Input Pages module is run before this module !", this);
                return context.Inputs;
            }
            //Step 1.2 : Convert to simple list for easier looping
            var pages = GetAllPages(sitePagesResponse);

            //Step 2: Create a document list
            var documents = new List<IDocument>();

            //Step 2 : Loop through Site pages            
            var homeItem = pages.Where(x => !string.IsNullOrEmpty(x.homeItemPath)).FirstOrDefault();
            foreach (var page in pages)
            {
                var pagePath = homeItem.homeItemPath.Equals(page.path) ? "/" : page.path.Replace(homeItem.homeItemPath, "");
                ///2.1 : Fetch layoutresponse using GraphQlLayoutRequestService
                var layoutResponse = Task.Run(async () => await _service.Fetch(new ContextParam()
                {
                    Language = page.language.name,
                    RoutePath = pagePath,
                    Site = _configuration.GetConfigurationEntries(Constants.Configuration_GrapheNetor_Pipelines_Site)
                }));

                ///2.2 : Create a keyValue list to map custom object in Idocument
                List<KeyValuePair<string, object>> list = new List<KeyValuePair<string, object>>();
                SitecoreLayoutResponseContent rendered = new Sitecore.LayoutService.Client.Newtonsoft.NewtonsoftLayoutServiceSerializer().Deserialize(layoutResponse.Result?.layout?.item?.rendered.ToString());
                list.Add(new KeyValuePair<string, object>(Constants.Generator_Module_LayoutResponseKey, rendered));

                ///2.3 Set Page path along with IDocument List and content type
                pagePath = pagePath.TrimStart('/').IsNullOrEmpty() ? "Home" : pagePath.TrimStart('/');
                var doc = context.CreateDocument(new NormalizedPath($"{page.language.name}/{pagePath}.html"), list, "", "text/html");
                documents.Add(doc);
            }

            return documents;
        }

        /// <summary>
        /// Gets all pages in a simple list
        /// </summary>
        /// <param name="sitePagesResponse"></param>
        /// <returns></returns>
        private List<Item> GetAllPages(SitePagesResponse sitePagesResponse)
        {
            List<Item> returnList = new List<Item>();
            List<Item> result = new List<Item>();
            result.AddRange(GetAllDescendants(sitePagesResponse?.layout?.item, returnList));
            return result;
        }

        private List<Item> GetAllDescendants(Item item, ICollection<Item> list)
        {
            list.Add(item);
            if (item?.children?.results != null)
                foreach (Item childItem in item?.children?.results)
                {
                    if (childItem != null && childItem?.id != item.id)
                        GetAllDescendants(childItem, list);
                }
            return list.ToList();
        }
    }
}