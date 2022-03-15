using Sitecore.AspNet.RenderingEngine.Binding.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sugcon.Feature.Authors.Rendering.Models
{
    public class Authors
    {
        [SitecoreComponentField]
        public Author[] AllAuthors { get; set; }
    }
}
