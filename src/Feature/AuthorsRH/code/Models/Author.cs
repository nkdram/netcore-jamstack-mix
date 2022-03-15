using Sitecore.Data.Fields;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sugcon.Feature.AuthorsRH.Models
{
    public class Author
    {
        public string Name { get; set; }

        public string ProfilePicture { get; set; }

        public string ShortNote { get; set; }

        public string Description { get; set; }
    }
}