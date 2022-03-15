using Sugcon.Feature.Basic.Rendering.Models;
using Sitecore.AspNet.RenderingEngine.Configuration;
using Sitecore.AspNet.RenderingEngine.Extensions;

namespace Sugcon.Feature.Basic.Rendering.Extensions
{
    public static class RenderingEngineOptionsExtensions
    {
        public static RenderingEngineOptions AddFeatureBasic(this RenderingEngineOptions options)
        {
            options
                .AddModelBoundView<PromoCard>("PromoCard")
                .AddPartialView("PromoContainer")
                .AddModelBoundView<SectionHeader>("SectionHeader")
                .AddModelBoundView<HeroBanner>("HeroBanner")
                .AddPartialView("Accordion")
                .AddModelBoundView<AccordionItem>("AccordionItem");
            return options;
        }
    }
}
