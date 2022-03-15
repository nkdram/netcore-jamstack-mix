using Sitecore.LayoutService.Client.Response.Model.Fields;

namespace Sugcon.Feature.Services.Rendering.Models
{
    public class Testimonial
    {
        public TextField Title { get; set; }

        public RichTextField Quote { get; set; }

        public ImageField Image { get; set; }
    }
}
