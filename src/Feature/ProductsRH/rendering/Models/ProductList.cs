using System.Collections.Generic;
using Sitecore.AspNet.RenderingEngine.Binding.Attributes;
using Sitecore.LayoutService.Client.Response.Model.Fields;

namespace Sugcon.Feature.Products.Rendering.Models
{
    public class ProductList
    {
        [SitecoreComponentField]
        public ContentListField<ListProduct> Products { get; set; }
    }
}
