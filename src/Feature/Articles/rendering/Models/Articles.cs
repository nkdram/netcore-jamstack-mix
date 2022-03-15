using Sitecore.AspNet.RenderingEngine.Binding.Attributes;
using Sitecore.LayoutService.Client.Response.Model.Fields;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sugcon.Feature.Articles.Rendering.Models
{
    public class Blog
    {
        public TextField Title { get; set; }

        public RichTextField BlogDetail { get; set; }

        public DateField BlogDate { get; set; }

        public TextField CanonicalURL { get; set; }

        public ItemLinkField<Author> AuthorDetails { get; set; }
    }
}
