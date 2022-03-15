using Sugcon.Foundation.Caching.Models;
using Sitecore.Data.Items;
using Sitecore.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sugcon.Foundation.Caching.Extension
{
    public static class ItemExtension
    {
        public static string GenerateCacheKeyFromItem(this Item item)
        {
            SiteInfo siteinfo = item.GetSiteInfo();
            if (siteinfo == null) return string.Empty;
            return
                $"{siteinfo.Name}_{item.Language.Name}_{item.Paths.FullPath.ToLower().Replace(siteinfo.RootPath.ToLower() + siteinfo.StartItem.ToLower(), "").Replace("/", "_") }".Replace("__", "_").ToLower();
        }

        public static SiteInfo GetSiteInfo(this Item item)
        {
            var allSites = Sitecore.Configuration.Factory.GetSiteInfoList();
            // No need to compare with the defaults site definitions
            return allSites.FirstOrDefault(info =>
                                    !(info.Name.Equals("shell") ||
                                    info.Name.Equals("login") ||
                                    info.Name.Equals("admin") ||
                                    info.Name.Equals("service") ||
                                    info.Name.Equals("modules_shell") ||
                                    info.Name.Equals("modules_website") ||
                                    info.Name.Equals("scheduler") ||
                                    info.Name.Equals("system") ||
                                    info.Name.Equals("publisher") ||
                                    info.Name.Equals("website")) &&
                                    item.Paths.FullPath.StartsWith(info.RootPath));
        }

        public static bool IsPageType(this Item item)
        {
            return item.Fields[Sitecore.FieldIDs.LayoutField] != null
                    && !String.IsNullOrEmpty(item.Fields[Sitecore.FieldIDs.LayoutField].Value);
        }
    }
}
