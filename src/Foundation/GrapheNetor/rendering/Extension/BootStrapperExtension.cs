using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NetlifySharp;
using Sugcon.Foundation.Caching.Rendering;
using Sugcon.Foundation.GrapheNetor.Rendering.Helpers;
using Sugcon.Foundation.GrapheNetor.Rendering.Pipelines;
using Sugcon.Foundation.GrapheNetor.Rendering.Services;
using Statiq.App;
using Statiq.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sugcon.Foundation.GrapheNetor.Rendering.Extension
{
    public static class BootStrapperExtension
    {
        public static IBootstrapper ConfigureSitecorePipelines(this IBootstrapper bootstrapper, ViewRenderer viewRenderer, IServiceProvider serviceprovider)
        {
            bootstrapper.AddInputPath(new NormalizedPath("views"));
            bootstrapper.AddInputPath(new NormalizedPath("wwwroot"));
            bootstrapper.AddPipeline(Constants.CopyAsset_Pipeline, new CopyAssetsPipeline());
            return bootstrapper.AddPipeline(Constants.Page_Pipeline, new PagesPipeline(serviceprovider, viewRenderer));
        }

        public static void NetlifyDeploy(this IBootstrapper bootstrapper, IServiceProvider serviceprovider)
        {
            IConfiguration configuration = (IConfiguration)serviceprovider.GetService(typeof(IConfiguration));
            new NetlifyClient(
                         configuration.GetConfigurationEntries(Constants.Configuration_GrapheNetor_Netlify_Token),
                         new System.Net.Http.HttpClient())
               .UpdateSiteAsync(
                         configuration.GetConfigurationEntries(Constants.Configuration_GrapheNetor_Netlify_SourcePath)
                        , configuration.GetConfigurationEntries(Constants.Configuration_GrapheNetor_Netlify_SiteId)
               );
        }
    }
}
