using Sugcon.Feature.Articles.Rendering.Models;
using Sitecore.AspNet.RenderingEngine.Configuration;
using Sitecore.AspNet.RenderingEngine.Extensions;

namespace Sugcon.Feature.Articles.Rendering.Extensions
{
    public static class RenderingEngineOptionsExtensions
    {
        public static RenderingEngineOptions AddFeatureArticles(this RenderingEngineOptions options)
        {
            options.AddModelBoundView<Sugcon.Feature.Articles.Rendering.Models.Blog>("Blog");
            options.AddModelBoundView<Sugcon.Feature.Articles.Rendering.Models.Author>("Authors");
            return options;
        }
    }
}
