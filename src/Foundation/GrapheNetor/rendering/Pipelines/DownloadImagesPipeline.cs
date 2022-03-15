using Sugcon.Foundation.GrapheNetor.Rendering.Modules;
using Statiq.Common;
using Statiq.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sugcon.Foundation.GrapheNetor.Rendering.Pipelines
{
    public class DownloadImagesPipeline : Pipeline
    {
        public DownloadImagesPipeline()
        {
            //Dependencies.Add(nameof(PagesPipeline));
            PostProcessModules = new ModuleList(
                // pull documents from other pipelines
                new ReplaceDocuments(Dependencies.ToArray()),
                new SitecoreDownloadImages()
            );
            OutputModules = new ModuleList(

                new WriteFiles()
            );
        }
    }
}
