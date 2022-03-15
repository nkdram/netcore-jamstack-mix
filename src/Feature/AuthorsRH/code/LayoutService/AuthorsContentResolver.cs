using Sitecore.LayoutService.ItemRendering.ContentsResolvers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.LayoutService.Configuration;
using Sitecore.Mvc.Presentation;
using Sugcon.Feature.AuthorsRH.Services;
using Sugcon.Feature.AuthorsRH.Models;

namespace Sugcon.Feature.AuthorsRH.LayoutService
{
    public class AuthorsContentResolver : RenderingContentsResolver
    {
        protected readonly IAuthorsBuilder AuthorsBuilder;
        public AuthorsContentResolver(IAuthorsBuilder authorbuilder)
        {
            AuthorsBuilder = authorbuilder;
        }
        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {
            var authors = new
            {
                allAuthors = AuthorsBuilder.GetAuthors(this.GetContextItem(rendering, renderingConfig))
            };
            return authors;
        }
    }
}