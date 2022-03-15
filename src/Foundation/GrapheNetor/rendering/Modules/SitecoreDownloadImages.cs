using Sugcon.Foundation.GrapheNetor.Rendering.Extension;
using Sugcon.Foundation.GrapheNetor.Rendering.Helpers;
using Sugcon.Foundation.GrapheNetor.Rendering.Param;
using Statiq.Common;
using Statiq.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sugcon.Foundation.GrapheNetor.Rendering.Modules
{
    public class SitecoreDownloadImages : ReadWeb
    {
        protected override async Task<IEnumerable<IDocument>> ExecuteContextAsync(IExecutionContext context)
        {
            SitecoreImageDownload[] assets = context.Inputs.SelectMany((IDocument doc) => doc.GetSitecoreImageDownloads()).ToArray();
            // Replacing the domain with Docker service name
            string[] source = (from a in assets.DistinctBy((SitecoreImageDownload a) => a.LocalPath)
                               select a.OriginalUrl.Replace("sugcon-cm.sc10.localhost", "cm")).ToArray();
            WithUris(source.ToArray());
            IEnumerable<IDocument> obj = await base.ExecuteContextAsync(context);
            List<IDocument> list = context.Inputs.ToList();
            foreach (IDocument item in obj)
            {
                string downloadedUrl = item.Get<string>("SourceUri");
                SitecoreImageDownload SitecoreImageDownload = assets.FirstOrDefault((SitecoreImageDownload a) => a.OriginalUrl == downloadedUrl.Replace("cm", "sugcon-cm.sc10.localhost"));
                if (SitecoreImageDownload != null)
                {
                    NormalizedPath destination = SitecoreImageDownload.LocalPath.ToString().TrimStart('/');
                    list.Add(item.Clone(in destination));
                    continue;
                }
                throw new InvalidOperationException("No asset found for url " + downloadedUrl);
            }
            return list;
        }
    }
}
