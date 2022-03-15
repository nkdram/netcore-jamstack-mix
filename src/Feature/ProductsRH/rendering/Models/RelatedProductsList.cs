using Sitecore.AspNet.RenderingEngine.Binding.Attributes;
using Sitecore.LayoutService.Client.Response.Model.Fields;

namespace Sugcon.Feature.Products.Rendering.Models
{
    public class RelatedProductsList
    {
        public ContentListField<ListProduct> RelatedProducts { get; set; }
    }
}