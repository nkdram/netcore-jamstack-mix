using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Sugcon.Foundation.GrapheNetor.Rendering.Helpers;
using Sugcon.Foundation.GrapheNetor.Rendering.Modules;
using Sugcon.Foundation.GrapheNetor.Rendering.Param;
using Sugcon.Foundation.GrapheNetor.Rendering.Response;
using Sugcon.Foundation.GrapheNetor.Rendering.Services;
using Statiq.Common;
using Statiq.Core;
using System;

namespace Sugcon.Foundation.GrapheNetor.Rendering.Pipelines
{
    /// <summary>
    /// Pipeline that generates pages
    /// </summary>
    public class PagesPipeline : Pipeline, IPipeline
    {
        /// <summary>
        /// Sitecore Pages Pipeline
        /// </summary>
        /// <param name="serviceprovider"></param>
        /// <param name="viewRenderer"></param>
        public PagesPipeline(IServiceProvider serviceprovider, ViewRenderer viewRenderer)
        {
            GraphQlSitePageService sitePageService = (GraphQlSitePageService)serviceprovider.GetService(typeof(IGraphQLService<SitePagesResponse>));
            GraphQlLayoutRequestService layoutService = (GraphQlLayoutRequestService)serviceprovider.GetService(typeof(IGraphQLService<LayoutResponse>));
            IConfiguration configuration = (IConfiguration)serviceprovider.GetService(typeof(IConfiguration));            
           
            InputModules = new Statiq.Common.ModuleList
            {
                //step 1: Query all pages - using GraphQlSitePageService
                new InputPages(sitePageService, new ContextParam(){
                    Language = configuration.GetConfigurationEntries(Constants.Configuration_GrapheNetor_Pipelines_Language) ,
                    RoutePath = configuration.GetConfigurationEntries(Constants.Configuration_GrapheNetor_Pipelines_Path),
                    Site = configuration.GetConfigurationEntries(Constants.Configuration_GrapheNetor_Pipelines_Site)
                }),
                //step 2: Get Layout Service - Store it in a location<Redis>
                new ProcessPage(layoutService,serviceprovider)
            };

            ProcessModules = new Statiq.Common.ModuleList
            {
                //step 3: RenderRazon converts Sitecore Route Data to Html
                new Modules.RenderRazor(viewRenderer),
                //step 4: Finds Sitecore Images in html and replaces it accoring to OutputPath
                new ImagesModule(),
                //step 5: Downloads Sitecore images
                new SitecoreDownloadImages()
            };

            OutputModules = new Statiq.Common.ModuleList
            {
                //step 6: Writes Html & Images to output directory
                new WriteFiles()
            };
        }
    }
}