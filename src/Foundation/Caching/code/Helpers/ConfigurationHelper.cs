using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sugcon.Foundation.Caching.Helpers
{
    public static class ConfigurationHelper
    {
        public static string GetSetting(string connectionName)
        {
            return Sitecore.Configuration.Settings.GetSetting(connectionName);
        }
    }
}
