using Sugcon.Feature.AuthorsRH.Models;
using Sitecore.Data.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sugcon.Feature.AuthorsRH.Services
{
    public interface IAuthorsBuilder
    {
        List<Author> GetAuthors(Item contextItem);
    }
}