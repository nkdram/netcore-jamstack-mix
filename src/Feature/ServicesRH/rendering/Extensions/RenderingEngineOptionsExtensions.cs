using Sugcon.Feature.Services.Rendering.Models;
using Sitecore.AspNet.RenderingEngine.Configuration;
using Sitecore.AspNet.RenderingEngine.Extensions;

namespace Sugcon.Feature.Services.Rendering.Extensions
{
    public static class RenderingEngineOptionsExtensions
    {
        public static RenderingEngineOptions AddFeatureServices(this RenderingEngineOptions options)
        {
            options
                .AddPartialView("TestimonialContainer")
                .AddModelBoundView<Testimonial>("Testimonial");
            return options;
        }
    }
}
