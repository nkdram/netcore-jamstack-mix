using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.Data.Items;

namespace Sugcon.Feature.NavigationRH.Models
{
    public class NavigationItem
    {
        public Item Item { get; set; }
        public string Url { get; set; }
        public bool IsActive { get; set; }
    }
}