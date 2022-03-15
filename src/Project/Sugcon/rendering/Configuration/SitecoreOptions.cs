using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sugcon.Project.Sugcon.Rendering.Configuration
{
    public class SitecoreOptions
    {
        public static readonly string Key = "Sitecore";

        public Uri InstanceUri { get; set; }
        public string LayoutServicePath { get; set; } = "/sitecore/api/layout/render/jss";
        public string DefaultSiteName { get; set; }
        public string ApiKey { get; set; }
        public bool EnableExperienceEditor { get; set; }

        public Uri LayoutServiceUri
        {
            get
            {
                if (InstanceUri == null) return null;

                return new Uri(InstanceUri, LayoutServicePath);
            }
        }
        //public Dictionary<string, string> Sites { get; set; } = new Dictionary<string, string>();
    }
}
