using Sitecore.LayoutService.Client.Response.Model.Fields;

namespace Sugcon.Feature.Basic.Rendering.Models
{
    public class PromoCard
    {
        public HyperLinkField Link { get; set; }
        public ImageField Image { get; set; }
        public TextField Headline { get; set; }
        public RichTextField Text { get; set; }
    }
}
