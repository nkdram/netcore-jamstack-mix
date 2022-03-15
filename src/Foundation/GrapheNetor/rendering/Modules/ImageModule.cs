#nullable enable
using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sugcon.Foundation.GrapheNetor.Rendering.Helpers;
using Sugcon.Foundation.GrapheNetor.Rendering.Param;
using Statiq.Common;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace Sugcon.Foundation.GrapheNetor.Rendering.Modules
{
    public class ImagesModule : Module
    {
        internal const string AssetDownloadKey = "SC-ASSET-DOWNLOADS";
        private Func<string, bool>? _urlFilter;

        private static readonly Regex _sourceSetRegex = new Regex("(?:(?<url>.*)\\s+(?<size>[0-9]+[w|x]))", RegexOptions.IgnoreCase | RegexOptions.Multiline, TimeSpan.FromSeconds(5.0));

        private static readonly Regex _remoteUrlRegex = new Regex("^https?://", RegexOptions.IgnoreCase, TimeSpan.FromSeconds(5.0));

        private Config<NormalizedPath> _localBasePath = Config.FromValue(new NormalizedPath("img"));

        public ImagesModule WithLocalPath(string path)
        {
            return this;
        }

        public ImagesModule Skip(Func<string, bool> filter)
        {
            _urlFilter = filter;
            return this;
        }

        protected override async Task<IEnumerable<Statiq.Common.IDocument>> ExecuteInputAsync(Statiq.Common.IDocument input, IExecutionContext context)
        {
            var documents = new List<Statiq.Common.IDocument>();
            IHtmlDocument htmlDocument = await ParseHtmlAsync(input, context);
            if (htmlDocument == null)
            {
                return input.Yield();
            }
            List<SitecoreImageDownload> downloadUrls = new List<SitecoreImageDownload>();
            NormalizedPath localBasePath = await _localBasePath.GetValueAsync(input, context);
            foreach (IHtmlImageElement image in htmlDocument.Images)
            {
                string source = image.Source;
                if (SkipImage(source))
                {
                    continue;
                }
                NormalizedPath path = SitecoreAssetHelpers.GetLocalFileName(source, localBasePath);
                context.LogInformation("Replacing image {0} => {1}", image.Source, path);
                image.Source = context.GetLink(in path);
                downloadUrls.Add(new SitecoreImageDownload(source, path));
            }
            // Loop all the tags contains image such as figure

            await Task.Run(() =>
           {
               foreach (IElement element in htmlDocument.All.ToList())
               {
                   if (!element.HasAttribute("style"))
                       continue;
                   string style = element.GetAttribute("style");
                   Regex backgrounImgReg = new Regex(@"(?<=\().+?(?=\))");
                   string bgImage = backgrounImgReg.Match(style)?.Value;
                   if (!string.IsNullOrEmpty(bgImage))
                   {
                       string source = bgImage;
                       NormalizedPath path = SitecoreAssetHelpers.GetLocalFileName(source, localBasePath);
                       context.LogInformation("Replacing image {0} => {1}", source, path);
                       var bgImageNew = context.GetLink(in path);
                       downloadUrls.Add(new SitecoreImageDownload(source, path));
                       element.SetAttribute("style", style.Replace(bgImage, bgImageNew));
                   }
               }
           });


            foreach (IElement item3 in htmlDocument.Head.Children.Where(IsImageMetaTag))
            {
                string attribute = item3.GetAttribute(AttributeNames.Content);
                if (!SkipImage(attribute))
                {
                    NormalizedPath path2 = SitecoreAssetHelpers.GetLocalFileName(attribute, localBasePath);
                    context.LogDebug("Replacing metadata image {0} => {1}", attribute, path2);
                    item3.SetAttribute(AttributeNames.Content, context.GetLink(in path2, includeHost: true));
                    downloadUrls.Add(new SitecoreImageDownload(attribute, path2));
                }
            }
            IEnumerable<KeyValuePair<string, object>> items = new KeyValuePair<string, object>[1]
            {
                 new KeyValuePair<string, object>(Constants.Generator_Module_SitecoreImageKey, downloadUrls.ToArray())
            };
            return input.Clone(items, context.GetContentProvider(htmlDocument.ToHtml(), MediaTypes.Html)).Yield();
        }

        private static bool IsImageMetaTag(IElement element)
        {
            if (string.Equals(element.TagName, TagNames.Meta, StringComparison.OrdinalIgnoreCase) && ((element.GetAttribute("Property")?.Contains("image", StringComparison.OrdinalIgnoreCase) ?? false) || (element.GetAttribute(AttributeNames.Name)?.Contains("image", StringComparison.OrdinalIgnoreCase) ?? false)))
            {
                return element.GetAttribute(AttributeNames.Content)?.StartsWith("http", StringComparison.OrdinalIgnoreCase) ?? false;
            }
            return false;
        }

        private bool SkipImage(string uri)
        {
            if (IsRemoteUrl(uri))
            {
                return !(_urlFilter?.Invoke(uri) ?? true);
            }
            return true;
        }


        private static bool IsRemoteUrl(string? url)
        {
            if (url != null)
            {
                return _remoteUrlRegex.IsMatch(url);
            }
            return false;
        }

        private static async Task<IHtmlDocument?> ParseHtmlAsync(Statiq.Common.IDocument document, IExecutionContext context)
        {
            _ = 1;
            try
            {
                HtmlParser parser = new HtmlParser();
                IHtmlDocument result;
                await using (Stream stream = document.ContentProvider.GetStream())
                {
                    result = await parser.ParseDocumentAsync(stream);
                }
                return result;
            }
            catch (Exception ex)
            {
                context.LogWarning("Exception while parsing HTML for {0}: {1}", document.ToSafeDisplayString(), ex.Message);
            }
            return null;
        }

        private static string GetLocalFileName(string url)
        {
            return url;
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
