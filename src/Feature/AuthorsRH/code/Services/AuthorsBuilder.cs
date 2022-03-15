using Sugcon.Feature.AuthorsRH.Models;
using Sitecore.Abstractions;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Links.UrlBuilders;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace Sugcon.Feature.AuthorsRH.Services
{
    public class AuthorsBuilder : IAuthorsBuilder
    {
        protected readonly BaseLinkManager LinkManager;
        protected readonly BaseMediaManager MediaManager;
        public AuthorsBuilder(BaseLinkManager linkManager, BaseMediaManager mediaManager)
        {
            Debug.Assert(linkManager != null);
            LinkManager = linkManager;
            MediaManager = mediaManager;
        }

        public List<Author> GetAuthors(Item contextItem)
        {
            Debug.Assert(contextItem != null);
            var items = new List<Item>();
            items.AddRange(contextItem.Children.Where(item => item.DescendsFrom(Templates.Authors.Id)));
            List<Author> authorlist = new List<Author>();
            foreach (var item in items)
            {
                string imgTag = string.Empty;
                ImageField imageField = item.Fields[Templates.Authors.Fields.DisplayPicture];
                if (imageField != null && imageField.MediaItem != null)
                {
                    MediaItem image = new MediaItem(imageField.MediaItem);
                    string src =
                    Sitecore.Resources.Media.MediaManager.GetMediaUrl(image, new MediaUrlBuilderOptions()
                    {
                        AlwaysIncludeServerUrl = true
                    });
                    imgTag = String.Format(@"<img src=""{0}"" alt=""{1}"" />", src, image.Alt);
                }
                authorlist.Add(new Author()
                {
                    Description = item.Fields[Templates.Authors.Fields.Description].Value,
                    Name = item.Fields[Templates.Authors.Fields.Name].Value,
                    ShortNote = item.Fields[Templates.Authors.Fields.ShortNote].Value,
                    ProfilePicture = imgTag
                });
            }
            return authorlist;
        }
    }
}