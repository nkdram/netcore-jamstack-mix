#nullable enable
using Statiq.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Sugcon.Foundation.GrapheNetor.Rendering.Extension;
using Sugcon.Foundation.GrapheNetor.Rendering.Services;
using Microsoft.AspNetCore.Routing;
using Sitecore.AspNet.RenderingEngine;

namespace Sugcon.Foundation.GrapheNetor.Rendering.Modules
{
    public class RenderRazor : Module
    {
        private readonly ViewRenderer _viewRenderer;
        public RenderRazor(ViewRenderer viewRender)
        {
            _viewRenderer = viewRender;
        }

        protected override async Task<IEnumerable<IDocument>> ExecuteContextAsync(IExecutionContext context)
        {
            var inputs = context.Inputs;
            var documents = new List<IDocument>();
            foreach (var document in inputs)
            {
                await Task.Run(async () =>
                 {
                     var model = document.AsLayoutResponse();
                    ////-----Critical piece of code
                    ISitecoreRenderingContext renderingContext = _viewRenderer.httpContext.GetSitecoreRenderingContext();
                     renderingContext.Response.Content = model;
                    /////--------
                    var html = _viewRenderer.Render("index", model?.Sitecore?.Route, NormalizedPath.AbsoluteRoot.FullPath);
                     var doc = context.CreateDocument(document.Destination, html, "text/html");
                     documents.Add(doc);
                 });


            }
            return documents;
        }
    }
}
