using Sitecore.AspNet.RenderingEngine.Binding.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sugcon.Feature.Navigation.Rendering.Models
{
    public class Footer
    {
        [SitecoreComponentField]
        public string FooterText { get; set; }
    }
}
