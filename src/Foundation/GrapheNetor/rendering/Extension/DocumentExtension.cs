using Sitecore.LayoutService.Client.Response;
using Statiq.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sugcon.Foundation.GrapheNetor.Rendering.Extension
{
    public static class DocumentExtension
    {
        /// <summary>
        /// Retrieves Sitecore Layout Response content from iDocument
        /// </summary>
        /// <param name="document"></param>
        /// <returns></returns>
        public static SitecoreLayoutResponseContent AsLayoutResponse(this IDocument document)
        {
            if (document == null)
            {
                throw new ArgumentNullException(nameof(document), $"Expected a document of type {typeof(SitecoreLayoutResponseContent).FullName}");
            }

            if (document.TryGetValue(Constants.Generator_Module_LayoutResponseKey, out SitecoreLayoutResponseContent sitecoreLayoutResponse))
            {
                return sitecoreLayoutResponse;
            }

            throw new InvalidOperationException($"This is not a LayouResponse document: {document.Source}");
        }
    }
}
