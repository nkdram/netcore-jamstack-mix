using Sugcon.Foundation.GrapheNetor.Rendering.Param;
using Statiq.Common;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace Sugcon.Foundation.GrapheNetor.Rendering.Helpers
{
    public static class SitecoreAssetHelpers
    {
        private static readonly MD5 Md5 = MD5.Create();
        public static SitecoreImageDownload[] GetSitecoreImageDownloads(this IDocument doc)
        {
            return doc.Get<SitecoreImageDownload[]>(Constants.Generator_Module_SitecoreImageKey) ?? Array.Empty<SitecoreImageDownload>();
        }

        public static NormalizedPath GetLocalFileName(string url, NormalizedPath localBasePath)
        {
            Uri uri = new Uri(url);
            string text = ReplaceInvalidChars(Path.GetFileName(uri.LocalPath));
            if (!string.IsNullOrEmpty(uri.Query))
            {
                NameValueCollection query = HttpUtility.ParseQueryString(uri.Query);
                string s = string.Join("&", from key in query.AllKeys
                                            select (key, query[key]) into kv
                                            orderby kv.key
                                            select kv.key + "=" + kv.Item2);
                text = string.Join("", from b in Md5.ComputeHash(Encoding.UTF8.GetBytes(s))
                                       select b.ToString("x2")) + "-" + text;
            }
            return Path.Combine(localBasePath.FullPath, text).Replace("\\", "/");
        }

        private static string ReplaceInvalidChars(string filename)
        {
            if (string.IsNullOrWhiteSpace(filename))
            {
                throw new ArgumentException("Value cannot be null or whitespace.", "filename");
            }
            return string.Join("_", filename.Split(Path.GetInvalidFileNameChars()));
        }
    }
}
