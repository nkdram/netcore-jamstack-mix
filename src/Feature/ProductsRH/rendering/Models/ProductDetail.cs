using Sitecore.LayoutService.Client.Response.Model.Fields;

namespace Sugcon.Feature.Products.Rendering.Models
{
    public class ProductDetail
    {
        public ImageField Image { get; set; }

        public RichTextField Features { get; set; }

        public TextField Price { get; set; }
    }
}
