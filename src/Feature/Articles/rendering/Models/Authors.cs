using Sitecore.AspNet.RenderingEngine.Binding.Attributes;
using Sitecore.LayoutService.Client.Response.Model.Fields;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sugcon.Feature.Articles.Rendering.Models
{
    public class Author
    {
        public TextField AuthorName { get; set; }

        public ImageField AuthorProfilePicture { get; set; }

        public RichTextField AuthorDescription { get; set; }

    }
}
