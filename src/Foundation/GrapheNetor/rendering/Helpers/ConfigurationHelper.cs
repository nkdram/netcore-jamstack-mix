using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sugcon.Foundation.GrapheNetor.Rendering.Helpers
{
    public static class ConfigurationHelper
    {
        public static string GetConfigurationEntries(this IConfiguration configuration, string key)
        {
            return configuration.GetValue<string>(key);
        }
    }
}
