using Sugcon.Feature.Navigation.Rendering.Models;
using Sitecore.AspNet.RenderingEngine.Configuration;
using Sitecore.AspNet.RenderingEngine.Extensions;

namespace Sugcon.Feature.Navigation.Rendering.Extensions
{
    public static class RenderingEngineOptionsExtensions
    {
        public static RenderingEngineOptions AddFeatureNavigation(this RenderingEngineOptions options)
        {
            options.AddModelBoundView<Header>("Header")
                   .AddModelBoundView<Footer>("Footer");
            return options;
        }
    }
}
