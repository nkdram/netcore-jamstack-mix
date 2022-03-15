using Sitecore.AspNet.RenderingEngine.Configuration;
using Sitecore.AspNet.RenderingEngine.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sugcon.Feature.Authors.Rendering.Extension
{
    public static class RenderingEngineOptionsExtension
    {
        public static RenderingEngineOptions AddFeatureAuthors(this RenderingEngineOptions options)
        {
            options.AddModelBoundView<Sugcon.Feature.Authors.Rendering.Models.Authors>("AuthorsGrid");
            return options;
        }
    }
}
