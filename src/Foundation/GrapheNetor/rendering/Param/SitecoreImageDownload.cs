using Statiq.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sugcon.Foundation.GrapheNetor.Rendering.Param
{
    public class SitecoreImageDownload
    {
        public string OriginalUrl { get; }

        public NormalizedPath LocalPath { get; }

        public SitecoreImageDownload(string originalUrl, NormalizedPath localPath)
        {
            OriginalUrl = originalUrl;
            LocalPath = localPath;
        }
    }
}
