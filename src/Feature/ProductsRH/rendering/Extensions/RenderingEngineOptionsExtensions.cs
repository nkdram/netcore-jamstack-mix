using Sugcon.Feature.Products.Rendering.Models;
using Sitecore.AspNet.RenderingEngine.Configuration;
using Sitecore.AspNet.RenderingEngine.Extensions;

namespace Sugcon.Feature.Products.Rendering.Extensions
{
    public static class RenderingEngineOptionsExtensions
    {
        public static RenderingEngineOptions AddFeatureProducts(this RenderingEngineOptions options)
        {
            options
                .AddModelBoundView<ProductList>("ProductList")
                .AddModelBoundView<ProductDetailHeader>("ProductDetailHeader")
                .AddModelBoundView<ProductDetail>("ProductDetail")
                .AddModelBoundView<RelatedProductsList>("RelatedProducts");
            return options;
        }
    }
}
